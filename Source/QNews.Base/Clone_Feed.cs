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
    // Clone_Feed
    public partial class Clone_Feed
    {
        public int FeedID { get; set; } // FeedID (Primary key)
        public string FeedTitle { get; set; } // FeedTitle
        public string FeedSource { get; set; } // FeedSource
        public bool FeedActive { get; set; } // FeedActive
        public int FeedDomainID { get; set; } // FeedDomainID
        public int FeedCategoryID { get; set; } // FeedCategoryID

        // Reverse navigation
        public virtual ICollection<Clone_Rss> Clone_Rss { get; set; } // Clone_Rss.FK_Clone_Rss_Clone_Feed

        // Foreign keys
        public virtual Category Category { get; set; } // FK_Clone_Feed_Category
        public virtual Clone_Domain Clone_Domain { get; set; } // FK_Clone_Feed_Clone_Domain

        public Clone_Feed()
        {
            FeedActive = true;
            FeedCategoryID = 1;
            Clone_Rss = new List<Clone_Rss>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
