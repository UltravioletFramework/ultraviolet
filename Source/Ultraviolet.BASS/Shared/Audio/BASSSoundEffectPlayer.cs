using System;
using Ultraviolet.Audio;
using Ultraviolet.BASS.Messages;
using Ultraviolet.BASS.Native;
using Ultraviolet.Core;
using Ultraviolet.Core.Messages;
using static Ultraviolet.BASS.Native.BASSFXNative;
using static Ultraviolet.BASS.Native.BASSNative;

namespace Ultraviolet.BASS.Audio
{
    /// <summary>
    /// Represents the BASS implementation of the <see cref="SoundEffectPlayer"/> class.
    /// </summary>
    public sealed class BASSSoundEffectPlayer : SoundEffectPlayer,
        IMessageSubscriber<UltravioletMessageID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BASSSoundEffectPlayer"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public BASSSoundEffectPlayer(UltravioletContext uv)
            : base(uv)
        {
            uv.Messages.Subscribe(this, BASSUltravioletMessages.BASSDeviceChanged);
        }

        /// <inheritdoc/>
        void IMessageSubscriber<UltravioletMessageID>.ReceiveMessage(UltravioletMessageID type, MessageData data)
        {
            if (type == BASSUltravioletMessages.BASSDeviceChanged)
            {
                StopInternal();

                if (BASSUtil.IsValidHandle(sample))
                {
                    var deviceID = ((BASSDeviceChangedMessageData)data).DeviceID;
                    if (!BASS_ChannelSetDevice(sample, deviceID))
                        throw new BASSException();
                }
                return;
            }
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

            if (BASSUtil.IsValidHandle(channel) && !BASS_ChannelStop(channel))
                throw new BASSException();

            StopInternal();
        }

        /// <inheritdoc/>
        public override void Pause()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            
            if (State == PlaybackState.Playing)
            {
                if (!BASS_ChannelPause(channel))
                    throw new BASSException();
            }
        }

        /// <inheritdoc/>
        public override void Resume()
        {
            Contract.EnsureNotDisposed(this, Disposed);
            
            if (State == PlaybackState.Paused)
            {
                if (!BASS_ChannelPlay(channel, false))
                    throw new BASSException();
            }
        }

        /// <inheritdoc/>
        public override void SlideVolume(Single volume, TimeSpan time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (State == PlaybackState.Stopped)
                throw new InvalidOperationException(BASSStrings.NotCurrentlyValid);

            BASSUtil.SlideVolume(channel, volume, time);
        }

        /// <inheritdoc/>
        public override void SlidePitch(Single pitch, TimeSpan time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (State == PlaybackState.Stopped)
                throw new InvalidOperationException(BASSStrings.NotCurrentlyValid);

            PromoteToStream(0f);
            BASSUtil.SlidePitch(channel, pitch, time);
        }

        /// <inheritdoc/>
        public override void SlidePan(Single pan, TimeSpan time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (State == PlaybackState.Stopped)
                throw new InvalidOperationException(BASSStrings.NotCurrentlyValid);

            BASSUtil.SlidePan(channel, pan, time);
        }

        /// <inheritdoc/>
        public override PlaybackState State
        {
            get
            {
                if (BASSUtil.IsValidHandle(channel))
                {
                    switch (BASS_ChannelIsActive(channel))
                    {
                        case BASS_ACTIVE_STALLED:
                            return PlaybackState.Playing;

                        case BASS_ACTIVE_PLAYING:
                            return PlaybackState.Playing;

                        case BASS_ACTIVE_STOPPED:
                            return PlaybackState.Stopped;

                        case BASS_ACTIVE_PAUSED:
                            return PlaybackState.Stopped;
                    }
                }
                return PlaybackState.Stopped;
            }
        }

        /// <inheritdoc/>
        public override Boolean IsPlaying
        {
            get => State == PlaybackState.Playing;
        }

        /// <inheritdoc/>
        public override Boolean IsLooping
        {
            get => IsHandleValid() ? BASSUtil.GetIsLooping(channel) : false;
            set
            {
                if (State == PlaybackState.Stopped)
                    throw new InvalidOperationException(BASSStrings.NotCurrentlyValid);

                BASSUtil.SetIsLooping(channel, value);
            }
        }

        /// <inheritdoc/>
        public override TimeSpan Position
        {
            get
            {
                if (!IsHandleValid())
                    return TimeSpan.Zero;

                if (!BASSUtil.IsValidHandle(stream))
                {
                    var position = BASSUtil.GetPositionInSeconds(channel);
                    return TimeSpan.FromSeconds(position);
                }
                else
                {
                    var position = BASS_ChannelBytes2Seconds(channel, (ulong)sampleDataPosition);
                    if (position < 0)
                        throw new BASSException();

                    return TimeSpan.FromSeconds(position);
                }
            }
            set
            {
                if (State == PlaybackState.Stopped)
                    throw new InvalidOperationException(BASSStrings.NotCurrentlyValid);

                if (value.TotalSeconds < 0 || value > Duration)
                    throw new ArgumentOutOfRangeException(nameof(value));

                if (BASSUtil.IsValidHandle(stream))
                {
                    var position = BASS_ChannelSeconds2Bytes(channel, value.TotalSeconds);
                    if (!BASSUtil.IsValidValue(position))
                        throw new BASSException();

                    sampleDataPosition = (int)position;
                }
                else
                {
                    BASSUtil.SetPositionInSeconds(channel, value.TotalSeconds);
                }
            }
        }

        /// <inheritdoc/>
        public override TimeSpan Duration
        {
            get => IsHandleValid() ? BASSUtil.GetDurationAsTimeSpan(channel) : TimeSpan.Zero;
        }

        /// <inheritdoc/>
        public override Single Volume
        {
            get => IsHandleValid() ? BASSUtil.GetVolume(channel) : 1f;
            set
            {
                if (State == PlaybackState.Stopped)
                    throw new InvalidOperationException(BASSStrings.NotCurrentlyValid);

                BASSUtil.SetVolume(channel, MathUtil.Clamp(value, 0f, 1f));
            }
        }

        /// <inheritdoc/>
        public override Single Pitch
        {
            get
            {
                if (!IsHandleValid() || !promoted)
                    return 0f;

                return BASSUtil.GetPitch(channel);
            }
            set
            {
                if (State == PlaybackState.Stopped)
                    throw new InvalidOperationException(BASSStrings.NotCurrentlyValid); ;

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
            get => IsHandleValid() ? BASSUtil.GetPan(channel) : 0f;
            set
            {
                if (State == PlaybackState.Stopped)
                    throw new InvalidOperationException(BASSStrings.NotCurrentlyValid);

                BASSUtil.SetPitch(channel, MathUtil.Clamp(value, -1f, 1f));
            }
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Ultraviolet != null && !Ultraviolet.Disposed)
                StopInternal();

            base.Dispose(disposing);
        }
        
        /// <summary>
        /// Gets a value indicating whether the channel is in a valid state.
        /// </summary>
        /// <returns>true if the channel is in a valid state; otherwise, false.</returns>
        private Boolean IsHandleValid()
        {
            if (playing != null && playing.Disposed)
            {
                StopInternal();
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
            var sample = bassfx.GetSampleInfo(out this.sampleData, out this.sampleInfo);

            // Get a channel on which to play the sample.
            channel = BASS_SampleGetChannel(sample, true);
            if (!BASSUtil.IsValidHandle(channel))
            {
                var error = BASS_ErrorGetCode();
                if (error == BASS_ERROR_NOCHAN)
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
            if (!BASS_ChannelPlay(channel, true))
                throw new BASSException();

            this.playing = soundEffect;

            return true;
        }

        /// <summary>
        /// Releases any BASS resources being held by the player.
        /// </summary>
        private Boolean StopInternal()
        {
            if (stream != 0)
            {
                if (!BASS_StreamFree(stream))
                    throw new BASSException();

                stream = 0;
            }

            if (sample != 0)
            {
                if (!BASS_SampleFree(sample))
                    throw new BASSException();

                sample = 0;
            }

            channel = 0;
            promoted = false;
            playing = null;

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
                if (!BASS_ChannelPause(channel))
                    throw new BASSException();
            }

            // Get the current position of the playing channel so that we can advance the stream to match.
            var streampos = (uint)BASS_ChannelGetPosition(channel, 0);
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
            stream = BASS_StreamCreate(sampleInfo.freq, sampleInfo.chans, sampleInfo.flags | BASS_STREAM_DECODE, sampleDataStreamProc, IntPtr.Zero);
            if (!BASSUtil.IsValidHandle(stream))
                throw new BASSException();

            // Create an FX stream to shift the sound effect's pitch.
            stream = BASS_FX_TempoCreate(stream, BASS_FX_FREESOURCE);
            if (!BASSUtil.IsValidHandle(stream))
                throw new BASSException();

            // Set the new stream's attributes.
            BASSUtil.SetVolume(stream, volume);
            BASSUtil.SetPitch(stream, pitch);
            BASSUtil.SetPan(stream, pan);
            BASSUtil.SetIsLooping(stream, loop);

            // Stop the old channel and switch to the stream.
            if (!BASS_ChannelStop(channel))
                throw new BASSException();
            channel = stream;

            // If we were previously playing, play the stream.
            if (playing)
            {
                if (!BASS_ChannelPlay(channel, false))
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
