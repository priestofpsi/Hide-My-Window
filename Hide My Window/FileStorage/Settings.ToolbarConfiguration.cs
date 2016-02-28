using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace theDiary.Tools.HideMyWindow
{
    public class ToolbarConfiguration
    {
        [XmlAttribute]
        public bool Visible
        {
            get; set;
        }

        [XmlAttribute]
        public bool SmallIcons
        {
            get; set;
        }

        [XmlAttribute]
        public bool HideText
        {
            get; set;
        }
    }
}
