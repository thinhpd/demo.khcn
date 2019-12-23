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
    // Target
    internal partial class TargetConfiguration : EntityTypeConfiguration<Target>
    {
        public TargetConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Target");
            HasKey(x => x.ID);

            Property(x => x.ID).HasColumnName("ID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Title).HasColumnName("Title").IsRequired().HasMaxLength(550);
            Property(x => x.Descriptions).HasColumnName("Descriptions").IsOptional();
            Property(x => x.Image).HasColumnName("Image").IsOptional().HasMaxLength(250);
            Property(x => x.Order).HasColumnName("Order").IsRequired();
            Property(x => x.Show).HasColumnName("Show").IsRequired();
            Property(x => x.IsRemoved).HasColumnName("IsRemoved").IsRequired();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
