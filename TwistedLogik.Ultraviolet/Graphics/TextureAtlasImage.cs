using System;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents an image to be included on a texture atlas.
    /// </summary>
    internal struct TextureAtlasImage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextureAtlasImage"/> structure.
        /// </summary>
        /// <param name="name">The name of the image.</param>
        /// <param name="path">The path to the image asset.</param>
        /// <param name="size">The size of the image.</param>
        public TextureAtlasImage(String name, String path, Size2 size)
        {
            this.name = name;
            this.path = path;
            this.size = size;
        }

        /// <summary>
        /// Gets the unique name of the image.
        /// </summary>
        public String Name { get { return name; } }

        /// <summary>
        /// Gets the path to the image asset.
        /// </summary>
        public String Path { get { return path; } }

        /// <summary>
        /// Gets the size of the image asset.
        /// </summary>
        public Size2 Size { get { return size; } }

        // Property values.
        private readonly String name;
        private readonly String path;
        private readonly Size2 size;
    }
}
