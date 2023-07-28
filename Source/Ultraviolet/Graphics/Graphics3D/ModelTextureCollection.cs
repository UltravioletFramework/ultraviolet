using System;
using System.Collections.Generic;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a collection of <see cref="Texture2D"/> objects which are owned by a particular <see cref="Model"/> instance.
    /// </summary>
    public sealed class ModelTextureCollection : ModelResourceCollection<Texture2D>, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelTextureCollection"/> class.
        /// </summary>
        /// <param name="textures">The initial list of <see cref="Texture2D"/> objects with which to populate this collection.</param>
        public ModelTextureCollection(IList<Texture2D> textures = null)
            : base(textures)
        { }

        /// <inheritdoc/>
        public void Dispose()
        {
            GC.SuppressFinalize(this);

            for (var i = 0; i < Count; i++)
                this[i]?.Dispose();
        }
    }
}