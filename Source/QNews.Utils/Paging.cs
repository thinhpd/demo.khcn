using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QNews.Utils
{

    public class PageItem
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public bool Current { get; set; }
        public bool Span { get; set; }
        public bool Link { get; set; }
    }

    public class Paging
    {
        #region Các biến xử dụng
        private int _PageStep;
        private int _CurrentPage;
        private string _LinkPage;
        private int _TotalPage;
        private string _LinkPageExt;
        #endregion

        #region Các thuộc tính

        public int PageStep
        {
            get { return _PageStep; }
            set { _PageStep = value; }
        }

        public int TotalPage
        {
            get { return _TotalPage; }
            set { _TotalPage = value; }
        }
        public int CurrentPage
        {
            get { return _CurrentPage; }
            set { _CurrentPage = value; }
        }

        public string LinkPage
        {
            get { return _LinkPage; }
            set { _LinkPage = value; }
        }
        public string LinkPageExt
        {
            get { return _LinkPageExt; }
            set { _LinkPageExt = value; }
        }
        #endregion

        /// <summary>
        /// Hàm Constructer
        /// </summary>
        /// <Modified>        
        ///	Name		Date		    Comment 
        /// QuangND     10/11/2009      Tạo mới
        /// </Modified>
        public Paging()
        {
            CurrentPage = 1;
            LinkPage = string.Empty;
            TotalPage = 1;
            PageStep = 3;
            LinkPageExt = "";
        }

        /// <summary>
        /// Hàm lấy về mã html phân trang
        /// </summary>
        /// <param name="_LinkPage">đường link của trang</param>
        /// <param name="_CurrentPage">Trang hiện tại</param>
        /// <param name="_RowPerPage">Số bản ghi trên trang</param>
        /// <param name="_TotalRow">Tổng số bản ghi</param>
        /// <Modified>        
        ///	Name		Date		    Comment 
        /// QuangND     10/11/2009      Tạo mới
        /// </Modified>
        public string getHtmlPage(string _LinkPage, int _PageStep, int _CurrentPage, int _RowPerPage, int _TotalRow)
        {
            this.PageStep = _PageStep;
            CurrentPage = _CurrentPage;
            LinkPage = _LinkPage;
            if (_RowPerPage == 0)
                _RowPerPage = 5;
            TotalPage = (_TotalRow % _RowPerPage == 0) ? _TotalRow / _RowPerPage : ((_TotalRow - (_TotalRow % _RowPerPage)) / _RowPerPage) + 1;
            return WriteHTMLPage();
        }

        /// <summary>
        /// Hàm lấy về phân trang, hỗ trợ cho urlRewrite
        /// </summary>
        /// <param name="_LinkPage">đường link của trang - Phía trước page</param>
        /// <param name="_LinkPageExt">đường link của trang - Phía sau page</param>
        /// <param name="_CurrentPage">Trang hiện tại</param>
        /// <param name="_RowPerPage">Số bản ghi trên trang</param>
        /// <param name="_TotalRow">Tổng số bản ghi</param>
        /// <Modified>        
        ///	Name		Date		    Comment 
        /// QuangND     10/11/2009      Tạo mới
        /// </Modified>
        public string getHtmlPage(string _LinkPage, string _LinkPageExt, int _PageStep, int _CurrentPage, int _RowPerPage, int _TotalRow)
        {
            this.PageStep = _PageStep;
            CurrentPage = _CurrentPage;
            LinkPage = _LinkPage;
            LinkPageExt = _LinkPageExt;
            TotalPage = (_TotalRow % _RowPerPage == 0) ? _TotalRow / _RowPerPage : ((_TotalRow - (_TotalRow % _RowPerPage)) / _RowPerPage) + 1;
            return WriteHTMLPage();
        }

        /// <summary>
        /// Phân trang version 3
        /// </summary>
        /// <param name="_PageStep"></param>
        /// <param name="_CurrentPage"></param>
        /// <param name="_RowPerPage"></param>
        /// <param name="_TotalRow"></param>
        /// <returns></returns>
        public List<PageItem> getPageItems(int _PageStep, int _CurrentPage, int _RowPerPage, int _TotalRow)
        {
            this.PageStep = _PageStep;
            CurrentPage = _CurrentPage;
            LinkPage = _LinkPage;
            LinkPageExt = _LinkPageExt;
            TotalPage = (_TotalRow % _RowPerPage == 0) ? _TotalRow / _RowPerPage : ((_TotalRow - (_TotalRow % _RowPerPage)) / _RowPerPage) + 1;
            List<PageItem> PageItems = new List<PageItem>();
            PageItem page;

            if (CurrentPage > PageStep + 1)
            {
                page = new PageItem();
                page.Text = "« Đầu";
                page.Value = "1";
                page.Link = true;
                PageItems.Add(page);

                page = new PageItem();
                page.Text = "Trước";
                page.Value = (CurrentPage - 1).ToString();
                page.Link = true;
                PageItems.Add(page);

                page = new PageItem();
                page.Span = true;
                page.Text = "...";
                PageItems.Add(page);

            }
            int BeginFor = ((CurrentPage - PageStep) > 1) ? (CurrentPage - PageStep) : 1;
            int EndFor = ((CurrentPage + PageStep) > TotalPage) ? TotalPage : (CurrentPage + PageStep);

            for (int pNumber = BeginFor; pNumber <= EndFor; pNumber++)
            {
                if (pNumber == CurrentPage)
                {
                    page = new PageItem();
                    page.Text = pNumber.ToString();
                    page.Current = true;
                    PageItems.Add(page);
                }
                else
                {
                    page = new PageItem();
                    page.Text = pNumber.ToString();
                    page.Value = pNumber.ToString();
                    page.Link = true;
                    PageItems.Add(page);
                }
            }

            if (CurrentPage < (TotalPage - PageStep))
            {
                page = new PageItem();
                page.Span = true;
                page.Text = "...";
                PageItems.Add(page);

                page = new PageItem();
                page.Text = "Sau";
                page.Value = (CurrentPage + 1).ToString();
                page.Link = true;
                PageItems.Add(page);

                page = new PageItem();
                page.Text = "Cuối »";
                page.Value = TotalPage.ToString();
                page.Link = true;
                PageItems.Add(page);
            }

            return PageItems;
        }

        /// <summary>
        /// Hàm write mã HTML phân trang
        /// </summary>
        /// <Modified>        
        ///	Name		Date		    Comment 
        /// QuangND     10/11/2009      Tạo mới
        /// </Modified>
        private string WriteHTMLPage()
        {
            string strPageHTML = "<div class=\"pageing-container\"><div class=\"paging\">";

            if (CurrentPage > PageStep + 1)
            {
                strPageHTML += "<a href=\"" + LinkPage + 1 + LinkPageExt + "\">« Đầu</a>";
                strPageHTML += "<a href=\"" + LinkPage + (CurrentPage - 1) + LinkPageExt + "\">Trước</a>";
                strPageHTML += "<span>...</span>";
            }

            int BeginFor = ((CurrentPage - PageStep) > 1) ? (CurrentPage - PageStep) : 1;
            int EndFor = ((CurrentPage + PageStep) > TotalPage) ? TotalPage : (CurrentPage + PageStep);

            for (int pNumber = BeginFor; pNumber <= EndFor; pNumber++)
            {
                if (pNumber == CurrentPage)
                    strPageHTML += "<a href=\"javascript:;\" class=\"current\">" + pNumber + "</a>";
                else
                    strPageHTML += "<a href=\"" + LinkPage + pNumber + LinkPageExt + "\">" + pNumber + "</a>";
            }

            if (CurrentPage < (TotalPage - PageStep))
            {
                strPageHTML += "<span>...</span>";
                strPageHTML += "<a href=\"" + LinkPage + (CurrentPage + 1) + LinkPageExt + "\">Sau</a>";
                strPageHTML += "<a href=\"" + LinkPage + TotalPage + LinkPageExt + "\">Cuối »</a>";

            }
            strPageHTML += "</div></div>";
            if (TotalPage > 1)
                return  strPageHTML;
            else
                return string.Empty;
        }
    }
}
