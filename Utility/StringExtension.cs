using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Utility
{
    public static class StringExtension
    {
        public static bool IsNumeric(this string theValue)
        {
            long retNum;
            return long.TryParse(theValue, System.Globalization.NumberStyles.Integer, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
        }
        public static bool IsValidEmailAddress(this string s)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(s);
        }
        /// <summary>
        /// Truncates the string to a specified length and replace the truncated to a ...
        /// </summary>
        /// <param name="text">string that will be truncated</param>
        /// <param name="maxLength">total length of characters to maintain before the truncate happens</param>
        /// <returns>truncated string</returns>
        public static string Truncate(this string text, int maxLength)
        {
            // replaces the truncated string to a ...
            const string suffix = "...";
            string truncatedString = text;

            if (maxLength <= 0) return truncatedString;
            int strLength = maxLength - suffix.Length;

            if (strLength <= 0) return truncatedString;

            if (text == null || text.Length <= maxLength) return truncatedString;

            truncatedString = text.Substring(0, strLength);
            truncatedString = truncatedString.TrimEnd();
            truncatedString += suffix;
            return truncatedString;
        }
        public static bool IsValidUrl(this string text)
        {
            Regex rx = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            return rx.IsMatch(text);
        }
        public static bool IsValidIPAddress(this string s)
        {
            return Regex.IsMatch(s,
                    @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b");
        }
        public static bool IsUnicode(this string value)
        {
            int asciiBytesCount = System.Text.Encoding.ASCII.GetByteCount(value);
            int unicodBytesCount = System.Text.Encoding.UTF8.GetByteCount(value);

            if (asciiBytesCount != unicodBytesCount)
            {
                return true;
            }
            return false;
        }
        public static bool ContainsNoSpaces(this string s)
        {
            var regex = new Regex(@"^[a-zA-Z0-9]+$");
            return regex.IsMatch(s);
        }
        public static string ExceptBlanks(this string str)
        {
            StringBuilder sb = new StringBuilder(str.Length);
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (!char.IsWhiteSpace(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }


        public static bool IsAlphaNum(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            return (str.ToCharArray().All(c => Char.IsLetter(c) || Char.IsNumber(c)));
        }


        public static string GetNetworkAddress(this string ipAddres)
        {
            string retVal = "";

            int l = ipAddres.LastIndexOf('.');

            if (l > 0)
                retVal = ipAddres.Substring(0, l);

            return retVal;

        }
        public static bool isCheckNameOk(this string sName)
        {
            bool bRetVal = false;
            char ch;
            int vFound = 0;
            int CharPosition = 0;
            int CharChk = 4;


            if (!string.IsNullOrEmpty(sName))
            {
                char[] charArr1 = sName.ToLower().ToCharArray();
                char[] vo = { 'a', 'e', 'i', 'o', 'u' };
                for (int i = 0; i < charArr1.Length; i++)
                {
                    ch = charArr1[i];

                    //if (!char.IsWhiteSpace(ch))
                    if (char.IsLetter(ch))
                    {
                        CharPosition++;
                        for (int j = 0; j < vo.Length; j++)
                        {
                            if (ch.CompareTo(vo[j]) == 0)
                            {
                                CharPosition = 0;
                                vFound++;
                                bRetVal = true;
                            }
                        }
                    }
                    else
                    {
                        //CharPosition--;
                        CharPosition = 0;
                    }

                    if (CharPosition == (CharChk))
                    {
                        bRetVal = false;
                        CharPosition = 0;
                        break;
                    }
                }
            }

            return bRetVal;
        }
    }
}
