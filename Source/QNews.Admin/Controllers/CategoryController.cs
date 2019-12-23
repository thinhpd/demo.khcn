using QNews.Base;
using QNews.Data.Admin;
using QNews.Utils;
using QNews.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace QNews.Admin.Controllers
{
    /// <summary>
    /// Class sử dụng cho quản lý sản phẩm
    /// </summary>
    public class CategoryController : BaseController
    {

        CategoryDA categoryDA = new CategoryDA("#");

        /// <summary>
        /// Trang chủ, index. Load ra grid dưới dạng ajax
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AjaxTreeSelect()
        {
            var LtsSourceCategory = categoryDA.getAllListSimple();
            List<int> LtsValues = StaticClass.getValuesArray(Request["ValuesSelected"]);
            StringBuilder stbHtml = new StringBuilder();
            categoryDA.BuildTreeViewCheckBox(LtsSourceCategory, 0, true, LtsValues, ref stbHtml);
            ViewData.Model = stbHtml.ToString();
            ViewBag.Container = Request["Container"];
            ViewBag.selectMutil = Convert.ToBoolean(Request["SelectMutil"]);

            return View();
        }

        public ActionResult AjaxSort()
        {
            var LtsSourceCategory = categoryDA.getAllListSimpleByParentID(ArrID.FirstOrDefault());
            ViewData.Model = LtsSourceCategory;
            return View();
        }

        /// <summary>
        /// Load danh sách bản ghi dưới dạng bảng
        /// </summary>
        /// <returns></returns>
        public ActionResult ListItems()
        {
            var LtsSourceCategory = categoryDA.getAllListSimple();
            StringBuilder stbHtml = new StringBuilder();
            categoryDA.BuildTreeView(LtsSourceCategory, 0, false, ref stbHtml);
            ViewData.Model = stbHtml.ToString();
            return View();
        }

        /// <summary>
        /// Trang xem chi tiết trong model
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxView()
        {
            var CategoryModel = categoryDA.getById(ArrID.FirstOrDefault());
            ViewData.Model = CategoryModel;
            return View();
        }

        /// <summary>
        /// Form dùng cho thêm mới, sửa. Load bằng Ajax dialog
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxForm()
        {
            var CategoryModel = new Base.Category()
            {
                Show = true,
                Order = 0,
                ParentID = (ArrID.Count() > 0) ? ArrID.FirstOrDefault() : 0
            };

            if (DoAction == ActionType.Edit)
                CategoryModel = categoryDA.getById(ArrID.FirstOrDefault());


            var LtsAllItems = categoryDA.getAllListSimple();
            ViewBag.CategoryParentID = categoryDA.getAllSelectList(LtsAllItems, CategoryModel.ID, true);
            ViewBag.TypeOfCategory = categoryDA.getAllTypeOfCategory();
            ViewData.Model = CategoryModel;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }


        /// <summary>
        /// Hứng các giá trị, phục vụ cho thêm, sửa, xóa, ẩn, hiện
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken()]public ActionResult Actions()
        {
            JsonMessage msg = new JsonMessage();
            Category category = new Category();
            List<Category> ltsCategoryItems;
            StringBuilder stbMessage;

            #region các biến dùng trong Update Mảng
            List<CategoryItem> LtsAllCategory;
            List<CategoryItem> LtsMapBeforUpdate;
            List<CategoryItem> CategoryUpdate;
            List<int> LtsAllIdByRoot; //Chứa tập hợp ID
            #endregion


            
            switch (DoAction)
            {
                case ActionType.Add:
                    if (hasAdd)
                    {
                        UpdateModel(category);

                        #region cập nhật url
                        Base.Url url = new Base.Url();
                        url.UrlID = Request["Url"];
                        category.Urls.Add(url);
                        #endregion


                        categoryDA.Add(category);
                        categoryDA.Save();

                        #region Update danh sách các ID con -
                        //Trường hợp thêm mới chỉ cần update chính bản thân nó. và các nhánh trở về trước
                        LtsAllCategory = categoryDA.getAllListSimple(); //Lấy về toàn bộ danh sách
                        CategoryUpdate = categoryDA.getListMap(LtsAllCategory, category.ID);//Lấy về danh nhánh trước thời điểm update
                        foreach (var itemUpdate in CategoryUpdate)
                        {
                            Base.Category categoryDataUpdate = categoryDA.getById(itemUpdate.ID); //Lấy về bản ghi cần update
                            LtsAllIdByRoot = new List<int>() { itemUpdate.ID }; //Bao gồm cả id hiện tại
                            categoryDA.getAllIdCategoryFromRoot(LtsAllCategory, itemUpdate.ID, LtsAllIdByRoot);
                            categoryDataUpdate.AllID = string.Join(",", LtsAllIdByRoot);
                            categoryDA.Save();
                        }
                        #endregion

                        msg = new JsonMessage()
                        {
                            Erros = false,
                            ID = category.ID.ToString(),
                            Message = string.Format("Đã thêm mới chuyên mục: <b>{0}</b>", Server.HtmlEncode(category.Name))
                        };
                    }
                    break;

                case ActionType.Edit:
                    if (hasEdit)
                    {
                        category = categoryDA.getById(ArrID.FirstOrDefault());

                        LtsMapBeforUpdate = new List<CategoryItem>();
                        if (category.ID > 0)
                        {
                            LtsAllCategory = categoryDA.getAllListSimple(); //Lấy về toàn bộ danh sách
                            LtsMapBeforUpdate = categoryDA.getListMap(LtsAllCategory, ArrID.FirstOrDefault());//Lấy về danh nhánh trước thời điểm update
                        }
                        UpdateModel(category);
                        categoryDA.Save();

                        if (category.ID > 0)
                        {
                            #region lấy về  và cập nhật danh sách các ID Con
                            LtsAllCategory = categoryDA.getAllListSimple(); //Lấy về lại toàn bộ danh sách sau thời điểm cập nhật.
                            CategoryUpdate = categoryDA.getListMap(LtsAllCategory, category.ID);//Lấy về danh sách cần update thời điểm sau update
                            foreach (var itemBefor in LtsMapBeforUpdate) //Cộng danh sách trước update để tiến hành cập nhật
                            {
                                if (!(CategoryUpdate.Where(c => c.ID == itemBefor.ID).Count() > 0))
                                    CategoryUpdate.Add(itemBefor);
                            }
                            foreach (var itemUpdate in CategoryUpdate)
                            {
                                Base.Category categoryDataUpdate = categoryDA.getById(itemUpdate.ID); //Lấy về bản ghi cần update
                                LtsAllIdByRoot = new List<int>() { itemUpdate.ID }; //Bao gồm cả id hiện tại
                                categoryDA.getAllIdCategoryFromRoot(LtsAllCategory, itemUpdate.ID, LtsAllIdByRoot);
                                categoryDataUpdate.AllID = string.Join(",", LtsAllIdByRoot);
                                categoryDA.Save();
                            }
                            #endregion
                        }
                        msg = new JsonMessage()
                        {
                            Erros = false,
                            ID = category.ID.ToString(),
                            Message = string.Format("Đã cập nhật chuyên mục: <b>{0}</b>", Server.HtmlEncode(category.Name))
                        };
                    }
                    break;

                case ActionType.Delete:
                    if (hasDelete)
                    {
                        ltsCategoryItems = categoryDA.getListByArrID(ArrID);
                        stbMessage = new StringBuilder();
                        foreach (var item in ltsCategoryItems)
                        {
                            if (item.Categories.Count() > 0 || item.Contents.Count() > 0 || item.Urls.Count() > 0)
                            {
                                stbMessage.AppendFormat("Chuyên mục <b>{0}</b> đang được sử dụng, không được phép xóa.<br />", Server.HtmlEncode(item.Name));
                                //msg.Erros = true;
                            }
                            else
                            {
                                categoryDA.Delete(item);
                                stbMessage.AppendFormat("Đã xóa chuyên mục <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                            }
                        }
                        msg.ID = string.Join(",", ArrID);
                        categoryDA.Save();
                        msg.Message = stbMessage.ToString();
                    }
                    break;

                case ActionType.Show:
                    if (hasEdit)
                    {
                        ltsCategoryItems = categoryDA.getListByArrID(ArrID).Where(o => !o.Show).ToList(); //Chỉ lấy những đối tượng ko được hiển thị
                        stbMessage = new StringBuilder();
                        foreach (var item in ltsCategoryItems)
                        {
                            item.Show = true;
                            stbMessage.AppendFormat("Đã hiển thị chuyên mục <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                        }
                        categoryDA.Save();
                        msg.ID = string.Join(",", ltsCategoryItems.Select(o => o.ID));
                        msg.Message = stbMessage.ToString();
                    }
                    break;

                case ActionType.Hide:
                    if (hasEdit)
                    {
                        ltsCategoryItems = categoryDA.getListByArrID(ArrID).Where(o => o.Show).ToList(); //Chỉ lấy những đối tượng được hiển thị
                        stbMessage = new StringBuilder();
                        foreach (var item in ltsCategoryItems)
                        {
                            item.Show = false;
                            stbMessage.AppendFormat("Đã ẩn chuyên mục <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                        }
                        categoryDA.Save();
                        msg.ID = string.Join(",", ltsCategoryItems.Select(o => o.ID));
                        msg.Message = stbMessage.ToString();
                    }
                    break;

                case ActionType.Order:
                    if (hasEdit)
                    {
                        if (!string.IsNullOrEmpty(Request["OrderValues"]))
                        {
                            var OrderValues = Request["OrderValues"];
                            if (OrderValues.Contains("|"))
                            {
                                foreach (var KeyValue in OrderValues.Split('|'))
                                {
                                    if (KeyValue.Contains("_"))
                                    {
                                        var tempCategory = categoryDA.getById(Convert.ToInt32(KeyValue.Split('_')[0]));
                                        tempCategory.Order = Convert.ToInt32(KeyValue.Split('_')[1]);
                                        categoryDA.Save();
                                    }
                                }
                            }
                            msg.ID = string.Join(",", OrderValues);
                            msg.Message = "Đã cập nhật lại thứ tự chuyên mục";
                        }
                    }
                    break;
            }

            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }
            HttpContext.Cache.Remove("TopMenu"); //Clear cache menu
            HttpContext.Cache.Remove("ProductLeftMenu");
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Dùng cho tra cứu nhanh
        /// </summary>
        /// <returns></returns>
        public ActionResult AutoComplete()
        {
            var term = Request["term"];
            var LtsResults = categoryDA.GetListSimpleByAutoComplete(term, 10, true);
            return Json(LtsResults, JsonRequestBehavior.AllowGet);
        }


    }
}