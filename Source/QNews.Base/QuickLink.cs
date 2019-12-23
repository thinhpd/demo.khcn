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
    // QuickLink
    public partial class QuickLink
    {
        public int ID { get; set; } // ID (Primary key)
        public string Title { get; set; } // Title
        public string Link { get; set; } // Link
        public int Order { get; set; } // Order
        public bool Show { get; set; } // Show

        public QuickLink()
        {
            Order = 0;
            Show = true;
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
