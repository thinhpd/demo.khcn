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
    // DocumentScope
    internal partial class DocumentScopeConfiguration : EntityTypeConfiguration<DocumentScope>
    {
        public DocumentScopeConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".DocumentScope");
            HasKey(x => x.ID);

            Property(x => x.ID).HasColumnName("ID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Title).HasColumnName("Title").IsRequired().HasMaxLength(150);
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(500);
            Property(x => x.Order).HasColumnName("Order").IsRequired();
            Property(x => x.Show).HasColumnName("Show").IsRequired();
            Property(x => x.IsRemoved).HasColumnName("IsRemoved").IsRequired();
            Property(x => x.CreateDate).HasColumnName("CreateDate").IsRequired();
            Property(x => x.CreateBy).HasColumnName("CreateBy").IsRequired().HasMaxLength(128);
            Property(x => x.ModifyDate).HasColumnName("ModifyDate").IsRequired();
            Property(x => x.ModifyBy).HasColumnName("ModifyBy").IsRequired().HasMaxLength(128);
            Property(x => x.Image).HasColumnName("Image").IsOptional().HasMaxLength(200);

            // Foreign keys
            HasRequired(a => a.AspNetUser_CreateBy).WithMany(b => b.DocumentScopes_CreateBy).HasForeignKey(c => c.CreateBy); // FK_DocumentScope_AspNetUsers
            HasRequired(a => a.AspNetUser_ModifyBy).WithMany(b => b.DocumentScopes_ModifyBy).HasForeignKey(c => c.ModifyBy); // FK_DocumentScope_AspNetUsers1
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
