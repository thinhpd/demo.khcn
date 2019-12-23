using QNews.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QNews.Models
{

    public class SearchResult
    {

        public int TotalRowFacet { get; set; }
        public string HtmlPage { get; set; }
        public string Kwd { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public List<Publishing.SearchItem> Items { get; set; }

        public int RowPerPage { get; set; }
        public int CurrentPage { get; set; }

        public int TotalRow { get; set; }

        public string ContentType { get; set; }

        public List<FacetItem> Facet { get; set; }
        public SearchResult()
        {
            Items = new List<Publishing.SearchItem>();
            Facet = new List<FacetItem>();
        }
    }

    public class FacetItem
    {
        public string UrlFacet { get; set; }

        public string Title { get; set; }

        public int Total { get; set; }
    }


    public class WeatherViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }

        public string MinTemper { get; set; }

        public string MaxTemper { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }
    }

    public class UrlViewModel
    {

        public string UrlID { get; set; } // UrlID (Primary key)
        public int? ContentID { get; set; } // ContentID
        public int? CategoryID { get; set; } // CategoryID

        public int? DocumentID { get; set; }

        public int? AlbumID { get; set; }


        public AlbumViewModel Album { get; set; }

        public DocumentViewModel Document { get; set; }

        public CategoryViewModel Category { get; set; }

        public ContentViewModel Content { get; set; }

        public List<CategoryViewModel> LtsMap { get; set; }
        public UrlViewModel()
        {
            Album = new AlbumViewModel();
            Category = new CategoryViewModel();
            Content = new ContentViewModel();
            LtsMap = new List<CategoryViewModel>();
        }
    }

    public class MenuViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public int? ParrentID { get; set; }

        public string Order { get; set; }


    }


    public class ListDocumentViewModel
    {
        public int DocumentTypeID { get; set; }
        public List<DocumentViewModel> LtsItem { get; set; }



        public string Title { get; set; }

        public string PageHtml { get; set; }

        public int CurrentPage { get; set; }
        public int Total { get; set; }
        public ListDocumentViewModel()
        {
            LtsItem = new List<DocumentViewModel>();
        }
    }

    public class MissionViewModal
    {
        public int ID { get; set; } // ID (Primary key)
        public string MaNhiemVu { get; set; } // MaNhiemVu
        public string TenNhiemVu { get; set; } // TenNhiemVu
        public string ToChucChuTri { get; set; } // ToChucChuTri
        public string ChuNhiemNhiemVu { get; set; } // ChuNhiemNhiemVu
        public DateTime? BatDau { get; set; } // BatDau
        public DateTime? KetThuc { get; set; } // KetThuc
        public string Details { get; set; } // NoiDung

        public List<MissionViewModal> LtsOther { get; set; }

        public MissionViewModal()
        {
            LtsOther = new List<MissionViewModal>();
        }

    }



    public class DocumentViewModel
    {
        public List<string> CoQuanBanHanh { get; set; }

        public List<string> LinhVuc { get; set; }
        public int ID { get; set; } // ID (Primary key)
        public string SoKyHieu { get; set; } // SoKyHieu
        public DateTime? NgayBanHanh { get; set; } // NgayBanHanh
        public DateTime? NgayHieuLuc { get; set; } // NgayHieuLuc
        public string TrichYeu { get; set; } // TrichYeu
        public int LoaiVanBanID { get; set; } // LoaiVanBanID
        public string NguoiKy { get; set; } // NguoiKy
        public string FileAttach { get; set; } // FileAttach

        public string Url { get; set; }

        public string DocumentType { get; set; }

        public List<DocumentViewModel> LtsOtherDocument { get; set; }

        public DocumentViewModel()
        {
            CoQuanBanHanh = new List<string>();
            LinhVuc = new List<string>();
        }

    }
    public class AboutViewModel
    {
        public ContentItem Gioithieuchung { get; set; }
        public QuickLinkItem quickLink { get; set; }
        public Register DKNTItem { get; set; }


        public string dkName { get; set; }
        public string dkEMail { get; set; }
        public string dkphone { get; set; }
        public int ct_id { get; set; }
        public string jmessage { get; set; }
        public AboutViewModel()
        {

        }

    }

        public class HomeViewModel
    {

        //public List<DocumentViewModel> LtsDocument { get; set; }
        public AlbumItem FirstAlbum { get; set; }
        public List<AlbumItem> LtsAlbum { get; set; }
        public List<AlbumItem> LtsAlbumHome { get; set; }
        public List<AdvertiseItem> LtsAD { get; set; }

        public List<CategoryViewModel> LtsCategory { get; set; }

        public List<ContentViewModel> LtsHotNews { get; set; }
        public List<PartnerItem> LtsPartner { get; set; }
        public CategoryViewModel ChuongTrinh { get; set; }


        public List<ChuongTrinhHomeViewModel> ChuongTrinhIndex { get; set; }
        public List<ChuongTrinhHomeViewModelNew> ChuongTrinhIndexNew { get; set; }

        public List<TagetItems> lstTagetItems { get; set; }
        public QuickLinkItem quickLink { get; set; }
        public Register DKNTItem { get; set; }


        public string dkName { get; set; }
        public string dkEMail { get; set; }
        public string dkphone { get; set; }
        public int ct_id { get; set; }
        public string jmessage { get; set; }
        public HomeViewModel()
        {
            LtsCategory = new List<CategoryViewModel>();
            LtsHotNews = new List<ContentViewModel>();
            LtsAD = new List<AdvertiseItem>();
            LtsAlbum = new List<AlbumItem>();
            FirstAlbum = new AlbumItem();
            quickLink = new QuickLinkItem();
            //ChuongTrinh = new CategoryViewModel();
            ChuongTrinhIndex = new List<ChuongTrinhHomeViewModel>();
            ChuongTrinhIndexNew = new List<ChuongTrinhHomeViewModelNew>();
            lstTagetItems = new List<TagetItems>();
            LtsAlbumHome = new List<AlbumItem>();
            LtsPartner = new List<PartnerItem>();
            DKNTItem = new Register();
        }
    }

    public class ChuongTrinhHomeViewModelNew
    {
        public List<DocumentViewModel> LtsDocument { get; set; }

        public int ID { get; set; }
        public string Title { get; set; }

        public string Image { get; set; }

        public ChuongTrinhHomeViewModelNew()
        {
            
        }
    }
    public class ChuongTrinhHomeViewModel
    {
        public List<DocumentViewModel> LtsDocument { get; set; }

        public int ID { get; set; }
        public string Title { get; set; }

        public ContentViewModel ThongTinChung { get; set; }

        public ChuongTrinhHomeViewModel()
        {
            LtsDocument = new List<DocumentViewModel>();
            ThongTinChung = new ContentViewModel();
        }
    }
    public class CategoryViewModel
    {
        public List<CategoryViewModel> LtsMap { get; set; }
        public int ID { get; set; } // ID (Primary key)
        public string Name { get; set; } // Name
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
        public QuickLinkItem quickLink { get; set; }
        public bool ShowInRight { get; set; }
        public int Total { get; set; }

        public string dkName { get; set; }
        public string dkEMail { get; set; }
        public string dkphone { get; set; }
        public int ct_id { get; set; }
        public string jmessage { get; set; }
        public List<ChuongTrinhHomeViewModelNew> ChuongTrinhIndexNew { get; set; }
        public string Url { get; set; }

        public List<ContentViewModel> LtsContent { get; set; }

        public List<ContentViewModel> LtsContentOther { get; set; }

        public string PageHtml { get; set; }

        public int CurrentPage { get; set; }

        public List<CategoryViewModel> ChildCategory { get; set; }


        public List<DocumentViewModel> LtsDocument { get; set; }

        public List<MissionViewModal> LtsMission { get; set; }

        public CategoryViewModel()
        {
            LtsContent = new List<ContentViewModel>();
            LtsMap = new List<CategoryViewModel>();
            LtsContentOther = new List<ContentViewModel>();
            ChildCategory = new List<CategoryViewModel>();
            LtsDocument = new List<DocumentViewModel>();
        }
    }

    public class AlbumViewModel
    {

        public List<CategoryViewModel> LtsMap { get; set; }
        public int ID { get; set; } // ID (Primary key)
        public string Title { get; set; } // Title
        public string Description { get; set; } // Description
        public string Image { get; set; } // Image
        public string Details { get; set; } // Details
        public DateTime? PublishDate { get; set; } // PublishDate
        public DateTime CreateDate { get; set; } // CreateDate
        public string CreateBy { get; set; } // CreateBy
        public DateTime ModifyDate { get; set; } // ModifyDate
        public string ModifyBy { get; set; } // ModifyBy
        public int Viewed { get; set; } // Viewed
        public int StatusID { get; set; } // StatusID
        public bool AllowComment { get; set; } // AllowComment
        public bool IsRemoved { get; set; } // IsRemoved
        public string SourceUrl { get; set; } // SourceUrl
        public string Url { get; set; }

        public List<AlbumPictureItem> LtsPicture { get; set; }
        public int CurrentCategoryID { get; set; }

        public List<ContentViewModel> LtsOtherContent { get; set; }


        public List<ContentViewModel> LtsNewerContent { get; set; }

        public AlbumViewModel()
        {
            LtsMap = new List<CategoryViewModel>();
            LtsOtherContent = new List<ContentViewModel>();
            LtsNewerContent = new List<ContentViewModel>();
            LtsPicture = new List<AlbumPictureItem>();
        }
    }



    public class ContentViewModel
    {

        public List<CategoryViewModel> LtsMap { get; set; }
        public int ID { get; set; } // ID (Primary key)
        public string Title { get; set; } // Title
        public string Description { get; set; } // Description
        public string Image { get; set; } // Image
        public string Details { get; set; } // Details
        public DateTime? PublishDate { get; set; } // PublishDate
        public DateTime CreateDate { get; set; } // CreateDate
        public string CreateBy { get; set; } // CreateBy
        public DateTime ModifyDate { get; set; } // ModifyDate
        public string ModifyBy { get; set; } // ModifyBy
        public int Viewed { get; set; } // Viewed
        public int StatusID { get; set; } // StatusID
        public string Source { get; set; } // Source
        public bool AllowComment { get; set; } // AllowComment
        public bool IsRemoved { get; set; } // IsRemoved
        public string SourceUrl { get; set; } // SourceUrl
        public string Url { get; set; }

        public bool ShowInRight { get; set; }
        public int CurrentCategoryID { get; set; }

        public bool ShowDate { get; set; }

        public bool ShowOther { get; set; }
        public List<ContentViewModel> LtsOtherContent { get; set; }


        public List<ContentViewModel> LtsNewerContent { get; set; }

        public ContentViewModel()
        {
            LtsMap = new List<CategoryViewModel>();
            LtsOtherContent = new List<ContentViewModel>();
            LtsNewerContent = new List<ContentViewModel>();
        }
    }


}
