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
    // Clone_Remove
    public partial class Clone_Remove
    {
        public int ID { get; set; } // ID (Primary key)
        public string Title { get; set; } // Title
        public string XpathValue { get; set; } // XpathValue
        public int DomainID { get; set; } // DomainID
        public int XpathRemoveIn { get; set; } // XpathRemoveIn

        // Foreign keys
        public virtual Clone_Domain Clone_Domain { get; set; } // FK_Clone_Remove_Clone_Domain
        public virtual Clone_Type Clone_Type { get; set; } // FK_Clone_Remove_Clone_Type1
    }

}
