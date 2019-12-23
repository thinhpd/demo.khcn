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
    // Clone_Replace
    internal partial class Clone_ReplaceConfiguration : EntityTypeConfiguration<Clone_Replace>
    {
        public Clone_ReplaceConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Clone_Replace");
            HasKey(x => x.ID);

            Property(x => x.ID).HasColumnName("ID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Title).HasColumnName("Title").IsRequired().HasMaxLength(255);
            Property(x => x.StringSource).HasColumnName("StringSource").IsRequired().HasMaxLength(255);
            Property(x => x.StringDest).HasColumnName("StringDest").IsOptional().HasMaxLength(255);
            Property(x => x.ReplaceIn).HasColumnName("ReplaceIn").IsRequired();
            Property(x => x.DomainID).HasColumnName("DomainID").IsRequired();

            // Foreign keys
            HasRequired(a => a.Clone_Type).WithMany(b => b.Clone_Replace).HasForeignKey(c => c.ReplaceIn); // FK_Clone_Replace_Clone_Type
            HasRequired(a => a.Clone_Domain).WithMany(b => b.Clone_Replace).HasForeignKey(c => c.DomainID); // FK_Clone_Replace_Clone_Domain
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
