﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace theDiary.Tools.HideMyWindow
{
    public class FormState
    {
        #region Constructors

        public FormState()
        {
        }

        public FormState(Form form)
        {
            this.WindowState = form.WindowState;
            this.Size = form.Size;
            this.Location = form.Location;
        }

        #endregion

        #region Declarations

        private Size? size;

        #endregion

        #region Properties

        [XmlAttribute]
        public FormWindowState WindowState { get; set; }

        [XmlElement]
        public Point Location { get; set; }

        [XmlElement]
        public Size Size
        {
            get { return this.size.GetValueOrDefault(); }
            set { this.size = value; }
        }

        [XmlIgnore]
        public bool IsEmpty
        {
            get { return !this.size.HasValue; }
        }

        #endregion

        #region Methods & Functions

        public void SetFormState(Form form)
        {
            if (this.IsEmpty)
                return;
            form.WindowState = this.WindowState;
            form.Location = this.Location;
            form.Size = this.Size;
        }

        #endregion
    }
}