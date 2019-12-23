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
    public class AlbumPictureController : BaseController
    {
        AlbumPictureDA _AlbumPictureDA = new AlbumPictureDA("#");
        
        public ActionResult Details(int id)
        {
            AlbumPictureDA albumPictureDA = new AlbumPictureDA();
            var model = albumPictureDA.getById(id);
            ViewBag.AllStatus = albumPictureDA.getAllStatusSimple();
            ViewBag.AllUser = albumPictureDA.getAllUserSimple();
            return View(model);
        }

        public ActionResult Addnew()
        {
           
            return View();
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
                    var _AlbumPicture = _AlbumPictureDA.getByName(SearchValue);
                    if (_AlbumPicture == null)
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
                            ID = _AlbumPicture.ID.ToString(),
                            Message = _AlbumPicture.Title
                        };
                    }
                }
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else //Nếu xem từ khóa
            {
                string query = Request["query"];
                var ltsResults = _AlbumPictureDA.GetListSimpleByAutoComplete(query, 10);
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
            AlbumDA albumDA = new AlbumDA();
            ViewBag.AllCategory = albumDA.getAllListSimple();
            ViewBag.AllStatus = albumDA.getAllStatusSimple();
            ViewBag.AllUser = albumDA.getAllUserSimple();
            return View();
        }

        /// <summary>
        /// Load danh sách bản ghi dưới dạng bảng
        /// </summary>
        /// <returns></returns>
        public ActionResult ListItems()
        {
            ViewData.Model = _AlbumPictureDA.getListSimpleByRequest(Request);
            ViewBag.PageHtml = _AlbumPictureDA.GridHtmlPage;
            return View();
        }


        /// <summary>
        /// Trang xem chi tiết trong model
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxView()
        {
            var AlbumPictureModel = _AlbumPictureDA.getById(ArrID.FirstOrDefault());
            ViewData.Model = AlbumPictureModel;
            return View();
        }

        /// <summary>
        /// Form dùng cho thêm mới, sửa. Load bằng Ajax dialog
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxForm()
        {
            var AlbumPictureModel = new Base.AlbumPicture()
            {
                AllowComment = true,
                Description = string.Empty
            };

            if (DoAction == ActionType.Edit)
                AlbumPictureModel = _AlbumPictureDA.getById(ArrID.FirstOrDefault());


            ViewBag.AllCategory = new Data.Admin.AlbumDA().getAllListSimple();
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(AlbumPictureModel);
        }

        public ActionResult AjaxApprove()
        {
            var contentDA = new Data.Admin.AlbumPictureDA();
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
            Base.AlbumPicture _AlbumPicture = new Base.AlbumPicture();
            List<Base.AlbumPicture> lts_AlbumPictureItems;
            StringBuilder stbMessage;


            List<int> IDValues;

            try { 
            switch (DoAction)
            {

                case ActionType.Add:
                    if (hasAdd)
                    {
                        UpdateModel(_AlbumPicture);
                        _AlbumPicture.Description = Sanitizer.GetSafeHtmlFragment(_AlbumPicture.Description);
                        _AlbumPicture.CreateDate = DateTime.Now;
                        _AlbumPicture.ModifyDate = DateTime.Now;
                        _AlbumPicture.CreateBy = User.Identity.GetUserId();
                        _AlbumPicture.ModifyBy = User.Identity.GetUserId();
                        _AlbumPicture.StatusID = (int)Utils.WorkFlowStatus.BAN_NHAP;

                        _AlbumPictureDA.Add(_AlbumPicture);
                        _AlbumPictureDA.Save();
                        msg = new JsonMessage()
                        {
                            Erros = false,
                            ID = _AlbumPicture.ID.ToString(),
                            Message = string.Format("Đã thêm mới nội dung: <b>{0}</b>", Server.HtmlEncode(_AlbumPicture.Title))
                        };
                    }
                    break;

                case ActionType.Edit:
                    if (hasEdit)
                    {
                        _AlbumPicture = _AlbumPictureDA.getById(ArrID.FirstOrDefault());

                        UpdateModel(_AlbumPicture);
                        _AlbumPicture.Description = Sanitizer.GetSafeHtmlFragment(_AlbumPicture.Description);
                        _AlbumPicture.ModifyDate = DateTime.Now;
                        _AlbumPicture.ModifyBy = User.Identity.GetUserId();
                        _AlbumPicture.StatusID = (int)Utils.WorkFlowStatus.CHO_DUYET;


                        _AlbumPictureDA.Save();
                        msg = new JsonMessage()
                        {
                            Erros = false,
                            ID = _AlbumPicture.ID.ToString(),
                            Message = string.Format("Đã cập nhật nội dung: <b>{0}</b>", Server.HtmlEncode(_AlbumPicture.Title))
                        };
                    }
                    break;

                case ActionType.Delete:
                    if (hasDelete)
                    {
                        lts_AlbumPictureItems = _AlbumPictureDA.getListByArrID(ArrID);
                        stbMessage = new StringBuilder();
                        foreach (var item in lts_AlbumPictureItems)
                        {

                            _AlbumPictureDA.Delete(item);
                            stbMessage.AppendFormat("Đã xóa nội dung <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                        }
                        msg.ID = string.Join(",", ArrID);
                        _AlbumPictureDA.Save();
                        msg.Message = stbMessage.ToString();
                    }
                    break;


                case ActionType.Approve:
                    if (hasApprove)
                    {
                        _AlbumPicture = _AlbumPictureDA.getById(ArrID.FirstOrDefault());
                        Base.ApproveLog log = new Base.ApproveLog();
                        log.StatusID = Convert.ToInt32(Request["StatusID"]);
                        log.Description = Request["Description"];
                        log.CreatedDate = DateTime.Now;
                        log.UserID = User.Identity.GetUserId();
                        log.PictureID = _AlbumPicture.ID;

                        _AlbumPicture.StatusID = log.StatusID;
                        _AlbumPictureDA.QNewsDB.ApproveLogs.Add(log);
                        _AlbumPictureDA.Save();

                        var status = _AlbumPictureDA.QNewsDB.Status.Where(o => o.ID == log.StatusID).FirstOrDefault().Name;

                        msg = new JsonMessage()
                        {
                            Erros = false,
                            ID = _AlbumPicture.ID.ToString(),
                            Message = string.Format("Đã thực hiện <strong>{1}</strong> nội dung <b>{0}</b>", _AlbumPicture.Title, status)
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