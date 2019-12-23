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
    public class DocumentDA  : BaseDA
    {
        #region Constructer
        public DocumentDA()
        {
        }

        public DocumentDA(string pathPaging)
        {
            this.PathPaging = pathPaging;
        }

        public DocumentDA(string pathPaging, string pathPagingExt)
        {
            this.PathPaging = pathPaging;
            this.PathPagingext = pathPagingExt;
        }
        #endregion

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<DocumentItem> getAllListSimple()
        {
            var query = from c in QNewsDB.Documents
                        orderby c.SoKyHieu
                        select new DocumentItem()
                        {
                            ID = c.ID,
                            SoKyHieu = c.SoKyHieu
                        };
            return query.ToList();
        }

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <param name="IsShow">Kiểm tra hiển thị</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<DocumentItem> getListSimpleAll(int StatusID)
        {
            var query = from c in QNewsDB.Documents
                        where (c.StatusID == StatusID)
                        orderby c.SoKyHieu
                        select new DocumentItem()
                        {
                            ID = c.ID,
                            SoKyHieu = c.SoKyHieu
                        };
            return query.ToList();
        }

        /// <summary>
        /// Lấy về dưới dạng Autocomplete
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="showLimit"></param>
        /// <returns></returns>
        public List<DocumentItem> GetListSimpleByAutoComplete(string keyword, int showLimit)
        {
            var query = from c in QNewsDB.Documents
                        orderby c.SoKyHieu
                        where c.SoKyHieu.Contains(keyword) //autoComplete
                        select new DocumentItem()
                        {
                            ID = c.ID,
                            SoKyHieu = c.SoKyHieu
                        };
            return query.Take(showLimit).ToList();
        }

        /// <summary>
        /// Lấy về dưới dạng Autocomplete
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="showLimit"></param>
        /// <returns></returns>
        public List<DocumentItem> GetListSimpleByAutoComplete(string keyword, int showLimit, int StatusID)
        {
            var query = from c in QNewsDB.Documents
                        orderby c.SoKyHieu
                        where c.StatusID == StatusID
                        && c.SoKyHieu.Contains(keyword) //autoComplete
                        select new DocumentItem()
                        {
                            ID = c.ID,
                            SoKyHieu = c.SoKyHieu
                        };
            return query.Take(showLimit).ToList();
        }

        /// <summary>
        /// Lấy về kiểu đơn giản, phân trang
        /// </summary>
        /// <param name="pageSize">Số bản ghi trên trang</param>
        /// <param name="Page">Trang hiển thị</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<DocumentItem> getListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);

            var query_first = from c in QNewsDB.Documents where !c.IsRemoved select c;
            if (Request.CategoryID > 0)
                query_first = query_first.Where(o => o.LoaiVanBanID == Request.CategoryID);

            if (Request.EventID > 0)
                query_first = query_first.Where(o => o.DocumentScopes.Any(c => c.ID == Request.EventID));

            if (Request.StatusID > 0)
                query_first = query_first.Where(o => o.StatusID == Request.StatusID);

            if (Request.TuNgay.HasValue)
                query_first = query_first.Where(o => o.NgayBanHanh >= Request.TuNgay);

            if (Request.DenNgay.HasValue)
                query_first = query_first.Where(o => o.NgayBanHanh <= Request.DenNgay);

            if(!string.IsNullOrEmpty(Request.CreateBy))
                query_first = query_first.Where(o => o.CreateBy == Request.CreateBy);

            if (!string.IsNullOrEmpty(Request.ModifyBy))
                query_first = query_first.Where(o => o.ModifyBy == Request.ModifyBy);

            var query = query_first.Select(o => new
                        {
                            ID = o.ID,
                            SoKyHieu = o.SoKyHieu,
                            CreateBy = o.AspNetUser_CreateBy.UserName,
                            CreateDate = o.CreateDate,
                            TrichYeu = o.TrichYeu,
                            ModifyBy = o.AspNetUser_ModifyBy.UserName,
                            ModifyDate = o.ModifyDate,
                            NgayBanHanh = o.NgayBanHanh,
                            NgayHieuLuc = o.NgayHieuLuc,
                            LoaiVanBan = o.DocumentType.Title,
                            LinhVuc = o.DocumentScopes.Select(c => c.Title),
                            StatusID = o.StatusID,
                            Status = o.Status.Name
                          
                        });
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList().Select(o => new DocumentItem()
            {
                ID = o.ID,
                SoKyHieu = o.SoKyHieu,
                CreateBy = o.CreateBy,
                CreateDate = o.CreateDate,
                TrichYeu = o.TrichYeu,
                ModifyBy = o.ModifyBy,
                ModifyDate = o.ModifyDate,
                NgayBanHanh = o.NgayBanHanh,
                NgayHieuLuc = o.NgayHieuLuc,
                LoaiVanBan = o.LoaiVanBan,
                LinhVuc = o.LinhVuc.ToList(),
                StatusID = o.StatusID,
                Status = o.Status
            }).ToList();
        }

        /// <summary>
        /// Lấy về mảng đơn giản qua mảng ID
        /// </summary>
        /// <param name="LtsArrID"></param>
        /// <returns></returns>
        public List<DocumentItem> getListSimpleByArrID(List<int> LtsArrID)
        {
            var query = from o in QNewsDB.Documents
                        where LtsArrID.Contains(o.ID)
                        orderby o.ID descending
                        select new DocumentItem()
                        {
                            ID = o.ID,
                            SoKyHieu = o.SoKyHieu,
                            CreateBy = o.AspNetUser_CreateBy.UserName,
                            CreateDate = o.CreateDate,
                            TrichYeu = o.TrichYeu,
                            ModifyBy = o.AspNetUser_ModifyBy.UserName,
                            ModifyDate = o.ModifyDate,
                            NgayBanHanh = o.NgayBanHanh,
                            NgayHieuLuc = o.NgayHieuLuc,
                            LoaiVanBan = o.DocumentType.Title,
                            LinhVuc = o.DocumentScopes.Select(c => c.Title).ToList(),
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
        public Document getById(int ID)
        {
            var query = from c in QNewsDB.Documents where c.ID == ID select c;
            return query.FirstOrDefault();
        }

    
        public Base.Version getVersionById(int ID)
        {
            var query = from c in QNewsDB.Versions where c.ID == ID select c;
            return query.FirstOrDefault();
        }

        public List<DocumentType> getListCategoryByArrID(List<int> LtsArrID)
        {
            var query = from c in QNewsDB.DocumentTypes where LtsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        public List<DocumentIssue> getListIssueByArrID(List<int> LtsArrID)
        {
            var query = from c in QNewsDB.DocumentIssues where LtsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        public List<DocumentScope> getListEventByArrID(List<int> LtsArrID)
        {
            var query = from c in QNewsDB.DocumentScopes where LtsArrID.Contains(c.ID) select c;
            return query.ToList();
        }


        /// <summary>
        /// Lấy về danh sách qua mảng id
        /// </summary>
        /// <param name="LtsArrID">Mảng ID</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<Document> getListByArrID(List<int> LtsArrID)
        {
            var query = from c in QNewsDB.Documents where LtsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        /// <summary>
        /// Kiểm tra bản ghi đã tồn tại hay chưa
        /// </summary>
        /// <param name="Document">Đối tượng kiểm tra</param>
        /// <returns>Trạng thái tồn tại</returns>
        public bool checkExits(Document Document)
        {
            var query = from c in QNewsDB.Documents where ((c.SoKyHieu == Document.SoKyHieu) && (c.ID != Document.ID)) select c;
            if (query.Count() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Lấy về bản ghi qua tên
        /// </summary>
        /// <param name="QuestionSoKyHieus">Tên bản ghi</param>
        /// <returns>Bản ghi</returns>
        public List<DocumentItem> getByNameArray(string SoKyHieus)
        {
            List<string> ltsName = new List<string>();
            if (SoKyHieus.Contains(';'))
                ltsName = SoKyHieus.Split(';').Select(o => o.Trim()).ToList();
            else
                ltsName.Add(SoKyHieus.Trim());

            var query = from c in QNewsDB.Documents
                        where ltsName.Contains(c.SoKyHieu.Trim())
                        select new DocumentItem()
                        {
                            ID = c.ID,
                            SoKyHieu = c.SoKyHieu
                        };
            return query.ToList();
        }

        /// <summary>
        /// Lấy về bản ghi qua tên
        /// </summary>
        /// <param name="SoKyHieu">Tên bản ghi</param>
        /// <returns>Bản ghi</returns>
        public Document getByName(string SoKyHieu)
        {
            var query = from c in QNewsDB.Documents where ((c.SoKyHieu == SoKyHieu)) select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="Document">bản ghi cần thêm</param>
        public void Add(Document Document)
        {
            QNewsDB.Documents.Add(Document);
        }

        public void AddVersion(Base.Version version)
        {
            QNewsDB.Versions.Add(version);
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="Document">Xóa bản ghi</param>
        public void Delete(Document Document)
        {
            Document.IsRemoved = true;

            //Document.Categories.Clear();
            //Document.Events.Clear();
            //QNewsDB.Urls.Remove(Document.Urls.FirstOrDefault());
            //QNewsDB.Documents.Remove(Document);
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
