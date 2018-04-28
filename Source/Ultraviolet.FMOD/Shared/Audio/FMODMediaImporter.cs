using System;
using System.IO;
using Ultraviolet.Content;

namespace Ultraviolet.FMOD.Audio
{
    /// <summary>
    /// Imports .mp3, .ogg, and .wav files.
    /// </summary>
    [ContentImporter(".aif")]
    [ContentImporter(".aiff")]
    [ContentImporter(".flac")]
    [ContentImporter(".it")]
    [ContentImporter(".m3u")]
    [ContentImporter(".mid")]
    [ContentImporter(".mod")]
    [ContentImporter(".mp2")]
    [ContentImporter(".mp3")]
    [ContentImporter(".ogg")]
    [ContentImporter(".s3m")]
    [ContentImporter(".wav")]
    public sealed class FMODMediaImporter : ContentImporter<String>
    {
        /// <inheritdoc/>
        public override String Import(IContentImporterMetadata metadata, Stream stream)
        {
            return metadata.AssetFilePath;
        }
    }
}
