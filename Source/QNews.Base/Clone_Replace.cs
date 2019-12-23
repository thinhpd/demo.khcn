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
    // Clone_Replace
    public partial class Clone_Replace
    {
        public int ID { get; set; } // ID (Primary key)
        public string Title { get; set; } // Title
        public string StringSource { get; set; } // StringSource
        public string StringDest { get; set; } // StringDest
        public int ReplaceIn { get; set; } // ReplaceIn
        public int DomainID { get; set; } // DomainID

        // Foreign keys
        public virtual Clone_Domain Clone_Domain { get; set; } // FK_Clone_Replace_Clone_Domain
        public virtual Clone_Type Clone_Type { get; set; } // FK_Clone_Replace_Clone_Type
    }

}
