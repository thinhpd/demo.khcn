using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QNews.Models
{
    public class FeedItem
    {
        public int FeedID { get; set; }
        public string FeedTitle { get; set; }
        public string FeedDomainName { get; set; }
        public string FeedSource { get; set; }

        public string CategoryName { get; set; }
        public bool FeedActive { get; set; }

    }
}
