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
    // Target
    public partial class Target
    {
        public int ID { get; set; } // ID (Primary key)
        public string Title { get; set; } // Title
        public string Descriptions { get; set; } // Descriptions
        public string Image { get; set; } // Image
        public int Order { get; set; } // Order
        public bool Show { get; set; } // Show
        public bool IsRemoved { get; set; } // IsRemoved
    }

}
