using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QNews.Models
{
    public class RssItem
    {
        public int RssID { get; set; }
        public string RssTitle { get; set; }
        public string RssDomainName { get; set; }
        public string RssSource { get; set; }

        public string RssImage { get; set; }

        public DateTime? RssCreated { get; set; }

        public string RssDescription { get; set; }

        public bool RssActive { get; set; }

        public int RssOrder { get; set; }
    }
}
