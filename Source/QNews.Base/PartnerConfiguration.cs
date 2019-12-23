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
    // Partner
    internal partial class PartnerConfiguration : EntityTypeConfiguration<Partner>
    {
        public PartnerConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Partner");
            HasKey(x => x.ID);

            Property(x => x.ID).HasColumnName("ID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Title).HasColumnName("Title").IsOptional().HasMaxLength(250);
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(250);
            Property(x => x.Link).HasColumnName("Link").IsOptional().HasMaxLength(250);
            Property(x => x.Image).HasColumnName("Image").IsOptional().HasMaxLength(250);
            Property(x => x.Order).HasColumnName("Order").IsRequired();
            Property(x => x.Show).HasColumnName("Show").IsRequired();
            Property(x => x.IsRemoved).HasColumnName("IsRemoved").IsRequired();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
