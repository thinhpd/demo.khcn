using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;
using QNews.Base;
using QNews.Data.Admin;
using QNews.Models;

namespace QNews.Data
{
    public partial class System_MemberShipDA : BaseDA
    {
        #region Constructer
        public System_MemberShipDA()
        {
        }

        public System_MemberShipDA(string pathPaging)
        {
            this.PathPaging = pathPaging;
        }

        public System_MemberShipDA(string pathPaging, string pathPagingExt)
        {
            this.PathPaging = pathPaging;
            this.PathPagingext = pathPagingExt;
        }
        #endregion



        /// <summary>
        /// Lấy về bản ghi qua tên
        /// </summary>
        /// <param name="QuestionTitles">Tên bản ghi</param>
        /// <returns>Bản ghi</returns>
        public List<KeyValue> getRoleByNameArray(string KeyTitle)
        {
            List<string> ltsName = new List<string>();
            if (KeyTitle.Contains(';'))
                ltsName = KeyTitle.Split(';').Select(o => o.Trim()).ToList();
            else
                ltsName.Add(KeyTitle.Trim());

            var query = from c in QNewsDB.AspNetRoles
                        where ltsName.Contains(c.Name.Trim())
                        select new KeyValue()
                        {
                            Key = c.Id,
                            Value = c.Name
                        };
            return query.ToList();
        }

        /// <summary>
        /// Lấy về dưới dạng Autocomplete
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="showLimit"></param>
        /// <returns></returns>
        public List<KeyValue> GetListRoleSimpleByAutoComplete(string keyword, int showLimit)
        {
            var query = from c in QNewsDB.AspNetRoles
                        orderby c.Id
                        select new KeyValue()
                        {
                           Key = c.Id,
                           Value = c.Name
                        };
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(o => o.Value.Contains(keyword));

            return query.Take(showLimit).ToList();
        }

        /// <summary>
        /// Lấy về dưới dạng Autocomplete
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="showLimit"></param>
        /// <returns></returns>
        public List<UserItem> GetListSimpleByAutoComplete(string keyword, int showLimit)
        {
            var query = from c in QNewsDB.AspNetUsers
                        orderby c.UserName
                        where c.UserName.Contains(keyword) //autoComplete
                        select new UserItem()
                        {
                           ID  = c.Id,
                           UserName = c.UserName
                        };
            return query.Take(showLimit).ToList();
        }

        /// <summary>
        /// Lấy về kiểu đơn giản, phân trang
        /// </summary>
        /// <param name="pageSize">Số bản ghi trên trang</param>
        /// <param name="Page">Trang hiển thị</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<UserItem> getListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in QNewsDB.AspNetUsers
                        select new 
                        {
                            Email = o.Email,
                            Id = o.Id,
                            Phone = o.PhoneNumber,
                            Roles = o.AspNetRoles.Select(r => r.Name),
                            Sex = o.Sex,
                            UserName = o.UserName,
                            UserFullName = o.UserFullName
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList().Select(o => new UserItem()
            {
                Email = o.Email,
                ID = o.Id,
                Phone = o.Phone,
                Roles = string.Join(",", o.Roles.ToList()),
                Sex = o.Sex,
                UserName = o.UserName,
                UserFullName = o.UserFullName
            }).ToList();
        }

        /// <summary>
        /// Lấy về mảng đơn giản qua mảng ID
        /// </summary>
        /// <param name="LtsArrID"></param>
        /// <returns></returns>
        public List<UserItem> getListSimpleByArrID(List<string> LtsArrID)
        {
            var query = from o in QNewsDB.AspNetUsers
                        where LtsArrID.Contains(o.Id)
                        orderby o.UserName descending
                        select new UserItem()
                        {
                            Email = o.Email,
                            ID = o.Id,
                            Phone = o.PhoneNumber,
                            Roles = string.Join(",", o.AspNetRoles.Select(r => r.Name).ToList()),
                            Sex = o.Sex,
                            UserName = o.UserName
                        };
            TotalRecord = query.Count();
            return query.ToList();
        }

        #region Check Exits, Add, Update, Delete
        /// <summary>
        /// Lấy về bản ghi qua khóa chính
        /// </summary>
        /// <param name="NewsID">ID bản ghi</param>
        /// <returns>Bản ghi</returns>
        public AspNetUser getById(string UserId)
        {
            var query = from c in QNewsDB.AspNetUsers where c.Id == UserId select c;
            return query.FirstOrDefault();
        }


        /// <summary>
        /// Kiểm tra bản ghi đã tồn tại hay chưa
        /// </summary>
        /// <param name="News_News">Đối tượng kiểm tra</param>
        /// <returns>Trạng thái tồn tại</returns>
        public bool checkExits(AspNetUser AspNetUser)
        {
            var query = from c in QNewsDB.AspNetUsers where ((c.UserName == AspNetUser.UserName)) select c;
            if (query.Count() > 0)
                return true;
            else
                return false;
        }


        /// <summary>
        /// Lấy về bản ghi qua tên
        /// </summary>
        /// <param name="NewsTitle">Tên bản ghi</param>
        /// <returns>Bản ghi</returns>
        public AspNetUser getByUserName(string userName)
        {
            var query = from c in QNewsDB.AspNetUsers where ((c.UserName == userName)) select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="News_News">bản ghi cần thêm</param>
        public void Add(AspNetUser aspNetUser)
        {
            QNewsDB.AspNetUsers.Add(aspNetUser);
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="News_News">Xóa bản ghi</param>
        public void Delete(AspNetUser aspNetUser)
        {
            aspNetUser.AspNetRoles.Clear();
            aspNetUser.AspNetUserClaims.Clear();
            aspNetUser.AspNetUserLogins.Clear();
            QNewsDB.AspNetUsers.Remove(aspNetUser);
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
