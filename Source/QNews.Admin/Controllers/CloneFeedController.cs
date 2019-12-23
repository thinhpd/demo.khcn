using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QNews.Data;
using QNews.Utils;
using QNews.Base;
using System.Text;
using QNews.Models;
using QNews.Data.Admin;

namespace QNews.Admin.Controllers
{
    /// <summary>
    /// Class sử dụng cho quản lý kênh rss
    /// </summary>
    public class CloneFeedController : BaseController
    {
        Clone_FeedDA noderemoveDA = new Clone_FeedDA("#");

        public ActionResult AutoComplete()
        {
            if (DoAction == ActionType.Add) //Nếu thêm từ khóa
            {

                List<JsonMessage> LtsMsg = new List<JsonMessage>();
                JsonMessage msg;
                string noderemoveValue = Request["Values"];
                if (string.IsNullOrEmpty(noderemoveValue))
                {
                    msg = new JsonMessage()
                    {
                        Erros = true,
                        Message = "Bạn phải nhập Thay thế trong tên miền"
                    };
                    LtsMsg.Add(msg);
                }
                else
                {
                    var noderemoves = noderemoveDA.getByNameArray(noderemoveValue);
                    if (noderemoves == null || noderemoves.Count == 0)
                    {
                        msg = new JsonMessage()
                        {
                            Erros = true,
                            Message = "Tên miền không tồn tại trong hệ thống"
                        };
                        LtsMsg.Add(msg);
                    }
                    else
                    {
                        foreach (var noderemove in noderemoves)
                        {
                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = noderemove.FeedID.ToString(),
                                Message = noderemove.FeedTitle
                            };
                            LtsMsg.Add(msg);
                        }
                    }
                }
                return Json(LtsMsg, JsonRequestBehavior.AllowGet);
            }
            else //Nếu xem
            {
                string query = Request["query"];
                var ltsResults = noderemoveDA.GetListSimpleByAutoComplete(query, 10);
                AutoCompleteItem ResulValues = new AutoCompleteItem()
                {
                    query = query,
                    data = ltsResults.Select(o => o.FeedID.ToString()).ToList(),
                    suggestions = ltsResults.Select(o => o.FeedTitle).ToList()
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
            return View();
        }

        /// <summary>
        /// Load danh sách bản ghi dưới dạng bảng
        /// </summary>
        /// <returns></returns>
        public ActionResult ListItems()
        {
            ViewData.Model = noderemoveDA.getListSimpleByRequest(Request);
            ViewBag.PageHtml = noderemoveDA.GridHtmlPage;
            return View();
        }


        /// <summary>
        /// Trang xem chi tiết trong model
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxView()
        {
            var BankModel = noderemoveDA.getById(ArrID.FirstOrDefault());
            ViewData.Model = BankModel;
            return View();
        }

        /// <summary>
        /// Form dùng cho thêm mới, sửa. Load bằng Ajax dialog
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxForm()
        {
            var BankModel = new Base.Clone_Feed();
            if (DoAction == ActionType.Edit)
                BankModel = noderemoveDA.getById(ArrID.FirstOrDefault());

            CategoryDA categoryDA = new CategoryDA();
            var LtsAllItems = categoryDA.getAllListSimple();
            ViewBag.CategoryParentID = categoryDA.getAllSelectList(LtsAllItems, 0, true);
            ViewBag.AllDomain = new Data.Clone_DomainDA().getListSimpleAll();
            ViewBag.AllType = noderemoveDA.QNewsDB.Clone_Type.ToList(); 
            ViewData.Model = BankModel;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
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
            Clone_Feed noderemove = new Clone_Feed();
            List<Clone_Feed> ltsBankItems;
            StringBuilder stbMessage;

            switch (DoAction)
            {
                case ActionType.Add:
                    if (hasAdd)
                    {
                        UpdateModel(noderemove);
                        noderemove.FeedActive = true;
                        noderemoveDA.Add(noderemove);
                        noderemoveDA.Save();
                        msg = new JsonMessage()
                        {
                            Erros = false,
                            ID = noderemove.FeedID.ToString(),
                            Message = string.Format("Đã thêm mới kênh rss: <b>{0}</b>", Server.HtmlEncode(noderemove.FeedTitle))
                        };
                    }
                    break;

                case ActionType.Edit:
                    if (hasEdit)
                    {
                        noderemove = noderemoveDA.getById(ArrID.FirstOrDefault());
                        UpdateModel(noderemove);
                        noderemoveDA.Save();
                        msg = new JsonMessage()
                        {
                            Erros = false,
                            ID = noderemove.FeedID.ToString(),
                            Message = string.Format("Đã cập nhật kênh rss: <b>{0}</b>", Server.HtmlEncode(noderemove.FeedTitle))
                        };
                    }
                    break;

                case ActionType.Delete:
                    if (hasDelete)
                    {
                        ltsBankItems = noderemoveDA.getListByArrID(ArrID);
                        stbMessage = new StringBuilder();
                        try
                        {
                            foreach (var item in ltsBankItems)
                            {
                                noderemoveDA.Delete(item);
                                stbMessage.AppendFormat("Đã xóa kênh rss <b>{0}</b>.<br />", Server.HtmlEncode(item.FeedTitle));
                            }
                            msg.ID = string.Join(",", ArrID);
                            noderemoveDA.Save();
                        }
                        catch (Exception ex)
                        {
                            stbMessage.Append(ex.Message);
                        }
                        msg.Message = stbMessage.ToString();
                    }
                    break;
            }

            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
