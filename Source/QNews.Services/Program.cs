using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QNews.Services
{
    class Program
    {
        static void Main(string[] args)
        {

            //QNews.Data.Publishing.API.Login("318379955", "123456");

            IndexService idx = new IndexService();
            idx.ResetIndex();
            idx.Index_All(true);
            //idx.Index_All(false);
            
        }
    }

}
