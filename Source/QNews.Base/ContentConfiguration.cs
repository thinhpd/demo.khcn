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
    // Content
    internal partial class ContentConfiguration : EntityTypeConfiguration<Content>
    {
        public ContentConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Content");
            HasKey(x => x.ID);

            Property(x => x.ID).HasColumnName("ID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Title).HasColumnName("Title").IsRequired().HasMaxLength(500);
            Property(x => x.Description).HasColumnName("Description").IsOptional();
            Property(x => x.Image).HasColumnName("Image").IsOptional().HasMaxLength(500);
            Property(x => x.Details).HasColumnName("Details").IsOptional().HasMaxLength(1073741823);
            Property(x => x.PublishDate).HasColumnName("PublishDate").IsOptional();
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
            Property(x => x.IsHot).HasColumnName("IsHot").IsRequired();
            Property(x => x.RssID).HasColumnName("RssID").IsOptional();
            Property(x => x.EditorBy).HasColumnName("EditorBy").IsOptional().HasMaxLength(150);
            Property(x => x.PhotoBy).HasColumnName("PhotoBy").IsOptional().HasMaxLength(150);
            Property(x => x.AuthorID).HasColumnName("AuthorID").IsOptional();
            Property(x => x.PhotoID).HasColumnName("PhotoID").IsOptional();
            Property(x => x.EditorID).HasColumnName("EditorID").IsOptional();
            Property(x => x.ScopeID).HasColumnName("ScopeID").IsOptional();
            Property(x => x.TypeOfScopeID).HasColumnName("TypeOfScopeID").IsOptional();
            Property(x => x.IsRight).HasColumnName("IsRight").IsRequired();
            Property(x => x.ShowDate).HasColumnName("ShowDate").IsRequired();
            Property(x => x.ShowOther).HasColumnName("ShowOther").IsRequired();

            // Foreign keys
            HasRequired(a => a.AspNetUser_CreateBy).WithMany(b => b.Contents_CreateBy).HasForeignKey(c => c.CreateBy); // FK_Content_AspNetUsers
            HasRequired(a => a.AspNetUser_ModifyBy).WithMany(b => b.Contents_ModifyBy).HasForeignKey(c => c.ModifyBy); // FK_Content_AspNetUsers1
            HasRequired(a => a.Status).WithMany(b => b.Contents).HasForeignKey(c => c.StatusID); // FK_Content_Status
            HasOptional(a => a.Clone_Rss).WithMany(b => b.Contents).HasForeignKey(c => c.RssID); // FK_Content_Clone_Rss
            HasOptional(a => a.Contributor_AuthorID).WithMany(b => b.Contents_AuthorID).HasForeignKey(c => c.AuthorID); // FK_Content_Contributer
            HasOptional(a => a.Contributor_PhotoID).WithMany(b => b.Contents_PhotoID).HasForeignKey(c => c.PhotoID); // FK_Content_Contributer1
            HasOptional(a => a.Contributor_EditorID).WithMany(b => b.Contents_EditorID).HasForeignKey(c => c.EditorID); // FK_Content_Contributer2
            HasOptional(a => a.DocumentScope).WithMany(b => b.Contents).HasForeignKey(c => c.ScopeID); // FK_Content_DocumentScope
            HasOptional(a => a.TypeOfScope).WithMany(b => b.Contents).HasForeignKey(c => c.TypeOfScopeID); // FK_Content_TypeOfScope
            HasMany(t => t.Events).WithMany(t => t.Contents).Map(m => 
            {
                m.ToTable("ContentEvent", schema);
                m.MapLeftKey("ContentID");
                m.MapRightKey("EventID");
            });
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
