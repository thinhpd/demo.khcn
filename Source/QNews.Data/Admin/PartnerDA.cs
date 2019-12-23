using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using QNews.Base;

namespace QNews.Data.Admin
{
    public class PartnerDA: BaseDA
    {

        public PartnerDA()
        {
        }

        public PartnerDA(string pathPaging)
        {
            this.PathPaging = pathPaging;
        }

        public PartnerDA(string pathPaging, string pathPagingExt)
        {
            this.PathPaging = pathPaging;
            this.PathPagingext = pathPagingExt;
        }


        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<Partner> getAllListSimple()
        {
            var query = from c in QNewsDB.Partners
                        where !c.IsRemoved
                        orderby c.Title
                        select new Partner()
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
        public List<Partner> getListSimpleAll(bool IsShow)
        {
            var query = from c in QNewsDB.Partners
                        where (c.Show == IsShow)
                        && !c.IsRemoved
                        orderby c.Title
                        select new Partner()
                        {
                            ID = c.ID,
                            Title = c.Title
                        };
            return query.ToList();
        }
        public void update(Partner Partner)
        {
            //QNewsDB.TSHTs.Remove(TSHT);
            QNewsDB.Entry(Partner).State = System.Data.Entity.EntityState.Modified;
        }
        /// <summary>
        /// Lấy về dưới dạng Autocomplete
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="showLimit"></param>
        /// <returns></returns>
        public List<Partner> GetListSimpleByAutoComplete(string keyword, int showLimit)
        {
            var query = from c in QNewsDB.Partners
                        orderby c.Title
                        where c.Title.Contains(keyword) //autoComplete
                        && !c.IsRemoved
                        select new Partner()
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
        public List<Partner> GetListSimpleByAutoComplete(string keyword, int showLimit, bool IsShow)
        {
            var query = from c in QNewsDB.Partners
                        orderby c.Title
                        where c.Show == IsShow
                        && !c.IsRemoved
                        && c.Title.Contains(keyword) //autoComplete
                        select new Partner()
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
        public List<Partner> getListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);

            var query_first = from c in QNewsDB.Partners where !c.IsRemoved select c;


            var query = query_first.Select(o => new
            {
                ID = o.ID,
                Title = o.Title,
                Show = o.Show,
                Order = o.Order,
                Description = o.Description,
                Image = o.Image,
                Link=o.Link
            });
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList().Select(o => new Partner()
            {
                ID = o.ID,
                Title = o.Title,
                Show = o.Show,
                Order = o.Order,
                Image = o.Image,
                Description = o.Description,
                Link=o.Link
                
            }).ToList();
        }

        /// <summary>
        /// Lấy về mảng đơn giản qua mảng ID
        /// </summary>
        /// <param name="LtsArrID"></param>
        /// <returns></returns>
        public List<Partner> getListSimpleByArrID(List<int> LtsArrID)
        {
            var query = from o in QNewsDB.Partners
                        where LtsArrID.Contains(o.ID)
                        && !o.IsRemoved
                        orderby o.ID descending
                        select new Partner()
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
        public Partner getById(int ID)
        {
            var query = from c in QNewsDB.Partners where c.ID == ID && !c.IsRemoved select c;
            return query.FirstOrDefault();
        }


        /// <summary>
        /// Lấy về danh sách qua mảng id
        /// </summary>
        /// <param name="LtsArrID">Mảng ID</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<Partner> getListByArrID(List<int> LtsArrID)
        {
            var query = from c in QNewsDB.Partners where LtsArrID.Contains(c.ID) && !c.IsRemoved select c;
            return query.ToList();
        }

        /// <summary>
        /// Kiểm tra bản ghi đã tồn tại hay chưa
        /// </summary>
        /// <param name="Partner">Đối tượng kiểm tra</param>
        /// <returns>Trạng thái tồn tại</returns>
        public bool checkExits(Partner Partner)
        {
            var query = from c in QNewsDB.Partners where ((c.Title == Partner.Title) && (c.ID != Partner.ID) && !c.IsRemoved) select c;
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
        public List<Partner> getByNameArray(string Titles)
        {
            List<string> ltsName = new List<string>();
            if (Titles.Contains(';'))
                ltsName = Titles.Split(';').Select(o => o.Trim()).ToList();
            else
                ltsName.Add(Titles.Trim());

            var query = from c in QNewsDB.Partners
                        where ltsName.Contains(c.Title.Trim()) && !c.IsRemoved
                        select new Partner()
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
        public Partner getByName(string Title)
        {
            var query = from c in QNewsDB.Partners where ((c.Title == Title)) && !c.IsRemoved select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="Partner">bản ghi cần thêm</param>
        public void Add(Partner Partner)
        {
            QNewsDB.Partners.Add(Partner);
        }


        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="DocumentScope">Xóa bản ghi</param>
        public void Delete(Partner Partner)
        {
            Partner.IsRemoved = true;
            //DocumentScope.Categories.Clear();
            //DocumentScope.Events.Clear();
            //QNewsDB.Urls.Remove(DocumentScope.Urls.FirstOrDefault());
            //QNewsDB.Partners.Remove(DocumentScope);
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
