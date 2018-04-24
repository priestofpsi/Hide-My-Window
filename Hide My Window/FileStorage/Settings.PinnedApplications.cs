using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace theDiary.Tools.HideMyWindow
{
    public class PinnedApplications
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PinnedApplications"/> class.
        /// </summary>
        public PinnedApplications()
        {
            this.HideOnMinimize = true;
        }

        #endregion

        #region Properties

        [XmlAttribute]
        public bool HideOnMinimize
        {
            get; set;
        }

        [XmlAttribute]
        public bool AddIconOverlay
        {
            get; set;
        }

        [XmlAttribute]
        public bool ModifyWindowTitle
        {
            get; set;
        }

        [XmlAttribute]
        public string PrefixWindowText
        {
            get; set;
        }

        [XmlAttribute]
        public string SuffixWindowText
        {
            get; set;
        }

        #endregion
    }
}
