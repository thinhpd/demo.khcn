using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QNews.Utils
{
    [Serializable]
    public class FileAttachForm
    {
        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        private string fileServer;

        public string FileServer
        {
            get { return fileServer; }
            set { fileServer = value; }
        }

        public FileAttachForm()
        {
        
        }

        public FileAttachForm(string _filename, string _fileserver)
        {
            fileName = _filename;
            fileServer = _fileserver;
        }
    }
}
