using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QNews.Models
{
    public class DocumentItem
    {
        public int ID { get; set; } // ID (Primary key)
        public string SoKyHieu { get; set; } // SoKyHieu
        public DateTime? NgayBanHanh { get; set; } // NgayBanHanh
        public DateTime? NgayHieuLuc { get; set; } // NgayHieuLuc
        public string TrichYeu { get; set; } // TrichYeu
        public int LoaiVanBanID { get; set; } // LoaiVanBanID
        public string LoaiVanBan { get; set; } // LoaiVanBanID
        public string NguoiKy { get; set; } // NguoiKy
        public string FileAttach { get; set; } // FileAttach
        public DateTime CreateDate { get; set; } // CreateDate
        public string CreateBy { get; set; } // CreateBy
        public DateTime ModifyDate { get; set; } // ModifyDate
        public string ModifyBy { get; set; } // ModifyBy
        public int StatusID { get; set; } // StatusID
        public string Status { get; set; } // StatusID
        public bool IsRemoved { get; set; } // IsRemoved
        public bool AllowComment { get; set; } // AllowComment

        public List<string> LinhVuc { get; set; }

        public DocumentItem()
        {
            LinhVuc = new List<string>();
        }
    }
}
