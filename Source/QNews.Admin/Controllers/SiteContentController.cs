using QNews.Data.Admin;
using QNews.Models;
using QNews.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Globalization;
using HtmlAgilityPack;
using QNews.Base;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Security.Application;
using Ganss.XSS;

namespace QNews.Admin.Controllers
{
    public class SiteContentController : BaseController
    {
        ContentDA _ContentDA = new ContentDA("#");


        private string RunDownloadImage(string url, DateTime createdDate)
        {

            string Save_Folder = System.Web.HttpContext.Current.Server.MapPath("/Uploads/images");
            url = System.Web.HttpUtility.UrlDecode(url);

            string urlReturn = string.Empty;

            #region Tạo thư mục
            DateTime DateFolder = createdDate;
            if (!System.IO.Directory.Exists(Save_Folder + "/" + DateFolder.ToString("yyyy")))
            {
                System.IO.Directory.CreateDirectory(Save_Folder + "/" + DateFolder.ToString("yyyy"));
            }
            if (!System.IO.Directory.Exists(Save_Folder + "/" + DateFolder.ToString("yyyy") + "\\" + DateFolder.ToString("MM")))
            {
                System.IO.Directory.CreateDirectory(Save_Folder + "/" + DateFolder.ToString("yyyy") + "\\" + DateFolder.ToString("MM"));
            }
            if (!System.IO.Directory.Exists(Save_Folder + "/" + DateFolder.ToString("yyyy") + "\\" + DateFolder.ToString("MM") + "\\" + DateFolder.ToString("dd")))
            {
                System.IO.Directory.CreateDirectory(Save_Folder + "/" + DateFolder.ToString("yyyy") + "\\" + DateFolder.ToString("MM") + "\\" + DateFolder.ToString("dd"));
            }
            string fileName = createdDate.ToString("yyyyMMddHHmmssffff") + "_" + url.Substring(url.LastIndexOf('/') + 1);
            string fileSavePath = string.Format("{0}/{1}/{2}/{3}/{4}", Save_Folder, DateFolder.ToString("yyyy"), DateFolder.ToString("MM"), DateFolder.ToString("dd"), fileName);
            #endregion

            using (WebClient w = new WebClient())
            {
                try
                {
                    w.DownloadFile(url, fileSavePath);
                    urlReturn = string.Format("{4}/images/{0}/{1}/{2}/{3}", DateFolder.ToString("yyyy"), DateFolder.ToString("MM"), DateFolder.ToString("dd"), fileName, "/Uploads/");
                }
                catch
                {
                    Console.WriteLine(string.Format("can't download:{0}", url));
                    urlReturn = url;
                }
            }
            return urlReturn;
        }

        private Content getCloneNews(string urlClone, ref JsonMessage msg)
        {

            msg.Erros = false;
            msg.Message = urlClone;

            Content newItem = new Content();
            newItem.AllowComment = true;
            newItem.PublishDate = DateTime.Now;

            var currentDomain = _ContentDA.QNewsDB.Clone_Domain.Where(o => urlClone.ToLower().Contains(o.DomainUrl.ToLower())).FirstOrDefault();
            if (currentDomain != null)
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument htmlDoc = web.Load(urlClone);

                HtmlAgilityPack.HtmlNode titleNode = htmlDoc.DocumentNode.SelectSingleNode(".//title");
                HtmlAgilityPack.HtmlNode desNode = htmlDoc.DocumentNode.SelectSingleNode(".//title");
                HtmlAgilityPack.HtmlNode contentNode = htmlDoc.DocumentNode.SelectSingleNode(".//title");

                #region lấy về các node
                try
                {
                    titleNode = htmlDoc.DocumentNode.SelectSingleNode(currentDomain.XpathTitle);
                }
                catch
                {
                    msg.Erros = true;
                    msg.Message = "XPath Tiêu đề không đúng.";
                }
                try
                {
                    desNode = htmlDoc.DocumentNode.SelectSingleNode(currentDomain.XpathDescription);
                }
                catch
                {
                    msg.Erros = true;
                    msg.Message = "XPath mô tả không đúng.";
                }
                try
                {
                    contentNode = htmlDoc.DocumentNode.SelectSingleNode(currentDomain.XpathContent);

                }
                catch
                {
                    msg.Erros = true;
                    msg.Message = "XPath nội dung không đúng.";
                }

                HtmlAgilityPack.HtmlNode createdNode = null;
                if (!string.IsNullOrEmpty(currentDomain.XpathCreated))
                    createdNode = htmlDoc.DocumentNode.SelectSingleNode(currentDomain.XpathCreated);
                #endregion


                #region Xóa các dữ liệu rác
                foreach (var item in currentDomain.Clone_Remove)
                {
                    try
                    {
                        HtmlAgilityPack.HtmlNodeCollection nodeRemoves;
                        if (item.XpathRemoveIn == 1)
                            nodeRemoves = titleNode.SelectNodes(item.XpathValue);
                        else if (item.XpathRemoveIn == 1)
                            nodeRemoves = desNode.SelectNodes(item.XpathValue);
                        else
                            nodeRemoves = contentNode.SelectNodes(item.XpathValue);

                        if (nodeRemoves != null && nodeRemoves.Count > 0)
                        {
                            foreach (HtmlAgilityPack.HtmlNode remove in nodeRemoves)
                                remove.Remove();
                        }
                    }
                    catch
                    {
                        msg.Erros = true;
                        msg.Message = "Kiểm tra lại dữ liệu xóa bỏ, ko xóa được.";
                    }
                }
                #endregion


                #region Update lại link và ảnh
                if (contentNode != null)
                {
                    HtmlAgilityPack.HtmlNodeCollection imgNodes = contentNode.SelectNodes(".//img");
                    if (imgNodes != null && imgNodes.Count > 0)
                    {
                        foreach (HtmlAgilityPack.HtmlNode img in imgNodes)
                        {
                            if (img.Attributes["src"] != null && !string.IsNullOrEmpty(img.Attributes["src"].Value)) //nếu khác null
                            {
                                if (img.Attributes["src"].Value.First() == '/') //nếu bắt đầu từ root
                                {
                                    string tempLink = urlClone.Replace("http://", "");
                                    img.Attributes["src"].Value = "http://" + tempLink.Substring(0, tempLink.IndexOf('/')) + img.Attributes["src"].Value;
                                }

                            }
                        }
                    }
                }
                #endregion



                #region Lấy dữ liệu đã được lọc và xóa rác
                if (titleNode != null)
                    newItem.Title = HttpUtility.HtmlDecode(titleNode.InnerText.Trim());
                if (desNode != null)
                    newItem.Description = HttpUtility.HtmlDecode(desNode.InnerText.Trim());
                if (contentNode != null)
                    newItem.Details = contentNode.InnerHtml;
                #endregion

                #region Thay thế các từ khóa
                foreach (var item in currentDomain.Clone_Replace)
                {
                    if (item.ReplaceIn == 1)
                        newItem.Title = newItem.Title.Replace(item.StringSource, item.StringDest);
                    else if (item.ReplaceIn == 2)
                        newItem.Description = newItem.Description.Replace(item.StringSource, item.StringDest);
                    else
                        newItem.Details = newItem.Details.Replace(item.StringSource, item.StringDest);
                }
                #endregion


                #region Lấy về ngày tháng
                if (createdNode != null)
                {
                    try
                    {
                        string dateTimeValue = string.Empty;
                        if (createdNode.Name == "meta") //Nếu là các thẻ như Meta lấy content
                        {
                            if (createdNode.Attributes["content"] != null && !string.IsNullOrEmpty(createdNode.Attributes["content"].Value))
                                dateTimeValue = createdNode.Attributes["content"].Value.Trim();
                        }
                        else if (createdNode.Name == "input") //Nếu là input lấy value
                        {
                            if (createdNode.Attributes["value"] != null && !string.IsNullOrEmpty(createdNode.Attributes["value"].Value))
                                dateTimeValue = createdNode.Attributes["value"].Value.Trim();
                        }
                        else //Lấy text
                        {
                            dateTimeValue = createdNode.InnerText.Trim();
                        }
                        dateTimeValue = dateTimeValue.Replace("SA", "AM").Replace("CH", "PM");
                        newItem.PublishDate = DateTime.ParseExact(dateTimeValue, currentDomain.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None);

                    }
                    catch (Exception)
                    {
                        newItem.PublishDate = DateTime.Now;
                    }

                }
                #endregion


                #region Lấy ảnh
                if (currentDomain.XpathImageInContent)
                {
                    if (contentNode != null)
                    {
                        HtmlAgilityPack.HtmlNode imgNode = contentNode.SelectSingleNode(currentDomain.XpathImage);
                        if (imgNode != null && imgNode.Attributes["src"] != null)
                        {
                            newItem.Image = imgNode.Attributes["src"].Value;
                        }
                    }
                }
                else
                {
                    HtmlAgilityPack.HtmlNode imgNode = htmlDoc.DocumentNode.SelectSingleNode(currentDomain.XpathImage);
                    if (imgNode != null && imgNode.Attributes["src"] != null)
                    {
                        newItem.Image = imgNode.Attributes["src"].Value;
                    }
                }
                if (!string.IsNullOrEmpty(newItem.Image))
                {
                    if (!newItem.Image.Contains("http"))
                    {
                        if (newItem.Image.FirstOrDefault() == '/')
                        {
                            newItem.Image = currentDomain.DomainUrl + newItem.Image;
                        }
                    }
                    else
                    {
                        newItem.Image = urlClone.Substring(urlClone.LastIndexOf('/')) + newItem.Image;
                    }
                }

                #endregion

                #region Remove link
                newItem.Details = Regex.Replace(newItem.Details, @"<a\b[^>]+>([^<]*(?:(?!</a)<[^<]*)*)</a>", "$1");
                #endregion

            }
            return newItem;
        }

        public ActionResult AjaxFormClone()
        {
            return View();
        }
        public ActionResult AutoComplete()
        {
            if (DoAction == ActionType.Add) //Nếu thêm mới
            {
                JsonMessage msg = new JsonMessage();
                string SearchValue = Request["Values"];
                if (string.IsNullOrEmpty(SearchValue))
                {
                    msg = new JsonMessage()
                    {
                        Erros = true,
                        Message = "Bạn phải nhập tên nội dung"
                    };
                }
                else
                {
                    var _Content = _ContentDA.getByName(SearchValue);
                    if (_Content == null)
                    {
                        msg = new JsonMessage()
                        {
                            Erros = true,
                            Message = "nội dung không tồn tại trong hệ thống"
                        };
                    }
                    else
                    {
                        msg = new JsonMessage()
                        {
                            Erros = false,
                            ID = _Content.ID.ToString(),
                            Message = _Content.Title
                        };
                    }
                }
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else //Nếu xem từ khóa
            {
                string query = Request["query"];
                var ltsResults = _ContentDA.GetListSimpleByAutoComplete(query, 10);
                AutoCompleteItem ResulValues = new AutoCompleteItem()
                {
                    query = query,
                    data = ltsResults.Select(o => o.ID.ToString()).ToList(),
                    suggestions = ltsResults.Select(o => o.Title).ToList()
                };
                return Json(ResulValues, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Trang chủ, index. Load ra grid dưới dạng ajax
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var catDA = new Data.Admin.CategoryDA();
            var allCategory = catDA.getAllListSimple();
            ViewBag.AllCategory = catDA.getAllSelectList(allCategory, 0, false);
            ViewBag.AllEvent = new Data.Admin.DocumentScopeDA().getAllListSimple();
            ViewBag.AllStatus = catDA.getAllStatusSimple();
            ViewBag.AllUser = catDA.getAllUserSimple();
            ViewBag.AllContributor = new Data.Admin.ContributorDA().getAllListSimple();
            return View();
        }

        /// <summary>
        /// Load danh sách bản ghi dưới dạng bảng
        /// </summary>
        /// <returns></returns>
        public ActionResult ListItems()
        {
            ViewData.Model = _ContentDA.getListSimpleByRequest(Request);
            ViewBag.PageHtml = _ContentDA.GridHtmlPage;
            return View();
        }

        public ActionResult Report()
        {
            if (!string.IsNullOrEmpty(Request["do"]))
            {
                return Excel();
            }

            int TotalRow = 0;
            var req = new ParramRequest(Request);
            ViewData.Model = _ContentDA.getListFullByRequest(Request, ref TotalRow);
            ViewBag.PageHtml = new QNews.Utils.Paging().getHtmlPage("?" + req.ToString(), 3, req.CurrentPage, req.RowPerPage, TotalRow);
            return View();
        }

        public FileResult Excel()
        {
            int TotalRow = 0;
            var req = new ParramRequest(Request);
            var LtsAllData = _ContentDA.getListSimpleByRequest_Excel(Request);

            StringBuilder stbBuilder = new StringBuilder();
            stbBuilder.Append("<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:x='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>Worksheet</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>\r\n");
            stbBuilder.Append("        \r\n");
            stbBuilder.Append("        <tbody><tr>\r\n");
            stbBuilder.Append("            <th>Tên bài viết</th>\r\n");
            stbBuilder.Append("            <th>Ngày viết</th>\r\n");
            stbBuilder.Append("            <th>Ngày Phê duyệt</th>\r\n");
            stbBuilder.Append("            <th>Người biên tập</th>\r\n");
            stbBuilder.Append("            <th>Phóng viên bài</th>\r\n");
            stbBuilder.Append("            <th>Phóng viên ảnh</th>\r\n");
            stbBuilder.Append("        </tr>\r\n");
            stbBuilder.Append("        \r\n");
            foreach (var item in LtsAllData)
            {
                stbBuilder.Append("       <tr>\r\n");
                stbBuilder.AppendFormat("            <td>{0}</td>\r\n", item.Title);
                stbBuilder.AppendFormat("            <td>{0}</td>\r\n", item.CreateDate.ToString("dd/MM/yyyy"));
                stbBuilder.AppendFormat("            <td>{0}</td>\r\n", (item.PublishDate.HasValue) ? item.PublishDate.Value.ToString("dd/MM/yyyy") : string.Empty);
                stbBuilder.AppendFormat("            <td>{0}</td>\r\n", item.EditorBy);
                stbBuilder.AppendFormat("            <td>{0}</td>\r\n", item.AuthorBy);
                stbBuilder.AppendFormat("            <td>{0}</td>\r\n", item.PhotoBy);
                stbBuilder.Append("        </tr>\r\n");
                stbBuilder.Append("        \r\n");
            }
            stbBuilder.Append("</tbody></table></body></html>\r\n");
            byte[] buffer = Encoding.UTF8.GetBytes(stbBuilder.ToString());
            return File(buffer, "application/vnd.ms-excel", "ExportData.xls");
        }


        /// <summary>
        /// Trang xem chi tiết trong model
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxView()
        {
            var ContentModel = _ContentDA.getById(ArrID.FirstOrDefault());
            ViewData.Model = ContentModel;
            return View();
        }

        /// <summary>
        /// Form dùng cho thêm mới, sửa. Load bằng Ajax dialog
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxForm()
        {

            string urlClone = Request["SourceUrl"];


            var ContentModel = new Base.Content()
            {
                ShowDate = true,
                ShowOther = true,
                AllowComment = true,
                Description = string.Empty,
                Details = string.Empty
            };

            if (DoAction == ActionType.Edit)
                ContentModel = _ContentDA.getById(ArrID.FirstOrDefault());


            if (!string.IsNullOrEmpty(urlClone))
            {
                JsonMessage msg = new JsonMessage();
                ContentModel = getCloneNews(urlClone, ref msg);
                if (!string.IsNullOrEmpty(Request["SourceID"]))
                {
                    ContentModel.RssID = Convert.ToInt32(Request["SourceID"]);
                }
            }

            var catDA = new Data.Admin.CategoryDA();
            var allCategory = catDA.getAllListSimple();
            ViewBag.AllCategory = catDA.getAllSelectList(allCategory, 0, false);
            ViewBag.AllEvent = new Data.Admin.EventDA().getAllListSimple();
            ViewBag.AllScope = new Data.Admin.DocumentScopeDA().getAllListSimple();
            ViewBag.AllTypeOfScope = new Data.Admin.DocumentScopeDA().QNewsDB.TypeOfScopes.Where(o => o.ID != 2).Select(o => new ScopeItem() {ID = o.ID, Title = o.Title });


            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            ViewBag.AllContributor = new Data.Admin.ContributorDA().getAllListSimple();
            return View(ContentModel);
        }

        public ActionResult AjaxApprove()
        {
            var contentDA = new Data.Admin.ContentDA();
            ViewBag.AllStatus = contentDA.getAllStatusSimple();
            var Model = contentDA.getById(ArrID.FirstOrDefault());
            return View(Model);
        }


        /// <summary>
        /// Hứng các giá trị, phục vụ cho thêm, sửa, xóa, ẩn, hiện
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken()]
        public ActionResult Actions()
        {
            JsonMessage msg = new JsonMessage();
            Base.Content _Content = new Base.Content();
            List<Base.Content> lts_ContentItems;
            StringBuilder stbMessage;


            List<int> IDValues;
            List<QNews.Base.Category> LtsCategorySelected;
            List<QNews.Base.Event> LtsEventSelected;

         
            try
            {
                switch (DoAction)
                {

                    case ActionType.Add:
                        if (hasAdd)
                        {
                            UpdateModel(_Content);

                            _Content.Description = sanitizer.Sanitize(_Content.Description);
                            _Content.Details = sanitizer.Sanitize(_Content.Details);
                            _Content.CreateDate = DateTime.Now;
                            _Content.ModifyDate = DateTime.Now;
                            _Content.CreateBy = User.Identity.GetUserId();
                            _Content.ModifyBy = User.Identity.GetUserId();
                            _Content.StatusID = (int)Utils.WorkFlowStatus.BAN_NHAP;


                            #region Cập nhật category
                            IDValues = StaticClass.getValuesArray(Request["CategoryID"]);
                            LtsCategorySelected = _ContentDA.getListCategoryByArrID(IDValues);
                            foreach (var category in LtsCategorySelected)
                            {
                                _Content.Categories.Add(category);
                            }
                            #endregion

                            #region Cập nhật event
                            IDValues = StaticClass.getValuesArray(Request["EventID"]);
                            LtsEventSelected = _ContentDA.getListEventByArrID(IDValues);
                            foreach (var category in LtsEventSelected)
                            {
                                _Content.Events.Add(category);
                            }
                            #endregion

                            #region cập nhật url
                            Base.Url url = new Base.Url();
                            url.UrlID = Request["Url"];
                            _Content.Urls.Add(url);
                            #endregion

                            _ContentDA.Add(_Content);
                            _ContentDA.Save();
                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = _Content.ID.ToString(),
                                Message = string.Format("Đã thêm mới nội dung: <b>{0}</b>", Server.HtmlEncode(_Content.Title))
                            };
                        }
                        break;

                    case ActionType.Edit:
                        if (hasEdit)
                        {
                            _Content = _ContentDA.getById(ArrID.FirstOrDefault());

                            #region Lưu thêm 1 version mới
                            Base.Version version = new Base.Version();
                            version.AllowComment = _Content.AllowComment;
                            version.ContentID = _Content.ID;
                            version.Description = _Content.Description;
                            version.Details = _Content.Details;
                            version.Image = _Content.Image;
                            version.IsRemoved = _Content.IsRemoved;
                            version.ModifyBy = _Content.ModifyBy;
                            version.ModifyDate = _Content.ModifyDate;
                            version.Source = _Content.Source;
                            version.StatusID = _Content.StatusID;
                            version.Title = _Content.Title;
                            version.PublishDate = _Content.PublishDate;
                            version.Url = _Content.Urls.FirstOrDefault().UrlID;
                            version.IsHot = _Content.IsHot;

                            #region Cập nhật category
                            IDValues = _Content.Categories.Select(o => o.ID).ToList();
                            LtsCategorySelected = _ContentDA.getListCategoryByArrID(IDValues);
                            foreach (var category in LtsCategorySelected)
                            {
                                version.Categories.Add(category);
                            }
                            #endregion

                            #region Cập nhật event
                            IDValues = _Content.Events.Select(o => o.ID).ToList();
                            LtsEventSelected = _ContentDA.getListEventByArrID(IDValues);
                            foreach (var category in LtsEventSelected)
                            {
                                version.Events.Add(category);
                            }
                            #endregion


                            _ContentDA.AddVersion(version);
                            #endregion


                            UpdateModel(_Content);

                            _Content.Description = sanitizer.Sanitize(_Content.Description);
                            _Content.Details = sanitizer.Sanitize(_Content.Details);
                            _Content.ModifyDate = DateTime.Now;
                            _Content.ModifyBy = User.Identity.GetUserId();
                            _Content.StatusID = (int)Utils.WorkFlowStatus.CHO_DUYET;

                            #region Cập nhật category
                            _Content.Categories.Clear();
                            IDValues = StaticClass.getValuesArray(Request["CategoryID"]);
                            LtsCategorySelected = _ContentDA.getListCategoryByArrID(IDValues);
                            foreach (var category in LtsCategorySelected)
                            {
                                _Content.Categories.Add(category);
                            }
                            #endregion

                            #region Cập nhật event
                            _Content.Events.Clear();
                            IDValues = StaticClass.getValuesArray(Request["EventID"]);
                            LtsEventSelected = _ContentDA.getListEventByArrID(IDValues);
                            foreach (var category in LtsEventSelected)
                            {
                                _Content.Events.Add(category);
                            }
                            #endregion


                            _ContentDA.Save();
                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = _Content.ID.ToString(),
                                Message = string.Format("Đã cập nhật nội dung: <b>{0}</b>", Server.HtmlEncode(_Content.Title))
                            };
                        }
                        break;

                    case ActionType.Meta:

                        string urlClone = Request["SourceUrl"];
                        bool isOk = _ContentDA.QNewsDB.Clone_Domain.Where(o => urlClone.ToLower().Contains(o.DomainUrl)).Count() > 0;
                        if (!isOk)
                        {
                            msg.Erros = true;
                            msg.Message = "Tên miền này không hỗ trợ. Vui lòng cấu hình domain trước khi thực hiện bóc tách dữ liệu";
                        }
                        else
                        {
                            getCloneNews(urlClone, ref msg);
                        }
                        break;

                    case ActionType.Reset:
                        if (hasEdit)
                        {
                            Base.Version versionRevert = _ContentDA.getVersionById(ArrID.FirstOrDefault());//Lấy về phiên bản cần revert
                            _Content = _ContentDA.getById(versionRevert.ContentID);

                            #region Lưu thêm 1 version mới
                            var version = new Base.Version();
                            version.AllowComment = _Content.AllowComment;
                            version.ContentID = _Content.ID;
                            version.Description = _Content.Description;
                            version.Details = _Content.Details;
                            version.Image = _Content.Image;
                            version.IsRemoved = _Content.IsRemoved;
                            version.ModifyBy = _Content.ModifyBy;
                            version.ModifyDate = _Content.ModifyDate;
                            version.Source = _Content.Source;
                            version.StatusID = _Content.StatusID;
                            version.Title = _Content.Title;
                            version.PublishDate = _Content.PublishDate;
                            version.Url = _Content.Urls.FirstOrDefault().UrlID;
                            version.IsHot = _Content.IsHot;

                            #region Cập nhật category
                            IDValues = _Content.Categories.Select(o => o.ID).ToList();
                            LtsCategorySelected = _ContentDA.getListCategoryByArrID(IDValues);
                            foreach (var category in LtsCategorySelected)
                            {
                                version.Categories.Add(category);
                            }
                            #endregion

                            #region Cập nhật event
                            IDValues = _Content.Events.Select(o => o.ID).ToList();
                            LtsEventSelected = _ContentDA.getListEventByArrID(IDValues);
                            foreach (var category in LtsEventSelected)
                            {
                                version.Events.Add(category);
                            }
                            #endregion


                            _ContentDA.AddVersion(version);
                            #endregion

                            #region Chuyển lại versionRevert sang content
                            _Content.AllowComment = versionRevert.AllowComment;
                            _Content.Description = versionRevert.Description;
                            _Content.Details = versionRevert.Details;
                            _Content.Image = versionRevert.Image;
                            _Content.IsRemoved = versionRevert.IsRemoved;
                            _Content.Source = versionRevert.Source;
                            _Content.StatusID = versionRevert.StatusID;
                            _Content.Title = versionRevert.Title;
                            _Content.PublishDate = versionRevert.PublishDate;
                            _Content.IsHot = versionRevert.IsHot;

                            _Content.ModifyDate = DateTime.Now;
                            _Content.ModifyBy = User.Identity.GetUserId();
                            _Content.StatusID = (int)Utils.WorkFlowStatus.CHO_DUYET;

                            #region Cập nhật category
                            _Content.Categories.Clear();
                            IDValues = versionRevert.Categories.Select(o => o.ID).ToList();
                            LtsCategorySelected = _ContentDA.getListCategoryByArrID(IDValues);
                            foreach (var category in LtsCategorySelected)
                            {
                                _Content.Categories.Add(category);
                            }
                            #endregion

                            #region Cập nhật event
                            _Content.Events.Clear();
                            IDValues = versionRevert.Events.Select(o => o.ID).ToList();
                            LtsEventSelected = _ContentDA.getListEventByArrID(IDValues);
                            foreach (var category in LtsEventSelected)
                            {
                                _Content.Events.Add(category);
                            }
                            #endregion


                            #endregion

                            _ContentDA.Save();


                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = _Content.ID.ToString(),
                                Message = string.Format("Đã chuyển lại về phiên bản lúc: <b>{0}</b>", versionRevert.ModifyDate.ToString("dd/MM/yyyy hh:mm:ss tt"))
                            };
                        }
                        break;

                    case ActionType.Delete:
                        if (hasDelete)
                        {
                            lts_ContentItems = _ContentDA.getListByArrID(ArrID);
                            stbMessage = new StringBuilder();
                            foreach (var item in lts_ContentItems)
                            {

                                _ContentDA.Delete(item);
                                stbMessage.AppendFormat("Đã xóa nội dung <b>{0}</b>.<br />", Server.HtmlEncode(item.Title));
                            }
                            msg.ID = string.Join(",", ArrID);
                            _ContentDA.Save();
                            msg.Message = stbMessage.ToString();
                        }
                        break;


                    case ActionType.Approve:
                        if (hasApprove)
                        {
                            _Content = _ContentDA.getById(ArrID.FirstOrDefault());
                            Base.ApproveLog log = new Base.ApproveLog();
                            log.StatusID = Convert.ToInt32(Request["StatusID"]);
                            log.Description = Request["Description"];
                            log.CreatedDate = DateTime.Now;
                            log.UserID = User.Identity.GetUserId();
                            log.ContentID = _Content.ID;

                            _Content.StatusID = log.StatusID;
                            _ContentDA.QNewsDB.ApproveLogs.Add(log);
                            _ContentDA.Save();

                            var status = _ContentDA.QNewsDB.Status.Where(o => o.ID == log.StatusID).FirstOrDefault().Name;

                            msg = new JsonMessage()
                            {
                                Erros = false,
                                ID = _Content.ID.ToString(),
                                Message = string.Format("Đã thực hiện <strong>{1}</strong> nội dung <b>{0}</b>", Server.HtmlEncode(_Content.Title), status)
                            };
                        }
                        break;
                }

                if (string.IsNullOrEmpty(msg.Message))
                {
                    msg.Message = "Không có hành động nào được thực hiện.";
                    msg.Erros = true;
                }
            }
            catch (Exception e)
            {
                msg.Erros = true;
                msg.Message = e.StackTrace;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

    }
}