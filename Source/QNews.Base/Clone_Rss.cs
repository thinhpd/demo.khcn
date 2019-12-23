// ReSharper disable RedundantUsingDirective
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable RedundantNameQualifier

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
//using DatabaseGeneratedOption = System.ComponentModel.DataAnnotations.DatabaseGeneratedOption;

namespace QNews.Base
{
    // Clone_Rss
    public partial class Clone_Rss
    {
        public int RssID { get; set; } // RssID (Primary key)
        public int RssFeedID { get; set; } // RssFeedID
        public string RssSource { get; set; } // RssSource
        public string RssTitle { get; set; } // RssTitle
        public string RssDescription { get; set; } // RssDescription
        public string RssImage { get; set; } // RssImage
        public DateTime? RssCreated { get; set; } // RssCreated
        public bool RssActive { get; set; } // RssActive
        public int RssOrder { get; set; } // RssOrder

        // Reverse navigation
        public virtual ICollection<Content> Contents { get; set; } // Content.FK_Content_Clone_Rss

        // Foreign keys
        public virtual Clone_Feed Clone_Feed { get; set; } // FK_Clone_Rss_Clone_Feed

        public Clone_Rss()
        {
            Contents = new List<Content>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
