using System;
using Ultraviolet.Audio;
using Ultraviolet.Core;

namespace Ultraviolet.FMOD.Audio
{
    /// <summary>
    /// Represents the FMOD implementation of the <see cref="SongPlayer"/> class.
    /// </summary>
    public sealed unsafe class FMODSongPlayer : SongPlayer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FMODSongPlayer"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public FMODSongPlayer(UltravioletContext uv)
            : base(uv)
        {
            this.channelPlayer = new FMODChannelPlayer(uv);
        }

        /// <inheritdoc/>
        public override void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            channelPlayer.Update(time);
        }

        /// <inheritdoc/>
        public override Boolean Play(Song song, Boolean loop = false)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(song, nameof(song));

            Ultraviolet.ValidateResource(song);
            var sound = ((FMODSong)song).Sound;
            var channelgroup = ((FMODSong)song).ChannelGroup;

            if (channelPlayer.Play(sound, channelgroup, song.Duration, loop))
            {
                OnStateChanged();
                OnSongStarted();
                return true;
            }
            return false;
        }

        /// <inheritdoc/>
        public override Boolean Play(Song song, TimeSpan loopStart, TimeSpan? loopLength)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(song, nameof(song));

            Ultraviolet.ValidateResource(song);
            var sound = ((FMODSong)song).Sound;
            var channelgroup = ((FMODSong)song).ChannelGroup;

            if (channelPlayer.Play(sound, channelgroup, song.Duration, loopStart, loopLength))
            {
                OnStateChanged();
                OnSongStarted();
                return true;
            }
            return false;
        }

        /// <inheritdoc/>
        public override Boolean Play(Song song, Single volume, Single pitch, Single pan, Boolean loop = false)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(song, nameof(song));

            Ultraviolet.ValidateResource(song);
            var sound = ((FMODSong)song).Sound;
            var channelgroup = ((FMODSong)song).ChannelGroup;

            if (channelPlayer.Play(sound, channelgroup, song.Duration, volume, pitch, pan, loop))
            {
                OnStateChanged();
                OnSongStarted();
                return true;
            }
            return false;
        }

        /// <inheritdoc/>
        public override Boolean Play(Song song, Single volume, Single pitch, Single pan, TimeSpan loopStart, TimeSpan? loopLength)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(song, nameof(song));

            Ultraviolet.ValidateResource(song);
            var sound = ((FMODSong)song).Sound;
            var channelgroup = ((FMODSong)song).ChannelGroup;

            if (channelPlayer.Play(sound, channelgroup, song.Duration, volume, pitch, pan, loopStart, loopLength))
            {
                OnStateChanged();
                OnSongStarted();
                return true;
            }
            return false;
        }

        /// <inheritdoc/>
        public override void Stop()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (channelPlayer.Stop())
            {
                OnStateChanged();
                OnSongEnded();
            }
        }

        /// <inheritdoc/>
        public override void Pause()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (channelPlayer.Pause())
            {
                OnStateChanged();
            }
        }

        /// <inheritdoc/>
        public override void Resume()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (channelPlayer.Resume())
            {
                OnStateChanged();
            }
        }

        /// <inheritdoc/>
        public override void SlideVolume(Single volume, TimeSpan time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            channelPlayer.SlideVolume(volume, time);
        }

        /// <inheritdoc/>
        public override void SlidePitch(Single pitch, TimeSpan time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            channelPlayer.SlidePitch(pitch, time);
        }

        /// <inheritdoc/>
        public override void SlidePan(Single pan, TimeSpan time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            channelPlayer.SlidePan(pan, time);
        }

        /// <inheritdoc/>
        public override PlaybackState State => channelPlayer.State;

        /// <inheritdoc/>
        public override Boolean IsPlaying => channelPlayer.IsPlaying;

        /// <inheritdoc/>
        public override Boolean IsLooping
        {
            get => channelPlayer.IsLooping;
            set => channelPlayer.IsLooping = value;
        }

        /// <inheritdoc/>
        public override TimeSpan Position
        {
            get => channelPlayer.Position;
            set => channelPlayer.Position = value;
        }

        /// <inheritdoc/>
        public override TimeSpan Duration => channelPlayer.Duration;

        /// <inheritdoc/>
        public override Single Volume
        {
            get => channelPlayer.Volume;
            set => channelPlayer.Volume = value;
        }

        /// <inheritdoc/>
        public override Single Pitch
        {
            get => channelPlayer.Pitch;
            set => channelPlayer.Pitch = value;
        }

        /// <inheritdoc/>
        public override Single Pan
        {
            get => channelPlayer.Pan;
            set => channelPlayer.Pan = value;
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                if (Ultraviolet != null && !Ultraviolet.Disposed)
                    channelPlayer.Dispose();
            }
            base.Dispose(disposing);
        }
        
        // State values.
        private readonly FMODChannelPlayer channelPlayer;
    }
}
