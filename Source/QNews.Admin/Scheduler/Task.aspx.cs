using QNews.Base;
using Revalee.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QNews.Web.Scheduler
{
    public partial class Task : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Scheduler();
        }

        private DateTimeOffset? previousCallbackTime = null;

        private void Scheduler()
        {
            Load_Feed();

            Uri uri = HttpContext.Current.Request.Url;
            var host = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;
            string urlTaskCallBack = string.Format("{0}/Scheduler/Task.aspx", host);

            DateTimeOffset callbackTime = DateTimeOffset.Now.AddMinutes(1);
            Uri callbackUrl = new Uri(urlTaskCallBack);
            RevaleeRegistrar.ScheduleCallback(callbackTime, callbackUrl);
            previousCallbackTime = callbackTime;
        }
        private void Load_Feed()
        {
            using (Base.QNewsDBContext db = new Base.QNewsDBContext())
            {
                var ltsFeed = (from c in db.Clone_Feed select c).ToList();
                foreach (var item in ltsFeed)
                {
                    Red_Feed(item.FeedSource, item.FeedID);
                }
            }
        }

        private void Red_Feed(string feed, int feedId)
        {
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            wc.Encoding = UTF8Encoding.UTF8;
            string xml = wc.DownloadString(feed);
            xml = "<!DOCTYPE html>" + xml;

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(xml);
            HtmlAgilityPack.HtmlNodeCollection items = doc.DocumentNode.SelectNodes(".//channel/item");
            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    var nodeLink = item.SelectSingleNode(".//guid");
                    if (nodeLink != null)
                    {
                        using (Base.QNewsDBContext db = new Base.QNewsDBContext())
                        {
                            string link = nodeLink.InnerText;
                            if (db.Clone_Rss.Where(o => o.RssSource == link).Count() == 0)
                            {
                                Clone_Rss rss = new Clone_Rss();
                                rss.RssSource = link;
                                rss.RssFeedID = feedId;

                                var titleNode = item.SelectSingleNode(".//title");
                                rss.RssTitle = Utils.StaticClass.RemoveHTMLTag(Server.HtmlDecode(titleNode.InnerText).Replace("]]>", "").Replace("<![CDATA[", ""));

                                var desNode = item.SelectSingleNode(".//description");
                                if (desNode != null)
                                {
                                    rss.RssDescription = Utils.StaticClass.RemoveHTMLTag(Server.HtmlDecode(desNode.InnerText).Replace("]]>", "").Replace("<![CDATA[", ""));

                                    var imgNode = desNode.SelectSingleNode(".//img");

                                    if (imgNode != null)
                                    {
                                        rss.RssImage = imgNode.Attributes["src"].Value;
                                    }
                                    else
                                    {
                                        imgNode = desNode.SelectSingleNode(".//enclosure");
                                        if (imgNode == null)
                                        {
                                            try
                                            {
                                                rss.RssImage = imgNode.Attributes["url"].Value;
                                            }
                                            catch
                                            { }
                                        }
                                    }
                                }

                                var pubNode = item.SelectSingleNode(".//pubdate");
                                if (pubNode != null)
                                {
                                    try
                                    {
                                        rss.RssCreated = Convert.ToDateTime(pubNode.InnerText);
                                    }
                                    catch
                                    {
                                        rss.RssCreated = DateTime.Now;
                                    }
                                }
                                db.Clone_Rss.Add(rss);
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
        }
    }
}