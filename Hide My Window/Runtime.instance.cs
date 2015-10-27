using System;
using System.Collections.Generic;
using System.Linq;

namespace theDiary.Tools.HideMyWindow
{
    public partial class Runtime
    {
        #region Constructors

        public Runtime()
        {
            this.WindowHidden += (s, e) => { Runtime.Instance.Store.Add(e.Window.Handle); };

            this.WindowShown += (s, e) =>
            {
                if (!e.Window.IsPinned)
                    Runtime.instance.Store.Remove(e.Window.Handle);
            };
        }

        #endregion

        #region Declarations

        private readonly Dictionary<IntPtr, WindowInfo> hiddenWindows = new Dictionary<IntPtr, WindowInfo>();

        private Settings settings;
        private HiddenWindowStore store;

        #endregion

        #region Properties

        public Settings Settings
        {
            get
            {
                if (this.settings == null)
                    this.settings = Settings.Load();

                return this.settings;
            }
            internal set { this.settings = value; }
        }

        public HiddenWindowStore Store
        {
            get
            {
                if (this.store == null)
                    this.store = HiddenWindowStore.Load();

                return this.store;
            }
        }

        public int Count
        {
            get { return this.hiddenWindows.Count; }
        }

        #endregion

        #region Methods & Functions

        public event EventHandler<WindowEventArgs> WindowHidden;

        public event EventHandler<WindowEventArgs> WindowShown;

        public WindowInfo LastWindow()
        {
            if (this.hiddenWindows.Count == 0)
                return null;

            return this.hiddenWindows.Last().Value;
        }

        internal void AddHiddenWindow(WindowInfo window)
        {
            if (window.Equals(Program.MainForm.Handle)
                || !window.Hide())
                return;
            if (!this.hiddenWindows.ContainsKey(window.Handle))
                this.hiddenWindows.Add(window.Handle, window);

            if (this.WindowHidden != null)
                this.WindowHidden(this, new WindowEventArgs(window));
        }

        internal void ToggleHiddenWindow(WindowInfo window)
        {
            if (window.CanShow)
            {
                this.RemoveHiddenWindow(window);
            }
            else if (window.CanHide)
            {
                this.AddHiddenWindow(window);
            }
        }

        internal void RemoveHiddenWindow(WindowInfo window)
        {
            if (!this.hiddenWindows.ContainsKey(window.Handle))
                this.hiddenWindows.Add(window.Handle, window);
            window.Show();

            if (this.WindowShown != null)
                this.WindowShown(this, new WindowEventArgs(window));
        }

        internal WindowInfo FindWindow(IntPtr handle)
        {
            if (this.hiddenWindows.ContainsKey(handle))
                return this.hiddenWindows[handle];

            return null;
        }

        #endregion
    }
}