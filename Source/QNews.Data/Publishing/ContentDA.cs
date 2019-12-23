using Microsoft.Practices.ServiceLocation;
using QNews.Base;
using QNews.Models;
using SolrNet;
using SolrNet.Commands.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QNews.Data.Publishing
{

    public class Option
    {
        public int RowPerPage { get; set; }

        public int CurrentPage { get; set; }

        public int TypeOfScope { get; set; }

        public int ScopeID { get; set; }
    }


    public class ContentDA
    {

        public static string FormatKeyword(string kwd)
        {
            if (!string.IsNullOrEmpty(kwd))
            {
                kwd = Regex.Replace(kwd, "[\\+|\\-|\\&\\&|\\|\\||\\!|\\(|\\)|||\\{|\\}|\\[|\\]|\\^|\\\\\\|\\~|\\*|\\?|\\:|\\>|\\<|\\\\|\\/]", " ", RegexOptions.IgnoreCase);
                if (kwd.ToArray().Where(o => o == '"').Count() % 2 == 1 || true) //Nếu là số lẻ
                {
                    kwd = Regex.Replace(kwd, "[\\+|\\\"]", " ", RegexOptions.IgnoreCase);
                }
            }
            return kwd;
        }

        public static AboutViewModel getAbout(int id)
        {
            AboutViewModel model = new AboutViewModel();

            using (QNewsDBContext db = new QNewsDBContext())
            {
                model.Gioithieuchung = (from c in db.Contents
                                        where (c.ID == id) && !c.IsRemoved && c.StatusID == (int)Utils.WorkFlowStatus.DA_DUYET
                                        select new ContentItem()
                                        {
                                            ID=c.ID,
                                            Description=c.Description,
                                            Details=c.Details
                                        }).SingleOrDefault();
                model.quickLink = (from c in db.QuickLinks
                                   where c.Show
                                   orderby c.Order descending
                                   select new Models.QuickLinkItem()
                                   {
                                       ID = c.ID,
                                       Title = c.Title,
                                       Link = c.Link
                                   }).FirstOrDefault();
            }

            return model;
        }
        public static SearchResult SearchContent(string kwd, string contentType, int rowPerPage, int CurrentPage = 1)
        {
            SearchResult model = new SearchResult();
            model.Kwd = kwd;
            model.RowPerPage = rowPerPage;
            model.CurrentPage = CurrentPage;


            string textQuery = "(IsRemoved:0)";
            string kwdInSorl = FormatKeyword(kwd);
            if (!string.IsNullOrEmpty(kwdInSorl))
                textQuery += string.Format("AND (FullText:{0})", FormatKeyword(kwd));

            if (!string.IsNullOrEmpty(contentType))
                textQuery += string.Format(" AND (ContentType:\"{0}\")", contentType);
            SolrQuery query = new SolrQuery(textQuery);

            var solrWorker = ServiceLocator.Current.GetInstance<ISolrOperations<Models.Publishing.SearchItem>>();
            List<SortOrder> LtsSort = new List<SortOrder>();
            LtsSort.Add(new SortOrder("score", Order.DESC));
            LtsSort.Add(new SortOrder("CreateDate", Order.DESC));
            QueryOptions options = new QueryOptions
            {
                Rows = rowPerPage,
                Start = (CurrentPage - 1) * rowPerPage,
                OrderBy = LtsSort,
                FilterQueries = new List<SolrQuery>() { new SolrQuery(textQuery) }.ToArray(),
                ExtraParams = new Dictionary<string, string>
                {
                    {"qf", "Title^10 Description^3 Details^2"},
                    {"defType", "edismax"}
                }
            };
            SolrQueryResults<Models.Publishing.SearchItem> results = solrWorker.Query(query, options);
            model.Items = results.ToList();
            int total = 0;
            model.Facet = getFacet(kwd,  ref total);
            model.TotalRowFacet = total;
            model.TotalRow = results.NumFound;
            model.HtmlPage = new Utils.Paging().getHtmlPage(string.Format("?contentType={1}&kwd={0}&p=", System.Web.HttpUtility.HtmlEncode(kwd), System.Web.HttpUtility.HtmlEncode(contentType)), 3, CurrentPage, rowPerPage, model.TotalRow);

            return model;
        }

        static List<FacetItem> getFacet(string kwd, ref int total)
        {
            List<FacetItem> model = new List<FacetItem>();
            List<ISolrFacetQuery> ListFacet = new List<ISolrFacetQuery>();
            SolrFacetFieldQuery queryFacet = new SolrFacetFieldQuery("ContentType");
            ListFacet.Add(queryFacet);

            string textQuery = "(IsRemoved:0)";
            string kwdInSorl = FormatKeyword(kwd);
            if (!string.IsNullOrEmpty(kwdInSorl))
                textQuery += string.Format("AND (FullText:{0})", FormatKeyword(kwd));
            SolrQuery query = new SolrQuery(textQuery);

            var solrWorker = ServiceLocator.Current.GetInstance<ISolrOperations<Models.Publishing.SearchItem>>();

            QueryOptions options = new QueryOptions
            {
                Rows = 0,
                Start = 0,
                FilterQueries = new List<SolrQuery>() { new SolrQuery(textQuery) }.ToArray(),
                ExtraParams = new Dictionary<string, string>
                {
                    {"qf", "Title^10 Description^3 Details^2"},
                    {"defType", "edismax"}
                },
                Facet = new FacetParameters
                {
                    Queries = ListFacet,
                    MinCount = 1
                }
            };
            SolrQueryResults<Models.Publishing.SearchItem> results = solrWorker.Query(query, options);
            total = results.NumFound;
            foreach (var item in results.FacetFields["ContentType"])
            {
                model.Add(new FacetItem()
                {
                    Title = item.Key,
                    Total = item.Value,
                    UrlFacet = item.Key
                });
            }
            return model;
        }

        public List<Models.ContentViewModel> getHotNews(int limit = 8)
        {
            var model = new List<ContentViewModel>();
            using (QNewsDBContext db = new QNewsDBContext())
            {
                model = db.Contents.Where(o => o.StatusID == (int)Utils.WorkFlowStatus.DA_DUYET && !o.IsRemoved && o.IsHot && !string.IsNullOrEmpty(o.Image)).OrderByDescending(o => o.PublishDate).ThenByDescending(o => o.CreateDate).Select(o => new ContentViewModel()
                {
                    ID = o.ID,
                    PublishDate = o.PublishDate,
                    CreateDate = o.CreateDate,
                    Title = o.Title,
                    Url = o.Urls.Select(c => c.UrlID).FirstOrDefault(),
                    Description = o.Description,
                    Image = o.Image
                }).Take(limit).ToList();
            }
            return model;

        }
        public List<Models.AdvertiseItem> getAllAds()
        {
            var model = new List<AdvertiseItem>();
            using (QNewsDBContext db = new QNewsDBContext())
            {
                model = (from c in db.Advertises
                         where c.StatusID == (int)QNews.Utils.WorkFlowStatus.DA_DUYET && (!c.NgayBatDau.HasValue || c.NgayBatDau.Value <= DateTime.Now) && (!c.NgayKetThuc.HasValue || c.NgayKetThuc.Value >= DateTime.Now) && !c.IsRemoved
                         select new QNews.Models.AdvertiseItem()
                         {
                             ID = c.ID,
                             Title = c.Title,
                             Image = c.Image,
                             Order = c.Order,
                             ZoneID = c.ZoneID,
                             Link = c.Link
                         }).OrderBy(o => o.Order).ToList();
            }
            return model;

        }
        public List<Models.SiteLinkItem> getAllSiteLinks()
        {
            var model = new List<SiteLinkItem>();
            using (QNewsDBContext db = new QNewsDBContext())
            {
                model = (from c in db.SiteLinks
                         where c.StatusID == (int)QNews.Utils.WorkFlowStatus.DA_DUYET && (!c.NgayBatDau.HasValue || c.NgayBatDau.Value <= DateTime.Now) && (!c.NgayKetThuc.HasValue || c.NgayKetThuc.Value >= DateTime.Now) && !c.IsRemoved
                         select new QNews.Models.SiteLinkItem()
                         {
                             ID = c.ID,
                             Title = c.Title,
                             SourceUrl = c.Link,
                             Order = c.Order
                         }).OrderBy(o => o.Order).ToList();
                return model;
            }
        }


        public List<Models.QuickLinkItem> getAllLeaderLink()
        {
            var model = new List<QuickLinkItem>();
            using (QNewsDBContext db = new QNewsDBContext())
            {
                model = (from c in db.QuickLinks
                         where c.Show
                         orderby c.Order
                         select new QuickLinkItem()
                         {
                             ID = c.ID,
                             Link = c.Link,
                             Order = c.Order,
                             Show = c.Show,
                             Title = c.Title

                         }).ToList();
            }
            return model;

        }


        public LayoutViewModels getLayoutModels()
        {
            LayoutViewModels model = new LayoutViewModels();

            using (QNews.Base.QNewsDBContext db = new QNews.Base.QNewsDBContext())
            {
                model.footer = (from c in db.TSHTs where c.KeyTSHT == "footer" select new TSHTItem() {
                    ID=c.ID,
                    KeyTSHT=c.KeyTSHT,
                    Value=c.Value
                }).FirstOrDefault();
                model.TopNews = db.Contents.Where(o => o.StatusID == (int)Utils.WorkFlowStatus.DA_DUYET && !o.IsRemoved).OrderByDescending(o => o.PublishDate).ThenByDescending(o => o.CreateDate).Select(o => new ContentViewModel()
                {
                    ID = o.ID,
                    PublishDate = o.PublishDate,
                    CreateDate = o.CreateDate,
                    Title = o.Title,
                    Url = o.Urls.Select(c => c.UrlID).FirstOrDefault(),
                    Description = o.Description,
                    Image = o.Image
                }).Skip(0).Take(3).ToList();

                model.FirstAlbum = (from c in db.Albums
                                    where c.StatusID == (int)QNews.Utils.WorkFlowStatus.DA_DUYET && !c.IsRemoved
                                    && (c.PublishDate <= DateTime.Now)
                                    orderby new { c.PublishDate , c.ID } descending
                                    select new AlbumItem()
                                    {
                                        ID = c.ID,
                                        Image = c.Image,
                                        Title = c.Title,
                                        Description = c.Description,
                                        SourceUrl = c.Urls.Select(o => o.UrlID).FirstOrDefault(),
                                        LtsPicture = c.AlbumPictures.Where(
                                            o => !o.IsRemoved && o.StatusID == (int)QNews.Utils.WorkFlowStatus.DA_DUYET && !c.IsRemoved
                                        )
                                        .OrderByDescending(o => o.CreateDate)
                                        .Select(o => new AlbumPictureItem()
                                        {
                                            ID = o.ID,
                                            Image = o.Image,
                                            Title = o.Title
                                        }).ToList()
                                    }
                                ).FirstOrDefault();

                model.LtsAlbum = (from c in db.Albums
                                  where c.StatusID == (int)QNews.Utils.WorkFlowStatus.DA_DUYET && !c.IsRemoved
                                  && (c.PublishDate <= DateTime.Now)
                                  orderby new { c.PublishDate, c.ID } descending
                                  select new AlbumItem()
                                  {
                                      ID = c.ID,
                                      Image = c.Image,
                                      Title = c.Title,
                                      Description = c.Description,
                                      SourceUrl = c.Urls.Select(o => o.UrlID).FirstOrDefault()
                                  }
                                ).Skip(1).Take(5).ToList();



                model.LtsVideo = (from c in db.Videos
                                  where
                                  c.StatusID == (int)QNews.Utils.WorkFlowStatus.DA_DUYET
                                  && (c.PublishDate <= DateTime.Now)
                                  && !c.IsRemoved
                                  select new QNews.Models.VideoItem()
                                  {
                                      ID = c.ID,
                                      Title = c.Title,
                                      Image = c.Image,
                                      FileAttach = c.FileAttach,
                                      PublishDate = c.PublishDate
                                  }).OrderByDescending(o => o.PublishDate).Take(5).ToList();


                model.LtsAudio = (from c in db.Audios
                                  where c.StatusID == (int)QNews.Utils.WorkFlowStatus.DA_DUYET && (!c.PublishDate.HasValue || c.PublishDate <= DateTime.Now) && !c.IsRemoved
                                  select new QNews.Models.AudioItem()
                                  {
                                      ID = c.ID,
                                      Title = c.Title,
                                      Image = c.Image,
                                      FileAttach = c.FileAttach,
                                      PublishDate = c.PublishDate
                                  }).OrderByDescending(o => o.PublishDate).Take(5).ToList();

                model.LtsAds = (from c in db.Advertises
                                where c.StatusID == (int)QNews.Utils.WorkFlowStatus.DA_DUYET && (!c.NgayBatDau.HasValue || c.NgayBatDau.Value <= DateTime.Now) && (!c.NgayKetThuc.HasValue || c.NgayKetThuc.Value >= DateTime.Now) && !c.IsRemoved
                                select new QNews.Models.AdvertiseItem()
                                {
                                    ID = c.ID,
                                    Title = c.Title,
                                    Image = c.Image,
                                    Order = c.Order,
                                    ZoneID = c.ZoneID,
                                    Link = c.Link
                                }).OrderBy(o => o.Order).ToList();

                model.LtsSiteLink = (from c in db.SiteLinks
                                     where c.StatusID == (int)QNews.Utils.WorkFlowStatus.DA_DUYET && (!c.NgayBatDau.HasValue || c.NgayBatDau.Value <= DateTime.Now) && (!c.NgayKetThuc.HasValue || c.NgayKetThuc.Value >= DateTime.Now) && !c.IsRemoved
                                     select new QNews.Models.SiteLinkItem()
                                     {
                                         ID = c.ID,
                                         Title = c.Title,
                                         SourceUrl = c.Link,
                                         Order = c.Order
                                     }).OrderBy(o => o.Order).ToList();

                model.LtsAllCategory = (from c in db.Categories
                                        select new CategoryViewModel()
                                        {
                                            AllID = c.AllID,
                                            CurrentUrl = c.CurrentUrl,
                                            Description = c.Description,
                                            ID = c.ID,
                                            Image = c.Image,
                                            Name = c.Name,
                                            Order = c.Order,
                                            ParentID = c.ParentID,
                                            Show = c.Show,
                                            ShowInHome = c.ShowInHome,
                                            ShowInTab = c.ShowInTab,
                                            ShowInTopMenu = c.ShowInTopMenu,
                                            TypeOfDisplay = c.TypeOfDisplay,
                                            ShowInRight = c.ShowInRight,
                                            Url = c.Urls.Select(o => o.UrlID).FirstOrDefault()
                                        }).ToList();


                foreach (var item in model.LtsAllCategory.Where(o => ((o.TypeOfDisplay == 6 || o.TypeOfDisplay == 7)) && o.ShowInHome > 0))
                {
                    item.LtsContent = db.Clone_Rss.Where(o => o.RssActive).OrderByDescending(o => o.RssCreated).ThenByDescending(o => o.RssOrder).Select(o => new ContentViewModel()
                    {
                        ID = o.RssID,
                        PublishDate = o.RssCreated,
                        Title = o.RssTitle,
                        Url = o.RssSource,
                        Description = o.RssDescription,
                        Image = o.RssImage,
                        CreateDate = o.RssCreated.HasValue ? o.RssCreated.Value : DateTime.Now
                    }).Skip(0).Take(item.ShowInHome).ToList();

                }

            }

            return model;
        }

        public ListDocumentViewModel GetDocumentList(int currentPage, int type)
        {
            ListDocumentViewModel model = new ListDocumentViewModel();
            using (QNewsDBContext db = new QNewsDBContext())
            {
                var query = (from c in db.Documents where !c.IsRemoved && c.StatusID == (int)QNews.Utils.WorkFlowStatus.DA_DUYET orderby c.NgayBanHanh descending select c);
                model.Title = "Toàn bộ văn bản";
                model.DocumentTypeID = type;

                if (type > 0)
                {
                    query = query.Where(o => o.LoaiVanBanID == type).OrderByDescending(o => o.NgayBanHanh);
                    model.Title = db.DocumentTypes.Where(o => o.ID == type).FirstOrDefault().Title;
                }

                model.Total = query.Count();
                model.PageHtml = new Utils.Paging().getHtmlPage(string.Format("?{0}p=", (type > 0) ? "type=" + type + "&" : ""), 3, currentPage, 20, model.Total);


                model.LtsItem = query.Select(o => new Models.DocumentViewModel()
                {
                    DocumentType = o.DocumentType.Title,
                    FileAttach = o.FileAttach,
                    ID = o.ID,
                    NgayBanHanh = o.NgayBanHanh,
                    NgayHieuLuc = o.NgayHieuLuc,
                    NguoiKy = o.NguoiKy,
                    SoKyHieu = o.SoKyHieu,
                    TrichYeu = o.TrichYeu,
                    Url = o.Urls.FirstOrDefault().UrlID,
                    LinhVuc = o.DocumentScopes.Select(c => c.Title).ToList(),
                    CoQuanBanHanh = o.DocumentIssues.Select(c => c.Title).ToList()
                }).Skip(20 * (currentPage - 1)).Take(20).ToList();
            }
            return model;
        }



        public ListDocumentViewModel GetDocumentList(int currentPage, int type, int scope, int issue, string kwd)
        {
            ListDocumentViewModel model = new ListDocumentViewModel();
            using (QNewsDBContext db = new QNewsDBContext())
            {
                var query = (from c in db.Documents where !c.IsRemoved && c.StatusID == (int)QNews.Utils.WorkFlowStatus.DA_DUYET orderby c.NgayBanHanh descending select c);
                model.Title = "Toàn bộ văn bản";
                model.DocumentTypeID = type;

                if (type > 0)
                {
                    query = query.Where(o => o.LoaiVanBanID == type).OrderByDescending(o => o.NgayBanHanh);
                    model.Title = db.DocumentTypes.Where(o => o.ID == type).FirstOrDefault().Title;
                }

                if (scope > 0)
                {
                    query = query.Where(o => o.DocumentScopes.Any(c => c.ID == scope)).OrderByDescending(o => o.NgayBanHanh);
                    model.Title = db.DocumentScopes.Where(o => o.ID == scope).FirstOrDefault().Title;
                }
                if (issue > 0)
                {
                    query = query.Where(o => o.DocumentIssues.Any(c => c.ID == issue)).OrderByDescending(o => o.NgayBanHanh);
                    model.Title = db.DocumentIssues.Where(o => o.ID == issue).FirstOrDefault().Title;
                }

                if (!string.IsNullOrEmpty(kwd))
                {
                    kwd = kwd.ToLower().Trim();
                    query = query.Where(o => o.SoKyHieu.ToLower().Contains(kwd) || o.CoQuanBanHanh.ToLower().Contains(kwd) || o.NguoiKy.ToLower().Contains(kwd) || o.DocumentScopes.Any(c => c.Title.ToLower().Contains(kwd)) || o.TrichYeu.ToLower().Contains(kwd)).OrderByDescending(o => o.NgayBanHanh);
                }


                model.Total = query.Count();
                model.PageHtml = new Utils.Paging().getHtmlPage(string.Format("?{0}p=", (type > 0) ? "type=" + type + "&" : ""), 3, currentPage, 20, model.Total);


                model.LtsItem = query.Select(o => new Models.DocumentViewModel()
                {
                    DocumentType = o.DocumentType.Title,
                    FileAttach = o.FileAttach,
                    ID = o.ID,
                    NgayBanHanh = o.NgayBanHanh,
                    NgayHieuLuc = o.NgayHieuLuc,
                    NguoiKy = o.NguoiKy,
                    SoKyHieu = o.SoKyHieu,
                    TrichYeu = o.TrichYeu,
                    Url = o.Urls.FirstOrDefault().UrlID,
                    LinhVuc = o.DocumentScopes.Select(c => c.Title).ToList(),
                    CoQuanBanHanh = o.DocumentIssues.Select(c => c.Title).ToList()
                }).Skip(20 * (currentPage - 1)).Take(20).ToList();
            }
            return model;
        }


        public HomeViewModel getIndexChuongTrinh()
        {
            HomeViewModel model = new HomeViewModel();
            using (QNewsDBContext db = new QNewsDBContext())
            {

                model.LtsCategory = (from c in db.DocumentScopes
                                     where c.Show
                                     orderby c.Order
                                     select new Models.CategoryViewModel()
                                     {
                                         ID = c.ID,
                                         Name = c.Title,
                                         LtsContent = c.Contents.Where(o => o.TypeOfScopeID == 1 && o.StatusID == (int)Utils.WorkFlowStatus.DA_DUYET && !o.IsRemoved).OrderByDescending(o => o.PublishDate).ThenByDescending(o => o.CreateDate).Select(o => new ContentViewModel()//Thông tin chung
                                            {
                                                ID = o.ID,
                                                PublishDate = o.PublishDate,
                                                CreateDate = o.CreateDate,
                                                Title = o.Title,
                                                Url = o.Urls.Select(u => u.UrlID).FirstOrDefault(),
                                                Description = o.Description,
                                                Image = o.Image
                                            }).Skip(0).Take(8).ToList(),

                                         LtsContentOther = c.Contents.Where(o => o.TypeOfScopeID == 2 && o.StatusID == (int)Utils.WorkFlowStatus.DA_DUYET && !o.IsRemoved).OrderByDescending(o => o.PublishDate).ThenByDescending(o => o.CreateDate).Select(o => new ContentViewModel()//Chương trình hđ
                                         {
                                             ID = o.ID,
                                             PublishDate = o.PublishDate,
                                             CreateDate = o.CreateDate,
                                             Title = o.Title,
                                             Url = o.Urls.Select(u => u.UrlID).FirstOrDefault(),
                                             Description = o.Description,
                                             Image = o.Image
                                         }).Skip(0).Take(8).ToList()
                                     }).ToList();
            }
            return model;
        }
        public HomeViewModel getChuongTrinhIndexNew()
        {
            HomeViewModel model = new HomeViewModel();
            using (QNewsDBContext db = new QNewsDBContext())
            {
                model.ChuongTrinhIndexNew = (from c in db.DocumentScopes
                                             where c.Show && !c.IsRemoved
                                             orderby c.Order
                                             select new Models.ChuongTrinhHomeViewModelNew()
                                             {
                                                 ID = c.ID,
                                                 Title = c.Title,
                                                 Image = c.Image
                                             }).ToList();
            }
            return model;
        }
        public HomeViewModel getHomeViewModel()
        {
            HomeViewModel model = new HomeViewModel();
            using (QNewsDBContext db = new QNewsDBContext())
            {
                model.ChuongTrinhIndexNew = (from c in db.DocumentScopes where c.Show && !c.IsRemoved orderby c.Order select new Models.ChuongTrinhHomeViewModelNew() {
                    ID=c.ID,
                    Title=c.Title,
                    Image=c.Image
                }).ToList();
                model.quickLink = (from c in db.QuickLinks where c.Show orderby c.Order descending select new Models.QuickLinkItem() {
                    ID=c.ID,
                    Title=c.Title,
                    Link=c.Link
                }).FirstOrDefault();
                model.lstTagetItems = (from c in db.Targets
                                       where c.Show && !c.IsRemoved
                                       orderby c.Order
                                       select new Models.TagetItems()
                                       {
                                           ID=c.ID,
                                           Title=c.Title,
                                           Descriptions=c.Descriptions,
                                           Image=c.Image,
                                           Order=c.Order
                                       }).Take(6).ToList();
                model.LtsAlbumHome = (from c in db.Albums
                                      where c.StatusID == (int)QNews.Utils.WorkFlowStatus.DA_DUYET && !c.IsRemoved
                                      && (c.PublishDate <= DateTime.Now)
                                      orderby new { c.PublishDate, c.ID } descending
                                      select new AlbumItem()
                                      {
                                          ID = c.ID,
                                          Image = c.Image,
                                          Title = c.Title,
                                          Description = c.Description,
                                          SourceUrl = c.Urls.Select(o => o.UrlID).FirstOrDefault(),
                                          LtsPicture = c.AlbumPictures.Where(
                                              o => !o.IsRemoved && o.StatusID == (int)QNews.Utils.WorkFlowStatus.DA_DUYET && !c.IsRemoved
                                          )
                                          .OrderByDescending(o => o.CreateDate)
                                          .Select(o => new AlbumPictureItem()
                                          {
                                              ID = o.ID,
                                              Image = o.Image,
                                              Title = o.Title
                                          }).ToList()
                                      }
                                ).Take(8).ToList();
                model.LtsPartner = (from c in db.Partners
                                    where c.Show && !c.IsRemoved
                                    orderby c.Order
                                    select new PartnerItem
                                    {
                                        ID=c.ID,
                                        Title=c.Title,
                                        Image=c.Image,
                                        Order=c.Order
                                    }).ToList();
                /*model.ChuongTrinhIndex = (from c in db.DocumentScopes
                                          where c.Show
                                          orderby Guid.NewGuid()
                                          select new Models.ChuongTrinhHomeViewModel()
                                          {
                                              ID = c.ID,
                                              Title = c.Title,
                                              ThongTinChung = c.Contents.Where(o => o.TypeOfScopeID == 1 && o.StatusID == (int)Utils.WorkFlowStatus.DA_DUYET && !o.IsRemoved).OrderByDescending(o => o.PublishDate).ThenByDescending(o => o.CreateDate).Select(o => new ContentViewModel()//Chương trình hđ
                                              {
                                                  ID = o.ID,
                                                  PublishDate = o.PublishDate,
                                                  CreateDate = o.CreateDate,
                                                  Title = o.Title,
                                                  Url = o.Urls.Select(u => u.UrlID).FirstOrDefault(),
                                                  Description = o.Description,
                                                  Image = o.Image,
                                                  Details = o.Details
                                              }).FirstOrDefault(),
                                              //LtsDocument = c.Documents.Where(o => !o.IsRemoved && o.StatusID == (int)QNews.Utils.WorkFlowStatus.DA_DUYET)
                                              //.OrderByDescending(o => o.NgayBanHanh)
                                              //.Select(o => new Models.DocumentViewModel() {
                                              //    DocumentType = o.DocumentType.Title,
                                              //    FileAttach = o.FileAttach,
                                              //    ID = o.ID,
                                              //    NgayBanHanh = o.NgayBanHanh,
                                              //    NgayHieuLuc = o.NgayHieuLuc,
                                              //    NguoiKy = o.NguoiKy,
                                              //    SoKyHieu = o.SoKyHieu,
                                              //    TrichYeu = o.TrichYeu,
                                              //    Url = o.Urls.FirstOrDefault().UrlID,
                                              //    LinhVuc = o.DocumentScopes.Select(cat => cat.Title).ToList(),
                                              //    CoQuanBanHanh = o.DocumentIssues.Select(cat => cat.Title).ToList()
                                              //}).Take(10).ToList()
                                          }).ToList();

                foreach(var item in model.ChuongTrinhIndex)
                {
                    item.LtsDocument = (from o in db.Documents
                                         where !o.IsRemoved && o.StatusID == (int)QNews.Utils.WorkFlowStatus.DA_DUYET
                                         && o.DocumentScopes.Any(c => c.ID == item.ID)
                                         orderby o.NgayBanHanh descending
                                         select new Models.DocumentViewModel()
                                             {
                                                 DocumentType = o.DocumentType.Title,
                                                 FileAttach = o.FileAttach,
                                                 ID = o.ID,
                                                 NgayBanHanh = o.NgayBanHanh,
                                                 NgayHieuLuc = o.NgayHieuLuc,
                                                 NguoiKy = o.NguoiKy,
                                                 SoKyHieu = o.SoKyHieu,
                                                 TrichYeu = o.TrichYeu,
                                                 Url = o.Urls.FirstOrDefault().UrlID,
                                                 LinhVuc = o.DocumentScopes.Select(c => c.Title).ToList(),
                                                 CoQuanBanHanh = o.DocumentIssues.Select(c => c.Title).ToList()
                                             }).Take(10).ToList();
                }
                */

                model.LtsAD = db.Advertises.OrderBy(o => o.Order).Where(o => (o.ZoneID == 2 || o.ZoneID == 3)
                    && o.StatusID == (int)Utils.WorkFlowStatus.DA_DUYET
                    && !o.IsRemoved
                    && (!o.NgayBatDau.HasValue || o.NgayBatDau.Value >= DateTime.Now)
                    && (!o.NgayKetThuc.HasValue || o.NgayKetThuc.Value <= DateTime.Now))
                    .Select(o => new AdvertiseItem()
                    {
                        ID = o.ID,
                        Image = o.Image,
                        Link = o.Link,
                        Title = o.Title,
                        ZoneID = o.ZoneID
                    }).ToList();

                model.LtsHotNews = db.Contents.Where(o => o.StatusID == (int)Utils.WorkFlowStatus.DA_DUYET && !o.IsRemoved && o.IsHot && !string.IsNullOrEmpty(o.Image)).OrderByDescending(o => o.PublishDate).ThenByDescending(o => o.CreateDate).Select(o => new ContentViewModel()
                {
                    ID = o.ID,
                    PublishDate = o.PublishDate,
                    CreateDate = o.CreateDate,
                    Title = o.Title,
                    Url = o.Urls.Select(c => c.UrlID).FirstOrDefault(),
                    Description = o.Description,
                    Image = o.Image
                }).Skip(0).Take(10).ToList();

                model.LtsCategory = (from c in db.Categories
                                     select new CategoryViewModel()
                                     {
                                         AllID = c.AllID,
                                         CurrentUrl = c.CurrentUrl,
                                         Description = c.Description,
                                         ID = c.ID,
                                         Image = c.Image,
                                         Name = c.Name,
                                         Order = c.Order,
                                         ParentID = c.ParentID,
                                         Show = c.Show,
                                         ShowInHome = c.ShowInHome,
                                         ShowInTab = c.ShowInTab,
                                         ShowInTopMenu = c.ShowInTopMenu,
                                         TypeOfDisplay = c.TypeOfDisplay,
                                         Url = c.Urls.Select(o => o.UrlID).FirstOrDefault()
                                     }).ToList();

                foreach (var item in model.LtsCategory.Where(o => o.ShowInTab && o.ShowInHome > 0))
                {
                    item.LtsContent = db.Contents.Where(o => o.StatusID == (int)Utils.WorkFlowStatus.DA_DUYET && !o.IsRemoved && o.Categories.Any(c => c.ID == item.ID)).OrderByDescending(o => o.PublishDate).ThenByDescending(o => o.CreateDate).Select(o => new ContentViewModel()
                    {
                        ID = o.ID,
                        PublishDate = o.PublishDate,
                        Title = o.Title,
                        Url = o.Urls.Select(c => c.UrlID).FirstOrDefault(),
                        Description = o.Description,
                        Image = o.Image
                    }).Skip(0).Take(item.ShowInHome).ToList();

                }
            }

            return model;
        }

        //public HomeViewModel getHomeViewModel()
        //{
        //    HomeViewModel model = new HomeViewModel();
        //    using (QNewsDBContext db = new QNewsDBContext())
        //    {

        //        model.ChuongTrinh = (from c in db.DocumentScopes
        //                             where c.Show
        //                             orderby Guid.NewGuid()
        //                             select new Models.CategoryViewModel()
        //                             {
        //                                 ID = c.ID,
        //                                 Name = c.Title,
        //                                 LtsContent = c.Contents.Where(o => o.TypeOfScopeID == 1 && o.StatusID == (int)Utils.WorkFlowStatus.DA_DUYET && !o.IsRemoved).OrderByDescending(o => o.PublishDate).ThenByDescending(o => o.CreateDate).Select(o => new ContentViewModel()//Thông tin chung
        //                                 {
        //                                     ID = o.ID,
        //                                     PublishDate = o.PublishDate,
        //                                     CreateDate = o.CreateDate,
        //                                     Title = o.Title,
        //                                     Url = o.Urls.Select(u => u.UrlID).FirstOrDefault(),
        //                                     Description = o.Description,
        //                                     Image = o.Image,
        //                                     Details = o.Details
        //                                 }).Skip(0).Take(8).ToList(),

        //                                 LtsContentOther = c.Contents.Where(o => o.TypeOfScopeID == 2 && o.StatusID == (int)Utils.WorkFlowStatus.DA_DUYET && !o.IsRemoved).OrderByDescending(o => o.PublishDate).ThenByDescending(o => o.CreateDate).Select(o => new ContentViewModel()//Chương trình hđ
        //                                 {
        //                                     ID = o.ID,
        //                                     PublishDate = o.PublishDate,
        //                                     CreateDate = o.CreateDate,
        //                                     Title = o.Title,
        //                                     Url = o.Urls.Select(u => u.UrlID).FirstOrDefault(),
        //                                     Description = o.Description,
        //                                     Image = o.Image,
        //                                     Details = o.Details
        //                                 }).Skip(0).Take(8).ToList()
        //                             }).FirstOrDefault();

        //        int ChuongTrinh = model.ChuongTrinh != null ?  model.ChuongTrinh.ID : 0;

        //        model.LtsDocument = (from o in db.Documents
        //                             where !o.IsRemoved && o.StatusID == (int)QNews.Utils.WorkFlowStatus.DA_DUYET
        //                             && o.DocumentScopes.Any(c => c.ID == ChuongTrinh)
        //                             orderby o.NgayBanHanh descending
        //                             select new Models.DocumentViewModel()
        //                                 {
        //                                     DocumentType = o.DocumentType.Title,
        //                                     FileAttach = o.FileAttach,
        //                                     ID = o.ID,
        //                                     NgayBanHanh = o.NgayBanHanh,
        //                                     NgayHieuLuc = o.NgayHieuLuc,
        //                                     NguoiKy = o.NguoiKy,
        //                                     SoKyHieu = o.SoKyHieu,
        //                                     TrichYeu = o.TrichYeu,
        //                                     Url = o.Urls.FirstOrDefault().UrlID,
        //                                     LinhVuc = o.DocumentScopes.Select(c => c.Title).ToList(),
        //                                     CoQuanBanHanh = o.DocumentIssues.Select(c => c.Title).ToList()
        //                                 }).Take(10).ToList();

        //        model.LtsAD = db.Advertises.OrderBy(o => o.Order).Where(o => (o.ZoneID == 2 || o.ZoneID == 3)
        //            && o.StatusID == (int)Utils.WorkFlowStatus.DA_DUYET
        //            && !o.IsRemoved
        //            && (!o.NgayBatDau.HasValue || o.NgayBatDau.Value >= DateTime.Now)
        //            && (!o.NgayKetThuc.HasValue || o.NgayKetThuc.Value <= DateTime.Now))
        //            .Select(o => new AdvertiseItem()
        //            {
        //                ID = o.ID,
        //                Image = o.Image,
        //                Link = o.Link,
        //                Title = o.Title,
        //                ZoneID = o.ZoneID
        //            }).ToList();

        //        model.LtsHotNews = db.Contents.Where(o => o.StatusID == (int)Utils.WorkFlowStatus.DA_DUYET && !o.IsRemoved && o.IsHot && !string.IsNullOrEmpty(o.Image)).OrderByDescending(o => o.PublishDate).ThenByDescending(o => o.CreateDate).Select(o => new ContentViewModel()
        //        {
        //            ID = o.ID,
        //            PublishDate = o.PublishDate,
        //            CreateDate = o.CreateDate,
        //            Title = o.Title,
        //            Url = o.Urls.Select(c => c.UrlID).FirstOrDefault(),
        //            Description = o.Description,
        //            Image = o.Image
        //        }).Skip(0).Take(10).ToList();

        //        model.LtsCategory = (from c in db.Categories
        //                             select new CategoryViewModel()
        //                             {
        //                                 AllID = c.AllID,
        //                                 CurrentUrl = c.CurrentUrl,
        //                                 Description = c.Description,
        //                                 ID = c.ID,
        //                                 Image = c.Image,
        //                                 Name = c.Name,
        //                                 Order = c.Order,
        //                                 ParentID = c.ParentID,
        //                                 Show = c.Show,
        //                                 ShowInHome = c.ShowInHome,
        //                                 ShowInTab = c.ShowInTab,
        //                                 ShowInTopMenu = c.ShowInTopMenu,
        //                                 TypeOfDisplay = c.TypeOfDisplay,
        //                                 Url = c.Urls.Select(o => o.UrlID).FirstOrDefault()
        //                             }).ToList();

        //        foreach (var item in model.LtsCategory.Where(o => o.ShowInTab && o.ShowInHome > 0))
        //        {
        //            item.LtsContent = db.Contents.Where(o => o.StatusID == (int)Utils.WorkFlowStatus.DA_DUYET && !o.IsRemoved && o.Categories.Any(c => c.ID == item.ID)).OrderByDescending(o => o.PublishDate).ThenByDescending(o => o.CreateDate).Select(o => new ContentViewModel()
        //            {
        //                ID = o.ID,
        //                PublishDate = o.PublishDate,
        //                Title = o.Title,
        //                Url = o.Urls.Select(c => c.UrlID).FirstOrDefault(),
        //                Description = o.Description,
        //                Image = o.Image
        //            }).Skip(0).Take(item.ShowInHome).ToList();

        //        }
        //    }

        //    return model;
        //}


        public List<CategoryViewModel> getAllCategory()
        {
            List<CategoryViewModel> model = new List<CategoryViewModel>();
            using (QNewsDBContext db = new QNewsDBContext())
            {
                model = (from c in db.Categories
                         select new CategoryViewModel()
                         {
                             AllID = c.AllID,
                             CurrentUrl = c.CurrentUrl,
                             Description = c.Description,
                             ID = c.ID,
                             Image = c.Image,
                             Name = c.Name,
                             Order = c.Order,
                             ParentID = c.ParentID,
                             Show = c.Show,
                             ShowInHome = c.ShowInHome,
                             ShowInTab = c.ShowInTab,
                             ShowInTopMenu = c.ShowInTopMenu,
                             TypeOfDisplay = c.TypeOfDisplay,
                             Url = c.Urls.Select(o => o.UrlID).FirstOrDefault()
                         }).ToList();
            }

            return model;
        }

        public List<ContentViewModel> getRss(string urlId)
        {
            List<ContentViewModel> model = new List<ContentViewModel>();
            using (QNewsDBContext db = new QNewsDBContext())
            {
                if (urlId == "newest")
                {
                    model = (from c in db.Contents
                             where c.StatusID == (int)Utils.WorkFlowStatus.DA_DUYET && !c.IsRemoved && c.PublishDate <= DateTime.Now
                             orderby c.PublishDate descending
                             select new ContentViewModel()
                             {
                                 ID = c.ID,
                                 Image = c.Image,
                                 PublishDate = c.PublishDate,
                                 Title = c.Title,
                                 Url = c.Urls.Select(o => o.UrlID).FirstOrDefault(),
                                 Description = c.Description
                             }).Take(20).ToList();
                }
                else
                {
                    var categoryAllID = (from c in db.Categories where c.Urls.Any(o => o.UrlID == urlId) select c.AllID).FirstOrDefault();
                    if (!string.IsNullOrEmpty(categoryAllID))
                    {
                        List<int> AllID = Utils.StaticClass.GetDanhSachIDsQuaFormPost(categoryAllID);
                        model = (from c in db.Contents
                                 where c.Categories.Any(o => AllID.Contains(o.ID))
                                 && c.StatusID == (int)Utils.WorkFlowStatus.DA_DUYET && !c.IsRemoved && c.PublishDate <= DateTime.Now
                                 orderby c.PublishDate descending
                                 select new ContentViewModel()
                                 {
                                     ID = c.ID,
                                     Image = c.Image,
                                     PublishDate = c.PublishDate,
                                     Title = c.Title,
                                     Url = c.Urls.Select(o => o.UrlID).FirstOrDefault(),
                                     Description = c.Description
                                 }).Take(20).ToList();
                    }
                }
            }
            return model;
        }

        public UrlViewModel Search(string kwd, int page)
        {
            UrlViewModel model = new UrlViewModel();

            using (QNewsDBContext db = new QNewsDBContext())
            {
                model.Category.CurrentPage = page;
                model.Category = new CategoryViewModel()
                {
                    Name = "Tìm kiếm",
                    Url = "tim-kiem"
                };

                var queryContent = from c in db.Contents where !c.IsRemoved && c.StatusID == (int)Utils.WorkFlowStatus.DA_DUYET orderby c.PublishDate descending select c;
                if (!string.IsNullOrEmpty(kwd))
                    queryContent = queryContent.Where(c => c.Title.Contains(kwd)).OrderByDescending(c => c.PublishDate);

                model.Category.Total = queryContent.Count();
                model.Category.LtsContent = queryContent.Skip(10 * (page - 1)).Take(10 + 5).Select(o => new ContentViewModel()
                {
                    ID = o.ID,
                    Image = o.Image,
                    PublishDate = o.PublishDate,
                    Title = o.Title,
                    Url = o.Urls.Select(c => c.UrlID).FirstOrDefault(),
                    Description = o.Description
                }).ToList();
                model.Category.PageHtml = new Utils.Paging().getHtmlPage(string.Format("?kwd={0}&p=", System.Web.HttpUtility.HtmlDecode(kwd)), 3, page, 10, model.Category.Total);
            }

            model.LtsMap.Add(model.Category);
            return model;
        }

        public UrlViewModel getContentData(string urlId, Option option)
        {
            List<CategoryViewModel> LtsMap = new List<CategoryViewModel>();
            List<CategoryViewModel> LtsAllCategory = getAllCategory();
            var currentCategoryID = 0;
            UrlViewModel model = new UrlViewModel();

            using (QNewsDBContext db = new QNewsDBContext())
            {

                if (option.ScopeID > 0 && option.TypeOfScope > 0) //Lấy theo scope
                {

                    var currentScope = (from c in db.DocumentScopes where c.ID == option.ScopeID select c).FirstOrDefault();

                    model.Category.CurrentPage = option.CurrentPage;
                    var queryContent = from c in db.Contents where !c.IsRemoved && c.StatusID == (int)Utils.WorkFlowStatus.DA_DUYET && c.ScopeID == option.ScopeID && option.TypeOfScope == c.TypeOfScopeID orderby c.PublishDate descending select c;
                    model.Category.Total = queryContent.Count();
                    model.Category.LtsContent = queryContent.Skip(option.RowPerPage * (option.CurrentPage - 1)).Take(option.RowPerPage + 5).Select(o => new ContentViewModel()
                    {
                        ID = o.ID,
                        Image = o.Image,
                        PublishDate = o.PublishDate,
                        Title = o.Title,
                        Url = o.Urls.Select(c => c.UrlID).FirstOrDefault(),
                        Description = o.Description,
                        ShowDate = o.ShowDate
                    }).ToList();
                    model.Category.PageHtml = new Utils.Paging().getHtmlPage("?p=", 3, option.CurrentPage, option.RowPerPage, model.Category.Total);
                    model.CategoryID = -1;
                    model.Category.Url = "/chuong-trinh-khcn-" + currentScope.ID;


                    CategoryViewModel scope = new CategoryViewModel();
                    if (option.TypeOfScope == 1)
                        scope.Name = "Thông tin chung";
                    else if (option.TypeOfScope == 2)
                        scope.Name = "Các chương trình đã thực hiện";
                    else
                        scope.Name = "Quy trình xét duyệt";

                    model.LtsMap.Add(scope);


                    model.Category.Name = currentScope.Title;
                    model.UrlID = "/chuong-trinh-khcn";
                    model.LtsMap.Add(model.Category);



                    return model;
                }
                else
                {

                    model = (from c in db.Urls
                             where c.UrlID == urlId
                             select new UrlViewModel()
                             {
                                 CategoryID = c.CategoryID,
                                 ContentID = c.ContentID,
                                 DocumentID = c.DocumentID,
                                 AlbumID = c.AlbumID,
                                 UrlID = c.UrlID
                             }).FirstOrDefault();


                    if (model!=null&&model.Category!=null&&model.CategoryID.HasValue)
                    {

                        model.Category.CurrentPage = option.CurrentPage;
                        currentCategoryID = model.CategoryID.Value;
                        model.Category = (from c in db.Categories
                                          where c.ID == model.CategoryID
                                          select new CategoryViewModel()
                                          {
                                              Description = c.Description,
                                              Name = c.Name,
                                              Url = c.Urls.Select(o => o.UrlID).FirstOrDefault(),
                                              AllID = c.AllID,
                                              ID = c.ID
                                          }).FirstOrDefault();
                        var allID = Utils.StaticClass.GetDanhSachIDsQuaFormPost(model.Category.AllID);

                        model.Category.ChildCategory = (from c in db.Categories
                                                        where c.ParentID == model.Category.ID && string.IsNullOrEmpty(c.CurrentUrl) && c.ShowInTab && c.Show
                                                        select new CategoryViewModel()
                                                        {
                                                            ID = c.ID,
                                                            Image = c.Image,
                                                            Name = c.Name,
                                                            Url = c.Urls.FirstOrDefault().UrlID,
                                                            Order = c.Order,
                                                            LtsContent = c.Contents.Where(o => !o.IsRemoved && o.StatusID == (int)Utils.WorkFlowStatus.DA_DUYET).OrderByDescending(o => o.PublishDate).Take(4).ToList().Select(o => new ContentViewModel()
                                                            {
                                                                ID = o.ID,
                                                                Image = o.Image,
                                                                PublishDate = o.PublishDate,
                                                                Title = o.Title,
                                                                Url = o.Urls.Select(u => u.UrlID).FirstOrDefault(),
                                                                Description = o.Description
                                                            }).ToList()
                                                        }).ToList();

                        if (model.Category.ChildCategory.Count == 0)
                        {

                            var queryContent = from c in db.Contents where !c.IsRemoved && c.StatusID == (int)Utils.WorkFlowStatus.DA_DUYET && c.Categories.Any(cat => allID.Contains(cat.ID)) orderby c.PublishDate descending select c;
                            model.Category.Total = queryContent.Count();
                            model.Category.LtsContent = queryContent.Skip(option.RowPerPage * (option.CurrentPage - 1)).Take(option.RowPerPage + 5).Select(o => new ContentViewModel()
                            {
                                ID = o.ID,
                                Image = o.Image,
                                PublishDate = o.PublishDate,
                                Title = o.Title,
                                Url = o.Urls.Select(c => c.UrlID).FirstOrDefault(),
                                Description = o.Description
                            }).ToList();
                            model.Category.PageHtml = new Utils.Paging().getHtmlPage("?p=", 3, option.CurrentPage, option.RowPerPage, model.Category.Total);
                        }

                    }
                    else if (model!=null&&model.DocumentID!=null&& model.DocumentID.HasValue)
                    {
                        model.Document = (from o in db.Documents
                                          where o.ID == model.DocumentID.Value && o.StatusID == (int)Utils.WorkFlowStatus.DA_DUYET
                                          select new DocumentViewModel()
                                          {
                                              DocumentType = o.DocumentType.Title,
                                              FileAttach = o.FileAttach,
                                              ID = o.ID,
                                              NgayBanHanh = o.NgayBanHanh,
                                              NgayHieuLuc = o.NgayHieuLuc,
                                              NguoiKy = o.NguoiKy,
                                              SoKyHieu = o.SoKyHieu,
                                              TrichYeu = o.TrichYeu,
                                              LoaiVanBanID = o.LoaiVanBanID,
                                              Url = o.Urls.FirstOrDefault().UrlID,
                                              LinhVuc = o.DocumentScopes.Select(c => c.Title).ToList(),
                                              CoQuanBanHanh = o.DocumentIssues.Select(c => c.Title).ToList()
                                          }).FirstOrDefault();

                        model.Document.LtsOtherDocument = (from o in db.Documents
                                                           where o.LoaiVanBanID == model.Document.LoaiVanBanID && o.NgayBanHanh < model.Document.NgayBanHanh
                                                           select new DocumentViewModel()
                                                           {
                                                               DocumentType = o.DocumentType.Title,
                                                               FileAttach = o.FileAttach,
                                                               ID = o.ID,
                                                               NgayBanHanh = o.NgayBanHanh,
                                                               NgayHieuLuc = o.NgayHieuLuc,
                                                               NguoiKy = o.NguoiKy,
                                                               SoKyHieu = o.SoKyHieu,
                                                               TrichYeu = o.TrichYeu,
                                                               LoaiVanBanID = o.LoaiVanBanID,
                                                               Url = o.Urls.FirstOrDefault().UrlID,
                                                               LinhVuc = o.DocumentScopes.Select(c => c.Title).ToList(),
                                                               CoQuanBanHanh = o.DocumentIssues.Select(c => c.Title).ToList()
                                                           }).Take(10).ToList();
                    }
                    else if (model != null && model.ContentID != null && model.ContentID.HasValue)
                    {
                        model.Content = (from c in db.Contents
                                         where c.ID == model.ContentID.Value && c.StatusID == (int)Utils.WorkFlowStatus.DA_DUYET
                                         select new ContentViewModel()
                                         {
                                             Description = c.Description,
                                             AllowComment = c.AllowComment,
                                             PublishDate = c.PublishDate,
                                             Details = c.Details,
                                             ID = c.ID,
                                             Image = c.Image,
                                             Source = c.Source,
                                             SourceUrl = c.SourceUrl,
                                             Title = c.Title,
                                             Url = model.UrlID,
                                             Viewed = c.Viewed,
                                             ShowDate = c.ShowDate,
                                             ShowOther = c.ShowOther,
                                             CurrentCategoryID = c.Categories.OrderBy(o => o.MapOrder).ThenByDescending(o => o.ID).Select(p => p.ID).FirstOrDefault()
                                         }).FirstOrDefault();
                        currentCategoryID = model.Content.CurrentCategoryID;

                        model.Content.LtsOtherContent = (from c in db.Contents
                                                         where c.StatusID == (int)Utils.WorkFlowStatus.DA_DUYET && !c.IsRemoved && c.PublishDate < model.Content.PublishDate && c.Categories.Any(cat => cat.ID == currentCategoryID)
                                                         orderby c.PublishDate descending
                                                         select new ContentViewModel()
                                                         {
                                                             ID = c.ID,
                                                             Image = c.Image,
                                                             PublishDate = c.PublishDate,
                                                             Title = c.Title,
                                                             Url = c.Urls.Select(o => o.UrlID).FirstOrDefault(),
                                                             Description = c.Description
                                                         }).Take(8).ToList();

                        model.Content.LtsNewerContent = (from c in db.Contents
                                                         where c.StatusID == (int)Utils.WorkFlowStatus.DA_DUYET && !c.IsRemoved && c.PublishDate > model.Content.PublishDate && c.Categories.Any(cat => cat.ID == currentCategoryID)
                                                         orderby c.PublishDate descending
                                                         select new ContentViewModel()
                                                         {
                                                             ID = c.ID,
                                                             Image = c.Image,
                                                             PublishDate = c.PublishDate,
                                                             Title = c.Title,
                                                             Url = c.Urls.Select(o => o.UrlID).FirstOrDefault(),
                                                             Description = c.Description
                                                         }).Take(8).ToList();
                    }
                    else if (model!=null&&model.AlbumID!=null&&model.AlbumID.HasValue)
                    {
                        model.Album = (from o in db.Albums
                                       where o.ID == model.AlbumID.Value && o.StatusID == (int)Utils.WorkFlowStatus.DA_DUYET
                                       select new AlbumViewModel()
                                       {
                                           ID = o.ID,
                                           Image = o.Image,
                                           PublishDate = o.PublishDate,
                                           Title = o.Title,
                                           Url = o.Urls.Select(c => c.UrlID).FirstOrDefault(),
                                           Description = o.Description,
                                           Details = o.Details,
                                           SourceUrl = o.SourceUrl,
                                           LtsPicture = o.AlbumPictures.Where(
                                                c => !c.IsRemoved && c.StatusID == (int)QNews.Utils.WorkFlowStatus.DA_DUYET && !c.IsRemoved
                                            )
                                            .OrderByDescending(c => c.CreateDate)
                                            .Select(c => new AlbumPictureItem()
                                            {
                                                ID = c.ID,
                                                Image = c.Image,
                                                Title = c.Title
                                            }).ToList()
                                       }).FirstOrDefault();

                    }
                }


                var currentCategory = LtsAllCategory.Where(o => o.ID == currentCategoryID).FirstOrDefault();
                if (currentCategory != null&&model!=null)
                {
                    LtsMap.Add(currentCategory);

                    for (int i = 0; i < 20; i++) //Tối đa 20 cấp
                    {
                        currentCategory = LtsAllCategory.Where(o => o.ID == currentCategory.ParentID).FirstOrDefault();

                        if (currentCategory == null || currentCategory.ID == 0)
                            break;
                        else
                        {
                            if (currentCategory != null)
                            {
                                LtsMap.Add(currentCategory);
                            }
                        }
                    }
                    model.LtsMap = (LtsMap != null) ? LtsMap : null;
                }
                
            }
            return model;
        }

        public MissionViewModal getDetailNhiemVu(int NhiemVuID)
        {
            MissionViewModal model = new MissionViewModal();
            using (Base.QNewsDBContext db = new QNewsDBContext())
            {
                model = (from o in db.Missions
                         where !o.IsRemoved && o.StatusID == (int)QNews.Utils.WorkFlowStatus.DA_DUYET && o.ID == NhiemVuID
                         orderby o.ID descending
                         select new Models.MissionViewModal()
                         {
                             ID = o.ID,
                             MaNhiemVu = o.MaNhiemVu,
                             TenNhiemVu = o.TenNhiemVu,
                             BatDau = o.BatDau,
                             KetThuc = o.KetThuc,
                             ToChucChuTri = o.ToChucChuTri,
                             ChuNhiemNhiemVu = o.ChuNhiemNhiemVu,
                             Details = o.Details
                         }).FirstOrDefault();
            }

            return model;
        }


        public CategoryViewModel getDetailChuongTrinh(int ChuongTrinhID)
        {
            CategoryViewModel model = new CategoryViewModel();
            using (Base.QNewsDBContext db = new QNewsDBContext())
            {
                var chuongTrinh = (from c in db.DocumentScopes where c.ID == ChuongTrinhID select c).FirstOrDefault();
                var allType = (from c in db.TypeOfScopes orderby c.Order select c).ToList();
                if (chuongTrinh != null)
                {
                    model.Name = chuongTrinh.Title;
                    model.Url = "/chuong-trinh-khcn";
                    model.ID = chuongTrinh.ID;
                    model.LtsMap.Add(model);
                    foreach (var type in allType)
                    {
                        var child = new CategoryViewModel();
                        child.Name = type.Title;
                        child.ID = type.ID;
                        child.Order = type.Order;

                        child.LtsContent = (from c in db.Contents
                                            where c.ScopeID == ChuongTrinhID && c.TypeOfScopeID == type.ID && !c.IsRemoved && c.StatusID == (int)QNews.Utils.WorkFlowStatus.DA_DUYET && c.PublishDate <= DateTime.Now
                                            orderby c.PublishDate descending
                                            select new ContentViewModel()
                                            {
                                                ID = c.ID,
                                                Image = c.Image,
                                                PublishDate = c.PublishDate,
                                                Title = c.Title,
                                                Url = c.Urls.Select(o => o.UrlID).FirstOrDefault(),
                                                Description = c.Description,
                                                Details = c.Details
                                            }).Take(10).ToList();

                        model.ChildCategory.Add(child);
                    }

                    model.LtsDocument = (from o in db.Documents
                                         where !o.IsRemoved && o.StatusID == (int)QNews.Utils.WorkFlowStatus.DA_DUYET && o.DocumentScopes.Any(c => c.ID == ChuongTrinhID)
                                         orderby o.NgayBanHanh descending
                                         select new Models.DocumentViewModel()
                                         {
                                             DocumentType = o.DocumentType.Title,
                                             FileAttach = o.FileAttach,
                                             ID = o.ID,
                                             NgayBanHanh = o.NgayBanHanh,
                                             NgayHieuLuc = o.NgayHieuLuc,
                                             NguoiKy = o.NguoiKy,
                                             SoKyHieu = o.SoKyHieu,
                                             TrichYeu = o.TrichYeu,
                                             Url = o.Urls.FirstOrDefault().UrlID,
                                             LinhVuc = o.DocumentScopes.Select(c => c.Title).ToList(),
                                             CoQuanBanHanh = o.DocumentIssues.Select(c => c.Title).ToList()
                                         }).Take(10).ToList();

                    model.LtsMission = (from o in db.Missions
                                        where !o.IsRemoved && o.StatusID == (int)QNews.Utils.WorkFlowStatus.DA_DUYET && o.DocumentScopes.Any(c => c.ID == ChuongTrinhID)
                                        orderby o.ID descending
                                        select new Models.MissionViewModal()
                                        {
                                            ID = o.ID,
                                            MaNhiemVu = o.MaNhiemVu,
                                            TenNhiemVu = o.TenNhiemVu,
                                            BatDau = o.BatDau,
                                            KetThuc = o.KetThuc,
                                            ToChucChuTri = o.ToChucChuTri,
                                            ChuNhiemNhiemVu = o.ChuNhiemNhiemVu
                                        }).Take(5).ToList();
                }
                model.ChuongTrinhIndexNew = (from c in db.DocumentScopes
                                             where c.Show && !c.IsRemoved
                                             orderby c.Order
                                             select new Models.ChuongTrinhHomeViewModelNew()
                                             {
                                                 ID = c.ID,
                                                 Title = c.Title,
                                                 Image = c.Image
                                             }).ToList();
                model.quickLink = (from c in db.QuickLinks
                                   where c.Show
                                   orderby c.Order descending
                                   select new Models.QuickLinkItem()
                                   {
                                       ID = c.ID,
                                       Title = c.Title,
                                       Link = c.Link
                                   }).FirstOrDefault();
            }
            return model;
        }
    }
}
