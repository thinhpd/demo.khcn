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
    // Clone_Type
    public partial class Clone_Type
    {
        public int ID { get; set; } // ID (Primary key)
        public string Title { get; set; } // Title

        // Reverse navigation
        public virtual ICollection<Clone_Remove> Clone_Remove { get; set; } // Clone_Remove.FK_Clone_Remove_Clone_Type1
        public virtual ICollection<Clone_Replace> Clone_Replace { get; set; } // Clone_Replace.FK_Clone_Replace_Clone_Type

        public Clone_Type()
        {
            Clone_Remove = new List<Clone_Remove>();
            Clone_Replace = new List<Clone_Replace>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
