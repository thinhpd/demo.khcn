using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QNews.Models
{
   public class AlbumPictureItem
    {
        public int ID { get; set; } // ID (Primary key)
        public string Title { get; set; } // Title
        public string Description { get; set; } // Description
        public string Image { get; set; } // Image
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
        public int? OldID { get; set; } // OldID
        public string SourceUrl { get; set; } // SourceUrl
        public int AlbumID { get; set; } // AlbumID

        public string Album { get; set; }
    }
}
