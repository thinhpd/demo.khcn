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
    public class AlbumPictureDA  : BaseDA
    {
        #region Constructer
        public AlbumPictureDA()
        {
        }

        public AlbumPictureDA(string pathPaging)
        {
            this.PathPaging = pathPaging;
        }

        public AlbumPictureDA(string pathPaging, string pathPagingExt)
        {
            this.PathPaging = pathPaging;
            this.PathPagingext = pathPagingExt;
        }
        #endregion

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<AlbumPictureItem> getAllListSimple()
        {
            var query = from c in QNewsDB.AlbumPictures
                        orderby c.Title
                        select new AlbumPictureItem()
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
        public List<AlbumPictureItem> getListSimpleAll(int StatusID)
        {
            var query = from c in QNewsDB.AlbumPictures
                        where (c.StatusID == StatusID)
                        orderby c.Title
                        select new AlbumPictureItem()
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
        public List<AlbumPictureItem> GetListSimpleByAutoComplete(string keyword, int showLimit)
        {
            var query = from c in QNewsDB.AlbumPictures
                        orderby c.Title
                        where c.Title.Contains(keyword) //autoComplete
                        select new AlbumPictureItem()
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
        public List<AlbumPictureItem> GetListSimpleByAutoComplete(string keyword, int showLimit, int StatusID)
        {
            var query = from c in QNewsDB.AlbumPictures
                        orderby c.Title
                        where c.StatusID == StatusID
                        && c.Title.Contains(keyword) //autoComplete
                        select new AlbumPictureItem()
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
        public List<AlbumPictureItem> getListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);

            var query_first = from c in QNewsDB.AlbumPictures where !c.IsRemoved select c;
            if (Request.CategoryID > 0)
                query_first = query_first.Where(o => o.AlbumID == Request.CategoryID);


            if (Request.StatusID > 0)
                query_first = query_first.Where(o => o.StatusID == Request.StatusID);

            if(!string.IsNullOrEmpty(Request.CreateBy))
                query_first = query_first.Where(o => o.CreateBy == Request.CreateBy);

            if (!string.IsNullOrEmpty(Request.ModifyBy))
                query_first = query_first.Where(o => o.ModifyBy == Request.ModifyBy);

            var query = query_first.Select(o => new
                        {
                            ID = o.ID,
                            Title = o.Title,
                            CreateBy = o.AspNetUser_CreateBy.UserName,
                            CreateDate = o.CreateDate,
                            Description = o.Description,
                            ModifyBy = o.AspNetUser_ModifyBy.UserName,
                            ModifyDate = o.ModifyDate,
                            Album = o.Album.Title,
                            AlbumID = o.AlbumID,
                            StatusID = o.StatusID,
                            Status = o.Status.Name,
                            Image = o.Image
                          
                        });
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList().Select(o => new AlbumPictureItem()
            {
                ID = o.ID,
                Title = o.Title,
                CreateBy = o.CreateBy,
                CreateDate = o.CreateDate,
                Description = o.Description,
                ModifyBy = o.ModifyBy,
                ModifyDate = o.ModifyDate,
                AlbumID = o.AlbumID,
                Album = o.Album,
                StatusID = o.StatusID,
                Status = o.Status,
                Image = o.Image
            }).ToList();
        }

        /// <summary>
        /// Lấy về mảng đơn giản qua mảng ID
        /// </summary>
        /// <param name="LtsArrID"></param>
        /// <returns></returns>
        public List<AlbumPictureItem> getListSimpleByArrID(List<int> LtsArrID)
        {
            var query = from o in QNewsDB.AlbumPictures
                        where LtsArrID.Contains(o.ID)
                        orderby o.ID descending
                        select new AlbumPictureItem()
                        {
                            ID = o.ID,
                            Title = o.Title,
                            CreateBy = o.AspNetUser_CreateBy.UserName,
                            CreateDate = o.CreateDate,
                            Description = o.Description,
                            ModifyBy = o.AspNetUser_ModifyBy.UserName,
                            ModifyDate = o.ModifyDate,
                            Album = o.Album.Title,
                            StatusID = o.StatusID,
                            Status = o.Status.Name
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
        public AlbumPicture getById(int ID)
        {
            var query = from c in QNewsDB.AlbumPictures where c.ID == ID select c;
            return query.FirstOrDefault();
        }

    
        public Base.Version getVersionById(int ID)
        {
            var query = from c in QNewsDB.Versions where c.ID == ID select c;
            return query.FirstOrDefault();
        }

        public List<Album> getListCategoryByArrID(List<int> LtsArrID)
        {
            var query = from c in QNewsDB.Albums where LtsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        /// <summary>
        /// Lấy về danh sách qua mảng id
        /// </summary>
        /// <param name="LtsArrID">Mảng ID</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<AlbumPicture> getListByArrID(List<int> LtsArrID)
        {
            var query = from c in QNewsDB.AlbumPictures where LtsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        /// <summary>
        /// Kiểm tra bản ghi đã tồn tại hay chưa
        /// </summary>
        /// <param name="AlbumPicture">Đối tượng kiểm tra</param>
        /// <returns>Trạng thái tồn tại</returns>
        public bool checkExits(AlbumPicture AlbumPicture)
        {
            var query = from c in QNewsDB.AlbumPictures where ((c.Title == AlbumPicture.Title) && (c.ID != AlbumPicture.ID)) select c;
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
        public List<AlbumPictureItem> getByNameArray(string Titles)
        {
            List<string> ltsName = new List<string>();
            if (Titles.Contains(';'))
                ltsName = Titles.Split(';').Select(o => o.Trim()).ToList();
            else
                ltsName.Add(Titles.Trim());

            var query = from c in QNewsDB.AlbumPictures
                        where ltsName.Contains(c.Title.Trim())
                        select new AlbumPictureItem()
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
        public AlbumPicture getByName(string Title)
        {
            var query = from c in QNewsDB.AlbumPictures where ((c.Title == Title)) select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="AlbumPicture">bản ghi cần thêm</param>
        public void Add(AlbumPicture AlbumPicture)
        {
            QNewsDB.AlbumPictures.Add(AlbumPicture);
        }

        public void AddVersion(Base.Version version)
        {
            QNewsDB.Versions.Add(version);
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="AlbumPicture">Xóa bản ghi</param>
        public void Delete(AlbumPicture AlbumPicture)
        {
            AlbumPicture.IsRemoved = true;

            //AlbumPicture.Categories.Clear();
            //AlbumPicture.Events.Clear();
            //QNewsDB.Urls.Remove(AlbumPicture.Urls.FirstOrDefault());
            //QNewsDB.AlbumPictures.Remove(AlbumPicture);
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
