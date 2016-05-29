using System;
using Newtonsoft.Json;

namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// An intermediate representation of the metadata for a <see cref="TextureAtlas"/> used during content processing.
    /// </summary>
    internal class TextureAtlasMetadataDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextureAtlasMetadataDescription"/> class.
        /// </summary>
        public TextureAtlasMetadataDescription()
        {
            this.RootDirectory = String.Empty;
            this.RequirePowerOfTwo = true;
            this.RequireSquare = false;
            this.MaximumWidth = 4096;
            this.MaximumHeight = 4096;
            this.Padding = 1;
        }

        /// <summary>
        /// Gets or sets the texture atlas' root directory.
        /// </summary>
        [JsonProperty(PropertyName = "rootDirectory", Required = Required.DisallowNull)]
        public String RootDirectory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the atlas' texture must had dimensions which are a power of two.
        /// </summary>
        [JsonProperty(PropertyName = "requirePowerOfTwo", Required = Required.DisallowNull)]
        public Boolean RequirePowerOfTwo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the atlas' texture must be square.
        /// </summary>
        [JsonProperty(PropertyName = "requireSquare", Required = Required.DisallowNull)]
        public Boolean RequireSquare { get; set; }

        /// <summary>
        /// Gets or sets the atlas' maximum width in pixels.
        /// </summary>
        [JsonProperty(PropertyName = "width", Required = Required.DisallowNull)]
        public Int32 MaximumWidth { get; set; }

        /// <summary>
        /// Gets or sets the atlas' maximum height in pixels.
        /// </summary>
        [JsonProperty(PropertyName = "height", Required = Required.DisallowNull)]
        public Int32 MaximumHeight { get; set; }

        /// <summary>
        /// Gets or sets the amount of padding between atlas cells in pixels.
        /// </summary>
        [JsonProperty(PropertyName = "padding", Required = Required.DisallowNull)]
        public Int32 Padding { get; set; }
    }
}
