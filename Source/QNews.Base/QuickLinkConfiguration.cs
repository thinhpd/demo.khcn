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
    // QuickLink
    internal partial class QuickLinkConfiguration : EntityTypeConfiguration<QuickLink>
    {
        public QuickLinkConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".QuickLink");
            HasKey(x => x.ID);

            Property(x => x.ID).HasColumnName("ID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Title).HasColumnName("Title").IsRequired().HasMaxLength(255);
            Property(x => x.Link).HasColumnName("Link").IsRequired().HasMaxLength(255);
            Property(x => x.Order).HasColumnName("Order").IsRequired();
            Property(x => x.Show).HasColumnName("Show").IsRequired();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
