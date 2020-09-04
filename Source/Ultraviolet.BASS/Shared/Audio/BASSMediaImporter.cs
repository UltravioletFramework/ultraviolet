using System;
using System.IO;
using Ultraviolet.Content;

namespace Ultraviolet.BASS.Audio
{
    /// <summary>
    /// Imports .mp3, .ogg, and .wav files.
    /// </summary>
    [ContentImporter(".mp3")]
    [ContentImporter(".ogg")]
    [ContentImporter(".wav")]
    public sealed class BASSMediaImporter : ContentImporter<BASSMediaDescription>
    {
        /// <inheritdoc/>
        public override BASSMediaDescription Import(IContentImporterMetadata metadata, Stream stream)
        {
            if (metadata.IsFile)
                return new BASSMediaDescription(metadata.AssetFilePath);

            var buffer = new Byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            return new BASSMediaDescription(buffer);
        }
    }
}
