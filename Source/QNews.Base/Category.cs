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
    // Category
    public partial class Category
    {
        public int ID { get; set; } // ID (Primary key)
        public string Name { get; set; } // Name
        public string NameSort { get; set; } // NameSort
        public string CurrentUrl { get; set; } // CurrentUrl
        public int ParentID { get; set; } // ParentID
        public string Description { get; set; } // Description
        public int Order { get; set; } // Order
        public bool Show { get; set; } // Show
        public bool ShowInTab { get; set; } // ShowInTab
        public string AllID { get; set; } // AllID
        public string Image { get; set; } // Image
        public bool ShowInTopMenu { get; set; } // ShowInTopMenu
        public int ShowInHome { get; set; } // ShowInHome
        public int TypeOfDisplay { get; set; } // TypeOfDisplay
        public bool ShowInRight { get; set; } // ShowInRight
        public int MapOrder { get; set; } // MapOrder

        // Reverse navigation
        public virtual ICollection<Category> Categories { get; set; } // Category.FK_Category_Category
        public virtual ICollection<Clone_Feed> Clone_Feed { get; set; } // Clone_Feed.FK_Clone_Feed_Category
        public virtual ICollection<Content> Contents { get; set; } // Many to many mapping
        public virtual ICollection<Url> Urls { get; set; } // Url.FK_Url_Category
        public virtual ICollection<Version> Versions { get; set; } // Many to many mapping

        // Foreign keys
        public virtual Category Category_ParentID { get; set; } // FK_Category_Category
        public virtual TypeOfCategory TypeOfCategory { get; set; } // FK_Category_TypeOfCategory

        public Category()
        {
            Order = 0;
            Show = true;
            ShowInTab = false;
            ShowInTopMenu = true;
            ShowInHome = 1;
            TypeOfDisplay = 0;
            ShowInRight = false;
            MapOrder = 0;
            Categories = new List<Category>();
            Clone_Feed = new List<Clone_Feed>();
            Urls = new List<Url>();
            Versions = new List<Version>();
            Contents = new List<Content>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
