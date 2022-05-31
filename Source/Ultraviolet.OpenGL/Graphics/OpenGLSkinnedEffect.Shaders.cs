using System;
using System.Linq;
using Ultraviolet.Core;
using Ultraviolet.Graphics;

namespace Ultraviolet.OpenGL.Graphics
{
    partial class OpenGLSkinnedEffect
    {
        /// <summary>
        /// Creates the effect implementation.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <returns>The effect implementation.</returns>
        private static EffectImplementation CreateEffectImplementation(UltravioletContext uv)
        {
            Contract.Require(uv, nameof(uv));

            var programs = new OpenGLShaderProgram[VSIndices.Length];
            for (var i = 0; i < programs.Length; i++)
            {
                var vShader = VSArray[VSIndices[i]];
                var fShader = PSArray[PSIndices[i]];
                programs[i] = new OpenGLShaderProgram(uv, vShader, fShader, false);
            }

            var passes = new[] { new OpenGLEffectPass(uv, null, programs) };
            var techniques = new[] { new OpenGLEffectTechnique(uv, null, passes) };
            return new OpenGLEffectImplementation(uv, techniques);
        }

        // An array containing all of the vertex shaders used by this effect.
        private static readonly UltravioletSingleton<OpenGLVertexShader>[] VSArray = new[]
        {
            new UltravioletSingleton<OpenGLVertexShader>(UltravioletSingletonFlags.DisabledInServiceMode | UltravioletSingletonFlags.Lazy,
                uv => { return new OpenGLVertexShader(uv, ResourceUtil.ReadShaderResourceString("SkinnedEffect_VSSkinnedVertexLightingOneBone.vert")); }),
            new UltravioletSingleton<OpenGLVertexShader>(UltravioletSingletonFlags.DisabledInServiceMode | UltravioletSingletonFlags.Lazy,
                uv => { return new OpenGLVertexShader(uv, ResourceUtil.ReadShaderResourceString("SkinnedEffect_VSSkinnedVertexLightingTwoBones.vert")); }),
            new UltravioletSingleton<OpenGLVertexShader>(UltravioletSingletonFlags.DisabledInServiceMode | UltravioletSingletonFlags.Lazy,
                uv => { return new OpenGLVertexShader(uv, ResourceUtil.ReadShaderResourceString("SkinnedEffect_VSSkinnedVertexLightingFourBones.vert")); }),

            new UltravioletSingleton<OpenGLVertexShader>(UltravioletSingletonFlags.DisabledInServiceMode | UltravioletSingletonFlags.Lazy,
                uv => { return new OpenGLVertexShader(uv, ResourceUtil.ReadShaderResourceString("SkinnedEffect_VSSkinnedOneLightOneBone.vert")); }),
            new UltravioletSingleton<OpenGLVertexShader>(UltravioletSingletonFlags.DisabledInServiceMode | UltravioletSingletonFlags.Lazy,
                uv => { return new OpenGLVertexShader(uv, ResourceUtil.ReadShaderResourceString("SkinnedEffect_VSSkinnedOneLightTwoBones.vert")); }),
            new UltravioletSingleton<OpenGLVertexShader>(UltravioletSingletonFlags.DisabledInServiceMode | UltravioletSingletonFlags.Lazy,
                uv => { return new OpenGLVertexShader(uv, ResourceUtil.ReadShaderResourceString("SkinnedEffect_VSSkinnedOneLightFourBones.vert")); }),

            new UltravioletSingleton<OpenGLVertexShader>(UltravioletSingletonFlags.DisabledInServiceMode | UltravioletSingletonFlags.Lazy,
                uv => { return new OpenGLVertexShader(uv, ResourceUtil.ReadShaderResourceString("SkinnedEffect_VSSkinnedPixelLightingOneBone.vert")); }),
            new UltravioletSingleton<OpenGLVertexShader>(UltravioletSingletonFlags.DisabledInServiceMode | UltravioletSingletonFlags.Lazy,
                uv => { return new OpenGLVertexShader(uv, ResourceUtil.ReadShaderResourceString("SkinnedEffect_VSSkinnedPixelLightingTwoBones.vert")); }),
            new UltravioletSingleton<OpenGLVertexShader>(UltravioletSingletonFlags.DisabledInServiceMode | UltravioletSingletonFlags.Lazy,
                uv => { return new OpenGLVertexShader(uv, ResourceUtil.ReadShaderResourceString("SkinnedEffect_VSSkinnedPixelLightingFourBones.vert")); }),
        };

        // An array correlating the shader index of this effect to the vertex shader which that index uses.
        private static readonly Int32[] VSIndices = new[]
        {
            0,      // vertex lighting, one bone
            0,      // vertex lighting, one bone, no fog
            1,      // vertex lighting, two bones
            1,      // vertex lighting, two bones, no fog
            2,      // vertex lighting, four bones
            2,      // vertex lighting, four bones, no fog
    
            3,      // one light, one bone
            3,      // one light, one bone, no fog
            4,      // one light, two bones
            4,      // one light, two bones, no fog
            5,      // one light, four bones
            5,      // one light, four bones, no fog
    
            6,      // pixel lighting, one bone
            6,      // pixel lighting, one bone, no fog
            7,      // pixel lighting, two bones
            7,      // pixel lighting, two bones, no fog
            8,      // pixel lighting, four bones
            8,      // pixel lighting, four bones, no fog
        };

        // An array containing all of the fragment shaders used by this effect.
        private static readonly UltravioletSingleton<OpenGLFragmentShader>[] PSArray = new[]
        {
            new UltravioletSingleton<OpenGLFragmentShader>(UltravioletSingletonFlags.DisabledInServiceMode | UltravioletSingletonFlags.Lazy,
                uv => { return new OpenGLFragmentShader(uv, ResourceUtil.ReadShaderResourceString("SkinnedEffect_PSSkinnedVertexLighting.frag")); }),
            new UltravioletSingleton<OpenGLFragmentShader>(UltravioletSingletonFlags.DisabledInServiceMode | UltravioletSingletonFlags.Lazy,
                uv => { return new OpenGLFragmentShader(uv, ResourceUtil.ReadShaderResourceString("SkinnedEffect_PSSkinnedVertexLightingNoFog.frag")); }),
            new UltravioletSingleton<OpenGLFragmentShader>(UltravioletSingletonFlags.DisabledInServiceMode | UltravioletSingletonFlags.Lazy,
                uv => { return new OpenGLFragmentShader(uv, ResourceUtil.ReadShaderResourceString("SkinnedEffect_PSSkinnedPixelLighting.frag")); }),
        };

        // An array correlating the shader index of this effect to the fragment shader which that index uses.
        private static readonly Int32[] PSIndices = new[]
        {
            0,      // vertex lighting, one bone
            1,      // vertex lighting, one bone, no fog
            0,      // vertex lighting, two bones
            1,      // vertex lighting, two bones, no fog
            0,      // vertex lighting, four bones
            1,      // vertex lighting, four bones, no fog
    
            0,      // one light, one bone
            1,      // one light, one bone, no fog
            0,      // one light, two bones
            1,      // one light, two bones, no fog
            0,      // one light, four bones
            1,      // one light, four bones, no fog
    
            2,      // pixel lighting, one bone
            2,      // pixel lighting, one bone, no fog
            2,      // pixel lighting, two bones
            2,      // pixel lighting, two bones, no fog
            2,      // pixel lighting, four bones
            2,      // pixel lighting, four bones, no fog
        };
    }
}
