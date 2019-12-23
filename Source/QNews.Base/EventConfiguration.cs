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
    // Event
    internal partial class EventConfiguration : EntityTypeConfiguration<Event>
    {
        public EventConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Event");
            HasKey(x => x.ID);

            Property(x => x.ID).HasColumnName("ID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Title).HasColumnName("Title").IsRequired().HasMaxLength(255);
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(2000);
            Property(x => x.Image).HasColumnName("Image").IsOptional().HasMaxLength(255);
            Property(x => x.Details).HasColumnName("Details").IsOptional();
            Property(x => x.PublishDate).HasColumnName("PublishDate").IsOptional();
            Property(x => x.CreateDate).HasColumnName("CreateDate").IsRequired();
            Property(x => x.CreateBy).HasColumnName("CreateBy").IsRequired().HasMaxLength(128);
            Property(x => x.ModifyDate).HasColumnName("ModifyDate").IsRequired();
            Property(x => x.ModifyBy).HasColumnName("ModifyBy").IsRequired().HasMaxLength(128);
            Property(x => x.Viewed).HasColumnName("Viewed").IsRequired();
            Property(x => x.StatusID).HasColumnName("StatusID").IsRequired();
            Property(x => x.Source).HasColumnName("Source").IsOptional().HasMaxLength(255);
            Property(x => x.AllowComment).HasColumnName("AllowComment").IsRequired();
            Property(x => x.IsRemove).HasColumnName("IsRemove").IsRequired();

            // Foreign keys
            HasRequired(a => a.AspNetUser_CreateBy).WithMany(b => b.Events_CreateBy).HasForeignKey(c => c.CreateBy); // FK_Event_AspNetUsers1
            HasRequired(a => a.AspNetUser_ModifyBy).WithMany(b => b.Events_ModifyBy).HasForeignKey(c => c.ModifyBy); // FK_Event_AspNetUsers
            HasRequired(a => a.Status).WithMany(b => b.Events).HasForeignKey(c => c.StatusID); // FK_Event_Status
            HasMany(t => t.Versions).WithMany(t => t.Events).Map(m => 
            {
                m.ToTable("VersionEvent", schema);
                m.MapLeftKey("EventID");
                m.MapRightKey("VersionID");
            });
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
