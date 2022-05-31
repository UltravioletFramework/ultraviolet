using Ultraviolet.Core;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents the material properties of a mesh.
    /// </summary>
    public abstract class Material
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Material"/> class.
        /// </summary>
        /// <param name="effectInstance">The material's <see cref="Effect"/> instance.</param>
        public Material(Effect effectInstance)
        {
            Contract.Require(effectInstance, nameof(effectInstance));

            this.Effect = effectInstance;
        }

        /// <summary>
        /// Applies the material for rendering.
        /// </summary>
        public abstract void Apply();

        /// <summary>
        /// Gets the <see cref="Effect"/> in
        /// </summary>
        public Effect Effect { get; }
    }
}
