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
    // AspNetUsers
    public partial class AspNetUser
    {
        public string Id { get; set; } // Id (Primary key)
        public string UserFullName { get; set; } // UserFullName
        public bool? Sex { get; set; } // Sex
        public string Position { get; set; } // Position
        public string Email { get; set; } // Email
        public bool EmailConfirmed { get; set; } // EmailConfirmed
        public string PasswordHash { get; set; } // PasswordHash
        public string SecurityStamp { get; set; } // SecurityStamp
        public string PhoneNumber { get; set; } // PhoneNumber
        public bool PhoneNumberConfirmed { get; set; } // PhoneNumberConfirmed
        public bool TwoFactorEnabled { get; set; } // TwoFactorEnabled
        public DateTime? LockoutEndDateUtc { get; set; } // LockoutEndDateUtc
        public bool LockoutEnabled { get; set; } // LockoutEnabled
        public int AccessFailedCount { get; set; } // AccessFailedCount
        public string UserName { get; set; } // UserName

        // Reverse navigation
        public virtual ICollection<Advertise> Advertises_CreateBy { get; set; } // Advertise.FK_Advertise_AspNetUsers
        public virtual ICollection<Advertise> Advertises_ModifyBy { get; set; } // Advertise.FK_Advertise_AspNetUsers1
        public virtual ICollection<Album> Albums_CreateBy { get; set; } // Album.FK_Album_AspNetUsers
        public virtual ICollection<Album> Albums_ModifyBy { get; set; } // Album.FK_Album_AspNetUsers1
        public virtual ICollection<AlbumPicture> AlbumPictures_CreateBy { get; set; } // AlbumPicture.FK_AlbumPicture_AspNetUsers
        public virtual ICollection<AlbumPicture> AlbumPictures_ModifyBy { get; set; } // AlbumPicture.FK_AlbumPicture_AspNetUsers1
        public virtual ICollection<AlbumTopic> AlbumTopics_CreateBy { get; set; } // AlbumTopic.FK_AlbumTopic_AspNetUsers
        public virtual ICollection<AlbumTopic> AlbumTopics_ModifyBy { get; set; } // AlbumTopic.FK_AlbumTopic_AspNetUsers1
        public virtual ICollection<ApproveLog> ApproveLogs { get; set; } // ApproveLog.FK_ApproveLog_AspNetUsers
        public virtual ICollection<AspNetRole> AspNetRoles { get; set; } // Many to many mapping
        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; } // AspNetUserClaims.FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; } // Many to many mapping
        public virtual ICollection<Audio> Audios_CreateBy { get; set; } // Audio.FK_Audio_AspNetUsers
        public virtual ICollection<Audio> Audios_ModifyBy { get; set; } // Audio.FK_Audio_AspNetUsers1
        public virtual ICollection<AudioTopic> AudioTopics_CreateBy { get; set; } // AudioTopic.FK_AudioTopic_AspNetUsers
        public virtual ICollection<AudioTopic> AudioTopics_ModifyBy { get; set; } // AudioTopic.FK_AudioTopic_AspNetUsers1
        public virtual ICollection<Content> Contents_CreateBy { get; set; } // Content.FK_Content_AspNetUsers
        public virtual ICollection<Content> Contents_ModifyBy { get; set; } // Content.FK_Content_AspNetUsers1
        public virtual ICollection<Document> Documents_CreateBy { get; set; } // Document.FK_Document_AspNetUsers
        public virtual ICollection<Document> Documents_ModifyBy { get; set; } // Document.FK_Document_AspNetUsers1
        public virtual ICollection<DocumentIssue> DocumentIssues_CreateBy { get; set; } // DocumentIssue.FK_DocumentIssue_AspNetUsers
        public virtual ICollection<DocumentIssue> DocumentIssues_ModifyBy { get; set; } // DocumentIssue.FK_DocumentIssue_AspNetUsers1
        public virtual ICollection<DocumentScope> DocumentScopes_CreateBy { get; set; } // DocumentScope.FK_DocumentScope_AspNetUsers
        public virtual ICollection<DocumentScope> DocumentScopes_ModifyBy { get; set; } // DocumentScope.FK_DocumentScope_AspNetUsers1
        public virtual ICollection<DocumentType> DocumentTypes_CreateBy { get; set; } // DocumentType.FK_DocumentType_AspNetUsers
        public virtual ICollection<DocumentType> DocumentTypes_ModifyBy { get; set; } // DocumentType.FK_DocumentType_AspNetUsers1
        public virtual ICollection<Event> Events_CreateBy { get; set; } // Event.FK_Event_AspNetUsers1
        public virtual ICollection<Event> Events_ModifyBy { get; set; } // Event.FK_Event_AspNetUsers
        public virtual ICollection<Mission> Missions_CreateBy { get; set; } // Mission.FK_NhiemVuDaTrienKhai_AspNetUsers
        public virtual ICollection<Mission> Missions_ModifyBy { get; set; } // Mission.FK_NhiemVuDaTrienKhai_AspNetUsers1
        public virtual ICollection<SiteLink> SiteLinks_CreateBy { get; set; } // SiteLink.FK_SiteLink_AspNetUsers
        public virtual ICollection<SiteLink> SiteLinks_ModifyBy { get; set; } // SiteLink.FK_SiteLink_AspNetUsers1
        public virtual ICollection<Version> Versions { get; set; } // Version.FK_Version_AspNetUsers
        public virtual ICollection<Video> Videos_CreateBy { get; set; } // Video.FK_Video_AspNetUsers
        public virtual ICollection<Video> Videos_ModifyBy { get; set; } // Video.FK_Video_AspNetUsers1
        public virtual ICollection<VideoTopic> VideoTopics_CreateBy { get; set; } // VideoTopic.FK_VideoTopic_AspNetUsers
        public virtual ICollection<VideoTopic> VideoTopics_ModifyBy { get; set; } // VideoTopic.FK_VideoTopic_AspNetUsers1

        public AspNetUser()
        {
            Advertises_CreateBy = new List<Advertise>();
            Advertises_ModifyBy = new List<Advertise>();
            Albums_CreateBy = new List<Album>();
            Albums_ModifyBy = new List<Album>();
            AlbumPictures_CreateBy = new List<AlbumPicture>();
            AlbumPictures_ModifyBy = new List<AlbumPicture>();
            AlbumTopics_CreateBy = new List<AlbumTopic>();
            AlbumTopics_ModifyBy = new List<AlbumTopic>();
            ApproveLogs = new List<ApproveLog>();
            AspNetUserClaims = new List<AspNetUserClaim>();
            AspNetUserLogins = new List<AspNetUserLogin>();
            Audios_CreateBy = new List<Audio>();
            Audios_ModifyBy = new List<Audio>();
            AudioTopics_CreateBy = new List<AudioTopic>();
            AudioTopics_ModifyBy = new List<AudioTopic>();
            Contents_CreateBy = new List<Content>();
            Contents_ModifyBy = new List<Content>();
            Documents_CreateBy = new List<Document>();
            Documents_ModifyBy = new List<Document>();
            DocumentIssues_CreateBy = new List<DocumentIssue>();
            DocumentIssues_ModifyBy = new List<DocumentIssue>();
            DocumentScopes_CreateBy = new List<DocumentScope>();
            DocumentScopes_ModifyBy = new List<DocumentScope>();
            DocumentTypes_CreateBy = new List<DocumentType>();
            DocumentTypes_ModifyBy = new List<DocumentType>();
            Events_CreateBy = new List<Event>();
            Events_ModifyBy = new List<Event>();
            Missions_CreateBy = new List<Mission>();
            Missions_ModifyBy = new List<Mission>();
            SiteLinks_CreateBy = new List<SiteLink>();
            SiteLinks_ModifyBy = new List<SiteLink>();
            Versions = new List<Version>();
            Videos_CreateBy = new List<Video>();
            Videos_ModifyBy = new List<Video>();
            VideoTopics_CreateBy = new List<VideoTopic>();
            VideoTopics_ModifyBy = new List<VideoTopic>();
            AspNetRoles = new List<AspNetRole>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
