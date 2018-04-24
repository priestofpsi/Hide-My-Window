using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace theDiary.Tools.HideMyWindow
{

    public class FormState
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FormState"/> class.
        /// </summary>
        public FormState()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormState"/> class.
        /// </summary>
        /// <param name="form">An instance of the <see cref="Form"/></param>
        public FormState(Form form)
            : this()
        {
            this.WindowState = form.WindowState;
            this.Size = form.Size;
            this.Location = form.Location;
        }

        #endregion

        #region Private Declarations

        private Size? size;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="FormWindowState"/> of a <see cref="Form"/>.
        /// </summary>
        [XmlAttribute]
        public FormWindowState WindowState { get; set; }

        /// <summary>
        /// Gets or sets the location of a <see cref="Form"/>.
        /// </summary>
        [XmlElement]
        public Point Location { get; set; }

        /// <summary>
        /// Gets or sets the size of a <see cref="Form"/>.
        /// </summary>
        [XmlElement]
        public Size Size
        {
            get { return this.size.GetValueOrDefault(); }
            set { this.size = value; }
        }

        /// <summary>
        /// Gets a value indicating if the <see cref="FormState"/> is <c>Empty</c>.
        /// </summary>
        [XmlIgnore]
        public bool IsEmpty
        {
            get { return (!this.size.HasValue || this.size.Value.IsEmpty) && this.Location.IsEmpty; }
        }

        #endregion

        #region Public Methods & Functions
        /// <summary>
        /// Sets the values contained in the <see cref="FormState"/> instance to the <paramref name="form"/>.
        /// </summary>
        /// <param name="form">The instance of <see cref="Form"/> to set.</param>
        /// <exception cref="ArgumentNullException">thrown if the <paramref name="form"/> is <c>Null</c>.</exception>
        public void SetFormState(Form form)
        {
            if (form == null)
                throw new ArgumentNullException();

            if (this.IsEmpty)
                return;

            form.WindowState = this.WindowState;
            form.Location = this.Location;
            form.Size = this.Size;
        }
        #endregion
    }
}