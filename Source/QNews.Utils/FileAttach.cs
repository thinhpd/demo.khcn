using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QNews.Utils
{
    /// <summary>
    /// Class chứa thông tin file đính kèm của một SPlistItem
    /// </summary>
    /// <Modified>        
    ///	Name		Date		    Comment 
    /// QuangND     25/09/2010      Tạo mới
    /// </Modified>
    public class FileAttach1
    {
        private string strName;
        public string Name
        {
            get { return strName; }
            set { strName = value; }
        }

        private string strUrl;
        public string Url
        {
            get { return strUrl; }
            set { strUrl = value; }
        }

        private byte[] byteDataFile;

        public byte[] DataFile
        {
            get { return byteDataFile; }
            set { byteDataFile = value; }
        }

        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="name">Tên file</param>
        ///	<param name="url">đường link file</param>
        /// <Modified>        
        ///	Name		Date		    Comment 
        /// QuangND     25/09/2010      Tạo mới
        /// </Modified>
        public FileAttach1(string name, string url)
        {
            Name = name;
            Url = url;
        }

        public FileAttach1(string name, byte[] data)
        {
            this.Name = name;
            this.DataFile = data;
        }

        public FileAttach1()
        {
            Name = string.Empty;
            Url = string.Empty;
        }
    }
}
