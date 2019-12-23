using QNews.Base;
using QNews.Data.Publishing;
using QNews.Models;
using QNews.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace QNews.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new Data.Publishing.ContentDA().getHomeViewModel();
            return View(model);
        }

        public ActionResult Search(string language, string contentType, string kwd, int p = 1)
        {
            var model = QNews.Data.Publishing.ContentDA.SearchContent(kwd, contentType, 10, p);
            return View(model);
        }
        public ActionResult GioiThieu()
        {
            var model = QNews.Data.Publishing.ContentDA.getAbout(10);
            return View(model);
        }
      

        public ActionResult Router(string urlId, int? TypeOfScope, int? ScopeID)
        {

            Data.Publishing.Option option = new Data.Publishing.Option()
            {
                RowPerPage = 10,
                CurrentPage = 1,
                ScopeID = ScopeID.HasValue ? ScopeID.Value : 0,
                TypeOfScope = TypeOfScope.HasValue ? TypeOfScope.Value : 0
            };

            if (!string.IsNullOrEmpty(Request["p"]))
            {
                option.CurrentPage = Convert.ToInt32(Request["p"]);
            }

            Data.Publishing.ContentDA contentDA = new Data.Publishing.ContentDA();
            var model = contentDA.getContentData(urlId, option);

            if (model.CategoryID.HasValue)
            {
                model.Category.LtsMap = model.LtsMap;
                return View("Category", model.Category);
            }
            else if (model.ContentID.HasValue)
            {
                model.Content.LtsMap = model.LtsMap;
                return View("Content", model.Content);
            }
            else if (model.DocumentID.HasValue)
            {
                model.Content.LtsMap = model.LtsMap;
                return View("Document", model.Document);
            }
            else if (model.AlbumID.HasValue)
            {
                model.Content.LtsMap = model.LtsMap;
                return View("Album", model.Album);
            }
            else
            {
                return View("Page");
            }
        }


        public ActionResult IndexChuongTrinh()
        {
            var model = new Data.Publishing.ContentDA().getChuongTrinhIndexNew();
            return View(model);
            
        }

        public ActionResult DetailChuongTrinh(int ChuongTrinhID)
        {
            Data.Publishing.ContentDA contentDA = new Data.Publishing.ContentDA();
            var model = contentDA.getDetailChuongTrinh(ChuongTrinhID);
            ViewData.Model = model;
            return View();
        }

        public ActionResult ListNhiemVu(int ScopeID)
        {
            Data.Publishing.ContentDA contentDA = new Data.Publishing.ContentDA();
            var model = contentDA.getDetailChuongTrinh(ScopeID);
            ViewData.Model = model;
            return View();
        }

        public ActionResult DetailNhiemVu(int ChuongTrinhID, int NhiemVuID)
        {
            Data.Publishing.ContentDA contentDA = new Data.Publishing.ContentDA();
            var model = contentDA.getDetailChuongTrinh(ChuongTrinhID);
            var nhiemvu = new QNews.Data.Publishing.ContentDA().getDetailNhiemVu(NhiemVuID);
            ViewData.Model = model;
            ViewBag.NhiemVu = nhiemvu;
            return View();
        }

        public ActionResult IndexDocument()
        {
            int currentPage = 1;
            int documentType = 0;
            int documentScope = 0;
            int documentIssue = 0;
            string kwd = string.Empty;

            if (!string.IsNullOrEmpty(Request["p"]))
                currentPage = Convert.ToInt32(Request["p"]);
            if (!string.IsNullOrEmpty(Request["type"]))
                documentType = Convert.ToInt32(Request["type"]);

            if (!string.IsNullOrEmpty(Request["scopeID"]))
                documentScope = Convert.ToInt32(Request["scopeID"]);

            if (!string.IsNullOrEmpty(Request["issue"]))
                documentIssue = Convert.ToInt32(Request["issue"]);

            if (!string.IsNullOrEmpty(Request["kwd"]))
                kwd = Request["kwd"];

            var model = new Data.Publishing.ContentDA().GetDocumentList(currentPage, documentType, documentScope, documentIssue,  kwd);
            return View(model);
        }

        public ActionResult Rss(string urlId)
        {
            if (urlId.Contains("/"))
                urlId = urlId.Replace("/", "");

            var model = new Data.Publishing.ContentDA().getRss(urlId);

            string domainPath = "http://" + Request.Url.Host;

            StringBuilder stb = new StringBuilder();
            int total = 0;

            //stb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n");
            ////stb.Append("<?xml-stylesheet type='text/xsl' href='/Content/Publishing/rss/ePiRSS.xsl'?>\r\n");
            //stb.Append("<rss version=\"2.0\">\r\n");
            //stb.Append("  <channel>\r\n");
            //stb.Append("    <title>Tin nổi bật trong ngày</title>\r\n");
            //stb.Append("    <description>Những tin tức được quan tâm nhất trong ngày</description>\r\n");
            //stb.AppendFormat("    <link>{0}/rss/newest</link>\r\n", domainPath);
            //stb.Append("    <language>vi-vn</language>\r\n");
            //stb.Append("    <copyright>Copyright (c) 2016 VGP. All rights reserved</copyright>\r\n");
            ////stb.Append("    <webMaster>webmaster@thanglong.chinhphu.vn</webMaster>\r\n");
            //stb.AppendFormat("    <lastBuildDate>{0}</lastBuildDate>\r\n", GetRFC822Date(DateTime.Now));
            //stb.AppendFormat("    <docs>{0}/rss/newest</docs>\r\n", domainPath);
            //stb.Append("    <generator>http://thanglong.chinhphu.vn</generator>\r\n");
            //stb.Append("    <ttl>5</ttl>\r\n");

            //foreach (var item in model)
            //{
            //    string image = !string.IsNullOrEmpty(item.Image) ? string.Format("{0}{1}", domainPath, item.Image) : "";
            //    string linkUrl = string.Format("{0}/{1}", domainPath, item.Url);
            //    stb.Append("    <item>\r\n");
            //    stb.AppendFormat("      <title>{0}</title>\r\n", item.Title);
            //    stb.AppendFormat("      <description>{0}</description>\r\n", System.Web.HttpUtility.HtmlDecode(Utils.StaticClass.RemoveHTMLTag(item.Description)).Replace("\\n", " ").Trim());
            //    stb.AppendFormat("      <link>{0}</link>\r\n", linkUrl);
            //    stb.AppendFormat("      <guid isPermaLink=\"false\">{0}</guid>\r\n", linkUrl);
            //    if (!string.IsNullOrEmpty(item.Image))
            //        stb.AppendFormat("      <enclosure url=\"{0}\" length=\"0\" type=\"image/jpeg\" />\r\n", image);
            //    stb.AppendFormat("      <pubDate>{0}</pubDate>\r\n", GetRFC822Date(item.PublishDate.Value));
            //    stb.Append("    </item>\r\n");
            //}

            //stb.Append("  </channel>\r\n");
            //stb.Append("</rss>\r\n");



            stb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            stb.Append("<rss version=\"2.0\" xmlns:atom=\"http://www.w3.org/2005/Atom\">");
            stb.Append("  <channel>");
            stb.Append("    <title><![CDATA[ Trang thông tin điện tử Thăng Long - Hà Nội]]></title>");
            stb.AppendFormat("    <link><![CDATA[ {0} ]]></link>", domainPath);
            stb.Append("    <description><![CDATA[  -  Trang thông tin điện tử Thăng Long - Hà Nội - ]]></description>");
            stb.Append("    <ttl>10</ttl>");
            stb.AppendFormat("    <copyright>{0}</copyright>", domainPath);
            stb.AppendFormat("    <pubDate>{0}</pubDate>", GetRFC822Date(DateTime.Now));
            stb.Append("    <generator>Trang thông tin điện tử Thăng Long - Hà Nội</generator>");
            stb.AppendFormat("    <docs>{0}</docs>", domainPath);
            stb.Append("    <image>");
            stb.Append("      <title><![CDATA[ Trang thông tin điện tử Thăng Long - Hà Nội]]></title>");
            stb.AppendFormat("      <url>{0}/Content/Publishing/img/logo.png</url>", domainPath);
            stb.AppendFormat("      <link><![CDATA[ {0} ]]></link>", domainPath);
            stb.Append("      <width>121</width>");
            stb.Append("      <height>40</height>");
            stb.Append("    </image>");

            foreach (var item in model)
            {
                string image = !string.IsNullOrEmpty(item.Image) ? string.Format("{0}{1}", domainPath, item.Image) : "";
                string linkUrl = string.Format("{0}/{1}", domainPath, item.Url);
                stb.Append("    <item>");
                stb.AppendFormat("      <title><![CDATA[ {0}]]></title>", ConvertEncode(item.Title));
                stb.AppendFormat("      <link><![CDATA[ {0} ]]></link>", linkUrl);
                stb.AppendFormat("      <enclosure url=\"{0}\" length=\"0\" type=\"image/jpeg\" />", image);
                stb.AppendFormat("      <guid isPermaLink=\"false\"><![CDATA[ {0} ]]></guid>", linkUrl);
                stb.AppendFormat("      <description><![CDATA[ {0}]]></description>", ConvertEncode(item.Description));
                stb.AppendFormat("      <pubDate>{0}</pubDate>", GetRFC822Date(item.PublishDate.Value));
                stb.Append("    </item>");
            }
            stb.Append("  </channel>");
            stb.Append("</rss>");

            return new ContentResult
            {
                ContentType = "text/xml",
                Content = stb.ToString(),
                ContentEncoding = System.Text.Encoding.UTF8
            };


        }

        public ActionResult IndexRss()
        {
            var model = new Data.Publishing.ContentDA().getAllCategory();
            return View(model);
        }

        public ActionResult Sitemap()
        {
            var model = new Data.Publishing.ContentDA().getAllCategory();
            return View(model);
        }

        private static string ConvertEncode(string source)
        {
            //source = source.Replace("<", "&#x3C;");
            //source = source.Replace(">", "&#x3E;");
            //source = source.Replace("&", "&#x26;");
            return source;
        }

        private static string GetRFC822Date(DateTime date)
        {
            int offset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
            string timeZone = "+" + offset.ToString().PadLeft(2, '0');
            if (offset < 0)
            {
                int i = offset * -1;
                timeZone = "-" + i.ToString().PadLeft(2, '0');
            }
            return date.ToString("ddd, dd MMM yyyy HH:mm:ss " + timeZone.PadRight(5, '0'));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(HomeViewModel model)
        {
            Register register = new Register();
            register.Name = model.dkName;
            register.Email = model.dkEMail;
            register.Phone = model.dkphone;
            register.Scope_ID = model.ct_id;
            register.CreateDate= DateTime.Now;
            string messages = "";
            RegisterDA _register = new RegisterDA();
            try
            {
                if (_register.checkExits(register))
                {
                    messages = "Email hoặc SĐT đã được đăng ký trên hệ thống!";
                }
                else
                {
                    _register.Add(register);
                    _register.Save();
                    messages = "Đăng ký nhận tin thành công!";
                }
            }catch(Exception e)
            {
                messages = "Lỗi đăng ký nhận tin!";
            }

            model = new Data.Publishing.ContentDA().getHomeViewModel();
            model.jmessage = messages;
            
            
            return View(model);
        }
        
        
    }
}