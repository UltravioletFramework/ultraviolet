using System;
using System.Collections.Generic;
using System.Linq;
using SharpGLTF.Schema2;
using Ultraviolet.Content;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents an implementation of the <see cref="GltfMaterialLoader"/> class which creates instances of <see cref="BasicMaterial"/>.
    /// </summary>
    [CLSCompliant(false)]
    public class GltfBasicMaterialLoader : GltfMaterialLoader
    {
        /// <inheritdoc/>
        public override Material CreateMaterialForPrimitive(ContentManager contentManager, MeshPrimitive primitive)
        {
            Contract.Require(contentManager, nameof(contentManager));
            Contract.Require(primitive, nameof(primitive));

            return new BasicMaterial
            {
                Alpha = GetMaterialAlpha(primitive.Material),
                DiffuseColor = GetMaterialDiffuseColor(primitive.Material),
                SpecularPower = GetMaterialSpecularPower(primitive.Material),
                SpecularColor = GetMaterialSpecularColor(primitive.Material),
                EmissiveColor = GetMaterialEmissiveColor(primitive.Material),
                Texture = GetMaterialTexture(contentManager, primitive.Material)
            };
        }

        /// <inheritdoc/>
        public override IList<Texture2D> GetModelTextures() => textureCache.Values.ToList();

        /// <summary>
        /// Gets the alpha for the specified material.
        /// </summary>
        protected Single GetMaterialAlpha(SharpGLTF.Schema2.Material material)
        {
            if (material == null)
                return 1f;

            if (material.Alpha == AlphaMode.OPAQUE)
                return 1f;

            var baseColor = material.FindChannel("BaseColor");
            if (baseColor == null)
                return 1f;

            return baseColor.Value.Parameter.W;
        }

        /// <summary>
        /// Gets the specular power for the specified material.
        /// </summary>
        protected Single GetMaterialSpecularPower(SharpGLTF.Schema2.Material material)
        {
            if (material == null)
                return 16f;

            var mr = material.FindChannel("MetallicRoughness");
            if (mr == null)
                return 16f;

            var metallic = mr.Value.Parameter.X;
            return 4f + 16f * metallic;
        }

        /// <summary>
        /// Gets the diffuse color for the specified material.
        /// </summary>
        protected Color GetMaterialDiffuseColor(SharpGLTF.Schema2.Material material)
        {
            if (material == null)
                return Color.White;

            var diffuse = material.FindChannel("Diffuse") ?? material.FindChannel("BaseColor");
            if (diffuse == null)
                return Color.White;

            return new Color(diffuse.Value.Parameter.X, diffuse.Value.Parameter.Y, diffuse.Value.Parameter.Z);
        }

        /// <summary>
        /// Gets the emissive color for the specified material.
        /// </summary>
        protected Color GetMaterialEmissiveColor(SharpGLTF.Schema2.Material material)
        {
            if (material == null)
                return Color.Black;

            var emissive = material.FindChannel("Emissive");
            if (emissive == null)
                return Color.Black;

            return new Color(emissive.Value.Parameter.X, emissive.Value.Parameter.Y, emissive.Value.Parameter.Z);
        }

        /// <summary>
        /// Gets the specular color for the specified material.
        /// </summary>
        protected Color GetMaterialSpecularColor(SharpGLTF.Schema2.Material material)
        {
            if (material == null)
                return Color.White;

            var mr = material.FindChannel("MetallicRoughness");
            if (mr == null)
                return Color.White;

            var diffuse = GetMaterialDiffuseColor(material).ToVector3();
            var metallic = mr.Value.Parameter.X;
            var roughness = mr.Value.Parameter.Y;

            var k = Vector3.Zero;
            k += Vector3.Lerp(diffuse, Vector3.Zero, roughness);
            k += Vector3.Lerp(diffuse, Vector3.One, metallic);
            k *= 0.5f;

            return new Color(k);
        }

        /// <summary>
        /// Gets the texture for the specified material.
        /// </summary>
        protected Texture2D GetMaterialTexture(ContentManager contentManager, SharpGLTF.Schema2.Material material)
        {
            if (material == null)
                return null;

            var diffuse = material.FindChannel("Diffuse") ?? material.FindChannel("BaseColor");
            if (diffuse == null)
                return null;

            var texture = diffuse.Value.Texture;
            if (texture != null)
            {
                var textureName = $"{diffuse.Value.LogicalParent.Name ?? "null"}-{diffuse.Value.Key}";
                var textureContent = texture.PrimaryImage?.Content;
                if (textureContent != null && textureContent.Value.IsValid)
                {
                    if (!textureCache.TryGetValue(textureName, out var textureResource))
                    {
                        using (var textureStream = textureContent.Value.Open())
                        {
                            textureResource = contentManager.LoadFromStream<Texture2D>(textureStream, textureContent.Value.FileExtension);
                        }
                        textureCache[textureName] = textureResource;
                    }
                    return textureResource;
                }
            }

            return null;
        }

        /// <summary>
        /// Creates a new blank texture.
        /// </summary>
        protected Texture2D GetBlankTexture()
        {
            if (textureCache.TryGetValue(String.Empty, out var blank))
                return blank;

            var texture = Texture2D.CreateTexture(1, 1, TextureOptions.Default);
            texture.SetData(new[] { Color.White });
            textureCache[String.Empty] = texture;

            return texture;
        }

        // A cache which contains all of the textures used by the model.
        private readonly Dictionary<String, Texture2D> textureCache = new Dictionary<String, Texture2D>();
    }
}
