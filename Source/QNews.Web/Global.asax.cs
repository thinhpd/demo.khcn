using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Mvc;
using System.Web.WebPages;
using System.Configuration;

namespace QNews.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {

        bool IsMobileByContext(HttpContext context)
        {
            bool IsMobile = context.Request.Browser.IsMobileDevice;
            bool IsOverride = new System.Web.HttpContextWrapper(context).GetOverriddenBrowser().IsMobileDevice;

            if (IsOverride) //Neu la mobile
            {
                return true;
            }
            else
            {
                if (IsMobile)
                    return true;
                else
                    return false;
            }
        }

        public override string GetVaryByCustomString(HttpContext context, string arg)
        {
            var IsMobileCache = IsMobileByContext(context);

            string prefix = context.User.Identity.Name;

            string mobile = (new System.Web.HttpContextWrapper(context).GetOverriddenBrowser().IsMobileDevice) ? "mobile" : "web";

            if (string.IsNullOrEmpty(prefix))
            {
                prefix = mobile + "anonymous";
            }
            else
            {
                prefix = mobile + prefix;
            }

            if (arg == "User")
            {
                return "User=" + prefix;
            }

            return base.GetVaryByCustomString(context, arg);
        }



        protected void Application_Start()
        {

            DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("Mobile")
            {
                ContextCondition = context => context.GetOverriddenBrowser().IsMobileDevice
            });

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            string solrUrl = ConfigurationManager.AppSettings["SOLR_SERVER"];
            SolrNet.Startup.Init<Models.Publishing.SearchItem>(solrUrl);
        }


        //protected void Application_BeginRequest(Object sender, EventArgs e)
        //{
        //    CultureInfo cInf = new CultureInfo("vi-VN", false);
        //    // NOTE: change the culture name en-ZA to whatever culture suits your needs

        //    cInf.DateTimeFormat.DateSeparator = "/";
        //    cInf.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
        //    cInf.DateTimeFormat.LongDatePattern = "dd/MM/yyyy hh:mm:ss tt";

        //    System.Threading.Thread.CurrentThread.CurrentCulture = cInf;
        //    System.Threading.Thread.CurrentThread.CurrentUICulture = cInf;
        //}
    }
}
