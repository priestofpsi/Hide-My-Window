using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace theDiary.Tools.HideMyWindow
{
    public partial class Runtime
    {
        public Runtime()
        {
            this.WindowHidden += (s, e) =>
            {
                Runtime.Instance.Store.Add(e.Window.Handle);
            };

            this.WindowShown += (s, e) =>
            {
                if (!e.Window.IsPinned)
                    Runtime.instance.Store.Remove(e.Window.Handle);
            };
        }
        private Settings settings;
        private HiddenWindowStore store;
        private Dictionary<IntPtr, WindowInfo> hiddenWindows = new Dictionary<IntPtr, WindowInfo>();

        public event EventHandler<WindowEventArgs> WindowHidden;
        public event EventHandler<WindowEventArgs> WindowShown;

        public Settings Settings
        {
            get
            {
                if (this.settings == null)
                    this.settings = Settings.Load();

                return this.settings;
            }
            internal set
            {
                this.settings = value;
            }
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

        //internal MainForm MainForm
        //{
        //    get
        //    {
        //        lock (this.instanceSyncObject)
        //        {
        //            if (this.mainForm == null)
        //                this.mainForm = new MainForm();

        //            return this.mainForm;
        //        }
        //    }
        //}

        public WindowInfo LastWindow()
        {
            if (this.hiddenWindows.Count == 0)
                return null;

            return this.hiddenWindows.Last().Value;
        }

        public int Count
        {
            get
            {
                return this.hiddenWindows.Count;
            }
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
    }
}
