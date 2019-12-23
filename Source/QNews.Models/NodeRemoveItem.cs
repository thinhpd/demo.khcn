using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QNews.Models
{
    public class NodeRemoveItem
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string DomainName { get; set; }
        public string XpathValue { get; set; }

        public string RemoveIn { get; set; }
    }
}
