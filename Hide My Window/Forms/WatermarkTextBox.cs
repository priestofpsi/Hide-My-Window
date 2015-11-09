using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    public class WatermarkTextBox : TextBox
    {
        #region Public Constructors
        public WatermarkTextBox()
        {
            this.Initialize();
        }
        #endregion

        #region Declarations
        protected Color _waterMarkActiveColor; //Color of the watermark when the control has focus
        protected Color _waterMarkColor; //Color of the watermark when the control does not have focus

        protected string _waterMarkText = "Default Watermark..."; //The watermark text
        private SolidBrush waterMarkBrush; //Brush for the watermark

        private Panel waterMarkContainer; //Container to hold the watermark
        private Font waterMarkFont; //Font of the watermark
        #endregion

        #region Properties
        [Category("Watermark attribtues")]
        [Description("Sets the text of the watermark")]
        public string WaterMark
        {
            get
            {
                return this._waterMarkText;
            }
            set
            {
                this._waterMarkText = value;
                this.Invalidate();
            }
        }

        [Category("Watermark attribtues")]
        [Description("When the control gaines focus, this color will be used as the watermark's forecolor")]
        public Color WaterMarkActiveForeColor
        {
            get
            {
                return this._waterMarkActiveColor;
            }

            set
            {
                this._waterMarkActiveColor = value;
                this.Invalidate();
            }
        }

        [Category("Watermark attribtues")]
        [Description("When the control looses focus, this color will be used as the watermark's forecolor")]
        public Color WaterMarkForeColor
        {
            get
            {
                return this._waterMarkColor;
            }

            set
            {
                this._waterMarkColor = value;
                this.Invalidate();
            }
        }

        [Category("Watermark attribtues")]
        [Description("The font used on the watermark. Default is the same as the control")]
        public Font WaterMarkFont
        {
            get
            {
                return this.waterMarkFont;
            }

            set
            {
                this.waterMarkFont = value;
                this.Invalidate();
            }
        }
        #endregion

        #region Methods & Functions
        /// <summary>
        ///     Initializes watermark properties and adds CtextBox events
        /// </summary>
        private void Initialize()
        {
            //Sets some default values to the watermark properties
            this._waterMarkColor = Color.LightGray;
            this._waterMarkActiveColor = Color.Gray;
            this.waterMarkFont = this.Font;
            this.waterMarkBrush = new SolidBrush(this._waterMarkActiveColor);
            this.waterMarkContainer = null;

            //Draw the watermark, so we can see it in design time
            this.DrawWaterMark();

            //Eventhandlers which contains function calls.
            //Either to draw or to remove the watermark
            this.Enter += this.ThisHasFocus;
            this.Leave += this.ThisWasLeaved;
            this.TextChanged += this.ThisTextChanged;
        }

        /// <summary>
        ///     Removes the watermark if it should
        /// </summary>
        private void RemoveWaterMark()
        {
            if (this.waterMarkContainer != null)
            {
                this.Controls.Remove(this.waterMarkContainer);
                this.waterMarkContainer = null;
            }
        }

        /// <summary>
        ///     Draws the watermark if the text length is 0
        /// </summary>
        private void DrawWaterMark()
        {
            if (this.waterMarkContainer == null
                && this.TextLength <= 0)
            {
                this.waterMarkContainer = new Panel(); // Creates the new panel instance
                this.waterMarkContainer.Paint += this.waterMarkContainer_Paint;
                this.waterMarkContainer.Invalidate();
                this.waterMarkContainer.Click += this.waterMarkContainer_Click;
                this.Controls.Add(this.waterMarkContainer); // adds the control
            }
        }

        private void waterMarkContainer_Click(object sender, EventArgs e)
        {
            this.Focus(); //Makes sure you can click wherever you want on the control to gain focus
        }

        private void waterMarkContainer_Paint(object sender, PaintEventArgs e)
        {
            //Setting the watermark container up
            this.waterMarkContainer.Location = new Point(2, 0); // sets the location
            this.waterMarkContainer.Height = this.Height; // Height should be the same as its parent
            this.waterMarkContainer.Width = this.Width; // same goes for width and the parent
            this.waterMarkContainer.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            // makes sure that it resizes with the parent control

            if (this.ContainsFocus)
            {
                //if focused use normal color
                this.waterMarkBrush = new SolidBrush(this._waterMarkActiveColor);
            }
            else
            {
                //if not focused use not active color
                this.waterMarkBrush = new SolidBrush(this._waterMarkColor);
            }

            //Drawing the string into the panel
            Graphics g = e.Graphics;
            g.DrawString(this._waterMarkText, this.waterMarkFont, this.waterMarkBrush, new PointF(-2f, 1f));

            //Take a look at that point
            //The reason I'm using the panel at all, is because of this feature, that it has no limits
            //I started out with a label but that looked very very bad because of its paddings
        }

        private void ThisHasFocus(object sender, EventArgs e)
        {
            //if focused use focus color
            this.waterMarkBrush = new SolidBrush(this._waterMarkActiveColor);

            //The watermark should not be drawn if the user has already written some text
            if (this.TextLength <= 0)
            {
                this.RemoveWaterMark();
                this.DrawWaterMark();
            }
        }

        private void ThisWasLeaved(object sender, EventArgs e)
        {
            //if the user has written something and left the control
            if (this.TextLength > 0)
            {
                //Remove the watermark
                this.RemoveWaterMark();
            }
            else
            {
                //But if the user didn't write anything, Then redraw the control.
                this.Invalidate();
            }
        }

        private void ThisTextChanged(object sender, EventArgs e)
        {
            //If the text of the textbox is not empty
            if (this.TextLength > 0)
            {
                //Remove the watermark
                this.RemoveWaterMark();
            }
            else
            {
                //But if the text is empty, draw the watermark again.
                this.DrawWaterMark();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //Draw the watermark even in design time
            this.DrawWaterMark();
        }

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            base.OnInvalidated(e);

            //Check if there is a watermark
            if (this.waterMarkContainer != null)

                //if there is a watermark it should also be invalidated();
                this.waterMarkContainer.Invalidate();
        }
        #endregion
    }
}