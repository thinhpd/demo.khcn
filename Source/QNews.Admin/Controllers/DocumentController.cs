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
    public class DocumentController : BaseController
    {
        DocumentDA _DocumentDA = new DocumentDA("#");

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
                    var _Document = _DocumentDA.getByName(SearchValue);
                    if (_Document == null)
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
                            ID = _Document.ID.ToString(),
                            Message = _Document.SoKyHieu
                        };
                    }
                }
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else //Nếu xem từ khóa
            {
                string query = Request["query"];
                var ltsResults = _DocumentDA.GetListSimpleByAutoComplete(query, 10);
                AutoCompleteItem ResulValues = new AutoCompleteItem()
                {
                    query = query,
                    data = ltsResults.Select(o => o.ID.ToString()).ToList(),
                    suggestions = ltsResults.Select(o => o.SoKyHieu).ToList()
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
            DocumentTypeDA documentTypeDA = new DocumentTypeDA();
            ViewBag.AllCategory = documentTypeDA.getAllListSimple();
            ViewBag.AllEvent = new Data.Admin.DocumentScopeDA().getAllListSimple();
            ViewBag.AllStatus = documentTypeDA.getAllStatusSimple();
            ViewBag.AllUser = documentTypeDA.getAllUserSimple();
            return View();
        }

        /// <summary>
        /// Load danh sách bản ghi dưới dạng bảng
        /// </summary>
        /// <returns></returns>
        public ActionResult ListItems()
        {
            ViewData.Model = _DocumentDA.getListSimpleByRequest(Request);
            ViewBag.PageHtml = _DocumentDA.GridHtmlPage;
            return View();
        }


        /// <summary>
        /// Trang xem chi tiết trong model
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxView()
        {
            var DocumentModel = _DocumentDA.getById(ArrID.FirstOrDefault());
            ViewData.Model = DocumentModel;
            return View();
        }

        /// <summary>
        /// Form dùng cho thêm mới, sửa. Load bằng Ajax dialog
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxForm()
        {
            var DocumentModel = new Base.Document()
            {
                AllowComment = true,
                TrichYeu = string.Empty,
                Details = string.Empty
            };

            if (DoAction == ActionType.Edit)
                DocumentModel = _DocumentDA.getById(ArrID.FirstOrDefault());


            ViewBag.AllCategory = new Data.Admin.DocumentTypeDA().getAllListSimple();
            ViewBag.AllEvent = new Data.Admin.DocumentScopeDA().getAllListSimple();
            ViewBag.AllIssue  = new Data.Admin.DocumentIssueDA().getAllListSimple();
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(DocumentModel);
        }

        public ActionResult AjaxApprove()
        {
            var contentDA = new Data.Admin.DocumentDA();
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
            Base.Document _Document = new Base.Document();
            List<Base.Document> lts_DocumentItems;
            StringBuilder stbMessage;


            List<int> IDValues;
            List<QNews.Base.DocumentScope> LtsEventSelected;

            try
            {
                switch (DoAction)
                {

                    case ActionType.Add:
                        if (hasAdd)
                        {
                            UpdateModel(_Document);

                            _Document.TrichYeu = Sanitizer.GetSafeHtmlFragment(_Document.TrichYeu);
                            _Document.CreateDate = DateTime.Now;
                            _Document.ModifyDate = DateTime.Now;
                            _Document.CreateBy = User.Identity.GetUserId();
                            _Document.ModifyBy = User.Identity.GetUserId();
                            _Document.StatusID = (int)Utils.WorkFlowStatus.BAN_NHAP;
                            if (!string.IsNullOrEmpty(Request["OtherFileAttach"]))
                                _Document.FileAttach = Request["OtherFileAttach"];


                            _Document.Details = Sanitizer.GetSafeHtmlFragment(_Document.Details);

                            #region Cập nhật event
                            IDValues = StaticClass.getValuesArray(Request["EventID"]);
                            LtsEventSelected = _DocumentDA.getListEventByArrID(IDValues);
                            foreach (var category in LtsEventSelected)
                            {
                                _Document.DocumentScopes.Add(category);
                            }
                            #endregion

                            #region Cập nhật event
                            IDValues = StaticClass.getValuesArray(Request["IssueID"]);
                            var LtsissueSelected = _DocumentDA.getListIssueByArrID(IDValues);
                            foreach (var issue in LtsissueSelected)
                            {
                                _Document.DocumentIssues.Add(issue);
                            }
                            #endregion

                            #region cập nhật url
                            Base.Url url = new Base.Url();
                            url.UrlID = Request["Url"];
                            _Document.Urls.Add(url);
                            #endregion

                            _DocumentDA.Add(_Document);
                            _DocumentDA.Save();
                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = _Document.ID.ToString(),
                                Message = string.Format("Đã thêm mới nội dung: <b>{0}</b>", Server.HtmlEncode(_Document.SoKyHieu))
                            };
                        }
                        break;

                    case ActionType.Edit:
                        if (hasEdit)
                        {
                            _Document = _DocumentDA.getById(ArrID.FirstOrDefault());

                            UpdateModel(_Document);

                            _Document.TrichYeu = Sanitizer.GetSafeHtmlFragment(_Document.TrichYeu);
                            _Document.ModifyDate = DateTime.Now;
                            _Document.ModifyBy = User.Identity.GetUserId();
                            _Document.StatusID = (int)Utils.WorkFlowStatus.CHO_DUYET;

                            if (!string.IsNullOrEmpty(Request["OtherFileAttach"]))
                                _Document.FileAttach = Request["OtherFileAttach"];


                            _Document.Details = Sanitizer.GetSafeHtmlFragment(_Document.Details);

                            #region Cập nhật event
                            _Document.DocumentScopes.Clear();
                            IDValues = StaticClass.getValuesArray(Request["EventID"]);
                            LtsEventSelected = _DocumentDA.getListEventByArrID(IDValues);
                            foreach (var category in LtsEventSelected)
                            {
                                _Document.DocumentScopes.Add(category);
                            }
                            #endregion

                            #region Cập nhật event
                            _Document.DocumentIssues.Clear();
                            IDValues = StaticClass.getValuesArray(Request["IssueID"]);
                            var LtsissueSelected = _DocumentDA.getListIssueByArrID(IDValues);
                            foreach (var issue in LtsissueSelected)
                            {
                                _Document.DocumentIssues.Add(issue);
                            }
                            #endregion

                            _DocumentDA.Save();
                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = _Document.ID.ToString(),
                                Message = string.Format("Đã cập nhật nội dung: <b>{0}</b>", Server.HtmlEncode(_Document.SoKyHieu))
                            };
                        }
                        break;

                    case ActionType.Delete:
                        if (hasDelete)
                        {
                            lts_DocumentItems = _DocumentDA.getListByArrID(ArrID);
                            stbMessage = new StringBuilder();
                            foreach (var item in lts_DocumentItems)
                            {

                                _DocumentDA.Delete(item);
                                stbMessage.AppendFormat("Đã xóa nội dung <b>{0}</b>.<br />", Server.HtmlEncode(item.SoKyHieu));
                            }
                            msg.ID = string.Join(",", ArrID);
                            _DocumentDA.Save();
                            msg.Message = stbMessage.ToString();
                        }
                        break;


                    case ActionType.Approve:
                        if (hasApprove)
                        {
                            _Document = _DocumentDA.getById(ArrID.FirstOrDefault());
                            Base.ApproveLog log = new Base.ApproveLog();
                            log.StatusID = Convert.ToInt32(Request["StatusID"]);
                            log.Description = Request["Description"];
                            log.CreatedDate = DateTime.Now;
                            log.UserID = User.Identity.GetUserId();
                            log.DocumentID = _Document.ID;

                            _Document.StatusID = log.StatusID;
                            _DocumentDA.QNewsDB.ApproveLogs.Add(log);
                            _DocumentDA.Save();

                            var status = _DocumentDA.QNewsDB.Status.Where(o => o.ID == log.StatusID).FirstOrDefault().Name;

                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = _Document.ID.ToString(),
                                Message = string.Format("Đã thực hiện <strong>{1}</strong> nội dung <b>{0}</b>", _Document.SoKyHieu, status)
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