using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using QNews.Utils;
using Microsoft.AspNet.Identity;

namespace QPortal.Web
{
    /// <summary>
    /// Summary description for upload
    /// </summary>
    public class uploadify : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpPostedFile file = context.Request.Files["Filedata"];
            string fileType = file.FileName.Substring(file.FileName.LastIndexOf('.'));
            if (ListAllFileAllow().Contains(fileType.ToLower()))
            {
                //string targetDirectory = ConfigData.IMAGE_UPLOAD_TEMP_FOLDER;
                var targetDirectory = string.Empty;
                string targetFilePath = Path.Combine(targetDirectory, file.FileName);
                file.SaveAs(targetFilePath);
                context.Response.Write("1");
            }
            else
            {
                context.Response.Write("0");
            }
        }


        private List<string> ListAllFileAllow()
        {
            List<string> list = new List<string>();
            list.Add(".jpg");
            list.Add(".jpeg");
            list.Add(".gif");
            list.Add(".bmp");
            list.Add(".png");
            list.Add(".tif");
            return list;
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}