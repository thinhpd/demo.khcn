using QNews.Base;
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
using QNews.Admin.Controllers;
using Microsoft.Security.Application;

namespace QNews.Admin.Controllers
{
    public class EventController : BaseController
    {
        EventDA _eventDA = new EventDA("#");

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
                        Message = "Bạn phải nhập tên sự kiện"
                    };
                }
                else
                {
                    var _event = _eventDA.getByName(SearchValue);
                    if (_event == null)
                    {
                        msg = new JsonMessage()
                        {
                            Erros = true,
                            Message = "sự kiện không tồn tại trong hệ thống"
                        };
                    }
                    else
                    {
                        msg = new JsonMessage()
                        {
                            Erros = false,
                            ID = _event.ID.ToString(),
                            Message = _event.Title
                        };
                    }
                }
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else //Nếu xem từ khóa
            {
                string query = Request["query"];
                var ltsResults = _eventDA.GetListSimpleByAutoComplete(query, 10);
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
            var catDA = new Data.Admin.CategoryDA();
            ViewBag.AllStatus = catDA.getAllStatusSimple();
            ViewBag.AllUser = catDA.getAllUserSimple();
            return View();
        }

        /// <summary>
        /// Load danh sách bản ghi dưới dạng bảng
        /// </summary>
        /// <returns></returns>
        public ActionResult ListItems()
        {
            ViewData.Model = _eventDA.getListSimpleByRequest(Request);
            ViewBag.PageHtml = _eventDA.GridHtmlPage;
            return View();
        }


        /// <summary>
        /// Trang xem chi tiết trong model
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxView()
        {
            var eventModel = _eventDA.getById(ArrID.FirstOrDefault());
            ViewData.Model = eventModel;
            return View();
        }

        /// <summary>
        /// Form dùng cho thêm mới, sửa. Load bằng Ajax dialog
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxForm()
        {
            var eventModel = new Base.Event()
            {
                AllowComment = true,
                Description = string.Empty,
                Details = string.Empty
            };

            if (DoAction == ActionType.Edit)
                eventModel = _eventDA.getById(ArrID.FirstOrDefault());

            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(eventModel);
        }


        public ActionResult AjaxApprove()
        {
            var eventDA = new Data.Admin.EventDA();
            ViewBag.AllStatus = eventDA.getAllStatusSimple();
            var Model = eventDA.getById(ArrID.FirstOrDefault());
            return View(Model);
        }

        /// <summary>
        /// Hứng các giá trị, phục vụ cho thêm, sửa, xóa, ẩn, hiện
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken()]
        public ActionResult Actions()
        {
            JsonMessage msg = new JsonMessage();
            Event _event = new Event();
            List<Event> lts_eventItems;
            StringBuilder stbMessage;
            try
            {
                switch (DoAction)
                {
                    case ActionType.Add:
                        if (hasAdd)
                        {
                            UpdateModel(_event);

                            _event.CreateDate = DateTime.Now;
                            _event.ModifyDate = DateTime.Now;
                            _event.CreateBy = User.Identity.GetUserId();
                            _event.ModifyBy = User.Identity.GetUserId();
                            _event.StatusID = (int)Utils.WorkFlowStatus.BAN_NHAP;



                            _event.Description = Sanitizer.GetSafeHtmlFragment(_event.Description);
                            _event.Details = Sanitizer.GetSafeHtmlFragment(_event.Details);

                            #region cập nhật url
                            Base.Url url = new Base.Url();
                            url.UrlID = Request["Url"];
                            _event.Urls.Add(url);
                            #endregion


                            _eventDA.Add(_event);
                            _eventDA.Save();
                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = _event.ID.ToString(),
                                Message = string.Format("Đã thêm mới sự kiện: <b>{0}</b>", Server.HtmlEncode(_event.Title))
                            };
                        }
                        break;

                    case ActionType.Edit:
                        if (hasEdit)
                        {
                            _event = _eventDA.getById(ArrID.FirstOrDefault());
                            UpdateModel(_event);

                            _event.Description = Sanitizer.GetSafeHtmlFragment(_event.Description);
                            _event.Details = Sanitizer.GetSafeHtmlFragment(_event.Details);


                            _event.ModifyDate = DateTime.Now;
                            _event.ModifyBy = User.Identity.GetUserId();
                            _event.StatusID = (int)Utils.WorkFlowStatus.CHO_DUYET;
                            _eventDA.Save();
                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = _event.ID.ToString(),
                                Message = string.Format("Đã cập nhật sự kiện: <b>{0}</b>", Server.HtmlEncode(_event.Title))
                            };
                        }
                        break;

                    case ActionType.Delete:
                        if (hasDelete)
                        {
                            lts_eventItems = _eventDA.getListByArrID(ArrID);
                            stbMessage = new StringBuilder();
                            foreach (var item in lts_eventItems)
                            {

                                _eventDA.Delete(item);
                                stbMessage.AppendFormat("Đã xóa sự kiện <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                            }
                            msg.ID = string.Join(",", ArrID);
                            _eventDA.Save();
                            msg.Message = stbMessage.ToString();
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