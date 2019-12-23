using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using QNews.Base;
using QNews.Data;
using QNews.Models;
using QNews.Utils;
using QNews.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace QNews.Admin.Controllers
{
    public class MemberShipController : BaseController
    {
        System_MemberShipDA MemberShipDA = new System_MemberShipDA("#");

        public ActionResult AutoComplete()
        {
            string query = Request["query"];
            var ltsResults = MemberShipDA.GetListSimpleByAutoComplete(query, 10);
            AutoCompleteItem ResulValues = new AutoCompleteItem()
            {
                query = query,
                data = ltsResults.Select(o => o.ID.ToString()).ToList(),
                suggestions = ltsResults.Select(o => o.UserName).ToList()
            };
            return Json(ResulValues, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AutoCompleteRole()
        {
            if (DoAction == ActionType.Add) //Nếu thêm mới
            {
                List<JsonMessage> LtsMsg = new List<JsonMessage>();
                JsonMessage msg;
                string SearchValue = Request["Values"];
                if (string.IsNullOrEmpty(SearchValue))
                {
                    msg = new JsonMessage()
                    {
                        Erros = true,
                        Message = "Bạn phải nhập vai trò, quyền"
                    };
                    LtsMsg.Add(msg);
                }
                else
                {
                    var LtsRole = MemberShipDA.getRoleByNameArray(SearchValue);
                    if (LtsRole == null || LtsRole.Count == 0)
                    {
                        msg = new JsonMessage()
                        {
                            Erros = true,
                            Message = "Quyền, vai trò không tồn tại"
                        };
                        LtsMsg.Add(msg);
                    }
                    else
                    {
                        foreach (var role in LtsRole)
                        {
                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = role.Key,
                                Message = role.Value
                            };
                            LtsMsg.Add(msg);
                        }
                    }
                }
                return Json(LtsMsg, JsonRequestBehavior.AllowGet);
            }
            else //Nếu xem từ khóa
            {
                string query = Request["query"];
                var ltsResults = MemberShipDA.GetListRoleSimpleByAutoComplete(query, 10);
                AutoCompleteItem ResulValues = new AutoCompleteItem()
                {
                    query = query,
                    data = ltsResults.Select(o => o.Key.ToString()).ToList(),
                    suggestions = ltsResults.Select(o => o.Value).ToList()
                };
                return Json(ResulValues, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// Trang chủ, index. Load ra grid dưới dạng ajax
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Load danh sách bản ghi dưới dạng bảng
        /// </summary>
        /// <returns></returns>
        public ActionResult ListItems()
        {
            ViewData.Model = MemberShipDA.getListSimpleByRequest(Request);
            ViewBag.PageHtml = MemberShipDA.GridHtmlPage;
            return View();
        }


        /// <summary>
        /// Trang xem chi tiết trong model
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxView()
        {
            var UserModel = MemberShipDA.getById(ArrGuidID.FirstOrDefault());
            ViewData.Model = UserModel;
            return View();
        }

        /// <summary>
        /// Form dùng cho thêm mới, sửa. Load bằng Ajax dialog
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxForm()
        {
            var UserModel = new Base.AspNetUser();

            if (DoAction == ActionType.Edit)
            {
                UserModel = MemberShipDA.getById(ArrGuidID.FirstOrDefault());
            }
            ViewData.Model = UserModel;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        public ActionResult AjaxReset()
        {
            

            var UserModel = MemberShipDA.getById(ArrGuidID.FirstOrDefault());
            ViewData.Model = UserModel;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        public ActionResult AjaxPermission()
        {
            var UserModel = MemberShipDA.getById(ArrGuidID.FirstOrDefault());
            ViewBag.ListAllRoles = MemberShipDA.QNewsDB.AspNetRoles.ToList();
            ViewData.Model = UserModel;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }


        public MemberShipController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public MemberShipController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }


        public ActionResult ResetPass()
        {
            var userId = "a258ea3a-7abf-4a25-8db4-a1ab29f9a785";
            var passwordNew = "123!@#a@";
            var user = MemberShipDA.getById(userId);
            var passwordWithSalt = Utils.StaticClass.MD5ByPHP(user.UserName) + passwordNew;
            UserManager.RemovePassword(userId);
            UserManager.AddPassword(userId, passwordWithSalt);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Hứng các giá trị, phục vụ cho thêm, sửa, xóa, ẩn, hiện
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken()]
        public ActionResult Actions(FormCollection collection)
        {
            JsonMessage msg = new JsonMessage();
            AspNetUser user = new AspNetUser();
            List<AspNetUser> ltsNewsItems;
            StringBuilder stbMessage;
            List<int> IDValues;
            string userId;
            bool hasAdmin = User.IsInRole("Admin");
            Regex regexPass = new Regex(@"^.*(?=.{7,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).*$");
            if (hasAdmin)
            {
                switch (DoAction)
                {
                    case ActionType.Add:

                        UpdateModel(user);

                        if (regexPass.IsMatch(user.PasswordHash))
                        {
                            var passwordWithSalt = Utils.StaticClass.MD5ByPHP(user.UserName) + user.PasswordHash;
                            var user_add = new ApplicationUser() { UserName = user.UserName, Email = user.Email, PhoneNumber = user.PhoneNumber, UserFullName = user.UserFullName };
                            IdentityResult result = UserManager.Create(user_add, passwordWithSalt);
                            if (result.Succeeded)
                            {
                                msg = new JsonMessage()
                                {
                                    Erros = false,
                                    ID = user.UserName,
                                    Message = string.Format("Đã thêm mới thành viên: <b>{0}</b>", Server.HtmlEncode(user.UserName))
                                };
                            }
                        }
                        else
                        {
                            msg = new JsonMessage()
                            {
                                Erros = true,
                                ID = string.Empty,
                                Message = "Mật khẩu phải ít nhất 7 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt"
                            };
                        }
                        break;

                    case ActionType.Permission:
                        userId = ArrGuidID.FirstOrDefault();
                        List<string> RoleValue = Utils.StaticClass.getStringValuesArray(collection["RoleValues"]);
                        user = MemberShipDA.getById(userId);
                        user.AspNetRoles.Clear();

                        var LtsRoleInDB = MemberShipDA.QNewsDB.AspNetRoles.Where(o => RoleValue.Contains(o.Id)).ToList();
                        foreach (var roleId in LtsRoleInDB)
                        {
                            user.AspNetRoles.Add(roleId);
                        }
                        MemberShipDA.Save();

                        msg = new JsonMessage()
                        {
                            Erros = false,
                            ID = user.Id.ToString(),
                            Message = string.Format("Đã phân quyền thành viên: <b>{0}</b>", Server.HtmlEncode(user.UserName))
                        };

                        break;

                    case ActionType.Reset:

                        string passwordNew = collection["Password"];
                        if (regexPass.IsMatch(passwordNew))
                        {

                            userId = ArrGuidID.FirstOrDefault();
                            user = MemberShipDA.getById(userId);
                            var passwordWithSalt = Utils.StaticClass.MD5ByPHP(user.UserName) + passwordNew;
                            UserManager.RemovePassword(userId);
                            UserManager.AddPassword(userId, passwordWithSalt);

                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = user.Id.ToString(),
                                Message = string.Format("Đã đổi mật khẩu thành viên: <b>{0}</b>", Server.HtmlEncode(user.UserName))
                            };
                        }
                        else
                        {
                            msg = new JsonMessage()
                            {
                                Erros = true,
                                ID = string.Empty,
                                Message = "Mật khẩu phải ít nhất 7 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt"
                            };
                        }
                        break;

                    case ActionType.Edit:

                        var user_edit = UserManager.FindById(ArrGuidID.FirstOrDefault());
                        if (user_edit != null)
                        {
                            UpdateModel(user);
                            user_edit.Email = user.Email;
                            user_edit.UserFullName = user.UserFullName;
                            user_edit.UserName = user.UserName;
                            UserManager.Update(user_edit);
                            if (!string.IsNullOrEmpty(user.PasswordHash))
                            {
                                UserManager.RemovePassword(user_edit.Id);
                                UserManager.AddPassword(user_edit.Id, user_edit.PasswordHash);
                            }
                        }
                        msg = new JsonMessage()
                        {
                            Erros = false,
                            ID = user.Id,
                            Message = string.Format("Đã cập nhật thành viên: <b>{0}</b>", Server.HtmlEncode(user.UserName))
                        };
                        break;


                    case ActionType.Delete:
                        bool hasDeleteUser = true;
                        using (Base.QNewsDBContext db = new QNewsDBContext())
                        {
                            var userDB = (from c in db.AspNetUsers where c.Id == ArrGuidID.FirstOrDefault() select c).FirstOrDefault();
                            if (userDB.Advertises_CreateBy.Count > 0 || userDB.Advertises_ModifyBy.Count > 0 || userDB.AlbumPictures_CreateBy.Count > 0 || userDB.AlbumPictures_ModifyBy.Count > 0 || userDB.Albums_CreateBy.Count > 0 || userDB.Albums_ModifyBy.Count > 0 || userDB.AlbumTopics_CreateBy.Count > 0 || userDB.AlbumTopics_ModifyBy.Count > 0 || userDB.ApproveLogs.Count > 0 || userDB.Audios_CreateBy.Count > 0 || userDB.Audios_ModifyBy.Count > 0 || userDB.AudioTopics_CreateBy.Count > 0 || userDB.AudioTopics_ModifyBy.Count > 0 || userDB.Contents_CreateBy.Count > 0 || userDB.Contents_ModifyBy.Count > 0 || userDB.DocumentScopes_CreateBy.Count > 0 || userDB.DocumentScopes_ModifyBy.Count > 0)
                            {
                                hasDeleteUser = false;
                            }
                        }
                        if (hasDeleteUser)
                        {
                            var user_delete = UserManager.FindById(ArrGuidID.FirstOrDefault());
                            string userNameDelete = user_delete.UserName;
                            string userIDDelete = user_delete.Id;

                            if (user_delete != null)
                            {
                                
                                UserManager.Delete(user_delete);
                            }
                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = userIDDelete,
                                Message = string.Format("Đã xóa thành viên: <b>{0}</b>", Server.HtmlEncode(userNameDelete))
                            };
                        }
                        else
                        {
                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = user.Id,
                                Message = string.Format("Tài khoản <b>{0}</b> đã đăng bài viết, nội dung không được phép xóa", Server.HtmlEncode(user.UserName))
                            };
                        }
                        break;

                }

                //case ActionType.Delete:
                //    ltsNewsItems = MemberShipDA.getListByArrID(ArrID);
                //    stbMessage = new StringBuilder();
                //    foreach (var item in ltsNewsItems)
                //    {
                //        MemberShipDA.Delete(item);
                //        stbMessage.AppendFormat("Đã xóa bài viết <b>{0}</b>.<br />", Server.HtmlEncode(item.NewsTitle));
                //    }
                //    msg.ID = string.Join(",", ArrID);
                //    MemberShipDA.Save();
                //    msg.Message = stbMessage.ToString();
                //    break;
            }

            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}