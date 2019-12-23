using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QNews.Models
{
    public class UserItem
    {
        public string ID { get; set; }
        public string UserName { get; set; }

        public string UserFullName { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public Nullable<bool> Sex { get; set; }
        public string Phone { get; set; }
        public string Discriminator { get; set; }

        public string Roles { get; set; }
    }


    public class CategoryItem
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

        public List<int> AllChild
        {
            get
            {
                if (!string.IsNullOrEmpty(AllID))
                    return AllID.Split(',').Select(o => int.Parse(o)).ToList();
                else
                    return new List<int>();
            }
        }

        public virtual List<CategoryItem> Categories { get; set; } // Category.FK_Category_Category

        public CategoryItem()
        {
            Order = 0;
            Show = true;
            ShowInTab = false;
            ShowInTopMenu = true;
            ShowInHome = 1;
            TypeOfDisplay = 0;
            Categories = new List<CategoryItem>();
        }
    }
}
