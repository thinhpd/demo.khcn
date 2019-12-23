using Ganss.XSS;
using Microsoft.Security.Application;
using QNews.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace QNews.Admin.Controllers
{

    public enum ActionType
    {
        View = 0,
        Add = 1,
        Edit = 2,
        Delete = 3,
        Show = 4,
        Hide = 5,
        Order = 6,
        Reset = 7,
        Permission = 8,
        Check = 9,
        Meta = 10,
        Approve = 11
    }

    [Authorize]
    [Authorize(Roles = "Admin,Approve,Delete,Edit,Add,View")]
    public class BaseController : Controller
    {
        protected  HtmlSanitizer sanitizer = new HtmlSanitizer();

        protected bool hasAdd = false;
        protected bool hasEdit = false;
        protected bool hasDelete = false;
        protected bool hasApprove = false;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            hasAdd = requestContext.HttpContext.User.IsInRole("Admin") || requestContext.HttpContext.User.IsInRole("Add");
            hasEdit = requestContext.HttpContext.User.IsInRole("Admin") || requestContext.HttpContext.User.IsInRole("Edit");
            hasDelete = requestContext.HttpContext.User.IsInRole("Admin") || requestContext.HttpContext.User.IsInRole("Delete");
            hasApprove = requestContext.HttpContext.User.IsInRole("Admin") || requestContext.HttpContext.User.IsInRole("Approve");
        }

        protected void MyUpdateModel<T>(T model) where T : class
        {
            UpdateModel(model, model.GetType().Name);
            var properties = model.GetType().GetProperties();
            foreach (var propertyInfo in properties)
            {
                string typeName = propertyInfo.PropertyType.Name.ToLower();
                if (typeName == "string")
                {
                    string safeValue = (string)propertyInfo.GetValue(model, null);
                    propertyInfo.SetValue(model, Sanitizer.GetSafeHtmlFragment(safeValue));
                }
            }
        }


        public ActionResult CheckUrl()
        {
            bool IsExits = false;
            string urlCheck = Request["url"];
            if (!string.IsNullOrEmpty(urlCheck))
            {
                using (Base.QNewsDBContext db = new Base.QNewsDBContext())
                {
                    IsExits = (from c in db.Urls where c.UrlID == urlCheck select c).Count() > 0;
                }
            }
            return Json(IsExits, JsonRequestBehavior.AllowGet);
        }

        public string ActionText
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["do"]))
                {
                    switch (Request["do"].Trim().ToLower())
                    {
                        default:
                        case "add":
                            return "Thêm mới";

                        case "delete":
                            return "Xóa";

                        case "edit":
                            return "Cập nhật";

                        case "show":
                            return "Hiển thị";

                        case "hide":
                            return "Ẩn";

                        case "order":
                            return "Sắp xếp";

                        case "reset":
                            return "Reset mật khẩu";

                        case "permissiom":
                            return "Phân quyền tài khoản";

                        case "sizecolorstock":
                            return "Màu sắc, kích thước & kho hàng";

                        case "approve":
                            return "Kiểm duyệt nội dung";

                        case "meta":
                            return "Thẻ Meta";

                    }
                }
                else
                    return "Xem";
            }
        }

        public ActionType DoAction
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["do"]))
                {
                    switch (Request["do"].Trim().ToLower())
                    {
                        default:
                        case "add":
                            return ActionType.Add;

                        case "delete":
                            return ActionType.Delete;

                        case "edit":
                            return ActionType.Edit;

                        case "show":
                            return ActionType.Show;

                        case "hide":
                            return ActionType.Hide;

                        case "order":
                            return ActionType.Order;

                        case "reset":
                            return ActionType.Reset;

                        case "permission":
                            return ActionType.Permission;

                        case "approve":
                            return ActionType.Approve;

                        case "meta":
                            return ActionType.Meta;
                    }
                }
                else
                    return ActionType.View;
            }
        }


        public List<string> ArrGuidID
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["guidID"]))
                {
                    if (Request["guidID"].Contains(","))
                    {
                        return Request["guidID"].Trim().Split(',').ToList();
                    }
                    else
                    {
                        var ltsTemp = new List<string>();
                        ltsTemp.Add(Request["guidID"].Trim());
                        return ltsTemp;
                    }

                }
                else
                    return new List<string>();
            }
        }

        public List<int> ArrID
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["itemID"]))
                {
                    if (Request["ItemID"].Contains(","))
                    {
                        return Request["ItemID"].Trim().Split(',').Select(o => Convert.ToInt32(o)).ToList();
                    }
                    else
                    {
                        var ltsTemp = new List<int>();
                        ltsTemp.Add(Convert.ToInt32(Request["ItemID"]));
                        return ltsTemp;
                    }

                }
                else
                    return new List<int>();
            }
        }

        public List<FileAttachForm> ListFileUpload
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["listValueFileAttach"]))
                {
                    string strListFileAttach = Request["listValueFileAttach"];
                    System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    List<FileAttachForm> ltsFileForm = oSerializer.Deserialize<List<Utils.FileAttachForm>>(strListFileAttach);
                    return ltsFileForm;
                }
                return new List<FileAttachForm>();
            }
        }

        public List<int> ListFileRemove
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["listValueFileAttachRemove"]))
                {
                    if (Request["listValueFileAttachRemove"].Contains(","))
                    {
                        return Request["listValueFileAttachRemove"].Trim().Split(',').Select(o => Convert.ToInt32(o)).ToList();
                    }
                    else
                    {
                        var ltsTemp = new List<int>();
                        ltsTemp.Add(Convert.ToInt32(Request["listValueFileAttachRemove"]));
                        return ltsTemp;
                    }

                }
                else
                    return new List<int>();
            }
        }
    }
}