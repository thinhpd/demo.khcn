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
    public class DocumentIssueDA : BaseDA
    {
        #region Constructer
        public DocumentIssueDA()
        {
        }

        public DocumentIssueDA(string pathPaging)
        {
            this.PathPaging = pathPaging;
        }

        public DocumentIssueDA(string pathPaging, string pathPagingExt)
        {
            this.PathPaging = pathPaging;
            this.PathPagingext = pathPagingExt;
        }
        #endregion

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<IssueItem> getAllListSimple()
        {
            var query = from c in QNewsDB.DocumentIssues
                        where !c.IsRemoved
                        orderby c.Title
                        select new IssueItem()
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
        public List<IssueItem> getListSimpleAll(bool IsShow)
        {
            var query = from c in QNewsDB.DocumentIssues
                        where (c.Show == IsShow)
                        && !c.IsRemoved
                        orderby c.Title
                        select new IssueItem()
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
        public List<IssueItem> GetListSimpleByAutoComplete(string keyword, int showLimit)
        {
            var query = from c in QNewsDB.DocumentIssues
                        orderby c.Title
                        where c.Title.Contains(keyword) //autoComplete
                        && !c.IsRemoved
                        select new IssueItem()
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
        public List<IssueItem> GetListSimpleByAutoComplete(string keyword, int showLimit, bool IsShow)
        {
            var query = from c in QNewsDB.DocumentIssues
                        orderby c.Title
                        where c.Show == IsShow
                        && !c.IsRemoved
                        && c.Title.Contains(keyword) //autoComplete
                        select new IssueItem()
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
        public List<IssueItem> getListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);

            var query_first = from c in QNewsDB.DocumentIssues where !c.IsRemoved select c;
    

            var query = query_first.Select(o => new
                        {
                            ID = o.ID,
                            Title = o.Title,
                            Show = o.Show,
                            Order = o.Order,
                            CreateBy = o.AspNetUser_CreateBy.UserName,
                            CreateDate = o.CreateDate,
                            ModifyBy = o.AspNetUser_ModifyBy.UserName,
                            ModifyDate = o.ModifyDate,
                            Description = o.Description
                        });
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList().Select(o => new IssueItem()
            {
                ID = o.ID,
                Title = o.Title,
                Show = o.Show,
                Order = o.Order,
                CreateBy = o.CreateBy,
                CreateDate = o.CreateDate,
                ModifyBy = o.ModifyBy,
                ModifyDate = o.ModifyDate,
                Description = o.Description
            }).ToList();
        }

        /// <summary>
        /// Lấy về mảng đơn giản qua mảng ID
        /// </summary>
        /// <param name="LtsArrID"></param>
        /// <returns></returns>
        public List<IssueItem> getListSimpleByArrID(List<int> LtsArrID)
        {
            var query = from o in QNewsDB.DocumentIssues
                        where LtsArrID.Contains(o.ID)
                        && !o.IsRemoved
                        orderby o.ID descending
                        select new IssueItem()
                        {
                            ID = o.ID,
                            Title = o.Title,
                            Show = o.Show,
                            Order = o.Order
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
        public DocumentIssue getById(int ID)
        {
            var query = from c in QNewsDB.DocumentIssues where c.ID == ID && !c.IsRemoved select c;
            return query.FirstOrDefault();
        }


        /// <summary>
        /// Lấy về danh sách qua mảng id
        /// </summary>
        /// <param name="LtsArrID">Mảng ID</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<DocumentIssue> getListByArrID(List<int> LtsArrID)
        {
            var query = from c in QNewsDB.DocumentIssues where LtsArrID.Contains(c.ID) && !c.IsRemoved select c;
            return query.ToList();
        }

        /// <summary>
        /// Kiểm tra bản ghi đã tồn tại hay chưa
        /// </summary>
        /// <param name="DocumentIssue">Đối tượng kiểm tra</param>
        /// <returns>Trạng thái tồn tại</returns>
        public bool checkExits(DocumentIssue DocumentIssue)
        {
            var query = from c in QNewsDB.DocumentIssues where ((c.Title == DocumentIssue.Title) && (c.ID != DocumentIssue.ID) && !c.IsRemoved) select c;
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
        public List<IssueItem> getByNameArray(string Titles)
        {
            List<string> ltsName = new List<string>();
            if (Titles.Contains(';'))
                ltsName = Titles.Split(';').Select(o => o.Trim()).ToList();
            else
                ltsName.Add(Titles.Trim());

            var query = from c in QNewsDB.DocumentIssues
                        where ltsName.Contains(c.Title.Trim()) && !c.IsRemoved
                        select new IssueItem()
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
        public DocumentIssue getByName(string Title)
        {
            var query = from c in QNewsDB.DocumentIssues where ((c.Title == Title)) && !c.IsRemoved select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="DocumentIssue">bản ghi cần thêm</param>
        public void Add(DocumentIssue DocumentIssue)
        {
            QNewsDB.DocumentIssues.Add(DocumentIssue);
        }

        public void AddVersion(Base.Version version)
        {
            QNewsDB.Versions.Add(version);
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="DocumentIssue">Xóa bản ghi</param>
        public void Delete(DocumentIssue DocumentIssue)
        {
            DocumentIssue.IsRemoved = true;
            //DocumentIssue.Categories.Clear();
            //DocumentIssue.Events.Clear();
            //QNewsDB.Urls.Remove(DocumentIssue.Urls.FirstOrDefault());
            //QNewsDB.DocumentIssues.Remove(DocumentIssue);
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
