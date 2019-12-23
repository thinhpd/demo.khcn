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
using QNews.Base;
using System.Net;
using System.Text;
using System.Web.Caching;

namespace QNews.Admin
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
        }



        void Application_BeginRequest(Object source, EventArgs e)
        {
            //HttpApplication app = (HttpApplication)source;
            //HttpContext context = app.Context;
            //string host = FirstRequestInitialisation.Initialise(context);
        }

        class FirstRequestInitialisation
        {
            private static string host = null;

            private static Object s_lock = new Object();

            // Initialise only on the first request
            public static string Initialise(HttpContext context)
            {
                if (string.IsNullOrEmpty(host))
                {
                    lock (s_lock)
                    {
                        if (string.IsNullOrEmpty(host))
                        {
                            Uri uri = HttpContext.Current.Request.Url;
                            host = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;
                            string urlTaskCallBack = string.Format("{0}/Scheduler/Task.aspx",host);
                            using (WebClient w = new WebClient())
                            {
                                w.DownloadString(urlTaskCallBack);
                            }
                        }
                    }
                }
                return host;
            }
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
