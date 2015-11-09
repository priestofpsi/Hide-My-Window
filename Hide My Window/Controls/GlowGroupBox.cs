using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof (GlowGroupBox), "theDiary.Tools.HideMyWindow")]
    public class GlowGroupBox : Panel
    {
        #region Public Constructors
        public GlowGroupBox()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }
        #endregion

        #region Declarations
        private Color effectColor = SystemColors.ButtonShadow;

        private EffectType effectType = EffectType.Glow;
        private bool glowOn;
        #endregion

        #region Properties
        [Category("Appearance")]
        [Description("Get or Set the color of the Glow")]
        [DefaultValue(typeof (Color), "Maroon")]
        public Color EffectColor
        {
            get
            {
                return this.effectColor;
            }
            set
            {
                if (this.effectColor == value)
                    return;
                this.effectColor = value;
                this.Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("Turn the Glow effect on or off")]
        [DefaultValue(false)]
        public bool GlowOn
        {
            get
            {
                return this.glowOn;
            }
            set
            {
                if (this.glowOn == value)
                    return;
                this.glowOn = value;
                this.Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("Choose Glow or Shadow")]
        [DefaultValue("Glow")]
        public EffectType EffectType
        {
            get
            {
                return this.effectType;
            }
            set
            {
                if (this.effectType == value)
                    return;

                this.effectType = value;
                this.Invalidate();
            }
        }
        #endregion

        #region Methods & Functions
        private void DrawShadow(Control control, PaintEventArgs e)
        {
            using (GraphicsPath shadowpath = new GraphicsPath())
            {
                Point shadowOffset = new Point(3, 3);
                Color shadowColor = this.effectColor;
                int shadowBlur = 2;
                int shadowFeather = 100;

                Rectangle rect = new Rectangle(control.Bounds.X + 4 + shadowOffset.X,
                    control.Bounds.Y + 4 + shadowOffset.Y, control.Bounds.Width - 8, control.Bounds.Height - 8);
                shadowpath.AddRectangle(rect);

                int x = 6;
                for (int i = 1; i < x; i++)
                {
                    using (
                        Pen pen = new Pen(Color.FromArgb((shadowFeather - ((shadowFeather / x) * i)), shadowColor),
                            ((float) i * (shadowBlur))))
                    {
                        pen.LineJoin = LineJoin.Round;
                        e.Graphics.DrawPath(pen, shadowpath);
                    }
                }

                e.Graphics.FillPath(new SolidBrush(shadowColor), shadowpath);
            }
        }

        private void DrawGlow(Control control, PaintEventArgs e)
        {
            using (GraphicsPath gp = new GraphicsPath())
            {
                //'Change these to Properties if you want Design Control of the Values
                int glowSteps = 15;
                int glowFeather = 50;

                //'Get a Rectangle a little smaller than the control's 'and make a GraphicsPath with it
                Rectangle rect = new Rectangle(control.Bounds.X, control.Bounds.Y, control.Bounds.Width - 1,
                    control.Bounds.Height - 1);
                rect.Inflate(-1, -1);
                gp.AddRectangle(rect);

                //Draw multiple rectangles with increasing thickness and transparency
                for (int i = 1; i < glowSteps; i = i + 2)
                {
                    int aGlow = (glowFeather - ((glowFeather / glowSteps) * i));
                    using (Pen pen = new Pen(Color.FromArgb(aGlow, this.effectColor), i))
                    {
                        pen.LineJoin = LineJoin.Round;
                        e.Graphics.DrawPath(pen, gp);
                    }
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            if (this.DesignMode
                && this.Controls.Count == 0)
            {
                TextRenderer.DrawText(e.Graphics, "Drop controls\n\ton the GlowGroupBox",
                    new Font("Arial", 8, FontStyle.Bold), new Point(20, 20), Color.DarkBlue);
                TextRenderer.DrawText(e.Graphics, "SSDiver2112", new Font("Arial", 7, FontStyle.Bold),
                    new Point(this.Width - 75, this.Height - 17), Color.LightGray);
            }
            else if (this.GlowOn)
            {
                foreach (Control control in this.Controls)
                {
                    if (!control.Focused)
                        continue;

                    bool doGlow = ((control.GetType().GetProperty("ReadOnly") == null)
                                   || !(bool) control.GetType().GetProperty("ReadOnly").GetValue(control));

                    if (!doGlow)
                        continue;

                    if (this.EffectType == EffectType.Glow)
                        this.DrawGlow(control, e);
                    else
                        this.DrawShadow(control, e);
                }
            }
        }
        #endregion
    }

    public enum EffectType
    {
        Glow,
        Shadow
    }
}