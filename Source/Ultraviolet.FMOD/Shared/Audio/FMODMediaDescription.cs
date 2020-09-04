using System;

namespace Ultraviolet.FMOD.Audio
{
    /// <summary>
    /// Represents the intermediate description of an FMOD media resource.
    /// </summary>
    public sealed class FMODMediaDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FMODMediaDescription"/> class from the specified filename.
        /// </summary>
        /// <param name="filename">The media file's filename.</param>
        internal FMODMediaDescription(String filename)
        {
            this.IsFilename = true;
            this.Data = filename;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FMODMediaDescription"/> class from the specified byte array.
        /// </summary>
        /// <param name="data">The media file's data.</param>
        internal FMODMediaDescription(Byte[] data)
        {
            this.IsFilename = false;
            this.Data = data;
        }

        /// <summary>
        /// Gets a value indicating whether this media description represents a filename.
        /// </summary>
        public Boolean IsFilename { get; }

        /// <summary>
        /// Gets a value indicating whether this media description represents raw data.
        /// </summary>
        public Boolean IsRawData => !IsFilename;

        /// <summary>
        /// Gets the media description's data.
        /// </summary>
        public Object Data { get; }
    }
}
