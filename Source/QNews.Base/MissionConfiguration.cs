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
    // Mission
    internal partial class MissionConfiguration : EntityTypeConfiguration<Mission>
    {
        public MissionConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Mission");
            HasKey(x => x.ID);

            Property(x => x.ID).HasColumnName("ID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.MaNhiemVu).HasColumnName("MaNhiemVu").IsRequired().HasMaxLength(50);
            Property(x => x.TenNhiemVu).HasColumnName("TenNhiemVu").IsRequired().HasMaxLength(255);
            Property(x => x.ToChucChuTri).HasColumnName("ToChucChuTri").IsOptional().HasMaxLength(255);
            Property(x => x.ChuNhiemNhiemVu).HasColumnName("ChuNhiemNhiemVu").IsOptional().HasMaxLength(255);
            Property(x => x.BatDau).HasColumnName("BatDau").IsOptional();
            Property(x => x.KetThuc).HasColumnName("KetThuc").IsOptional();
            Property(x => x.Details).HasColumnName("Details").IsOptional();
            Property(x => x.CreateDate).HasColumnName("CreateDate").IsRequired();
            Property(x => x.CreateBy).HasColumnName("CreateBy").IsRequired().HasMaxLength(128);
            Property(x => x.ModifyDate).HasColumnName("ModifyDate").IsRequired();
            Property(x => x.ModifyBy).HasColumnName("ModifyBy").IsRequired().HasMaxLength(128);
            Property(x => x.StatusID).HasColumnName("StatusID").IsRequired();
            Property(x => x.IsRemoved).HasColumnName("IsRemoved").IsRequired();

            // Foreign keys
            HasRequired(a => a.AspNetUser_CreateBy).WithMany(b => b.Missions_CreateBy).HasForeignKey(c => c.CreateBy); // FK_NhiemVuDaTrienKhai_AspNetUsers
            HasRequired(a => a.AspNetUser_ModifyBy).WithMany(b => b.Missions_ModifyBy).HasForeignKey(c => c.ModifyBy); // FK_NhiemVuDaTrienKhai_AspNetUsers1
            HasRequired(a => a.Status).WithMany(b => b.Missions).HasForeignKey(c => c.StatusID); // FK_Mission_Status
            HasMany(t => t.DocumentScopes).WithMany(t => t.Missions).Map(m => 
            {
                m.ToTable("MissionInScope", schema);
                m.MapLeftKey("NhiemVuID");
                m.MapRightKey("ScopeID");
            });
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
