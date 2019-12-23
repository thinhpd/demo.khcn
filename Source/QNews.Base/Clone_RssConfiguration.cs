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
    // Clone_Rss
    internal partial class Clone_RssConfiguration : EntityTypeConfiguration<Clone_Rss>
    {
        public Clone_RssConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Clone_Rss");
            HasKey(x => x.RssID);

            Property(x => x.RssID).HasColumnName("RssID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.RssFeedID).HasColumnName("RssFeedID").IsRequired();
            Property(x => x.RssSource).HasColumnName("RssSource").IsRequired().HasMaxLength(255);
            Property(x => x.RssTitle).HasColumnName("RssTitle").IsRequired().HasMaxLength(255);
            Property(x => x.RssDescription).HasColumnName("RssDescription").IsOptional().HasMaxLength(1000);
            Property(x => x.RssImage).HasColumnName("RssImage").IsOptional().HasMaxLength(255);
            Property(x => x.RssCreated).HasColumnName("RssCreated").IsOptional();
            Property(x => x.RssActive).HasColumnName("RssActive").IsRequired();
            Property(x => x.RssOrder).HasColumnName("RssOrder").IsRequired();

            // Foreign keys
            HasRequired(a => a.Clone_Feed).WithMany(b => b.Clone_Rss).HasForeignKey(c => c.RssFeedID); // FK_Clone_Rss_Clone_Feed
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
