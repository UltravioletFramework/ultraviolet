using System;
using System.Collections.Generic;
using Ultraviolet.Audio;

namespace Ultraviolet
{
    /// <summary>
    /// Initializes a new instance of the IUltravioletAudio implementation.
    /// </summary>
    /// <param name="context">The Ultraviolet context.</param>
    /// <param name="configuration">The Ultraviolet configuration settings for the current context.</param>
    public delegate IUltravioletAudio UltravioletAudioFactory(UltravioletContext context, UltravioletConfiguration configuration);

    /// <summary>
    /// Represents the Ultraviolet Framework's audio subsystem.
    /// </summary>
    public interface IUltravioletAudio : IUltravioletSubsystem
    {
        /// <summary>
        /// Produces an enumeration of the system's currently installed audio devices.
        /// </summary>
        /// <returns>A collection which contains the system's currently installed audio devices.</returns>
        IEnumerable<IUltravioletAudioDevice> EnumerateAudioDevices();

        /// <summary>
        /// Searches the list of known audio devices for a valid device with the specified name.
        /// </summary>
        /// <param name="name">The name of the audio device for which to search.</param>
        /// <returns>The first valid audio device with the specified name, if it is found; otherwise, <see langword="null"/>.</returns>
        IUltravioletAudioDevice FindAudioDeviceByName(String name);

        /// <summary>
        /// Suspends all audio output.
        /// </summary>
        void Suspend();

        /// <summary>
        /// Resumes audio output after a call to <see cref="Suspend"/>.
        /// </summary>
        void Resume();

        /// <summary>
        /// Gets or sets the device which is used for audio playback.
        /// </summary>
        IUltravioletAudioDevice PlaybackDevice { get; set; }

        /// <summary>
        /// Gets a <see cref="AudioCapabilities"/> object which exposes the capabilities of the current audio device.
        /// </summary>
        AudioCapabilities Capabilities { get; }

        /// <summary>
        /// Gets or sets the master volume for all audio output.
        /// </summary>
        Single AudioMasterVolume { get; set; }

        /// <summary>
        /// Gets or sets the master volume for songs.
        /// </summary>
        Single SongsMasterVolume { get; set; }

        /// <summary>
        /// Gets or sets the master volume for sound effects.
        /// </summary>
        Single SoundEffectsMasterVolume { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether all audio output is globally muted.
        /// </summary>
        Boolean AudioMuted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether songs are globally muted.
        /// </summary>
        Boolean SongsMuted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether sound effects are globally muted.
        /// </summary>
        Boolean SoundEffectsMuted { get; set; }
    }
}
