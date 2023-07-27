using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;

namespace Ultraviolet.OpenGL.Bindings
{
    public static unsafe partial class GL
    {
        [MonoNativeFunctionWrapper]
        private delegate void glTexImage2DMultisampleDelegate(uint target, int samples, uint internalformat, int width, int height, [MarshalAs(UnmanagedType.I1)] bool fixedsamplelocations);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_texture_multisample")]
        private static glTexImage2DMultisampleDelegate glTexImage2DMultisample = null;

        public static void TexImage2DMultisample(uint target, int samples, uint internalformat, int width, int height, bool fixedsamplelocations)
        {
            glTexImage2DMultisample(target, samples, internalformat, width, height, fixedsamplelocations);
        }

        [MonoNativeFunctionWrapper]
        private delegate void glTexImage3DMultisampleDelegate(uint target, int samples, uint internalformat, int width, int height, int depth, [MarshalAs(UnmanagedType.I1)] bool fixedsamplelocations);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_texture_multisample")]
        private static glTexImage3DMultisampleDelegate glTexImage3DMultisample = null;

        public static void TexImage3DMultisample(uint target, int samples, uint internalformat, int width, int height, int depth, bool fixedsamplelocations)
        {
            glTexImage3DMultisample(target, samples, internalformat, width, height, depth, fixedsamplelocations);
        }

        [MonoNativeFunctionWrapper]
        private delegate void glGetMultisamplefvDelegate(uint pname, uint index, IntPtr val);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_texture_multisample")]
        private static glGetMultisamplefvDelegate glGetMultisamplefv = null;

        public static void GetMultisamplefv(uint pname, uint index, float* val)
        {
            glGetMultisamplefv(pname, index, (IntPtr)val);
        }

        public static float GetMultisamplefv(uint pname, uint index)
        {
            float value;
            glGetMultisamplefv(pname, index, (IntPtr)(&value));
            return value;
        }

        [MonoNativeFunctionWrapper]
        private delegate void glSampleMaskiDelegate(uint index, uint mask);
        [Require(MinVersion = "4.5", Extension = "GL_ARB_texture_multisample")]
        private static glSampleMaskiDelegate glSampleMaski = null;

        public static void SampleMaski(uint index, uint mask)
        {
            glSampleMaski(index, mask);
        }

        public const UInt32 GL_SAMPLE_POSITION                           = 0x8E50;
        public const UInt32 GL_SAMPLE_MASK                               = 0x8E51;
        public const UInt32 GL_TEXTURE_2D_MULTISAMPLE                    = 0x9100;
        public const UInt32 GL_PROXY_TEXTURE_2D_MULTISAMPLE              = 0x9101;
        public const UInt32 GL_TEXTURE_2D_MULTISAMPLE_ARRAY              = 0x9102;
        public const UInt32 GL_PROXY_TEXTURE_2D_MULTISAMPLE_ARRAY        = 0x9103;
        public const UInt32 GL_MAX_SAMPLE_MASK_WORDS                     = 0x8E59;
        public const UInt32 GL_MAX_COLOR_TEXTURE_SAMPLES                 = 0x910E;
        public const UInt32 GL_MAX_DEPTH_TEXTURE_SAMPLES                 = 0x910F;
        public const UInt32 GL_MAX_INTEGER_SAMPLES                       = 0x9110;
        public const UInt32 GL_TEXTURE_BINDING_2D_MULTISAMPLE            = 0x9104;
        public const UInt32 GL_TEXTURE_BINDING_2D_MULTISAMPLE_ARRAY      = 0x9105;
        public const UInt32 GL_TEXTURE_SAMPLES                           = 0x9106;
        public const UInt32 GL_TEXTURE_FIXED_SAMPLE_LOCATIONS            = 0x9107;
        public const UInt32 GL_SAMPLER_2D_MULTISAMPLE                    = 0x9108;
        public const UInt32 GL_INT_SAMPLER_2D_MULTISAMPLE                = 0x9109;
        public const UInt32 GL_UNSIGNED_INT_SAMPLER_2D_MULTISAMPLE       = 0x910A;
        public const UInt32 GL_SAMPLER_2D_MULTISAMPLE_ARRAY              = 0x910B;
        public const UInt32 GL_INT_SAMPLER_2D_MULTISAMPLE_ARRAY          = 0x910C;
        public const UInt32 GL_UNSIGNED_INT_SAMPLER_2D_MULTISAMPLE_ARRAY = 0x910D;
    }
}
