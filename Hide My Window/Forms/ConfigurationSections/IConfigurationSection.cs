namespace theDiary.Tools.HideMyWindow.Forms.ConfigurationSections
{
    using System;

    /// <summary>
    /// Specifies the properties and methods that define a Configurable Section within the application.
    /// </summary>
    public interface IConfigurationSection
    {
        #region Properties

        /// <summary>
        /// Gets the name identifing the section.
        /// </summary>
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

    public interface IConfigurationSection<T>
        : IConfigurationSection
    {
        /// <summary>
        /// Gets the object that contains the values used into the configuration.
        /// </summary>
        T ConfigurationSection { get; }
    }
}