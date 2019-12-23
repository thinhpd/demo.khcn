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
    public class AdvertiseController : BaseController
    {
        AdvertiseDA _AdvertiseDA = new AdvertiseDA("#");

        public ActionResult Details(int id)
        {
            AdvertiseDA advertiseDA = new AdvertiseDA();
            var model = advertiseDA.getById(id);
            ViewBag.AllStatus = advertiseDA.getAllStatusSimple();
            ViewBag.AllUser = advertiseDA.getAllUserSimple();
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
                    var _Advertise = _AdvertiseDA.getByName(SearchValue);
                    if (_Advertise == null)
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
                            ID = _Advertise.ID.ToString(),
                            Message = _Advertise.Title
                        };
                    }
                }
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else //Nếu xem từ khóa
            {
                string query = Request["query"];
                var ltsResults = _AdvertiseDA.GetListSimpleByAutoComplete(query, 10);
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
            AdvertiseDA advertiseTypeDA = new AdvertiseDA();
            ViewBag.AllStatus = advertiseTypeDA.getAllStatusSimple();
            ViewBag.AllUser = advertiseTypeDA.getAllUserSimple();
            return View();
        }

        /// <summary>
        /// Load danh sách bản ghi dưới dạng bảng
        /// </summary>
        /// <returns></returns>
        public ActionResult ListItems()
        {
            ViewData.Model = _AdvertiseDA.getListSimpleByRequest(Request);
            ViewBag.PageHtml = _AdvertiseDA.GridHtmlPage;
            return View();
        }


        /// <summary>
        /// Trang xem chi tiết trong model
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxView()
        {
            var AdvertiseModel = _AdvertiseDA.getById(ArrID.FirstOrDefault());
            ViewData.Model = AdvertiseModel;
            return View();
        }

        /// <summary>
        /// Form dùng cho thêm mới, sửa. Load bằng Ajax dialog
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxForm()
        {
            var AdvertiseModel = new Base.Advertise()
            {
                Description = string.Empty
            };

            if (DoAction == ActionType.Edit)
                AdvertiseModel = _AdvertiseDA.getById(ArrID.FirstOrDefault());


            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(AdvertiseModel);
        }

        public ActionResult AjaxApprove()
        {
            var contentDA = new Data.Admin.AdvertiseDA();
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
        [ValidateAntiForgeryToken()]
        public ActionResult Actions()
        {
            JsonMessage msg = new JsonMessage();
            Base.Advertise _Advertise = new Base.Advertise();
            List<Base.Advertise> lts_AdvertiseItems;
            StringBuilder stbMessage;


            List<int> IDValues;

            try
            {

                switch (DoAction)
                {

                    case ActionType.Add:
                        if (hasAdd)
                        {
                            UpdateModel(_Advertise);
                            _Advertise.Description = Sanitizer.GetSafeHtmlFragment(_Advertise.Description);
                            _Advertise.CreateDate = DateTime.Now;
                            _Advertise.ModifyDate = DateTime.Now;
                            _Advertise.CreateBy = User.Identity.GetUserId();
                            _Advertise.ModifyBy = User.Identity.GetUserId();
                            _Advertise.StatusID = (int)Utils.WorkFlowStatus.BAN_NHAP;


                            _AdvertiseDA.Add(_Advertise);
                            _AdvertiseDA.Save();
                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = _Advertise.ID.ToString(),
                                Message = string.Format("Đã thêm mới nội dung: <b>{0}</b>", Server.HtmlEncode(_Advertise.Title))
                            };
                        }
                        break;

                    case ActionType.Edit:
                        if (hasEdit)
                        {
                            _Advertise = _AdvertiseDA.getById(ArrID.FirstOrDefault());

                            UpdateModel(_Advertise);
                            _Advertise.Description = Sanitizer.GetSafeHtmlFragment(_Advertise.Description);
                            _Advertise.ModifyDate = DateTime.Now;
                            _Advertise.ModifyBy = User.Identity.GetUserId();
                            _Advertise.StatusID = (int)Utils.WorkFlowStatus.CHO_DUYET;


                            _AdvertiseDA.Save();
                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = _Advertise.ID.ToString(),
                                Message = string.Format("Đã cập nhật nội dung: <b>{0}</b>", Server.HtmlEncode(_Advertise.Title))
                            };
                        }
                        break;

                    case ActionType.Delete:
                        if (hasDelete)
                        {
                            lts_AdvertiseItems = _AdvertiseDA.getListByArrID(ArrID);
                            stbMessage = new StringBuilder();
                            foreach (var item in lts_AdvertiseItems)
                            {

                                _AdvertiseDA.Delete(item);
                                stbMessage.AppendFormat("Đã xóa nội dung <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                            }
                            msg.ID = string.Join(",", ArrID);
                            _AdvertiseDA.Save();
                            msg.Message = stbMessage.ToString();
                        }
                        break;


                    case ActionType.Approve:
                        if (hasApprove)
                        {
                            _Advertise = _AdvertiseDA.getById(ArrID.FirstOrDefault());
                            Base.ApproveLog log = new Base.ApproveLog();
                            log.StatusID = Convert.ToInt32(Request["StatusID"]);
                            log.Description = Request["Description"];
                            log.CreatedDate = DateTime.Now;
                            log.UserID = User.Identity.GetUserId();
                            log.AdvertiseID = _Advertise.ID;

                            _Advertise.StatusID = log.StatusID;
                            _AdvertiseDA.QNewsDB.ApproveLogs.Add(log);
                            _AdvertiseDA.Save();

                            var status = _AdvertiseDA.QNewsDB.Status.Where(o => o.ID == log.StatusID).FirstOrDefault().Name;

                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = _Advertise.ID.ToString(),
                                Message = string.Format("Đã thực hiện <strong>{1}</strong> nội dung <b>{0}</b>", _Advertise.Title, status)
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