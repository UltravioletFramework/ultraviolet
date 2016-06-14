using System;
using System.IO;

namespace TwistedLogik.Ultraviolet
{
    partial class UltravioletApplication
    {
        /// <summary>
        /// Loads the application's settings.
        /// </summary>
        partial void LoadSettings()
        {
            if (!PreserveApplicationSettings)
                return;

            var path = Path.Combine(GetLocalApplicationSettingsDirectory(), "UltravioletSettings.xml");
            if (!File.Exists(path))
                return;

            var settings = UltravioletApplicationSettings.Load(path);
            if (settings == null)
                return;

            this.settings = settings;
        }

        /// <summary>
        /// Saves the application's settings.
        /// </summary>
        partial void SaveSettings()
        {
            if (!PreserveApplicationSettings)
                return;

            var path = Path.Combine(GetLocalApplicationSettingsDirectory(), "UltravioletSettings.xml");

            this.settings = UltravioletApplicationSettings.FromCurrentSettings(Ultraviolet);
            UltravioletApplicationSettings.Save(path, settings);
        }

        /// <summary>
        /// Applies the application's settings.
        /// </summary>
        partial void ApplySettings()
        {
            if (this.settings == null)
                return;

            this.settings.Apply(uv);
        }

        /// <summary>
        /// Populates the Ultraviolet configuration from the application settings.
        /// </summary>
        partial void PopulateConfigurationFromSettings(UltravioletConfiguration configuration)
        {
            if (this.settings?.Window != null)
            {
                configuration.InitialWindowPosition = this.settings.Window.WindowedPosition;
            }
        }

        // The application's settings.
        private UltravioletApplicationSettings settings;
    }
}
