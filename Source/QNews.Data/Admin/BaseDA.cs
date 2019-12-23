using QNews.Models;
using QNews.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QNews.Data.Admin
{
    public class ParramRequest
    {
        public ParramRequest() { }

        public ParramRequest(HttpRequestBase request)
        {

            DateTimeFormatInfo dtinfo = new DateTimeFormatInfo();
            dtinfo.ShortDatePattern = "dd/MM/yyyy";
            dtinfo.DateSeparator = "/";
            if (!string.IsNullOrEmpty(request["TuNgay"]))
                TuNgay = Convert.ToDateTime(request["TuNgay"], dtinfo);

            if (!string.IsNullOrEmpty(request["DenNgay"]))
                DenNgay = Convert.ToDateTime(request["DenNgay"], dtinfo);


            if (!string.IsNullOrEmpty(request["ModifyBy"]))
                ModifyBy = Convert.ToString(request["ModifyBy"]);
            else
                ModifyBy = string.Empty;

            if (!string.IsNullOrEmpty(request["CreateBy"]))
                CreateBy = Convert.ToString(request["CreateBy"]);
            else
                CreateBy = string.Empty;

            if (!string.IsNullOrEmpty(request["AlbumID"]))
                AlbumID = Convert.ToInt32(request["AlbumID"]);
            else
                AlbumID = 0;

            if (!string.IsNullOrEmpty(request["StatusID"]))
                StatusID = Convert.ToInt32(request["StatusID"]);
            else
                StatusID = 0;

            if (!string.IsNullOrEmpty(request["EventID"]))
                EventID = Convert.ToInt32(request["EventID"]);
            else
                EventID = 0;

            if (!string.IsNullOrEmpty(request["IsShow"]))
                IsShow = Convert.ToBoolean(request["IsShow"]);

            if (!string.IsNullOrEmpty(request["CategoryID"]))
                CategoryID = Convert.ToInt32(request["CategoryID"]);
            else
                CategoryID = 0;

            if (!string.IsNullOrEmpty(request["Page"]))
                CurrentPage = Convert.ToInt32(request["page"]);
            else
                CurrentPage = 1;

            if (!string.IsNullOrEmpty(request["RowPerPage"]))
                RowPerPage = Convert.ToInt32(request["RowPerPage"]);
            else
                RowPerPage = 10;

            if (!string.IsNullOrEmpty(request["Keyword"]))
                Keyword = request["Keyword"].Trim();
            else
                Keyword = string.Empty;

            if (!string.IsNullOrEmpty(request["Field"]))
                FieldSort = request["Field"].Trim();
            else
                FieldSort = string.Empty;

            if (!string.IsNullOrEmpty(request["FieldOption"]))
                TypeSort = Convert.ToInt32(request["FieldOption"]) > 0;
            else
                TypeSort = false;


            if (!string.IsNullOrEmpty(request["EditorID"]))
                EditorID = Convert.ToInt32(request["EditorID"]);
            else
                EditorID = 0;

            if (!string.IsNullOrEmpty(request["AuthorID"]))
                AuthorID = Convert.ToInt32(request["AuthorID"]);
            else
                AuthorID = 0;

            if (!string.IsNullOrEmpty(request["PhotoID"]))
                PhotoID = Convert.ToInt32(request["PhotoID"]);
            else
                PhotoID = 0;


            SearchInField = new List<string>();
            if (!string.IsNullOrEmpty(request["SearchIn"]))
            {
                string temp = request["SearchIn"];
                if (temp.IndexOf(',') > 0)
                    SearchInField = temp.Split(',').ToList();
                else
                    SearchInField.Add(temp);
            }
        }

        public int EditorID { get; set; }
        public int AuthorID { get; set; }
        public int PhotoID { get; set; }


        public string UserId { get; set; }

        public DateTime? TuNgay { get; set; }

        public DateTime? DenNgay { get; set; }

        public int CategoryID { get; set; }
        public int StatusID { get; set; }

        public int EventID { get; set; }
        public bool? IsShow { get; set; }
        public int AlbumID { get; set; }
        public int CurrentPage { get; set; }
        public int RowPerPage { get; set; }
        public string Keyword { get; set; }
        public List<string> SearchInField { get; set; }
        public string FieldSort { get; set; }
        public bool TypeSort { get; set; }
        public int StatusIDCategoryID { get; set; }

        public string CreateBy { get; set; }

        public string ModifyBy { get; set; }

        public override string ToString()
        {
            string toString = string.Empty;
            toString = string.Format("Keyword={0}&CategoryID={5}&StatusID={7}&EventID={8}&IsShow={9}&ModifyBy={10}&CreateBy={11}&TuNgay={12}&DenNgay={13}&AlbumID={6}&SearchIn={4}&RowPerPage={1}&Field={2}&FieldOption={3}&Page=", Keyword, RowPerPage, FieldSort, (TypeSort) ? 1 : 0, string.Join(",", SearchInField), CategoryID, AlbumID,
                StatusID, EventID, ((IsShow.HasValue) ? IsShow.Value.ToString() : string.Empty), ModifyBy, CreateBy, (TuNgay.HasValue) ? TuNgay.Value.ToString("dd/MM/yyyy") : string.Empty, (DenNgay.HasValue) ? DenNgay.Value.ToString("dd/MM/yyyy") : string.Empty);

            toString = string.Format("AuthorID={0}&PhotoID={1}&EditorID={2}&{3}", AuthorID, PhotoID, EditorID, toString);

            return toString;
        }

        public string GetCategoryString()
        {
            string toString = string.Empty;
            toString = string.Format("Keyword={0}&SearchIn={4}&AlbumID={5}&StatusID={6}&EventID={7}&IsShow={8}&ModifyBy={9}&CreateBy={10}&TuNgay={11}&DenNgay={12}&RowPerPage={1}&Field={2}&FieldOption={3}&Page=1&CategoryID=", Keyword, RowPerPage, FieldSort, (TypeSort) ? 1 : 0, string.Join(",", SearchInField), AlbumID
                , StatusID, EventID, ((IsShow.HasValue) ? IsShow.Value.ToString() : string.Empty), ModifyBy, CreateBy, (TuNgay.HasValue) ? TuNgay.Value.ToString("dd/MM/yyyy") : string.Empty, (DenNgay.HasValue) ? DenNgay.Value.ToString("dd/MM/yyyy") : string.Empty);

            toString = string.Format("AuthorID={0}&PhotoID={1}&EditorID={2}&{3}", AuthorID, PhotoID, EditorID, toString);

            return toString;
        }

        public string ParramArr
        {
            get
            {
                return string.Format("Keyword={0}&RowPerPage={1}&Field={2}&FieldOption={3}&Page=", Keyword, RowPerPage, FieldSort, (TypeSort) ? 1 : 0);
            }
        }

        public string SortUrl
        {
            get
            {
                return string.Format("FieldOption={2}&Keyword={0}&SearchIn={1}", Keyword, string.Join(",", SearchInField), (TypeSort) ? 1 : 0);
            }
        }
    }

    public class BaseDA : IDisposable
    {

        public List<UserItem> getAllUserSimple()
        {
            var Items = (from c in QNewsDB.AspNetUsers
                         select new UserItem()
                         {
                             ID = c.Id,
                             UserName = c.UserName
                         }).ToList();

            return Items;
        }

        public List<StatusItem> getAllStatusSimple()
        {
            var Items = (from c in QNewsDB.Status
                         select new StatusItem()
                         {
                             ID = c.ID,
                             Name = c.Name
                         }).ToList();

            return Items;
        }


        protected Base.QNewsDBContext _QNewsDB = new Base.QNewsDBContext();
        public Base.QNewsDBContext QNewsDB { get { return _QNewsDB; } }

        private Utils.Paging objPaging = new Utils.Paging();
        public int TotalRecord = 0;
        public string PathPaging { get; set; }
        public string PathPagingext { get; set; }
        //public int CurrentPage { get; set; }
        //public int PageSize { get; set; }

        public ParramRequest Request { get; set; }

        public List<PageItem> PageItems
        {
            get
            {
                return objPaging.getPageItems(3, Request.CurrentPage, Request.RowPerPage, TotalRecord);
            }
        }

        public string HTMLPageing
        {
            get
            {
                return objPaging.getHtmlPage(PathPaging, PathPagingext, 3, Request.CurrentPage, Request.RowPerPage, TotalRecord);
            }
        }

        public string GridHtmlPage
        {
            get
            {
                return Utils.Pager.getPage(PathPaging + Request.ToString(), Request.CurrentPage, Request.RowPerPage, TotalRecord);
            }
        }

        #region cơ chế dọn rác
        public BaseDA()
        {

        }

        private bool IsDisposed = false;
        public void Free()
        {
            if (IsDisposed)
                throw new System.ObjectDisposedException("Object Name");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~BaseDA()
        {
            //Pass false as param because no need to free managed resources when you call finalize it
            //by GC itself as its work of finalize to manage managed resources.
            Dispose(false);
        }

        //Implement dispose to free resources
        protected virtual void Dispose(bool disposedStatus)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                _QNewsDB.Dispose(); // Released unmanaged Resources
                if (disposedStatus)
                {
                    // Released managed Resources
                }
            }
        }
        #endregion
    }
}
