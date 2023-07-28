using System;
using Ultraviolet.Core;

namespace Ultraviolet.OpenGL.Bindings
{
    public static unsafe partial class GL
    {
        [MonoNativeFunctionWrapper]
        private delegate void glBlendEquationDelegate(uint mode);
        [Require(MinVersion = "1.4")]
        private static glBlendEquationDelegate glBlendEquation = null;

        public static void BlendEquation(uint mode) { glBlendEquation(mode); }

        [MonoNativeFunctionWrapper]
        private delegate void glBlendColorDelegate(float red, float green, float blue, float alpha);
        [Require(MinVersion = "1.4")]
        private static glBlendColorDelegate glBlendColor = null;

        public static void BlendColor(float red, float green, float blue, float alpha) { glBlendColor(red, green, blue, alpha); }

        [MonoNativeFunctionWrapper]
        private delegate void glFogCoordfDelegate(float coord);
        [Require(MinVersion = "1.4")]
        private static glFogCoordfDelegate glFogCoordf = null;

        public static void FogCoordf(float coord) { glFogCoordf(coord); }

        [MonoNativeFunctionWrapper]
        private delegate void glFogCoordfvDelegate(IntPtr coord);
        [Require(MinVersion = "1.4")]
        private static glFogCoordfvDelegate glFogCoordfv = null;

        public static void FogCoordfv(float* coord) { glFogCoordfv((IntPtr)coord); }

        [MonoNativeFunctionWrapper]
        private delegate void glFogCoorddDelegate(double coord);
        [Require(MinVersion = "1.4")]
        private static glFogCoorddDelegate glFogCoordd = null;

        public static void FogCoordd(double coord) { glFogCoordd(coord); }

        [MonoNativeFunctionWrapper]
        private delegate void glFogCoorddvDelegate(IntPtr coord);
        [Require(MinVersion = "1.4")]
        private static glFogCoorddvDelegate glFogCoorddv = null;

        public static void FogCoorddv(double* coord) { glFogCoorddv((IntPtr)coord); }

        [MonoNativeFunctionWrapper]
        private delegate void glFogCoordPointerDelegate(uint type, int stride, IntPtr pointer);
        [Require(MinVersion = "1.4")]
        private static glFogCoordPointerDelegate glFogCoordPointer = null;

        public static void FogCoordPointer(uint type, int stride, void* pointer) { glFogCoordPointer(type, stride, (IntPtr)pointer); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiDrawArraysDelegate(uint mode, IntPtr first, IntPtr count, int drawcount);
        [Require(MinVersion = "1.4")]
        private static glMultiDrawArraysDelegate glMultiDrawArrays = null;

        public static void MultiDrawArrays(uint mode, int* first, int* count, int drawcount) { glMultiDrawArrays(mode, (IntPtr)first, (IntPtr)count, drawcount); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultiDrawElementsDelegate(uint mode, IntPtr count, uint type, IntPtr indices, int drawcount);
        [Require(MinVersion = "1.4")]
        private static glMultiDrawElementsDelegate glMultiDrawElements = null;

        public static void MultiDrawElements(uint mode, int* count, uint type, void** indices, int drawcount) { glMultiDrawElements(mode, (IntPtr)count, type, (IntPtr)indices, drawcount); }

        [MonoNativeFunctionWrapper]
        private delegate void glPointParameteriDelegate(uint pname, int param);
        [Require(MinVersion = "1.4")]
        private static glPointParameteriDelegate glPointParameteri = null;

        public static void PointParameteri(uint pname, int param) { glPointParameteri(pname, param); }

        [MonoNativeFunctionWrapper]
        private delegate void glPointParameterivDelegate(uint pname, IntPtr @params);
        [Require(MinVersion = "1.4")]
        private static glPointParameterivDelegate glPointParameteriv = null;

        public static void PointParameteriv(uint pname, int* @params) { glPointParameteriv(pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glPointParameterfDelegate(uint pname, float param);
        [Require(MinVersion = "1.4")]
        private static glPointParameterfDelegate glPointParameterf = null;

        public static void PointParameterf(uint pname, float param) { glPointParameterf(pname, param); }

        [MonoNativeFunctionWrapper]
        private delegate void glPointParameterfvDelegate(uint pname, IntPtr @params);
        [Require(MinVersion = "1.4")]
        private static glPointParameterfvDelegate glPointParameterfv = null;

        public static void PointParameterfv(uint pname, float* @params) { glPointParameterfv(pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glSecondaryColor3bDelegate(sbyte red, sbyte green, sbyte blue);
        [Require(MinVersion = "1.4")]
        private static glSecondaryColor3bDelegate glSecondaryColor3b = null;

        public static void SecondaryColor3b(sbyte red, sbyte green, sbyte blue) { glSecondaryColor3b(red, green, blue); }

        [MonoNativeFunctionWrapper]
        private delegate void glSecondaryColor3bvDelegate(IntPtr v);
        [Require(MinVersion = "1.4")]
        private static glSecondaryColor3bvDelegate glSecondaryColor3bv = null;

        public static void SecondaryColor3bv(sbyte* v) { glSecondaryColor3bv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glSecondaryColor3dDelegate(double red, double green, double blue);
        [Require(MinVersion = "1.4")]
        private static glSecondaryColor3dDelegate glSecondaryColor3d = null;

        public static void SecondaryColor3d(double red, double green, double blue) { glSecondaryColor3d(red, green, blue); }

        [MonoNativeFunctionWrapper]
        private delegate void glSecondaryColor3dvDelegate(IntPtr v);
        [Require(MinVersion = "1.4")]
        private static glSecondaryColor3dvDelegate glSecondaryColor3dv = null;

        public static void SecondaryColor3dv(double* v) { glSecondaryColor3dv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glSecondaryColor3fDelegate(float red, float green, float blue);
        [Require(MinVersion = "1.4")]
        private static glSecondaryColor3fDelegate glSecondaryColor3f = null;

        public static void SecondaryColor3f(float red, float green, float blue) { glSecondaryColor3f(red, green, blue); }

        [MonoNativeFunctionWrapper]
        private delegate void glSecondaryColor3fvDelegate(IntPtr v);
        [Require(MinVersion = "1.4")]
        private static glSecondaryColor3fvDelegate glSecondaryColor3fv = null;

        public static void SecondaryColor3fv(float* v) { glSecondaryColor3fv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glSecondaryColor3iDelegate(int red, int green, int blue);
        [Require(MinVersion = "1.4")]
        private static glSecondaryColor3iDelegate glSecondaryColor3i = null;

        public static void SecondaryColor3i(int red, int green, int blue) { glSecondaryColor3i(red, green, blue); }

        [MonoNativeFunctionWrapper]
        private delegate void glSecondaryColor3ivDelegate(IntPtr v);
        [Require(MinVersion = "1.4")]
        private static glSecondaryColor3ivDelegate glSecondaryColor3iv = null;

        public static void SecondaryColor3iv(int* v) { glSecondaryColor3iv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glSecondaryColor3sDelegate(short red, short green, short blue);
        [Require(MinVersion = "1.4")]
        private static glSecondaryColor3sDelegate glSecondaryColor3s = null;

        public static void SecondaryColor3s(short red, short green, short blue) { glSecondaryColor3s(red, green, blue); }

        [MonoNativeFunctionWrapper]
        private delegate void glSecondaryColor3svDelegate(IntPtr v);
        [Require(MinVersion = "1.4")]
        private static glSecondaryColor3svDelegate glSecondaryColor3sv = null;

        public static void SecondaryColor3sv(short* v) { glSecondaryColor3sv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glSecondaryColor3ubDelegate(byte red, byte green, byte blue);
        [Require(MinVersion = "1.4")]
        private static glSecondaryColor3ubDelegate glSecondaryColor3ub = null;

        public static void SecondaryColor3ub(byte red, byte green, byte blue) { glSecondaryColor3ub(red, green, blue); }

        [MonoNativeFunctionWrapper]
        private delegate void glSecondaryColor3ubvDelegate(IntPtr v);
        [Require(MinVersion = "1.4")]
        private static glSecondaryColor3ubvDelegate glSecondaryColor3ubv = null;

        public static void SecondaryColor3ubv(byte* v) { glSecondaryColor3ubv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glSecondaryColor3uiDelegate(uint red, uint green, uint blue);
        [Require(MinVersion = "1.4")]
        private static glSecondaryColor3uiDelegate glSecondaryColor3ui = null;

        public static void SecondaryColor3ui(uint red, uint green, uint blue) { glSecondaryColor3ui(red, green, blue); }

        [MonoNativeFunctionWrapper]
        private delegate void glSecondaryColor3uivDelegate(IntPtr v);
        [Require(MinVersion = "1.4")]
        private static glSecondaryColor3uivDelegate glSecondaryColor3uiv = null;

        public static void SecondaryColor3uiv(uint* v) { glSecondaryColor3uiv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glSecondaryColor3usDelegate(ushort red, ushort green, ushort blue);
        [Require(MinVersion = "1.4")]
        private static glSecondaryColor3usDelegate glSecondaryColor3us = null;

        public static void SecondaryColor3us(ushort red, ushort green, ushort blue) { glSecondaryColor3us(red, green, blue); }

        [MonoNativeFunctionWrapper]
        private delegate void glSecondaryColor3usvDelegate(IntPtr v);
        [Require(MinVersion = "1.4")]
        private static glSecondaryColor3usvDelegate glSecondaryColor3usv = null;

        public static void SecondaryColor3usv(ushort* v) { glSecondaryColor3usv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glSecondaryColorPointerDelegate(int size, uint type, int stride, IntPtr pointer);
        [Require(MinVersion = "1.4")]
        private static glSecondaryColorPointerDelegate glSecondaryColorPointer = null;

        public static void SecondaryColorPointer(int size, uint type, int stride, void* pointer) { glSecondaryColorPointer(size, type, stride, (IntPtr)pointer); }

        [MonoNativeFunctionWrapper]
        private delegate void glBlendFuncSeparateDelegate(uint sfactorRGB, uint dfactorRGB, uint sfactorAlpha, uint dfactorAlpha);
        [Require(MinVersion = "1.4")]
        private static glBlendFuncSeparateDelegate glBlendFuncSeparate = null;

        public static void BlendFuncSeparate(uint sfactorRGB, uint dfactorRGB, uint sfactorAlpha, uint dfactorAlpha) { glBlendFuncSeparate(sfactorRGB, dfactorRGB, sfactorAlpha, dfactorAlpha); }

        [MonoNativeFunctionWrapper]
        private delegate void glWindowPos2dDelegate(double x, double y);
        [Require(MinVersion = "1.4")]
        private static glWindowPos2dDelegate glWindowPos2d = null;

        public static void WindowPos2d(double x, double y) { glWindowPos2d(x, y); }

        [MonoNativeFunctionWrapper]
        private delegate void glWindowPos2fDelegate(float x, float y);
        [Require(MinVersion = "1.4")]
        private static glWindowPos2fDelegate glWindowPos2f = null;

        public static void WindowPos2f(float x, float y) { glWindowPos2f(x, y); }

        [MonoNativeFunctionWrapper]
        private delegate void glWindowPos2iDelegate(int x, int y);
        [Require(MinVersion = "1.4")]
        private static glWindowPos2iDelegate glWindowPos2i = null;

        public static void WindowPos2i(int x, int y) { glWindowPos2i(x, y); }

        [MonoNativeFunctionWrapper]
        private delegate void glWindowPos2sDelegate(short x, short y);
        [Require(MinVersion = "1.4")]
        private static glWindowPos2sDelegate glWindowPos2s = null;

        public static void WindowPos2s(short x, short y) { glWindowPos2s(x, y); }

        [MonoNativeFunctionWrapper]
        private delegate void glWindowPos2dvDelegate(IntPtr p);
        [Require(MinVersion = "1.4")]
        private static glWindowPos2dvDelegate glWindowPos2dv = null;

        public static void WindowPos2dv(double* p) { glWindowPos2dv((IntPtr)p); }

        [MonoNativeFunctionWrapper]
        private delegate void glWindowPos2fvDelegate(IntPtr p);
        [Require(MinVersion = "1.4")]
        private static glWindowPos2fvDelegate glWindowPos2fv = null;

        public static void WindowPos2fv(float* p) { glWindowPos2fv((IntPtr)p); }

        [MonoNativeFunctionWrapper]
        private delegate void glWindowPos2ivDelegate(IntPtr p);
        [Require(MinVersion = "1.4")]
        private static glWindowPos2ivDelegate glWindowPos2iv = null;

        public static void WindowPos2iv(int* p) { glWindowPos2iv((IntPtr)p); }

        [MonoNativeFunctionWrapper]
        private delegate void glWindowPos2svDelegate(IntPtr p);
        [Require(MinVersion = "1.4")]
        private static glWindowPos2svDelegate glWindowPos2sv = null;

        public static void WindowPos2sv(short* p) { glWindowPos2sv((IntPtr)p); }

        [MonoNativeFunctionWrapper]
        private delegate void glWindowPos3dDelegate(double x, double y, double z);
        [Require(MinVersion = "1.4")]
        private static glWindowPos3dDelegate glWindowPos3d = null;

        public static void WindowPos3d(double x, double y, double z) { glWindowPos3d(x, y, z); }

        [MonoNativeFunctionWrapper]
        private delegate void glWindowPos3fDelegate(float x, float y, float z);
        [Require(MinVersion = "1.4")]
        private static glWindowPos3fDelegate glWindowPos3f = null;

        public static void WindowPos3f(float x, float y, float z) { glWindowPos3f(x, y, z); }

        [MonoNativeFunctionWrapper]
        private delegate void glWindowPos3iDelegate(int x, int y, int z);
        [Require(MinVersion = "1.4")]
        private static glWindowPos3iDelegate glWindowPos3i = null;

        public static void WindowPos3i(int x, int y, int z) { glWindowPos3i(x, y, z); }

        [MonoNativeFunctionWrapper]
        private delegate void glWindowPos3sDelegate(short x, short y, short z);
        [Require(MinVersion = "1.4")]
        private static glWindowPos3sDelegate glWindowPos3s = null;

        public static void WindowPos3s(short x, short y, short z) { glWindowPos3s(x, y, z); }

        [MonoNativeFunctionWrapper]
        private delegate void glWindowPos3dvDelegate(IntPtr p);
        [Require(MinVersion = "1.4")]
        private static glWindowPos3dvDelegate glWindowPos3dv = null;

        public static void WindowPos3dv(double* p) { glWindowPos3dv((IntPtr)p); }

        [MonoNativeFunctionWrapper]
        private delegate void glWindowPos3fvDelegate(IntPtr p);
        [Require(MinVersion = "1.4")]
        private static glWindowPos3fvDelegate glWindowPos3fv = null;

        public static void WindowPos3fv(float* p) { glWindowPos3fv((IntPtr)p); }

        [MonoNativeFunctionWrapper]
        private delegate void glWindowPos3ivDelegate(IntPtr p);
        [Require(MinVersion = "1.4")]
        private static glWindowPos3ivDelegate glWindowPos3iv = null;

        public static void WindowPos3iv(int* p) { glWindowPos3iv((IntPtr)p); }

        [MonoNativeFunctionWrapper]
        private delegate void glWindowPos3svDelegate(IntPtr p);
        [Require(MinVersion = "1.4")]
        private static glWindowPos3svDelegate glWindowPos3sv = null;

        public static void WindowPos3sv(short* p) { glWindowPos3sv((IntPtr)p); }

        public const UInt32 GL_BLEND_DST_RGB = 0x80C8;
        public const UInt32 GL_BLEND_SRC_RGB = 0x80C9;
        public const UInt32 GL_BLEND_DST_ALPHA = 0x80CA;
        public const UInt32 GL_BLEND_SRC_ALPHA = 0x80CB;
        public const UInt32 GL_POINT_SIZE_MIN = 0x8126;
        public const UInt32 GL_POINT_SIZE_MAX = 0x8127;
        public const UInt32 GL_POINT_FADE_THRESHOLD_SIZE = 0x8128;
        public const UInt32 GL_POINT_DISTANCE_ATTENUATION = 0x8129;
        public const UInt32 GL_GENERATE_MIPMAP = 0x8191;
        public const UInt32 GL_GENERATE_MIPMAP_HINT = 0x8192;
        public const UInt32 GL_DEPTH_COMPONENT16 = 0x81A5;
        public const UInt32 GL_DEPTH_COMPONENT24 = 0x81A6;
        public const UInt32 GL_DEPTH_COMPONENT32 = 0x81A7;
        public const UInt32 GL_MIRRORED_REPEAT = 0x8370;
        public const UInt32 GL_FOG_COORDINATE_SOURCE = 0x8450;
        public const UInt32 GL_FOG_COORDINATE = 0x8451;
        public const UInt32 GL_FRAGMENT_DEPTH = 0x8452;
        public const UInt32 GL_CURRENT_FOG_COORDINATE = 0x8453;
        public const UInt32 GL_FOG_COORDINATE_ARRAY_TYPE = 0x8454;
        public const UInt32 GL_FOG_COORDINATE_ARRAY_STRIDE = 0x8455;
        public const UInt32 GL_FOG_COORDINATE_ARRAY_POINTER = 0x8456;
        public const UInt32 GL_FOG_COORDINATE_ARRAY = 0x8457;
        public const UInt32 GL_COLOR_SUM = 0x8458;
        public const UInt32 GL_CURRENT_SECONDARY_COLOR = 0x8459;
        public const UInt32 GL_SECONDARY_COLOR_ARRAY_SIZE = 0x845A;
        public const UInt32 GL_SECONDARY_COLOR_ARRAY_TYPE = 0x845B;
        public const UInt32 GL_SECONDARY_COLOR_ARRAY_STRIDE = 0x845C;
        public const UInt32 GL_SECONDARY_COLOR_ARRAY_POINTER = 0x845D;
        public const UInt32 GL_SECONDARY_COLOR_ARRAY = 0x845E;
        public const UInt32 GL_MAX_TEXTURE_LOD_BIAS = 0x84FD;
        public const UInt32 GL_TEXTURE_FILTER_CONTROL = 0x8500;
        public const UInt32 GL_TEXTURE_LOD_BIAS = 0x8501;
        public const UInt32 GL_INCR_WRAP = 0x8507;
        public const UInt32 GL_DECR_WRAP = 0x8508;
        public const UInt32 GL_TEXTURE_DEPTH_SIZE = 0x884A;
        public const UInt32 GL_DEPTH_TEXTURE_MODE = 0x884B;
        public const UInt32 GL_TEXTURE_COMPARE_MODE = 0x884C;
        public const UInt32 GL_TEXTURE_COMPARE_FUNC = 0x884D;
        public const UInt32 GL_COMPARE_R_TO_TEXTURE = 0x884E;
    }
}
