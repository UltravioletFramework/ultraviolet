using System;
using System.Xml.Linq;
using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Represents an Ultraviolet application's internal settings.
    /// </summary>
    internal class UltravioletApplicationSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletApplicationSettings"/> class.
        /// </summary>
        private UltravioletApplicationSettings()
        {
            Window = new UltravioletApplicationWindowSettings();
        }

        /// <summary>
        /// Saves the specified application settings to the specified file.
        /// </summary>
        /// <param name="path">The path to the file in which to save the application settings.</param>
        /// <param name="settings">The <see cref="UltravioletApplicationSettings"/> to serialize to the specified file.</param>
        public static void Save(String path, UltravioletApplicationSettings settings)
        {
            var xml = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("Settings",
                    UltravioletApplicationWindowSettings.Save(settings.Window),
                    UltravioletApplicationAudioSettings.Save(settings.Audio)
                ));
            xml.Save(path);
        }

        /// <summary>
        /// Loads a set of application settings from the specified file.
        /// </summary>
        /// <param name="path">The path to the file from which to load the application settings.</param>
        /// <returns>The <see cref="UltravioletApplicationSettings"/> which were deserialized from the specified file
        /// or <see langword="null"/> if settings could not be loaded correctly.</returns>
        public static UltravioletApplicationSettings Load(String path)
        {
            var xml = XDocument.Load(path);

            var settings = new UltravioletApplicationSettings();

            settings.Window = UltravioletApplicationWindowSettings.Load(xml.Root.Element("Window"));
            settings.Audio = UltravioletApplicationAudioSettings.Load(xml.Root.Element("Audio"));

            if (settings.Window == null && settings.Audio == null)
                return null;

            return settings;
        }

        /// <summary>
        /// Creates a set of application settings from the current application state.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <returns>The <see cref="UltravioletApplicationSettings"/> which was retrieved.</returns>
        public static UltravioletApplicationSettings FromCurrentSettings(UltravioletContext uv)
        {
            Contract.Require(uv, nameof(uv));

            var settings = new UltravioletApplicationSettings();

            settings.Window = UltravioletApplicationWindowSettings.FromCurrentSettings(uv);
            settings.Audio = UltravioletApplicationAudioSettings.FromCurrentSettings(uv);

            return settings;
        }

        /// <summary>
        /// Applies the specified settings.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public void Apply(UltravioletContext uv)
        {
            if (Window != null)
                Window.Apply(uv);

            if (Audio != null)
                Audio.Apply(uv);
        }

        /// <summary>
        /// Gets the application's window settings.
        /// </summary>
        public UltravioletApplicationWindowSettings Window
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the application's audio settings.
        /// </summary>
        public UltravioletApplicationAudioSettings Audio
        {
            get;
            private set;
        }
    }
}