using QNews.Admin.Controllers;
using System;
using QNews.Base;
using QNews.Data.Admin;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QNews.Web.Controllers
{
    public class HomeDefaultController : BaseController
    {
        QuickLinkDA _Quicklink = new QuickLinkDA();
        QuickLink quickLink = new QuickLink();
        // GET: HomeDefault
        public ActionResult Index()
        {
            quickLink = _Quicklink.getQLLastest();
            var model = quickLink;
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(QuickLink model)
        {
            //_TSHTDA.Add(model);
            _Quicklink.update(model);
            _Quicklink.Save();


            return View(model);
        }
    }
}