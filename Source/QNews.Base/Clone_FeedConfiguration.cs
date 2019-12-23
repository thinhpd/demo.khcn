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
    // Clone_Feed
    internal partial class Clone_FeedConfiguration : EntityTypeConfiguration<Clone_Feed>
    {
        public Clone_FeedConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Clone_Feed");
            HasKey(x => x.FeedID);

            Property(x => x.FeedID).HasColumnName("FeedID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.FeedTitle).HasColumnName("FeedTitle").IsRequired().HasMaxLength(255);
            Property(x => x.FeedSource).HasColumnName("FeedSource").IsRequired().HasMaxLength(255);
            Property(x => x.FeedActive).HasColumnName("FeedActive").IsRequired();
            Property(x => x.FeedDomainID).HasColumnName("FeedDomainID").IsRequired();
            Property(x => x.FeedCategoryID).HasColumnName("FeedCategoryID").IsRequired();

            // Foreign keys
            HasRequired(a => a.Clone_Domain).WithMany(b => b.Clone_Feed).HasForeignKey(c => c.FeedDomainID); // FK_Clone_Feed_Clone_Domain
            HasRequired(a => a.Category).WithMany(b => b.Clone_Feed).HasForeignKey(c => c.FeedCategoryID); // FK_Clone_Feed_Category
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
