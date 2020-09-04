using System;

namespace Ultraviolet.BASS.Audio
{
    /// <summary>
    /// Represents the intermediate description of a BASS media resource.
    /// </summary>
    public sealed class BASSMediaDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BASSMediaDescription"/> class from the specified filename.
        /// </summary>
        /// <param name="filename">The media file's filename.</param>
        internal BASSMediaDescription(String filename)
        {
            this.IsFilename = true;
            this.Data = filename;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BASSMediaDescription"/> class from the specified byte array.
        /// </summary>
        /// <param name="data">The media file's data.</param>
        internal BASSMediaDescription(Byte[] data)
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
