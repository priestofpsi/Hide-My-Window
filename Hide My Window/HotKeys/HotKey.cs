namespace theDiary.Tools.HideMyWindow
{
    using System;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    public sealed class HotKey : IDisposable
    {
        #region Constructors

        public HotKey()
        {
        }

        internal HotKey(HotKey hotKey)
            : this(hotKey.Function, hotKey.Key, hotKey.ModifierKeys)
        {
        }

        public HotKey(HotKeyFunction function)
            : this()
        {
            this.Function = function;
        }

        private HotKey(HotKeyFunction function, Keys key, HotModifierKeys modifiers)
            : this()
        {
            this.Function = function;
            this.Key = key;
            this.ModifierKeys = modifiers;
        }

        #endregion

        #region Declarations

        #region Static Declarations

        public static HotKey DefaultHideCurrentWindow = new HotKey(HotKeyFunction.HideCurrentWindow, Keys.H,
            HotModifierKeys.Control | HotModifierKeys.Shift);

        public static HotKey DefaultUnhideLastWindow = new HotKey(HotKeyFunction.UnhideLastWindow, Keys.S,
            HotModifierKeys.Control | HotModifierKeys.Shift);

        public static HotKey DefaultToggleLastWindow = new HotKey(HotKeyFunction.ToggleLastWindow, Keys.H,
            HotModifierKeys.Control | HotModifierKeys.Shift | HotModifierKeys.Alt);

        #endregion

        #region Public Declarations

        [XmlAttribute] public HotKeyFunction Function;

        [XmlAttribute] public Keys Key;

        [XmlAttribute] public HotModifierKeys ModifierKeys;

        #endregion

        #endregion

        #region Interface Implementations

        public void Dispose()
        {
            this.Dispose(true);
        }

        #endregion

        [XmlIgnore] internal int Id;

        #region Properties

        public bool IsEmpty
        {
            get
            {
                return this.Key == Keys.None
                       || this.ModifierKeys == HotModifierKeys.None;
            }
        }


        private static string GetModifierString(HotModifierKeys keys, params HotModifierKeys[] modifiers)
        {
            StringBuilder returnValue = new StringBuilder();
            foreach (HotModifierKeys mod in modifiers)
                if ((keys & mod) > 0)
                    returnValue.AppendFormat("{0}+", (mod == HotModifierKeys.Control) ? "Ctrl" : mod.ToString());

            return returnValue.ToString();
        }

        [XmlIgnore]
        public string HKFunction
        {
            get { return this.Function.ToString(); }
            set { this.Function = (HotKeyFunction) Enum.Parse(typeof (HotKeyFunction), value); }
        }

        [XmlIgnore]
        public bool Control
        {
            get { return this.ModifierKeys.HasFlag(HotModifierKeys.Control); }
            set { this.ModHotFlag(value, HotModifierKeys.Control); }
        }

        [XmlIgnore]
        public bool Alt
        {
            get { return this.ModifierKeys.HasFlag(HotModifierKeys.Alt); }
            set { this.ModHotFlag(value, HotModifierKeys.Alt); }
        }

        [XmlIgnore]
        public bool Shift
        {
            get { return this.ModifierKeys.HasFlag(HotModifierKeys.Shift); }
            set { this.ModHotFlag(value, HotModifierKeys.Shift); }
        }

        [XmlIgnore]
        public bool Win
        {
            get { return this.ModifierKeys.HasFlag(HotModifierKeys.Win); }
            set { this.ModHotFlag(value, HotModifierKeys.Win); }
        }

        #endregion

        #region Methods & Functions

        private void Dispose(bool disposing)
        {
            if (disposing)
                this.Unregister();
        }

        private void ModHotFlag(bool enabled, HotModifierKeys toggleEnum)
        {
            if (enabled)
                this.ModifierKeys |= toggleEnum;
            else
                this.ModifierKeys &= ~toggleEnum;
        }

        internal bool Register()
        {
            //if (this.HotKey == Keys.None)
            if (this.IsEmpty)
                return false;

            if (this.Id != 0)
                this.Unregister();

            this.Id = GlobalHotKeyManager.RegisterGlobalHotKey(this.Id, this.ModifierKeys, this.Key);
            return this.Id != 0;
        }

        internal void Unregister()
        {
            GlobalHotKeyManager.UnregisterGlobalHotKey(this.Id);
        }

        public override int GetHashCode()
        {
            return this.Function.GetHashCode() | this.Key.GetHashCode() | this.Shift.GetHashCode()
                   | this.Win.GetHashCode() | this.Alt.GetHashCode() | this.Control.GetHashCode();
        }

        public override string ToString()
        {
            if (this.IsEmpty)
                return "Not Set";

            return string.Format("{0}{1}",
                GetModifierString(this.ModifierKeys, HotModifierKeys.Alt, HotModifierKeys.Shift,
                    HotModifierKeys.Win, HotModifierKeys.Control), this.Key);
        }

        #endregion
    }
}