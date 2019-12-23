using QNews.Base;
using QNews.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QNews.Data.Admin
{
    public class CategoryDA: BaseDA
    {
        #region Constructer
        public CategoryDA()
        { 
        }

        public CategoryDA(string pathPaging)
        {
            this.PathPaging = pathPaging;
        }

        public CategoryDA(string pathPaging, string pathPagingExt)
        {
            this.PathPaging = pathPaging;
            this.PathPagingext = pathPagingExt;
        }
        #endregion

        #region các function lấy đệ quy


        public List<CategoryItem> getListMap(List<CategoryItem> LtsSource, int CategoryID)
        {
            List<CategoryItem> LtsMap = new List<CategoryItem>();
            CategoryItem Parent = LtsSource.Where(o => o.ID == CategoryID).FirstOrDefault();
            LtsMap.Add(Parent);
            while (true)
            {
                Parent = LtsSource.Where(o => o.ID == Parent.ParentID).FirstOrDefault();
                if (Parent == null)
                    break;
                LtsMap.Insert(0, Parent);
            }
            return LtsMap;
        }



        public void getAllIdCategoryFromRoot(List<CategoryItem> LtsSource, int CategoryID, List<int> LtsItemID)
        {
            IEnumerable<CategoryItem> tempCategory = LtsSource.OrderBy(o => o.Order).Where(m => m.ParentID == CategoryID && m.ID > 0);
            foreach (CategoryItem category in tempCategory)
            {
                LtsItemID.Add(category.ID);
                var countQuery = LtsSource.Where(m => m.ParentID == category.ID && m.ID > 0);
                if (countQuery.Count() > 0)
                {
                    getAllIdCategoryFromRoot(LtsSource, category.ID, LtsItemID);
                }
            }
        }

        /// <summary>
        /// Lấy về cây có tổ chức
        /// </summary>
        /// <param name="LtsSource">Toàn bộ danh mục</param>
        /// <param name="IDRemove">ID danh mục select</param>
        /// <returns></returns>
        public List<CategoryItem> getAllSelectList(List<CategoryItem> LtsSource, int IDRemove, bool checkShow)
        {
            if (checkShow)
                LtsSource = LtsSource.Where(c => c.Show).ToList();
            List<CategoryItem> LtsConvert = new List<CategoryItem>();
            LtsConvert.Add(new CategoryItem()
            {
                ID = 0,
                Name = "Thư mục gốc"
            });

            BuildTreeListItem(LtsSource, 0, string.Empty, IDRemove , ref LtsConvert);
            return LtsConvert;
        }

        public List<Base.TypeOfCategory> getAllTypeOfCategory()
        {
            return QNewsDB.TypeOfCategories.ToList();
        }

        /// <summary>
        /// Build cây đệ quy
        /// </summary>
        /// <param name="LtsItems"></param>
        /// <param name="RootID"></param>
        /// <param name="space"></param>
        /// <param name="LtsConvert"></param>
        private void BuildTreeListItem(List<CategoryItem> LtsItems, int RootID, string space, int IDRemove, ref List<CategoryItem> LtsConvert)
        {
            space += "---";
            var LtsChils = LtsItems.Where(o => o.ParentID == RootID && o.ID != IDRemove).OrderBy(o => o.Order).ToList();
            foreach (var currentItem in LtsChils)
            {
                currentItem.Name = string.Format("|{0} {1}", space, currentItem.Name);
                LtsConvert.Add(currentItem);
                BuildTreeListItem(LtsItems, currentItem.ID, space, IDRemove, ref LtsConvert);
            }
        }

        /// <summary>
        /// Hàm build ra treeview có checkbox chứa danh sách category
        /// </summary>
        /// <param name="MaDonVi"></param>
        public void BuildTreeViewCheckBox(List<CategoryItem> LtsSource, int ID, bool CheckShow, List<int> LtsValues, ref StringBuilder TreeViewHtml)
        {
            IEnumerable<CategoryItem> tempCategory = LtsSource.OrderBy(o => o.Order).Where(m => m.ParentID == ID && m.ID > 0);
            if (CheckShow)
                tempCategory = tempCategory.Where(m => m.Show == CheckShow);

            foreach (CategoryItem category in tempCategory)
            {
                var countQuery = LtsSource.Where(m => m.ParentID == category.ID && m.ID > 0);
                if (CheckShow)
                    countQuery = countQuery.Where(m => m.Show == CheckShow);
                int totalChild = countQuery.Count();
                if (totalChild > 0)
                {
                    TreeViewHtml.Append("<li title=\"" + category.Description + "\" public partial class=\"unselect\" id=\"" + category.ID.ToString() + "\"><span public partial class=\"folder\"> <input id=\"Category_" + category.ID + "\" name=\"Category_" + category.ID + "\" value=\"" + category.ID + "\" type=\"checkbox\" title=\"" + category.Name + "\" " + (LtsValues.Contains(category.ID) ? " checked" : string.Empty) + "/> ");
                    if (!category.Show)
                        TreeViewHtml.Append("<strike>" + HttpContext.Current.Server.HtmlEncode(category.Name) + "</strike>");
                    else
                        TreeViewHtml.Append(HttpContext.Current.Server.HtmlEncode(category.Name));
                    TreeViewHtml.Append("</span>\r\n");
                    TreeViewHtml.Append("<ul>\r\n");
                    BuildTreeViewCheckBox(LtsSource, category.ID, CheckShow, LtsValues, ref TreeViewHtml);
                    TreeViewHtml.Append("</ul>\r\n");
                    TreeViewHtml.Append("</li>\r\n");
                }
                else
                {
                    TreeViewHtml.Append("<li title=\"" + category.Description + "\" public partial class=\"unselect\" id=\"" + category.ID.ToString() + "\"><span public partial class=\"file\"> <input id=\"Category_" + category.ID + "\" name=\"Category_" + category.ID + "\" value=\"" + category.ID + "\" type=\"checkbox\" title=\"" + category.Name + "\" " + (LtsValues.Contains(category.ID) ? " checked" : string.Empty) + "/> ");
                    if (!category.Show)
                        TreeViewHtml.Append("<strike>" + HttpContext.Current.Server.HtmlEncode(category.Name) + "</strike>");
                    else
                        TreeViewHtml.Append(HttpContext.Current.Server.HtmlEncode(category.Name));
                    TreeViewHtml.Append("</span></li>\r\n");
                }
            }
        }


        /// <summary>
        /// Hàm build ra treeview chứa danh sách category
        /// </summary>
        /// <param name="MaDonVi"></param>
        public void BuildTreeView(List<CategoryItem> LtsSource, int ID, bool CheckShow, ref StringBuilder TreeViewHtml)
        {
            IEnumerable<CategoryItem> tempCategory = LtsSource.OrderBy(o => o.Order).Where(m => m.ParentID == ID && m.ID > 0);
            if (CheckShow)
                tempCategory = tempCategory.Where(m => m.Show == CheckShow);

            foreach (CategoryItem category in tempCategory)
            {
                var countQuery = LtsSource.Where(m => m.ParentID == category.ID && m.ID > 0);
                if (CheckShow)
                    countQuery = countQuery.Where(m => m.Show == CheckShow);
                int totalChild = countQuery.Count();
                if (totalChild > 0)
                {
                    TreeViewHtml.Append("<li title=\"" + HttpContext.Current.Server.HtmlEncode(category.Description) + "\" public partial class=\"unselect\" id=\"" + category.ID.ToString() + "\"><span public partial class=\"folder\"><a public partial class=\"tool\" href=\"javascript:;\">");
                    if (!category.Show)
                        TreeViewHtml.Append("<strike>" + HttpContext.Current.Server.HtmlEncode(category.Name) + "</strike>");
                    else
                        TreeViewHtml.Append(HttpContext.Current.Server.HtmlEncode(category.Name));
                    TreeViewHtml.Append("</a>\r\n");
                    TreeViewHtml.AppendFormat(" <i>({0})</i>\r\n", totalChild);
                    TreeViewHtml.Append(buildEditToolByID(category) + "\r\n");
                    TreeViewHtml.Append("</span>\r\n");
                    TreeViewHtml.Append("<ul>\r\n");
                    BuildTreeView(LtsSource, category.ID, CheckShow, ref TreeViewHtml);
                    TreeViewHtml.Append("</ul>\r\n");
                    TreeViewHtml.Append("</li>\r\n");
                }
                else
                {
                    TreeViewHtml.Append("<li title=\"" + HttpContext.Current.Server.HtmlEncode(category.Description) + "\" public partial class=\"unselect\" id=\"" + category.ID.ToString() + "\"><span public partial class=\"file\"><a public partial class=\"tool\" href=\"javascript:;\">");
                    if (!category.Show)
                        TreeViewHtml.Append("<strike>" + HttpContext.Current.Server.HtmlEncode(category.Name) + "</strike>");
                    else
                        TreeViewHtml.Append(HttpContext.Current.Server.HtmlEncode(category.Name));
                    TreeViewHtml.Append("</a> <i>(0)</i>" + buildEditToolByID(category) + "</span></li>\r\n");
                }
            }
        }

        /// Replace for upper function
        /// <summary>
        /// Build ra editor cho từng categoryitem
        /// </summary>
        /// <param name="IDTool"></param>
        /// <param name="Active"></param>
        /// <returns></returns>
        private string buildEditToolByID(CategoryItem categoryItem)
        {
            StringBuilder strTool = new StringBuilder();
            strTool.Append("<div public partial class=\"quickTool\">\r\n");
            strTool.AppendFormat("    <a title=\"Thêm mới category: {1}\" public partial class=\"add\" href=\"#{0}\">\r\n", categoryItem.ID, HttpContext.Current.Server.HtmlEncode(categoryItem.Name));
            strTool.Append("        <img border=\"0\" title=\"Thêm mới category\" src=\"/Content/Admin/images/gridview/add.gif\">\r\n");
            strTool.Append("    </a>");
            strTool.AppendFormat("    <a title=\"Chỉnh sửa: {1}\" public partial class=\"edit\" href=\"#{0}\">\r\n", categoryItem.ID, HttpContext.Current.Server.HtmlEncode(categoryItem.Name));
            strTool.Append("        <img border=\"0\" title=\"Sửa category\" src=\"/Content/Admin/images/gridview/edit.gif\">\r\n");
            strTool.Append("    </a>");
            if (categoryItem.Show)
            {
                strTool.AppendFormat("    <a title=\"Ẩn: {1}\" href=\"#{0}\" public partial class=\"hide\">\r\n", categoryItem.ID, HttpContext.Current.Server.HtmlEncode(categoryItem.Name));
                strTool.Append("        <img border=\"0\" title=\"Đang hiển thị\" src=\"/Content/Admin/images/gridview/show.gif\">\r\n");
                strTool.Append("    </a>\r\n");
            }
            else
            {
                strTool.AppendFormat("    <a title=\"Hiển thị: {1}\" href=\"#{0}\" public partial class=\"show\">\r\n", categoryItem.ID, HttpContext.Current.Server.HtmlEncode(categoryItem.Name));
                strTool.Append("        <img border=\"0\" title=\"Đang ẩn\" src=\"/Content/Admin/images/gridview/hide.gif\">\r\n");
                strTool.Append("    </a>\r\n");
            }
            strTool.AppendFormat("    <a title=\"Xóa: {1}\" href=\"#{0}\" public partial class=\"delete\">\r\n", categoryItem.ID, HttpContext.Current.Server.HtmlEncode(categoryItem.Name));
            strTool.Append("        <img border=\"0\" title=\"Xóa category\" src=\"/Content/Admin/images/gridview/delete.gif\">\r\n");
            strTool.Append("    </a>\r\n");

            strTool.AppendFormat("    <a title=\"Sắp xếp các category con: {1}\" href=\"#{0}\" public partial class=\"sort\">\r\n", categoryItem.ParentID, HttpContext.Current.Server.HtmlEncode(categoryItem.Name));
            strTool.Append("        <img border=\"0\" title=\"Xắp xếp category\" src=\"/Content/Admin/images/gridview/sort.gif\">\r\n");
            strTool.Append("    </a>\r\n");

            strTool.Append("</div>\r\n");
            return strTool.ToString();
        }
        #endregion

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<CategoryItem> getAllListSimple()
        {
            var query = from c in QNewsDB.Categories
                        where c.ID > 0
                        orderby c.Order
                        select new CategoryItem()
                        {
                            ID = c.ID,
                            Name = c.Name,
                            Description = c.Description,
                            Order = c.Order,
                            ParentID = c.ParentID,
                            Show = c.Show,
                            NameSort = c.NameSort,
                            ShowInTab = c.ShowInTab,
                            AllID = c.AllID,
                            ShowInTopMenu = c.ShowInTopMenu,
                            ShowInHome = c.ShowInHome
                        };
            return query.ToList();
        }

        

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<CategoryItem> getAllListSimple(bool IsShow)
        {
            var query = from c in QNewsDB.Categories
                        where c.ID > 0 && c.Show == IsShow
                        orderby c.Order
                        select new
                        {
                            c.ID,
                            c.Name,
                            c.Description,
                            c.Order,
                            c.ParentID,
                            c.Show,
                            c.NameSort,
                            ShowInTab = c.ShowInTab,
                            AllID = c.AllID,
                            ShowInTopMenu = c.ShowInTopMenu,
                            ShowInHome = c.ShowInHome
                        };

            return query.ToList().Select(c => new CategoryItem()
            {
                ID = c.ID,
                Name = c.Name,
                Description = c.Description,
                Order = c.Order,
                ParentID = c.ParentID,
                Show = c.Show,
                NameSort = c.NameSort,
                ShowInTab = c.ShowInTab,
                AllID = c.AllID,
                ShowInTopMenu = c.ShowInTopMenu,
                ShowInHome = c.ShowInHome
            }).ToList();
        }


        public List<CategoryItem> getAllListSimpleByParentID(int ParentID)
        {
            var query = from c in QNewsDB.Categories
                        where c.ID > 0 && c.ParentID == ParentID
                        orderby c.Order
                        select new CategoryItem()
                        {
                            ID = c.ID,
                            Name = c.Name,
                            Description = c.Description,
                            Order = c.Order,
                            ParentID = c.ParentID,
                            Show = c.Show,
                            NameSort = c.NameSort
                        };
            return query.ToList();
        }

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <param name="IsShow">Kiểm tra hiển thị</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<CategoryItem> getListSimpleAll(bool IsShow)
        {
            var query = from c in QNewsDB.Categories
                        where (c.Show == IsShow && c.ID > 0)
                        orderby c.Name
                        select new CategoryItem()
                        {
                            ID = c.ID,
                            Name = c.Name,
                            Description = c.Description,
                            Order = c.Order,
                            ParentID = c.ParentID,
                            Show = c.Show,
                            NameSort = c.NameSort
                        };
            return query.ToList();
        }

        /// <summary>
        /// Lấy về dưới dạng Autocomplete
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="showLimit"></param>
        /// <returns></returns>
        public List<CategoryItem> GetListSimpleByAutoComplete(string keyword, int showLimit)
        {
            var query = from c in QNewsDB.Categories
                        where c.ID > 0
                        orderby c.Name
                        where c.Name.Contains(keyword) //autoComplete
                        select new CategoryItem()
                        {
                            ID = c.ID,
                            Name = c.Name,
                            Description = c.Description,
                            Order = c.Order,
                            ParentID = c.ParentID,
                            Show = c.Show,
                            NameSort = c.NameSort
                        };
            return query.Take(showLimit).ToList(); 
        }

        /// <summary>
        /// Lấy về dưới dạng Autocomplete
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="showLimit"></param>
        /// <returns></returns>
        public List<CategoryItem> GetListSimpleByAutoComplete(string keyword, int showLimit, bool IsShow)
        {
            var query = from c in QNewsDB.Categories
                        where c.ID > 0
                        orderby c.Name
                        where c.Show == IsShow
                        && c.Name.Contains(keyword) //autoComplete
                        select new CategoryItem()
                        {
                            ID = c.ID,
                            Name = c.Name,
                            Description = c.Description,
                            Order = c.Order,
                            ParentID = c.ParentID,
                            Show = c.Show,
                            NameSort = c.NameSort

                        };
            return query.Take(showLimit).ToList();
        }

        /// <summary>
        /// Lấy về kiểu đơn giản, phân trang
        /// </summary>
        /// <param name="pageSize">Số bản ghi trên trang</param>
        /// <param name="Page">Trang hiển thị</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<CategoryItem> getListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in QNewsDB.Categories select new CategoryItem() {
                ID = c.ID,
                Name = c.Name,
                Description = c.Description,
                Order = c.Order,
                ParentID = c.ParentID,
                Show = c.Show,
                NameSort = c.NameSort
            };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        /// <summary>
        /// Lấy về mảng đơn giản qua mảng ID
        /// </summary>
        /// <param name="LtsArrID"></param>
        /// <returns></returns>
        public List<CategoryItem> getListSimpleByArrID(List<int> LtsArrID)
        {
            var query = from c in QNewsDB.Categories
                        where LtsArrID.Contains(c.ID)
                        select new CategoryItem()
                        {
                            ID = c.ID,
                            Name = c.Name,
                            Description = c.Description,
                            Order = c.Order,
                            ParentID = c.ParentID,
                            Show = c.Show,
                            NameSort = c.NameSort
                        };
            TotalRecord = query.Count();
            return query.ToList();
        }

        #region Check Exits, Add, Update, Delete
        
        /// <summary>
        /// Lấy về bản ghi qua khóa chính
        /// </summary>
        /// <param name="ID">ID bản ghi</param>
        /// <returns>Bản ghi</returns>
        public Category getById(int ID)
        {
            var query = from c in QNewsDB.Categories where c.ID == ID select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Lấy về danh sách qua mảng id
        /// </summary>
        /// <param name="LtsArrID">Mảng ID</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<Category> getListByArrID(List<int> LtsArrID)
        {
            var query = from c in QNewsDB.Categories where LtsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        /// <summary>
        /// Kiểm tra bản ghi đã tồn tại hay chưa
        /// </summary>
        /// <param name="Category">Đối tượng kiểm tra</param>
        /// <returns>Trạng thái tồn tại</returns>
        public bool checkExits(Category Category)
        {
            var query = from c in QNewsDB.Categories where ((c.Name == Category.Name) && (c.ID != Category.ID)) select c;
            if (query.Count() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Lấy về bản ghi qua tên
        /// </summary>
        /// <param name="Name">Tên bản ghi</param>
        /// <returns>Bản ghi</returns>
        public Category getByName(string Name)
        {
            var query = from c in QNewsDB.Categories where ((c.Name == Name)) select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="Category">bản ghi cần thêm</param>
        public void Add(Category Category)
        {
            QNewsDB.Categories.Add(Category);
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="Category">Xóa bản ghi</param>
        public void Delete(Category Category)
        {
            QNewsDB.Categories.Remove(Category);
        }

        /// <summary>
        /// save bản ghi vào DB
        /// </summary>
        public void Save()
        {
            QNewsDB.SaveChanges();
        }
        #endregion
    }
}
