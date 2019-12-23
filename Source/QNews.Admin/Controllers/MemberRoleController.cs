using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using QNews.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace QNews.Admin.Controllers
{
    public class MemberRoleController : BaseController
    {
        //
        // GET: /Admin/MemberRole/
        public ActionResult Index()
        {
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            rm.Create(new IdentityRole()
            {
                Id = new Guid().ToString(),
                Name = "Admin"
            });


            return View();
        }


        /// <summary>
        /// Load danh sách bản ghi dưới dạng bảng
        /// </summary>
        /// <returns></returns>
        public ActionResult ListItems()
        {
            return View();
        }

    }
}