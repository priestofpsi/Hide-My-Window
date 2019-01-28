using System;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    internal static class Extensions
    {
        #region Public Extension Methods & Functions

        public static string GetMd5Hash(this string value)
        {
            using (MD5 hash = MD5.Create())
                return hash.ComputeHash(Encoding.UTF8.GetBytes(value)).GetHashString();
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

        /// <summary>
        /// Executes the specified delegate on the thread that owns the control's underlying window handle.
        /// </summary>
        /// <param name="control">The <see cref="Control"/> on which the <paramref name="action"/> should be invoked.</param>
        /// <param name="action">The <see cref="Action"/> to invoke on the <paramref name="control"/> instance.</param>
        /// <returns>The return value from the delegate being invoked, or null if the delegate has no return value</returns>
        /// <exception cref="ArgumentNullException">thrown if <paramref name="control"/> is <c>Null</c>.</exception>
        /// <exception cref="ObjectDisposedException">thrown if the <paramref name="control"/> is already disposed.</exception>
        public static object Invoke(this Control control, Action action)
        {
            if (control.FindForm().IsDisposed || control.FindForm().Disposing)
                return null;

            if (control == null)
                throw new ArgumentNullException(nameof(control));

            if (control.IsDisposed)
                throw new ObjectDisposedException(nameof(control));

            if (action == null)
                return null;

            if (control.InvokeRequired)
                return control.Invoke(action);
            
            action();
            return null;
        }
        #endregion

        #region Private Extension Methods & Functions
        private static string GetHashString(this byte[] value)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in value)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
        #endregion
    }
}