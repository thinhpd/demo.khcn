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
    public class MissionDA  : BaseDA
    {
        #region Constructer
        public MissionDA()
        {
        }

        public MissionDA(string pathPaging)
        {
            this.PathPaging = pathPaging;
        }

        public MissionDA(string pathPaging, string pathPagingExt)
        {
            this.PathPaging = pathPaging;
            this.PathPagingext = pathPagingExt;
        }
        #endregion

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<MissionItem> getAllListSimple()
        {
            var query = from c in QNewsDB.Missions
                        orderby c.MaNhiemVu
                        select new MissionItem()
                        {
                            ID = c.ID,
                            MaNhiemVu = c.MaNhiemVu,
                            TenNhiemVu = c.TenNhiemVu
                        };
            return query.ToList();
        }

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <param name="IsShow">Kiểm tra hiển thị</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<MissionItem> getListSimpleAll(int StatusID)
        {
            var query = from c in QNewsDB.Missions
                        where (c.StatusID == StatusID)
                        orderby c.MaNhiemVu
                        select new MissionItem()
                        {
                            ID = c.ID,
                            MaNhiemVu = c.MaNhiemVu,
                            TenNhiemVu = c.TenNhiemVu
                        };
            return query.ToList();
        }

        /// <summary>
        /// Lấy về dưới dạng Autocomplete
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="showLimit"></param>
        /// <returns></returns>
        public List<MissionItem> GetListSimpleByAutoComplete(string keyword, int showLimit)
        {
            var query = from c in QNewsDB.Missions
                        orderby c.MaNhiemVu
                        where c.MaNhiemVu.Contains(keyword) //autoComplete
                        select new MissionItem()
                        {
                            ID = c.ID,
                            MaNhiemVu = c.MaNhiemVu
                        };
            return query.Take(showLimit).ToList();
        }

        /// <summary>
        /// Lấy về dưới dạng Autocomplete
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="showLimit"></param>
        /// <returns></returns>
        public List<MissionItem> GetListSimpleByAutoComplete(string keyword, int showLimit, int StatusID)
        {
            var query = from c in QNewsDB.Missions
                        orderby c.MaNhiemVu
                        where c.StatusID == StatusID
                        && c.MaNhiemVu.Contains(keyword) //autoComplete
                        select new MissionItem()
                        {
                            ID = c.ID,
                            MaNhiemVu = c.MaNhiemVu
                        };
            return query.Take(showLimit).ToList();
        }

        /// <summary>
        /// Lấy về kiểu đơn giản, phân trang
        /// </summary>
        /// <param name="pageSize">Số bản ghi trên trang</param>
        /// <param name="Page">Trang hiển thị</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<MissionItem> getListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);

            var query_first = from c in QNewsDB.Missions where !c.IsRemoved select c;
           

            if (Request.StatusID > 0)
                query_first = query_first.Where(o => o.StatusID == Request.StatusID);

            if (Request.TuNgay.HasValue)
                query_first = query_first.Where(o => o.BatDau >= Request.TuNgay);

            if (Request.DenNgay.HasValue)
                query_first = query_first.Where(o => o.KetThuc <= Request.DenNgay);

            if(!string.IsNullOrEmpty(Request.CreateBy))
                query_first = query_first.Where(o => o.CreateBy == Request.CreateBy);

            if (!string.IsNullOrEmpty(Request.ModifyBy))
                query_first = query_first.Where(o => o.ModifyBy == Request.ModifyBy);

            var query = query_first.Select(o => new
                        {
                            ID = o.ID,
                            MaNhiemVu = o.MaNhiemVu,
                            CreateBy = o.AspNetUser_CreateBy.UserName,
                            CreateDate = o.CreateDate,
                            TenNhiemVu = o.TenNhiemVu,
                            ModifyBy = o.AspNetUser_ModifyBy.UserName,
                            ModifyDate = o.ModifyDate,
                            BatDau = o.BatDau,
                            KetThuc = o.KetThuc,
                            StatusID = o.StatusID,
                            Status = o.Status.Name,
                            ToChucChuTri =o.ToChucChuTri,
                            ChuNhiemNhiemVu = o.ChuNhiemNhiemVu
                          
                        });
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList().Select(o => new MissionItem()
            {
                ID = o.ID,
                MaNhiemVu = o.MaNhiemVu,
                CreateBy = o.CreateBy,
                CreateDate = o.CreateDate,
                TenNhiemVu = o.TenNhiemVu,
                ModifyBy = o.ModifyBy,
                ModifyDate = o.ModifyDate,
                BatDau = o.BatDau,
                KetThuc = o.KetThuc,
                StatusID = o.StatusID,
                Status = o.Status
            }).ToList();
        }

        /// <summary>
        /// Lấy về mảng đơn giản qua mảng ID
        /// </summary>
        /// <param name="LtsArrID"></param>
        /// <returns></returns>
        public List<MissionItem> getListSimpleByArrID(List<int> LtsArrID)
        {
            var query = from o in QNewsDB.Missions
                        where LtsArrID.Contains(o.ID)
                        orderby o.ID descending
                        select new MissionItem()
                        {
                            ID = o.ID,
                            MaNhiemVu = o.MaNhiemVu,
                            CreateBy = o.AspNetUser_CreateBy.UserName,
                            CreateDate = o.CreateDate,
                            TenNhiemVu = o.TenNhiemVu,
                            ModifyBy = o.AspNetUser_ModifyBy.UserName,
                            ModifyDate = o.ModifyDate,
                            BatDau = o.BatDau,
                            KetThuc = o.KetThuc,
                            StatusID = o.StatusID,
                            Status = o.Status.Name,
                            ToChucChuTri = o.ToChucChuTri,
                            ChuNhiemNhiemVu = o.ChuNhiemNhiemVu
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
        public Mission getById(int ID)
        {
            var query = from c in QNewsDB.Missions where c.ID == ID select c;
            return query.FirstOrDefault();
        }

    
        public Base.Version getVersionById(int ID)
        {
            var query = from c in QNewsDB.Versions where c.ID == ID select c;
            return query.FirstOrDefault();
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
        public List<Mission> getListByArrID(List<int> LtsArrID)
        {
            var query = from c in QNewsDB.Missions where LtsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        /// <summary>
        /// Kiểm tra bản ghi đã tồn tại hay chưa
        /// </summary>
        /// <param name="Mission">Đối tượng kiểm tra</param>
        /// <returns>Trạng thái tồn tại</returns>
        public bool checkExits(Mission Mission)
        {
            var query = from c in QNewsDB.Missions where ((c.MaNhiemVu == Mission.MaNhiemVu) && (c.ID != Mission.ID)) select c;
            if (query.Count() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Lấy về bản ghi qua tên
        /// </summary>
        /// <param name="QuestionMaNhiemVus">Tên bản ghi</param>
        /// <returns>Bản ghi</returns>
        public List<MissionItem> getByNameArray(string MaNhiemVus)
        {
            List<string> ltsName = new List<string>();
            if (MaNhiemVus.Contains(';'))
                ltsName = MaNhiemVus.Split(';').Select(o => o.Trim()).ToList();
            else
                ltsName.Add(MaNhiemVus.Trim());

            var query = from c in QNewsDB.Missions
                        where ltsName.Contains(c.MaNhiemVu.Trim())
                        select new MissionItem()
                        {
                            ID = c.ID,
                            MaNhiemVu = c.MaNhiemVu
                        };
            return query.ToList();
        }

        /// <summary>
        /// Lấy về bản ghi qua tên
        /// </summary>
        /// <param name="MaNhiemVu">Tên bản ghi</param>
        /// <returns>Bản ghi</returns>
        public Mission getByName(string MaNhiemVu)
        {
            var query = from c in QNewsDB.Missions where ((c.MaNhiemVu == MaNhiemVu)) select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="Mission">bản ghi cần thêm</param>
        public void Add(Mission Mission)
        {
            QNewsDB.Missions.Add(Mission);
        }

        public void AddVersion(Base.Version version)
        {
            QNewsDB.Versions.Add(version);
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="Mission">Xóa bản ghi</param>
        public void Delete(Mission Mission)
        {
            Mission.IsRemoved = true;

            //Mission.Categories.Clear();
            //Mission.Events.Clear();
            //QNewsDB.Urls.Remove(Mission.Urls.FirstOrDefault());
            //QNewsDB.Missions.Remove(Mission);
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
