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
    // DocumentType
    internal partial class DocumentTypeConfiguration : EntityTypeConfiguration<DocumentType>
    {
        public DocumentTypeConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".DocumentType");
            HasKey(x => x.ID);

            Property(x => x.ID).HasColumnName("ID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Title).HasColumnName("Title").IsRequired().HasMaxLength(255);
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(500);
            Property(x => x.Order).HasColumnName("Order").IsRequired();
            Property(x => x.Show).HasColumnName("Show").IsRequired();
            Property(x => x.IsRemoved).HasColumnName("IsRemoved").IsRequired();
            Property(x => x.CreateDate).HasColumnName("CreateDate").IsRequired();
            Property(x => x.CreateBy).HasColumnName("CreateBy").IsRequired().HasMaxLength(128);
            Property(x => x.ModifyDate).HasColumnName("ModifyDate").IsRequired();
            Property(x => x.ModifyBy).HasColumnName("ModifyBy").IsRequired().HasMaxLength(128);

            // Foreign keys
            HasRequired(a => a.AspNetUser_CreateBy).WithMany(b => b.DocumentTypes_CreateBy).HasForeignKey(c => c.CreateBy); // FK_DocumentType_AspNetUsers
            HasRequired(a => a.AspNetUser_ModifyBy).WithMany(b => b.DocumentTypes_ModifyBy).HasForeignKey(c => c.ModifyBy); // FK_DocumentType_AspNetUsers1
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
