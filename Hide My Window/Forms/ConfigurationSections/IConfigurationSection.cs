using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace theDiary.Tools.HideMyWindow.Forms.ConfigurationSections
{
    public interface IConfigurationSection
    {
        string SectionName
        {
            get;
        }

        bool ConfigurationChanged
        {
            get;
        }
        
        void Activated(object sender, EventArgs e);

        void ResetConfiguration(object sender, EventArgs e);

        void SaveConfiguration(object sender, EventArgs e);

        void LoadConfiguration(object sender, EventArgs e);

    }
}
