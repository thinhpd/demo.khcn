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
    public class ContributorDA : BaseDA
    {
        #region Constructer
        public ContributorDA()
        {
        }

        public ContributorDA(string pathPaging)
        {
            this.PathPaging = pathPaging;
        }

        public ContributorDA(string pathPaging, string pathPagingExt)
        {
            this.PathPaging = pathPaging;
            this.PathPagingext = pathPagingExt;
        }
        #endregion

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<ScopeItem> getAllListSimple()
        {
            var query = from c in QNewsDB.Contributors
                        orderby c.Title
                        select new ScopeItem()
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
        public List<ScopeItem> GetListSimpleByAutoComplete(string keyword, int showLimit)
        {
            var query = from c in QNewsDB.Contributors
                        orderby c.Title
                        where c.Title.Contains(keyword) //autoComplete
                        select new ScopeItem()
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
        public List<ScopeItem> getListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);

            var query_first = from c in QNewsDB.Contributors select c;
    

            var query = query_first.Select(o => new
                        {
                            ID = o.ID,
                            Title = o.Title,
                            Description = o.Description
                        });
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList().Select(o => new ScopeItem()
            {
                ID = o.ID,
                Title = o.Title,
                Description = o.Description
            }).ToList();
        }

        /// <summary>
        /// Lấy về mảng đơn giản qua mảng ID
        /// </summary>
        /// <param name="LtsArrID"></param>
        /// <returns></returns>
        public List<ScopeItem> getListSimpleByArrID(List<int> LtsArrID)
        {
            var query = from o in QNewsDB.Contributors
                        where LtsArrID.Contains(o.ID)
                        orderby o.ID descending
                        select new ScopeItem()
                        {
                            ID = o.ID,
                            Title = o.Title
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
        public Contributor getById(int ID)
        {
            var query = from c in QNewsDB.Contributors where c.ID == ID select c;
            return query.FirstOrDefault();
        }


        /// <summary>
        /// Lấy về danh sách qua mảng id
        /// </summary>
        /// <param name="LtsArrID">Mảng ID</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<Contributor> getListByArrID(List<int> LtsArrID)
        {
            var query = from c in QNewsDB.Contributors where LtsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        /// <summary>
        /// Kiểm tra bản ghi đã tồn tại hay chưa
        /// </summary>
        /// <param name="Contributor">Đối tượng kiểm tra</param>
        /// <returns>Trạng thái tồn tại</returns>
        public bool checkExits(Contributor Contributor)
        {
            var query = from c in QNewsDB.Contributors where ((c.Title == Contributor.Title) && (c.ID != Contributor.ID)) select c;
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
        public List<ScopeItem> getByNameArray(string Titles)
        {
            List<string> ltsName = new List<string>();
            if (Titles.Contains(';'))
                ltsName = Titles.Split(';').Select(o => o.Trim()).ToList();
            else
                ltsName.Add(Titles.Trim());

            var query = from c in QNewsDB.Contributors
                        where ltsName.Contains(c.Title.Trim()) 
                        select new ScopeItem()
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
        public Contributor getByName(string Title)
        {
            var query = from c in QNewsDB.Contributors where ((c.Title == Title)) select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="Contributor">bản ghi cần thêm</param>
        public void Add(Contributor Contributor)
        {
            QNewsDB.Contributors.Add(Contributor);
        }

        public void AddVersion(Base.Version version)
        {
            QNewsDB.Versions.Add(version);
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="Contributor">Xóa bản ghi</param>
        public void Delete(Contributor Contributor)
        {
            //Contributor.IsRemoved = true;
            //Contributor.Categories.Clear();
            //Contributor.Events.Clear();
            //QNewsDB.Urls.Remove(Contributor.Urls.FirstOrDefault());
            QNewsDB.Contributors.Remove(Contributor);
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
