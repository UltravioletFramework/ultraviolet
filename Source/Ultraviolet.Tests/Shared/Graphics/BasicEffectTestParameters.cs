using System;
using System.Text;
using Ultraviolet.Graphics;

namespace Ultraviolet.Tests.Graphics
{
    /// <summary>
    /// Represents the parameters for a <see cref="BasicEffect"/> test.
    /// </summary>
    public class BasicEffectTestParameters
    {
        /// <inheritdoc/>
        public override String ToString()
        {
            var name = new StringBuilder("Basic");

            if (LightingEnabled)
            {
                name.Append(PreferPerPixelLighting ? "Pixel" : "Vertex");
                name.Append("Lighting");
            }

            if (TextureEnabled)
                name.Append("Tx");

            if (VertexColorEnabled)
                name.Append("Vc");

            if (!FogEnabled)
                name.Append("NoFog");

            return name.ToString();
        }

        /// <summary>
        /// Gets or sets a value indicating whether fog is enabled for the test.
        /// </summary>
        public Boolean FogEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether textures are enabled for the test.
        /// </summary>
        public Boolean TextureEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether vertex color is enabled for the test.
        /// </summary>
        public Boolean VertexColorEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether lighting is enabled for the test.
        /// </summary>
        public Boolean LightingEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to prefer per-pixel lighting.
        /// </summary>
        public Boolean PreferPerPixelLighting { get; set; }
    }
}
