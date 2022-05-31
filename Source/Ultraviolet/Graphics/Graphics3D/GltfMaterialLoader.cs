using System;
using System.Collections.Generic;
using SharpGLTF.Schema2;
using Ultraviolet.Content;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents an object which creates new <see cref="Material"/> instances for a GLTF model which is being loaded.
    /// </summary>
    [CLSCompliant(false)]
    public abstract class GltfMaterialLoader : IDisposable
    {
        /// <inheritdoc/>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        /// <summary>
        /// Creates a new <see cref="Material"/> instance for the specified primitve.
        /// </summary>
        /// <param name="contentManager">The content manager with which the model is being loaded.</param>
        /// <param name="primitive">The <see cref="MeshPrimitive"/> for which to create a material.</param>
        /// <returns>The <see cref="Material"/> instance which was created.</returns>
        public abstract Material CreateMaterialForPrimitive(ContentManager contentManager, MeshPrimitive primitive);

        /// <summary>
        /// Gets the list of textures used by the model.
        /// </summary>
        /// <returns>The list of textures used by the model.</returns>
        public abstract IList<Texture2D> GetModelTextures();

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> if the object is being disposed; <see langword="false"/> if the object is being finalized.</param>
        protected virtual void Dispose(Boolean disposing) { }
    }
}
