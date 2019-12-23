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
    // TSHT
    internal partial class TSHTConfiguration : EntityTypeConfiguration<TSHT>
    {
        public TSHTConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".TSHT");
            HasKey(x => new { x.ID, x.KeyTSHT });

            Property(x => x.ID).HasColumnName("ID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.KeyTSHT).HasColumnName("KeyTSHT").IsRequired().IsFixedLength().HasMaxLength(10);
            Property(x => x.Value).HasColumnName("Value").IsOptional();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
