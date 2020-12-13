using System;
using System.Runtime.InteropServices;
using Ultraviolet.Audio;
using Ultraviolet.BASS.Native;
using Ultraviolet.Core;
using Ultraviolet.Platform;
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
            Contract.Require(filename, nameof(filename));

            var fileSystemService = FileSystemService.Create();
            var fileData = default(Byte[]);

            using (var stream = fileSystemService.OpenRead(filename))
            {
                fileData = new Byte[stream.Length];
                stream.Read(fileData, 0, fileData.Length);
            }

            InitializeSampleData(fileData, out this.sample, out this.sampleInfo, out this.sampleData);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BASSSoundEffect"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="fileData">An array containing the sample data to load.</param>
        public BASSSoundEffect(UltravioletContext uv, Byte[] fileData)
            : base(uv)
        {
            Contract.Require(fileData, nameof(fileData));

            InitializeSampleData(fileData, out this.sample, out this.sampleInfo, out this.sampleData);
        }
        
        /// <summary>
        /// Gets the sound effect's sample information.
        /// </summary>
        /// <param name="data">The sound effect's raw PCM sample data.</param>
        /// <param name="info">The sound effect's sample info.</param>
        /// <returns>The handle to the sound effect's BASS sample.</returns>
        public UInt32 GetSampleInfo(out IntPtr data, out BASS_SAMPLE info)
        {
            data = this.sampleData;
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

            var channel = BASS_SampleGetChannel(sample, false);
            if (!BASSUtil.IsValidHandle(channel))
                throw new BASSException();

            BASSUtil.SetVolume(channel, MathUtil.Clamp(volume, 0f, 1f));
            BASSUtil.SetPan(channel, MathUtil.Clamp(pan, -1f, 1f));

            if (!BASS_ChannelPlay(channel, false))
                throw new BASSException();
        }

        /// <inheritdoc/>
        public override TimeSpan Duration => BASSUtil.IsValidHandle(sample) ? BASSUtil.GetDurationAsTimeSpan(sample) : TimeSpan.Zero;

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (!BASS_SampleFree(sample))
                throw new BASSException();

            if (this.sampleData != IntPtr.Zero)
                Marshal.FreeHGlobal(this.sampleData);

            this.sample = 0;
            this.sampleData = IntPtr.Zero;

            base.Dispose(disposing);
        }

        /// <summary>
        /// Initializes a <see cref="BASSSoundEffect"/> instance from the specified data.
        /// </summary>
        private static void InitializeSampleData(Byte[] fileData, out UInt32 sample, out BASS_SAMPLE sampleInfo, out IntPtr sampleData)
        {
            sample = BASS_SampleLoad(fileData, 0, (UInt32)fileData.Length, UInt16.MaxValue, 0);
            if (!BASSUtil.IsValidHandle(sample))
                throw new BASSException();

            if (!BASS_SampleGetInfo(sample, out sampleInfo))
                throw new BASSException();

            sampleData = Marshal.AllocHGlobal((Int32)sampleInfo.length);
            if (!BASS_SampleGetData(sample, sampleData))
                throw new BASSException();
        }

        // The sound effect's sample data.
        private IntPtr sampleData;
        private UInt32 sample;
        private BASS_SAMPLE sampleInfo;
    }
}