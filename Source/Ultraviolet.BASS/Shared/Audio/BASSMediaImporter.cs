using System;
using System.IO;
using Ultraviolet.Content;
using Ultraviolet.Core;

namespace Ultraviolet.BASS.Audio
{
    /// <summary>
    /// Imports .mp3, .ogg, and .wav files.
    /// </summary>
    [ContentImporter(".mp3")]
    [ContentImporter(".ogg")]
    [ContentImporter(".wav")]
    public sealed class BASSMediaImporter : ContentImporter<String>
    {
        /// <inheritdoc/>
        public override String Import(IContentImporterMetadata metadata, Stream stream)
        {
            return metadata.AssetFilePath;
        }
    }
}
