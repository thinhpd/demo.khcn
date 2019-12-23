
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

namespace QNews.Admin.Controllers
{
    public class PartnerController : BaseController
    {
        PartnerDA _PartnerDA = new PartnerDA("#");
        //Partner Partner = new Partner();
        // GET: Partners
        public ActionResult Index()
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
                        Message = "Bạn phải nhập tên chương trình"
                    };
                }
                else
                {
                    var _PartnerType = _PartnerDA.getByName(SearchValue);
                    if (_PartnerType == null)
                    {
                        msg = new JsonMessage()
                        {
                            Erros = true,
                            Message = "Mục tiêu không tồn tại trong hệ thống"
                        };
                    }
                    else
                    {
                        msg = new JsonMessage()
                        {
                            Erros = false,
                            ID = _PartnerType.ID.ToString(),
                            Message = _PartnerType.Title
                        };
                    }
                }
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else //Nếu xem từ khóa
            {
                string query = Request["query"];
                var ltsResults = _PartnerDA.GetListSimpleByAutoComplete(query, 10);
                AutoCompleteItem ResulValues = new AutoCompleteItem()
                {
                    query = query,
                    data = ltsResults.Select(o => o.ID.ToString()).ToList(),
                    suggestions = ltsResults.Select(o => o.Title).ToList()
                };
                return Json(ResulValues, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ListItems()
        {
            ViewData.Model = _PartnerDA.getListSimpleByRequest(Request);
            ViewBag.PageHtml = _PartnerDA.GridHtmlPage;
            return View();
        }
        public ActionResult AjaxView()
        {
            var _PartnerTypeModel = _PartnerDA.getById(ArrID.FirstOrDefault());
            ViewData.Model = _PartnerTypeModel;
            
            return View();
        }

        public ActionResult AjaxForm()
        {
            var _PartnerTypeModel = new Partner()
            {
                Order = 0,
                Show = true
            };

            if (DoAction == ActionType.Edit)
                _PartnerTypeModel = _PartnerDA.getById(ArrID.FirstOrDefault());

            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(_PartnerTypeModel);
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken()]
        public ActionResult Actions()
        {
            JsonMessage msg = new JsonMessage();
            Partner _Partner = new Partner();
            List<Partner> lts_Partner;
            StringBuilder stbMessage;

            try
            {
                switch (DoAction)
                {
                    case ActionType.Add:
                        if (hasAdd)
                        {
                            UpdateModel(_Partner);

                            //_Partner.Descriptions = _Partner.Descriptions;



                            _PartnerDA.Add(_Partner);
                            _PartnerDA.Save();
                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = _Partner.ID.ToString(),
                                Message = string.Format("Đã thêm mới mục tiêu: <b>{0}</b>", Server.HtmlEncode(_Partner.Title))
                            };
                        }
                        break;

                    case ActionType.Edit:
                        if (hasEdit)
                        {
                            _Partner = _PartnerDA.getById(ArrID.FirstOrDefault());
                            UpdateModel(_Partner);

                            //_Partner.Descriptions = Sanitizer.GetSafeHtmlFragment(_Partner.Descriptions);
                            _PartnerDA.Save();
                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = _Partner.ID.ToString(),
                                Message = string.Format("Đã cập nhật mục tiêu: <b>{0}</b>", Server.HtmlEncode(_Partner.Title))
                            };
                        }
                        break;

                    case ActionType.Delete:
                        if (hasDelete)
                        {
                            lts_Partner = _PartnerDA.getListByArrID(ArrID);
                            stbMessage = new StringBuilder();
                            foreach (var item in lts_Partner)
                            {

                                _PartnerDA.Delete(item);
                                stbMessage.AppendFormat("Đã xóa mục tiêu <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                            }
                            msg.ID = string.Join(",", ArrID);
                            _PartnerDA.Save();
                            msg.Message = stbMessage.ToString();
                        }
                        break;

                    case ActionType.Show:
                        if (hasEdit)
                        {
                            lts_Partner = _PartnerDA.getListByArrID(ArrID);
                            stbMessage = new StringBuilder();
                            foreach (var item in lts_Partner.Where(o => !o.Show))
                            {

                                item.Show = true;
                                stbMessage.AppendFormat("Đã hiển thị mục tiêu <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                            }
                            msg.ID = string.Join(",", ArrID);
                            _PartnerDA.Save();
                            msg.Message = stbMessage.ToString();
                        }
                        break;

                    case ActionType.Hide:
                        if (hasEdit)
                        {
                            lts_Partner = _PartnerDA.getListByArrID(ArrID);
                            stbMessage = new StringBuilder();
                            foreach (var item in lts_Partner.Where(o => o.Show))
                            {
                                item.Show = false;
                                stbMessage.AppendFormat("Đã ẩn mục tiêu <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                            }
                            msg.ID = string.Join(",", ArrID);
                            _PartnerDA.Save();
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