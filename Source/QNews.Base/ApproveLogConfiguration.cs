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
    internal partial class ApproveLogConfiguration : EntityTypeConfiguration<ApproveLog>
    {
        public ApproveLogConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".ApproveLog");
            HasKey(x => x.ApproveID);

            Property(x => x.ApproveID).HasColumnName("ApproveID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.UserID).HasColumnName("UserID").IsRequired().HasMaxLength(128);
            Property(x => x.StatusID).HasColumnName("StatusID").IsRequired();
            Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(1000);
            Property(x => x.ContentID).HasColumnName("ContentID").IsOptional();
            Property(x => x.EventID).HasColumnName("EventID").IsOptional();
            Property(x => x.AlbumID).HasColumnName("AlbumID").IsOptional();
            Property(x => x.PictureID).HasColumnName("PictureID").IsOptional();
            Property(x => x.DocumentID).HasColumnName("DocumentID").IsOptional();
            Property(x => x.VideoID).HasColumnName("VideoID").IsOptional();
            Property(x => x.AudioID).HasColumnName("AudioID").IsOptional();
            Property(x => x.SiteLinkID).HasColumnName("SiteLinkID").IsOptional();
            Property(x => x.AdvertiseID).HasColumnName("AdvertiseID").IsOptional();
            Property(x => x.MissionID).HasColumnName("MissionID").IsOptional();

            // Foreign keys
            HasRequired(a => a.AspNetUser).WithMany(b => b.ApproveLogs).HasForeignKey(c => c.UserID); // FK_ApproveLog_AspNetUsers
            HasRequired(a => a.Status).WithMany(b => b.ApproveLogs).HasForeignKey(c => c.StatusID); // FK_ApproveLog_Status
            HasOptional(a => a.Content).WithMany(b => b.ApproveLogs).HasForeignKey(c => c.ContentID); // FK_ApproveLog_Content
            HasOptional(a => a.Event).WithMany(b => b.ApproveLogs).HasForeignKey(c => c.EventID); // FK_ApproveLog_Event
            HasOptional(a => a.Album).WithMany(b => b.ApproveLogs).HasForeignKey(c => c.AlbumID); // FK_ApproveLog_Album
            HasOptional(a => a.AlbumPicture).WithMany(b => b.ApproveLogs).HasForeignKey(c => c.PictureID); // FK_ApproveLog_AlbumPicture
            HasOptional(a => a.Document).WithMany(b => b.ApproveLogs).HasForeignKey(c => c.DocumentID); // FK_ApproveLog_Document
            HasOptional(a => a.Video).WithMany(b => b.ApproveLogs).HasForeignKey(c => c.VideoID); // FK_ApproveLog_Video
            HasOptional(a => a.AudioTopic).WithMany(b => b.ApproveLogs).HasForeignKey(c => c.AudioID); // FK_ApproveLog_AudioTopic
            HasOptional(a => a.SiteLink).WithMany(b => b.ApproveLogs).HasForeignKey(c => c.SiteLinkID); // FK_ApproveLog_SiteLink
            HasOptional(a => a.Advertise).WithMany(b => b.ApproveLogs).HasForeignKey(c => c.AdvertiseID); // FK_ApproveLog_Advertise
            HasOptional(a => a.Mission).WithMany(b => b.ApproveLogs).HasForeignKey(c => c.MissionID); // FK_ApproveLog_Mission
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
