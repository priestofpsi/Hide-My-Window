using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace theDiary.Tools.HideMyWindow
{
    internal static class Extensions
    {
        

        public static string GetMD5Hash(this string value)
        {
            using (var hash = MD5.Create())
            {
                return hash.ComputeHash(Encoding.UTF8.GetBytes(value)).GetHashString();
            }
        }

        private static string GetHashString(this byte[] value)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in value)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        


    }
}
