using QNews.Base;
using QNews.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QNews.Data.Admin
{
    public class ContentDA  : BaseDA
    {
        #region Constructer
        public ContentDA()
        {
        }

        public ContentDA(string pathPaging)
        {
            this.PathPaging = pathPaging;
        }

        public ContentDA(string pathPaging, string pathPagingExt)
        {
            this.PathPaging = pathPaging;
            this.PathPagingext = pathPagingExt;
        }
        #endregion

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<ContentItem> getAllListSimple()
        {
            var query = from c in QNewsDB.Contents
                        orderby c.Title
                        select new ContentItem()
                        {
                            ID = c.ID,
                            Title = c.Title
                        };
            return query.ToList();
        }

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <param name="IsShow">Kiểm tra hiển thị</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<ContentItem> getListSimpleAll(int StatusID)
        {
            var query = from c in QNewsDB.Contents
                        where (c.StatusID == StatusID)
                        orderby c.Title
                        select new ContentItem()
                        {
                            ID = c.ID,
                            Title = c.Title
                        };
            return query.ToList();
        }

        /// <summary>
        /// Lấy về dưới dạng Autocomplete
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="showLimit"></param>
        /// <returns></returns>
        public List<ContentItem> GetListSimpleByAutoComplete(string keyword, int showLimit)
        {
            var query = from c in QNewsDB.Contents
                        orderby c.Title
                        where c.Title.Contains(keyword) //autoComplete
                        select new ContentItem()
                        {
                            ID = c.ID,
                            Title = c.Title
                        };
            return query.Take(showLimit).ToList();
        }

        /// <summary>
        /// Lấy về dưới dạng Autocomplete
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="showLimit"></param>
        /// <returns></returns>
        public List<ContentItem> GetListSimpleByAutoComplete(string keyword, int showLimit, int StatusID)
        {
            var query = from c in QNewsDB.Contents
                        orderby c.Title
                        where c.StatusID == StatusID
                        && c.Title.Contains(keyword) //autoComplete
                        select new ContentItem()
                        {
                            ID = c.ID,
                            Title = c.Title
                        };
            return query.Take(showLimit).ToList();
        }


        /// <summary>
        /// Lấy về kiểu đơn giản, phân trang
        /// </summary>
        /// <param name="pageSize">Số bản ghi trên trang</param>
        /// <param name="Page">Trang hiển thị</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<ContentItem> getListFullByRequest(HttpRequestBase httpRequest, ref int Total)
        {
            Request = new ParramRequest(httpRequest);

            var query_first = from c in QNewsDB.Contents where !c.IsRemoved select c;
            if (Request.CategoryID > 0)
                query_first = query_first.Where(o => o.Categories.Any(c => c.ID == Request.CategoryID));

            if (Request.EventID > 0)
                query_first = query_first.Where(o => o.ScopeID == Request.EventID);

            if (Request.StatusID > 0)
                query_first = query_first.Where(o => o.StatusID == Request.StatusID);

            if (Request.TuNgay.HasValue)
                query_first = query_first.Where(o => o.PublishDate >= Request.TuNgay);

            if (Request.DenNgay.HasValue)
                query_first = query_first.Where(o => o.PublishDate <= Request.DenNgay);

            if (!string.IsNullOrEmpty(Request.CreateBy))
                query_first = query_first.Where(o => o.CreateBy == Request.CreateBy);

            if (!string.IsNullOrEmpty(Request.ModifyBy))
                query_first = query_first.Where(o => o.ModifyBy == Request.ModifyBy);

            if (Request.AuthorID > 0)
                query_first = query_first.Where(o => o.AuthorID == Request.AuthorID);

            if (Request.PhotoID > 0)
                query_first = query_first.Where(o => o.PhotoID == Request.PhotoID);

            if (Request.EditorID > 0)
                query_first = query_first.Where(o => o.EditorID == Request.EditorID);

            var query = query_first.Select(o => new
            {
                ID = o.ID,
                Title = o.Title,
                CreateBy = o.AspNetUser_CreateBy.UserName,
                CreateDate = o.CreateDate,
                Description = o.Description,
                Image = o.Image,
                ModifyBy = o.AspNetUser_ModifyBy.UserName,
                CreateBy_Name = o.AspNetUser_CreateBy.UserFullName,
                ModifyBy_Name = o.AspNetUser_ModifyBy.UserFullName,
                ModifyDate = o.ModifyDate,
                PublishDate = o.PublishDate,
                StatusID = o.StatusID,
                EditorBy = o.EditorID.HasValue ? o.Contributor_EditorID.Title : string.Empty,
                PhotoBy = o.PhotoID.HasValue ? o.Contributor_PhotoID.Title : string.Empty,
                AuthorBy = o.AuthorID.HasValue ? o.Contributor_AuthorID.Title : string.Empty,
                Status = new StatusItem()
                {
                    ID = o.Status.ID,
                    Name = o.Status.Name
                },
                Viewed = o.Viewed,
                Version = o.Versions.Count + 1,
                Categories = o.Categories.Select(c => c.Name),
                Details = o.Details,
                Urls = o.Urls.Select(c => c.UrlID).FirstOrDefault()

            });
            query = query.SelectByRequest(Request, ref TotalRecord);
            Total = TotalRecord;
            return query.ToList().Select(o => new ContentItem()
            {
                ID = o.ID,
                Title = o.Title,
                CreateBy = o.CreateBy,
                CreateDate = o.CreateDate,
                Description = o.Description,
                Image = o.Image,
                ModifyBy = o.ModifyBy,
                CreateBy_Name = o.CreateBy_Name,
                ModifyBy_Name = o.ModifyBy_Name,
                ModifyDate = o.ModifyDate,
                PublishDate = o.PublishDate,
                StatusID = o.StatusID,
                Status = o.Status,
                Viewed = o.Viewed,
                Version = o.Version,
                Categories = o.Categories.ToList(),
                Details = o.Details,
                EditorBy = o.EditorBy,
                PhotoBy = o.PhotoBy,
                AuthorBy = o.AuthorBy,
                Url = o.Urls
            }).ToList();
        }

        /// <summary>
        /// Lấy về kiểu đơn giản, phân trang
        /// </summary>
        /// <param name="pageSize">Số bản ghi trên trang</param>
        /// <param name="Page">Trang hiển thị</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<ContentItem> getListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);

            var query_first = from c in QNewsDB.Contents where !c.IsRemoved select c;
            if (Request.CategoryID > 0)
                query_first = query_first.Where(o => o.Categories.Any(c => c.ID == Request.CategoryID));

            if (Request.EventID > 0)
                query_first = query_first.Where(o => o.ScopeID == Request.EventID);

            if (Request.StatusID > 0)
                query_first = query_first.Where(o => o.StatusID == Request.StatusID);

            if (Request.TuNgay.HasValue)
                query_first = query_first.Where(o => o.PublishDate >= Request.TuNgay);

            if (Request.DenNgay.HasValue)
                query_first = query_first.Where(o => o.PublishDate <= Request.DenNgay);

            if(!string.IsNullOrEmpty(Request.CreateBy))
                query_first = query_first.Where(o => o.CreateBy == Request.CreateBy);

            if (!string.IsNullOrEmpty(Request.ModifyBy))
                query_first = query_first.Where(o => o.ModifyBy == Request.ModifyBy);

            if(Request.AuthorID > 0)
                query_first = query_first.Where(o => o.AuthorID == Request.AuthorID);

            if (Request.PhotoID > 0)
                query_first = query_first.Where(o => o.PhotoID == Request.PhotoID);

            if (Request.EditorID > 0)
                query_first = query_first.Where(o => o.EditorID == Request.EditorID);

            var query = query_first.Select(o => new
                        {
                            ID = o.ID,
                            Title = o.Title,
                            CreateBy = o.AspNetUser_CreateBy.UserName,
                            CreateBy_Name = o.AspNetUser_CreateBy.UserFullName,
                            ModifyBy_Name = o.AspNetUser_ModifyBy.UserFullName,
                            CreateDate = o.CreateDate,
                            Description = o.Description,
                            Image = o.Image,
                            ModifyBy = o.AspNetUser_ModifyBy.UserName,
                            ModifyDate = o.ModifyDate,
                            PublishDate = o.PublishDate,
                            StatusID = o.StatusID,
                            Status = new StatusItem() { 
                                ID = o.Status.ID,
                                Name = o.Status.Name
                            },
                            EditorBy = o.EditorID.HasValue ? o.Contributor_EditorID.Title : string.Empty,
                            PhotoBy = o.PhotoID.HasValue ? o.Contributor_PhotoID.Title : string.Empty,
                            AuthorBy = o.AuthorID.HasValue ? o.Contributor_AuthorID.Title : string.Empty,
                            Viewed = o.Viewed,
                            Version = o.Versions.Count + 1
                        });
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList().Select(o => new ContentItem()
            {
                ID = o.ID,
                Title = o.Title,
                CreateBy = o.CreateBy,
                CreateDate = o.CreateDate,
                Description = o.Description,
                Image = o.Image,
                ModifyBy = o.ModifyBy,
                ModifyDate = o.ModifyDate,
                PublishDate = o.PublishDate,
                StatusID = o.StatusID,
                CreateBy_Name = o.CreateBy_Name,
                ModifyBy_Name = o.ModifyBy_Name,
                EditorBy = o.EditorBy,
                PhotoBy = o.PhotoBy,
                AuthorBy = o.AuthorBy,
                Status = o.Status,
                Viewed = o.Viewed,
                Version = o.Version
            }).ToList();
        }

        public List<ContentItem> getListSimpleByRequest_Excel(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);

            var query_first = from c in QNewsDB.Contents where !c.IsRemoved select c;
            if (Request.CategoryID > 0)
                query_first = query_first.Where(o => o.Categories.Any(c => c.ID == Request.CategoryID));

            if (Request.EventID > 0)
                query_first = query_first.Where(o => o.ScopeID == Request.EventID);

            if (Request.StatusID > 0)
                query_first = query_first.Where(o => o.StatusID == Request.StatusID);

            if (Request.TuNgay.HasValue)
                query_first = query_first.Where(o => o.PublishDate >= Request.TuNgay);

            if (Request.DenNgay.HasValue)
                query_first = query_first.Where(o => o.PublishDate <= Request.DenNgay);

            if (!string.IsNullOrEmpty(Request.CreateBy))
                query_first = query_first.Where(o => o.CreateBy == Request.CreateBy);

            if (!string.IsNullOrEmpty(Request.ModifyBy))
                query_first = query_first.Where(o => o.ModifyBy == Request.ModifyBy);

            if (Request.AuthorID > 0)
                query_first = query_first.Where(o => o.AuthorID == Request.AuthorID);

            if (Request.PhotoID > 0)
                query_first = query_first.Where(o => o.PhotoID == Request.PhotoID);

            if (Request.EditorID > 0)
                query_first = query_first.Where(o => o.EditorID == Request.EditorID);

            var query = query_first.Select(o => new
            {
                ID = o.ID,
                Title = o.Title,
                CreateBy = o.AspNetUser_CreateBy.UserName,
                CreateBy_Name = o.AspNetUser_CreateBy.UserFullName,
                ModifyBy_Name = o.AspNetUser_ModifyBy.UserFullName,
                CreateDate = o.CreateDate,
                Description = o.Description,
                Image = o.Image,
                ModifyBy = o.AspNetUser_ModifyBy.UserName,
                ModifyDate = o.ModifyDate,
                PublishDate = o.PublishDate,
                StatusID = o.StatusID,
                Status = new StatusItem()
                {
                    ID = o.Status.ID,
                    Name = o.Status.Name
                },
                EditorBy = o.EditorID.HasValue ? o.Contributor_EditorID.Title : string.Empty,
                PhotoBy = o.PhotoID.HasValue ? o.Contributor_PhotoID.Title : string.Empty,
                AuthorBy = o.AuthorID.HasValue ? o.Contributor_AuthorID.Title : string.Empty,
                Viewed = o.Viewed,
                Version = o.Versions.Count + 1
            });
            Request.RowPerPage = 100000;
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList().Select(o => new ContentItem()
            {
                ID = o.ID,
                Title = o.Title,
                CreateBy = o.CreateBy,
                CreateDate = o.CreateDate,
                Description = o.Description,
                Image = o.Image,
                ModifyBy = o.ModifyBy,
                ModifyDate = o.ModifyDate,
                PublishDate = o.PublishDate,
                StatusID = o.StatusID,
                CreateBy_Name = o.CreateBy_Name,
                ModifyBy_Name = o.ModifyBy_Name,
                EditorBy = o.EditorBy,
                PhotoBy = o.PhotoBy,
                AuthorBy = o.AuthorBy,
                Status = o.Status,
                Viewed = o.Viewed,
                Version = o.Version
            }).ToList();
        }

        /// <summary>
        /// Lấy về mảng đơn giản qua mảng ID
        /// </summary>
        /// <param name="LtsArrID"></param>
        /// <returns></returns>
        public List<ContentItem> getListSimpleByArrID(List<int> LtsArrID)
        {
            var query = from o in QNewsDB.Contents
                        where LtsArrID.Contains(o.ID)
                        orderby o.ID descending
                        select new ContentItem()
                        {
                            ID = o.ID,
                            Title = o.Title,
                            CreateBy = o.AspNetUser_CreateBy.UserName,
                            CreateDate = o.CreateDate,
                            Description = o.Description,
                            Image = o.Image,
                            ModifyBy = o.AspNetUser_ModifyBy.UserName,
                            ModifyDate = o.ModifyDate,
                            PublishDate = o.PublishDate,
                            Status = new StatusItem()
                            {
                                ID = o.Status.ID,
                                Name = o.Status.Name
                            },
                            Viewed = o.Viewed
                        };
            TotalRecord = query.Count();
            return query.ToList();
        }

        #region Check Exits, Add, Update, Delete
        /// <summary>
        /// Lấy về bản ghi qua khóa chính
        /// </summary>
        /// <param name="ID">ID bản ghi</param>
        /// <returns>Bản ghi</returns>
        public Content getById(int ID)
        {
            var query = from c in QNewsDB.Contents where c.ID == ID select c;
            return query.FirstOrDefault();
        }

    
        public Base.Version getVersionById(int ID)
        {
            var query = from c in QNewsDB.Versions where c.ID == ID select c;
            return query.FirstOrDefault();
        }

        public List<Category> getListCategoryByArrID(List<int> LtsArrID)
        {
            var query = from c in QNewsDB.Categories where LtsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        public List<Event> getListEventByArrID(List<int> LtsArrID)
        {
            var query = from c in QNewsDB.Events where LtsArrID.Contains(c.ID) select c;
            return query.ToList();
        }


        /// <summary>
        /// Lấy về danh sách qua mảng id
        /// </summary>
        /// <param name="LtsArrID">Mảng ID</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<Content> getListByArrID(List<int> LtsArrID)
        {
            var query = from c in QNewsDB.Contents where LtsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        /// <summary>
        /// Kiểm tra bản ghi đã tồn tại hay chưa
        /// </summary>
        /// <param name="Content">Đối tượng kiểm tra</param>
        /// <returns>Trạng thái tồn tại</returns>
        public bool checkExits(Content Content)
        {
            var query = from c in QNewsDB.Contents where ((c.Title == Content.Title) && (c.ID != Content.ID)) select c;
            if (query.Count() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Lấy về bản ghi qua tên
        /// </summary>
        /// <param name="QuestionTitles">Tên bản ghi</param>
        /// <returns>Bản ghi</returns>
        public List<ContentItem> getByNameArray(string Titles)
        {
            List<string> ltsName = new List<string>();
            if (Titles.Contains(';'))
                ltsName = Titles.Split(';').Select(o => o.Trim()).ToList();
            else
                ltsName.Add(Titles.Trim());

            var query = from c in QNewsDB.Contents
                        where ltsName.Contains(c.Title.Trim())
                        select new ContentItem()
                        {
                            ID = c.ID,
                            Title = c.Title
                        };
            return query.ToList();
        }

        /// <summary>
        /// Lấy về bản ghi qua tên
        /// </summary>
        /// <param name="Title">Tên bản ghi</param>
        /// <returns>Bản ghi</returns>
        public Content getByName(string Title)
        {
            var query = from c in QNewsDB.Contents where ((c.Title == Title)) select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="Content">bản ghi cần thêm</param>
        public void Add(Content Content)
        {
            QNewsDB.Contents.Add(Content);
        }

        public void AddVersion(Base.Version version)
        {
            QNewsDB.Versions.Add(version);
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="Content">Xóa bản ghi</param>
        public void Delete(Content Content)
        {
            Content.IsRemoved = true;

            //Content.Categories.Clear();
            //Content.Events.Clear();
            //QNewsDB.Urls.Remove(Content.Urls.FirstOrDefault());
            //QNewsDB.Contents.Remove(Content);
        }

        /// <summary>
        /// save bản ghi vào DB
        /// </summary>
        public void Save()
        {
            QNewsDB.SaveChanges();
        }
        #endregion
    }
}
