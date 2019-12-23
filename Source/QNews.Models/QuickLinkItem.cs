using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QNews.Models
{
    public partial class QuickLinkItem
    {
        public int ID { get; set; } // ID (Primary key)
        public string Title { get; set; } // Title
        public string Link { get; set; } // Link
        public int Order { get; set; } // Order
        public bool Show { get; set; } // Show

        public QuickLinkItem()
        {
            Order = 0;
            Show = true;
        }
    }
}
