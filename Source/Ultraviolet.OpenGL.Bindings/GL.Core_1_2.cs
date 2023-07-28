using System;
using Ultraviolet.Core;

namespace Ultraviolet.OpenGL.Bindings
{
    public static unsafe partial class GL
    {
        [MonoNativeFunctionWrapper]
        private delegate void glDrawRangeElementsDelegate(uint mode, uint start, uint end, int count, uint type, IntPtr indices);
        [Require(MinVersion = "1.2")]
        private static glDrawRangeElementsDelegate glDrawRangeElements = null;

        public static void DrawRangeElements(uint mode, uint start, uint end, int count, uint type, void* indices) { glDrawRangeElements(mode, start, end, count, type, (IntPtr)indices); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexImage3DDelegate(uint target, int level, int internalFormat, int width, int height, int depth, int border, uint format, uint type, IntPtr data);
        [Require(MinVersion = "1.2")]
        private static glTexImage3DDelegate glTexImage3D = null;

        public static void TexImage3D(uint target, int level, int internalFormat, int width, int height, int depth, int border, uint format, uint type, void* data) { glTexImage3D(target, level, internalFormat, width, height, depth, border, format, type, (IntPtr)data); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexSubImage3DDelegate(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr data);
        [Require(MinVersion = "1.2")]
        private static glTexSubImage3DDelegate glTexSubImage3D = null;

        public static void TexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, void* data) { glTexSubImage3D(target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, (IntPtr)data); }

        [MonoNativeFunctionWrapper]
        private delegate void glCopyTexSubImage3DDelegate(uint target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height);
        [Require(MinVersion = "1.2")]
        private static glCopyTexSubImage3DDelegate glCopyTexSubImage3D = null;

        public static void CopyTexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height) { glCopyTexSubImage3D(target, level, xoffset, yoffset, zoffset, x, y, width, height); }

        public const UInt32 GL_SMOOTH_POINT_SIZE_RANGE = 0x0B12;
        public const UInt32 GL_SMOOTH_POINT_SIZE_GRANULARITY = 0x0B13;
        public const UInt32 GL_SMOOTH_LINE_WIDTH_RANGE = 0x0B22;
        public const UInt32 GL_SMOOTH_LINE_WIDTH_GRANULARITY = 0x0B23;
        public const UInt32 GL_UNSIGNED_BYTE_3_3_2 = 0x8032;
        public const UInt32 GL_UNSIGNED_SHORT_4_4_4_4 = 0x8033;
        public const UInt32 GL_UNSIGNED_SHORT_5_5_5_1 = 0x8034;
        public const UInt32 GL_UNSIGNED_INT_8_8_8_8 = 0x8035;
        public const UInt32 GL_UNSIGNED_INT_10_10_10_2 = 0x8036;
        public const UInt32 GL_RESCALE_NORMAL = 0x803A;
        public const UInt32 GL_TEXTURE_BINDING_3D = 0x806A;
        public const UInt32 GL_PACK_SKIP_IMAGES = 0x806B;
        public const UInt32 GL_PACK_IMAGE_HEIGHT = 0x806C;
        public const UInt32 GL_UNPACK_SKIP_IMAGES = 0x806D;
        public const UInt32 GL_UNPACK_IMAGE_HEIGHT = 0x806E;
        public const UInt32 GL_TEXTURE_3D = 0x806F;
        public const UInt32 GL_PROXY_TEXTURE_3D = 0x8070;
        public const UInt32 GL_TEXTURE_DEPTH = 0x8071;
        public const UInt32 GL_TEXTURE_WRAP_R = 0x8072;
        public const UInt32 GL_MAX_3D_TEXTURE_SIZE = 0x8073;
        public const UInt32 GL_BGR = 0x80E0;
        public const UInt32 GL_BGRA = 0x80E1;
        public const UInt32 GL_MAX_ELEMENTS_VERTICES = 0x80E8;
        public const UInt32 GL_MAX_ELEMENTS_INDICES = 0x80E9;
        public const UInt32 GL_CLAMP_TO_EDGE = 0x812F;
        public const UInt32 GL_TEXTURE_MIN_LOD = 0x813A;
        public const UInt32 GL_TEXTURE_MAX_LOD = 0x813B;
        public const UInt32 GL_TEXTURE_BASE_LEVEL = 0x813C;
        public const UInt32 GL_TEXTURE_MAX_LEVEL = 0x813D;
        public const UInt32 GL_LIGHT_MODEL_COLOR_CONTROL = 0x81F8;
        public const UInt32 GL_SINGLE_COLOR = 0x81F9;
        public const UInt32 GL_SEPARATE_SPECULAR_COLOR = 0x81FA;
        public const UInt32 GL_UNSIGNED_BYTE_2_3_3_REV = 0x8362;
        public const UInt32 GL_UNSIGNED_SHORT_5_6_5 = 0x8363;
        public const UInt32 GL_UNSIGNED_SHORT_5_6_5_REV = 0x8364;
        public const UInt32 GL_UNSIGNED_SHORT_4_4_4_4_REV = 0x8365;
        public const UInt32 GL_UNSIGNED_SHORT_1_5_5_5_REV = 0x8366;
        public const UInt32 GL_UNSIGNED_INT_8_8_8_8_REV = 0x8367;
        public const UInt32 GL_ALIASED_POINT_SIZE_RANGE = 0x846D;
        public const UInt32 GL_ALIASED_LINE_WIDTH_RANGE = 0x846E;
    }
}
