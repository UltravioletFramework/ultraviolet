using System;
using Ultraviolet.Graphics;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Contains methods for calculating various effect parameter values.
    /// </summary>
    internal static class EffectHelpers
    {
        /// <summary>
        /// Checks to see whether the shader index for the specified effect has possibly changed.
        /// </summary>
        public static Boolean CheckForShaderIndexChanges<TEffect>(TEffect effect, EffectDirtyFlags dirtyFlags, ref Boolean oneLight)
            where TEffect : IEffectLights
        {
            var oneLightNew = !effect.DirectionalLight1.Enabled && !effect.DirectionalLight2.Enabled;
            if (oneLightNew != oneLight)
            {
                oneLight = oneLightNew;
                return true;
            }
            return (dirtyFlags & EffectDirtyFlags.ShaderIndex) == EffectDirtyFlags.ShaderIndex;
        }

        /// <summary>
        /// Updates the material color parameters for the specified effect.
        /// </summary>
        public static void UpdateEffectMaterialColor<TEffect>(TEffect effect, OpenGLBasicEffectParameterBlock parameters)
            where TEffect : IEffectLights, IEffectMaterialColor
        {
            var srgb = effect.SrgbColor;
            var alpha = effect.Alpha;

            if (parameters.SpecularColor != null)
                parameters.SpecularColor.SetValue(effect.SpecularColor);

            if (parameters.SpecularPower != null)
                parameters.SpecularPower.SetValue(effect.SpecularPower);

            var diffuseColor = (srgb ? Color.ConvertSrgbColorToLinear(effect.DiffuseColor) : effect.DiffuseColor).ToVector4();
            var diffuse = new Vector4
            {
                X = diffuseColor.X * alpha,
                Y = diffuseColor.Y * alpha,
                Z = diffuseColor.Z * alpha,
                W = alpha
            };

            if (parameters.DiffuseColor != null)
                parameters.DiffuseColor.SetValue(diffuse);

            var ambientLightColor = (srgb ? Color.ConvertSrgbColorToLinear(effect.AmbientLightColor) : effect.AmbientLightColor).ToVector3();
            var emissiveColor = (srgb ? Color.ConvertSrgbColorToLinear(effect.EmissiveColor) : effect.EmissiveColor).ToVector3();
            var emissive = new Vector3
            {
                X = (emissiveColor.X + ambientLightColor.X * diffuseColor.X) * alpha,
                Y = (emissiveColor.Y + ambientLightColor.Y * diffuseColor.Y) * alpha,
                Z = (emissiveColor.Z + ambientLightColor.Z * diffuseColor.Z) * alpha
            };

            if (parameters.EmissiveColor != null)
                parameters.EmissiveColor.SetValue(emissive);
        }

        /// <summary>
        /// Updates the world matrix parameter for the specified effect.
        /// </summary>
        public static void UpdateEffectWorld<TEffect>(TEffect effect, OpenGLBasicEffectParameterBlock parameters)
            where TEffect : IEffectMatrices
        {
            var world = effect.World;

            if (parameters.World != null)
                parameters.World.SetValue(world);

            if (parameters.WorldInverseTranspose != null)
            {
                Matrix.TryInvertRef(ref world, out var worldInverse);
                Matrix.Transpose(ref worldInverse, out var worldInverseTranspose);
                parameters.WorldInverseTranspose.SetValueRef(ref worldInverseTranspose);
            }
        }

        /// <summary>
        /// Updates the world/view/proj matrix parameter for the specified effect.
        /// </summary>
        public static void UpdateEffectWorldViewProj<TEffect>(TEffect effect, OpenGLBasicEffectParameterBlock parameters, ref Matrix worldView)
            where TEffect : IEffectMatrices
        {
            var world = effect.World;
            var view = effect.View;
            Matrix.Multiply(ref world, ref view, out worldView);

            if (parameters.WorldViewProj != null)
            {
                var proj = effect.Projection;

                Matrix.Multiply(ref worldView, ref proj, out var worldViewProj);
                parameters.WorldViewProj.SetValue(worldViewProj);
            }
        }

        /// <summary>
        /// Updates the eye position parameter for the specified effect.
        /// </summary>
        public static void UpdateEffectEyePosition<TEffect>(TEffect effect, OpenGLBasicEffectParameterBlock parameters)
            where TEffect : IEffectMatrices
        {
            var view = effect.View;

            if (parameters.EyePosition != null)
            {
                Matrix.TryInvertRef(ref view, out var viewInverse);
                parameters.EyePosition.SetValue(viewInverse.Translation);
            }
        }

        /// <summary>
        /// Updates the fog parameters for the specified effect.
        /// </summary>
        public static void UpdateEffectFog<TEffect>(TEffect effect, OpenGLBasicEffectParameterBlock parameters, ref Matrix worldView)
            where TEffect : IEffectFog
        {
            if (effect.FogEnabled)
            {
                if (parameters.FogColor != null)
                    parameters.FogColor.SetValue(effect.FogColor);

                if (parameters.FogVector != null)
                {
                    var fogStart = effect.FogStart;
                    var fogEnd = effect.FogEnd;

                    if (fogStart == fogEnd)
                    {
                        parameters.FogVector.SetValue(new Vector4(0, 0, 0, 1));
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
                        parameters.FogVector.SetValue(fogVector);
                    }
                }
            }
            else
            {
                if (parameters.FogVector != null)
                    parameters.FogVector.SetValue(Vector4.Zero);
            }
        }

        /// <summary>
        /// Updates the basic effect parameters for the specified effect.
        /// </summary>
        public static void UpdateEffectParameters<TEffect>(TEffect effect, EffectDirtyFlags dirtyFlags, OpenGLBasicEffectParameterBlock parameters, ref Matrix worldView)
            where TEffect : IEffectMatrices, IEffectFog, IEffectLights, IEffectTexture, IEffectMaterialColor
        {
            if ((dirtyFlags & EffectDirtyFlags.SrgbColor) == EffectDirtyFlags.SrgbColor)
            {
                if (parameters.SrgbColor != null)
                    parameters.SrgbColor.SetValue(effect.SrgbColor);
            }

            if ((dirtyFlags & EffectDirtyFlags.MaterialColor) == EffectDirtyFlags.MaterialColor)
            {
                UpdateEffectMaterialColor(effect, parameters);
            }

            if ((dirtyFlags & EffectDirtyFlags.MaterialTexture) == EffectDirtyFlags.MaterialTexture)
            {
                if (parameters.Texture != null)
                    parameters.Texture.SetValue(effect.Texture);
            }

            if ((dirtyFlags & EffectDirtyFlags.World) == EffectDirtyFlags.World)
            {
                UpdateEffectWorld(effect, parameters);
            }

            if ((dirtyFlags & EffectDirtyFlags.WorldViewProjection) == EffectDirtyFlags.WorldViewProjection)
            {
                UpdateEffectWorldViewProj(effect, parameters, ref worldView);
            }

            if ((dirtyFlags & EffectDirtyFlags.EyePosition) == EffectDirtyFlags.EyePosition)
            {
                UpdateEffectEyePosition(effect, parameters);
            }

            if ((dirtyFlags & EffectDirtyFlags.Fog) == EffectDirtyFlags.Fog || (dirtyFlags & EffectDirtyFlags.FogEnabled) == EffectDirtyFlags.FogEnabled)
            {
                UpdateEffectFog(effect, parameters, ref worldView);
            }

            effect.DirectionalLight0.Apply();
            effect.DirectionalLight1.Apply();
            effect.DirectionalLight2.Apply();
        }
    }
}
