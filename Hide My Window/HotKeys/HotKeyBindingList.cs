namespace theDiary.Tools.HideMyWindow
{
    using System.ComponentModel;
    using System.Linq;

    public sealed class HotKeyBindingList : BindingList<HotKey>
    {
        public bool Exists(short id)
        {
            return this.Any(key => key.Id.Equals(id));
        }

        public bool Exists(HotKey key)
        {
            return this.Any(hotKey => hotKey.Equals(key));
        }

        public bool Exists(HotKeyFunction function)
        {
            return this.Any(key => key.Function.Equals(function));
        }

        #region Methods & Functions

        public HotKeyFunction GetFunction(short id)
        {
            if (!this.Exists(id))
                return HotKeyFunction.None;

            return this.Find(id).Function;
        }

        public HotKey Find(short id)
        {
            return this.FirstOrDefault(item => item.Id.Equals(id));
        }

        public HotKey this[HotKeyFunction function]
        {
            get { return this.Find(function); }
            set
            {
                int index = this.IndexOf(function);
                if (index == -1)
                {
                    this.Add(value);
                }
                else
                {
                    this[index] = value;
                }
            }
        }

        private int IndexOf(HotKeyFunction function)
        {
            for (int i = 0; i < this.Count; i++)
                if (this[i].Function == function)
                    return i;

            return -1;
        }

        public HotKey Find(HotKeyFunction function)
        {
            return this.FirstOrDefault(item => item.Function.Equals(function));
        }

        #endregion
    }
}