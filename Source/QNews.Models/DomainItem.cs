using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QNews.Models
{
    public class DomainItem
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string DomainUrl { get; set; }
        public string XpathTitle { get; set; }
    }
}
