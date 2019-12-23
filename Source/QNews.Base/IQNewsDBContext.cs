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
    public interface IQNewsDBContext : IDisposable
    {
        IDbSet<Advertise> Advertises { get; set; } // Advertise
        IDbSet<Album> Albums { get; set; } // Album
        IDbSet<AlbumPicture> AlbumPictures { get; set; } // AlbumPicture
        IDbSet<AlbumTopic> AlbumTopics { get; set; } // AlbumTopic
        IDbSet<ApproveLog> ApproveLogs { get; set; } // ApproveLog
        IDbSet<AspNetRole> AspNetRoles { get; set; } // AspNetRoles
        IDbSet<AspNetUser> AspNetUsers { get; set; } // AspNetUsers
        IDbSet<AspNetUserClaim> AspNetUserClaims { get; set; } // AspNetUserClaims
        IDbSet<AspNetUserLogin> AspNetUserLogins { get; set; } // AspNetUserLogins
        IDbSet<Audio> Audios { get; set; } // Audio
        IDbSet<AudioTopic> AudioTopics { get; set; } // AudioTopic
        IDbSet<Category> Categories { get; set; } // Category
        IDbSet<Clone_Domain> Clone_Domain { get; set; } // Clone_Domain
        IDbSet<Clone_Feed> Clone_Feed { get; set; } // Clone_Feed
        IDbSet<Clone_Remove> Clone_Remove { get; set; } // Clone_Remove
        IDbSet<Clone_Replace> Clone_Replace { get; set; } // Clone_Replace
        IDbSet<Clone_Rss> Clone_Rss { get; set; } // Clone_Rss
        IDbSet<Clone_Type> Clone_Type { get; set; } // Clone_Type
        IDbSet<Content> Contents { get; set; } // Content
        IDbSet<Contributor> Contributors { get; set; } // Contributor
        IDbSet<Document> Documents { get; set; } // Document
        IDbSet<DocumentIssue> DocumentIssues { get; set; } // DocumentIssue
        IDbSet<DocumentScope> DocumentScopes { get; set; } // DocumentScope
        IDbSet<DocumentType> DocumentTypes { get; set; } // DocumentType
        IDbSet<Event> Events { get; set; } // Event
        IDbSet<Home_TD> Home_TD { get; set; } // Home_TD
        IDbSet<Mission> Missions { get; set; } // Mission
        IDbSet<Partner> Partners { get; set; } // Partner
        IDbSet<QuickLink> QuickLinks { get; set; } // QuickLink
        IDbSet<Register> Registers { get; set; } // Register
        IDbSet<SiteLink> SiteLinks { get; set; } // SiteLink
        IDbSet<Status> Status { get; set; } // Status
        IDbSet<Target> Targets { get; set; } // Target
        IDbSet<TSHT> TSHTs { get; set; } // TSHT
        IDbSet<TypeOfCategory> TypeOfCategories { get; set; } // TypeOfCategory
        IDbSet<TypeOfScope> TypeOfScopes { get; set; } // TypeOfScope
        IDbSet<Url> Urls { get; set; } // Url
        IDbSet<Version> Versions { get; set; } // Version
        IDbSet<Video> Videos { get; set; } // Video
        IDbSet<VideoTopic> VideoTopics { get; set; } // VideoTopic

        int SaveChanges();
    }

}
