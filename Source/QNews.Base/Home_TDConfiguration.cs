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
    // Home_TD
    internal partial class Home_TDConfiguration : EntityTypeConfiguration<Home_TD>
    {
        public Home_TDConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Home_TD");
            HasKey(x => x.ID);

            Property(x => x.ID).HasColumnName("ID").IsRequired();
            Property(x => x.Title).HasColumnName("Title").IsOptional().HasMaxLength(250);
            Property(x => x.Link).HasColumnName("Link").IsOptional().HasMaxLength(150);
            Property(x => x.Order).HasColumnName("Order").IsOptional();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
