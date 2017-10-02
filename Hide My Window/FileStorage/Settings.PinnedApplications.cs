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
            public string SufixWindowText
            {
                get; set;
            }

            #endregion
    }
}
