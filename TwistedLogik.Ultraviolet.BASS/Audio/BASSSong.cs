using System;
using System.Collections.Generic;
using Ultraviolet.Audio;
using Ultraviolet.BASS.Native;
using Ultraviolet.Core;
using Ultraviolet.Platform;

namespace Ultraviolet.BASS.Audio
{
    /// <summary>
    /// Represents the BASS implementation of the Song class.
    /// </summary>
    public sealed partial class BASSSong : Song
    {
        /// <summary>
        /// Initializes a new instance of the BASSSong class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="file">The path to the file from which to stream the song.</param>
        public BASSSong(UltravioletContext uv, String file)
            : base(uv)
        {
            Contract.RequireNotEmpty(file, nameof(file));

            this.file = file;

            var stream = CreateStream(BASSNative.BASS_STREAM_DECODE);

            tags = new SongTagCollection();
            ReadTagsFromStream(stream);
            
            var duration = BASSUtil.GetDurationInSeconds(stream);

            if (!BASSNative.StreamFree(stream))
                throw new BASSException();

            this.duration = TimeSpan.FromSeconds(duration);
        }

        /// <inheritdoc/>
        public override SongTagCollection Tags
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return tags;
            }
        }

        /// <inheritdoc/>
        public override TimeSpan Duration
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return duration;
            }
        }

        /// <summary>
        /// Creates a BASS stream that represents the song.
        /// </summary>
        /// <param name="flags">The flags to apply to the stream that is created.</param>
        /// <returns>The handle to the BASS stream that was created.</returns>
        internal UInt32 CreateStream(UInt32 flags)
        {
            if (FileSystemService.Source == null)
            {
                var stream = BASSNative.StreamCreateFile(file, flags);
                if (!BASSUtil.IsValidHandle(stream))
                    throw new BASSException();

                return stream;
            }
            else
            {
                if (instanceManager == null)
                {
                    instanceManager = new BASSSongInstanceManager(file);
                }
                return instanceManager.CreateInstance(flags);
            }
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SafeDispose.DisposeRef(ref instanceManager);
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Reads any supported tags which are contained by the specified stream.
        /// </summary>
        private void ReadTagsFromStream(UInt32 stream)
        {
            var tagsOgg = default(IDictionary<String, String>);
            if (BASSNative.ChannelGetTags_Ogg(stream, out tagsOgg))
            {
                foreach (var tagOgg in tagsOgg)
                    tags.Add(tagOgg.Key, tagOgg.Value);
            }
        }

        // The file from which to stream the song.
        private readonly String file;
        private readonly SongTagCollection tags;
        private readonly TimeSpan duration;

        // The instance manager used when we can't read files directly from the file system using BASS.
        private BASSSongInstanceManager instanceManager;
    }
}
