using System;
using System.Reflection;
using System.Xml.Linq;
using Ultraviolet.Core;
using Ultraviolet.Core.Xml;

namespace Ultraviolet
{
    /// <summary>
    /// Represents an Ultraviolet activity's internal audio settings.
    /// </summary>
    internal class UltravioletApplicationAudioSettings
    {
        /// <summary>
        /// Saves the specified audio settings to XML.
        /// </summary>
        /// <param name="settings">The audio settings to save.</param>
        /// <returns>An XML element that represents the specified audio settings.</returns>
        public static XElement Save(UltravioletApplicationAudioSettings settings)
        {
            if (settings == null)
                return null;

            return new XElement("Audio",
                new XElement(nameof(PlaybackDeviceName), settings.PlaybackDeviceName),
                new XElement(nameof(AudioMasterVolume), settings.AudioMasterVolume),
                new XElement(nameof(AudioMuted), settings.AudioMuted),
                new XElement(nameof(SongsMasterVolume), settings.SongsMasterVolume),
                new XElement(nameof(SongsMuted), settings.SongsMuted),
                new XElement(nameof(SoundEffectsMasterVolume), settings.SoundEffectsMasterVolume),
                new XElement(nameof(SoundEffectsMuted), settings.SoundEffectsMuted)
            );
        }

        /// <summary>
        /// Loads audio settings from the specified XML element.
        /// </summary>
        /// <param name="xml">The XML element that contains the audio settings to load.</param>
        /// <returns>The audio settings that were loaded from the specified XML element or <see langword="null"/> if 
        /// settings could not be loaded correctly.</returns>
        public static UltravioletApplicationAudioSettings Load(XElement xml)
        {
            if (xml == null)
                return null;

            try
            {
                var settings = new UltravioletApplicationAudioSettings();

                settings.PlaybackDeviceName = xml.ElementValue<String>(nameof(PlaybackDeviceName));
                settings.AudioMasterVolume = xml.ElementValue<Single>(nameof(AudioMasterVolume));
                settings.AudioMuted = xml.ElementValue<Boolean>(nameof(AudioMuted));
                settings.SongsMasterVolume = xml.ElementValue<Single>(nameof(SongsMasterVolume));
                settings.SongsMuted = xml.ElementValue<Boolean>(nameof(SongsMuted));
                settings.SoundEffectsMasterVolume = xml.ElementValue<Single>(nameof(SoundEffectsMasterVolume));
                settings.SoundEffectsMuted = xml.ElementValue<Boolean>(nameof(SoundEffectsMuted));

                return settings;
            }
            catch (FormatException)
            {
                return null;
            }
            catch (TargetInvocationException e)
            {
                if (e.InnerException is FormatException)
                {
                    return null;
                }
                throw;
            }
        }

        /// <summary>
        /// Creates a set of audio settings from the current application state.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <returns>The audio settings which were retrieved.</returns>
        public static UltravioletApplicationAudioSettings FromCurrentSettings(UltravioletContext uv)
        {
            Contract.Require(uv, nameof(uv));

            var audio = uv.GetAudio();
            var settings = new UltravioletApplicationAudioSettings();

            settings.PlaybackDeviceName = audio.PlaybackDevice?.Name;
            settings.AudioMasterVolume = audio.AudioMasterVolume;
            settings.AudioMuted = audio.AudioMuted;
            settings.SongsMasterVolume = audio.SongsMasterVolume;
            settings.SongsMuted = audio.SongsMuted;
            settings.SoundEffectsMasterVolume = audio.SoundEffectsMasterVolume;
            settings.SoundEffectsMuted = audio.SoundEffectsMuted;

            return settings;
        }

        /// <summary>
        /// Applies the specified settings.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public void Apply(UltravioletContext uv)
        {
            Contract.Require(uv, nameof(uv));

            var audio = uv.GetAudio();

            audio.PlaybackDevice = String.IsNullOrEmpty(PlaybackDeviceName) ? null : audio.FindAudioDeviceByName(PlaybackDeviceName);
            audio.AudioMasterVolume = AudioMasterVolume;
            audio.AudioMuted = AudioMuted;
            audio.SongsMasterVolume = SongsMasterVolume;
            audio.SongsMuted = SongsMuted;
            audio.SoundEffectsMasterVolume = SoundEffectsMasterVolume;
            audio.SoundEffectsMuted = SoundEffectsMuted;
        }

        /// <summary>
        /// Gets the name of the selected playback device.
        /// </summary>
        public String PlaybackDeviceName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the master volume for all audio.
        /// </summary>
        public Single AudioMasterVolume
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the game's audio has been muted.
        /// </summary>
        public Boolean AudioMuted
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the master volume for music.
        /// </summary>
        public Single SongsMasterVolume
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the game's music has been muted.
        /// </summary>
        public Boolean SongsMuted
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the master volume for sound effects.
        /// </summary>
        public Single SoundEffectsMasterVolume
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the game's sound effects have been muted.
        /// </summary>
        public Boolean SoundEffectsMuted
        {
            get;
            private set;
        }
    }
}