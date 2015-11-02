using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    public class GlowPanel
        : Panel
    {
        #region Constructors

        public GlowPanel()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        #endregion

        #region Declarations

        private Color effectColor = SystemColors.ButtonShadow;
        private bool effectEnabled;
        private int featherEffect = 50;
        private int glowSize = 7;
        private EffectTarget target = EffectTarget.Controls;
        private EffectTrigger trigger = EffectTrigger.Hover | EffectTrigger.Focus;

        #endregion

        #region Properties

        [Category("Appearance")]
        [Description("Get or Set the thickness of the Glow")]
        [DefaultValue(typeof (int), "7")]
        public int GlowThickness
        {
            get { return (this.glowSize - 1)/2; }
            set
            {
                if (this.glowSize == (value*2) + 1)
                    return;

                this.glowSize = (value*2) + 1;
                this.Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("Get or Set the intesity of the feathering on the effect.")]
        [DefaultValue(50)]
        public int FeatherEffect
        {
            get { return this.featherEffect; }
            set
            {
                if (value < 0 || value > 100 || this.featherEffect == value)
                    return;

                this.featherEffect = value;
                this.Invalidate();
            }
        }

        [Category("Behavior")]
        [Description("Get or Set the Trigger used to enable the effect.")]
        [DefaultValue(typeof (int), "6")]
        public EffectTrigger Trigger
        {
            get { return this.trigger; }
            set
            {
                if (this.trigger == value)
                    return;

                this.trigger = value;
                this.Invalidate();
            }
        }

        [Category("Behavior")]
        [Description("Get or Set the which Control should the effect be enabled on.")]
        [DefaultValue(typeof (int), "1")]
        public EffectTarget Target
        {
            get { return this.target; }
            set
            {
                if (this.target == value)
                    return;

                this.target = value;
                this.Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("Get or Set the color of the Glow")]
        [DefaultValue(typeof (Color), "Maroon")]
        public Color EffectColor
        {
            get { return this.effectColor; }
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
        public bool EffectEnabled
        {
            get { return this.effectEnabled; }
            set
            {
                if (this.effectEnabled == value)
                    return;
                this.effectEnabled = value;
                this.Invalidate();
            }
        }

        #endregion

        #region Methods & Functions

        private void DrawGlow(Control control, PaintEventArgs e)
        {
            if (!this.Enabled || (control != null && !control.Enabled))
                return;

            using (GraphicsPath gp = new GraphicsPath())
            {
                //'Change these to Properties if you want Design Control of the Values
                int glowSteps = this.GlowThickness;
                int glowFeather = this.FeatherEffect;
                //'Get a Rectangle a little smaller than the control's 'and make a GraphicsPath with it
                Rectangle rect = (control ?? this).Bounds;
                rect.Inflate(2, 2);
                gp.AddRectangle(rect);
                e.Graphics.SetClip(new Rectangle(new Point(-this.GlowThickness, -this.GlowThickness),
                    new Size(this.Width + this.GlowThickness, this.Height + this.GlowThickness)));

                //Draw multiple rectangles with increasing thickness and transparency
                for (int i = 1; i < glowSteps; i = i + 2)
                {
                    int aGlow = (glowFeather - ((glowFeather/glowSteps)*i));
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

            if (this.DesignMode && this.Controls.Count == 0)
            {
                TextRenderer.DrawText(e.Graphics, this.Name, this.Font, new Point(20, 20), this.ForeColor);
                //TextRenderer.DrawText(e.Graphics, "SSDiver2112", new Font("Arial", 7, FontStyle.Bold), new Point(this.Width - 75, this.Height - 17), Color.LightGray);
            }
            else if (this.Target == EffectTarget.Self)
            {
                this.DrawGlow(null, e);
            }
            else if (this.Target == EffectTarget.Controls)
            {
                foreach (Control control in this.Controls)
                {
                    if ((this.Trigger & EffectTrigger.AlwaysOn) != 0)
                    {
                        this.DrawGlow(control, e);
                    }
                    else if ((this.Trigger & EffectTrigger.Hover) != 0)
                    {
                        control.Paint += this.Control_PaintOnHover;
                        control.MouseEnter += this.Control_Redraw;
                        control.LocationChanged += this.Control_Redraw;
                    }
                    else if ((this.Trigger & EffectTrigger.Focus) != 0)
                    {
                        control.Paint += this.Control_PaintOnFocus;
                        control.MouseEnter += this.Control_Redraw;
                        control.LocationChanged += this.Control_Redraw;
                    }

                    if ((this.Trigger & EffectTrigger.Focus) == 0)
                        control.Paint -= this.Control_PaintOnFocus;
                    if ((this.Trigger & EffectTrigger.Hover) == 0)
                        control.Paint -= this.Control_PaintOnHover;
                    if ((this.Trigger & EffectTrigger.Focus) == 0 && (this.Trigger & EffectTrigger.Hover) == 0)
                        control.MouseEnter -= this.Control_Redraw;
                }
            }
        }

        private void Control_Redraw(object sender, EventArgs e)
        {
            ((Control) sender).Invalidate();
            this.Invalidate();
        }

        private void Control_PaintOnFocus(object sender, PaintEventArgs e)
        {
            if (this.EffectEnabled)
            {
                if (this.Target == EffectTarget.Controls && (this.Trigger & EffectTrigger.Hover) != 0)
                    if (
                        ((Control) sender).ClientRectangle.Contains(
                            ((Control) sender).PointToClient(Control.MousePosition)))
                        this.DrawGlow((Control) sender, e);
            }
        }

        private void Control_PaintOnHover(object sender, PaintEventArgs e)
        {
            if (this.EffectEnabled)
            {
                if (this.Target == EffectTarget.Controls && (this.Trigger & EffectTrigger.Hover) != 0)
                    if (
                        ((Control) sender).ClientRectangle.Contains(
                            ((Control) sender).PointToClient(Control.MousePosition)))
                        this.DrawGlow((Control) sender, e);
            }
        }

        private void Control_MouseEnter(object sender, EventArgs e)
        {
        }

        #endregion
    }

    public enum EffectTarget
    {
        Self = 0,

        Controls = 1
    }

    [Flags]
    public enum EffectTrigger
    {
        AlwaysOn = 1,

        Hover = 2,

        Focus = 4
    }
}