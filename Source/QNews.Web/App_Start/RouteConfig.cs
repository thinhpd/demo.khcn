using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QNews.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Default", "", new { controller = "Home", action = "Index" });
            routes.MapRoute("search", "tim-kiem", new { controller = "Home", action = "Search" });

            routes.MapRoute("chuong-trinh-khcn", "chuong-trinh-khcn", new { controller = "Home", action = "IndexChuongTrinh" });
            routes.MapRoute("gioi-thieu", "gioi-thieu", new { controller = "Home", action = "GioiThieu" });
            routes.MapRoute("chuong-trinh-khcn-details", "chuong-trinh-khcn-{ChuongTrinhID}", new { controller = "Home", action = "DetailChuongTrinh" }, new { ChuongTrinhID = @"\d+" });

            routes.MapRoute("chuong-trinh-khcn-thong-tin-chung", "chuong-trinh-khcn/thong-tin-chung-{ScopeID}", new { controller = "Home", action = "Router", TypeOfScope = 1 }, new { ScopeID = @"\d+" });
            routes.MapRoute("chuong-trinh-khcn-cac-nhiem-vu-da-trien-khai", "chuong-trinh-khcn/nhiem-vu-da-trien-khai-{ScopeID}", new { controller = "Home", action = "ListNhiemVu", TypeOfScope = 2 }, new { ScopeID = @"\d+" });
            routes.MapRoute("chuong-trinh-khcn-quy-trinh-tham-gia", "chuong-trinh-khcn/quy-trinh-tham-gia-{ScopeID}", new { controller = "Home", action = "Router", TypeOfScope = 3 }, new { ScopeID = @"\d+" });


            routes.MapRoute("chuong-trinh-khcn-cac-nhiem-vu-da-trien-khai-chitiet", "chuong-trinh-khcn/nhiem-vu-da-trien-khai-{ChuongTrinhID}.{NhiemVuID}", new { controller = "Home", action = "DetailNhiemVu", TypeOfScope = 2 }, new { ChuongTrinhID = @"\d+", NhiemVuID = @"\d+" });


            routes.MapRoute("van-ban", "van-ban", new { controller = "Home", action = "IndexDocument" });
            routes.MapRoute("van-ban_detail", "van-ban/{urlId}", new { controller = "Home", action = "Router" }, new { urlId = @"(.*)" });
            routes.MapRoute("IndexRssRouter", "rss", new { controller = "Home", action = "IndexRss" });
            routes.MapRoute("so-do-website", "so-do-website", new { controller = "Home", action = "Sitemap" });
            routes.MapRoute("lien-he", "lien-he", new { controller = "Home", action = "Contact" });

            routes.MapRoute("RssRouter", "rss/{urlId}", new { controller = "Home", action = "Rss" }, new { urlId = @"(.*)" });
            routes.MapRoute("contentRouter", "{urlId}", new { controller = "Home", action = "Router" }, new { urlId = @"(.*)" });
            routes.MapRoute("home", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = 0 }, new { id = @"\d+" });
            routes.RouteExistingFiles = false;
        }
    }
}
