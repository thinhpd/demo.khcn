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
    // SiteLink
    public partial class SiteLink
    {
        public int ID { get; set; } // ID (Primary key)
        public string Title { get; set; } // Title
        public string Description { get; set; } // Description
        public int Order { get; set; } // Order
        public string Link { get; set; } // Link
        public DateTime? NgayBatDau { get; set; } // NgayBatDau
        public DateTime? NgayKetThuc { get; set; } // NgayKetThuc
        public DateTime CreateDate { get; set; } // CreateDate
        public string CreateBy { get; set; } // CreateBy
        public DateTime ModifyDate { get; set; } // ModifyDate
        public string ModifyBy { get; set; } // ModifyBy
        public int Viewed { get; set; } // Viewed
        public int StatusID { get; set; } // StatusID
        public bool IsRemoved { get; set; } // IsRemoved

        // Reverse navigation
        public virtual ICollection<ApproveLog> ApproveLogs { get; set; } // ApproveLog.FK_ApproveLog_SiteLink

        // Foreign keys
        public virtual AspNetUser AspNetUser_CreateBy { get; set; } // FK_SiteLink_AspNetUsers
        public virtual AspNetUser AspNetUser_ModifyBy { get; set; } // FK_SiteLink_AspNetUsers1
        public virtual Status Status { get; set; } // FK_SiteLink_Status

        public SiteLink()
        {
            Order = 0;
            Viewed = 0;
            StatusID = 0;
            IsRemoved = false;
            ApproveLogs = new List<ApproveLog>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
