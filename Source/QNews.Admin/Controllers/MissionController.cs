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
    public class MissionController : BaseController
    {
        MissionDA _MissionDA = new MissionDA("#");

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
                    var _Mission = _MissionDA.getByName(SearchValue);
                    if (_Mission == null)
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
                            ID = _Mission.ID.ToString(),
                            Message = _Mission.MaNhiemVu
                        };
                    }
                }
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else //Nếu xem từ khóa
            {
                string query = Request["query"];
                var ltsResults = _MissionDA.GetListSimpleByAutoComplete(query, 10);
                AutoCompleteItem ResulValues = new AutoCompleteItem()
                {
                    query = query,
                    data = ltsResults.Select(o => o.ID.ToString()).ToList(),
                    suggestions = ltsResults.Select(o => o.MaNhiemVu).ToList()
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
            var TypeDA = new Data.Admin.DocumentScopeDA();
            ViewBag.AllEvent = TypeDA.getAllListSimple();
            ViewBag.AllStatus = TypeDA.getAllStatusSimple();
            ViewBag.AllUser = TypeDA.getAllUserSimple();
            return View();
        }

        /// <summary>
        /// Load danh sách bản ghi dưới dạng bảng
        /// </summary>
        /// <returns></returns>
        public ActionResult ListItems()
        {
            ViewData.Model = _MissionDA.getListSimpleByRequest(Request);
            ViewBag.PageHtml = _MissionDA.GridHtmlPage;
            return View();
        }


        /// <summary>
        /// Trang xem chi tiết trong model
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxView()
        {
            var MissionModel = _MissionDA.getById(ArrID.FirstOrDefault());
            ViewData.Model = MissionModel;
            return View();
        }

        /// <summary>
        /// Form dùng cho thêm mới, sửa. Load bằng Ajax dialog
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxForm()
        {
            var MissionModel = new Base.Mission()
            {
                TenNhiemVu = string.Empty,
                Details = string.Empty
            };

            if (DoAction == ActionType.Edit)
                MissionModel = _MissionDA.getById(ArrID.FirstOrDefault());


            ViewBag.AllEvent = new Data.Admin.DocumentScopeDA().getAllListSimple();
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(MissionModel);
        }

        public ActionResult AjaxApprove()
        {
            var contentDA = new Data.Admin.MissionDA();
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
            Base.Mission _Mission = new Base.Mission();
            List<Base.Mission> lts_MissionItems;
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
                            UpdateModel(_Mission);

                            _Mission.TenNhiemVu = Sanitizer.GetSafeHtmlFragment(_Mission.TenNhiemVu);
                            _Mission.CreateDate = DateTime.Now;
                            _Mission.ModifyDate = DateTime.Now;
                            _Mission.CreateBy = User.Identity.GetUserId();
                            _Mission.ModifyBy = User.Identity.GetUserId();
                            _Mission.StatusID = (int)Utils.WorkFlowStatus.BAN_NHAP;


                            _Mission.Details = Sanitizer.GetSafeHtmlFragment(_Mission.Details);

                            #region Cập nhật event
                            IDValues = StaticClass.getValuesArray(Request["EventID"]);
                            LtsEventSelected = _MissionDA.getListEventByArrID(IDValues);
                            foreach (var category in LtsEventSelected)
                            {
                                _Mission.DocumentScopes.Add(category);
                            }
                            #endregion


                            _MissionDA.Add(_Mission);
                            _MissionDA.Save();
                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = _Mission.ID.ToString(),
                                Message = string.Format("Đã thêm mới nội dung: <b>{0}</b>", Server.HtmlEncode(_Mission.MaNhiemVu))
                            };
                        }
                        break;

                    case ActionType.Edit:
                        if (hasEdit)
                        {
                            _Mission = _MissionDA.getById(ArrID.FirstOrDefault());

                            UpdateModel(_Mission);

                            _Mission.TenNhiemVu = Sanitizer.GetSafeHtmlFragment(_Mission.TenNhiemVu);
                            _Mission.ModifyDate = DateTime.Now;
                            _Mission.ModifyBy = User.Identity.GetUserId();
                            _Mission.StatusID = (int)Utils.WorkFlowStatus.CHO_DUYET;

                  


                            _Mission.Details = Sanitizer.GetSafeHtmlFragment(_Mission.Details);

                            #region Cập nhật event
                            _Mission.DocumentScopes.Clear();
                            IDValues = StaticClass.getValuesArray(Request["EventID"]);
                            LtsEventSelected = _MissionDA.getListEventByArrID(IDValues);
                            foreach (var category in LtsEventSelected)
                            {
                                _Mission.DocumentScopes.Add(category);
                            }
                            #endregion

                         

                            _MissionDA.Save();
                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = _Mission.ID.ToString(),
                                Message = string.Format("Đã cập nhật nội dung: <b>{0}</b>", Server.HtmlEncode(_Mission.MaNhiemVu))
                            };
                        }
                        break;

                    case ActionType.Delete:
                        if (hasDelete)
                        {
                            lts_MissionItems = _MissionDA.getListByArrID(ArrID);
                            stbMessage = new StringBuilder();
                            foreach (var item in lts_MissionItems)
                            {

                                _MissionDA.Delete(item);
                                stbMessage.AppendFormat("Đã xóa nội dung <b>{0}</b>.<br />", Server.HtmlEncode(item.MaNhiemVu));
                            }
                            msg.ID = string.Join(",", ArrID);
                            _MissionDA.Save();
                            msg.Message = stbMessage.ToString();
                        }
                        break;


                    case ActionType.Approve:
                        if (hasApprove)
                        {
                            _Mission = _MissionDA.getById(ArrID.FirstOrDefault());
                            Base.ApproveLog log = new Base.ApproveLog();
                            log.StatusID = Convert.ToInt32(Request["StatusID"]);
                            log.Description = Request["Description"];
                            log.CreatedDate = DateTime.Now;
                            log.UserID = User.Identity.GetUserId();
                            log.MissionID = _Mission.ID;

                            _Mission.StatusID = log.StatusID;
                            _MissionDA.QNewsDB.ApproveLogs.Add(log);
                            _MissionDA.Save();

                            var status = _MissionDA.QNewsDB.Status.Where(o => o.ID == log.StatusID).FirstOrDefault().Name;

                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = _Mission.ID.ToString(),
                                Message = string.Format("Đã thực hiện <strong>{1}</strong> nội dung <b>{0}</b>", _Mission.MaNhiemVu, status)
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