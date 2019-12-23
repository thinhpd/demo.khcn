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
    public class AudioController : BaseController
    {
        AudioDA _AudioDA = new AudioDA("#");
        
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
                    var _Audio = _AudioDA.getByName(SearchValue);
                    if (_Audio == null)
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
                            ID = _Audio.ID.ToString(),
                            Message = _Audio.Title
                        };
                    }
                }
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else //Nếu xem từ khóa
            {
                string query = Request["query"];
                var ltsResults = _AudioDA.GetListSimpleByAutoComplete(query, 10);
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
            AudioTopicDA audioTypeDA = new AudioTopicDA();
            ViewBag.AllCategory = audioTypeDA.getAllListSimple();
            ViewBag.AllStatus = audioTypeDA.getAllStatusSimple();
            ViewBag.AllUser = audioTypeDA.getAllUserSimple();
            return View();
        }

        /// <summary>
        /// Load danh sách bản ghi dưới dạng bảng
        /// </summary>
        /// <returns></returns>
        public ActionResult ListItems()
        {
            ViewData.Model = _AudioDA.getListSimpleByRequest(Request);
            ViewBag.PageHtml = _AudioDA.GridHtmlPage;
            return View();
        }


        /// <summary>
        /// Trang xem chi tiết trong model
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxView()
        {
            var AudioModel = _AudioDA.getById(ArrID.FirstOrDefault());
            ViewData.Model = AudioModel;
            return View();
        }

        /// <summary>
        /// Form dùng cho thêm mới, sửa. Load bằng Ajax dialog
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxForm()
        {
            var AudioModel = new Base.Audio()
            {
                AllowComment = true,
                Description = string.Empty,
                Details = string.Empty
            };

            if (DoAction == ActionType.Edit)
                AudioModel = _AudioDA.getById(ArrID.FirstOrDefault());


            ViewBag.AllCategory = new Data.Admin.AudioTopicDA().getAllListSimple();
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(AudioModel);
        }

        public ActionResult AjaxApprove()
        {
            var contentDA = new Data.Admin.AudioDA();
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
            Base.Audio _Audio = new Base.Audio();
            List<Base.Audio> lts_AudioItems;
            StringBuilder stbMessage;


            List<int> IDValues;
            try { 

            switch (DoAction)
            {

                case ActionType.Add:
                    if (hasAdd)
                    {
                        UpdateModel(_Audio);
                        _Audio.Description = Sanitizer.GetSafeHtmlFragment(_Audio.Description);
                        _Audio.CreateDate = DateTime.Now;
                        _Audio.ModifyDate = DateTime.Now;
                        _Audio.CreateBy = User.Identity.GetUserId();
                        _Audio.ModifyBy = User.Identity.GetUserId();
                        _Audio.StatusID = (int)Utils.WorkFlowStatus.BAN_NHAP;

                        _Audio.Details = Sanitizer.GetSafeHtmlFragment(_Audio.Details);


                        if (!string.IsNullOrEmpty(Request["OtherFileAttach"]))
                            _Audio.FileAttach = Request["OtherFileAttach"];

                        #region cập nhật url
                        Base.Url url = new Base.Url();
                        url.UrlID = Request["Url"];
                        _Audio.Urls.Add(url);
                        #endregion

                        _AudioDA.Add(_Audio);
                        _AudioDA.Save();
                        msg = new JsonMessage()
                        {
                            Erros = false,
                            ID = _Audio.ID.ToString(),
                            Message = string.Format("Đã thêm mới nội dung: <b>{0}</b>", Server.HtmlEncode(_Audio.Title))
                        };
                    }
                    break;

                case ActionType.Edit:
                    if (hasEdit)
                    {
                        _Audio = _AudioDA.getById(ArrID.FirstOrDefault());

                        UpdateModel(_Audio);
                        _Audio.Description = Sanitizer.GetSafeHtmlFragment(_Audio.Description);
                        _Audio.ModifyDate = DateTime.Now;
                        _Audio.ModifyBy = User.Identity.GetUserId();
                        _Audio.StatusID = (int)Utils.WorkFlowStatus.CHO_DUYET;

                        _Audio.Details = Sanitizer.GetSafeHtmlFragment(_Audio.Details);

                        if (!string.IsNullOrEmpty(Request["OtherFileAttach"]))
                            _Audio.FileAttach = Request["OtherFileAttach"];

                        _AudioDA.Save();
                        msg = new JsonMessage()
                        {
                            Erros = false,
                            ID = _Audio.ID.ToString(),
                            Message = string.Format("Đã cập nhật nội dung: <b>{0}</b>", Server.HtmlEncode(_Audio.Title))
                        };
                    }
                    break;

                case ActionType.Delete:
                    if (hasDelete)
                    {
                        lts_AudioItems = _AudioDA.getListByArrID(ArrID);
                        stbMessage = new StringBuilder();
                        foreach (var item in lts_AudioItems)
                        {

                            _AudioDA.Delete(item);
                            stbMessage.AppendFormat("Đã xóa nội dung <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                        }
                        msg.ID = string.Join(",", ArrID);
                        _AudioDA.Save();
                        msg.Message = stbMessage.ToString();
                    }
                    break;


                case ActionType.Approve:
                    if (hasApprove)
                    {
                        _Audio = _AudioDA.getById(ArrID.FirstOrDefault());
                        Base.ApproveLog log = new Base.ApproveLog();
                        log.StatusID = Convert.ToInt32(Request["StatusID"]);
                        log.Description = Request["Description"];
                        log.CreatedDate = DateTime.Now;
                        log.UserID = User.Identity.GetUserId();
                        log.AudioID = _Audio.ID;

                        _Audio.StatusID = log.StatusID;
                        _AudioDA.QNewsDB.ApproveLogs.Add(log);
                        _AudioDA.Save();

                        var status = _AudioDA.QNewsDB.Status.Where(o => o.ID == log.StatusID).FirstOrDefault().Name;

                        msg = new JsonMessage()
                        {
                            Erros = false,
                            ID = _Audio.ID.ToString(),
                            Message = string.Format("Đã thực hiện <strong>{1}</strong> nội dung <b>{0}</b>", _Audio.Title, status)
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