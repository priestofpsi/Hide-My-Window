using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace theDiary.Tools.HideMyWindow
{
    public sealed class WindowInfoManager
        : IEnumerable<WindowInfo>
    {
        public WindowInfoManager()
        {
            WindowInfoManager.Instance = this;
        }

        private static volatile WindowInfoManager Instance;
        private static readonly object syncObject = new object();
        private static volatile Dictionary<IntPtr, WindowInfo> items = new Dictionary<IntPtr, WindowInfo>();
        private Task checkApplicationProcessesTask = null;
        #region Private Declarations

        #endregion

        public event WindowEventHandler Registered;
        public event WindowEventHandler UnRegistered;
        #region Properties
        private Dictionary<IntPtr, WindowInfo> Items
        {
            get
            {
                lock (WindowInfoManager.syncObject)
                    return WindowInfoManager.items;
            }
        }

        /// <summary>
        /// Gets the number of <see cref="WindowInfo"/> instances contained in the <see cref="WindowInfoManager"/>.
        /// </summary>
        public int Count
        {
            get
            {
                return this.Items.Count;
            }
        }

        public WindowInfo this[IntPtr handle]
        {
            get
            {
                lock (WindowInfoManager.syncObject)
                {
                    if (WindowInfoManager.items.ContainsKey(handle))
                        return WindowInfoManager.items[handle];

                    return null;
                }

            }
        }

        private bool CanCheckProcesses
        {
            get
            {
                return this.Count > 0;
            }
        }

        private bool Running
        {
            get
            {
                return this.checkApplicationProcessesTask != null
                       && this.checkApplicationProcessesTask.Status == TaskStatus.Running;
            }
        }
        #endregion

        #region Public Methods & Functions
        /// <summary>
        /// Indicates if the <see cref="WindowInfoManager"/> contains the specified <paramref name="window"/> instance.
        /// </summary>
        /// <param name="window">The <see cref="WindowInfo"/> instance to check for.</param>
        /// <returns><c>True</c> if the <see cref="WindowInfoManager"/> contains the <paramref name="window"/>, otherwise <c>False</c>.</returns>
        public bool Exists(WindowInfo window)
        {
            return this.Items.ContainsKey(window.Handle);
        }

        /// <summary>
        /// Indicates if the <see cref="WindowInfoManager"/> contains the specified <paramref name="windowHandle"/>.
        /// </summary>
        /// <param name="windowHandle">A <see cref="IntPtr"/> reference to the handle to check for.</param>
        /// <returns><c>True</c> if the <see cref="WindowInfoManager"/> contains the <paramref name="windowHandle"/>, otherwise <c>False</c>.</returns>
        public bool Exists(IntPtr windowHandle)
        {
            return this.Items.ContainsKey(windowHandle);
        }

        public bool TryGetLastWindow(out WindowInfo lastWindow)
        {
            lastWindow = null;
            if (this.Count > 0)
                lastWindow = this.GetLastWindow();

            return (lastWindow != null);
        }

        public WindowInfo GetLastWindow()
        {
            return this.Items.Values.LastOrDefault();
        }

        public bool Register(IntPtr windowHandle)
        {
            return this.Register(WindowInfo.Find(windowHandle));
        }

        public bool Register(WindowInfo window)
        {
            try
            {

                lock (WindowInfoManager.syncObject)
                {
                    if (!window.IsValid || WindowInfoManager.items.ContainsKey(window.Handle))
                        return false;

                    WindowInfoManager.items.Add(window.Handle, window);
                    //Task task = Task.Run(() =>
                    //{
                        window.Shown += this.Window_Shown;
                        Runtime.Instance.Store.Add(window.Handle);
                        if (this.Registered != null)
                            this.Registered(this, new WindowInfoEventArgs(window));
                    //});
                    return true;
                }
            }
            finally
            {
                this.CheckApplicationProcesses();
            }
        }

        public bool UnRegister(IntPtr windowHandle)
        {
            return this.UnRegister(WindowInfo.Find(windowHandle));
        }

        public bool UnRegister(WindowInfo window)
        {
            try
            {
                lock (WindowInfoManager.syncObject)
                {
                    if (WindowInfoManager.items.ContainsKey(window.Handle)
                        && WindowInfoManager.items.Remove(window.Handle))
                    {
                        //Task task = Task.Run(() =>
                        //{
                            window.Shown -= this.Window_Shown;
                            Runtime.Instance.Store.Remove(window.Handle);
                            if (this.UnRegistered != null)
                                this.UnRegistered(this, new WindowInfoEventArgs(window));
                        //});
                        
                        return true;
                    }
                    return false;
                }
            }
            finally
            {
                this.CheckApplicationProcesses();
            }
        }

        public IEnumerator<WindowInfo> GetEnumerator()
        {
            return this.Items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Items.Values.GetEnumerator();
        }
        #endregion
        private void Window_Shown(object sender, WindowInfoEventArgs e)
        {
            if (!e.Window.IsPinned)
                this.UnRegister(e.Window);
        }

        private void CheckApplicationProcesses()
        {
            if (this.Running)
                return;

            this.checkApplicationProcessesTask = Task.Run(() =>
            {
                Thread.CurrentThread.Name = "HideMyWindow:CheckWindowProcesses";
                while (this.CanCheckProcesses)
                {
                    IntPtr[] handles = this.Items.Keys.ToArray();
                    handles.AsParallel().ForAll(handle => this.CheckWindowProcess(handle));
                    Thread.Sleep(100);
                }
            });
        }

        private void CheckWindowProcess(IntPtr handle)
        {
            WindowInfo window = WindowInfo.Find(handle);
            if (!window.CheckWindowProcess())
                this.UnRegister(window);
        }

        public static WindowInfo Find(IntPtr handle)
        {
            return WindowInfoManager.Instance[handle];
        }
    }
}
