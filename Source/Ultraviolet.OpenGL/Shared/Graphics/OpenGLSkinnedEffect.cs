using System;
using Ultraviolet.Core;
using Ultraviolet.Graphics;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL implementation of the <see cref="Ultraviolet.Graphics.SkinnedEffect"/> class.
    /// </summary>
    public sealed partial class OpenGLSkinnedEffect : SkinnedEffect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLSkinnedEffect"/> class.
        /// </summary>
        public OpenGLSkinnedEffect(UltravioletContext uv)
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
            epBlock.Bones = Parameters["Bones"];
        }

        /// <inheritdoc/>
        public override void SetBoneTransforms(Matrix[] boneTransforms)
        {
            Contract.Require(boneTransforms, nameof(boneTransforms));

            if (boneTransforms.Length == 0)
                throw new ArgumentException(nameof(boneTransforms));

            epBlock.Bones?.SetValue(boneTransforms);
        }

        /// <inheritdoc/>
        public override void GetBoneTransforms(Matrix[] boneTransforms)
        {
            Contract.Require(boneTransforms, nameof(boneTransforms));

            epBlock.Bones?.GetValueMatrixArray(boneTransforms, boneTransforms.Length);
        }

        /// <inheritdoc/>
        public override void GetBoneTransforms(Matrix[] boneTransforms, Int32 count)
        {
            Contract.Require(boneTransforms, nameof(boneTransforms));
            Contract.EnsureRange(count >= 0 && count < MaxBoneCount, nameof(count));

            epBlock.Bones?.GetValueMatrixArray(boneTransforms, count);
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
        protected override void OnWeightsPerVertexSet() => dirtyFlags |= EffectDirtyFlags.ShaderIndex;

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

            switch (WeightsPerVertex)
            {
                case 2:
                    shaderIndexNew += 2;
                    break;

                case 4:
                    shaderIndexNew += 4;
                    break;
            }

            if (PreferPerPixelLighting)
            {
                shaderIndexNew += 12;
            }
            else
            {
                var oneLight = !DirectionalLight1.Enabled && !DirectionalLight2.Enabled;
                if (oneLight)
                {
                    shaderIndexNew += 6;
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
        private readonly OpenGLSkinnedEffectParameterBlock epBlock = new OpenGLSkinnedEffectParameterBlock();
        private EffectDirtyFlags dirtyFlags = EffectDirtyFlags.All;
    }
}
