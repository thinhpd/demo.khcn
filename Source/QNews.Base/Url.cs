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
    // Url
    public partial class Url
    {
        public string UrlID { get; set; } // UrlID (Primary key)
        public int? ContentID { get; set; } // ContentID
        public int? CategoryID { get; set; } // CategoryID
        public int? DocumentID { get; set; } // DocumentID
        public int? EventID { get; set; } // EventID
        public int? AlbumID { get; set; } // AlbumID
        public int? VideoID { get; set; } // VideoID
        public int? AudioID { get; set; } // AudioID

        // Foreign keys
        public virtual Album Album { get; set; } // FK_Url_Album
        public virtual Audio Audio { get; set; } // FK_Url_Audio
        public virtual Category Category { get; set; } // FK_Url_Category
        public virtual Content Content { get; set; } // FK_Url_Content
        public virtual Document Document { get; set; } // FK_Url_Document
        public virtual Event Event { get; set; } // FK_Url_Event
        public virtual Video Video { get; set; } // FK_Url_Video
    }

}
