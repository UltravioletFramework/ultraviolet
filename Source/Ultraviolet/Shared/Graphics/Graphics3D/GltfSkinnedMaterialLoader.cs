using System;
using SharpGLTF.Schema2;
using Ultraviolet.Content;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents an implementation of the <see cref="GltfMaterialLoader"/> class which creates instances of <see cref="SkinnedMaterial"/>.
    /// </summary>
    [CLSCompliant(false)]
    public class GltfSkinnedMaterialLoader : GltfBasicMaterialLoader
    {
        /// <inheritdoc/>
        public override Material CreateMaterialForPrimitive(ContentManager contentManager, MeshPrimitive primitive)
        {
            Contract.Require(contentManager, nameof(contentManager));
            Contract.Require(primitive, nameof(primitive));

            return new SkinnedMaterial
            {
                Alpha = GetMaterialAlpha(primitive.Material),
                DiffuseColor = GetMaterialDiffuseColor(primitive.Material),
                SpecularPower = GetMaterialSpecularPower(primitive.Material),
                SpecularColor = GetMaterialSpecularColor(primitive.Material),
                EmissiveColor = GetMaterialEmissiveColor(primitive.Material),
                Texture = GetMaterialTexture(contentManager, primitive.Material) ?? GetBlankTexture(),
            };
        }
    }
}
