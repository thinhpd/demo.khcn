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
    // ApproveLog
    public partial class ApproveLog
    {
        public int ApproveID { get; set; } // ApproveID (Primary key)
        public string UserID { get; set; } // UserID
        public int StatusID { get; set; } // StatusID
        public DateTime CreatedDate { get; set; } // CreatedDate
        public string Description { get; set; } // Description
        public int? ContentID { get; set; } // ContentID
        public int? EventID { get; set; } // EventID
        public int? AlbumID { get; set; } // AlbumID
        public int? PictureID { get; set; } // PictureID
        public int? DocumentID { get; set; } // DocumentID
        public int? VideoID { get; set; } // VideoID
        public int? AudioID { get; set; } // AudioID
        public int? SiteLinkID { get; set; } // SiteLinkID
        public int? AdvertiseID { get; set; } // AdvertiseID
        public int? MissionID { get; set; } // MissionID

        // Foreign keys
        public virtual Advertise Advertise { get; set; } // FK_ApproveLog_Advertise
        public virtual Album Album { get; set; } // FK_ApproveLog_Album
        public virtual AlbumPicture AlbumPicture { get; set; } // FK_ApproveLog_AlbumPicture
        public virtual AspNetUser AspNetUser { get; set; } // FK_ApproveLog_AspNetUsers
        public virtual AudioTopic AudioTopic { get; set; } // FK_ApproveLog_AudioTopic
        public virtual Content Content { get; set; } // FK_ApproveLog_Content
        public virtual Document Document { get; set; } // FK_ApproveLog_Document
        public virtual Event Event { get; set; } // FK_ApproveLog_Event
        public virtual Mission Mission { get; set; } // FK_ApproveLog_Mission
        public virtual SiteLink SiteLink { get; set; } // FK_ApproveLog_SiteLink
        public virtual Status Status { get; set; } // FK_ApproveLog_Status
        public virtual Video Video { get; set; } // FK_ApproveLog_Video
    }

}
