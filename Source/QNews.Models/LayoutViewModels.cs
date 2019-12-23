using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QNews.Models
{


    public class LayoutViewModels
    {

        public List<ContentViewModel> TopNews { get; set; }
        public List<QNews.Models.CategoryViewModel> LtsAllCategory { get; set; }
        public List<QNews.Models.VideoItem> LtsVideo { get; set; }
        public List<QNews.Models.AudioItem> LtsAudio { get; set; }
        public List<QNews.Models.AdvertiseItem> LtsAds { get; set; }
        public List<QNews.Models.SiteLinkItem> LtsSiteLink { get; set; }

        public List<QNews.Models.ContentItem> LtsRightContent { get; set; }

        public List<AlbumItem> LtsAlbum { get; set; }

        public TSHTItem footer { get; set; }

        public AlbumItem FirstAlbum { get; set; }
        public LayoutViewModels()
        {
            TopNews = new List<ContentViewModel>();
            LtsAllCategory = new List<CategoryViewModel>();
            LtsVideo = new List<VideoItem>();
            LtsAudio = new List<AudioItem>();
            LtsAds = new List<AdvertiseItem>();
            LtsSiteLink = new List<SiteLinkItem>();
            LtsAlbum = new List<AlbumItem>();
            FirstAlbum = new AlbumItem();
            footer =new TSHTItem();
        }
    }
}
