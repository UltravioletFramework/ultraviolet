using System;
using System.Collections.Generic;
using Ultraviolet.Audio;

namespace Ultraviolet
{
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
        /// Suspends all audio output.
        /// </summary>
        void Suspend();

        /// <summary>
        /// Resumes audio output after a call to <see cref="Suspend"/>.
        /// </summary>
        void Resume();

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
