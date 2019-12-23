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
    // Document
    internal partial class DocumentConfiguration : EntityTypeConfiguration<Document>
    {
        public DocumentConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Document");
            HasKey(x => x.ID);

            Property(x => x.ID).HasColumnName("ID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.SoKyHieu).HasColumnName("SoKyHieu").IsRequired().HasMaxLength(100);
            Property(x => x.NgayBanHanh).HasColumnName("NgayBanHanh").IsOptional();
            Property(x => x.NgayHieuLuc).HasColumnName("NgayHieuLuc").IsOptional();
            Property(x => x.TrichYeu).HasColumnName("TrichYeu").IsOptional().HasMaxLength(500);
            Property(x => x.LoaiVanBanID).HasColumnName("LoaiVanBanID").IsRequired();
            Property(x => x.NguoiKy).HasColumnName("NguoiKy").IsOptional().HasMaxLength(50);
            Property(x => x.FileAttach).HasColumnName("FileAttach").IsOptional();
            Property(x => x.CreateDate).HasColumnName("CreateDate").IsRequired();
            Property(x => x.CreateBy).HasColumnName("CreateBy").IsRequired().HasMaxLength(128);
            Property(x => x.ModifyDate).HasColumnName("ModifyDate").IsRequired();
            Property(x => x.ModifyBy).HasColumnName("ModifyBy").IsRequired().HasMaxLength(128);
            Property(x => x.StatusID).HasColumnName("StatusID").IsRequired();
            Property(x => x.IsRemoved).HasColumnName("IsRemoved").IsRequired();
            Property(x => x.AllowComment).HasColumnName("AllowComment").IsRequired();
            Property(x => x.Details).HasColumnName("Details").IsOptional();
            Property(x => x.CoQuanBanHanh).HasColumnName("CoQuanBanHanh").IsOptional().HasMaxLength(100);

            // Foreign keys
            HasRequired(a => a.DocumentType).WithMany(b => b.Documents).HasForeignKey(c => c.LoaiVanBanID); // FK_Document_DocumentType
            HasRequired(a => a.AspNetUser_CreateBy).WithMany(b => b.Documents_CreateBy).HasForeignKey(c => c.CreateBy); // FK_Document_AspNetUsers
            HasRequired(a => a.AspNetUser_ModifyBy).WithMany(b => b.Documents_ModifyBy).HasForeignKey(c => c.ModifyBy); // FK_Document_AspNetUsers1
            HasRequired(a => a.Status).WithMany(b => b.Documents).HasForeignKey(c => c.StatusID); // FK_Document_Status
            HasMany(t => t.DocumentIssues).WithMany(t => t.Documents).Map(m => 
            {
                m.ToTable("DocumentInIssue", schema);
                m.MapLeftKey("DocumentID");
                m.MapRightKey("IssueID");
            });
            HasMany(t => t.DocumentScopes).WithMany(t => t.Documents).Map(m => 
            {
                m.ToTable("DocumentInScope", schema);
                m.MapLeftKey("DocumentID");
                m.MapRightKey("ScopeID");
            });
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
