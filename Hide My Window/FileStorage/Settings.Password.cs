using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace theDiary.Tools.HideMyWindow
{
    public class PasswordSettings
        : IXmlSerializable
    {
        #region Declarations

        private string password;

        #endregion

        #region Properties

        public bool RequirePasswordOnShow { get; set; }

        public bool HasPassword
        {
            get { return !string.IsNullOrEmpty(this.password); }
        }

        public string Password
        {
            get { return this.password; }
            set { this.SetPassword(value); }
        }

        #endregion

        #region Methods & Functions

        public bool CheckPassword(string password)
        {
            if (!this.HasPassword && string.IsNullOrEmpty(password))
                return true;

            using (MD5 hash = MD5.Create())
            {
                return
                    this.Password.Equals(
                        PasswordSettings.GetHashString(hash.ComputeHash(Encoding.UTF8.GetBytes(password))));
            }
        }

        private void SetPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                this.password = password;
            }
            else
            {
                using (MD5 hash = MD5.Create())
                {
                    this.password = PasswordSettings.GetHashString(hash.ComputeHash(Encoding.UTF8.GetBytes(password)));
                }
            }
        }

        private static string GetHashString(byte[] value)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in value)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        #endregion

        #region Interface Implementations

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            bool requirePasswordOnShow = false;
            if (!bool.TryParse(reader.GetAttribute("RequirePasswordOnShow"), out requirePasswordOnShow))
                requirePasswordOnShow = false;
            try
            {
                this.password = reader.ReadElementString("Password");
            }
            catch
            {
                this.password = string.Empty;
            }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("RequirePasswordOnShow", this.RequirePasswordOnShow.ToString());
            writer.WriteElementString("Password", this.password);
        }

        #endregion
    }
}