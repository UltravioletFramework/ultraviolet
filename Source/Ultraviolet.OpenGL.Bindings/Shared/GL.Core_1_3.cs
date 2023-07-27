using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;

namespace Ultraviolet.OpenGL.Bindings
{
    public static unsafe partial class GL
    {
        [MonoNativeFunctionWrapper]
        private delegate void glActiveTextureDelegate(uint texture);
        [Require(MinVersion = "1.3")]
        private static glActiveTextureDelegate glActiveTexture = null;

        public static void ActiveTexture(uint texture) { glActiveTexture(texture); }

        [MonoNativeFunctionWrapper]
        private delegate void glClientActiveTextureDelegate(uint texture);
        [Require(MinVersion = "1.3")]
        private static glClientActiveTextureDelegate glClientActiveTexture = null;

        public static void ClientActiveTexture(uint texture) { glClientActiveTexture(texture); }

        [MonoNativeFunctionWrapper]
        private delegate void glCompressedTexImage1DDelegate(uint target, int level, uint internalformat, int width, int border, int imageSize, IntPtr data);
        [Require(MinVersion = "1.3")]
        private static glCompressedTexImage1DDelegate glCompressedTexImage1D = null;

        public static void CompressedTexImage1D(uint target, int level, uint internalformat, int width, int border, int imageSize, void* data) { glCompressedTexImage1D(target, level, internalformat, width, border, imageSize, (IntPtr)data); }

        [MonoNativeFunctionWrapper]
        private delegate void glCompressedTexImage2DDelegate(uint target, int level, uint internalformat, int width, int height, int border, int imageSize, IntPtr data);
        [Require(MinVersion = "1.3")]
        private static glCompressedTexImage2DDelegate glCompressedTexImage2D = null;

        public static void CompressedTexImage2D(uint target, int level, uint internalformat, int width, int height, int border, int imageSize, void* data) { glCompressedTexImage2D(target, level, internalformat, width, height, border, imageSize, (IntPtr)data); }

        [MonoNativeFunctionWrapper]
        private delegate void glCompressedTexImage3DDelegate(uint target, int level, uint internalformat, int width, int height, int depth, int border, int imageSize, IntPtr data);
        [Require(MinVersion = "1.3")]
        private static glCompressedTexImage3DDelegate glCompressedTexImage3D = null;

        public static void CompressedTexImage3D(uint target, int level, uint internalformat, int width, int height, int depth, int border, int imageSize, void* data) { glCompressedTexImage3D(target, level, internalformat, width, height, depth, border, imageSize, (IntPtr)data); }

        [MonoNativeFunctionWrapper]
        private delegate void glCompressedTexSubImage1DDelegate(uint target, int level, int xoffset, int width, uint format, int imageSize, IntPtr data);
        [Require(MinVersion = "1.3")]
        private static glCompressedTexSubImage1DDelegate glCompressedTexSubImage1D = null;

        public static void CompressedTexSubImage1D(uint target, int level, int xoffset, int width, uint format, int imageSize, void* data) { glCompressedTexSubImage1D(target, level, xoffset, width, format, imageSize, (IntPtr)data); }

        [MonoNativeFunctionWrapper]
        private delegate void glCompressedTexSubImage2DDelegate(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, int imageSize, IntPtr data);
        [Require(MinVersion = "1.3")]
        private static glCompressedTexSubImage2DDelegate glCompressedTexSubImage2D = null;

        public static void CompressedTexSubImage2D(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, int imageSize, void* data) { glCompressedTexSubImage2D(target, level, xoffset, yoffset, width, height, format, imageSize, (IntPtr)data); }

        [MonoNativeFunctionWrapper]
        private delegate void glCompressedTexSubImage3DDelegate(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, IntPtr data);
        [Require(MinVersion = "1.3")]
        private static glCompressedTexSubImage3DDelegate glCompressedTexSubImage3D = null;

        public static void CompressedTexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, void* data) { glCompressedTexSubImage3D(target, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, (IntPtr)data); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetCompressedTexImageDelegate(uint target, int lod, IntPtr img);
        [Require(MinVersion = "1.3")]
        private static glGetCompressedTexImageDelegate glGetCompressedTexImage = null;

        public static void GetCompressedTexImage(uint target, int lod, void* img) { glGetCompressedTexImage(target, lod, (IntPtr)img); }

        [MonoNativeFunctionWrapper]
        private delegate void glLoadTransposeMatrixdDelegate(IntPtr m);
        [Require(MinVersion = "1.3")]
        private static glLoadTransposeMatrixdDelegate glLoadTransposeMatrixd = null;

        public static void LoadTransposeMatrixd(double* m) { glLoadTransposeMatrixd((IntPtr)m); }

        [MonoNativeFunctionWrapper]
        private delegate void glLoadTransposeMatrixfDelegate(IntPtr m);
        [Require(MinVersion = "1.3")]
        private static glLoadTransposeMatrixfDelegate glLoadTransposeMatrixf = null;

        public static void LoadTransposeMatrixf(float* m) { glLoadTransposeMatrixf((IntPtr)m); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultTransposeMatrixdDelegate(IntPtr m);
        [Require(MinVersion = "1.3")]
        private static glMultTransposeMatrixdDelegate glMultTransposeMatrixd = null;

        public static void MultTransposeMatrixd(double* m) { glMultTransposeMatrixd((IntPtr)m); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultTransposeMatrixfDelegate(IntPtr m);
        [Require(MinVersion = "1.3")]
        private static glMultTransposeMatrixfDelegate glMultTransposeMatrixf = null;

        public static void MultTransposeMatrixf(float* m) { glMultTransposeMatrixf((IntPtr)m); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord1dDelegate(uint target, double s);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord1dDelegate glMultiTexCoord1d = null;

        public static void MultiTexCoord1d(uint target, double s) { glMultiTexCoord1d(target, s); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord1dvDelegate(uint target, IntPtr v);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord1dvDelegate glMultiTexCoord1dv = null;

        public static void MultiTexCoord1dv(uint target, double* v) { glMultiTexCoord1dv(target, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord1fDelegate(uint target, float s);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord1fDelegate glMultiTexCoord1f = null;

        public static void MultiTexCoord1f(uint target, float s) { glMultiTexCoord1f(target, s); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord1fvDelegate(uint target, IntPtr v);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord1fvDelegate glMultiTexCoord1fv = null;

        public static void MultiTexCoord1fv(uint target, float* v) { glMultiTexCoord1fv(target, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord1iDelegate(uint target, int s);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord1iDelegate glMultiTexCoord1i = null;

        public static void MultiTexCoord1i(uint target, int s) { glMultiTexCoord1i(target, s); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord1ivDelegate(uint target, IntPtr v);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord1ivDelegate glMultiTexCoord1iv = null;

        public static void MultiTexCoord1iv(uint target, int* v) { glMultiTexCoord1iv(target, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord1sDelegate(uint target, short s);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord1sDelegate glMultiTexCoord1s = null;

        public static void MultiTexCoord1s(uint target, short s) { glMultiTexCoord1s(target, s); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord1svDelegate(uint target, IntPtr v);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord1svDelegate glMultiTexCoord1sv = null;

        public static void MultiTexCoord1sv(uint target, short* v) { glMultiTexCoord1sv(target, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord2dDelegate(uint target, double s, double t);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord2dDelegate glMultiTexCoord2d = null;

        public static void MultiTexCoord2d(uint target, double s, double t) { glMultiTexCoord2d(target, s, t); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord2dvDelegate(uint target, IntPtr v);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord2dvDelegate glMultiTexCoord2dv = null;

        public static void MultiTexCoord2dv(uint target, double* v) { glMultiTexCoord2dv(target, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord2fDelegate(uint target, float s, float t);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord2fDelegate glMultiTexCoord2f = null;

        public static void MultiTexCoord2f(uint target, float s, float t) { glMultiTexCoord2f(target, s, t); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord2fvDelegate(uint target, IntPtr v);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord2fvDelegate glMultiTexCoord2fv = null;

        public static void MultiTexCoord2fv(uint target, float* v) { glMultiTexCoord2fv(target, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord2iDelegate(uint target, int s, int t);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord2iDelegate glMultiTexCoord2i = null;

        public static void MultiTexCoord2i(uint target, int s, int t) { glMultiTexCoord2i(target, s, t); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord2ivDelegate(uint target, IntPtr v);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord2ivDelegate glMultiTexCoord2iv = null;

        public static void MultiTexCoord2iv(uint target, int* v) { glMultiTexCoord2iv(target, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord2sDelegate(uint target, short s, short t);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord2sDelegate glMultiTexCoord2s = null;

        public static void MultiTexCoord2s(uint target, short s, short t) { glMultiTexCoord2s(target, s, t); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord2svDelegate(uint target, IntPtr v);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord2svDelegate glMultiTexCoord2sv = null;

        public static void MultiTexCoord2sv(uint target, short* v) { glMultiTexCoord2sv(target, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord3dDelegate(uint target, double s, double t, double r);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord3dDelegate glMultiTexCoord3d = null;

        public static void MultiTexCoord3d(uint target, double s, double t, double r) { glMultiTexCoord3d(target, s, t, r); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord3dvDelegate(uint target, IntPtr v);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord3dvDelegate glMultiTexCoord3dv = null;

        public static void MultiTexCoord3dv(uint target, double* v) { glMultiTexCoord3dv(target, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord3fDelegate(uint target, float s, float t, float r);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord3fDelegate glMultiTexCoord3f = null;

        public static void MultiTexCoord3f(uint target, float s, float t, float r) { glMultiTexCoord3f(target, s, t, r); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord3fvDelegate(uint target, IntPtr v);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord3fvDelegate glMultiTexCoord3fv = null;

        public static void MultiTexCoord3fv(uint target, float* v) { glMultiTexCoord3fv(target, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord3iDelegate(uint target, int s, int t, int r);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord3iDelegate glMultiTexCoord3i = null;

        public static void MultiTexCoord3i(uint target, int s, int t, int r) { glMultiTexCoord3i(target, s, t, r); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord3ivDelegate(uint target, IntPtr v);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord3ivDelegate glMultiTexCoord3iv = null;

        public static void MultiTexCoord3iv(uint target, int* v) { glMultiTexCoord3iv(target, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord3sDelegate(uint target, short s, short t, short r);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord3sDelegate glMultiTexCoord3s = null;

        public static void MultiTexCoord3s(uint target, short s, short t, short r) { glMultiTexCoord3s(target, s, t, r); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord3svDelegate(uint target, IntPtr v);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord3svDelegate glMultiTexCoord3sv = null;

        public static void MultiTexCoord3sv(uint target, short* v) { glMultiTexCoord3sv(target, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord4dDelegate(uint target, double s, double t, double r, double q);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord4dDelegate glMultiTexCoord4d = null;

        public static void MultiTexCoord4d(uint target, double s, double t, double r, double q) { glMultiTexCoord4d(target, s, t, r, q); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord4dvDelegate(uint target, IntPtr v);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord4dvDelegate glMultiTexCoord4dv = null;

        public static void MultiTexCoord4dv(uint target, double* v) { glMultiTexCoord4dv(target, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord4fDelegate(uint target, float s, float t, float r, float q);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord4fDelegate glMultiTexCoord4f = null;

        public static void MultiTexCoord4f(uint target, float s, float t, float r, float q) { glMultiTexCoord4f(target, s, t, r, q); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord4fvDelegate(uint target, IntPtr v);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord4fvDelegate glMultiTexCoord4fv = null;

        public static void MultiTexCoord4fv(uint target, float* v) { glMultiTexCoord4fv(target, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord4iDelegate(uint target, int s, int t, int r, int q);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord4iDelegate glMultiTexCoord4i = null;

        public static void MultiTexCoord4i(uint target, int s, int t, int r, int q) { glMultiTexCoord4i(target, s, t, r, q); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord4ivDelegate(uint target, IntPtr v);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord4ivDelegate glMultiTexCoord4iv = null;

        public static void MultiTexCoord4iv(uint target, int* v) { glMultiTexCoord4iv(target, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord4sDelegate(uint target, short s, short t, short r, short q);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord4sDelegate glMultiTexCoord4s = null;

        public static void MultiTexCoord4s(uint target, short s, short t, short r, short q) { glMultiTexCoord4s(target, s, t, r, q); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiTexCoord4svDelegate(uint target, IntPtr v);
        [Require(MinVersion = "1.3")]
        private static glMultiTexCoord4svDelegate glMultiTexCoord4sv = null;

        public static void MultiTexCoord4sv(uint target, short* v) { glMultiTexCoord4sv(target, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glSampleCoverageDelegate(float value, [MarshalAs(UnmanagedType.I1)] bool invert);
        [Require(MinVersion = "1.3")]
        private static glSampleCoverageDelegate glSampleCoverage = null;

        public static void SampleCoverage(float value, bool invert) { glSampleCoverage(value, invert); }

        public const UInt32 GL_MULTISAMPLE = 0x809D;
        public const UInt32 GL_SAMPLE_ALPHA_TO_COVERAGE = 0x809E;
        public const UInt32 GL_SAMPLE_ALPHA_TO_ONE = 0x809F;
        public const UInt32 GL_SAMPLE_COVERAGE = 0x80A0;
        public const UInt32 GL_SAMPLE_BUFFERS = 0x80A8;
        public const UInt32 GL_SAMPLES = 0x80A9;
        public const UInt32 GL_SAMPLE_COVERAGE_VALUE = 0x80AA;
        public const UInt32 GL_SAMPLE_COVERAGE_INVERT = 0x80AB;
        public const UInt32 GL_CLAMP_TO_BORDER = 0x812D;
        public const UInt32 GL_TEXTURE0 = 0x84C0;
        public const UInt32 GL_TEXTURE1 = 0x84C1;
        public const UInt32 GL_TEXTURE2 = 0x84C2;
        public const UInt32 GL_TEXTURE3 = 0x84C3;
        public const UInt32 GL_TEXTURE4 = 0x84C4;
        public const UInt32 GL_TEXTURE5 = 0x84C5;
        public const UInt32 GL_TEXTURE6 = 0x84C6;
        public const UInt32 GL_TEXTURE7 = 0x84C7;
        public const UInt32 GL_TEXTURE8 = 0x84C8;
        public const UInt32 GL_TEXTURE9 = 0x84C9;
        public const UInt32 GL_TEXTURE10 = 0x84CA;
        public const UInt32 GL_TEXTURE11 = 0x84CB;
        public const UInt32 GL_TEXTURE12 = 0x84CC;
        public const UInt32 GL_TEXTURE13 = 0x84CD;
        public const UInt32 GL_TEXTURE14 = 0x84CE;
        public const UInt32 GL_TEXTURE15 = 0x84CF;
        public const UInt32 GL_TEXTURE16 = 0x84D0;
        public const UInt32 GL_TEXTURE17 = 0x84D1;
        public const UInt32 GL_TEXTURE18 = 0x84D2;
        public const UInt32 GL_TEXTURE19 = 0x84D3;
        public const UInt32 GL_TEXTURE20 = 0x84D4;
        public const UInt32 GL_TEXTURE21 = 0x84D5;
        public const UInt32 GL_TEXTURE22 = 0x84D6;
        public const UInt32 GL_TEXTURE23 = 0x84D7;
        public const UInt32 GL_TEXTURE24 = 0x84D8;
        public const UInt32 GL_TEXTURE25 = 0x84D9;
        public const UInt32 GL_TEXTURE26 = 0x84DA;
        public const UInt32 GL_TEXTURE27 = 0x84DB;
        public const UInt32 GL_TEXTURE28 = 0x84DC;
        public const UInt32 GL_TEXTURE29 = 0x84DD;
        public const UInt32 GL_TEXTURE30 = 0x84DE;
        public const UInt32 GL_TEXTURE31 = 0x84DF;
        public const UInt32 GL_ACTIVE_TEXTURE = 0x84E0;
        public const UInt32 GL_CLIENT_ACTIVE_TEXTURE = 0x84E1;
        public const UInt32 GL_MAX_TEXTURE_UNITS = 0x84E2;
        public const UInt32 GL_TRANSPOSE_MODELVIEW_MATRIX = 0x84E3;
        public const UInt32 GL_TRANSPOSE_PROJECTION_MATRIX = 0x84E4;
        public const UInt32 GL_TRANSPOSE_TEXTURE_MATRIX = 0x84E5;
        public const UInt32 GL_TRANSPOSE_COLOR_MATRIX = 0x84E6;
        public const UInt32 GL_SUBTRACT = 0x84E7;
        public const UInt32 GL_COMPRESSED_ALPHA = 0x84E9;
        public const UInt32 GL_COMPRESSED_LUMINANCE = 0x84EA;
        public const UInt32 GL_COMPRESSED_LUMINANCE_ALPHA = 0x84EB;
        public const UInt32 GL_COMPRESSED_INTENSITY = 0x84EC;
        public const UInt32 GL_COMPRESSED_RGB = 0x84ED;
        public const UInt32 GL_COMPRESSED_RGBA = 0x84EE;
        public const UInt32 GL_TEXTURE_COMPRESSION_HINT = 0x84EF;
        public const UInt32 GL_NORMAL_MAP = 0x8511;
        public const UInt32 GL_REFLECTION_MAP = 0x8512;
        public const UInt32 GL_TEXTURE_CUBE_MAP = 0x8513;
        public const UInt32 GL_TEXTURE_BINDING_CUBE_MAP = 0x8514;
        public const UInt32 GL_TEXTURE_CUBE_MAP_POSITIVE_X = 0x8515;
        public const UInt32 GL_TEXTURE_CUBE_MAP_NEGATIVE_X = 0x8516;
        public const UInt32 GL_TEXTURE_CUBE_MAP_POSITIVE_Y = 0x8517;
        public const UInt32 GL_TEXTURE_CUBE_MAP_NEGATIVE_Y = 0x8518;
        public const UInt32 GL_TEXTURE_CUBE_MAP_POSITIVE_Z = 0x8519;
        public const UInt32 GL_TEXTURE_CUBE_MAP_NEGATIVE_Z = 0x851A;
        public const UInt32 GL_PROXY_TEXTURE_CUBE_MAP = 0x851B;
        public const UInt32 GL_MAX_CUBE_MAP_TEXTURE_SIZE = 0x851C;
        public const UInt32 GL_COMBINE = 0x8570;
        public const UInt32 GL_COMBINE_RGB = 0x8571;
        public const UInt32 GL_COMBINE_ALPHA = 0x8572;
        public const UInt32 GL_RGB_SCALE = 0x8573;
        public const UInt32 GL_ADD_SIGNED = 0x8574;
        public const UInt32 GL_INTERPOLATE = 0x8575;
        public const UInt32 GL_CONSTANT = 0x8576;
        public const UInt32 GL_PRIMARY_COLOR = 0x8577;
        public const UInt32 GL_PREVIOUS = 0x8578;
        public const UInt32 GL_SOURCE0_RGB = 0x8580;
        public const UInt32 GL_SOURCE1_RGB = 0x8581;
        public const UInt32 GL_SOURCE2_RGB = 0x8582;
        public const UInt32 GL_SOURCE0_ALPHA = 0x8588;
        public const UInt32 GL_SOURCE1_ALPHA = 0x8589;
        public const UInt32 GL_SOURCE2_ALPHA = 0x858A;
        public const UInt32 GL_OPERAND0_RGB = 0x8590;
        public const UInt32 GL_OPERAND1_RGB = 0x8591;
        public const UInt32 GL_OPERAND2_RGB = 0x8592;
        public const UInt32 GL_OPERAND0_ALPHA = 0x8598;
        public const UInt32 GL_OPERAND1_ALPHA = 0x8599;
        public const UInt32 GL_OPERAND2_ALPHA = 0x859A;
        public const UInt32 GL_TEXTURE_COMPRESSED_IMAGE_SIZE = 0x86A0;
        public const UInt32 GL_TEXTURE_COMPRESSED = 0x86A1;
        public const UInt32 GL_NUM_COMPRESSED_TEXTURE_FORMATS = 0x86A2;
        public const UInt32 GL_COMPRESSED_TEXTURE_FORMATS = 0x86A3;
        public const UInt32 GL_DOT3_RGB = 0x86AE;
        public const UInt32 GL_DOT3_RGBA = 0x86AF;
        public const UInt32 GL_MULTISAMPLE_BIT = 0x20000000;
    }
}
