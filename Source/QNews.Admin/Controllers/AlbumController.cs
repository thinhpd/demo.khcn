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
    public class AlbumController : BaseController
    {
        AlbumDA _AlbumDA = new AlbumDA("#");

        public ActionResult Details(int id)
        {
            AlbumDA albumDA = new AlbumDA();
            var model = albumDA.getById(id);
            ViewBag.AllStatus = albumDA.getAllStatusSimple();
            ViewBag.AllUser = albumDA.getAllUserSimple();
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
                    var _Album = _AlbumDA.getByName(SearchValue);
                    if (_Album == null)
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
                            ID = _Album.ID.ToString(),
                            Message = _Album.Title
                        };
                    }
                }
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else //Nếu xem từ khóa
            {
                string query = Request["query"];
                var ltsResults = _AlbumDA.GetListSimpleByAutoComplete(query, 10);
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
            AlbumTopicDA albumTypeDA = new AlbumTopicDA();
            ViewBag.AllCategory = albumTypeDA.getAllListSimple();
            ViewBag.AllStatus = albumTypeDA.getAllStatusSimple();
            ViewBag.AllUser = albumTypeDA.getAllUserSimple();
            return View();
        }

        /// <summary>
        /// Load danh sách bản ghi dưới dạng bảng
        /// </summary>
        /// <returns></returns>
        public ActionResult ListItems()
        {
            ViewData.Model = _AlbumDA.getListSimpleByRequest(Request);
            ViewBag.PageHtml = _AlbumDA.GridHtmlPage;
            return View();
        }


        /// <summary>
        /// Trang xem chi tiết trong model
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxView()
        {
            var AlbumModel = _AlbumDA.getById(ArrID.FirstOrDefault());
            ViewData.Model = AlbumModel;
            return View();
        }

        /// <summary>
        /// Form dùng cho thêm mới, sửa. Load bằng Ajax dialog
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxForm()
        {
            var AlbumModel = new Base.Album()
            {
                AllowComment = true,
                Description = string.Empty,
                Details = string.Empty
            };

            if (DoAction == ActionType.Edit)
                AlbumModel = _AlbumDA.getById(ArrID.FirstOrDefault());


            ViewBag.AllCategory = new Data.Admin.AlbumTopicDA().getAllListSimple();
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(AlbumModel);
        }
        public ActionResult AjaxForms()
        {
            var AlbumModel = new Base.Album()
            {
                AllowComment = true,
                Description = string.Empty,
                Details = string.Empty
            };

            if (DoAction == ActionType.Edit)
                AlbumModel = _AlbumDA.getById(ArrID.FirstOrDefault());


            ViewBag.AllCategory = new Data.Admin.AlbumTopicDA().getAllListSimple();
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(AlbumModel);
        }
        
        public ActionResult AjaxApprove()
        {
            var contentDA = new Data.Admin.AlbumDA();
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
            Base.Album _Album = new Base.Album();
            List<Base.Album> lts_AlbumItems;
            StringBuilder stbMessage;


            List<int> IDValues;

            try
            {

                switch (DoAction)
                {

                    case ActionType.Add:
                        if (hasAdd)
                        {
                            UpdateModel(_Album);

                            _Album.Description = Sanitizer.GetSafeHtmlFragment(_Album.Description);
                            _Album.Details = Sanitizer.GetSafeHtmlFragment(_Album.Details);
                            _Album.CreateDate = DateTime.Now;
                            _Album.ModifyDate = DateTime.Now;
                            _Album.CreateBy = User.Identity.GetUserId();
                            _Album.ModifyBy = User.Identity.GetUserId();
                            _Album.StatusID = (int)Utils.WorkFlowStatus.BAN_NHAP;


                            #region cập nhật url
                            Base.Url url = new Base.Url();
                            url.UrlID = Request["Url"];
                            _Album.Urls.Add(url);
                            #endregion

                            _AlbumDA.Add(_Album);
                            _AlbumDA.Save();
                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = _Album.ID.ToString(),
                                Message = string.Format("Đã thêm mới nội dung: <b>{0}</b>", Server.HtmlEncode(_Album.Title))
                            };
                        }
                        break;

                    case ActionType.Edit:
                        if (hasEdit)
                        {
                            _Album = _AlbumDA.getById(ArrID.FirstOrDefault());

                            UpdateModel(_Album);
                            _Album.Description = Sanitizer.GetSafeHtmlFragment(_Album.Description);
                            _Album.Details = Sanitizer.GetSafeHtmlFragment(_Album.Details);
                            _Album.ModifyDate = DateTime.Now;
                            _Album.ModifyBy = User.Identity.GetUserId();
                            _Album.StatusID = (int)Utils.WorkFlowStatus.CHO_DUYET;


                            _AlbumDA.Save();
                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = _Album.ID.ToString(),
                                Message = string.Format("Đã cập nhật nội dung: <b>{0}</b>", Server.HtmlEncode(_Album.Title))
                            };
                        }
                        break;

                    case ActionType.Delete:
                        if (hasDelete)
                        {
                            lts_AlbumItems = _AlbumDA.getListByArrID(ArrID);
                            stbMessage = new StringBuilder();
                            foreach (var item in lts_AlbumItems)
                            {

                                _AlbumDA.Delete(item);
                                stbMessage.AppendFormat("Đã xóa nội dung <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                            }
                            msg.ID = string.Join(",", ArrID);
                            _AlbumDA.Save();
                            msg.Message = stbMessage.ToString();
                        }
                        break;


                    case ActionType.Approve:
                        if (hasApprove)
                        {
                            _Album = _AlbumDA.getById(ArrID.FirstOrDefault());
                            Base.ApproveLog log = new Base.ApproveLog();
                            log.StatusID = Convert.ToInt32(Request["StatusID"]);
                            log.Description = Request["Description"];
                            log.CreatedDate = DateTime.Now;
                            log.UserID = User.Identity.GetUserId();
                            log.AlbumID = _Album.ID;

                            _Album.StatusID = log.StatusID;
                            _AlbumDA.QNewsDB.ApproveLogs.Add(log);
                            _AlbumDA.Save();

                            var status = _AlbumDA.QNewsDB.Status.Where(o => o.ID == log.StatusID).FirstOrDefault().Name;

                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = _Album.ID.ToString(),
                                Message = string.Format("Đã thực hiện <strong>{1}</strong> nội dung <b>{0}</b>", _Album.Title, status)
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