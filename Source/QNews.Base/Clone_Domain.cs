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
    // Clone_Domain
    public partial class Clone_Domain
    {
        public int ID { get; set; } // ID (Primary key)
        public string Title { get; set; } // Title
        public string DomainUrl { get; set; } // DomainUrl
        public string XpathTitle { get; set; } // XpathTitle
        public string XpathDescription { get; set; } // XpathDescription
        public string XpathImage { get; set; } // XpathImage
        public string XpathContent { get; set; } // XpathContent
        public string XpathCreated { get; set; } // XpathCreated
        public string DateFormat { get; set; } // DateFormat
        public string DateSplit { get; set; } // DateSplit
        public bool XpathImageInContent { get; set; } // XpathImageInContent

        // Reverse navigation
        public virtual ICollection<Clone_Feed> Clone_Feed { get; set; } // Clone_Feed.FK_Clone_Feed_Clone_Domain
        public virtual ICollection<Clone_Remove> Clone_Remove { get; set; } // Clone_Remove.FK_Clone_Remove_Clone_Domain
        public virtual ICollection<Clone_Replace> Clone_Replace { get; set; } // Clone_Replace.FK_Clone_Replace_Clone_Domain

        public Clone_Domain()
        {
            XpathImageInContent = false;
            Clone_Feed = new List<Clone_Feed>();
            Clone_Remove = new List<Clone_Remove>();
            Clone_Replace = new List<Clone_Replace>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
