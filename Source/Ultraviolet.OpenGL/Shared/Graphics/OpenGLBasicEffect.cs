using System;
using Ultraviolet.Graphics;

namespace Ultraviolet.OpenGL.Graphics.Graphics2D
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
            epDiffuseColor = Parameters["DiffuseColor"];
            epEmissiveColor = Parameters["EmissiveColor"];
            epSpecularColor = Parameters["SpecularColor"];
            epSpecularPower = Parameters["SpecularPower"];
            epEyePosition = Parameters["EyePosition"];
            epFogColor = Parameters["FogColor"];
            epFogVector = Parameters["FogVector"];
            epWorld = Parameters["World"];
            epWorldInverseTranspose = Parameters["WorldInverseTranspose"];
            epWorldViewProj = Parameters["WorldViewProj"];
            epSrgbColor = Parameters["SrgbColor"];
            epTexture = Parameters["Texture"];
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
            var oneLightNew = !DirectionalLight1.Enabled && !DirectionalLight2.Enabled;
            if (oneLightNew != oneLight)
            {
                oneLight = oneLightNew;
                dirtyFlags |= EffectDirtyFlags.ShaderIndex;
            }

            if ((dirtyFlags & EffectDirtyFlags.ShaderIndex) == EffectDirtyFlags.ShaderIndex)
            {
                if (UpdateShaderIndex())
                    dirtyFlags = EffectDirtyFlags.All;
            }

            if ((dirtyFlags & EffectDirtyFlags.SrgbColor) == EffectDirtyFlags.SrgbColor)
            {
                if (epSrgbColor != null)
                    epSrgbColor.SetValue(SrgbColor);
            }

            if ((dirtyFlags & EffectDirtyFlags.MaterialColor) == EffectDirtyFlags.MaterialColor)
            {
                var alpha = Alpha;

                if (epSpecularColor != null)
                    epSpecularColor.SetValue(SpecularColor);

                if (epSpecularPower != null)
                    epSpecularPower.SetValue(SpecularPower);

                if (LightingEnabled)
                {
                    var diffuseColor = (SrgbColor ? Color.ConvertSrgbColorToLinear(DiffuseColor) : DiffuseColor).ToVector4();
                    var diffuse = new Vector4
                    {
                        X = diffuseColor.X * alpha,
                        Y = diffuseColor.Y * alpha,
                        Z = diffuseColor.Z * alpha,
                        W = alpha
                    };

                    if (epDiffuseColor != null)
                        epDiffuseColor.SetValue(diffuse);

                    var ambientLightColor = (SrgbColor ? Color.ConvertSrgbColorToLinear(AmbientLightColor) : AmbientLightColor).ToVector3();
                    var emissiveColor = (SrgbColor ? Color.ConvertSrgbColorToLinear(EmissiveColor) : EmissiveColor).ToVector3();
                    var emissive = new Vector3
                    {
                        X = (emissiveColor.X + ambientLightColor.X * diffuseColor.X) * alpha,
                        Y = (emissiveColor.Y + ambientLightColor.Y * diffuseColor.Y) * alpha,
                        Z = (emissiveColor.Z + ambientLightColor.Z * diffuseColor.Z) * alpha
                    };

                    if (epEmissiveColor != null)
                        epEmissiveColor.SetValue(emissive);
                }
                else
                {
                    if (epDiffuseColor != null)
                    {
                        var emissiveColor = (SrgbColor ? Color.ConvertSrgbColorToLinear(EmissiveColor) : EmissiveColor).ToVector3();
                        var diffuseColor = (SrgbColor ? Color.ConvertSrgbColorToLinear(DiffuseColor) : DiffuseColor).ToVector4();
                        var diffuse = new Vector4
                        {
                            X = (diffuseColor.X + emissiveColor.X) * alpha,
                            Y = (diffuseColor.Y + emissiveColor.Y) * alpha,
                            Z = (diffuseColor.Z + emissiveColor.Z) * alpha,
                            W = alpha
                        };

                        epDiffuseColor.SetValue(diffuse);
                    }
                }
            }

            if ((dirtyFlags & EffectDirtyFlags.MaterialTexture) == EffectDirtyFlags.MaterialTexture)
            {
                if (epTexture != null)
                    epTexture.SetValue(Texture);
            }

            if ((dirtyFlags & EffectDirtyFlags.World) == EffectDirtyFlags.World)
            {
                var world = World;
                
                if (epWorld != null)
                    epWorld.SetValue(world);

                if (epWorldInverseTranspose != null)
                {
                    Matrix.TryInvertRef(ref world, out var worldInverse);
                    Matrix.Transpose(ref worldInverse, out var worldInverseTranspose);
                    epWorldInverseTranspose.SetValueRef(ref worldInverseTranspose);
                }
            }

            if ((dirtyFlags & EffectDirtyFlags.WorldViewProjection) == EffectDirtyFlags.WorldViewProjection)
            {
                if (epWorldViewProj != null)
                {
                    var world = World;
                    var view = View;
                    var proj = Projection;

                    Matrix.Multiply(ref world, ref view, out worldView);
                    Matrix.Multiply(ref worldView, ref proj, out var worldViewProj);
                    epWorldViewProj.SetValue(worldViewProj);
                }
            }

            if ((dirtyFlags & EffectDirtyFlags.EyePosition) == EffectDirtyFlags.EyePosition)
            {
                var view = View;

                if (epEyePosition != null)
                {
                    Matrix.TryInvertRef(ref view, out var viewInverse);
                    epEyePosition.SetValue(viewInverse.Translation);
                }
            }

            if ((dirtyFlags & EffectDirtyFlags.Fog) == EffectDirtyFlags.Fog || (dirtyFlags & EffectDirtyFlags.FogEnabled) == EffectDirtyFlags.FogEnabled)
            {
                if (FogEnabled)
                {
                    if (epFogColor != null)
                        epFogColor.SetValue(FogColor);

                    if (epFogVector != null)
                    {
                        var fogStart = FogStart;
                        var fogEnd = FogEnd;

                        if (fogStart == fogEnd)
                        {
                            epFogVector.SetValue(new Vector4(0, 0, 0, 1));
                        }
                        else
                        {
                            var scale = 1f / (fogStart - fogEnd);
                            var fogVector = new Vector4
                            {
                                X = worldView.M13 * scale,
                                Y = worldView.M23 * scale,
                                Z = worldView.M33 * scale,
                                W = (worldView.M43 + fogStart) * scale
                            };
                            epFogVector.SetValue(fogVector);
                        }
                    }
                }
                else
                {
                    if (epFogVector != null)
                        epFogVector.SetValue(Vector4.Zero);
                }
            }

            DirectionalLight0.Apply();
            DirectionalLight1.Apply();
            DirectionalLight2.Apply();

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
        private readonly EffectParameter epDiffuseColor;
        private readonly EffectParameter epEmissiveColor;
        private readonly EffectParameter epSpecularColor;
        private readonly EffectParameter epSpecularPower;
        private readonly EffectParameter epEyePosition;
        private readonly EffectParameter epFogColor;
        private readonly EffectParameter epFogVector;
        private readonly EffectParameter epWorld;
        private readonly EffectParameter epWorldInverseTranspose;
        private readonly EffectParameter epWorldViewProj;
        private readonly EffectParameter epSrgbColor;
        private readonly EffectParameter epTexture;
        private EffectDirtyFlags dirtyFlags = EffectDirtyFlags.All;
    }
}
