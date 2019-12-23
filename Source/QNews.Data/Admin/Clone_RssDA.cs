using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;
using QNews.Models;
using QNews.Base;
using QNews.Data.Admin;

namespace QNews.Data
{
    public partial class Clone_RssDA : BaseDA
    {
        #region Constructer
        public Clone_RssDA()
        { 
        }

        public Clone_RssDA(string pathPaging)
        {
            this.PathPaging = pathPaging;
        }

        public Clone_RssDA(string pathPaging, string pathPagingExt)
        {
            this.PathPaging = pathPaging;
            this.PathPagingext = pathPagingExt;
        }
        #endregion

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<RssItem> getListSimpleAll()
        {
            var query = from c in QNewsDB.Clone_Rss
                        orderby c.RssTitle
                        select new RssItem()
                        {
                            RssID = c.RssID,
                            RssTitle = c.RssTitle,
                            RssDomainName = c.Clone_Feed.Clone_Domain.Title,
                            RssSource = c.RssSource
                        };
            return query.ToList();
        }

        /// <summary>
        /// Lấy về dưới dạng Autocomplete
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="showLimit"></param>
        /// <returns></returns>
        public List<RssItem> GetListSimpleByAutoComplete(string keyword, int showLimit)
        {
            var query = from c in QNewsDB.Clone_Rss
                        orderby c.RssTitle
                        where c.RssTitle.Contains(keyword) //autoComplete
                        select new RssItem()
                        {
                            RssID = c.RssID,
                            RssTitle = c.RssTitle
                        };
            return query.Take(showLimit).ToList(); 
        }

        /// <summary>
        /// Lấy về kiểu đơn giản, phân trang
        /// </summary>
        /// <param name="pageSize">Số bản ghi trên trang</param>
        /// <param name="Page">Trang hiển thị</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<RssItem> getListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in QNewsDB.Clone_Rss select new RssItem() {
                RssID = c.RssID,
                RssTitle = c.RssTitle,
                RssDomainName = c.Clone_Feed.Clone_Domain.Title,
                RssActive = c.RssActive,
                RssSource = c.RssSource,
                RssCreated = c.RssCreated,
                RssDescription = c.RssDescription,
                RssImage = c.RssImage,
                RssOrder = c.RssOrder
            };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        /// <summary>
        /// Lấy về mảng đơn giản qua mảng ID
        /// </summary>
        /// <param name="LtsArrID"></param>
        /// <returns></returns>
        public List<RssItem> getListSimpleByArrID(List<int> LtsArrID)
        {
            var query = from c in QNewsDB.Clone_Rss
                        where LtsArrID.Contains(c.RssID)
                        orderby c.RssID descending
                        select new RssItem()
                        {
                            RssID = c.RssID,
                            RssTitle = c.RssTitle,
                            RssDomainName = c.Clone_Feed.Clone_Domain.Title,
                            RssActive = c.Contents.Count == 0,
                            RssSource = c.RssSource,
                            RssCreated = c.RssCreated,
                            RssDescription = c.RssDescription
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
        public Clone_Rss getById(int ID)
        {
            var query = from c in QNewsDB.Clone_Rss where c.RssID == ID select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Lấy về danh sách qua mảng id
        /// </summary>
        /// <param name="LtsArrID">Mảng ID</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<Clone_Rss> getListByArrID(List<int> LtsArrID)
        {
            var query = from c in QNewsDB.Clone_Rss where LtsArrID.Contains(c.RssID) select c;
            return query.ToList();
        }

        /// <summary>
        /// Kiểm tra bản ghi đã tồn tại hay chưa
        /// </summary>
        /// <param name="clone_noderemove">Đối tượng kiểm tra</param>
        /// <returns>Trạng thái tồn tại</returns>
        public bool checkExits(Clone_Rss clone_noderemove)
        {
            var query = from c in QNewsDB.Clone_Rss where ((c.RssTitle == clone_noderemove.RssTitle) && (c.RssID != clone_noderemove.RssID)) select c;
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
        public List<RssItem> getByNameArray(string NewsTitles)
        {
            List<string> ltsName = new List<string>();
            if (NewsTitles.Contains(';'))
                ltsName = NewsTitles.Split(';').Select(o => o.Trim()).ToList();
            else
                ltsName.Add(NewsTitles.Trim());

            var query = from c in QNewsDB.Clone_Rss
                        where ltsName.Contains(c.RssTitle.Trim())
                        select new RssItem()
                        {
                            RssID = c.RssID,
                            RssTitle = c.RssTitle
                        };
            return query.ToList();
        }

        /// <summary>
        /// Lấy về bản ghi qua tên
        /// </summary>
        /// <param name="Title">Tên bản ghi</param>
        /// <returns>Bản ghi</returns>
        public Clone_Rss getByName(string Title)
        {
            var query = from c in QNewsDB.Clone_Rss where ((c.RssTitle == Title)) select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="clone_noderemove">bản ghi cần thêm</param>
        public void Add(Clone_Rss clone_noderemove)
        {
            QNewsDB.Clone_Rss.Add(clone_noderemove);
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="clone_noderemove">Xóa bản ghi</param>
        public void Delete(Clone_Rss clone_noderemove)
        {
            QNewsDB.Clone_Rss.Remove(clone_noderemove);
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
