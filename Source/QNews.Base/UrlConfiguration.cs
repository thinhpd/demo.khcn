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
    // Url
    internal partial class UrlConfiguration : EntityTypeConfiguration<Url>
    {
        public UrlConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Url");
            HasKey(x => x.UrlID);

            Property(x => x.UrlID).HasColumnName("UrlID").IsRequired().HasMaxLength(255).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.ContentID).HasColumnName("ContentID").IsOptional();
            Property(x => x.CategoryID).HasColumnName("CategoryID").IsOptional();
            Property(x => x.DocumentID).HasColumnName("DocumentID").IsOptional();
            Property(x => x.EventID).HasColumnName("EventID").IsOptional();
            Property(x => x.AlbumID).HasColumnName("AlbumID").IsOptional();
            Property(x => x.VideoID).HasColumnName("VideoID").IsOptional();
            Property(x => x.AudioID).HasColumnName("AudioID").IsOptional();

            // Foreign keys
            HasOptional(a => a.Content).WithMany(b => b.Urls).HasForeignKey(c => c.ContentID); // FK_Url_Content
            HasOptional(a => a.Category).WithMany(b => b.Urls).HasForeignKey(c => c.CategoryID); // FK_Url_Category
            HasOptional(a => a.Document).WithMany(b => b.Urls).HasForeignKey(c => c.DocumentID); // FK_Url_Document
            HasOptional(a => a.Event).WithMany(b => b.Urls).HasForeignKey(c => c.EventID); // FK_Url_Event
            HasOptional(a => a.Album).WithMany(b => b.Urls).HasForeignKey(c => c.AlbumID); // FK_Url_Album
            HasOptional(a => a.Video).WithMany(b => b.Urls).HasForeignKey(c => c.VideoID); // FK_Url_Video
            HasOptional(a => a.Audio).WithMany(b => b.Urls).HasForeignKey(c => c.AudioID); // FK_Url_Audio
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
