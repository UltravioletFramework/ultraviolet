using System;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a directional light.
    /// </summary>
    public partial class DirectionalLight
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectionalLight"/> class.
        /// </summary>
        /// <param name="directionParameter">The effect parameter which represents the light's direction.</param>
        /// <param name="diffuseColorParameter">The effect parameter which represents the light's diffuse color.</param>
        /// <param name="specularColorParameter">The effect parameter which represents the light's specular color.</param>
        /// <param name="cloneSource">An existing instance from which to copy parameters.</param>
        /// <remarks>The initial parameter values are either copied from the cloned object or set to default values if no cloned object is specified.
        /// The three <see cref="EffectParameter"/> instances are updated whenever the direction, diffuse color, or specular color properties are
        /// changed, or you can set these to <see langword="null"/> if you are cloning an existing instance.</remarks>
        public DirectionalLight(
            EffectParameter directionParameter,
            EffectParameter diffuseColorParameter,
            EffectParameter specularColorParameter,
            DirectionalLight cloneSource = null)
        {
            if (cloneSource == null)
            {
                this.epDirection = directionParameter;
                this.epDiffuseColor = diffuseColorParameter;
                this.epSpecularColor = specularColorParameter;
            }
            else
            {
                this.Enabled = cloneSource.Enabled;
                this.epDirection = cloneSource.epDirection;
                this.direction = cloneSource.direction;
                this.epDiffuseColor = cloneSource.epDiffuseColor;
                this.diffuseColor = cloneSource.diffuseColor;
                this.epSpecularColor = cloneSource.epSpecularColor;
                this.specularColor = cloneSource.specularColor;
            }
        }

        /// <summary>
        /// Applies the directional light's parameters as part of an effect pass.
        /// </summary>
        public void Apply()
        {
            if ((dirtyFlags & DirtyFlags.Direction) == DirtyFlags.Direction)
            {
                epDirection?.SetValue(Vector3.Normalize(direction));
            }

            if (enabled)
            {
                if ((dirtyFlags & DirtyFlags.DiffuseColor) == DirtyFlags.DiffuseColor)
                {
                    var specularColorSrgb = srgbColor ? Color.ConvertSrgbColorToLinear(diffuseColor) : diffuseColor;
                    epDiffuseColor?.SetValue(specularColorSrgb);
                }

                if ((dirtyFlags & DirtyFlags.SpecularColor) == DirtyFlags.SpecularColor)
                {
                    var specularColorSrgb = srgbColor ? Color.ConvertSrgbColorToLinear(specularColor) : specularColor;
                    epSpecularColor?.SetValue(specularColorSrgb);
                }
            }
            else
            {
                if ((dirtyFlags & DirtyFlags.DiffuseColor) == DirtyFlags.DiffuseColor)
                {
                    epDiffuseColor?.SetValue(Color.Transparent);
                }

                if ((dirtyFlags & DirtyFlags.SpecularColor) == DirtyFlags.SpecularColor)
                {
                    epSpecularColor?.SetValue(Color.Transparent);
                }
            }

            dirtyFlags = DirtyFlags.None;
        }

        /// <summary>
        /// Gets or sets a flag indicating whether the light is enabled.
        /// </summary>
        public Boolean Enabled
        {
            get => enabled;
            set
            {
                enabled = value;
                dirtyFlags |= DirtyFlags.SpecularColor | DirtyFlags.DiffuseColor;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether sRGB color is enabled for this light.
        /// </summary>
        public Boolean SrgbColor
        {
            get => srgbColor;
            set
            {
                srgbColor = value;
                dirtyFlags |= DirtyFlags.SpecularColor | DirtyFlags.DiffuseColor;
            }
        }

        /// <summary>
        /// Gets or sets the light's direction. This value must be a unit vector.
        /// </summary>
        public Vector3 Direction
        {
            get => direction;
            set
            {
                direction = value;
                dirtyFlags |= DirtyFlags.Direction;
            }
        }

        /// <summary>
        /// Gets or sets the diffuse color of the light.
        /// </summary>
        public Color DiffuseColor
        {
            get => diffuseColor;
            set
            {
                diffuseColor = value;
                dirtyFlags |= DirtyFlags.DiffuseColor;
            }
        }

        /// <summary>
        /// Gets or sets the specular color of the light.
        /// </summary>
        public Color SpecularColor
        {
            get => specularColor;
            set
            {
                specularColor = value;
                dirtyFlags |= DirtyFlags.SpecularColor;
            }
        }

        // Effect parameters.
        private readonly EffectParameter epDirection;
        private readonly EffectParameter epDiffuseColor;
        private readonly EffectParameter epSpecularColor;

        // Parameter values.
        private Boolean enabled;
        private Boolean srgbColor;
        private Vector3 direction;
        private Color diffuseColor;
        private Color specularColor;
        private DirtyFlags dirtyFlags = DirtyFlags.None;
    }
}
