using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QNews.Utils
{
    public enum WorkFlowStatus: int
    {
        BAN_NHAP = 1,
        DA_DUYET = 2,
        //BAN_NHAP = 0,
        //DA_DUYET = 1,
        CHO_DUYET = 3,
        TRA_LAI = 4,
        TRANG_THAI_KHAC = 5

    }
}
