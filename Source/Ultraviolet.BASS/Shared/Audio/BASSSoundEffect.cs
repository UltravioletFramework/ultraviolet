using System;
using System.Runtime.InteropServices;
using Ultraviolet.Audio;
using Ultraviolet.BASS.Native;
using Ultraviolet.Core;
using Ultraviolet.Platform;
using static Ultraviolet.BASS.Native.BASSFXNative;
using static Ultraviolet.BASS.Native.BASSNative;

namespace Ultraviolet.BASS.Audio
{
    /// <summary>
    /// Represents the BASS implementation of the <see cref="SoundEffect"/> class.
    /// </summary>
    public sealed class BASSSoundEffect : SoundEffect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BASSSoundEffect"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="filename">The filename of the sample to load.</param>
        public BASSSoundEffect(UltravioletContext uv, String filename)
            : base(uv)
        {
            var fileSystemService = FileSystemService.Create();
            var fileData = default(Byte[]);

            using (var stream = fileSystemService.OpenRead(filename))
            {
                fileData = new Byte[stream.Length];
                stream.Read(fileData, 0, fileData.Length);
            }

            sample = BASS_SampleLoad(fileData, 0, (UInt32)fileData.Length, UInt16.MaxValue, 0);
            if (!BASSUtil.IsValidHandle(sample))
                throw new BASSException();

            if (!BASS_SampleGetInfo(sample, out this.sampleInfo))
                throw new BASSException();

            this.data = Marshal.AllocHGlobal((int)sampleInfo.length);
            if (!BASS_SampleGetData(sample, this.data))
                throw new BASSException();
        }
        
        /// <summary>
        /// Gets the sound effect's sample information.
        /// </summary>
        /// <param name="data">The sound effect's raw PCM sample data.</param>
        /// <param name="info">The sound effect's sample info.</param>
        /// <returns>The handle to the sound effect's BASS sample.</returns>
        public UInt32 GetSampleInfo(out IntPtr data, out BASS_SAMPLE info)
        {
            data = this.data;
            info = this.sampleInfo;
            return sample;
        }

        /// <inheritdoc/>
        public override void Play()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var channel = BASS_SampleGetChannel(sample, false);
            if (!BASSUtil.IsValidHandle(channel))
                throw new BASSException();

            if (!BASS_ChannelPlay(channel, true))
                throw new BASSException();
        }

        /// <inheritdoc/>
        public override void Play(Single volume, Single pitch, Single pan)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var channel = 0u;

            if (pitch == 0)
            {
                channel = BASS_SampleGetChannel(sample, false);
                if (!BASSUtil.IsValidHandle(channel))
                    throw new BASSException();
            }
            else
            {
                var stream = BASS_StreamCreate(sampleInfo.freq, sampleInfo.chans, sampleInfo.flags | BASS_STREAM_DECODE, STREAMPROC_PUSH, IntPtr.Zero);
                if (!BASSUtil.IsValidHandle(stream))
                    throw new BASSException();

                var pushed = BASS_StreamPutData(stream, data, sampleInfo.length);
                if (!BASSUtil.IsValidValue(pushed))
                    throw new BASSException();

                stream = BASS_FX_TempoCreate(stream, BASS_FX_FREESOURCE | BASS_STREAM_AUTOFREE);
                if (!BASSUtil.IsValidHandle(stream))
                    throw new BASSException();

                channel = stream;

                BASSUtil.SetPitch(channel, MathUtil.Clamp(pitch, -1f, 1f));
            }

            BASSUtil.SetVolume(channel, MathUtil.Clamp(volume, 0f, 1f));
            BASSUtil.SetPan(channel, MathUtil.Clamp(pan, -1f, 1f));

            if (!BASS_ChannelPlay(channel, false))
                throw new BASSException();
        }

        /// <inheritdoc/>
        public override TimeSpan Duration
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                var duration = BASSUtil.GetDurationInSeconds(sample);
                return TimeSpan.FromSeconds(duration);
            }
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (!BASS_SampleFree(sample))
                throw new BASSException();

            if (this.data != IntPtr.Zero)
                Marshal.FreeHGlobal(this.data);

            this.sample = 0;
            this.data = IntPtr.Zero;

            base.Dispose(disposing);
        }

        // The sound effect's sample data.
        private IntPtr data;
        private UInt32 sample;
        private BASS_SAMPLE sampleInfo;
    }
}