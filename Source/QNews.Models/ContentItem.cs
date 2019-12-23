using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QNews.Models
{
    public class ContentItem
    {
        public int ID { get; set; } // ID (Primary key)
        public string Title { get; set; } // Title
        public string Description { get; set; } // Description
        public string Image { get; set; } // Image
        public string Details { get; set; } // Details
        public DateTime? PublishDate { get; set; } // PublishDate
        public DateTime CreateDate { get; set; } // CreateDate
        public string CreateBy { get; set; } // CreateBy
        public string CreateBy_Name { get; set; } // CreateBy
        public DateTime ModifyDate { get; set; } // ModifyDate
        public string ModifyBy { get; set; } // ModifyBy
        public string ModifyBy_Name { get; set; } // ModifyBy
        public int Viewed { get; set; } // Viewed
        public int StatusID { get; set; } // StatusID
        public string Source { get; set; } // Source
        public bool AllowComment { get; set; } // AllowComment
        public bool IsRemoved { get; set; } // IsRemoved
        public string SourceUrl { get; set; } // SourceUrl

        public string EditorBy { get; set; }
        public string PhotoBy { get; set; }

        public string AuthorBy { get; set; }

        public int Version { get; set; }
        public List<string> Categories {get;set;}

        public List<string> Events {get;set;}

        public string Url {get;set;}

        public int CurrentVersion {get;set;}

        public virtual StatusItem Status { get; set; }

        public ContentItem()
        {
            Categories = new List<string>();
            Events = new List<string>();
        }
    }
}
