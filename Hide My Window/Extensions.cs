using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace theDiary.Tools.HideMyWindow
{
    internal static class Extensions
    {
        #region Methods & Functions
        public static string GetMD5Hash(this string value)
        {
            using (MD5 hash = MD5.Create())
                return hash.ComputeHash(Encoding.UTF8.GetBytes(value)).GetHashString();
        }

        private static string GetHashString(this byte[] value)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in value)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static bool TryGetCustomAttribute<TAttribute>(this ICustomAttributeProvider provider, bool inherited,
                                                             out TAttribute attribute) where TAttribute : Attribute
        {
            attribute = provider.GetCustomAttribute<TAttribute>(inherited);
            return attribute != null;
        }

        public static bool TryGetCustomAttribute<TAttribute>(this ICustomAttributeProvider provider,
                                                             out TAttribute attribute) where TAttribute : Attribute
        {
            return provider.TryGetCustomAttribute(true, out attribute);
        }

        public static TAttribute GetCustomAttribute<TAttribute>(this ICustomAttributeProvider provder,
                                                                bool inherited = true) where TAttribute : Attribute
        {
            return provder.GetCustomAttributes(typeof (TAttribute), inherited).Cast<TAttribute>().FirstOrDefault();
        }

        public static TAttribute GetCustomAttribute<TAttribute>(this object instance, bool inherited = true)
            where TAttribute : Attribute
        {
            return instance.GetType().GetCustomAttribute<TAttribute>(inherited);
        }
        #endregion
    }
}