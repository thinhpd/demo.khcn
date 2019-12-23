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
    // TypeOfCategory
    public partial class TypeOfCategory
    {
        public int ID { get; set; } // ID (Primary key)
        public string Name { get; set; } // Name

        // Reverse navigation
        public virtual ICollection<Category> Categories { get; set; } // Category.FK_Category_TypeOfCategory

        public TypeOfCategory()
        {
            Categories = new List<Category>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
