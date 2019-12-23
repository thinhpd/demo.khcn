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
    // Advertise
    internal partial class AdvertiseConfiguration : EntityTypeConfiguration<Advertise>
    {
        public AdvertiseConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Advertise");
            HasKey(x => x.ID);

            Property(x => x.ID).HasColumnName("ID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Title).HasColumnName("Title").IsRequired().HasMaxLength(255);
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(1000);
            Property(x => x.Image).HasColumnName("Image").IsOptional().HasMaxLength(255);
            Property(x => x.ZoneID).HasColumnName("ZoneID").IsRequired();
            Property(x => x.Order).HasColumnName("Order").IsRequired();
            Property(x => x.Link).HasColumnName("Link").IsOptional().HasMaxLength(255);
            Property(x => x.NgayBatDau).HasColumnName("NgayBatDau").IsOptional();
            Property(x => x.NgayKetThuc).HasColumnName("NgayKetThuc").IsOptional();
            Property(x => x.CreateDate).HasColumnName("CreateDate").IsRequired();
            Property(x => x.CreateBy).HasColumnName("CreateBy").IsRequired().HasMaxLength(128);
            Property(x => x.ModifyDate).HasColumnName("ModifyDate").IsRequired();
            Property(x => x.ModifyBy).HasColumnName("ModifyBy").IsRequired().HasMaxLength(128);
            Property(x => x.Viewed).HasColumnName("Viewed").IsRequired();
            Property(x => x.StatusID).HasColumnName("StatusID").IsRequired();
            Property(x => x.IsRemoved).HasColumnName("IsRemoved").IsRequired();

            // Foreign keys
            HasRequired(a => a.AspNetUser_CreateBy).WithMany(b => b.Advertises_CreateBy).HasForeignKey(c => c.CreateBy); // FK_Advertise_AspNetUsers
            HasRequired(a => a.AspNetUser_ModifyBy).WithMany(b => b.Advertises_ModifyBy).HasForeignKey(c => c.ModifyBy); // FK_Advertise_AspNetUsers1
            HasRequired(a => a.Status).WithMany(b => b.Advertises).HasForeignKey(c => c.StatusID); // FK_Advertise_Status
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
