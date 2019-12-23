using SolrNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QNews.Models.Publishing
{
    public class SearchItem
    {
        [SolrField("id")]
        public string UID { get; set; }

     

        [SolrField("ObjectID")]
        public int ID { get; set; }

        [SolrField("Title")]
        public string Title { get; set; }

        [SolrField("Description")]
        public string Description { get; set; }

        [SolrField("Details")]
        public string Details { get; set; }

        [SolrField("UrlID")]
        public string UrlID { get; set; }


        [SolrField("IsRemoved")]
        public bool IsRemoved { get; set; }


        [SolrField("CreateDate")]
        public DateTime CreateDate { get; set; }

        [SolrField("PublishDate")]
        public DateTime? PublishDate { get; set; }

        [SolrField("ExpriedDate")]
        public DateTime? ExpriedDate { get; set; }


        [SolrField("ModifyDate")]
        public DateTime ModifyDate { get; set; }


        [SolrField("StatusID")]
        public int StatusID { get; set; }


        [SolrField("ContentID")]
        public int ContentID { get; set; }


        [SolrField("ContentType")]
        public string ContentType { get; set; }
    }
}
