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
    public partial class Clone_DomainDA : BaseDA
    {
        #region Constructer
        public Clone_DomainDA()
        { 
        }

        public Clone_DomainDA(string pathPaging)
        {
            this.PathPaging = pathPaging;
        }

        public Clone_DomainDA(string pathPaging, string pathPagingExt)
        {
            this.PathPaging = pathPaging;
            this.PathPagingext = pathPagingExt;
        }
        #endregion

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<DomainItem> getListSimpleAll()
        {
            var query = from c in QNewsDB.Clone_Domain
                        orderby c.Title
                        select new DomainItem()
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
        public List<DomainItem> GetListSimpleByAutoComplete(string keyword, int showLimit)
        {
            var query = from c in QNewsDB.Clone_Domain
                        orderby c.Title
                        where c.Title.Contains(keyword) //autoComplete
                        select new DomainItem()
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
        public List<DomainItem> getListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in QNewsDB.Clone_Domain select new DomainItem() {
                ID = o.ID,
                Title = o.Title,
                DomainUrl  = o.DomainUrl,
                XpathTitle = o.XpathTitle
            };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        /// <summary>
        /// Lấy về mảng đơn giản qua mảng ID
        /// </summary>
        /// <param name="LtsArrID"></param>
        /// <returns></returns>
        public List<DomainItem> getListSimpleByArrID(List<int> LtsArrID)
        {
            var query = from o in QNewsDB.Clone_Domain
                        where LtsArrID.Contains(o.ID)
                        orderby o.ID descending
                        select new DomainItem()
                        {
                            ID = o.ID,
                            Title = o.Title,
                            DomainUrl = o.DomainUrl,
                            XpathTitle = o.XpathTitle
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
        public Clone_Domain getById(int ID)
        {
            var query = from c in QNewsDB.Clone_Domain where c.ID == ID select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Lấy về danh sách qua mảng id
        /// </summary>
        /// <param name="LtsArrID">Mảng ID</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<Clone_Domain> getListByArrID(List<int> LtsArrID)
        {
            var query = from c in QNewsDB.Clone_Domain where LtsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        /// <summary>
        /// Kiểm tra bản ghi đã tồn tại hay chưa
        /// </summary>
        /// <param name="clone_domain">Đối tượng kiểm tra</param>
        /// <returns>Trạng thái tồn tại</returns>
        public bool checkExits(Clone_Domain clone_domain)
        {
            var query = from c in QNewsDB.Clone_Domain where ((c.Title == clone_domain.Title) && (c.ID != clone_domain.ID)) select c;
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
        public List<DomainItem> getByNameArray(string NewsTitles)
        {
            List<string> ltsName = new List<string>();
            if (NewsTitles.Contains(';'))
                ltsName = NewsTitles.Split(';').Select(o => o.Trim()).ToList();
            else
                ltsName.Add(NewsTitles.Trim());

            var query = from c in QNewsDB.Clone_Domain
                        where ltsName.Contains(c.Title.Trim())
                        select new DomainItem()
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
        public Clone_Domain getByName(string Title)
        {
            var query = from c in QNewsDB.Clone_Domain where ((c.Title == Title)) select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="clone_domain">bản ghi cần thêm</param>
        public void Add(Clone_Domain clone_domain)
        {
            QNewsDB.Clone_Domain.Add(clone_domain);
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="clone_domain">Xóa bản ghi</param>
        public void Delete(Clone_Domain clone_domain)
        {
            clone_domain.Clone_Remove.Clear();
            clone_domain.Clone_Replace.Clear();
            QNewsDB.Clone_Domain.Remove(clone_domain);
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
