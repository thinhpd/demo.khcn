using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QNews.Models
{
    public class IssueItem
    {
        public int ID { get; set; } // ID (Primary key)
        public string Title { get; set; } // Title
        public int Order { get; set; } // Order
        public bool Show { get; set; } // Show

        public string Description { get; set; }
        public DateTime CreateDate { get; set; } // CreateDate
        public string CreateBy { get; set; } // CreateBy
        public DateTime ModifyDate { get; set; } // ModifyDate
        public string ModifyBy { get; set; } // ModifyBy
    }
}
