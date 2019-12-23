using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QNews.Models
{
    public class TargetItem
    {
        
            public int ID { get; set; } // ID
            public string Title { get; set; } // Title
            public string Descriptions { get; set; } // Descriptions
            public string Image { get; set; } // Image
            public int Order { get; set; } // Order
            public bool Show { get; set; } // Show
            public bool IsRemoved { get; set; } // IsRemoved
        
    }
}
