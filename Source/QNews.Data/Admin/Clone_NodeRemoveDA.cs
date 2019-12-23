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
    public partial class Clone_RemoveDA : BaseDA
    {
        #region Constructer
        public Clone_RemoveDA()
        { 
        }

        public Clone_RemoveDA(string pathPaging)
        {
            this.PathPaging = pathPaging;
        }

        public Clone_RemoveDA(string pathPaging, string pathPagingExt)
        {
            this.PathPaging = pathPaging;
            this.PathPagingext = pathPagingExt;
        }
        #endregion

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<NodeRemoveItem> getListSimpleAll()
        {
            var query = from c in QNewsDB.Clone_Remove
                        orderby c.Title
                        select new NodeRemoveItem()
                        {
                            ID = c.ID,
                            Title = c.Title,
                            DomainName = c.Clone_Domain.Title,
                            RemoveIn = c.Clone_Type.Title
                        };
            return query.ToList();
        }

        /// <summary>
        /// Lấy về dưới dạng Autocomplete
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="showLimit"></param>
        /// <returns></returns>
        public List<NodeRemoveItem> GetListSimpleByAutoComplete(string keyword, int showLimit)
        {
            var query = from c in QNewsDB.Clone_Remove
                        orderby c.Title
                        where c.Title.Contains(keyword) //autoComplete
                        select new NodeRemoveItem()
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
        public List<NodeRemoveItem> getListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in QNewsDB.Clone_Remove select new NodeRemoveItem() {
                ID = o.ID,
                Title = o.Title,
                DomainName  = o.Clone_Domain.Title,
                XpathValue = o.XpathValue,
                RemoveIn = o.Clone_Type.Title
            };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        /// <summary>
        /// Lấy về mảng đơn giản qua mảng ID
        /// </summary>
        /// <param name="LtsArrID"></param>
        /// <returns></returns>
        public List<NodeRemoveItem> getListSimpleByArrID(List<int> LtsArrID)
        {
            var query = from o in QNewsDB.Clone_Remove
                        where LtsArrID.Contains(o.ID)
                        orderby o.ID descending
                        select new NodeRemoveItem()
                        {
                            ID = o.ID,
                            Title = o.Title,
                            DomainName = o.Clone_Domain.Title,
                            XpathValue = o.XpathValue
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
        public Clone_Remove getById(int ID)
        {
            var query = from c in QNewsDB.Clone_Remove where c.ID == ID select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Lấy về danh sách qua mảng id
        /// </summary>
        /// <param name="LtsArrID">Mảng ID</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<Clone_Remove> getListByArrID(List<int> LtsArrID)
        {
            var query = from c in QNewsDB.Clone_Remove where LtsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        /// <summary>
        /// Kiểm tra bản ghi đã tồn tại hay chưa
        /// </summary>
        /// <param name="clone_noderemove">Đối tượng kiểm tra</param>
        /// <returns>Trạng thái tồn tại</returns>
        public bool checkExits(Clone_Remove clone_noderemove)
        {
            var query = from c in QNewsDB.Clone_Remove where ((c.Title == clone_noderemove.Title) && (c.ID != clone_noderemove.ID)) select c;
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
        public List<NodeRemoveItem> getByNameArray(string NewsTitles)
        {
            List<string> ltsName = new List<string>();
            if (NewsTitles.Contains(';'))
                ltsName = NewsTitles.Split(';').Select(o => o.Trim()).ToList();
            else
                ltsName.Add(NewsTitles.Trim());

            var query = from c in QNewsDB.Clone_Remove
                        where ltsName.Contains(c.Title.Trim())
                        select new NodeRemoveItem()
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
        public Clone_Remove getByName(string Title)
        {
            var query = from c in QNewsDB.Clone_Remove where ((c.Title == Title)) select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="clone_noderemove">bản ghi cần thêm</param>
        public void Add(Clone_Remove clone_noderemove)
        {
            QNewsDB.Clone_Remove.Add(clone_noderemove);
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="clone_noderemove">Xóa bản ghi</param>
        public void Delete(Clone_Remove clone_noderemove)
        {
            QNewsDB.Clone_Remove.Remove(clone_noderemove);
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
