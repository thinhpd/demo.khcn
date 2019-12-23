using QNews.Data.Admin;
using QNews.Models;
using QNews.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Security.Application;

namespace QNews.Admin.Controllers
{
    public class SiteLinkController : BaseController
    {
        SiteLinkDA _SiteLinkDA = new SiteLinkDA("#");
        
        public ActionResult Details(int id)
        {
            SiteLinkDA siteLinkDA = new SiteLinkDA();
            var model = siteLinkDA.getById(id);
            ViewBag.AllStatus = siteLinkDA.getAllStatusSimple();
            ViewBag.AllUser = siteLinkDA.getAllUserSimple();
            return View(model);
        }


        public ActionResult AutoComplete()
        {
            if (DoAction == ActionType.Add) //Nếu thêm mới
            {
                JsonMessage msg = new JsonMessage();
                string SearchValue = Request["Values"];
                if (string.IsNullOrEmpty(SearchValue))
                {
                    msg = new JsonMessage()
                    {
                        Erros = true,
                        Message = "Bạn phải nhập tên nội dung"
                    };
                }
                else
                {
                    var _SiteLink = _SiteLinkDA.getByName(SearchValue);
                    if (_SiteLink == null)
                    {
                        msg = new JsonMessage()
                        {
                            Erros = true,
                            Message = "nội dung không tồn tại trong hệ thống"
                        };
                    }
                    else
                    {
                        msg = new JsonMessage()
                        {
                            Erros = false,
                            ID = _SiteLink.ID.ToString(),
                            Message = _SiteLink.Title
                        };
                    }
                }
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else //Nếu xem từ khóa
            {
                string query = Request["query"];
                var ltsResults = _SiteLinkDA.GetListSimpleByAutoComplete(query, 10);
                AutoCompleteItem ResulValues = new AutoCompleteItem()
                {
                    query = query,
                    data = ltsResults.Select(o => o.ID.ToString()).ToList(),
                    suggestions = ltsResults.Select(o => o.Title).ToList()
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
            SiteLinkDA siteLinkTypeDA = new SiteLinkDA();
            ViewBag.AllStatus = siteLinkTypeDA.getAllStatusSimple();
            ViewBag.AllUser = siteLinkTypeDA.getAllUserSimple();
            return View();
        }

        /// <summary>
        /// Load danh sách bản ghi dưới dạng bảng
        /// </summary>
        /// <returns></returns>
        public ActionResult ListItems()
        {
            ViewData.Model = _SiteLinkDA.getListSimpleByRequest(Request);
            ViewBag.PageHtml = _SiteLinkDA.GridHtmlPage;
            return View();
        }


        /// <summary>
        /// Trang xem chi tiết trong model
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxView()
        {
            var SiteLinkModel = _SiteLinkDA.getById(ArrID.FirstOrDefault());
            ViewData.Model = SiteLinkModel;
            return View();
        }

        /// <summary>
        /// Form dùng cho thêm mới, sửa. Load bằng Ajax dialog
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxForm()
        {
            var SiteLinkModel = new Base.SiteLink()
            {
                Description = string.Empty
            };

            if (DoAction == ActionType.Edit)
                SiteLinkModel = _SiteLinkDA.getById(ArrID.FirstOrDefault());


            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(SiteLinkModel);
        }

        public ActionResult AjaxApprove()
        {
            var contentDA = new Data.Admin.SiteLinkDA();
            ViewBag.AllStatus = contentDA.getAllStatusSimple();
            var Model = contentDA.getById(ArrID.FirstOrDefault());
            return View(Model);
        }


        /// <summary>
        /// Hứng các giá trị, phục vụ cho thêm, sửa, xóa, ẩn, hiện
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken()]public ActionResult Actions()
        {
            JsonMessage msg = new JsonMessage();
            Base.SiteLink _SiteLink = new Base.SiteLink();
            List<Base.SiteLink> lts_SiteLinkItems;
            StringBuilder stbMessage;


            List<int> IDValues;

            try { 
            switch (DoAction)
            {

                case ActionType.Add:
                    if (hasAdd)
                    {
                        UpdateModel(_SiteLink);

                        _SiteLink.Description = Sanitizer.GetSafeHtmlFragment(_SiteLink.Description);
                        _SiteLink.CreateDate = DateTime.Now;
                        _SiteLink.ModifyDate = DateTime.Now;
                        _SiteLink.CreateBy = User.Identity.GetUserId();
                        _SiteLink.ModifyBy = User.Identity.GetUserId();
                        _SiteLink.StatusID = (int)Utils.WorkFlowStatus.BAN_NHAP;


                        _SiteLinkDA.Add(_SiteLink);
                        _SiteLinkDA.Save();
                        msg = new JsonMessage()
                        {
                            Erros = false,
                            ID = _SiteLink.ID.ToString(),
                            Message = string.Format("Đã thêm mới nội dung: <b>{0}</b>", Server.HtmlEncode(_SiteLink.Title))
                        };
                    }
                    break;

                case ActionType.Edit:
                    if (hasEdit)
                    {
                        _SiteLink = _SiteLinkDA.getById(ArrID.FirstOrDefault());

                        UpdateModel(_SiteLink);

                        _SiteLink.Description = Sanitizer.GetSafeHtmlFragment(_SiteLink.Description);
                        _SiteLink.ModifyDate = DateTime.Now;
                        _SiteLink.ModifyBy = User.Identity.GetUserId();
                        _SiteLink.StatusID = (int)Utils.WorkFlowStatus.CHO_DUYET;


                        _SiteLinkDA.Save();
                        msg = new JsonMessage()
                        {
                            Erros = false,
                            ID = _SiteLink.ID.ToString(),
                            Message = string.Format("Đã cập nhật nội dung: <b>{0}</b>", Server.HtmlEncode(_SiteLink.Title))
                        };
                    }
                    break;

                case ActionType.Delete:
                    if (hasDelete)
                    {
                        lts_SiteLinkItems = _SiteLinkDA.getListByArrID(ArrID);
                        stbMessage = new StringBuilder();
                        foreach (var item in lts_SiteLinkItems)
                        {

                            _SiteLinkDA.Delete(item);
                            stbMessage.AppendFormat("Đã xóa nội dung <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                        }
                        msg.ID = string.Join(",", ArrID);
                        _SiteLinkDA.Save();
                        msg.Message = stbMessage.ToString();
                    }
                    break;


                case ActionType.Approve:
                    if (hasApprove)
                    {
                        _SiteLink = _SiteLinkDA.getById(ArrID.FirstOrDefault());
                        Base.ApproveLog log = new Base.ApproveLog();
                        log.StatusID = Convert.ToInt32(Request["StatusID"]);
                        log.Description = Request["Description"];
                        log.CreatedDate = DateTime.Now;
                        log.UserID = User.Identity.GetUserId();
                        log.SiteLinkID = _SiteLink.ID;

                        _SiteLink.StatusID = log.StatusID;
                        _SiteLinkDA.QNewsDB.ApproveLogs.Add(log);
                        _SiteLinkDA.Save();

                        var status = _SiteLinkDA.QNewsDB.Status.Where(o => o.ID == log.StatusID).FirstOrDefault().Name;

                        msg = new JsonMessage()
                        {
                            Erros = false,
                            ID = _SiteLink.ID.ToString(),
                            Message = string.Format("Đã thực hiện <strong>{1}</strong> nội dung <b>{0}</b>", _SiteLink.Title, status)
                        };
                    }
                    break;
            }

            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }
            }
            catch (Exception e)
            {
                msg.Erros = true;
                msg.Message = e.StackTrace;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

    }
}