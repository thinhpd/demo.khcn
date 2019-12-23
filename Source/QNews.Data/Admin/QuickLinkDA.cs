using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QNews.Base;

namespace QNews.Data.Admin
{
    public class QuickLinkDA : BaseDA
    {

        public QuickLink getQLLastest()
        {
            var query = from c in QNewsDB.QuickLinks where c.Show orderby c.Order descending select c;
            return query.FirstOrDefault();
        }
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="clone_noderemove">bản ghi cần thêm</param>
        public void Add(QuickLink quickLink)
        {
            QNewsDB.QuickLinks.Add(quickLink);

        }
        public void Delete(QuickLink quickLink)
        {
            QNewsDB.QuickLinks.Remove(quickLink);
            //QNewsDB.Entry(TSHT).State=System.Data.Entity.EntityState.
        }
        public void update(QuickLink quickLink)
        {
            //QNewsDB.TSHTs.Remove(TSHT);
            QNewsDB.Entry(quickLink).State = System.Data.Entity.EntityState.Modified;
        }
        public void Save()
        {
            QNewsDB.SaveChanges();
        }

    }
}
