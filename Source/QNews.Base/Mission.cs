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
    // Mission
    public partial class Mission
    {
        public int ID { get; set; } // ID (Primary key)
        public string MaNhiemVu { get; set; } // MaNhiemVu
        public string TenNhiemVu { get; set; } // TenNhiemVu
        public string ToChucChuTri { get; set; } // ToChucChuTri
        public string ChuNhiemNhiemVu { get; set; } // ChuNhiemNhiemVu
        public DateTime? BatDau { get; set; } // BatDau
        public DateTime? KetThuc { get; set; } // KetThuc
        public string Details { get; set; } // Details
        public DateTime CreateDate { get; set; } // CreateDate
        public string CreateBy { get; set; } // CreateBy
        public DateTime ModifyDate { get; set; } // ModifyDate
        public string ModifyBy { get; set; } // ModifyBy
        public int StatusID { get; set; } // StatusID
        public bool IsRemoved { get; set; } // IsRemoved

        // Reverse navigation
        public virtual ICollection<ApproveLog> ApproveLogs { get; set; } // ApproveLog.FK_ApproveLog_Mission
        public virtual ICollection<DocumentScope> DocumentScopes { get; set; } // Many to many mapping

        // Foreign keys
        public virtual AspNetUser AspNetUser_CreateBy { get; set; } // FK_NhiemVuDaTrienKhai_AspNetUsers
        public virtual AspNetUser AspNetUser_ModifyBy { get; set; } // FK_NhiemVuDaTrienKhai_AspNetUsers1
        public virtual Status Status { get; set; } // FK_Mission_Status

        public Mission()
        {
            CreateDate = System.DateTime.Now;
            StatusID = 1;
            IsRemoved = false;
            ApproveLogs = new List<ApproveLog>();
            DocumentScopes = new List<DocumentScope>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
