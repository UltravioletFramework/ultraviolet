using System;
using System.Collections.Generic;
using SharpGLTF.Schema2;
using Ultraviolet.Content;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a content processor which converts <see cref="ModelRoot"/> instances to <see cref="SkinnedModel"/> instances.
    /// </summary>
    [ContentProcessor, CLSCompliant(false)]
    public class GltfSkinnedModelProcessor : GltfModelProcessor<SkinnedModel>
    {
        /// <inheritdoc/>
        protected override SkinnedModel CreateModelResource(ContentManager contentManager, ModelRoot input, IList<ModelScene> scenes, IList<Texture2D> textures)
        {
            return new SkinnedModel(contentManager.Ultraviolet, scenes, textures);
        }
    }
}
