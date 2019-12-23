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
    // Status
    public partial class Status
    {
        public int ID { get; set; } // ID (Primary key)
        public string Name { get; set; } // Name

        // Reverse navigation
        public virtual ICollection<Advertise> Advertises { get; set; } // Advertise.FK_Advertise_Status
        public virtual ICollection<Album> Albums { get; set; } // Album.FK_Album_Status
        public virtual ICollection<AlbumPicture> AlbumPictures { get; set; } // AlbumPicture.FK_AlbumPicture_Status
        public virtual ICollection<ApproveLog> ApproveLogs { get; set; } // ApproveLog.FK_ApproveLog_Status
        public virtual ICollection<Audio> Audios { get; set; } // Audio.FK_Audio_Status
        public virtual ICollection<Content> Contents { get; set; } // Content.FK_Content_Status
        public virtual ICollection<Document> Documents { get; set; } // Document.FK_Document_Status
        public virtual ICollection<Event> Events { get; set; } // Event.FK_Event_Status
        public virtual ICollection<Mission> Missions { get; set; } // Mission.FK_Mission_Status
        public virtual ICollection<SiteLink> SiteLinks { get; set; } // SiteLink.FK_SiteLink_Status
        public virtual ICollection<Video> Videos { get; set; } // Video.FK_Video_Status

        public Status()
        {
            Advertises = new List<Advertise>();
            Albums = new List<Album>();
            AlbumPictures = new List<AlbumPicture>();
            ApproveLogs = new List<ApproveLog>();
            Audios = new List<Audio>();
            Contents = new List<Content>();
            Documents = new List<Document>();
            Events = new List<Event>();
            Missions = new List<Mission>();
            SiteLinks = new List<SiteLink>();
            Videos = new List<Video>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
