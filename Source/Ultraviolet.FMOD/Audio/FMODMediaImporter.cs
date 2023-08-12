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
    public sealed class FMODMediaImporter : ContentImporter<FMODMediaDescription>
    {
        /// <summary>
        /// An array of file extensions supported by this importer 
        /// </summary>
        public static String[] SupportedExtensions { get; } = new string[] { ".aif", ".aiff", ".flac", ".it", ".m3u", ".mid", ".mod", ".mp2", ".mp3", ".ogg", ".s3m", ".wav" };

        /// <inheritdoc/>
        public override FMODMediaDescription Import(IContentImporterMetadata metadata, Stream stream)
        {
            if (metadata.IsFile)
                return new FMODMediaDescription(metadata.AssetFilePath);

            var buffer = new Byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            return new FMODMediaDescription(buffer);
        }
    }
}
