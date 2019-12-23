using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QNews.Models
{
    public class AudioItem
    {
        public int ID { get; set; } // ID (Primary key)
        public string Title { get; set; } // Title
        public string Description { get; set; } // Description
        public string Image { get; set; } // Image
        public string Details { get; set; } // Details
        public DateTime? PublishDate { get; set; } // PublishDate
        public DateTime CreateDate { get; set; } // CreateDate
        public string CreateBy { get; set; } // CreateBy
        public DateTime ModifyDate { get; set; } // ModifyDate
        public string ModifyBy { get; set; } // ModifyBy
        public int Viewed { get; set; } // Viewed
        public int StatusID { get; set; } // StatusID

        public string Status { get; set; }
        public string Source { get; set; } // Source
        public bool AllowComment { get; set; } // AllowComment
        public bool IsRemoved { get; set; } // IsRemoved
        public string SourceUrl { get; set; } // SourceUrl

        public string FileAttach { get; set; }
        public bool IsHot { get; set; } // IsHot

        public int? TopicID { get; set; }

        public string Topic { get; set; }

    }
}
