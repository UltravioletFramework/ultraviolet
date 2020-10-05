using System;
using Ultraviolet.Graphics;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL implementation of the <see cref="Ultraviolet.Graphics.BasicEffect"/> class.
    /// </summary>
    public sealed partial class OpenGLBasicEffect : BasicEffect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLBasicEffect"/> class.
        /// </summary>
        public OpenGLBasicEffect(UltravioletContext uv)
            : base(CreateEffectImplementation(uv))
        {
            epBlock.DiffuseColor = Parameters["DiffuseColor"];
            epBlock.EmissiveColor = Parameters["EmissiveColor"];
            epBlock.SpecularColor = Parameters["SpecularColor"];
            epBlock.SpecularPower = Parameters["SpecularPower"];
            epBlock.EyePosition = Parameters["EyePosition"];
            epBlock.FogColor = Parameters["FogColor"];
            epBlock.FogVector = Parameters["FogVector"];
            epBlock.World = Parameters["World"];
            epBlock.WorldInverseTranspose = Parameters["WorldInverseTranspose"];
            epBlock.WorldViewProj = Parameters["WorldViewProj"];
            epBlock.SrgbColor = Parameters["SrgbColor"];
            epBlock.Texture = Parameters["Texture"];
        }

        /// <inheritdoc/>
        protected override DirectionalLight CreateDirectionalLight(Int32 index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (index > 2)
                throw new NotSupportedException();

            return new DirectionalLight(
                Parameters[$"DirLight{index}Direction"],
                Parameters[$"DirLight{index}DiffuseColor"],
                Parameters[$"DirLight{index}SpecularColor"]);
        }

        /// <inheritdoc/>
        protected override void OnSrgbColorSet() => dirtyFlags |= EffectDirtyFlags.SrgbColor;

        /// <inheritdoc/>
        protected override void OnPreferPerPixelLightingSet() => dirtyFlags |= EffectDirtyFlags.ShaderIndex;

        /// <inheritdoc/>
        protected override void OnTextureEnabledSet() => dirtyFlags |= EffectDirtyFlags.ShaderIndex;

        /// <inheritdoc/>
        protected override void OnVertexColorEnabledSet() => dirtyFlags |= EffectDirtyFlags.ShaderIndex;

        /// <inheritdoc/>
        protected override void OnAlphaSet() => dirtyFlags |= EffectDirtyFlags.MaterialColor;

        /// <inheritdoc/>
        protected override void OnDiffuseColorSet() => dirtyFlags |= EffectDirtyFlags.MaterialColor;

        /// <inheritdoc/>
        protected override void OnEmissiveColorSet() => dirtyFlags |= EffectDirtyFlags.MaterialColor;

        /// <inheritdoc/>
        protected override void OnSpecularColorSet() => dirtyFlags |= EffectDirtyFlags.MaterialColor;

        /// <inheritdoc/>
        protected override void OnSpecularPowerSet() => dirtyFlags |= EffectDirtyFlags.MaterialColor;

        /// <inheritdoc/>
        protected override void OnWorldSet() => dirtyFlags |= EffectDirtyFlags.World | EffectDirtyFlags.WorldViewProjection | EffectDirtyFlags.Fog;

        /// <inheritdoc/>
        protected override void OnViewSet() => dirtyFlags |= EffectDirtyFlags.EyePosition | EffectDirtyFlags.WorldViewProjection | EffectDirtyFlags.Fog;

        /// <inheritdoc/>
        protected override void OnProjectionSet() => dirtyFlags |= EffectDirtyFlags.WorldViewProjection;

        /// <inheritdoc/>
        protected override void OnLightingEnabledSet() => dirtyFlags |= EffectDirtyFlags.ShaderIndex;

        /// <inheritdoc/>
        protected override void OnAmbientLightColorSet() => dirtyFlags |= EffectDirtyFlags.MaterialColor;

        /// <inheritdoc/>
        protected override void OnFogEnabledSet() => dirtyFlags |= EffectDirtyFlags.FogEnabled | EffectDirtyFlags.Fog | EffectDirtyFlags.ShaderIndex;

        /// <inheritdoc/>
        protected override void OnFogColorSet() => dirtyFlags |= EffectDirtyFlags.Fog;

        /// <inheritdoc/>
        protected override void OnFogStartSet() => dirtyFlags |= EffectDirtyFlags.Fog;

        /// <inheritdoc/>
        protected override void OnFogEndSet() => dirtyFlags |= EffectDirtyFlags.Fog;

        /// <inheritdoc/>
        protected override void OnTextureSet() => dirtyFlags |= EffectDirtyFlags.MaterialTexture;

        /// <inheritdoc/>
        protected override void OnApply()
        {
            if (EffectHelpers.CheckForShaderIndexChanges(this, dirtyFlags, ref oneLight) && UpdateShaderIndex())
                dirtyFlags = EffectDirtyFlags.All;

            EffectHelpers.UpdateEffectParameters(this, dirtyFlags, epBlock, ref worldView);
            dirtyFlags = EffectDirtyFlags.None;

            base.OnApply();
        }

        /// <summary>
        /// Updates the effect's shader index.
        /// </summary>
        /// <returns><see langword="true"/> if the shader index changed; otherwise, <see langword="false"/>.</returns>
        private Boolean UpdateShaderIndex()
        {
            var shaderIndexOld = ((OpenGLEffectPass)CurrentTechnique.Passes[0]).ProgramIndex;
            var shaderIndexNew = 0;

            if (!FogEnabled)
                shaderIndexNew += 1;

            if (VertexColorEnabled)
                shaderIndexNew += 2;

            if (TextureEnabled)
                shaderIndexNew += 4;

            if (LightingEnabled)
            {
                if (PreferPerPixelLighting)
                {
                    shaderIndexNew += 24;
                }
                else
                {
                    if (oneLight)
                    {
                        shaderIndexNew += 16;
                    }
                    else
                    {
                        shaderIndexNew += 8;
                    }
                }
            }

            if (shaderIndexNew != shaderIndexOld)
            {
                ((OpenGLEffectPass)CurrentTechnique.Passes[0]).ProgramIndex = shaderIndexNew;
                return true;
            }

            return false;
        }

        // Calculated parameter values.
        private Matrix worldView;
        private Boolean oneLight;

        // Effect parameters.
        private readonly OpenGLBasicEffectParameterBlock epBlock = new OpenGLBasicEffectParameterBlock();
        private EffectDirtyFlags dirtyFlags = EffectDirtyFlags.All;
    }
}
