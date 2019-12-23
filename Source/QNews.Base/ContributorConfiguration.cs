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
    // Contributor
    internal partial class ContributorConfiguration : EntityTypeConfiguration<Contributor>
    {
        public ContributorConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Contributor");
            HasKey(x => x.ID);

            Property(x => x.ID).HasColumnName("ID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Title).HasColumnName("Title").IsRequired().HasMaxLength(255);
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(2000);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
