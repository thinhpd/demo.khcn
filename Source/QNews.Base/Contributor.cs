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
    // Contributor
    public partial class Contributor
    {
        public int ID { get; set; } // ID (Primary key)
        public string Title { get; set; } // Title
        public string Description { get; set; } // Description

        // Reverse navigation
        public virtual ICollection<Content> Contents_AuthorID { get; set; } // Content.FK_Content_Contributer
        public virtual ICollection<Content> Contents_EditorID { get; set; } // Content.FK_Content_Contributer2
        public virtual ICollection<Content> Contents_PhotoID { get; set; } // Content.FK_Content_Contributer1

        public Contributor()
        {
            Contents_AuthorID = new List<Content>();
            Contents_EditorID = new List<Content>();
            Contents_PhotoID = new List<Content>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
