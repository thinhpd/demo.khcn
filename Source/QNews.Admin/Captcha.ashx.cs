using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace QNews.Web
{
    /// <summary>
    /// Summary description for Captcha
    /// </summary>
    public class Captcha : IHttpHandler, IRequiresSessionState 
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
            context.Session["CaptchaImageText"] = GenerateRandomCode();
            CaptchaImage.CaptchaImage ci = new CaptchaImage.CaptchaImage(context.Session["CaptchaImageText"].ToString(), 240, 80, "Tahoma");
            context.Response.Clear();
            context.Response.ContentType = "image/jpeg";
            ci.Image.Save(context.Response.OutputStream, ImageFormat.Jpeg);
            ci.Dispose();
        }

        private Random random = new Random();
        private string GenerateRandomCode()
        {
            string s = "";
            for (int i = 0; i < 5; i++)
                s = String.Concat(s, this.random.Next(10).ToString());
            return s;
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