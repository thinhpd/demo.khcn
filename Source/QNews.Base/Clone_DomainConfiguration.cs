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
    // Clone_Domain
    internal partial class Clone_DomainConfiguration : EntityTypeConfiguration<Clone_Domain>
    {
        public Clone_DomainConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Clone_Domain");
            HasKey(x => x.ID);

            Property(x => x.ID).HasColumnName("ID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Title).HasColumnName("Title").IsRequired().HasMaxLength(255);
            Property(x => x.DomainUrl).HasColumnName("DomainUrl").IsRequired().HasMaxLength(255);
            Property(x => x.XpathTitle).HasColumnName("XpathTitle").IsRequired().HasMaxLength(255);
            Property(x => x.XpathDescription).HasColumnName("XpathDescription").IsOptional().HasMaxLength(255);
            Property(x => x.XpathImage).HasColumnName("XpathImage").IsOptional().HasMaxLength(255);
            Property(x => x.XpathContent).HasColumnName("XpathContent").IsRequired().HasMaxLength(255);
            Property(x => x.XpathCreated).HasColumnName("XpathCreated").IsOptional().HasMaxLength(255);
            Property(x => x.DateFormat).HasColumnName("DateFormat").IsOptional().HasMaxLength(255);
            Property(x => x.DateSplit).HasColumnName("DateSplit").IsOptional().HasMaxLength(255);
            Property(x => x.XpathImageInContent).HasColumnName("XpathImageInContent").IsRequired();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
