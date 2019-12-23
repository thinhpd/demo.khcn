using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QNews.Utils
{


    public static class Configs
    {
        public static string SITE_NAME = "Trang thông tin điện tử Văn phòng các chương trình Khoa học và Công nghệ Quốc gia";

    }

    public static class StaticClass
    {
        public static string getDesFromContent(string Content)
        {
            if (string.IsNullOrEmpty(Content))
                return string.Empty;

            string des = string.Empty;
            Content = Regex.Replace(Content, @"<[^>]*>", String.Empty);
            if (Content.Contains("."))
            {
                foreach (var paragprap in Content.Split('.'))
                {
                    if (des.Length > 150)
                    {
                        break;
                    }
                    des += paragprap.Trim() + ". ";
                }
            }
            else
            {
                des = (Content.Length > 300) ? Content.Substring(0, 255) + "..." : Content;
            }
            return des;
        }
        
        public static string FormatLongDate(DateTime now)
        {
            string stringReturn = now.ToString("hh:mm tt - ");
            string extReturn = now.ToString("dd/MM/yyyy");
            switch (now.DayOfWeek)
            {

                case DayOfWeek.Friday:
                    return stringReturn + "Thứ sáu, " + extReturn;
                case DayOfWeek.Monday:
                default:
                    return stringReturn + "Thứ hai, " + extReturn;
                case DayOfWeek.Saturday:
                    return stringReturn + "Thứ bảy, " + extReturn;
                case DayOfWeek.Sunday:
                    return stringReturn + "Chủ nhật, " + extReturn;
                case DayOfWeek.Thursday:
                    return stringReturn + "Thứ năm, " + extReturn;
                case DayOfWeek.Tuesday:
                    return stringReturn + "Thứ ba, " + extReturn;
                case DayOfWeek.Wednesday:
                    return stringReturn + "Thứ tư, " + extReturn;
            }
        }

        public static string GetDateText()
        {
            DateTime now = DateTime.Now;
            string stringReturn = now.ToString(" dd/MM/yyyy HH:mm");
            switch (now.DayOfWeek)
            { 
                    
                case DayOfWeek.Friday:
                    return "Thứ sáu, " + stringReturn;
                case DayOfWeek.Monday:
                default:
                    return "Thứ hai, " + stringReturn;
                case DayOfWeek.Saturday:
                    return "Thứ bảy, " + stringReturn;
                case DayOfWeek.Sunday:
                    return "Chủ nhật, " + stringReturn;
                case DayOfWeek.Thursday:
                    return "Thứ năm, " + stringReturn;
                case DayOfWeek.Tuesday:
                    return "Thứ ba, " + stringReturn;
                case DayOfWeek.Wednesday:
                    return "Thứ năm, " + stringReturn;
            }
            
        }

        public static List<FileAttach> ConvertFileAttach(string source)
        {
            List<FileAttach> LtsItem = new List<FileAttach>();
            if (!string.IsNullOrEmpty(source))
            {
                if (source.Contains("\n"))
                {
                    foreach (var item in source.Split('\n'))
                    {
                        //http://www.hochiminhcity.gov.vn/nguoidan-doanhnghiep/hethongvanban/_layouts/WordViewer.aspx?id=http://www.hochiminhcity.gov.vn/nguoidan-doanhnghiep/hethongvanban/Lists/VanBanDH/Attachments/32/th%C3%B4ngt%C6%B0%2032.doc&amp;source=/nguoidan-doanhnghiep/hethongvanban/
                        if (item.ToLower().Contains("wordviewer"))
                        {
                            FileAttach file = new FileAttach();
                            file.FileUrl = item;
                            file.FileIcon = "doc";
                            file.FileName = item.Replace("&source=/nguoidan-doanhnghiep/hethongvanban/", "");
                            file.FileName = file.FileName.Replace("/nguoidan-doanhnghiep/hethongvanban/_layouts/WordViewer.aspx?id=", "");
                            file.FileName = System.Web.HttpUtility.UrlDecode(file.FileName);
                            file.FileName = source.Substring(file.FileName.LastIndexOf("/") + 1);
                            file.FileName = file.FileName.Replace("&amp;source=/nguoidan-doanhnghiep/hethongvanban/", "");
                            LtsItem.Add(file);
                        }
                        else
                            LtsItem.Add(getFileAttach(item.Trim()));
                    }
                }
                else
                {
                    if (source.ToLower().Contains("wordviewer"))
                    {
                        FileAttach file = new FileAttach();
                        file.FileUrl = source;
                        file.FileIcon = "doc";
                        file.FileName = source.Replace("&source=/nguoidan-doanhnghiep/hethongvanban/", "");
                        file.FileName = file.FileName.Replace("/nguoidan-doanhnghiep/hethongvanban/_layouts/WordViewer.aspx?id=", "");
                        file.FileName = System.Web.HttpUtility.UrlDecode(file.FileName);
                        file.FileName = source.Substring(file.FileName.LastIndexOf("/") + 1);
                        file.FileName = file.FileName.Replace("&amp;source=/nguoidan-doanhnghiep/hethongvanban/", "");
                        LtsItem.Add(file);
                    }
                    else
                        LtsItem.Add(getFileAttach(source.Trim()));
                }
            }
            return LtsItem;
        }

        public static FileAttach getFileAttach(string source)
        {
            FileAttach file = new FileAttach();
            if (!string.IsNullOrEmpty(source))
            {
                file.FileUrl = source;
                file.FileName = source.Substring(source.LastIndexOf("/") + 1);
                file.FileIcon = source.Substring(source.LastIndexOf(".") + 1);
            }
            return file;
        }

        public static string RandomString(int size)
        {
            Random _rng = new Random();
            string _chars = "ABCDEFGHIJKLMNPQRSTUVWXYZ";
            char[] buffer = new char[size];

            for (int i = 0; i < size; i++)
            {
                buffer[i] = _chars[_rng.Next(_chars.Length)];
            }
            return new string(buffer);
        }

        public static List<string> GrapTextLabel(List<string> values)
        {
            List<string> ValueReturns = new List<string>();
            try
            {
                foreach (var item in values)
                {
                    string[] kvl = Regex.Split(item, "\\|\\|");
                    ValueReturns.Add(kvl[1].Trim());
                }
            }
            catch
            {
            }
            return ValueReturns;
        }

        public static string So_chu(double gNum)
        {
            if (gNum == 0.0)
            {
                return "Kh\x00f4ng đồng";
            }
            string str = "";
            string str2 = "";
            string str3 = "";
            double num = Math.Round(gNum, 0);
            string str4 = Convert.ToString(num);
            int num2 = Convert.ToInt32((int)(str4.Length / 3));
            int startIndex = str4.Length - (num2 * 3);
            string str5 = "[+]";
            if (gNum < 0.0)
            {
                str5 = "[-]";
            }
            str5 = "";
            if (startIndex.Equals(1))
            {
                str2 = "00" + Convert.ToString(num.ToString().Trim().Substring(0, 1)).Trim();
            }
            if (startIndex.Equals(2))
            {
                str2 = "0" + Convert.ToString(num.ToString().Trim().Substring(0, 2)).Trim();
            }
            if (startIndex.Equals(0))
            {
                str2 = "000";
            }
            if (num.ToString().Length > 2)
            {
                str3 = Convert.ToString(num.ToString().Trim().Substring(startIndex, num.ToString().Length - startIndex)).Trim();
            }
            int num4 = num2 + 1;
            if (startIndex > 0)
            {
                str = Tach(str2).ToString().Trim() + " " + Donvi(num4.ToString().Trim());
            }
            int num5 = num2;
            int num6 = num2;
            int num7 = 1;
            string str6 = "";
            string str7 = "";
            while (num5 > 0)
            {
                str6 = str3.Trim().Substring(0, 3).Trim();
                str7 = str6;
                str = str.Trim() + " " + Tach(str6.Trim()).Trim();
                num2 = (num6 + 1) - num7;
                if (!str7.Equals("000"))
                {
                    str = str.Trim() + " " + Donvi(num2.ToString().Trim()).Trim();
                }
                str3 = str3.Trim().Substring(3, str3.Trim().Length - 3);
                num5--;
                num7++;
            }
            if (str.Trim().Substring(0, 1).Equals("k"))
            {
                str = str.Trim().Substring(10, str.Trim().Length - 10).Trim();
            }
            if (str.Trim().Substring(0, 1).Equals("l"))
            {
                str = str.Trim().Substring(2, str.Trim().Length - 2).Trim();
            }
            if (str.Trim().Length > 0)
            {
                str = str5.Trim() + " " + str.Trim().Substring(0, 1).Trim().ToUpper() + str.Trim().Substring(1, str.Trim().Length - 1).Trim() + " đồng chẵn.";
            }
            return str.ToString().Trim();
        }

        private static string Chu(string gNumber)
        {
            switch (gNumber)
            {
                case "0":
                    return "kh\x00f4ng";

                case "1":
                    return "một";

                case "2":
                    return "hai";

                case "3":
                    return "ba";

                case "4":
                    return "bốn";

                case "5":
                    return "năm";

                case "6":
                    return "s\x00e1u";

                case "7":
                    return "bảy";

                case "8":
                    return "t\x00e1m";

                case "9":
                    return "ch\x00edn";
            }
            return "";
        }

        private static string Donvi(string so)
        {
            string str = "";
            if (so.Equals("1"))
            {
                str = "";
            }
            if (so.Equals("2"))
            {
                str = "ngh\x00ecn";
            }
            if (so.Equals("3"))
            {
                str = "triệu";
            }
            if (so.Equals("4"))
            {
                str = "tỷ";
            }
            if (so.Equals("5"))
            {
                str = "ngh\x00ecn tỷ";
            }
            if (so.Equals("6"))
            {
                str = "triệu tỷ";
            }
            if (so.Equals("7"))
            {
                str = "tỷ tỷ";
            }
            return str;
        }

        private static string Tach(string tach3)
        {
            string str = "";
            if (tach3.Equals("000"))
            {
                return "";
            }
            if (tach3.Length == 3)
            {
                string str2 = tach3.Trim().Substring(0, 1).ToString().Trim();
                string str3 = tach3.Trim().Substring(1, 1).ToString().Trim();
                string str4 = tach3.Trim().Substring(2, 1).ToString().Trim();
                if (str2.Equals("0") && str3.Equals("0"))
                {
                    str = " kh\x00f4ng trăm lẻ " + Chu(str4.ToString().Trim()) + " ";
                }
                if ((!str2.Equals("0") && str3.Equals("0")) && str4.Equals("0"))
                {
                    str = Chu(str2.ToString().Trim()).Trim() + " trăm ";
                }
                if (!((str2.Equals("0") || !str3.Equals("0")) || str4.Equals("0")))
                {
                    str = Chu(str2.ToString().Trim()).Trim() + " trăm lẻ " + Chu(str4.Trim()).Trim() + " ";
                }
                if (!(((!str2.Equals("0") || (Convert.ToInt32(str3) <= 1)) || (Convert.ToInt32(str4) <= 0)) || str4.Equals("5")))
                {
                    str = " kh\x00f4ng trăm " + Chu(str3.Trim()).Trim() + " mươi " + Chu(str4.Trim()).Trim() + " ";
                }
                if ((str2.Equals("0") && (Convert.ToInt32(str3) > 1)) && str4.Equals("0"))
                {
                    str = " kh\x00f4ng trăm " + Chu(str3.Trim()).Trim() + " mươi ";
                }
                if ((str2.Equals("0") && (Convert.ToInt32(str3) > 1)) && str4.Equals("5"))
                {
                    str = " kh\x00f4ng trăm " + Chu(str3.Trim()).Trim() + " mươi lăm ";
                }
                if (!(((!str2.Equals("0") || !str3.Equals("1")) || (Convert.ToInt32(str4) <= 0)) || str4.Equals("5")))
                {
                    str = " kh\x00f4ng trăm mười " + Chu(str4.Trim()).Trim() + " ";
                }
                if ((str2.Equals("0") && str3.Equals("1")) && str4.Equals("0"))
                {
                    str = " kh\x00f4ng trăm mười ";
                }
                if ((str2.Equals("0") && str3.Equals("1")) && str4.Equals("5"))
                {
                    str = " kh\x00f4ng trăm mười lăm ";
                }
                if (!((((Convert.ToInt32(str2) <= 0) || (Convert.ToInt32(str3) <= 1)) || (Convert.ToInt32(str4) <= 0)) || str4.Equals("5")))
                {
                    str = Chu(str2.Trim()).Trim() + " trăm " + Chu(str3.Trim()).Trim() + " mươi " + Chu(str4.Trim()).Trim() + " ";
                }
                if (((Convert.ToInt32(str2) > 0) && (Convert.ToInt32(str3) > 1)) && str4.Equals("0"))
                {
                    str = Chu(str2.Trim()).Trim() + " trăm " + Chu(str3.Trim()).Trim() + " mươi ";
                }
                if (((Convert.ToInt32(str2) > 0) && (Convert.ToInt32(str3) > 1)) && str4.Equals("5"))
                {
                    str = Chu(str2.Trim()).Trim() + " trăm " + Chu(str3.Trim()).Trim() + " mươi lăm ";
                }
                if (!((((Convert.ToInt32(str2) <= 0) || !str3.Equals("1")) || (Convert.ToInt32(str4) <= 0)) || str4.Equals("5")))
                {
                    str = Chu(str2.Trim()).Trim() + " trăm mười " + Chu(str4.Trim()).Trim() + " ";
                }
                if (((Convert.ToInt32(str2) > 0) && str3.Equals("1")) && str4.Equals("0"))
                {
                    str = Chu(str2.Trim()).Trim() + " trăm mười ";
                }
                if (((Convert.ToInt32(str2) > 0) && str3.Equals("1")) && str4.Equals("5"))
                {
                    str = Chu(str2.Trim()).Trim() + " trăm mười lăm ";
                }
            }
            return str;
        }


        public static List<string> getStringValuesArray(string LtsSourceValues)
        {
            List<string> LtsValues = new List<string>();
            if (!string.IsNullOrEmpty(LtsSourceValues))
            {
                if (LtsSourceValues.Contains(","))
                    LtsValues = LtsSourceValues.Split(',').Select(o => o).ToList();
                else
                    LtsValues.Add(LtsSourceValues);
            }
            return LtsValues;
        }

        public static List<int> getValuesArray(string LtsSourceValues)
        {
            List<int> LtsValues = new List<int>();
            if (!string.IsNullOrEmpty(LtsSourceValues) && LtsSourceValues != "null" && LtsSourceValues != "undefine")
            {
                if (LtsSourceValues.Contains(","))
                    LtsValues = LtsSourceValues.Split(',').Select(o => Convert.ToInt32(o)).ToList();
                else
                    try
                    {
                        LtsValues.Add(Convert.ToInt32(LtsSourceValues));
                    }
                    catch { }
            }
            return LtsValues;
        }

        public static string FormatBytes(int bytes)
        {
            const int scale = 1024;
            string[] orders = new string[] { "GB", "MB", "KB", "Bytes" };
            long max = (long)Math.Pow(scale, orders.Length - 1);

            foreach (string order in orders)
            {
                if (bytes > max)
                    return string.Format("{0:##.##} {1}", decimal.Divide(bytes, max), order);

                max /= scale;
            }
            return "0 Bytes";
        }

        public static List<string> getYoutubeArrID(string source)
        {
            List<string> ArrVideos = new List<string>();
            if (!string.IsNullOrEmpty(source))
            {
                if (source.Contains("\n"))
                {
                    foreach (var item in source.Split('\n'))
                    {
                        ArrVideos.Add(getYoutubeID(item));
                    }
                }
                else
                    ArrVideos.Add(getYoutubeID(source));
            }
            return ArrVideos;
        }
        public static string getYoutubeID(string source)
        {
            if (source.IndexOf("?v=") < 0)
                return string.Empty;
            else
            {
                int start = source.IndexOf("?v=") + 3;
                int end;
                if (source.IndexOf("&") < 0)
                    end = source.Length;
                else
                    end = source.IndexOf("&");

                return source.Substring(start, end - start);
            }
        }

        public static string getYoutubeLink(string source)
        {
            if (source.IndexOf("?v=") < 0)
                return string.Empty;
            else
            {
                string temp = "http://www.youtube.com/v/";
                return temp + getYoutubeID(source);
            }
        }


        public static string getDataVotePercent(int VoteCount, int TotalVote)
        {
            try
            {
                double value = ((double)VoteCount / TotalVote);
                return (value.ToString("0.0%") == "NaN") ? "0" : value.ToString("0.0%");
            }
            catch { return string.Empty; }
        }

        public static string getDataVoteWidthCss(int VoteCount, int TotalVote)
        {
            try
            {
                double value = ((double)VoteCount / TotalVote);
                return (value.ToString("0.0%") == "NaN") ? "0" : value.ToString("0.0%").Replace("%", "");
            }
            catch { return string.Empty; }
        }

        /// <summary>
        /// Lấy về danh sách ID
        /// </summary>
        /// <param name="arrID"></param>
        /// <returns></returns>
        public static List<int> GetDanhSachIDsQuaFormPost(string arrID)
        {
            List<int> dsID = new List<int>();
            if (!string.IsNullOrEmpty(arrID)) // Nếu không rỗng
            {
                if (arrID.IndexOf(',') > 0) //nếu nhiều hơn 1
                {
                    string[] tempIDs = arrID.Split(',');
                    foreach (string idConvert in tempIDs)
                    {
                        dsID.Add(Convert.ToInt32(idConvert));
                    }
                }
                else
                    dsID.Add(Convert.ToInt32(arrID));
            }
            return dsID;
        }

        public static byte[] ReadFile(string filePath)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;  // get file length
                buffer = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }

        /// <summary>
        /// Hàm chuyển một chuỗi tiếng việt có dấu thành tiếng việt không dấu
        /// </summary>
        /// <param name="Unicode">xâu tiếng việt có dấu</param>
        /// <Modified>        
        ///	Name		Date		    Comment 
        /// QuangND     10/11/2009      Tạo mới
        /// </Modified>
        public static string UnicodeToAscii(string Unicode)
        {
            Unicode = Regex.Replace(Unicode, "[á|à|ả|ã|ạ|â|ă|ấ|ầ|ẩ|ẫ|ậ|ắ|ằ|ẳ|ẵ|ặ]", "a", RegexOptions.IgnoreCase);
            Unicode = Regex.Replace(Unicode, "[é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ]", "e", RegexOptions.IgnoreCase);
            Unicode = Regex.Replace(Unicode, "[ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự]", "u", RegexOptions.IgnoreCase);
            Unicode = Regex.Replace(Unicode, "[í|ì|ỉ|ĩ|ị]", "i", RegexOptions.IgnoreCase);
            Unicode = Regex.Replace(Unicode, "[ó|ò|ỏ|õ|ọ|ô|ơ|ố|ồ|ổ|ỗ|ộ|ớ|ờ|ở|ỡ|ợ]", "o", RegexOptions.IgnoreCase);
            Unicode = Regex.Replace(Unicode, "[đ|Đ]", "d", RegexOptions.IgnoreCase);
            Unicode = Regex.Replace(Unicode, "[ý|ỳ|ỷ|ỹ|ỵ|Ý|Ỳ|Ỷ|Ỹ|Ỵ]", "y", RegexOptions.IgnoreCase);
            Unicode = Regex.Replace(Unicode, "[^A-Za-z0-9-]", "");

            return Unicode;
        }

        public static string getStock(List<string> source)
        {
            if (source.Count > 0)
            {
                var stk = source.FirstOrDefault();
                if (stk.Contains("||"))
                    stk = stk.Substring(stk.IndexOf("||") + 2);
                return stk;
            }
            return "N/A";
        }

        /// <summary>
        /// hàm Conver rewrite url
        /// </summary>
        /// <param name="Unicode"></param>
        /// <returns></returns>
        public static string ConverRewrite(string Unicode)
        {
            Unicode = Unicode.ToLower();
            Unicode = Regex.Replace(Unicode, "[\\s]", "-");
            Unicode = Unicode.Replace("----", "-");
            Unicode = Unicode.Replace("---", "-");
            Unicode = Unicode.Replace("--", "-");
            Unicode = UnicodeToAscii(Unicode);
            return Unicode;
        }

        /// <summary>
        /// Hàm tạo mã md5
        /// </summary>
        /// <param name="str">xâu cần mã hóa</param>
        /// <Modified>        
        ///	Name		Date		    Comment 
        /// QuangND     10/11/2009      Tạo mới
        /// </Modified>
        public static string GetMd5Sum(string str)
        {
            Encoder enc = System.Text.Encoding.Unicode.GetEncoder();
            byte[] unicodeText = new byte[str.Length * 2];
            enc.GetBytes(str.ToCharArray(), 0, str.Length, unicodeText, 0, true);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(unicodeText);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string MD5ByPHP(string password)
        {
            byte[] textBytes = System.Text.Encoding.Default.GetBytes(password);
            try
            {
                System.Security.Cryptography.MD5CryptoServiceProvider cryptHandler;
                cryptHandler = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] hash = cryptHandler.ComputeHash(textBytes);
                string ret = "";
                foreach (byte a in hash)
                {
                    if (a < 16)
                        ret += "0" + a.ToString("x");
                    else
                        ret += a.ToString("x");
                }
                return ret;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Xóa thẻ HTML
        /// </summary>
        /// <param name="source">xâu có chứa HTML</param>
        /// <Modified>        
        ///	Name		Date		    Comment 
        /// QuangND     10/11/2009      Tạo mới
        /// </Modified>
        public static string RemoveHTMLTag(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                return Regex.Replace(source, "<.*?>", string.Empty).Replace("\r\n", " ");
            }
            else
                return source;
        }

        public static string RemoveHTMLTagWithBr(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                string temp = Regex.Replace(source, "<.*?>", string.Empty);
                if (!string.IsNullOrEmpty(temp))
                    temp = temp.Replace("\r\n", "<br />\r\n");

                return temp;
            }
            else
                return source;
        }

        public static string GetFileExtend(string fileName)
        {
            return fileName.Substring(fileName.LastIndexOf("."));
        }

        public static string GetFileNameNoneExtent(string fileName)
        {
            return fileName.Substring(0, fileName.LastIndexOf("."));
        }
    }

    public class FileAttach
    {
        public string FileUrl { get; set; }
        public string FileName { get; set; }
        public string FileIcon { get; set; }
    }

}
