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
    // Register
    internal partial class RegisterConfiguration : EntityTypeConfiguration<Register>
    {
        public RegisterConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Register");
            HasKey(x => x.ID);

            Property(x => x.ID).HasColumnName("ID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(250);
            Property(x => x.Email).HasColumnName("Email").IsRequired().HasMaxLength(250);
            Property(x => x.Phone).HasColumnName("Phone").IsRequired().HasMaxLength(15);
            Property(x => x.Scope_ID).HasColumnName("Scope_ID").IsRequired();
            Property(x => x.Scope_Name).HasColumnName("Scope_Name").IsOptional().HasMaxLength(250);
            Property(x => x.CreateDate).HasColumnName("CreateDate").IsRequired();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
