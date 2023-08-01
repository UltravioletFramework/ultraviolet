using System;
using Newtonsoft.Json;

namespace Ultraviolet.Graphics
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
        [JsonProperty(Required = Required.DisallowNull)]
        public String RootDirectory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the atlas' texture must had dimensions which are a power of two.
        /// </summary>
        [JsonProperty(Required = Required.DisallowNull)]
        public Boolean RequirePowerOfTwo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the atlas' texture must be square.
        /// </summary>
        [JsonProperty(Required = Required.DisallowNull)]
        public Boolean RequireSquare { get; set; }

        /// <summary>
        /// Gets or sets the atlas' maximum width in pixels.
        /// </summary>
        [JsonProperty(Required = Required.DisallowNull)]
        public Int32 MaximumWidth { get; set; }

        /// <summary>
        /// Gets or sets the atlas' maximum height in pixels.
        /// </summary>
        [JsonProperty(Required = Required.DisallowNull)]
        public Int32 MaximumHeight { get; set; }

        /// <summary>
        /// Gets or sets the amount of padding between atlas cells in pixels.
        /// </summary>
        [JsonProperty(Required = Required.DisallowNull)]
        public Int32 Padding { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the cell names should be flattened.
        /// </summary>
        [JsonProperty(Required = Required.DisallowNull)]
        public Boolean FlattenCellName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the texture is SRGB encoded. If no value is provided,
        /// the default specified by the <see cref="UltravioletContextProperties.SrgbDefaultForTexture2D"/>
        /// property is used.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public Boolean? SrgbEncoded { get; set; }
    }
}
