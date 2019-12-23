using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HtmlAgilityPack;
using QNews.Models;
using System.Text.RegularExpressions;

namespace QNews.Web.Controllers
{
    public class AjaxController : Controller
    {
        // GET: Ajax
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Weather(int id)
        {
            string urlRequest = string.Format("http://www.nchmf.gov.vn/web/vi-VN/62/0/{0}/map/Default.aspx", id);
            List<WeatherViewModel> LtsItem = new List<WeatherViewModel>();

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(urlRequest);
            string html = doc.DocumentNode.InnerHtml;
            html = Regex.Replace(html, "<!--.*?-->", "", RegexOptions.Singleline);
            doc.LoadHtml(html);
            doc.OptionUseIdAttribute = true;

            var TitleNode = doc.DocumentNode.SelectNodes(".//td[@class='tieude_dubao']").Skip(1).Take(4).ToList();
            var ImageNode = doc.DocumentNode.SelectNodes(".//table[@class='tableMap']/tr/td/img").ToList();
            var NhietDoCaoNhat = doc.DocumentNode.SelectNodes(".//tr[3]/td[@class='tieude_dubao_yeuto_content']").ToList();
            var NhietDoThapNhat = doc.DocumentNode.SelectNodes(".//tr[4]/td[@class='tieude_dubao_yeuto_content']").ToList();
            var MoTa = doc.DocumentNode.SelectNodes(".//td[@class='tieude_dubao_yeuto_content']/div").Take(4).ToList();

            if (TitleNode != null && TitleNode.Count() > 0)
            {
                for (int idx = 0; idx < TitleNode.Count(); idx++)
                {
                    if (!string.IsNullOrEmpty(TitleNode[idx].InnerText))
                    {
                        try
                        {
                            WeatherViewModel item = new WeatherViewModel();
                            item.Title = TitleNode[idx].InnerText;
                            item.Image = ImageNode[idx].Attributes["src"].Value;
                            item.MaxTemper = NhietDoCaoNhat[idx].InnerText;
                            item.MinTemper = NhietDoThapNhat[idx].InnerText;
                            item.Description = MoTa[idx].InnerText;
                            if (!string.IsNullOrEmpty(item.Image) && item.Image.Contains(".gif"))
                                LtsItem.Add(item);
                        }
                        catch { }
                    }
                }
            }

            return View(LtsItem);
        }
    }
}