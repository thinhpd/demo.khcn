using QNews.Base;
using QNews.Data.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QNews.Data.Publishing
{
    public class RegisterDA: BaseDA
    {
        public bool checkExits(Register register)
        {
            var query = from c in QNewsDB.Registers where ((c.Email == register.Email) || (c.Phone != register.Phone)) select c;
            if (query.Count() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Lấy về bản ghi qua tên
        /// </summary>
        /// <param name="QuestionTitles">Tên bản ghi</param>
        /// <returns>Bản ghi</returns>
        

        /// <summary>
        /// Lấy về bản ghi qua tên
        /// </summary>
        /// <param name="Title">Tên bản ghi</param>
        /// <returns>Bản ghi</returns>
        
        public void Add(Register register)
        {
            QNewsDB.Registers.Add(register);
        }


        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="DocumentScope">Xóa bản ghi</param>
       

        /// <summary>
        /// save bản ghi vào DB
        /// </summary>
        public void Save()
        {
            QNewsDB.SaveChanges();
        }
        
    }
}
