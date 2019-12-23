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
    // Register
    public partial class Register
    {
        public int ID { get; set; } // ID (Primary key)
        public string Name { get; set; } // Name
        public string Email { get; set; } // Email
        public string Phone { get; set; } // Phone
        public int Scope_ID { get; set; } // Scope_ID
        public string Scope_Name { get; set; } // Scope_Name
        public DateTime CreateDate { get; set; } // CreateDate
    }

}
