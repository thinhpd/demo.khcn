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
    // AlbumPicture
    internal partial class AlbumPictureConfiguration : EntityTypeConfiguration<AlbumPicture>
    {
        public AlbumPictureConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".AlbumPicture");
            HasKey(x => x.ID);

            Property(x => x.ID).HasColumnName("ID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Title).HasColumnName("Title").IsRequired().HasMaxLength(255);
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(1000);
            Property(x => x.Image).HasColumnName("Image").IsOptional().HasMaxLength(255);
            Property(x => x.CreateDate).HasColumnName("CreateDate").IsRequired();
            Property(x => x.CreateBy).HasColumnName("CreateBy").IsRequired().HasMaxLength(128);
            Property(x => x.ModifyDate).HasColumnName("ModifyDate").IsRequired();
            Property(x => x.ModifyBy).HasColumnName("ModifyBy").IsRequired().HasMaxLength(128);
            Property(x => x.Viewed).HasColumnName("Viewed").IsRequired();
            Property(x => x.StatusID).HasColumnName("StatusID").IsRequired();
            Property(x => x.Source).HasColumnName("Source").IsOptional().HasMaxLength(255);
            Property(x => x.AllowComment).HasColumnName("AllowComment").IsRequired();
            Property(x => x.IsRemoved).HasColumnName("IsRemoved").IsRequired();
            Property(x => x.OldID).HasColumnName("OldID").IsOptional();
            Property(x => x.SourceUrl).HasColumnName("SourceUrl").IsOptional().HasMaxLength(255);
            Property(x => x.AlbumID).HasColumnName("AlbumID").IsRequired();

            // Foreign keys
            HasRequired(a => a.AspNetUser_CreateBy).WithMany(b => b.AlbumPictures_CreateBy).HasForeignKey(c => c.CreateBy); // FK_AlbumPicture_AspNetUsers
            HasRequired(a => a.AspNetUser_ModifyBy).WithMany(b => b.AlbumPictures_ModifyBy).HasForeignKey(c => c.ModifyBy); // FK_AlbumPicture_AspNetUsers1
            HasRequired(a => a.Status).WithMany(b => b.AlbumPictures).HasForeignKey(c => c.StatusID); // FK_AlbumPicture_Status
            HasRequired(a => a.Album).WithMany(b => b.AlbumPictures).HasForeignKey(c => c.AlbumID); // FK_AlbumPicture_Album
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
