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
    // Version
    internal partial class VersionConfiguration : EntityTypeConfiguration<Version>
    {
        public VersionConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Version");
            HasKey(x => x.ID);

            Property(x => x.ID).HasColumnName("ID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Url).HasColumnName("Url").IsRequired().IsUnicode(false).HasMaxLength(255);
            Property(x => x.Title).HasColumnName("Title").IsRequired().HasMaxLength(500);
            Property(x => x.Description).HasColumnName("Description").IsOptional();
            Property(x => x.Image).HasColumnName("Image").IsOptional().HasMaxLength(255);
            Property(x => x.Details).HasColumnName("Details").IsOptional();
            Property(x => x.ModifyDate).HasColumnName("ModifyDate").IsRequired();
            Property(x => x.ModifyBy).HasColumnName("ModifyBy").IsRequired().HasMaxLength(128);
            Property(x => x.StatusID).HasColumnName("StatusID").IsRequired();
            Property(x => x.Source).HasColumnName("Source").IsOptional().HasMaxLength(255);
            Property(x => x.AllowComment).HasColumnName("AllowComment").IsRequired();
            Property(x => x.IsRemoved).HasColumnName("IsRemoved").IsRequired();
            Property(x => x.ContentID).HasColumnName("ContentID").IsRequired();
            Property(x => x.PublishDate).HasColumnName("PublishDate").IsOptional();
            Property(x => x.Viewed).HasColumnName("Viewed").IsRequired();
            Property(x => x.IsHot).HasColumnName("IsHot").IsRequired();

            // Foreign keys
            HasRequired(a => a.AspNetUser).WithMany(b => b.Versions).HasForeignKey(c => c.ModifyBy); // FK_Version_AspNetUsers
            HasRequired(a => a.Content).WithMany(b => b.Versions).HasForeignKey(c => c.ContentID); // FK_Version_Content
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
