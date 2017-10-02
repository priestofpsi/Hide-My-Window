using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow.Forms.ConfigurationSections
{
    internal static class ConfigurationSectionExtensions
    {
        public static void SetConfigurationToControl(this IConfigurationSection section, Control control, string propertyName, object value)
        {
            control.Tag = value;
            control.GetType().GetProperty(propertyName).SetValue(control, value);
        }
        public static void SetConfigurationToControl<T>(this IConfigurationSection<T> section, Control control, string propertyName, string configurationName)
        {
            object value = typeof(T).GetProperty(configurationName).GetValue(section.ConfigurationSection);
            section.SetConfigurationToControl(control, propertyName, value);
        }

        public static void ResetConfigurationValue(this IConfigurationSection section, Control control, object value)
        {
            control.Tag = value;
        }

        public static void SetConfigurationFromControl<T>(this IConfigurationSection<T> section, Control control, string propertyName, string configurationName)
        {
            typeof(T).GetProperty(configurationName).SetValue(section.ConfigurationSection, control.GetType().GetProperty(propertyName).GetValue(control));
        }
    }
}
