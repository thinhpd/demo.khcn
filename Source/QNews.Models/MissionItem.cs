using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QNews.Models
{
    public class MissionItem
    {
        public int ID { get; set; } // ID (Primary key)

        public string MaNhiemVu { get; set; } // MaNhiemVu
        public string TenNhiemVu { get; set; } // TenNhiemVu
        public string ToChucChuTri { get; set; } // ToChucChuTri
        public string ChuNhiemNhiemVu { get; set; } // ChuNhiemNhiemVu
        public DateTime? BatDau { get; set; } // BatDau
        public DateTime? KetThuc { get; set; } // KetThuc
        public string NoiDung { get; set; } // NoiDung


        public DateTime CreateDate { get; set; } // CreateDate
        public string CreateBy { get; set; } // CreateBy
        public DateTime ModifyDate { get; set; } // ModifyDate
        public string ModifyBy { get; set; } // ModifyBy
        public int StatusID { get; set; } // StatusID
        public string Status { get; set; } // StatusID
        public bool IsRemoved { get; set; } // IsRemoved

        public List<string> LinhVuc { get; set; }

        public MissionItem()
        {
            LinhVuc = new List<string>();
        }
    }
}
