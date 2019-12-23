using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QNews.Data;
using QNews.Base;
using QNews.Data.Admin;
using QNews.Utils;
using System.Text;
using QNews.Models;

namespace QNews.Admin.Controllers
{
    public class TargetsController : BaseController
    {
        TargetDA _targetDA = new TargetDA("#");
        //Target target = new Target();
        // GET: Targets
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
                    var _targetType = _targetDA.getByName(SearchValue);
                    if (_targetType == null)
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
                            ID = _targetType.ID.ToString(),
                            Message = _targetType.Title
                        };
                    }
                }
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else //Nếu xem từ khóa
            {
                string query = Request["query"];
                var ltsResults = _targetDA.GetListSimpleByAutoComplete(query, 10);
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
            ViewData.Model = _targetDA.getListSimpleByRequest(Request);
            ViewBag.PageHtml = _targetDA.GridHtmlPage;
            return View();
        }
        public ActionResult AjaxView()
        {
            var _targetTypeModel = _targetDA.getById(ArrID.FirstOrDefault());
            ViewData.Model = _targetTypeModel;
            
            return View();
        }

        public ActionResult AjaxForm()
        {
            var _targetTypeModel = new Target()
            {
                Order = 0,
                Show = true
            };

            if (DoAction == ActionType.Edit)
                _targetTypeModel = _targetDA.getById(ArrID.FirstOrDefault());

            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(_targetTypeModel);
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken()]
        public ActionResult Actions()
        {
            JsonMessage msg = new JsonMessage();
            Target _target = new Target();
            List<Target> lts_target;
            StringBuilder stbMessage;

            try
            {
                switch (DoAction)
                {
                    case ActionType.Add:
                        if (hasAdd)
                        {
                            UpdateModel(_target);

                            //_target.Descriptions = _target.Descriptions;



                            _targetDA.Add(_target);
                            _targetDA.Save();
                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = _target.ID.ToString(),
                                Message = string.Format("Đã thêm mới mục tiêu: <b>{0}</b>", Server.HtmlEncode(_target.Title))
                            };
                        }
                        break;

                    case ActionType.Edit:
                        if (hasEdit)
                        {
                            _target = _targetDA.getById(ArrID.FirstOrDefault());
                            UpdateModel(_target);

                            //_target.Descriptions = Sanitizer.GetSafeHtmlFragment(_target.Descriptions);
                            _targetDA.Save();
                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = _target.ID.ToString(),
                                Message = string.Format("Đã cập nhật mục tiêu: <b>{0}</b>", Server.HtmlEncode(_target.Title))
                            };
                        }
                        break;

                    case ActionType.Delete:
                        if (hasDelete)
                        {
                            lts_target = _targetDA.getListByArrID(ArrID);
                            stbMessage = new StringBuilder();
                            foreach (var item in lts_target)
                            {

                                _targetDA.Delete(item);
                                stbMessage.AppendFormat("Đã xóa mục tiêu <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                            }
                            msg.ID = string.Join(",", ArrID);
                            _targetDA.Save();
                            msg.Message = stbMessage.ToString();
                        }
                        break;

                    case ActionType.Show:
                        if (hasEdit)
                        {
                            lts_target = _targetDA.getListByArrID(ArrID);
                            stbMessage = new StringBuilder();
                            foreach (var item in lts_target.Where(o => !o.Show))
                            {

                                item.Show = true;
                                stbMessage.AppendFormat("Đã hiển thị mục tiêu <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                            }
                            msg.ID = string.Join(",", ArrID);
                            _targetDA.Save();
                            msg.Message = stbMessage.ToString();
                        }
                        break;

                    case ActionType.Hide:
                        if (hasEdit)
                        {
                            lts_target = _targetDA.getListByArrID(ArrID);
                            stbMessage = new StringBuilder();
                            foreach (var item in lts_target.Where(o => o.Show))
                            {
                                item.Show = false;
                                stbMessage.AppendFormat("Đã ẩn mục tiêu <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                            }
                            msg.ID = string.Join(",", ArrID);
                            _targetDA.Save();
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