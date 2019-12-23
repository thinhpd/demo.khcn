using Microsoft.Security.Application;
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
    public class AdvertiseDA  : BaseDA
    {
        #region Constructer
        public AdvertiseDA()
        {
        }

        public AdvertiseDA(string pathPaging)
        {
            this.PathPaging = pathPaging;
        }

        public AdvertiseDA(string pathPaging, string pathPagingExt)
        {
            this.PathPaging = pathPaging;
            this.PathPagingext = pathPagingExt;
        }
        #endregion

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<AdvertiseItem> getAllListSimple()
        {
            var query = from c in QNewsDB.Advertises
                        orderby c.Title
                        select new AdvertiseItem()
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
        public List<AdvertiseItem> getListSimpleAll(int StatusID)
        {
            var query = from c in QNewsDB.Advertises
                        where (c.StatusID == StatusID)
                        orderby c.Title
                        select new AdvertiseItem()
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
        public List<AdvertiseItem> GetListSimpleByAutoComplete(string keyword, int showLimit)
        {
            var query = from c in QNewsDB.Advertises
                        orderby c.Title
                        where c.Title.Contains(keyword) //autoComplete
                        select new AdvertiseItem()
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
        public List<AdvertiseItem> GetListSimpleByAutoComplete(string keyword, int showLimit, int StatusID)
        {
            var query = from c in QNewsDB.Advertises
                        orderby c.Title
                        where c.StatusID == StatusID
                        && c.Title.Contains(keyword) //autoComplete
                        select new AdvertiseItem()
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
        public List<AdvertiseItem> getListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);

            var query_first = from c in QNewsDB.Advertises where !c.IsRemoved select c;


            if (Request.StatusID > 0)
                query_first = query_first.Where(o => o.StatusID == Request.StatusID);

            if (Request.TuNgay.HasValue)
                query_first = query_first.Where(o => o.NgayBatDau >= Request.TuNgay);

            if (Request.DenNgay.HasValue)
                query_first = query_first.Where(o => o.NgayKetThuc <= Request.DenNgay);

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
                            NgayBatDau = o.NgayBatDau,
                            NgayKetThuc = o.NgayKetThuc,
                            StatusID = o.StatusID,
                            Status = o.Status.Name,
                            Image = o.Image,
                            Order = o.Order
                        });
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList().Select(o => new AdvertiseItem()
            {
                ID = o.ID,
                Title = o.Title,
                CreateBy = o.CreateBy,
                CreateDate = o.CreateDate,
                Description = o.Description,
                ModifyBy = o.ModifyBy,
                ModifyDate = o.ModifyDate,
                NgayBatDau = o.NgayBatDau,
                NgayKetThuc = o.NgayKetThuc,
                StatusID = o.StatusID,
                Status = o.Status,
                Image = o.Image,
                Order = o.Order
            }).ToList();
        }

        /// <summary>
        /// Lấy về mảng đơn giản qua mảng ID
        /// </summary>
        /// <param name="LtsArrID"></param>
        /// <returns></returns>
        public List<AdvertiseItem> getListSimpleByArrID(List<int> LtsArrID)
        {
            var query = from o in QNewsDB.Advertises
                        where LtsArrID.Contains(o.ID)
                        orderby o.ID descending
                        select new AdvertiseItem()
                        {
                            ID = o.ID,
                            Title = o.Title,
                            CreateBy = o.AspNetUser_CreateBy.UserName,
                            CreateDate = o.CreateDate,
                            Description = o.Description,
                            ModifyBy = o.AspNetUser_ModifyBy.UserName,
                            ModifyDate = o.ModifyDate,
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
        public Advertise getById(int ID)
        {
            var query = from c in QNewsDB.Advertises where c.ID == ID select c;
            return query.FirstOrDefault();
        }

    
        public Base.Version getVersionById(int ID)
        {
            var query = from c in QNewsDB.Versions where c.ID == ID select c;
            return query.FirstOrDefault();
        }


        /// <summary>
        /// Lấy về danh sách qua mảng id
        /// </summary>
        /// <param name="LtsArrID">Mảng ID</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<Advertise> getListByArrID(List<int> LtsArrID)
        {
            var query = from c in QNewsDB.Advertises where LtsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        /// <summary>
        /// Kiểm tra bản ghi đã tồn tại hay chưa
        /// </summary>
        /// <param name="Advertise">Đối tượng kiểm tra</param>
        /// <returns>Trạng thái tồn tại</returns>
        public bool checkExits(Advertise Advertise)
        {
            var query = from c in QNewsDB.Advertises where ((c.Title == Advertise.Title) && (c.ID != Advertise.ID)) select c;
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
        public List<AdvertiseItem> getByNameArray(string Titles)
        {
            List<string> ltsName = new List<string>();
            if (Titles.Contains(';'))
                ltsName = Titles.Split(';').Select(o => o.Trim()).ToList();
            else
                ltsName.Add(Titles.Trim());

            var query = from c in QNewsDB.Advertises
                        where ltsName.Contains(c.Title.Trim())
                        select new AdvertiseItem()
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
        public Advertise getByName(string Title)
        {
            var query = from c in QNewsDB.Advertises where ((c.Title == Title)) select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="Advertise">bản ghi cần thêm</param>
        public void Add(Advertise Advertise)
        {
            
            QNewsDB.Advertises.Add(Advertise);
        }

        public void AddVersion(Base.Version version)
        {
            QNewsDB.Versions.Add(version);
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="Advertise">Xóa bản ghi</param>
        public void Delete(Advertise Advertise)
        {
            Advertise.IsRemoved = true;

            //Advertise.Categories.Clear();
            //Advertise.Events.Clear();
            //QNewsDB.Urls.Remove(Advertise.Urls.FirstOrDefault());
            //QNewsDB.Advertises.Remove(Advertise);
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
