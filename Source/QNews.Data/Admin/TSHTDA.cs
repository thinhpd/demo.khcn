using QNews.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QNews.Data.Admin
{
    public class TSHTDA : BaseDA
    { 
        public TSHT getTSHTByKey(string key)
        {
            var query = from c in QNewsDB.TSHTs where c.KeyTSHT == key select c;
            return query.FirstOrDefault();
        }

        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="clone_noderemove">bản ghi cần thêm</param>
        public void Add(TSHT TSHT)
        {
            QNewsDB.TSHTs.Add(TSHT);
            
        }
        public void Delete(TSHT TSHT)
        {
            QNewsDB.TSHTs.Remove(TSHT);
            //QNewsDB.Entry(TSHT).State=System.Data.Entity.EntityState.
        }
        public void update(TSHT TSHT)
        {
            //QNewsDB.TSHTs.Remove(TSHT);
            QNewsDB.Entry(TSHT).State = System.Data.Entity.EntityState.Modified;
        }
        public void Save()
        {
            QNewsDB.SaveChanges();
        }
    }
}
