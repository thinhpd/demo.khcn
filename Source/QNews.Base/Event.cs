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
    // Event
    public partial class Event
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
        public string Source { get; set; } // Source
        public bool AllowComment { get; set; } // AllowComment
        public bool IsRemove { get; set; } // IsRemove

        // Reverse navigation
        public virtual ICollection<ApproveLog> ApproveLogs { get; set; } // ApproveLog.FK_ApproveLog_Event
        public virtual ICollection<Content> Contents { get; set; } // Many to many mapping
        public virtual ICollection<Url> Urls { get; set; } // Url.FK_Url_Event
        public virtual ICollection<Version> Versions { get; set; } // Many to many mapping

        // Foreign keys
        public virtual AspNetUser AspNetUser_CreateBy { get; set; } // FK_Event_AspNetUsers1
        public virtual AspNetUser AspNetUser_ModifyBy { get; set; } // FK_Event_AspNetUsers
        public virtual Status Status { get; set; } // FK_Event_Status

        public Event()
        {
            Viewed = 0;
            StatusID = 0;
            IsRemove = false;
            ApproveLogs = new List<ApproveLog>();
            Urls = new List<Url>();
            Versions = new List<Version>();
            Contents = new List<Content>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
