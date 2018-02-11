using System;
using System.IO;
using Ultraviolet.Content;
using Ultraviolet.Core;

namespace Ultraviolet.FMOD.Audio
{
    /// <summary>
    /// Imports .mp3, .ogg, and .wav files.
    /// </summary>
    [Preserve(AllMembers = true)]
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
        /// <summary>
        /// Imports the data from the specified file.
        /// </summary>
        /// <param name="metadata">The asset metadata for the asset to import.</param>
        /// <param name="stream">The stream that contains the data to import.</param>
        /// <returns>The data structure that was imported from the file.</returns>
        public override String Import(IContentImporterMetadata metadata, Stream stream)
        {
            return metadata.AssetFilePath;
        }
    }
}
