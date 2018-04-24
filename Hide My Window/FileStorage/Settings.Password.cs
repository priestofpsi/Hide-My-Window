using System;
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
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordSettings"/> class.
        /// </summary>
        public PasswordSettings()
            : base()
        {

        }
        #endregion

        #region Private Declarations

        private string password;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating if a password is required for windows when they are been showen.
        /// </summary>
        public bool RequirePasswordOnShow { get; set; }

        /// <summary>
        /// Gets a value indicating if a password has been set.
        /// </summary>
        public bool HasPassword
        {
            get
            {
                return !string.IsNullOrEmpty(this.password);
            }
        }

        /// <summary>
        /// Gets or sets the password used when toggling a window.
        /// </summary>
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

            return this.Password.Equals(PasswordSettings.GetHashedPassword(password));
        }

        #endregion

        #region Private Methods & Functions
        private void SetPassword(string password)
        {
            this.password = string.IsNullOrEmpty(password) ? string.Empty : PasswordSettings.GetHashedPassword(password);
        }
        #endregion

        #region Private Static Methods & Functions
        private static string GetHashedPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            return password.GetMd5Hash();
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