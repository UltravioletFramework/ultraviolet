using System.IO;
using System.Xml;

namespace Ultraviolet
{
    partial class UltravioletApplication
    {
        /// <summary>
        /// Loads the application's settings.
        /// </summary>
        partial void LoadSettings()
        {
            lock (stateSyncObject)
            {
                if (!PreserveApplicationSettings)
                    return;

                var directory = GetLocalApplicationSettingsDirectory();
                var path = Path.Combine(directory, "UltravioletSettings.xml");

                try
                {
                    var settings = UltravioletApplicationSettings.Load(path);
                    if (settings == null)
                        return;

                    this.settings = settings;
                }
                catch (FileNotFoundException) { }
                catch (DirectoryNotFoundException) { }
                catch (XmlException) { }
            }
        }

        /// <summary>
        /// Saves the application's settings.
        /// </summary>
        partial void SaveSettings()
        {
            lock (stateSyncObject)
            {
                if (!PreserveApplicationSettings)
                    return;

                var directory = GetLocalApplicationSettingsDirectory();
                var path = Path.Combine(directory, "UltravioletSettings.xml");

                this.settings = UltravioletApplicationSettings.FromCurrentSettings(Ultraviolet);
                UltravioletApplicationSettings.Save(path, settings);
            }
        }

        /// <summary>
        /// Applies the application's settings.
        /// </summary>
        partial void ApplySettings()
        {
            lock (stateSyncObject)
            {
                if (this.settings == null)
                    return;

                this.settings.Apply(uv);
            }
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