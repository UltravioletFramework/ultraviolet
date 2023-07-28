using System;
using Ultraviolet.Core;

namespace Ultraviolet.OpenGL.Bindings
{
    public static unsafe partial class GL
    {
        [MonoNativeFunctionWrapper]
        private delegate void glDrawArraysInstancedDelegate(uint mode, int first, int count, int primcount);
        [Require(MinVersion = "3.1")]
        private static glDrawArraysInstancedDelegate glDrawArraysInstanced = null;

        public static void DrawArraysInstanced(uint mode, int first, int count, int primcount) { glDrawArraysInstanced(mode, first, count, primcount); }

        [MonoNativeFunctionWrapper]
        private delegate void glDrawElementsInstancedDelegate(uint mode, int count, uint type, IntPtr indices, int primcount);
        [Require(MinVersion = "3.1")]
        private static glDrawElementsInstancedDelegate glDrawElementsInstanced = null;

        public static void DrawElementsInstanced(uint mode, int count, uint type, void* indices, int primcount) { glDrawElementsInstanced(mode, count, type, (IntPtr)indices, primcount); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexBufferDelegate(uint target, uint internalFormat, uint buffer);
        [Require(MinVersion = "3.1")]
        private static glTexBufferDelegate glTexBuffer = null;

        public static void TexBuffer(uint target, uint internalFormat, uint buffer) { glTexBuffer(target, internalFormat, buffer); }

        [MonoNativeFunctionWrapper]
        private delegate void glPrimitiveRestartIndexDelegate(uint index);
        [Require(MinVersion = "3.1")]
        private static glPrimitiveRestartIndexDelegate glPrimitiveRestartIndex = null;

        public static void PrimitiveRestartIndex(uint index) { glPrimitiveRestartIndex(index); }

        public const UInt32 GL_TEXTURE_RECTANGLE = 0x84F5;
        public const UInt32 GL_TEXTURE_BINDING_RECTANGLE = 0x84F6;
        public const UInt32 GL_PROXY_TEXTURE_RECTANGLE = 0x84F7;
        public const UInt32 GL_MAX_RECTANGLE_TEXTURE_SIZE = 0x84F8;
        public const UInt32 GL_SAMPLER_2D_RECT = 0x8B63;
        public const UInt32 GL_SAMPLER_2D_RECT_SHADOW = 0x8B64;
        public const UInt32 GL_TEXTURE_BUFFER = 0x8C2A;
        public const UInt32 GL_MAX_TEXTURE_BUFFER_SIZE = 0x8C2B;
        public const UInt32 GL_TEXTURE_BINDING_BUFFER = 0x8C2C;
        public const UInt32 GL_TEXTURE_BUFFER_DATA_STORE_BINDING = 0x8C2D;
        public const UInt32 GL_TEXTURE_BUFFER_FORMAT = 0x8C2E;
        public const UInt32 GL_SAMPLER_BUFFER = 0x8DC2;
        public const UInt32 GL_INT_SAMPLER_2D_RECT = 0x8DCD;
        public const UInt32 GL_INT_SAMPLER_BUFFER = 0x8DD0;
        public const UInt32 GL_UNSIGNED_INT_SAMPLER_2D_RECT = 0x8DD5;
        public const UInt32 GL_UNSIGNED_INT_SAMPLER_BUFFER = 0x8DD8;
        public const UInt32 GL_RED_SNORM = 0x8F90;
        public const UInt32 GL_RG_SNORM = 0x8F91;
        public const UInt32 GL_RGB_SNORM = 0x8F92;
        public const UInt32 GL_RGBA_SNORM = 0x8F93;
        public const UInt32 GL_R8_SNORM = 0x8F94;
        public const UInt32 GL_RG8_SNORM = 0x8F95;
        public const UInt32 GL_RGB8_SNORM = 0x8F96;
        public const UInt32 GL_RGBA8_SNORM = 0x8F97;
        public const UInt32 GL_R16_SNORM = 0x8F98;
        public const UInt32 GL_RG16_SNORM = 0x8F99;
        public const UInt32 GL_RGB16_SNORM = 0x8F9A;
        public const UInt32 GL_RGBA16_SNORM = 0x8F9B;
        public const UInt32 GL_SIGNED_NORMALIZED = 0x8F9C;
        public const UInt32 GL_PRIMITIVE_RESTART = 0x8F9D;
        public const UInt32 GL_PRIMITIVE_RESTART_INDEX = 0x8F9E;
        public const UInt32 GL_BUFFER_ACCESS_FLAGS = 0x911F;
        public const UInt32 GL_BUFFER_MAP_LENGTH = 0x9120;
        public const UInt32 GL_BUFFER_MAP_OFFSET = 0x9121;
    }
}