using Microsoft.Practices.ServiceLocation;
using QNews.Base;
using SolrNet;
using SolrNet.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QNews.Services
{
    public class IndexService
    {
        DateTime DateStart;
        DateTime DateFinish;

        SolrConnection connection;
        public IndexService()
        {
            string solrServer = ConfigurationManager.AppSettings["SOLR_SERVER"];
            connection = new SolrConnection(solrServer);
            SolrNet.Startup.Init<Models.Publishing.SearchItem>(connection);
        }

        public void ResetIndex()
        {
            //delete all
            SolrNet.Commands.CommitCommand Commit = new SolrNet.Commands.CommitCommand();
            Commit.WaitFlush = null;
            Commit.WaitSearcher = true;
            var solrWorker = ServiceLocator.Current.GetInstance<ISolrOperations<Models.Publishing.SearchItem>>();
            solrWorker.Delete(new SolrQuery("*:*"));
            Commit.Execute(connection);
        }

        public void Index_All(bool isKH = true)
        {
            Console.WriteLine("Start Index Content.!");
            string ContextConnection = "QNewsDBContext";

            SolrNet.Commands.CommitCommand Commit = new SolrNet.Commands.CommitCommand();
            Commit.WaitFlush = null;
            Commit.WaitSearcher = true;
            var solrWorker = ServiceLocator.Current.GetInstance<ISolrOperations<Models.Publishing.SearchItem>>();


            using (QNewsDBContext db = new QNewsDBContext(ContextConnection))
            {
                db.Database.CommandTimeout = 1800;
                int start = 0;
                int row = 5000;
                List<Models.Publishing.SearchItem> ltsItem = new List<Models.Publishing.SearchItem>();
                Models.Publishing.SearchItem search;

                var allCategory = db.Categories.ToList();
                foreach(var item in allCategory)
                {
                    search = new Models.Publishing.SearchItem();
                    search.UrlID = item.Urls.Select(o => o.UrlID).FirstOrDefault();
                    search.UID = search.UrlID;
                    search.Title = item.Name;
                    search.Description = item.Description;
                    search.ContentType = "Category";
                    search.IsRemoved = !item.Show;
                    if (!string.IsNullOrEmpty(search.UID))
                        ltsItem.Add(search);
                }
                solrWorker.AddRange(ltsItem);
                Commit.Execute(connection);
                Console.WriteLine("Commit Index: {0} | {1} --->", start, start + row);
                Thread.Sleep(100);
                start = start + row;

                var allContent = db.Contents.ToList();
                foreach (var item in allContent)
                {
                    search = new Models.Publishing.SearchItem();
                    search.UrlID = item.Urls.Select(o => o.UrlID).FirstOrDefault();
                    search.UID = search.UrlID;
                    search.Title = item.Title;
                    search.Description = item.Description;
                    search.ContentType = "Content";
                    search.IsRemoved = item.IsRemoved || item.StatusID != (int)Utils.WorkFlowStatus.DA_DUYET;
                    search.Details = item.Details;
                    search.CreateDate = item.CreateDate;
                    search.PublishDate = item.PublishDate;
                    if (!string.IsNullOrEmpty(search.UID))
                    ltsItem.Add(search);
                }
                solrWorker.AddRange(ltsItem);
                Commit.Execute(connection);
                Console.WriteLine("Commit Index: {0} | {1} --->", start, start + row);
                Thread.Sleep(100);
                start = start + row;

                var allDocument = db.Documents.ToList();
                foreach (var item in allDocument)
                {
                    search = new Models.Publishing.SearchItem();
                    search.UrlID = "/van-ban/" + item.Urls.Select(o => o.UrlID).FirstOrDefault();
                    search.UID = search.UrlID;
                    search.Title = item.SoKyHieu;
                    search.Description = item.TrichYeu;
                    search.ContentType = "Document";
                    search.IsRemoved = item.IsRemoved || item.StatusID != (int)Utils.WorkFlowStatus.DA_DUYET;
                    search.Details = item.Details;
                    search.CreateDate = item.CreateDate;
                    search.PublishDate = item.NgayBanHanh;
                    if (!string.IsNullOrEmpty(search.UID))
                    ltsItem.Add(search);
                }
                solrWorker.AddRange(ltsItem);
                Commit.Execute(connection);
                Console.WriteLine("Commit Index: {0} | {1} --->", start, start + row);
                Thread.Sleep(100);
                start = start + row;

                var allChuongTrinh = db.DocumentScopes.ToList();
                foreach (var item in allChuongTrinh)
                {
                    search = new Models.Publishing.SearchItem();
                    search.UrlID = string.Format("{0}-{1}", Utils.StaticClass.ConverRewrite(item.Title), item.ID);
                    search.UID = search.UrlID;
                    search.Title = item.Title;
                    search.Description = item.Description;
                    search.ContentType = "Programe";
                    search.IsRemoved = item.IsRemoved;
                    search.Details = item.Contents.Where(o => o.TypeOfScopeID == 1 && o.StatusID == (int)Utils.WorkFlowStatus.DA_DUYET && !o.IsRemoved).OrderByDescending(o => o.PublishDate).ThenByDescending(o => o.CreateDate).Select(o => o.Details).FirstOrDefault();
                    search.CreateDate = item.CreateDate;
                    if (!string.IsNullOrEmpty(search.UID))
                    ltsItem.Add(search);
                }

                      
                solrWorker.AddRange(ltsItem);
                Commit.Execute(connection);
                Console.WriteLine("Commit Index: {0} | {1} --->", start, start + row);
                Thread.Sleep(100);
                start = start + row;
            }
        }


        public void Index_Update()
        {
            #region Lấy về thời gian cũ & cập nhật thời gian mới
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var directory = System.IO.Path.GetDirectoryName(path);
            try
            {
                DateStart = Convert.ToDateTime(System.IO.File.ReadAllText(directory + "\\log.txt"));
            }
            catch
            {
                DateStart = DateTime.Now;
            }

            #endregion

            #region query update
            #endregion

            DateFinish = DateTime.Now;
            System.IO.File.WriteAllText(directory + "\\log.txt", DateFinish.ToString()); //Index xong mới ghi file
        }
    }
}
