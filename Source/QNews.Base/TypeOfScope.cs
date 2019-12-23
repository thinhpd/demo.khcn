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
    public partial class TypeOfScope
    {
        public int ID { get; set; } // ID (Primary key)
        public string Title { get; set; } // Title
        public int Order { get; set; } // Order

        // Reverse navigation
        public virtual ICollection<Content> Contents { get; set; } // Content.FK_Content_TypeOfScope

        public TypeOfScope()
        {
            Order = 0;
            Contents = new List<Content>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
