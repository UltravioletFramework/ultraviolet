using System;

namespace TwistedLogik.Gluon
{
    public static unsafe partial class gl
    {
        private delegate void glMinSampleShadingDelegate(float value);
        [Require(MinVersion = "4.0")]
        private static readonly glMinSampleShadingDelegate glMinSampleShading = null;

        public static void MinSampleShading(float value) { glMinSampleShading(value); }

        private delegate void glBlendEquationSeparateiDelegate(uint buf, uint modeRGB, uint modeAlpha);
        [Require(MinVersion = "4.0")]
        private static readonly glBlendEquationSeparateiDelegate glBlendEquationSeparatei = null;

        public static void BlendEquationSeparatei(uint buf, uint modeRGB, uint modeAlpha) { glBlendEquationSeparatei(buf, modeRGB, modeAlpha); }

        private delegate void glBlendEquationiDelegate(uint buf, uint mode);
        [Require(MinVersion = "4.0")]
        private static readonly glBlendEquationiDelegate glBlendEquationi = null;

        public static void BlendEquationi(uint buf, uint mode) { glBlendEquationi(buf, mode); }

        private delegate void glBlendFuncSeparateiDelegate(uint buf, uint srcRGB, uint dstRGB, uint srcAlpha, uint dstAlpha);
        [Require(MinVersion = "4.0")]
        private static readonly glBlendFuncSeparateiDelegate glBlendFuncSeparatei = null;

        public static void BlendFuncSeparatei(uint buf, uint srcRGB, uint dstRGB, uint srcAlpha, uint dstAlpha) { glBlendFuncSeparatei(buf, srcRGB, dstRGB, srcAlpha, dstAlpha); }

        private delegate void glBlendFunciDelegate(uint buf, uint src, uint dst);
        [Require(MinVersion = "4.0")]
        private static readonly glBlendFunciDelegate glBlendFunci = null;

        public static void BlendFunci(uint buf, uint src, uint dst) { glBlendFunci(buf, src, dst); }

        public const UInt32 GL_SAMPLE_SHADING = 0x8C36;
        public const UInt32 GL_MIN_SAMPLE_SHADING_VALUE = 0x8C37;
        public const UInt32 GL_MIN_PROGRAM_TEXTURE_GATHER_OFFSET = 0x8E5E;
        public const UInt32 GL_MAX_PROGRAM_TEXTURE_GATHER_OFFSET = 0x8E5F;
        public const UInt32 GL_MAX_PROGRAM_TEXTURE_GATHER_COMPONENTS = 0x8F9F;
        public const UInt32 GL_TEXTURE_CUBE_MAP_ARRAY = 0x9009;
        public const UInt32 GL_TEXTURE_BINDING_CUBE_MAP_ARRAY = 0x900A;
        public const UInt32 GL_PROXY_TEXTURE_CUBE_MAP_ARRAY = 0x900B;
        public const UInt32 GL_SAMPLER_CUBE_MAP_ARRAY = 0x900C;
        public const UInt32 GL_SAMPLER_CUBE_MAP_ARRAY_SHADOW = 0x900D;
        public const UInt32 GL_INT_SAMPLER_CUBE_MAP_ARRAY = 0x900E;
        public const UInt32 GL_UNSIGNED_INT_SAMPLER_CUBE_MAP_ARRAY = 0x900F;
    }
}
