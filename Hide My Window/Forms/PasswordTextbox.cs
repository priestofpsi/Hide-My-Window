using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace theDiary.Tools.HideMyWindow
{
    public class PasswordTextBox
        : WatermarkTextBox
    {
        public PasswordTextBox()
        {
            
        }

        public string Password
        {
            get
            {
                if (string.IsNullOrEmpty(this.Text)
                    ||this.Text == this.WaterMark)
                    return string.Empty;

                return this.Text;
            }
            set
            {
                this.Text = value;
            }
        }

        public bool ClearPassword
        {
            get;
            set;
        }
    }
}
