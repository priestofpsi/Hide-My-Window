using System;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace theDiary.Tools.HideMyWindow
{
    public sealed class Hotkey
        : IDisposable
    {
        [XmlIgnore]
        public string HotKeyString
        {
            get
            {
                var keystring = "";
                if ((this.ModifierKeys & HotModifierKeys.Alt) > 0)
                    keystring += "Alt+";
                if ((this.ModifierKeys & HotModifierKeys.Control) > 0)
                    keystring += "Ctrl+";
                if ((this.ModifierKeys & HotModifierKeys.Shift) > 0)
                    keystring += "Shift+";
                if ((this.ModifierKeys & HotModifierKeys.Win) > 0)
                    keystring += "Win+";
                keystring += this.Key.ToString();

                return keystring;
            }
        }

        [XmlAttribute]
        public HotkeyFunction Function;

        [XmlAttribute]
        public HotModifierKeys ModifierKeys;

        [XmlAttribute]
        public Keys HotKey;

        [XmlIgnore]
        internal short ID;

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
                this.HotKey = (Keys) Enum.Parse(typeof(Keys), value);
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
                this.Function = (HotkeyFunction) Enum.Parse(typeof(HotkeyFunction), value);
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

        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Unregister();
            }
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
            ExternalReferences.UnregisterGlobalHotKey(ID);
        }

        public override int GetHashCode()
        {
            return this.Function.GetHashCode()
                | this.HotKey.GetHashCode()
                | this.Shift.GetHashCode()
                | this.Win.GetHashCode()
                | this.Alt.GetHashCode()
                | this.Control.GetHashCode();
        }
    }
}
