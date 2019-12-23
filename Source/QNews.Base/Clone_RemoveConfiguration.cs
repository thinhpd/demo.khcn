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
    // Clone_Remove
    internal partial class Clone_RemoveConfiguration : EntityTypeConfiguration<Clone_Remove>
    {
        public Clone_RemoveConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Clone_Remove");
            HasKey(x => x.ID);

            Property(x => x.ID).HasColumnName("ID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Title).HasColumnName("Title").IsRequired().HasMaxLength(255);
            Property(x => x.XpathValue).HasColumnName("XpathValue").IsRequired().HasMaxLength(255);
            Property(x => x.DomainID).HasColumnName("DomainID").IsRequired();
            Property(x => x.XpathRemoveIn).HasColumnName("XpathRemoveIn").IsRequired();

            // Foreign keys
            HasRequired(a => a.Clone_Domain).WithMany(b => b.Clone_Remove).HasForeignKey(c => c.DomainID); // FK_Clone_Remove_Clone_Domain
            HasRequired(a => a.Clone_Type).WithMany(b => b.Clone_Remove).HasForeignKey(c => c.XpathRemoveIn); // FK_Clone_Remove_Clone_Type1
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
