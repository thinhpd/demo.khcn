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

namespace QNews.Admin.Controllers
{
    /// <summary>
    /// Class sử dụng cho quản lý tên miền
    /// </summary>
    public class CloneDomainController : BaseController
    {
        Clone_DomainDA domainDA = new Clone_DomainDA("#");

        public ActionResult AutoComplete()
        {
            if (DoAction == ActionType.Add) //Nếu thêm từ khóa
            {

                List<JsonMessage> LtsMsg = new List<JsonMessage>();
                JsonMessage msg;
                string domainValue = Request["Values"];
                if (string.IsNullOrEmpty(domainValue))
                {
                    msg = new JsonMessage()
                    {
                        Erros = true,
                        Message = "Bạn phải nhập Địa chỉ domain"
                    };
                    LtsMsg.Add(msg);
                }
                else
                {
                    var domains = domainDA.getByNameArray(domainValue);
                    if (domains == null || domains.Count == 0)
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
                        foreach (var domain in domains)
                        {
                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = domain.ID.ToString(),
                                Message = domain.Title
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
                var ltsResults = domainDA.GetListSimpleByAutoComplete(query, 10);
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
            return View();
        }

        /// <summary>
        /// Load danh sách bản ghi dưới dạng bảng
        /// </summary>
        /// <returns></returns>
        public ActionResult ListItems()
        {
            ViewData.Model = domainDA.getListSimpleByRequest(Request);
            ViewBag.PageHtml = domainDA.GridHtmlPage;
            return View();
        }


        /// <summary>
        /// Trang xem chi tiết trong model
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxView()
        {
            var BankModel = domainDA.getById(ArrID.FirstOrDefault());
            ViewData.Model = BankModel;
            return View();
        }

        /// <summary>
        /// Form dùng cho thêm mới, sửa. Load bằng Ajax dialog
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxForm()
        {
            var BankModel = new Base.Clone_Domain();
            if (DoAction == ActionType.Edit)
                BankModel = domainDA.getById(ArrID.FirstOrDefault());

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
            Clone_Domain domain = new Clone_Domain();
            List<Clone_Domain> ltsBankItems;
            StringBuilder stbMessage;

            switch (DoAction)
            {
                case ActionType.Add:
                    if (hasAdd)
                    {
                        UpdateModel(domain);
                        domainDA.Add(domain);
                        domainDA.Save();
                        msg = new JsonMessage()
                        {
                            Erros = false,
                            ID = domain.ID.ToString(),
                            Message = string.Format("Đã thêm mới tên miền: <b>{0}</b>", Server.HtmlEncode(domain.Title))
                        };
                    }
                    break;

                case ActionType.Edit:
                    if (hasEdit)
                    {
                        domain = domainDA.getById(ArrID.FirstOrDefault());
                        UpdateModel(domain);
                        domainDA.Save();
                        msg = new JsonMessage()
                        {
                            Erros = false,
                            ID = domain.ID.ToString(),
                            Message = string.Format("Đã cập nhật tên miền: <b>{0}</b>", Server.HtmlEncode(domain.Title))
                        };
                    }
                    break;

                case ActionType.Delete:
                    if (hasDelete)
                    {
                        ltsBankItems = domainDA.getListByArrID(ArrID);
                        stbMessage = new StringBuilder();
                        try
                        {
                            foreach (var item in ltsBankItems)
                            {
                                domainDA.Delete(item);
                                stbMessage.AppendFormat("Đã xóa tên miền <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                            }
                            msg.ID = string.Join(",", ArrID);
                            domainDA.Save();
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
