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
    // Album
    public partial class Album
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
        public int? TopicID { get; set; } // TopicID
        public bool IsHot { get; set; } // IsHot

        // Reverse navigation
        public virtual ICollection<AlbumPicture> AlbumPictures { get; set; } // AlbumPicture.FK_AlbumPicture_Album
        public virtual ICollection<ApproveLog> ApproveLogs { get; set; } // ApproveLog.FK_ApproveLog_Album
        public virtual ICollection<Url> Urls { get; set; } // Url.FK_Url_Album

        // Foreign keys
        public virtual AlbumTopic AlbumTopic { get; set; } // FK_Album_AlbumTopic
        public virtual AspNetUser AspNetUser_CreateBy { get; set; } // FK_Album_AspNetUsers
        public virtual AspNetUser AspNetUser_ModifyBy { get; set; } // FK_Album_AspNetUsers1
        public virtual Status Status { get; set; } // FK_Album_Status

        public Album()
        {
            Viewed = 0;
            StatusID = 0;
            IsRemoved = false;
            IsHot = false;
            AlbumPictures = new List<AlbumPicture>();
            ApproveLogs = new List<ApproveLog>();
            Urls = new List<Url>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
