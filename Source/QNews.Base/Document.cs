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
    // Document
    public partial class Document
    {
        public int ID { get; set; } // ID (Primary key)
        public string SoKyHieu { get; set; } // SoKyHieu
        public DateTime? NgayBanHanh { get; set; } // NgayBanHanh
        public DateTime? NgayHieuLuc { get; set; } // NgayHieuLuc
        public string TrichYeu { get; set; } // TrichYeu
        public int LoaiVanBanID { get; set; } // LoaiVanBanID
        public string NguoiKy { get; set; } // NguoiKy
        public string FileAttach { get; set; } // FileAttach
        public DateTime CreateDate { get; set; } // CreateDate
        public string CreateBy { get; set; } // CreateBy
        public DateTime ModifyDate { get; set; } // ModifyDate
        public string ModifyBy { get; set; } // ModifyBy
        public int StatusID { get; set; } // StatusID
        public bool IsRemoved { get; set; } // IsRemoved
        public bool AllowComment { get; set; } // AllowComment
        public string Details { get; set; } // Details
        public string CoQuanBanHanh { get; set; } // CoQuanBanHanh

        // Reverse navigation
        public virtual ICollection<ApproveLog> ApproveLogs { get; set; } // ApproveLog.FK_ApproveLog_Document
        public virtual ICollection<DocumentIssue> DocumentIssues { get; set; } // Many to many mapping
        public virtual ICollection<DocumentScope> DocumentScopes { get; set; } // Many to many mapping
        public virtual ICollection<Url> Urls { get; set; } // Url.FK_Url_Document

        // Foreign keys
        public virtual AspNetUser AspNetUser_CreateBy { get; set; } // FK_Document_AspNetUsers
        public virtual AspNetUser AspNetUser_ModifyBy { get; set; } // FK_Document_AspNetUsers1
        public virtual DocumentType DocumentType { get; set; } // FK_Document_DocumentType
        public virtual Status Status { get; set; } // FK_Document_Status

        public Document()
        {
            StatusID = 1;
            IsRemoved = false;
            AllowComment = true;
            ApproveLogs = new List<ApproveLog>();
            Urls = new List<Url>();
            DocumentIssues = new List<DocumentIssue>();
            DocumentScopes = new List<DocumentScope>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
