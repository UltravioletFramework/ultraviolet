using System;
using System.Diagnostics.CodeAnalysis;
using Ultraviolet.Audio;
using Ultraviolet.BASS.Native;
using Ultraviolet.Core;

namespace Ultraviolet.BASS.Audio
{
    /// <summary>
    /// Represents the BASS implementation of the SoundEffectPlayer class.
    /// </summary>
    public sealed class BASSSoundEffectPlayer : SoundEffectPlayer
    {
        /// <summary>
        /// Initializes a new instance of the BASSSoundEffectPlayer class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public BASSSoundEffectPlayer(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <inheritdoc/>
        public override void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public override Boolean Play(SoundEffect soundEffect, Boolean loop = false)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return PlayInternal(soundEffect, 1f, 0f, 0f, loop);
        }

        /// <inheritdoc/>
        public override Boolean Play(SoundEffect soundEffect, Single volume, Single pitch, Single pan, Boolean loop = false)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return PlayInternal(soundEffect, volume, pitch, pan, loop);
        }

        /// <inheritdoc/>
        public override void Stop()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            
            if (State != PlaybackState.Stopped)
            {
                if (!BASSNative.ChannelStop(channel))
                    throw new BASSException();
            }

            Release();
        }

        /// <inheritdoc/>
        public override void Pause()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            EnsureChannelIsValid();

            if (State == PlaybackState.Playing)
            {
                if (!BASSNative.ChannelPause(channel))
                    throw new BASSException();
            }
        }

        /// <inheritdoc/>
        public override void Resume()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            EnsureChannelIsValid();

            if (State == PlaybackState.Paused)
            {
                if (!BASSNative.ChannelPlay(channel, false))
                    throw new BASSException();
            }
        }

        /// <inheritdoc/>
        public override void SlideVolume(Single volume, TimeSpan time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            EnsureChannelIsValid();

            BASSUtil.SlideVolume(channel, volume, time);
        }

        /// <inheritdoc/>
        public override void SlidePitch(Single pitch, TimeSpan time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            EnsureChannelIsValid();

            PromoteToStream(0f);
            BASSUtil.SlidePitch(channel, pitch, time);
        }

        /// <inheritdoc/>
        public override void SlidePan(Single pan, TimeSpan time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            EnsureChannelIsValid();

            BASSUtil.SlidePan(channel, pan, time);
        }

        /// <inheritdoc/>
        public override PlaybackState State
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (BASSUtil.IsValidHandle(channel))
                {
                    switch (BASSNative.ChannelIsActive(channel))
                    {
                        case BASSNative.BASS_ACTIVE_STALLED:
                            return PlaybackState.Playing;

                        case BASSNative.BASS_ACTIVE_PLAYING:
                            return PlaybackState.Playing;

                        case BASSNative.BASS_ACTIVE_STOPPED:
                            return PlaybackState.Stopped;

                        case BASSNative.BASS_ACTIVE_PAUSED:
                            return PlaybackState.Stopped;
                    }
                }

                return PlaybackState.Stopped;
            }
        }

        /// <inheritdoc/>
        public override Boolean IsPlaying
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return State == PlaybackState.Playing;
            }
        }

        /// <inheritdoc/>
        public override Boolean IsLooping
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (!IsChannelValid())
                    return false;

                return BASSUtil.GetIsLooping(channel);
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                EnsureChannelIsValid();

                BASSUtil.SetIsLooping(channel, value);
            }
        }

        /// <inheritdoc/>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public override TimeSpan Position
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (!IsChannelValid())
                    return TimeSpan.Zero;

                if (!BASSUtil.IsValidHandle(stream))
                {
                    var position = BASSUtil.GetPositionInSeconds(channel);
                    return TimeSpan.FromSeconds(position);
                }
                else
                {
                    var position = BASSNative.ChannelBytes2Seconds(channel, (ulong)sampleDataPosition);
                    if (position < 0)
                        throw new BASSException();

                    return TimeSpan.FromSeconds(position);
                }
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                EnsureChannelIsValid();

                if (value.TotalSeconds < 0 || value > Duration)
                    throw new ArgumentOutOfRangeException("value");

                if (!BASSUtil.IsValidHandle(stream))
                {
                    BASSUtil.SetPositionInSeconds(channel, value.TotalSeconds);
                }
                else
                {
                    var position = BASSNative.ChannelSeconds2Bytes(channel, value.TotalSeconds);
                    if (!BASSUtil.IsValidValue(position))
                        throw new BASSException();

                    sampleDataPosition = (int)position;
                }
            }
        }

        /// <inheritdoc/>
        public override TimeSpan Duration
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (!IsChannelValid())
                    return TimeSpan.Zero;

                var duration = BASSUtil.GetDurationInSeconds(channel);
                return TimeSpan.FromSeconds(duration);
            }
        }

        /// <inheritdoc/>
        public override Single Volume
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (!IsChannelValid())
                    return 1f;

                return BASSUtil.GetVolume(channel);
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                EnsureChannelIsValid();

                BASSUtil.SetVolume(channel, MathUtil.Clamp(value, 0f, 1f));
            }
        }

        /// <inheritdoc/>
        public override Single Pitch
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (!IsChannelValid() || !promoted)
                    return 0f;

                return BASSUtil.GetPitch(channel);
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                EnsureChannelIsValid();

                var pitch = MathUtil.Clamp(value, -1f, 1f);
                if (!PromoteToStream(pitch))
                {
                    BASSUtil.SetPitch(channel, pitch);
                }
            }
        }

        /// <inheritdoc/>
        public override Single Pan
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (!IsChannelValid())
                    return 0f;

                return BASSUtil.GetPan(channel);
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                EnsureChannelIsValid();

                BASSUtil.SetPitch(channel, MathUtil.Clamp(value, -1f, 1f));
            }
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Ultraviolet != null && !Ultraviolet.Disposed)
                Release();

            base.Dispose(disposing);
        }

        /// <summary>
        /// Releases any BASS resources being held by the player.
        /// </summary>
        private void Release()
        {
            if (stream != 0)
            {
                if (!BASSNative.StreamFree(stream))
                    throw new BASSException();

                stream = 0;
            }

            if (sample != 0)
            {
                if (!BASSNative.SampleFree(sample))
                    throw new BASSException();

                sample = 0;
            }

            channel = 0;
            promoted = false;
            playing = null;
        }

        /// <summary>
        /// Throws an <see cref="System.InvalidOperationException"/> if the channel is not in a valid state.
        /// </summary>
        private void EnsureChannelIsValid()
        {
            if (State == PlaybackState.Stopped)
            {
                throw new InvalidOperationException(BASSStrings.NotCurrentlyValid);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the channel is in a valid state.
        /// </summary>
        /// <returns>true if the channel is in a valid state; otherwise, false.</returns>
        private Boolean IsChannelValid()
        {
            if (playing != null && playing.Disposed)
            {
                Release();
            }
            return State != PlaybackState.Stopped;
        }

        /// <summary>
        /// Plays a sound effect.
        /// </summary>
        private Boolean PlayInternal(SoundEffect soundEffect, Single volume, Single pitch, Single pan, Boolean loop = false)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            // Stop any sound that's already playing.
            Stop();

            // Retrieve the sample data from the sound effect.
            Ultraviolet.ValidateResource(soundEffect);
            var bassfx = (BASSSoundEffect)soundEffect;
            var sample = bassfx.GetSampleData(out this.sampleData, out this.sampleInfo);

            // Get a channel on which to play the sample.
            channel = BASSNative.SampleGetChannel(sample, true);
            if (!BASSUtil.IsValidHandle(channel))
            {
                var error = BASSNative.ErrorGetCode();
                if (error == BASSNative.BASS_ERROR_NOCHAN)
                    return false;

                throw new BASSException(error);
            }

            // Set the channel's attributes.
            if (pitch == 0)
            {
                BASSUtil.SetIsLooping(channel, loop);
                BASSUtil.SetVolume(channel, MathUtil.Clamp(volume, 0f, 1f));
                BASSUtil.SetPan(channel, MathUtil.Clamp(pan, -1f, 1f));
            }
            else
            {
                PromoteToStream(volume, MathUtil.Clamp(pitch, -1f, 1f), pan, loop);
            }

            // Play the channel.
            if (!BASSNative.ChannelPlay(channel, true))
                throw new BASSException();

            this.playing = soundEffect;

            return true;
        }
        
        /// <summary>
        /// Promotes the current channel to a stream.  
        /// This is necessary if the pitch is shifted, because BASS_FX only works on streams.
        /// </summary>
        private Boolean PromoteToStream(Single pitch)
        {
            return PromoteToStream(Volume, pitch, Pan, IsLooping);
        }

        /// <summary>
        /// Promotes the current channel to a stream.  
        /// This is necessary if the pitch is shifted, because BASS_FX only works on streams.
        /// </summary>
        /// <param name="volume">The stream's initial volume.</param>
        /// <param name="pitch">The stream's initial pitch.</param>
        /// <param name="pan">The stream's initial pan.</param>
        /// <param name="loop">A value indicating whether to loop the stream.</param>
        private Boolean PromoteToStream(Single volume, Single pitch, Single pan, Boolean loop)
        {
            if (BASSUtil.IsValidHandle(stream))
                return false;

            // If the channel is currently playing, pause it.
            var playing = (State == PlaybackState.Playing);
            if (playing)
            {
                if (!BASSNative.ChannelPause(channel))
                    throw new BASSException();
            }

            // Get the current position of the playing channel so that we can advance the stream to match.
            var streampos = (uint)BASSNative.ChannelGetPosition(channel, 0);
            if (!BASSUtil.IsValidValue(streampos))
                throw new BASSException();

            // Create a process for streaming data from our sample into the new stream.
            sampleDataPosition = (int)streampos;
            sampleDataLength = (int)sampleInfo.length;
            sampleDataStreamProc = new StreamProc((handle, buffer, length, user) =>
            {
                if (sampleDataPosition >= sampleDataLength)
                    sampleDataPosition = 0;

                var sfx = (BASSSoundEffect)this.playing;
                if (sfx == null || sfx.Disposed)
                    return BASSNative.BASS_STREAMPROC_END;

                var byteCount = Math.Min(length, (uint)sampleDataLength - streampos);
                unsafe
                {
                    byte* pBufferSrc = (byte*)(sampleData + sampleDataPosition).ToPointer();
                    byte* pBufferDst = (byte*)(buffer).ToPointer();
                    for (int i = 0; i < byteCount; i++)
                    {
                        *pBufferDst++ = *pBufferSrc++;
                    }
                }
                sampleDataPosition += (int)byteCount;

                return streampos >= sampleDataLength ?
                    byteCount | BASSNative.BASS_STREAMPROC_END :
                    byteCount;
            });

            // Create a decoding stream based on our sample channel.
            stream = BASSNative.StreamCreate(sampleInfo.freq, sampleInfo.chans, sampleInfo.flags | BASSNative.BASS_STREAM_DECODE, sampleDataStreamProc, IntPtr.Zero);
            if (!BASSUtil.IsValidHandle(stream))
                throw new BASSException();

            // Create an FX stream to shift the sound effect's pitch.
            stream = BASSFXNative.TempoCreate(stream, BASSNative.BASS_FX_FREESOURCE);
            if (!BASSUtil.IsValidHandle(stream))
                throw new BASSException();

            // Set the new stream's attributes.
            BASSUtil.SetVolume(stream, volume);
            BASSUtil.SetPitch(stream, pitch);
            BASSUtil.SetPan(stream, pan);
            BASSUtil.SetIsLooping(stream, loop);

            // Stop the old channel and switch to the stream.
            if (!BASSNative.ChannelStop(channel))
                throw new BASSException();
            channel = stream;

            // If we were previously playing, play the stream.
            if (playing)
            {
                if (!BASSNative.ChannelPlay(channel, false))
                    throw new BASSException();
            }

            promoted = true;
            return true;
        }

        // The currently-playing BASS resources.
        private Boolean promoted;
        private UInt32 sample;
        private UInt32 stream;
        private UInt32 channel;
        private SoundEffect playing;

        // The data source for promoted streams.
        private BASS_SAMPLE sampleInfo;
        private IntPtr sampleData;
        private Int32 sampleDataLength;
        private Int32 sampleDataPosition;
        private StreamProc sampleDataStreamProc;
    }
}
