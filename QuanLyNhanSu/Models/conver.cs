using System;
using System.Globalization;

namespace QuanLyNhanSu.Models
{
    public class conver
    {
        public static string ConvertToThousand64(object number, string code = "vi-VN")
        {
            string str;
            if (number != null)
                str = string.Format(CultureInfo.GetCultureInfo(code), "{0:N0}", Convert.ToInt64(number));
            else
                str = "0";
            return str;
        }
        public static string ConvertToThousand64_From_Float(object number, string code = "vi-VN")
        {
            var numStr = number.ToString();
            if (number != null && numStr.Contains(","))
            {
                var decNum = numStr.Split(',')[1];
                var str = ConvertToThousand64(numStr.Split(',')[0], code);
                return str + "," + decNum;
            }
            return ConvertToThousand64(number, code);
        }
    }
}