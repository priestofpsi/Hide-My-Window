using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace theDiary.Tools.HideMyWindow
{
    public sealed class HotKeyBindingList : BindingList<Hotkey>
    {
        #region Methods & Functions
        public Hotkey GetById(short id)
        {
            return this.FirstOrDefault(item => item.ID.Equals(id));
        }
        #endregion
    }
}