using QNews.Admin.Controllers;
using QNews.Base;
using QNews.Data.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QNews.Web.Controllers
{
    public class TSHTController : BaseController
    {
        TSHTDA _TSHTDA = new TSHTDA();
        TSHT tsht = new TSHT();

        // GET: TSHT
        public ActionResult Index()
        {
            tsht= _TSHTDA.getTSHTByKey("footer");
            var model = tsht;

            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(TSHT model)
        {
            //_TSHTDA.Add(model);
            _TSHTDA.update(model);
            _TSHTDA.Save();


            return View(model);
        }
    }
}