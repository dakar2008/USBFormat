using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USBFormat
{
    class Helper
    {
    }
    public static class StringExtensions
    {
        public static string ToUrlName(this string extension)
        {
            string NewUrlName = extension.ToLower().Trim();
            NewUrlName = NewUrlName.Replace(",-", "");
            NewUrlName = NewUrlName.Replace(" ", "-");
            NewUrlName = NewUrlName.Replace("c#", "c-sharp");
            NewUrlName = NewUrlName.Replace("æ", "ae");
            NewUrlName = NewUrlName.Replace("ø", "oe");
            NewUrlName = NewUrlName.Replace("å", "aa");
            NewUrlName = NewUrlName.Replace("'", "");
            NewUrlName = NewUrlName.Replace("/", "");
            NewUrlName = NewUrlName.Replace("&", "");
            NewUrlName = NewUrlName.Replace(";", "");
            NewUrlName = NewUrlName.Replace(":", "");
            NewUrlName = NewUrlName.Replace(",", "");
            NewUrlName = NewUrlName.Replace(".", "");
            NewUrlName = NewUrlName.Replace("+", "");
            NewUrlName = NewUrlName.Replace("=", "");
            NewUrlName = NewUrlName.Replace("(", "");
            NewUrlName = NewUrlName.Replace(")", "");
            NewUrlName = NewUrlName.Replace("%", "");
            NewUrlName = NewUrlName.Replace("#", "");
            NewUrlName = NewUrlName.Replace("!", "");
            NewUrlName = NewUrlName.Replace("---", "-");
            NewUrlName = NewUrlName.Replace("--", "-");
            return NewUrlName;

        }
        public static string Truncate(this string source, int length)
        {
            if (source.Length > length)
            {
                source = source.Substring(0, length);
                source = source + "...";
            }
            return source;
        }
        public static string CapFirst(this string text)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(text[0]) + text.Substring(1);
        }

        public static decimal ToTax(this decimal price, int addFee = 0)
        {
            decimal NewPrice = price + addFee;
            return Math.Round(Decimal.Multiply(new decimal(0.2), NewPrice), 2);
        }

        public static string ToPrice(this object price)
        {
            return string.Format("{0}", price.ToString().Replace(",00", ""));
        }

        public static int ToInt(this object data)
        {
            int value = 0;
            int.TryParse(data.ToString(), out value);

            return value;
        }
        public static string ToCountdown(this DateTime extension)
        {
            TimeSpan date = extension - DateTime.Now.AddMinutes(-15);

            string days = date.Days.ToString();
            string second = date.Seconds.ToString();
            string minut = date.Minutes.ToString();
            string hour = date.Hours.ToString();

            string returnString = "";

            if (date.Seconds < 10)
            {
                second = "0" + date.Seconds.ToString();
            }
            if (date.Minutes < 10)
            {
                minut = "0" + date.Minutes.ToString();
            }
            if (date.Hours < 10)
            {
                hour = "0" + date.Hours.ToString();
            }
            if (date >= new TimeSpan(0, 0, 0))
            {
                returnString = string.Format("tid tilbage:<span>{0} dage</span><span>{1}:{2}:{3}</span>", days, hour, minut, second);
            }

            return returnString;
        }

        public static string ToFileSize(this object source)
        {
            const int byteConversion = 1024;
            double bytes = Convert.ToDouble(source);
            if (bytes >= Math.Pow(byteConversion, 3)) //GB Range    
            {
                return string.Concat(Math.Round(bytes / Math.Pow(byteConversion, 3), 2), " GB");
            }
            else if (bytes >= Math.Pow(byteConversion, 2)) //MB Range    
            {
                return string.Concat(Math.Round(bytes / Math.Pow(byteConversion, 2), 2), " MB");
            }
            else if (bytes >= byteConversion) //KB Range    
            {
                return string.Concat(Math.Round(bytes / byteConversion, 2), " KB");
            }
            else //Bytes    
            {
                return string.Concat(bytes, " Bytes");
            }
        }
        public static string ToFormatedDate(this DateTime date)
        {
            return date.ToLongDateString() + " - (" + date.ToShortTimeString() + ")";
        }
        public static string ToFormatedDate(this object date)
        {
            DateTime GetDate = Convert.ToDateTime(date);
            return GetDate.ToLongDateString() + " - (" + GetDate.ToShortTimeString() + ")";
        }
    }
}
