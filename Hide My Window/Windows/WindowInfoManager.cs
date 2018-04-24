namespace theDiary.Tools.HideMyWindow
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class WindowInfoManager : IEnumerable<WindowInfo>
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="WindowInfoManager" /> class.
        /// </summary>
        public WindowInfoManager()
        {
            instance = this;
        }

        #endregion

        #region Declarations

        #region Public Static Declarations

        private static volatile WindowInfoManager instance;
        private static readonly object syncObject = new object();
        private static volatile Dictionary<IntPtr, WindowInfo> items = new Dictionary<IntPtr, WindowInfo>();

        #endregion

        #region Private Static Declarations

        private Task checkApplicationProcessesTask;

        #endregion

        #endregion

        #region Properties

        private Dictionary<IntPtr, WindowInfo> Items
        {
            get
            {
                lock (syncObject)
                    return items;
            }
        }

        /// <summary>
        ///     Gets the number of <see cref="WindowInfo" /> instances contained in the <see cref="WindowInfoManager" />.
        /// </summary>
        public int Count
        {
            get { return this.Items.Count; }
        }

        public WindowInfo this[IntPtr handle]
        {
            get
            {
                lock (syncObject)
                {
                    if (items.ContainsKey(handle))
                        return items[handle];

                    return null;
                }
            }
        }

        private bool CanCheckProcesses
        {
            get { return this.Count > 0; }
        }

        private bool Running
        {
            get
            {
                return this.checkApplicationProcessesTask != null
                       && this.checkApplicationProcessesTask.Status == TaskStatus.Running;
            }
        }

        public WindowInfo LastWindow { get; private set; }

        #endregion

        #region Methods & Functions

        /// <summary>
        ///     The event that is raised when a Window has been registered.
        /// </summary>
        public event WindowEventHandler Registered;

        public event WindowEventHandler WindowHidden;

        public event WindowEventHandler WindowShown;

        public event WindowEventHandler WindowPinned;
        public event WindowEventHandler WindowUnpinned;

        /// <summary>
        ///     The event that is raised when a Window has been unregistered.
        /// </summary>
        public event WindowEventHandler UnRegistered;

        /// <summary>
        ///     Indicates if the <see cref="WindowInfoManager" /> contains the specified <paramref name="window" /> instance.
        /// </summary>
        /// <param name="window">The <see cref="WindowInfo" /> instance to check for.</param>
        /// <returns>
        ///     <c>True</c> if the <see cref="WindowInfoManager" /> contains the <paramref name="window" />, otherwise
        ///     <c>False</c>.
        /// </returns>
        public bool Exists(WindowInfo window)
        {
            return this.Items.ContainsKey(window.Handle);
        }

        /// <summary>
        ///     Indicates if the <see cref="WindowInfoManager" /> contains the specified <paramref name="windowHandle" />.
        /// </summary>
        /// <param name="windowHandle">A <see cref="IntPtr" /> reference to the handle to check for.</param>
        /// <returns>
        ///     <c>True</c> if the <see cref="WindowInfoManager" /> contains the <paramref name="windowHandle" />, otherwise
        ///     <c>False</c>.
        /// </returns>
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

        private async void RegisterAsync(WindowInfo window)
        {
            await Task.Run((Action)(() =>
            {
                this.LastWindow = window;
                window.Shown += this.Window_Shown;
                window.Hidden += this.Window_Hidden;
                window.Pinned += this.Window_Pinned;
                window.Unpinned += this.Window_Unpinned;

                Runtime.Instance.Store.Add(window.Handle);
                this.WindowHidden?.Invoke(this, new WindowInfoEventArgs(window));
                this.Registered?.Invoke(this, new WindowInfoEventArgs(window));                
            }));
        }

        private void Window_Unpinned(object sender, WindowInfoEventArgs e)
        {
            this.WindowUnpinned?.Invoke(sender, e);
        }

        private void Window_Pinned(object sender, WindowInfoEventArgs e)
        {
            this.WindowPinned?.Invoke(sender, e);
        }

        public bool Register(WindowInfo window)
        {
            try
            {
                lock (syncObject)
                {
                    if (!window.IsValid
                        || items.ContainsKey(window.Handle))
                        return false;

                    items.Add(window.Handle, window);
                    this.RegisterAsync(window);
                }
            }
            finally
            {
                this.CheckApplicationProcesses();
            }
            return true;
        }

        public bool UnRegister(IntPtr windowHandle)
        {
            return this.UnRegister(WindowInfo.Find(windowHandle));
        }

        private async void UnregisterAsync(WindowInfo window)
        {
            await Task.Run(() =>
            {                
                Runtime.Instance.Store.Remove(window.Handle);
                this.UnRegistered?.Invoke(this, new WindowInfoEventArgs(window));

                window.Shown -= this.Window_Shown;
                window.Hidden -= this.Window_Hidden;
                window.Pinned -= this.Window_Pinned;
                window.Unpinned -= this.Window_Unpinned;
            });
        }

        public bool UnRegister(WindowInfo window)
        {
            try
            {
                lock (syncObject)
                {
                    if (!items.Remove(window.Handle))
                        return false;
                    this.UnregisterAsync(window);
                }
            }
            finally
            {
                this.CheckApplicationProcesses();
            }
            return true;
        }

        private void Window_Shown(object sender, WindowInfoEventArgs e)
        {
            this.WindowShown?.Invoke(this, e);
            if (!e.Window.IsPinned)
                this.UnRegister(e.Window);
        }

        private void Window_Hidden(object sender, WindowInfoEventArgs e)
        {
            this.WindowHidden?.Invoke(this, e);
        }

        private void CheckApplicationProcesses()
        {
            if (this.Running)
                return;

            this.checkApplicationProcessesTask = Task.Run(() =>
            {
                Thread.CurrentThread.Name =
                    "HideMyWindow:CheckWindowProcesses";
                while (this.CanCheckProcesses)
                {
                    IntPtr[] handles = this.Items.Keys.ToArray();
                    handles.AsParallel()
                        .ForAll(
                            handle => this.CheckWindowProcess(handle));
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
            return instance[handle];
        }

        #endregion

        #region Interface Implementations

        public IEnumerator<WindowInfo> GetEnumerator()
        {
            return this.Items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Items.Values.GetEnumerator();
        }

        #endregion
    }
}