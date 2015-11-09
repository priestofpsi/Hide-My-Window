using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace theDiary.Tools.HideMyWindow
{
    public sealed class Hotkey : IDisposable
    {
        #region Public Constructors
        public Hotkey() {}
        #endregion

        #region Private Constructors
        private Hotkey(HotkeyFunction function, Keys hotKey, HotModifierKeys modifiers)
        {
            this.Function = function;
            this.HotKey = hotKey;
            this.ModifierKeys = modifiers;
        }
        #endregion

        #region Constant Declarations
        public static Hotkey DefaultHideCurrentWindow = new Hotkey(HotkeyFunction.HideCurrentWindow, Keys.H,
            HotModifierKeys.Control | HotModifierKeys.Shift);

        public static Hotkey DefaultUnhideLastWindow = new Hotkey(HotkeyFunction.UnhideLastWindow, Keys.S,
            HotModifierKeys.Control | HotModifierKeys.Shift);
        #endregion

        #region Declarations
        [XmlAttribute]
        public HotkeyFunction Function;

        [XmlAttribute]
        public Keys HotKey;

        [XmlIgnore]
        internal short ID;

        [XmlAttribute]
        public HotModifierKeys ModifierKeys;
        #endregion

        #region Properties
        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(this.Key) && this.ModifierKeys == 0;
            }
        }

        [XmlIgnore]
        public string HotKeyString
        {
            get
            {
                string keystring = "";
                if ((this.ModifierKeys & HotModifierKeys.Alt) > 0)
                    keystring += "Alt+";
                if ((this.ModifierKeys & HotModifierKeys.Control) > 0)
                    keystring += "Ctrl+";
                if ((this.ModifierKeys & HotModifierKeys.Shift) > 0)
                    keystring += "Shift+";
                if ((this.ModifierKeys & HotModifierKeys.Win) > 0)
                    keystring += "Win+";
                keystring += this.Key;

                return keystring;
            }
        }

        [XmlIgnore]
        public string Key
        {
            get
            {
                if (this.HotKey == 0)
                    return string.Empty;

                return this.HotKey.ToString();
            }
            set
            {
                this.HotKey = (Keys) Enum.Parse(typeof (Keys), value);
            }
        }

        [XmlIgnore]
        public string HKFunction
        {
            get
            {
                return this.Function.ToString();
            }
            set
            {
                this.Function = (HotkeyFunction) Enum.Parse(typeof (HotkeyFunction), value);
            }
        }

        [XmlIgnore]
        public bool Control
        {
            get
            {
                return this.ModifierKeys.HasFlag(HotModifierKeys.Control);
            }
            set
            {
                this.ModHotFlag(value, HotModifierKeys.Control);
            }
        }

        [XmlIgnore]
        public bool Alt
        {
            get
            {
                return this.ModifierKeys.HasFlag(HotModifierKeys.Alt);
            }
            set
            {
                this.ModHotFlag(value, HotModifierKeys.Alt);
            }
        }

        [XmlIgnore]
        public bool Shift
        {
            get
            {
                return this.ModifierKeys.HasFlag(HotModifierKeys.Shift);
            }
            set
            {
                this.ModHotFlag(value, HotModifierKeys.Shift);
            }
        }

        [XmlIgnore]
        public bool Win
        {
            get
            {
                return this.ModifierKeys.HasFlag(HotModifierKeys.Win);
            }
            set
            {
                this.ModHotFlag(value, HotModifierKeys.Win);
            }
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
            if (this.HotKey == Keys.None)
                return false;

            if (this.ID != 0)
                this.Unregister();

            this.ID = ExternalReferences.RegisterGlobalHotKey(this.ID, this.ModifierKeys, this.HotKey);
            return this.ID != 0;
        }

        internal void Unregister()
        {
            ExternalReferences.UnregisterGlobalHotKey(this.ID);
        }

        public override int GetHashCode()
        {
            return this.Function.GetHashCode() | this.HotKey.GetHashCode() | this.Shift.GetHashCode()
                   | this.Win.GetHashCode() | this.Alt.GetHashCode() | this.Control.GetHashCode();
        }
        #endregion

        #region Interface Implementations
        public void Dispose()
        {
            this.Dispose(true);
        }
        #endregion
    }
}