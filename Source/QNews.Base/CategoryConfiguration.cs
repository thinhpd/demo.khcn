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
    // Category
    internal partial class CategoryConfiguration : EntityTypeConfiguration<Category>
    {
        public CategoryConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Category");
            HasKey(x => x.ID);

            Property(x => x.ID).HasColumnName("ID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(100);
            Property(x => x.NameSort).HasColumnName("NameSort").IsOptional().HasMaxLength(100);
            Property(x => x.CurrentUrl).HasColumnName("CurrentUrl").IsOptional().HasMaxLength(255);
            Property(x => x.ParentID).HasColumnName("ParentID").IsRequired();
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(500);
            Property(x => x.Order).HasColumnName("Order").IsRequired();
            Property(x => x.Show).HasColumnName("Show").IsRequired();
            Property(x => x.ShowInTab).HasColumnName("ShowInTab").IsRequired();
            Property(x => x.AllID).HasColumnName("AllID").IsOptional().IsUnicode(false).HasMaxLength(500);
            Property(x => x.Image).HasColumnName("Image").IsOptional().HasMaxLength(255);
            Property(x => x.ShowInTopMenu).HasColumnName("ShowInTopMenu").IsRequired();
            Property(x => x.ShowInHome).HasColumnName("ShowInHome").IsRequired();
            Property(x => x.TypeOfDisplay).HasColumnName("TypeOfDisplay").IsRequired();
            Property(x => x.ShowInRight).HasColumnName("ShowInRight").IsRequired();
            Property(x => x.MapOrder).HasColumnName("MapOrder").IsRequired();

            // Foreign keys
            HasRequired(a => a.Category_ParentID).WithMany(b => b.Categories).HasForeignKey(c => c.ParentID); // FK_Category_Category
            HasRequired(a => a.TypeOfCategory).WithMany(b => b.Categories).HasForeignKey(c => c.TypeOfDisplay); // FK_Category_TypeOfCategory
            HasMany(t => t.Versions).WithMany(t => t.Categories).Map(m => 
            {
                m.ToTable("VersionCategory", schema);
                m.MapLeftKey("CategoryID");
                m.MapRightKey("VersionID");
            });
            HasMany(t => t.Contents).WithMany(t => t.Categories).Map(m => 
            {
                m.ToTable("ContentCategory", schema);
                m.MapLeftKey("CategoryID");
                m.MapRightKey("ContentID");
            });
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
