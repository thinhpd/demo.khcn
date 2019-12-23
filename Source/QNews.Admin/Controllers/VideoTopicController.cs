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
    public class VideoTopicController : BaseController
    {
        VideoTopicDA _videoTopicDA = new VideoTopicDA("#");

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
                        Message = "Bạn phải nhập tên chủ đề"
                    };
                }
                else
                {
                    var _videoTopic = _videoTopicDA.getByName(SearchValue);
                    if (_videoTopic == null)
                    {
                        msg = new JsonMessage()
                        {
                            Erros = true,
                            Message = "chủ đề không tồn tại trong hệ thống"
                        };
                    }
                    else
                    {
                        msg = new JsonMessage()
                        {
                            Erros = false,
                            ID = _videoTopic.ID.ToString(),
                            Message = _videoTopic.Title
                        };
                    }
                }
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else //Nếu xem từ khóa
            {
                string query = Request["query"];
                var ltsResults = _videoTopicDA.GetListSimpleByAutoComplete(query, 10);
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
            ViewData.Model = _videoTopicDA.getListSimpleByRequest(Request);
            ViewBag.PageHtml = _videoTopicDA.GridHtmlPage;
            return View();
        }


        /// <summary>
        /// Trang xem chi tiết trong model
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxView()
        {
            var videoTopicModel = _videoTopicDA.getById(ArrID.FirstOrDefault());
            ViewData.Model = videoTopicModel;
            return View();
        }

        /// <summary>
        /// Form dùng cho thêm mới, sửa. Load bằng Ajax dialog
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxForm()
        {
            var videoTopicModel = new Base.VideoTopic()
            {
                Order = 0,
                Show = true
            };

            if (DoAction == ActionType.Edit)
                videoTopicModel = _videoTopicDA.getById(ArrID.FirstOrDefault());

            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(videoTopicModel);
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
            VideoTopic _videoTopic = new VideoTopic();
            List<VideoTopic> lts_videoTopicItems;
            StringBuilder stbMessage;
            try
            {
                switch (DoAction)
                {
                    case ActionType.Add:
                        if (hasAdd)
                        {
                            UpdateModel(_videoTopic);
                            _videoTopic.Description = Sanitizer.GetSafeHtmlFragment(_videoTopic.Description);
                            _videoTopic.CreateDate = DateTime.Now;
                            _videoTopic.ModifyDate = DateTime.Now;
                            _videoTopic.CreateBy = User.Identity.GetUserId();
                            _videoTopic.ModifyBy = User.Identity.GetUserId();

                            _videoTopicDA.Add(_videoTopic);
                            _videoTopicDA.Save();
                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = _videoTopic.ID.ToString(),
                                Message = string.Format("Đã thêm mới chủ đề: <b>{0}</b>", Server.HtmlEncode(_videoTopic.Title))
                            };
                        }
                        break;

                    case ActionType.Edit:
                        if (hasEdit)
                        {
                            _videoTopic = _videoTopicDA.getById(ArrID.FirstOrDefault());
                            UpdateModel(_videoTopic);
                            _videoTopic.Description = Sanitizer.GetSafeHtmlFragment(_videoTopic.Description);
                            _videoTopic.ModifyDate = DateTime.Now;
                            _videoTopic.ModifyBy = User.Identity.GetUserId();
                            _videoTopicDA.Save();
                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = _videoTopic.ID.ToString(),
                                Message = string.Format("Đã cập nhật chủ đề: <b>{0}</b>", Server.HtmlEncode(_videoTopic.Title))
                            };
                        }
                        break;

                    case ActionType.Delete:
                        if (hasDelete)
                        {
                            lts_videoTopicItems = _videoTopicDA.getListByArrID(ArrID);
                            stbMessage = new StringBuilder();
                            foreach (var item in lts_videoTopicItems)
                            {

                                _videoTopicDA.Delete(item);
                                stbMessage.AppendFormat("Đã xóa chủ đề <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                            }
                            msg.ID = string.Join(",", ArrID);
                            _videoTopicDA.Save();
                            msg.Message = stbMessage.ToString();
                        }
                        break;

                    case ActionType.Show:
                        if (hasEdit)
                        {
                            lts_videoTopicItems = _videoTopicDA.getListByArrID(ArrID);
                            stbMessage = new StringBuilder();
                            foreach (var item in lts_videoTopicItems.Where(o => !o.Show))
                            {

                                item.Show = true;
                                _videoTopic.ModifyDate = DateTime.Now;
                                _videoTopic.ModifyBy = User.Identity.GetUserId();
                                stbMessage.AppendFormat("Đã hiển thị chủ đề <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                            }
                            msg.ID = string.Join(",", ArrID);
                            _videoTopicDA.Save();
                            msg.Message = stbMessage.ToString();
                        }
                        break;

                    case ActionType.Hide:
                        if (hasEdit)
                        {
                            lts_videoTopicItems = _videoTopicDA.getListByArrID(ArrID);
                            stbMessage = new StringBuilder();
                            foreach (var item in lts_videoTopicItems.Where(o => o.Show))
                            {
                                item.Show = false;
                                _videoTopic.ModifyDate = DateTime.Now;
                                _videoTopic.ModifyBy = User.Identity.GetUserId();
                                stbMessage.AppendFormat("Đã ẩn chủ đề <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                            }
                            msg.ID = string.Join(",", ArrID);
                            _videoTopicDA.Save();
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