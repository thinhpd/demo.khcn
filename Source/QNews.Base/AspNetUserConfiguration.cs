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
    // AspNetUsers
    internal partial class AspNetUserConfiguration : EntityTypeConfiguration<AspNetUser>
    {
        public AspNetUserConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".AspNetUsers");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasMaxLength(128).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.UserFullName).HasColumnName("UserFullName").IsOptional().HasMaxLength(256);
            Property(x => x.Sex).HasColumnName("Sex").IsOptional();
            Property(x => x.Position).HasColumnName("Position").IsOptional().HasMaxLength(256);
            Property(x => x.Email).HasColumnName("Email").IsOptional().HasMaxLength(256);
            Property(x => x.EmailConfirmed).HasColumnName("EmailConfirmed").IsRequired();
            Property(x => x.PasswordHash).HasColumnName("PasswordHash").IsOptional().HasMaxLength(256);
            Property(x => x.SecurityStamp).HasColumnName("SecurityStamp").IsOptional().HasMaxLength(256);
            Property(x => x.PhoneNumber).HasColumnName("PhoneNumber").IsOptional().HasMaxLength(256);
            Property(x => x.PhoneNumberConfirmed).HasColumnName("PhoneNumberConfirmed").IsRequired();
            Property(x => x.TwoFactorEnabled).HasColumnName("TwoFactorEnabled").IsRequired();
            Property(x => x.LockoutEndDateUtc).HasColumnName("LockoutEndDateUtc").IsOptional();
            Property(x => x.LockoutEnabled).HasColumnName("LockoutEnabled").IsRequired();
            Property(x => x.AccessFailedCount).HasColumnName("AccessFailedCount").IsRequired();
            Property(x => x.UserName).HasColumnName("UserName").IsRequired().HasMaxLength(256);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
