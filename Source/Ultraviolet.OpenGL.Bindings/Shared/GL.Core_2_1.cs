using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;

namespace Ultraviolet.OpenGL.Bindings
{
    public static unsafe partial class GL
    {
        [MonoNativeFunctionWrapper]
        private delegate void glUniformMatrix2x3fvDelegate(int location, int count, [MarshalAs(UnmanagedType.I1)] bool transpose, IntPtr value);
        [Require(MinVersion = "2.1")]
        private static glUniformMatrix2x3fvDelegate glUniformMatrix2x3fv = null;

        public static void UniformMatrix2x3fv(int location, int count, bool transpose, float* value) { glUniformMatrix2x3fv(location, count, transpose, (IntPtr)value); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniformMatrix3x2fvDelegate(int location, int count, [MarshalAs(UnmanagedType.I1)] bool transpose, IntPtr value);
        [Require(MinVersion = "2.1")]
        private static glUniformMatrix3x2fvDelegate glUniformMatrix3x2fv = null;

        public static void UniformMatrix3x2fv(int location, int count, bool transpose, float* value) { glUniformMatrix3x2fv(location, count, transpose, (IntPtr)value); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniformMatrix2x4fvDelegate(int location, int count, [MarshalAs(UnmanagedType.I1)] bool transpose, IntPtr value);
        [Require(MinVersion = "2.1")]
        private static glUniformMatrix2x4fvDelegate glUniformMatrix2x4fv = null;

        public static void UniformMatrix2x4fv(int location, int count, bool transpose, float* value) { glUniformMatrix2x4fv(location, count, transpose, (IntPtr)value); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniformMatrix4x2fvDelegate(int location, int count, [MarshalAs(UnmanagedType.I1)] bool transpose, IntPtr value);
        [Require(MinVersion = "2.1")]
        private static glUniformMatrix4x2fvDelegate glUniformMatrix4x2fv = null;

        public static void UniformMatrix4x2fv(int location, int count, bool transpose, float* value) { glUniformMatrix4x2fv(location, count, transpose, (IntPtr)value); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniformMatrix3x4fvDelegate(int location, int count, [MarshalAs(UnmanagedType.I1)] bool transpose, IntPtr value);
        [Require(MinVersion = "2.1")]
        private static glUniformMatrix3x4fvDelegate glUniformMatrix3x4fv = null;

        public static void UniformMatrix3x4fv(int location, int count, bool transpose, float* value) { glUniformMatrix3x4fv(location, count, transpose, (IntPtr)value); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniformMatrix4x3fvDelegate(int location, int count, [MarshalAs(UnmanagedType.I1)] bool transpose, IntPtr value);
        [Require(MinVersion = "2.1")]
        private static glUniformMatrix4x3fvDelegate glUniformMatrix4x3fv = null;

        public static void UniformMatrix4x3fv(int location, int count, bool transpose, float* value) { glUniformMatrix4x3fv(location, count, transpose, (IntPtr)value); }

        public const UInt32 GL_CURRENT_RASTER_SECONDARY_COLOR = 0x845F;
        public const UInt32 GL_PIXEL_PACK_BUFFER = 0x88EB;
        public const UInt32 GL_PIXEL_UNPACK_BUFFER = 0x88EC;
        public const UInt32 GL_PIXEL_PACK_BUFFER_BINDING = 0x88ED;
        public const UInt32 GL_PIXEL_UNPACK_BUFFER_BINDING = 0x88EF;
        public const UInt32 GL_FLOAT_MAT2x3 = 0x8B65;
        public const UInt32 GL_FLOAT_MAT2x4 = 0x8B66;
        public const UInt32 GL_FLOAT_MAT3x2 = 0x8B67;
        public const UInt32 GL_FLOAT_MAT3x4 = 0x8B68;
        public const UInt32 GL_FLOAT_MAT4x2 = 0x8B69;
        public const UInt32 GL_FLOAT_MAT4x3 = 0x8B6A;
        public const UInt32 GL_SRGB = 0x8C40;
        public const UInt32 GL_SRGB8 = 0x8C41;
        public const UInt32 GL_SRGB_ALPHA = 0x8C42;
        public const UInt32 GL_SRGB8_ALPHA8 = 0x8C43;
        public const UInt32 GL_SLUMINANCE_ALPHA = 0x8C44;
        public const UInt32 GL_SLUMINANCE8_ALPHA8 = 0x8C45;
        public const UInt32 GL_SLUMINANCE = 0x8C46;
        public const UInt32 GL_SLUMINANCE8 = 0x8C47;
        public const UInt32 GL_COMPRESSED_SRGB = 0x8C48;
        public const UInt32 GL_COMPRESSED_SRGB_ALPHA = 0x8C49;
        public const UInt32 GL_COMPRESSED_SLUMINANCE = 0x8C4A;
        public const UInt32 GL_COMPRESSED_SLUMINANCE_ALPHA = 0x8C4B;
    }
}
