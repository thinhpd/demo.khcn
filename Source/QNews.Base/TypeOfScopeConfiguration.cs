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
    // TypeOfScope
    internal partial class TypeOfScopeConfiguration : EntityTypeConfiguration<TypeOfScope>
    {
        public TypeOfScopeConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".TypeOfScope");
            HasKey(x => x.ID);

            Property(x => x.ID).HasColumnName("ID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Title).HasColumnName("Title").IsRequired().HasMaxLength(150);
            Property(x => x.Order).HasColumnName("Order").IsRequired();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
