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
    public class EventDA  : BaseDA
    {
        #region Constructer
        public EventDA()
        {
        }

        public EventDA(string pathPaging)
        {
            this.PathPaging = pathPaging;
        }

        public EventDA(string pathPaging, string pathPagingExt)
        {
            this.PathPaging = pathPaging;
            this.PathPagingext = pathPagingExt;
        }
        #endregion

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<EventItem> getAllListSimple()
        {
            var query = from c in QNewsDB.Events
                        where  !c.IsRemove
                        orderby c.Title
                        select new EventItem()
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
        public List<EventItem> getListSimpleAll(int StatusID)
        {
            var query = from c in QNewsDB.Events
                        where !c.IsRemove && (c.StatusID == StatusID)
                        orderby c.Title
                        select new EventItem()
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
        public List<EventItem> GetListSimpleByAutoComplete(string keyword, int showLimit)
        {
            var query = from c in QNewsDB.Events
                        orderby c.Title
                        where !c.IsRemove && c.Title.Contains(keyword) //autoComplete
                        select new EventItem()
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
        public List<EventItem> GetListSimpleByAutoComplete(string keyword, int showLimit, int StatusID)
        {
            var query = from c in QNewsDB.Events
                        orderby c.Title
                        where !c.IsRemove && c.StatusID == StatusID
                        && c.Title.Contains(keyword) //autoComplete
                        select new EventItem()
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
        public List<EventItem> getListSimpleByRequest(HttpRequestBase httpRequest)
        {

            Request = new ParramRequest(httpRequest);

           var query_first = from c in QNewsDB.Events where !c.IsRemove select c;
           
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

            var query = query_first.Select(o => new
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
                            StatusID = o.StatusID,
                            Status = new StatusItem() { 
                                ID = o.Status.ID,
                                Name = o.Status.Name
                            },
                            Viewed = o.Viewed
                        });
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList().Select(o => new EventItem()
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
                Status = o.Status,
                Viewed = o.Viewed
            }).ToList();
        }

        /// <summary>
        /// Lấy về mảng đơn giản qua mảng ID
        /// </summary>
        /// <param name="LtsArrID"></param>
        /// <returns></returns>
        public List<EventItem> getListSimpleByArrID(List<int> LtsArrID)
        {
            var query = from o in QNewsDB.Events
                        where !o.IsRemove && LtsArrID.Contains(o.ID)
                        orderby o.ID descending
                        select new EventItem()
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
        public Event getById(int ID)
        {
            var query = from c in QNewsDB.Events where !c.IsRemove && c.ID == ID select c;
            return query.FirstOrDefault();
        }

    
        /// <summary>
        /// Lấy về danh sách qua mảng id
        /// </summary>
        /// <param name="LtsArrID">Mảng ID</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<Event> getListByArrID(List<int> LtsArrID)
        {
            var query = from c in QNewsDB.Events where !c.IsRemove && LtsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        /// <summary>
        /// Kiểm tra bản ghi đã tồn tại hay chưa
        /// </summary>
        /// <param name="Event">Đối tượng kiểm tra</param>
        /// <returns>Trạng thái tồn tại</returns>
        public bool checkExits(Event Event)
        {
            var query = from c in QNewsDB.Events where !c.IsRemove && ((c.Title == Event.Title) && (c.ID != Event.ID)) select c;
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
        public List<EventItem> getByNameArray(string Titles)
        {
            List<string> ltsName = new List<string>();
            if (Titles.Contains(';'))
                ltsName = Titles.Split(';').Select(o => o.Trim()).ToList();
            else
                ltsName.Add(Titles.Trim());

            var query = from c in QNewsDB.Events
                        where !c.IsRemove && ltsName.Contains(c.Title.Trim())
                        select new EventItem()
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
        public Event getByName(string Title)
        {
            var query = from c in QNewsDB.Events where !c.IsRemove && ((c.Title == Title)) select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="Event">bản ghi cần thêm</param>
        public void Add(Event Event)
        {
            QNewsDB.Events.Add(Event);
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="Event">Xóa bản ghi</param>
        public void Delete(Event Event)
        {
            Event.IsRemove = true;
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
