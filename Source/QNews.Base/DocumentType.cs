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
    // DocumentType
    public partial class DocumentType
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

        // Reverse navigation
        public virtual ICollection<Document> Documents { get; set; } // Document.FK_Document_DocumentType

        // Foreign keys
        public virtual AspNetUser AspNetUser_CreateBy { get; set; } // FK_DocumentType_AspNetUsers
        public virtual AspNetUser AspNetUser_ModifyBy { get; set; } // FK_DocumentType_AspNetUsers1

        public DocumentType()
        {
            Show = true;
            IsRemoved = false;
            Documents = new List<Document>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
