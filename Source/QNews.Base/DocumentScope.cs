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
    // DocumentScope
    public partial class DocumentScope
    {
        public int ID { get; set; } // ID (Primary key)
        public string Title { get; set; } // Title
        public string Description { get; set; } // Description
        public int Order { get; set; } // Order
        public bool Show { get; set; } // Show
        public bool IsRemoved { get; set; } // IsRemoved
        public DateTime CreateDate { get; set; } // CreateDate
        public string CreateBy { get; set; } // CreateBy
        public DateTime ModifyDate { get; set; } // ModifyDate
        public string ModifyBy { get; set; } // ModifyBy
        public string Image { get; set; } // Image

        // Reverse navigation
        public virtual ICollection<Content> Contents { get; set; } // Content.FK_Content_DocumentScope
        public virtual ICollection<Document> Documents { get; set; } // Many to many mapping
        public virtual ICollection<Mission> Missions { get; set; } // Many to many mapping

        // Foreign keys
        public virtual AspNetUser AspNetUser_CreateBy { get; set; } // FK_DocumentScope_AspNetUsers
        public virtual AspNetUser AspNetUser_ModifyBy { get; set; } // FK_DocumentScope_AspNetUsers1

        public DocumentScope()
        {
            Show = true;
            IsRemoved = false;
            Contents = new List<Content>();
            Documents = new List<Document>();
            Missions = new List<Mission>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
