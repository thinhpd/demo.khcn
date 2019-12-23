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
    // Content
    public partial class Content
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
        public bool IsRemoved { get; set; } // IsRemoved
        public int? OldID { get; set; } // OldID
        public string SourceUrl { get; set; } // SourceUrl
        public bool IsHot { get; set; } // IsHot
        public int? RssID { get; set; } // RssID
        public string EditorBy { get; set; } // EditorBy
        public string PhotoBy { get; set; } // PhotoBy
        public int? AuthorID { get; set; } // AuthorID
        public int? PhotoID { get; set; } // PhotoID
        public int? EditorID { get; set; } // EditorID
        public int? ScopeID { get; set; } // ScopeID
        public int? TypeOfScopeID { get; set; } // TypeOfScopeID
        public bool IsRight { get; set; } // IsRight
        public bool ShowDate { get; set; } // ShowDate
        public bool ShowOther { get; set; } // ShowOther

        // Reverse navigation
        public virtual ICollection<ApproveLog> ApproveLogs { get; set; } // ApproveLog.FK_ApproveLog_Content
        public virtual ICollection<Category> Categories { get; set; } // Many to many mapping
        public virtual ICollection<Event> Events { get; set; } // Many to many mapping
        public virtual ICollection<Url> Urls { get; set; } // Url.FK_Url_Content
        public virtual ICollection<Version> Versions { get; set; } // Version.FK_Version_Content

        // Foreign keys
        public virtual AspNetUser AspNetUser_CreateBy { get; set; } // FK_Content_AspNetUsers
        public virtual AspNetUser AspNetUser_ModifyBy { get; set; } // FK_Content_AspNetUsers1
        public virtual Clone_Rss Clone_Rss { get; set; } // FK_Content_Clone_Rss
        public virtual Contributor Contributor_AuthorID { get; set; } // FK_Content_Contributer
        public virtual Contributor Contributor_EditorID { get; set; } // FK_Content_Contributer2
        public virtual Contributor Contributor_PhotoID { get; set; } // FK_Content_Contributer1
        public virtual DocumentScope DocumentScope { get; set; } // FK_Content_DocumentScope
        public virtual Status Status { get; set; } // FK_Content_Status
        public virtual TypeOfScope TypeOfScope { get; set; } // FK_Content_TypeOfScope

        public Content()
        {
            Viewed = 0;
            StatusID = 0;
            IsRemoved = false;
            IsHot = false;
            IsRight = false;
            ShowDate = true;
            ShowOther = true;
            ApproveLogs = new List<ApproveLog>();
            Urls = new List<Url>();
            Versions = new List<Version>();
            Categories = new List<Category>();
            Events = new List<Event>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
