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
    public partial class QNewsDBContext : DbContext, IQNewsDBContext
    {
        public IDbSet<Advertise> Advertises { get; set; } // Advertise
        public IDbSet<Album> Albums { get; set; } // Album
        public IDbSet<AlbumPicture> AlbumPictures { get; set; } // AlbumPicture
        public IDbSet<AlbumTopic> AlbumTopics { get; set; } // AlbumTopic
        public IDbSet<ApproveLog> ApproveLogs { get; set; } // ApproveLog
        public IDbSet<AspNetRole> AspNetRoles { get; set; } // AspNetRoles
        public IDbSet<AspNetUser> AspNetUsers { get; set; } // AspNetUsers
        public IDbSet<AspNetUserClaim> AspNetUserClaims { get; set; } // AspNetUserClaims
        public IDbSet<AspNetUserLogin> AspNetUserLogins { get; set; } // AspNetUserLogins
        public IDbSet<Audio> Audios { get; set; } // Audio
        public IDbSet<AudioTopic> AudioTopics { get; set; } // AudioTopic
        public IDbSet<Category> Categories { get; set; } // Category
        public IDbSet<Clone_Domain> Clone_Domain { get; set; } // Clone_Domain
        public IDbSet<Clone_Feed> Clone_Feed { get; set; } // Clone_Feed
        public IDbSet<Clone_Remove> Clone_Remove { get; set; } // Clone_Remove
        public IDbSet<Clone_Replace> Clone_Replace { get; set; } // Clone_Replace
        public IDbSet<Clone_Rss> Clone_Rss { get; set; } // Clone_Rss
        public IDbSet<Clone_Type> Clone_Type { get; set; } // Clone_Type
        public IDbSet<Content> Contents { get; set; } // Content
        public IDbSet<Contributor> Contributors { get; set; } // Contributor
        public IDbSet<Document> Documents { get; set; } // Document
        public IDbSet<DocumentIssue> DocumentIssues { get; set; } // DocumentIssue
        public IDbSet<DocumentScope> DocumentScopes { get; set; } // DocumentScope
        public IDbSet<DocumentType> DocumentTypes { get; set; } // DocumentType
        public IDbSet<Event> Events { get; set; } // Event
        public IDbSet<Home_TD> Home_TD { get; set; } // Home_TD
        public IDbSet<Mission> Missions { get; set; } // Mission
        public IDbSet<Partner> Partners { get; set; } // Partner
        public IDbSet<QuickLink> QuickLinks { get; set; } // QuickLink
        public IDbSet<Register> Registers { get; set; } // Register
        public IDbSet<SiteLink> SiteLinks { get; set; } // SiteLink
        public IDbSet<Status> Status { get; set; } // Status
        public IDbSet<Target> Targets { get; set; } // Target
        public IDbSet<TSHT> TSHTs { get; set; } // TSHT
        public IDbSet<TypeOfCategory> TypeOfCategories { get; set; } // TypeOfCategory
        public IDbSet<TypeOfScope> TypeOfScopes { get; set; } // TypeOfScope
        public IDbSet<Url> Urls { get; set; } // Url
        public IDbSet<Version> Versions { get; set; } // Version
        public IDbSet<Video> Videos { get; set; } // Video
        public IDbSet<VideoTopic> VideoTopics { get; set; } // VideoTopic

        static QNewsDBContext()
        {
            Database.SetInitializer<QNewsDBContext>(null);
        }

        public QNewsDBContext()
            : base("Name=QNewsDBContext")
        {
        InitializePartial();
        }

        public QNewsDBContext(string connectionString) : base(connectionString)
        {
        InitializePartial();
        }

        public QNewsDBContext(string connectionString, System.Data.Entity.Infrastructure.DbCompiledModel model) : base(connectionString, model)
        {
        InitializePartial();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new AdvertiseConfiguration());
            modelBuilder.Configurations.Add(new AlbumConfiguration());
            modelBuilder.Configurations.Add(new AlbumPictureConfiguration());
            modelBuilder.Configurations.Add(new AlbumTopicConfiguration());
            modelBuilder.Configurations.Add(new ApproveLogConfiguration());
            modelBuilder.Configurations.Add(new AspNetRoleConfiguration());
            modelBuilder.Configurations.Add(new AspNetUserConfiguration());
            modelBuilder.Configurations.Add(new AspNetUserClaimConfiguration());
            modelBuilder.Configurations.Add(new AspNetUserLoginConfiguration());
            modelBuilder.Configurations.Add(new AudioConfiguration());
            modelBuilder.Configurations.Add(new AudioTopicConfiguration());
            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new Clone_DomainConfiguration());
            modelBuilder.Configurations.Add(new Clone_FeedConfiguration());
            modelBuilder.Configurations.Add(new Clone_RemoveConfiguration());
            modelBuilder.Configurations.Add(new Clone_ReplaceConfiguration());
            modelBuilder.Configurations.Add(new Clone_RssConfiguration());
            modelBuilder.Configurations.Add(new Clone_TypeConfiguration());
            modelBuilder.Configurations.Add(new ContentConfiguration());
            modelBuilder.Configurations.Add(new ContributorConfiguration());
            modelBuilder.Configurations.Add(new DocumentConfiguration());
            modelBuilder.Configurations.Add(new DocumentIssueConfiguration());
            modelBuilder.Configurations.Add(new DocumentScopeConfiguration());
            modelBuilder.Configurations.Add(new DocumentTypeConfiguration());
            modelBuilder.Configurations.Add(new EventConfiguration());
            modelBuilder.Configurations.Add(new Home_TDConfiguration());
            modelBuilder.Configurations.Add(new MissionConfiguration());
            modelBuilder.Configurations.Add(new PartnerConfiguration());
            modelBuilder.Configurations.Add(new QuickLinkConfiguration());
            modelBuilder.Configurations.Add(new RegisterConfiguration());
            modelBuilder.Configurations.Add(new SiteLinkConfiguration());
            modelBuilder.Configurations.Add(new StatusConfiguration());
            modelBuilder.Configurations.Add(new TargetConfiguration());
            modelBuilder.Configurations.Add(new TSHTConfiguration());
            modelBuilder.Configurations.Add(new TypeOfCategoryConfiguration());
            modelBuilder.Configurations.Add(new TypeOfScopeConfiguration());
            modelBuilder.Configurations.Add(new UrlConfiguration());
            modelBuilder.Configurations.Add(new VersionConfiguration());
            modelBuilder.Configurations.Add(new VideoConfiguration());
            modelBuilder.Configurations.Add(new VideoTopicConfiguration());
        OnModelCreatingPartial(modelBuilder);
        }

        public static DbModelBuilder CreateModel(DbModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Configurations.Add(new AdvertiseConfiguration(schema));
            modelBuilder.Configurations.Add(new AlbumConfiguration(schema));
            modelBuilder.Configurations.Add(new AlbumPictureConfiguration(schema));
            modelBuilder.Configurations.Add(new AlbumTopicConfiguration(schema));
            modelBuilder.Configurations.Add(new ApproveLogConfiguration(schema));
            modelBuilder.Configurations.Add(new AspNetRoleConfiguration(schema));
            modelBuilder.Configurations.Add(new AspNetUserConfiguration(schema));
            modelBuilder.Configurations.Add(new AspNetUserClaimConfiguration(schema));
            modelBuilder.Configurations.Add(new AspNetUserLoginConfiguration(schema));
            modelBuilder.Configurations.Add(new AudioConfiguration(schema));
            modelBuilder.Configurations.Add(new AudioTopicConfiguration(schema));
            modelBuilder.Configurations.Add(new CategoryConfiguration(schema));
            modelBuilder.Configurations.Add(new Clone_DomainConfiguration(schema));
            modelBuilder.Configurations.Add(new Clone_FeedConfiguration(schema));
            modelBuilder.Configurations.Add(new Clone_RemoveConfiguration(schema));
            modelBuilder.Configurations.Add(new Clone_ReplaceConfiguration(schema));
            modelBuilder.Configurations.Add(new Clone_RssConfiguration(schema));
            modelBuilder.Configurations.Add(new Clone_TypeConfiguration(schema));
            modelBuilder.Configurations.Add(new ContentConfiguration(schema));
            modelBuilder.Configurations.Add(new ContributorConfiguration(schema));
            modelBuilder.Configurations.Add(new DocumentConfiguration(schema));
            modelBuilder.Configurations.Add(new DocumentIssueConfiguration(schema));
            modelBuilder.Configurations.Add(new DocumentScopeConfiguration(schema));
            modelBuilder.Configurations.Add(new DocumentTypeConfiguration(schema));
            modelBuilder.Configurations.Add(new EventConfiguration(schema));
            modelBuilder.Configurations.Add(new Home_TDConfiguration(schema));
            modelBuilder.Configurations.Add(new MissionConfiguration(schema));
            modelBuilder.Configurations.Add(new PartnerConfiguration(schema));
            modelBuilder.Configurations.Add(new QuickLinkConfiguration(schema));
            modelBuilder.Configurations.Add(new RegisterConfiguration(schema));
            modelBuilder.Configurations.Add(new SiteLinkConfiguration(schema));
            modelBuilder.Configurations.Add(new StatusConfiguration(schema));
            modelBuilder.Configurations.Add(new TargetConfiguration(schema));
            modelBuilder.Configurations.Add(new TSHTConfiguration(schema));
            modelBuilder.Configurations.Add(new TypeOfCategoryConfiguration(schema));
            modelBuilder.Configurations.Add(new TypeOfScopeConfiguration(schema));
            modelBuilder.Configurations.Add(new UrlConfiguration(schema));
            modelBuilder.Configurations.Add(new VersionConfiguration(schema));
            modelBuilder.Configurations.Add(new VideoConfiguration(schema));
            modelBuilder.Configurations.Add(new VideoTopicConfiguration(schema));
            return modelBuilder;
        }

        partial void InitializePartial();
        partial void OnModelCreatingPartial(DbModelBuilder modelBuilder);
    }
}
