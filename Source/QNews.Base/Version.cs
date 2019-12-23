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
    // Version
    public partial class Version
    {
        public int ID { get; set; } // ID (Primary key)
        public string Url { get; set; } // Url
        public string Title { get; set; } // Title
        public string Description { get; set; } // Description
        public string Image { get; set; } // Image
        public string Details { get; set; } // Details
        public DateTime ModifyDate { get; set; } // ModifyDate
        public string ModifyBy { get; set; } // ModifyBy
        public int StatusID { get; set; } // StatusID
        public string Source { get; set; } // Source
        public bool AllowComment { get; set; } // AllowComment
        public bool IsRemoved { get; set; } // IsRemoved
        public int ContentID { get; set; } // ContentID
        public DateTime? PublishDate { get; set; } // PublishDate
        public int Viewed { get; set; } // Viewed
        public bool IsHot { get; set; } // IsHot

        // Reverse navigation
        public virtual ICollection<Category> Categories { get; set; } // Many to many mapping
        public virtual ICollection<Event> Events { get; set; } // Many to many mapping

        // Foreign keys
        public virtual AspNetUser AspNetUser { get; set; } // FK_Version_AspNetUsers
        public virtual Content Content { get; set; } // FK_Version_Content

        public Version()
        {
            StatusID = 0;
            IsRemoved = false;
            Viewed = 0;
            IsHot = false;
            Categories = new List<Category>();
            Events = new List<Event>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
