namespace theDiary.Tools.HideMyWindow.Forms.ConfigurationSections
{
    using System;

    public interface IConfigurationSection
    {
        #region Properties

        string SectionName { get; }

        bool ConfigurationChanged { get; }

        #endregion

        #region Methods & Functions

        void Activated(object sender, EventArgs e);

        void ResetConfiguration(object sender, EventArgs e);

        void SaveConfiguration(object sender, EventArgs e);

        void LoadConfiguration(object sender, EventArgs e);

        #endregion
    }
}