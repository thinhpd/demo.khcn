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
    public class AudioDA  : BaseDA
    {
        #region Constructer
        public AudioDA()
        {
        }

        public AudioDA(string pathPaging)
        {
            this.PathPaging = pathPaging;
        }

        public AudioDA(string pathPaging, string pathPagingExt)
        {
            this.PathPaging = pathPaging;
            this.PathPagingext = pathPagingExt;
        }
        #endregion

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<AudioItem> getAllListSimple()
        {
            var query = from c in QNewsDB.Audios
                        orderby c.Title
                        select new AudioItem()
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
        public List<AudioItem> getListSimpleAll(int StatusID)
        {
            var query = from c in QNewsDB.Audios
                        where (c.StatusID == StatusID)
                        orderby c.Title
                        select new AudioItem()
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
        public List<AudioItem> GetListSimpleByAutoComplete(string keyword, int showLimit)
        {
            var query = from c in QNewsDB.Audios
                        orderby c.Title
                        where c.Title.Contains(keyword) //autoComplete
                        select new AudioItem()
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
        public List<AudioItem> GetListSimpleByAutoComplete(string keyword, int showLimit, int StatusID)
        {
            var query = from c in QNewsDB.Audios
                        orderby c.Title
                        where c.StatusID == StatusID
                        && c.Title.Contains(keyword) //autoComplete
                        select new AudioItem()
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
        public List<AudioItem> getListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);

            var query_first = from c in QNewsDB.Audios where !c.IsRemoved select c;
            if (Request.CategoryID > 0)
                query_first = query_first.Where(o => o.TopicID == Request.CategoryID);


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
                            ModifyBy = o.AspNetUser_ModifyBy.UserName,
                            ModifyDate = o.ModifyDate,
                            PublishDate = o.PublishDate,
                            Topic = o.AudioTopic.Title,
                            TopicID = o.TopicID,
                            StatusID = o.StatusID,
                            Status = o.Status.Name,
                            Image = o.Image
                        });
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList().Select(o => new AudioItem()
            {
                ID = o.ID,
                Title = o.Title,
                CreateBy = o.CreateBy,
                CreateDate = o.CreateDate,
                Description = o.Description,
                ModifyBy = o.ModifyBy,
                ModifyDate = o.ModifyDate,
                PublishDate = o.PublishDate,
                TopicID = o.TopicID,
                Topic = o.Topic,
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
        public List<AudioItem> getListSimpleByArrID(List<int> LtsArrID)
        {
            var query = from o in QNewsDB.Audios
                        where LtsArrID.Contains(o.ID)
                        orderby o.ID descending
                        select new AudioItem()
                        {
                            ID = o.ID,
                            Title = o.Title,
                            CreateBy = o.AspNetUser_CreateBy.UserName,
                            CreateDate = o.CreateDate,
                            Description = o.Description,
                            ModifyBy = o.AspNetUser_ModifyBy.UserName,
                            ModifyDate = o.ModifyDate,
                            PublishDate = o.PublishDate,
                            Topic = o.AudioTopic.Title,
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
        public Audio getById(int ID)
        {
            var query = from c in QNewsDB.Audios where c.ID == ID select c;
            return query.FirstOrDefault();
        }

    
        public Base.Version getVersionById(int ID)
        {
            var query = from c in QNewsDB.Versions where c.ID == ID select c;
            return query.FirstOrDefault();
        }

        public List<AudioTopic> getListCategoryByArrID(List<int> LtsArrID)
        {
            var query = from c in QNewsDB.AudioTopics where LtsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        /// <summary>
        /// Lấy về danh sách qua mảng id
        /// </summary>
        /// <param name="LtsArrID">Mảng ID</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<Audio> getListByArrID(List<int> LtsArrID)
        {
            var query = from c in QNewsDB.Audios where LtsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        /// <summary>
        /// Kiểm tra bản ghi đã tồn tại hay chưa
        /// </summary>
        /// <param name="Audio">Đối tượng kiểm tra</param>
        /// <returns>Trạng thái tồn tại</returns>
        public bool checkExits(Audio Audio)
        {
            var query = from c in QNewsDB.Audios where ((c.Title == Audio.Title) && (c.ID != Audio.ID)) select c;
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
        public List<AudioItem> getByNameArray(string Titles)
        {
            List<string> ltsName = new List<string>();
            if (Titles.Contains(';'))
                ltsName = Titles.Split(';').Select(o => o.Trim()).ToList();
            else
                ltsName.Add(Titles.Trim());

            var query = from c in QNewsDB.Audios
                        where ltsName.Contains(c.Title.Trim())
                        select new AudioItem()
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
        public Audio getByName(string Title)
        {
            var query = from c in QNewsDB.Audios where ((c.Title == Title)) select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="Audio">bản ghi cần thêm</param>
        public void Add(Audio Audio)
        {
            QNewsDB.Audios.Add(Audio);
        }

        public void AddVersion(Base.Version version)
        {
            QNewsDB.Versions.Add(version);
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="Audio">Xóa bản ghi</param>
        public void Delete(Audio Audio)
        {
            Audio.IsRemoved = true;

            //Audio.Categories.Clear();
            //Audio.Events.Clear();
            //QNewsDB.Urls.Remove(Audio.Urls.FirstOrDefault());
            //QNewsDB.Audios.Remove(Audio);
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
