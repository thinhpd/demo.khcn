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
    public partial class Clone_FeedDA : BaseDA
    {
        #region Constructer
        public Clone_FeedDA()
        { 
        }

        public Clone_FeedDA(string pathPaging)
        {
            this.PathPaging = pathPaging;
        }

        public Clone_FeedDA(string pathPaging, string pathPagingExt)
        {
            this.PathPaging = pathPaging;
            this.PathPagingext = pathPagingExt;
        }
        #endregion

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<FeedItem> getListSimpleAll()
        {
            var query = from c in QNewsDB.Clone_Feed
                        orderby c.FeedTitle
                        select new FeedItem()
                        {
                            FeedID = c.FeedID,
                            FeedTitle = c.FeedTitle,
                            FeedDomainName = c.Clone_Domain.Title,
                            FeedActive = c.FeedActive,
                            FeedSource = c.FeedSource
                        };
            return query.ToList();
        }

        /// <summary>
        /// Lấy về dưới dạng Autocomplete
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="showLimit"></param>
        /// <returns></returns>
        public List<FeedItem> GetListSimpleByAutoComplete(string keyword, int showLimit)
        {
            var query = from c in QNewsDB.Clone_Feed
                        orderby c.FeedTitle
                        where c.FeedTitle.Contains(keyword) //autoComplete
                        select new FeedItem()
                        {
                            FeedID = c.FeedID,
                            FeedTitle = c.FeedTitle
                        };
            return query.Take(showLimit).ToList(); 
        }

        /// <summary>
        /// Lấy về kiểu đơn giản, phân trang
        /// </summary>
        /// <param name="pageSize">Số bản ghi trên trang</param>
        /// <param name="Page">Trang hiển thị</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<FeedItem> getListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in QNewsDB.Clone_Feed select new FeedItem() {
                FeedID = c.FeedID,
                FeedTitle = c.FeedTitle,
                FeedDomainName = c.Clone_Domain.Title,
                FeedActive = c.FeedActive,
                FeedSource = c.FeedSource,
                CategoryName = c.Category.Name
            };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        /// <summary>
        /// Lấy về mảng đơn giản qua mảng ID
        /// </summary>
        /// <param name="LtsArrID"></param>
        /// <returns></returns>
        public List<FeedItem> getListSimpleByArrID(List<int> LtsArrID)
        {
            var query = from c in QNewsDB.Clone_Feed
                        where LtsArrID.Contains(c.FeedID)
                        orderby c.FeedID descending
                        select new FeedItem()
                        {
                            FeedID = c.FeedID,
                            FeedTitle = c.FeedTitle,
                            FeedDomainName = c.Clone_Domain.Title,
                            FeedActive = c.FeedActive,
                            FeedSource = c.FeedSource
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
        public Clone_Feed getById(int ID)
        {
            var query = from c in QNewsDB.Clone_Feed where c.FeedID == ID select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Lấy về danh sách qua mảng id
        /// </summary>
        /// <param name="LtsArrID">Mảng ID</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<Clone_Feed> getListByArrID(List<int> LtsArrID)
        {
            var query = from c in QNewsDB.Clone_Feed where LtsArrID.Contains(c.FeedID) select c;
            return query.ToList();
        }

        /// <summary>
        /// Kiểm tra bản ghi đã tồn tại hay chưa
        /// </summary>
        /// <param name="clone_noderemove">Đối tượng kiểm tra</param>
        /// <returns>Trạng thái tồn tại</returns>
        public bool checkExits(Clone_Feed clone_noderemove)
        {
            var query = from c in QNewsDB.Clone_Feed where ((c.FeedTitle == clone_noderemove.FeedTitle) && (c.FeedID != clone_noderemove.FeedID)) select c;
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
        public List<FeedItem> getByNameArray(string NewsTitles)
        {
            List<string> ltsName = new List<string>();
            if (NewsTitles.Contains(';'))
                ltsName = NewsTitles.Split(';').Select(o => o.Trim()).ToList();
            else
                ltsName.Add(NewsTitles.Trim());

            var query = from c in QNewsDB.Clone_Feed
                        where ltsName.Contains(c.FeedTitle.Trim())
                        select new FeedItem()
                        {
                            FeedID = c.FeedID,
                            FeedTitle = c.FeedTitle
                        };
            return query.ToList();
        }

        /// <summary>
        /// Lấy về bản ghi qua tên
        /// </summary>
        /// <param name="Title">Tên bản ghi</param>
        /// <returns>Bản ghi</returns>
        public Clone_Feed getByName(string Title)
        {
            var query = from c in QNewsDB.Clone_Feed where ((c.FeedTitle == Title)) select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="clone_noderemove">bản ghi cần thêm</param>
        public void Add(Clone_Feed clone_noderemove)
        {
            QNewsDB.Clone_Feed.Add(clone_noderemove);
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="clone_noderemove">Xóa bản ghi</param>
        public void Delete(Clone_Feed clone_noderemove)
        {
            QNewsDB.Clone_Feed.Remove(clone_noderemove);
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
