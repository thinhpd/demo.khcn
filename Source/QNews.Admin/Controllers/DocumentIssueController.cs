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
using Microsoft.Security.Application;

namespace QNews.Admin.Controllers
{
    public class DocumentIssueController : BaseController
    {
        DocumentIssueDA _documentTypeDA = new DocumentIssueDA("#");

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
                        Message = "Bạn phải nhập tên cơ quan ban hành văn bản"
                    };
                }
                else
                {
                    var _documentType = _documentTypeDA.getByName(SearchValue);
                    if (_documentType == null)
                    {
                        msg = new JsonMessage()
                        {
                            Erros = true,
                            Message = "cơ quan ban hành văn bản không tồn tại trong hệ thống"
                        };
                    }
                    else
                    {
                        msg = new JsonMessage()
                        {
                            Erros = false,
                            ID = _documentType.ID.ToString(),
                            Message = _documentType.Title
                        };
                    }
                }
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else //Nếu xem từ khóa
            {
                string query = Request["query"];
                var ltsResults = _documentTypeDA.GetListSimpleByAutoComplete(query, 10);
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
            ViewData.Model = _documentTypeDA.getListSimpleByRequest(Request);
            ViewBag.PageHtml = _documentTypeDA.GridHtmlPage;
            return View();
        }


        /// <summary>
        /// Trang xem chi tiết trong model
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxView()
        {
            var documentTypeModel = _documentTypeDA.getById(ArrID.FirstOrDefault());
            ViewData.Model = documentTypeModel;
            return View();
        }

        /// <summary>
        /// Form dùng cho thêm mới, sửa. Load bằng Ajax dialog
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxForm()
        {
            var documentTypeModel = new Base.DocumentIssue()
            {
                Order = 0,
                Show = true
            };

            if (DoAction == ActionType.Edit)
                documentTypeModel = _documentTypeDA.getById(ArrID.FirstOrDefault());

            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(documentTypeModel);
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
            DocumentIssue _documentType = new DocumentIssue();
            List<DocumentIssue> lts_documentTypeItems;
            StringBuilder stbMessage;

            try { 
            switch (DoAction)
            {
                case ActionType.Add:
                    if (hasAdd)
                    {
                        UpdateModel(_documentType);

                        _documentType.Description = Sanitizer.GetSafeHtmlFragment(_documentType.Description);
                        _documentType.CreateDate = DateTime.Now;
                        _documentType.ModifyDate = DateTime.Now;
                        _documentType.CreateBy = User.Identity.GetUserId();
                        _documentType.ModifyBy = User.Identity.GetUserId();

                        _documentTypeDA.Add(_documentType);
                        _documentTypeDA.Save();
                        msg = new JsonMessage()
                        {
                            Erros = false,
                            ID = _documentType.ID.ToString(),
                            Message = string.Format("Đã thêm mới cơ quan ban hành văn bản: <b>{0}</b>", Server.HtmlEncode(_documentType.Title))
                        };
                    }
                    break;

                case ActionType.Edit:
                    if (hasEdit)
                    {
                        _documentType = _documentTypeDA.getById(ArrID.FirstOrDefault());
                        UpdateModel(_documentType);

                        _documentType.Description = Sanitizer.GetSafeHtmlFragment(_documentType.Description);
                        _documentType.ModifyDate = DateTime.Now;
                        _documentType.ModifyBy = User.Identity.GetUserId();
                        _documentTypeDA.Save();
                        msg = new JsonMessage()
                        {
                            Erros = false,
                            ID = _documentType.ID.ToString(),
                            Message = string.Format("Đã cập nhật cơ quan ban hành văn bản: <b>{0}</b>", Server.HtmlEncode(_documentType.Title))
                        };
                    }
                    break;

                case ActionType.Delete:
                    if (hasDelete)
                    {
                        lts_documentTypeItems = _documentTypeDA.getListByArrID(ArrID);
                        stbMessage = new StringBuilder();
                        foreach (var item in lts_documentTypeItems)
                        {

                            _documentTypeDA.Delete(item);
                            stbMessage.AppendFormat("Đã xóa cơ quan ban hành văn bản <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                        }
                        msg.ID = string.Join(",", ArrID);
                        _documentTypeDA.Save();
                        msg.Message = stbMessage.ToString();
                    }
                    break;

                case ActionType.Show:
                    if (hasEdit)
                    {
                        lts_documentTypeItems = _documentTypeDA.getListByArrID(ArrID);
                        stbMessage = new StringBuilder();
                        foreach (var item in lts_documentTypeItems.Where(o => !o.Show))
                        {

                            item.Show = true;
                            _documentType.ModifyDate = DateTime.Now;
                            _documentType.ModifyBy = User.Identity.GetUserId();
                            stbMessage.AppendFormat("Đã hiển thị cơ quan ban hành văn bản <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                        }
                        msg.ID = string.Join(",", ArrID);
                        _documentTypeDA.Save();
                        msg.Message = stbMessage.ToString();
                    }
                    break;

                case ActionType.Hide:
                    if (hasEdit)
                    {
                        lts_documentTypeItems = _documentTypeDA.getListByArrID(ArrID);
                        stbMessage = new StringBuilder();
                        foreach (var item in lts_documentTypeItems.Where(o => o.Show))
                        {
                            item.Show = false;
                            _documentType.ModifyDate = DateTime.Now;
                            _documentType.ModifyBy = User.Identity.GetUserId();
                            stbMessage.AppendFormat("Đã ẩn cơ quan ban hành văn bản <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                        }
                        msg.ID = string.Join(",", ArrID);
                        _documentTypeDA.Save();
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