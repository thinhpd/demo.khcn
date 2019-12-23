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
    public class VideoController : BaseController
    {
        VideoDA _VideoDA = new VideoDA("#");
        
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
                    var _Video = _VideoDA.getByName(SearchValue);
                    if (_Video == null)
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
                            ID = _Video.ID.ToString(),
                            Message = _Video.Title
                        };
                    }
                }
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else //Nếu xem từ khóa
            {
                string query = Request["query"];
                var ltsResults = _VideoDA.GetListSimpleByAutoComplete(query, 10);
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
            VideoTopicDA videoTypeDA = new VideoTopicDA();
            ViewBag.AllCategory = videoTypeDA.getAllListSimple();
            ViewBag.AllStatus = videoTypeDA.getAllStatusSimple();
            ViewBag.AllUser = videoTypeDA.getAllUserSimple();
            return View();
        }

        /// <summary>
        /// Load danh sách bản ghi dưới dạng bảng
        /// </summary>
        /// <returns></returns>
        public ActionResult ListItems()
        {
            ViewData.Model = _VideoDA.getListSimpleByRequest(Request);
            ViewBag.PageHtml = _VideoDA.GridHtmlPage;
            return View();
        }


        /// <summary>
        /// Trang xem chi tiết trong model
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxView()
        {
            var VideoModel = _VideoDA.getById(ArrID.FirstOrDefault());
            ViewData.Model = VideoModel;
            return View();
        }

        /// <summary>
        /// Form dùng cho thêm mới, sửa. Load bằng Ajax dialog
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxForm()
        {
            var VideoModel = new Base.Video()
            {
                AllowComment = true,
                Description = string.Empty,
                Details = string.Empty
            };

            if (DoAction == ActionType.Edit)
                VideoModel = _VideoDA.getById(ArrID.FirstOrDefault());


            ViewBag.AllCategory = new Data.Admin.VideoTopicDA().getAllListSimple();
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(VideoModel);
        }

        public ActionResult AjaxApprove()
        {
            var contentDA = new Data.Admin.VideoDA();
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
            Base.Video _Video = new Base.Video();
            List<Base.Video> lts_VideoItems;
            StringBuilder stbMessage;


            List<int> IDValues;

            try { 
            switch (DoAction)
            {

                case ActionType.Add:
                    if (hasAdd)
                    {
                        UpdateModel(_Video);
                        _Video.Description = Sanitizer.GetSafeHtmlFragment(_Video.Description);
                        _Video.CreateDate = DateTime.Now;
                        _Video.ModifyDate = DateTime.Now;
                        _Video.CreateBy = User.Identity.GetUserId();
                        _Video.ModifyBy = User.Identity.GetUserId();
                        _Video.StatusID = (int)Utils.WorkFlowStatus.BAN_NHAP;

                        _Video.Details = Sanitizer.GetSafeHtmlFragment(_Video.Details);


                        if (!string.IsNullOrEmpty(Request["OtherFileAttach"]))
                            _Video.FileAttach = Request["OtherFileAttach"];


                        #region cập nhật url
                        Base.Url url = new Base.Url();
                        url.UrlID = Request["Url"];
                        _Video.Urls.Add(url);
                        #endregion

                        _VideoDA.Add(_Video);
                        _VideoDA.Save();
                        msg = new JsonMessage()
                        {
                            Erros = false,
                            ID = _Video.ID.ToString(),
                            Message = string.Format("Đã thêm mới nội dung: <b>{0}</b>", Server.HtmlEncode(_Video.Title))
                        };
                    }
                    break;

                case ActionType.Edit:
                    if (hasEdit)
                    {
                        _Video = _VideoDA.getById(ArrID.FirstOrDefault());

                        UpdateModel(_Video);

                        _Video.Description = Sanitizer.GetSafeHtmlFragment(_Video.Description);
                        _Video.Details = Sanitizer.GetSafeHtmlFragment(_Video.Details);
                        _Video.ModifyDate = DateTime.Now;
                        _Video.ModifyBy = User.Identity.GetUserId();
                        _Video.StatusID = (int)Utils.WorkFlowStatus.CHO_DUYET;


                        if (!string.IsNullOrEmpty(Request["OtherFileAttach"]))
                            _Video.FileAttach = Request["OtherFileAttach"];

                        _VideoDA.Save();
                        msg = new JsonMessage()
                        {
                            Erros = false,
                            ID = _Video.ID.ToString(),
                            Message = string.Format("Đã cập nhật nội dung: <b>{0}</b>", Server.HtmlEncode(_Video.Title))
                        };
                    }
                    break;

                case ActionType.Delete:
                    if (hasDelete)
                    {
                        lts_VideoItems = _VideoDA.getListByArrID(ArrID);
                        stbMessage = new StringBuilder();
                        foreach (var item in lts_VideoItems)
                        {

                            _VideoDA.Delete(item);
                            stbMessage.AppendFormat("Đã xóa nội dung <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                        }
                        msg.ID = string.Join(",", ArrID);
                        _VideoDA.Save();
                        msg.Message = stbMessage.ToString();
                    }
                    break;


                case ActionType.Approve:
                    if (hasApprove)
                    {
                        _Video = _VideoDA.getById(ArrID.FirstOrDefault());
                        Base.ApproveLog log = new Base.ApproveLog();
                        log.StatusID = Convert.ToInt32(Request["StatusID"]);
                        log.Description = Request["Description"];
                        log.CreatedDate = DateTime.Now;
                        log.UserID = User.Identity.GetUserId();
                        log.VideoID = _Video.ID;

                        _Video.StatusID = log.StatusID;
                        _VideoDA.QNewsDB.ApproveLogs.Add(log);
                        _VideoDA.Save();

                        var status = _VideoDA.QNewsDB.Status.Where(o => o.ID == log.StatusID).FirstOrDefault().Name;

                        msg = new JsonMessage()
                        {
                            Erros = false,
                            ID = _Video.ID.ToString(),
                            Message = string.Format("Đã thực hiện <strong>{1}</strong> nội dung <b>{0}</b>", _Video.Title, status)
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