using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QNews.Models
{
    public class ReplaceItem
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string DomainName { get; set; }
        public string StringSource { get; set; }

        public string ReplaceIn { get; set; }
        public string StringDest { get; set; }
    }
}
