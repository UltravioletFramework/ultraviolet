using System;
using System.Xml.Linq;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents an Ultraviolet activity's internal settings.
    /// </summary>
    internal class UltravioletActivitySettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletActivitySettings"/> class.
        /// </summary>
        private UltravioletActivitySettings()
        {

        }

        /// <summary>
        /// Saves the specified application settings to the specified file.
        /// </summary>
        /// <param name="path">The path to the file in which to save the application settings.</param>
        /// <param name="settings">The <see cref="UltravioletApplicationSettings"/> to serialize to the specified file.</param>
        public static void Save(String path, UltravioletActivitySettings settings)
        {
            var xml = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("Settings",
                    UltravioletActivityAudioSettings.Save(settings.Audio)
                ));
            xml.Save(path);
        }

        /// <summary>
        /// Loads a set of application settings from the specified file.
        /// </summary>
        /// <param name="path">The path to the file from which to load the application settings.</param>
        /// <returns>The <see cref="UltravioletApplicationSettings"/> which were deserialized from the specified file.</returns>
        public static UltravioletActivitySettings Load(String path)
        {
            var xml = XDocument.Load(path);

            var settings = new UltravioletActivitySettings();

            settings.Audio = UltravioletActivityAudioSettings.Load(xml.Root.Element("Audio"));

            if (settings.Audio == null)
                return null;

            return settings;
        }

        /// <summary>
        /// Creates a set of application settings from the current application state.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <returns>The <see cref="UltravioletApplicationSettings"/> which was retrieved.</returns>
        public static UltravioletActivitySettings FromCurrentSettings(UltravioletContext uv)
        {
            Contract.Require(uv, "uv");

            var settings = new UltravioletActivitySettings();

            settings.Audio = UltravioletActivityAudioSettings.FromCurrentSettings(uv);

            return settings;
        }

        /// <summary>
        /// Applies the specified settings.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public void Apply(UltravioletContext uv)
        {
            if (Audio != null)
                Audio.Apply(uv);
        }

        /// <summary>
        /// Gets the activity's audio settings.
        /// </summary>
        public UltravioletActivityAudioSettings Audio
        {
            get;
            private set;
        }
    }
}
