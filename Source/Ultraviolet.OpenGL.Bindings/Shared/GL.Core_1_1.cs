using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;

namespace Ultraviolet.OpenGL.Bindings
{
    public static unsafe partial class GL
    {
        [MonoNativeFunctionWrapper]
        private delegate void glAccumDelegate(uint op, float value);
        [Require(MinVersion = "1.1")]
        private static glAccumDelegate glAccum = null;

        public static void Accum(uint op, float value) { glAccum(op, value); }

        [MonoNativeFunctionWrapper]
        private delegate void glAlphaFuncDelegate(uint func, float @ref);
        [Require(MinVersion = "1.1")]
        private static glAlphaFuncDelegate glAlphaFunc = null;

        public static void AlphaFunc(uint func, float @ref) { glAlphaFunc(func, @ref); }

        [MonoNativeFunctionWrapper]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool glAreTexturesResidentDelegate(int n, IntPtr textures, IntPtr residences);
        [Require(MinVersion = "1.1")]
        private static glAreTexturesResidentDelegate glAreTexturesResident = null;

        public static bool AreTexturesResident(int n, uint* textures, bool* residences) { return glAreTexturesResident(n, (IntPtr)textures, (IntPtr)residences); }

        [MonoNativeFunctionWrapper]
        private delegate void glArrayElementDelegate(int i);
        [Require(MinVersion = "1.1")]
        private static glArrayElementDelegate glArrayElement = null;

        public static void ArrayElement(int i) { glArrayElement(i); }

        [MonoNativeFunctionWrapper]
        private delegate void glBeginDelegate(uint mode);
        [Require(MinVersion = "1.1")]
        private static glBeginDelegate glBegin = null;

        public static void Begin(uint mode) { glBegin(mode); }

        [MonoNativeFunctionWrapper]
        private delegate void glBindTextureDelegate(uint target, uint texture);
        [Require(MinVersion = "1.1")]
        private static glBindTextureDelegate glBindTexture = null;

        public static void BindTexture(uint target, uint texture) { glBindTexture(target, texture); }

        [MonoNativeFunctionWrapper]
        private delegate void glBitmapDelegate(int width, int height, float xorig, float yorig, float xmove, float ymove, IntPtr bitmap);
        [Require(MinVersion = "1.1")]
        private static glBitmapDelegate glBitmap = null;

        public static void Bitmap(int width, int height, float xorig, float yorig, float xmove, float ymove, byte* bitmap) { glBitmap(width, height, xorig, yorig, xmove, ymove, (IntPtr)bitmap); }

        [MonoNativeFunctionWrapper]
        private delegate void glBlendFuncDelegate(uint sfactor, uint dfactor);
        [Require(MinVersion = "1.1")]
        private static glBlendFuncDelegate glBlendFunc = null;

        public static void BlendFunc(uint sfactor, uint dfactor) { glBlendFunc(sfactor, dfactor); }

        [MonoNativeFunctionWrapper]
        private delegate void glCallListDelegate(uint list);
        [Require(MinVersion = "1.1")]
        private static glCallListDelegate glCallList = null;

        public static void CallList(uint list) { glCallList(list); }

        [MonoNativeFunctionWrapper]
        private delegate void glCallListsDelegate(int n, uint type, IntPtr lists);
        [Require(MinVersion = "1.1")]
        private static glCallListsDelegate glCallLists = null;

        public static void CallLists(int n, uint type, void* lists) { glCallLists(n, type, (IntPtr)lists); }

        [MonoNativeFunctionWrapper]
        private delegate void glClearDelegate(uint mask);
        [Require(MinVersion = "1.1")]
        private static glClearDelegate glClear = null;

        public static void Clear(uint mask) { glClear(mask); }

        [MonoNativeFunctionWrapper]
        private delegate void glClearAccumDelegate(float red, float green, float blue, float alpha);
        [Require(MinVersion = "1.1")]
        private static glClearAccumDelegate glClearAccum = null;

        public static void ClearAccum(float red, float green, float blue, float alpha) { glClearAccum(red, green, blue, alpha); }

        [MonoNativeFunctionWrapper]
        private delegate void glClearColorDelegate(float red, float green, float blue, float alpha);
        [Require(MinVersion = "1.1")]
        private static glClearColorDelegate glClearColor = null;

        public static void ClearColor(float red, float green, float blue, float alpha) { glClearColor(red, green, blue, alpha); }

        [MonoNativeFunctionWrapper]
        private delegate void glClearDepthDelegate(double depth);
        [Require(MinVersion = "1.1")]
        private static glClearDepthDelegate glClearDepth = null;

        [MonoNativeFunctionWrapper]
        private delegate void glClearDepthfDelegate(float depth);
        [Require(MinVersion = "4.1", MinVersionES = "2.0")]
        private static glClearDepthfDelegate glClearDepthf = null;

        public static void ClearDepth(double depth) 
        {
            if (isGLES)
            {
                glClearDepthf((float)depth);
            }
            else
            {
                glClearDepth(depth);
            }
        }

        [MonoNativeFunctionWrapper]
        private delegate void glClearIndexDelegate(float c);
        [Require(MinVersion = "1.1")]
        private static glClearIndexDelegate glClearIndex = null;

        public static void ClearIndex(float c) { glClearIndex(c); }

        [MonoNativeFunctionWrapper]
        private delegate void glClearStencilDelegate(int s);
        [Require(MinVersion = "1.1")]
        private static glClearStencilDelegate glClearStencil = null;

        public static void ClearStencil(int s) { glClearStencil(s); }

        [MonoNativeFunctionWrapper]
        private delegate void glClipPlaneDelegate(uint plane, IntPtr equation);
        [Require(MinVersion = "1.1")]
        private static glClipPlaneDelegate glClipPlane = null;

        public static void ClipPlane(uint plane, double* equation) { glClipPlane(plane, (IntPtr)equation); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor3bDelegate(sbyte red, sbyte green, sbyte blue);
        [Require(MinVersion = "1.1")]
        private static glColor3bDelegate glColor3b = null;

        public static void Color3b(sbyte red, sbyte green, sbyte blue) { glColor3b(red, green, blue); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor3bvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glColor3bvDelegate glColor3bv = null;

        public static void Color3bv(sbyte* v) { glColor3bv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor3dDelegate(double red, double green, double blue);
        [Require(MinVersion = "1.1")]
        private static glColor3dDelegate glColor3d = null;

        public static void Color3d(double red, double green, double blue) { glColor3d(red, green, blue); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor3dvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glColor3dvDelegate glColor3dv = null;

        public static void Color3dv(double* v) { glColor3dv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor3fDelegate(float red, float green, float blue);
        [Require(MinVersion = "1.1")]
        private static glColor3fDelegate glColor3f = null;

        public static void Color3f(float red, float green, float blue) { glColor3f(red, green, blue); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor3fvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glColor3fvDelegate glColor3fv = null;

        public static void Color3fv(float* v) { glColor3fv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor3iDelegate(int red, int green, int blue);
        [Require(MinVersion = "1.1")]
        private static glColor3iDelegate glColor3i = null;

        public static void Color3i(int red, int green, int blue) { glColor3i(red, green, blue); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor3ivDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glColor3ivDelegate glColor3iv = null;

        public static void Color3iv(int* v) { glColor3iv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor3sDelegate(short red, short green, short blue);
        [Require(MinVersion = "1.1")]
        private static glColor3sDelegate glColor3s = null;

        public static void Color3s(short red, short green, short blue) { glColor3s(red, green, blue); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor3svDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glColor3svDelegate glColor3sv = null;

        public static void Color3sv(short* v) { glColor3sv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor3ubDelegate(byte red, byte green, byte blue);
        [Require(MinVersion = "1.1")]
        private static glColor3ubDelegate glColor3ub = null;

        public static void Color3ub(byte red, byte green, byte blue) { glColor3ub(red, green, blue); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor3ubvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glColor3ubvDelegate glColor3ubv = null;

        public static void Color3ubv(byte* v) { glColor3ubv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor3uiDelegate(uint red, uint green, uint blue);
        [Require(MinVersion = "1.1")]
        private static glColor3uiDelegate glColor3ui = null;

        public static void Color3ui(uint red, uint green, uint blue) { glColor3ui(red, green, blue); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor3uivDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glColor3uivDelegate glColor3uiv = null;

        public static void Color3uiv(uint* v) { glColor3uiv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor3usDelegate(ushort red, ushort green, ushort blue);
        [Require(MinVersion = "1.1")]
        private static glColor3usDelegate glColor3us = null;

        public static void Color3us(ushort red, ushort green, ushort blue) { glColor3us(red, green, blue); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor3usvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glColor3usvDelegate glColor3usv = null;

        public static void Color3usv(ushort* v) { glColor3usv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor4bDelegate(sbyte red, sbyte green, sbyte blue, sbyte alpha);
        [Require(MinVersion = "1.1")]
        private static glColor4bDelegate glColor4b = null;

        public static void Color4b(sbyte red, sbyte green, sbyte blue, sbyte alpha) { glColor4b(red, green, blue, alpha); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor4bvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glColor4bvDelegate glColor4bv = null;

        public static void Color4bv(sbyte* v) { glColor4bv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor4dDelegate(double red, double green, double blue, double alpha);
        [Require(MinVersion = "1.1")]
        private static glColor4dDelegate glColor4d = null;

        public static void Color4d(double red, double green, double blue, double alpha) { glColor4d(red, green, blue, alpha); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor4dvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glColor4dvDelegate glColor4dv = null;

        public static void Color4dv(double* v) { glColor4dv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor4fDelegate(float red, float green, float blue, float alpha);
        [Require(MinVersion = "1.1")]
        private static glColor4fDelegate glColor4f = null;

        public static void Color4f(float red, float green, float blue, float alpha) { glColor4f(red, green, blue, alpha); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor4fvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glColor4fvDelegate glColor4fv = null;

        public static void Color4fv(float* v) { glColor4fv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor4iDelegate(int red, int green, int blue, int alpha);
        [Require(MinVersion = "1.1")]
        private static glColor4iDelegate glColor4i = null;

        public static void Color4i(int red, int green, int blue, int alpha) { glColor4i(red, green, blue, alpha); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor4ivDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glColor4ivDelegate glColor4iv = null;

        public static void Color4iv(int* v) { glColor4iv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor4sDelegate(short red, short green, short blue, short alpha);
        [Require(MinVersion = "1.1")]
        private static glColor4sDelegate glColor4s = null;

        public static void Color4s(short red, short green, short blue, short alpha) { glColor4s(red, green, blue, alpha); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor4svDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glColor4svDelegate glColor4sv = null;

        public static void Color4sv(short* v) { glColor4sv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor4ubDelegate(byte red, byte green, byte blue, byte alpha);
        [Require(MinVersion = "1.1")]
        private static glColor4ubDelegate glColor4ub = null;

        public static void Color4ub(byte red, byte green, byte blue, byte alpha) { glColor4ub(red, green, blue, alpha); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor4ubvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glColor4ubvDelegate glColor4ubv = null;

        public static void Color4ubv(byte* v) { glColor4ubv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor4uiDelegate(uint red, uint green, uint blue, uint alpha);
        [Require(MinVersion = "1.1")]
        private static glColor4uiDelegate glColor4ui = null;

        public static void Color4ui(uint red, uint green, uint blue, uint alpha) { glColor4ui(red, green, blue, alpha); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor4uivDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glColor4uivDelegate glColor4uiv = null;

        public static void Color4uiv(uint* v) { glColor4uiv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor4usDelegate(ushort red, ushort green, ushort blue, ushort alpha);
        [Require(MinVersion = "1.1")]
        private static glColor4usDelegate glColor4us = null;

        public static void Color4us(ushort red, ushort green, ushort blue, ushort alpha) { glColor4us(red, green, blue, alpha); }

        [MonoNativeFunctionWrapper]
        private delegate void glColor4usvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glColor4usvDelegate glColor4usv = null;

        public static void Color4usv(ushort* v) { glColor4usv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glColorMaskDelegate([MarshalAs(UnmanagedType.I1)] bool red, [MarshalAs(UnmanagedType.I1)] bool green, [MarshalAs(UnmanagedType.I1)] bool blue, [MarshalAs(UnmanagedType.I1)] bool alpha);
        [Require(MinVersion = "1.1")]
        private static glColorMaskDelegate glColorMask = null;

        public static void ColorMask(bool red, bool green, bool blue, bool alpha) { glColorMask(red, green, blue, alpha); }

        [MonoNativeFunctionWrapper]
        private delegate void glColorMaterialDelegate(uint face, uint mode);
        [Require(MinVersion = "1.1")]
        private static glColorMaterialDelegate glColorMaterial = null;

        public static void ColorMaterial(uint face, uint mode) { glColorMaterial(face, mode); }

        [MonoNativeFunctionWrapper]
        private delegate void glColorPointerDelegate(int size, uint type, int stride, IntPtr pointer);
        [Require(MinVersion = "1.1")]
        private static glColorPointerDelegate glColorPointer = null;

        public static void ColorPointer(int size, uint type, int stride, void* pointer) { glColorPointer(size, type, stride, (IntPtr)pointer); }

        [MonoNativeFunctionWrapper]
        private delegate void glCopyPixelsDelegate(int x, int y, int width, int height, uint type);
        [Require(MinVersion = "1.1")]
        private static glCopyPixelsDelegate glCopyPixels = null;

        public static void CopyPixels(int x, int y, int width, int height, uint type) { glCopyPixels(x, y, width, height, type); }

        [MonoNativeFunctionWrapper]
        private delegate void glCopyTexImage1DDelegate(uint target, int level, uint internalFormat, int x, int y, int width, int border);
        [Require(MinVersion = "1.1")]
        private static glCopyTexImage1DDelegate glCopyTexImage1D = null;

        public static void CopyTexImage1D(uint target, int level, uint internalFormat, int x, int y, int width, int border) { glCopyTexImage1D(target, level, internalFormat, x, y, width, border); }

        [MonoNativeFunctionWrapper]
        private delegate void glCopyTexImage2DDelegate(uint target, int level, uint internalFormat, int x, int y, int width, int height, int border);
        [Require(MinVersion = "1.1")]
        private static glCopyTexImage2DDelegate glCopyTexImage2D = null;

        public static void CopyTexImage2D(uint target, int level, uint internalFormat, int x, int y, int width, int height, int border) { glCopyTexImage2D(target, level, internalFormat, x, y, width, height, border); }

        [MonoNativeFunctionWrapper]
        private delegate void glCopyTexSubImage1DDelegate(uint target, int level, int xoffset, int x, int y, int width);
        [Require(MinVersion = "1.1")]
        private static glCopyTexSubImage1DDelegate glCopyTexSubImage1D = null;

        public static void CopyTexSubImage1D(uint target, int level, int xoffset, int x, int y, int width) { glCopyTexSubImage1D(target, level, xoffset, x, y, width); }

        [MonoNativeFunctionWrapper]
        private delegate void glCopyTexSubImage2DDelegate(uint target, int level, int xoffset, int yoffset, int x, int y, int width, int height);
        [Require(MinVersion = "1.1")]
        private static glCopyTexSubImage2DDelegate glCopyTexSubImage2D = null;

        public static void CopyTexSubImage2D(uint target, int level, int xoffset, int yoffset, int x, int y, int width, int height) { glCopyTexSubImage2D(target, level, xoffset, yoffset, x, y, width, height); }

        [MonoNativeFunctionWrapper]
        private delegate void glCullFaceDelegate(uint mode);
        [Require(MinVersion = "1.1")]
        private static glCullFaceDelegate glCullFace = null;

        public static void CullFace(uint mode) { glCullFace(mode); }

        [MonoNativeFunctionWrapper]
        private delegate void glDeleteListsDelegate(uint list, int range);
        [Require(MinVersion = "1.1")]
        private static glDeleteListsDelegate glDeleteLists = null;

        public static void DeleteLists(uint list, int range) { glDeleteLists(list, range); }

        [MonoNativeFunctionWrapper]
        private delegate void glDeleteTexturesDelegate(int n, IntPtr textures);
        [Require(MinVersion = "1.1")]
        private static glDeleteTexturesDelegate glDeleteTextures = null;

        public static void DeleteTextures(int n, uint* textures) { glDeleteTextures(n, (IntPtr)textures); }

        public static void DeleteTextures(uint[] textures)
        {
            fixed (uint* ptextures = textures)
            {
                glDeleteTextures(textures.Length, (IntPtr)ptextures);
            }
        }

        public static void DeleteTexture(uint texture)
        {
            glDeleteTextures(1, (IntPtr)(&texture));
        }

        [MonoNativeFunctionWrapper]
        private delegate void glDepthFuncDelegate(uint func);
        [Require(MinVersion = "1.1")]
        private static glDepthFuncDelegate glDepthFunc = null;

        public static void DepthFunc(uint func) { glDepthFunc(func); }

        [MonoNativeFunctionWrapper]
        private delegate void glDepthMaskDelegate([MarshalAs(UnmanagedType.I1)] bool flag);
        [Require(MinVersion = "1.1")]
        private static glDepthMaskDelegate glDepthMask = null;

        public static void DepthMask(bool flag) { glDepthMask(flag); }

        [MonoNativeFunctionWrapper]
        private delegate void glDepthRangeDelegate(double zNear, double zFar);
        [Require(MinVersion = "1.1")]
        private static glDepthRangeDelegate glDepthRange = null;

        public static void DepthRange(double zNear, double zFar) { glDepthRange(zNear, zFar); }

        [MonoNativeFunctionWrapper]
        private delegate void glDisableDelegate(uint cap);
        [Require(MinVersion = "1.1")]
        private static glDisableDelegate glDisable = null;

        public static void Disable(uint cap) { glDisable(cap); }

        [MonoNativeFunctionWrapper]
        private delegate void glDisableClientStateDelegate(uint array);
        [Require(MinVersion = "1.1")]
        private static glDisableClientStateDelegate glDisableClientState = null;

        public static void DisableClientState(uint array) { glDisableClientState(array); }

        [MonoNativeFunctionWrapper]
        private delegate void glDrawArraysDelegate(uint mode, int first, int count);
        [Require(MinVersion = "1.1")]
        private static glDrawArraysDelegate glDrawArrays = null;

        public static void DrawArrays(uint mode, int first, int count) { glDrawArrays(mode, first, count); }

        [MonoNativeFunctionWrapper]
        private delegate void glDrawBufferDelegate(uint mode);
        [Require(MinVersion = "1.1")]
        private static glDrawBufferDelegate glDrawBuffer = null;

        public static void DrawBuffer(uint mode) { glDrawBuffer(mode); }

        [MonoNativeFunctionWrapper]
        private delegate void glDrawElementsDelegate(uint mode, int count, uint type, IntPtr indices);
        [Require(MinVersion = "1.1")]
        private static glDrawElementsDelegate glDrawElements = null;

        public static void DrawElements(uint mode, int count, uint type, void* indices) { glDrawElements(mode, count, type, (IntPtr)indices); }

        [MonoNativeFunctionWrapper]
        private delegate void glDrawPixelsDelegate(int width, int height, uint format, uint type, IntPtr pixels);
        [Require(MinVersion = "1.1")]
        private static glDrawPixelsDelegate glDrawPixels = null;

        public static void DrawPixels(int width, int height, uint format, uint type, void* pixels) { glDrawPixels(width, height, format, type, (IntPtr)pixels); }

        [MonoNativeFunctionWrapper]
        private delegate void glEdgeFlagDelegate([MarshalAs(UnmanagedType.I1)] bool flag);
        [Require(MinVersion = "1.1")]
        private static glEdgeFlagDelegate glEdgeFlag = null;

        public static void EdgeFlag(bool flag) { glEdgeFlag(flag); }

        [MonoNativeFunctionWrapper]
        private delegate void glEdgeFlagPointerDelegate(int stride, IntPtr pointer);
        [Require(MinVersion = "1.1")]
        private static glEdgeFlagPointerDelegate glEdgeFlagPointer = null;

        public static void EdgeFlagPointer(int stride, void* pointer) { glEdgeFlagPointer(stride, (IntPtr)pointer); }

        [MonoNativeFunctionWrapper]
        private delegate void glEdgeFlagvDelegate(IntPtr flag);
        [Require(MinVersion = "1.1")]
        private static glEdgeFlagvDelegate glEdgeFlagv = null;

        public static void EdgeFlagv(bool* flag) { glEdgeFlagv((IntPtr)flag); }

        [MonoNativeFunctionWrapper]
        private delegate void glEnableDelegate(uint cap);
        [Require(MinVersion = "1.1")]
        private static glEnableDelegate glEnable = null;

        public static void Enable(uint cap) { glEnable(cap); }

        public static void Enable(uint cap, bool enabled)
        {
            if (enabled)
            {
                glEnable(cap);
            }
            else
            {
                glDisable(cap);
            }
        }

        [MonoNativeFunctionWrapper]
        private delegate void glEnableClientStateDelegate(uint array);
        [Require(MinVersion = "1.1")]
        private static glEnableClientStateDelegate glEnableClientState = null;

        public static void EnableClientState(uint array) { glEnableClientState(array); }

        [MonoNativeFunctionWrapper]
        private delegate void glEndDelegate();
        [MonoNativeFunctionWrapper]
        private delegate void glEndListDelegate();
        [MonoNativeFunctionWrapper]
        private delegate void glEvalCoord1dDelegate(double u);
        [Require(MinVersion = "1.1")]
        private static glEvalCoord1dDelegate glEvalCoord1d = null;

        public static void EvalCoord1d(double u) { glEvalCoord1d(u); }

        [MonoNativeFunctionWrapper]
        private delegate void glEvalCoord1dvDelegate(IntPtr u);
        [Require(MinVersion = "1.1")]
        private static glEvalCoord1dvDelegate glEvalCoord1dv = null;

        public static void EvalCoord1dv(double* u) { glEvalCoord1dv((IntPtr)u); }

        [MonoNativeFunctionWrapper]
        private delegate void glEvalCoord1fDelegate(float u);
        [Require(MinVersion = "1.1")]
        private static glEvalCoord1fDelegate glEvalCoord1f = null;

        public static void EvalCoord1f(float u) { glEvalCoord1f(u); }

        [MonoNativeFunctionWrapper]
        private delegate void glEvalCoord1fvDelegate(IntPtr u);
        [Require(MinVersion = "1.1")]
        private static glEvalCoord1fvDelegate glEvalCoord1fv = null;

        public static void EvalCoord1fv(float* u) { glEvalCoord1fv((IntPtr)u); }

        [MonoNativeFunctionWrapper]
        private delegate void glEvalCoord2dDelegate(double u, double v);
        [Require(MinVersion = "1.1")]
        private static glEvalCoord2dDelegate glEvalCoord2d = null;

        public static void EvalCoord2d(double u, double v) { glEvalCoord2d(u, v); }

        [MonoNativeFunctionWrapper]
        private delegate void glEvalCoord2dvDelegate(IntPtr u);
        [Require(MinVersion = "1.1")]
        private static glEvalCoord2dvDelegate glEvalCoord2dv = null;

        public static void EvalCoord2dv(double* u) { glEvalCoord2dv((IntPtr)u); }

        [MonoNativeFunctionWrapper]
        private delegate void glEvalCoord2fDelegate(float u, float v);
        [Require(MinVersion = "1.1")]
        private static glEvalCoord2fDelegate glEvalCoord2f = null;

        public static void EvalCoord2f(float u, float v) { glEvalCoord2f(u, v); }

        [MonoNativeFunctionWrapper]
        private delegate void glEvalCoord2fvDelegate(IntPtr u);
        [Require(MinVersion = "1.1")]
        private static glEvalCoord2fvDelegate glEvalCoord2fv = null;

        public static void EvalCoord2fv(float* u) { glEvalCoord2fv((IntPtr)u); }

        [MonoNativeFunctionWrapper]
        private delegate void glEvalMesh1Delegate(uint mode, int i1, int i2);
        [Require(MinVersion = "1.1")]
        private static glEvalMesh1Delegate glEvalMesh1 = null;

        public static void EvalMesh1(uint mode, int i1, int i2) { glEvalMesh1(mode, i1, i2); }

        [MonoNativeFunctionWrapper]
        private delegate void glEvalMesh2Delegate(uint mode, int i1, int i2, int j1, int j2);
        [Require(MinVersion = "1.1")]
        private static glEvalMesh2Delegate glEvalMesh2 = null;

        public static void EvalMesh2(uint mode, int i1, int i2, int j1, int j2) { glEvalMesh2(mode, i1, i2, j1, j2); }

        [MonoNativeFunctionWrapper]
        private delegate void glEvalPoint1Delegate(int i);
        [Require(MinVersion = "1.1")]
        private static glEvalPoint1Delegate glEvalPoint1 = null;

        public static void EvalPoint1(int i) { glEvalPoint1(i); }

        [MonoNativeFunctionWrapper]
        private delegate void glEvalPoint2Delegate(int i, int j);
        [Require(MinVersion = "1.1")]
        private static glEvalPoint2Delegate glEvalPoint2 = null;

        public static void EvalPoint2(int i, int j) { glEvalPoint2(i, j); }

        [MonoNativeFunctionWrapper]
        private delegate void glFeedbackBufferDelegate(int size, uint type, IntPtr buffer);
        [Require(MinVersion = "1.1")]
        private static glFeedbackBufferDelegate glFeedbackBuffer = null;

        public static void FeedbackBuffer(int size, uint type, float* buffer) { glFeedbackBuffer(size, type, (IntPtr)buffer); }

        [MonoNativeFunctionWrapper]
        private delegate void glFinishDelegate();
        [MonoNativeFunctionWrapper]
        private delegate void glFlushDelegate();
        [MonoNativeFunctionWrapper]
        private delegate void glFogfDelegate(uint pname, float param);
        [Require(MinVersion = "1.1")]
        private static glFogfDelegate glFogf = null;

        public static void Fogf(uint pname, float param) { glFogf(pname, param); }

        [MonoNativeFunctionWrapper]
        private delegate void glFogfvDelegate(uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glFogfvDelegate glFogfv = null;

        public static void Fogfv(uint pname, float* @params) { glFogfv(pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glFogiDelegate(uint pname, int param);
        [Require(MinVersion = "1.1")]
        private static glFogiDelegate glFogi = null;

        public static void Fogi(uint pname, int param) { glFogi(pname, param); }

        [MonoNativeFunctionWrapper]
        private delegate void glFogivDelegate(uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glFogivDelegate glFogiv = null;

        public static void Fogiv(uint pname, int* @params) { glFogiv(pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glFrontFaceDelegate(uint mode);
        [Require(MinVersion = "1.1")]
        private static glFrontFaceDelegate glFrontFace = null;

        public static void FrontFace(uint mode) { glFrontFace(mode); }

        [MonoNativeFunctionWrapper]
        private delegate void glFrustumDelegate(double left, double right, double bottom, double top, double zNear, double zFar);
        [Require(MinVersion = "1.1")]
        private static glFrustumDelegate glFrustum = null;

        public static void Frustum(double left, double right, double bottom, double top, double zNear, double zFar) { glFrustum(left, right, bottom, top, zNear, zFar); }

        [MonoNativeFunctionWrapper]
        private delegate uint glGenListsDelegate(int range);
        [Require(MinVersion = "1.1")]
        private static glGenListsDelegate glGenLists = null;

        public static uint GenLists(int range) { return glGenLists(range); }

        [MonoNativeFunctionWrapper]
        private delegate void glGenTexturesDelegate(int n, IntPtr textures);
        [Require(MinVersion = "1.1")]
        private static glGenTexturesDelegate glGenTextures = null;

        public static void GenTextures(int n, uint* textures) { glGenTextures(n, (IntPtr)textures); }

        public static void GenTextures(uint[] textures)
        {
            fixed (uint* ptextures = textures)
            {
                glGenTextures(textures.Length, (IntPtr)ptextures);
            }
        }

        public static uint GenTexture()
        {
            uint value;
            glGenTextures(1, (IntPtr)(&value));
            return value;
        }

        [MonoNativeFunctionWrapper]
        private delegate void glGetBooleanvDelegate(uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glGetBooleanvDelegate glGetBooleanv = null;

        public static void GetBooleanv(uint pname, bool* @params) { glGetBooleanv(pname, (IntPtr)@params); }

        public static bool GetBoolean(uint pname)
        {
            bool value;
            glGetBooleanv(pname, (IntPtr)(&value));
            return value;
        }

        [MonoNativeFunctionWrapper]
        private delegate void glGetClipPlaneDelegate(uint plane, IntPtr equation);
        [Require(MinVersion = "1.1")]
        private static glGetClipPlaneDelegate glGetClipPlane = null;

        public static void GetClipPlane(uint plane, double* equation) { glGetClipPlane(plane, (IntPtr)equation); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetDoublevDelegate(uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glGetDoublevDelegate glGetDoublev = null;

        public static void GetDoublev(uint pname, double* @params) { glGetDoublev(pname, (IntPtr)@params); }

        public static double GetDouble(uint pname)
        {
            double value;
            glGetDoublev(pname, (IntPtr)(&value));
            return value;
        }

        [MonoNativeFunctionWrapper]
        private delegate uint glGetErrorDelegate();
        [Require(MinVersion = "1.1")]
        private static glGetErrorDelegate glGetError = null;

        public static uint GetError() { return glGetError(); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetFloatvDelegate(uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glGetFloatvDelegate glGetFloatv = null;

        public static void GetFloatv(uint pname, float* @params) { glGetFloatv(pname, (IntPtr)@params); }

        public static float GetFloat(uint pname)
        {
            float value;
            glGetFloatv(pname, (IntPtr)(&value));
            return value;
        }

        [MonoNativeFunctionWrapper]
        private delegate void glGetIntegervDelegate(uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glGetIntegervDelegate glGetIntegerv = null;

        public static void GetIntegerv(uint pname, int* @params) { glGetIntegerv(pname, (IntPtr)@params); }

        public static int GetInteger(uint pname)
        {
            int value;
            glGetIntegerv(pname, (IntPtr)(&value));
            return value;
        }

        [MonoNativeFunctionWrapper]
        private delegate void glGetLightfvDelegate(uint light, uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glGetLightfvDelegate glGetLightfv = null;

        public static void GetLightfv(uint light, uint pname, float* @params) { glGetLightfv(light, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetLightivDelegate(uint light, uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glGetLightivDelegate glGetLightiv = null;

        public static void GetLightiv(uint light, uint pname, int* @params) { glGetLightiv(light, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetMapdvDelegate(uint target, uint query, IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glGetMapdvDelegate glGetMapdv = null;

        public static void GetMapdv(uint target, uint query, double* v) { glGetMapdv(target, query, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetMapfvDelegate(uint target, uint query, IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glGetMapfvDelegate glGetMapfv = null;

        public static void GetMapfv(uint target, uint query, float* v) { glGetMapfv(target, query, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetMapivDelegate(uint target, uint query, IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glGetMapivDelegate glGetMapiv = null;

        public static void GetMapiv(uint target, uint query, int* v) { glGetMapiv(target, query, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetMaterialfvDelegate(uint face, uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glGetMaterialfvDelegate glGetMaterialfv = null;

        public static void GetMaterialfv(uint face, uint pname, float* @params) { glGetMaterialfv(face, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetMaterialivDelegate(uint face, uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glGetMaterialivDelegate glGetMaterialiv = null;

        public static void GetMaterialiv(uint face, uint pname, int* @params) { glGetMaterialiv(face, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetPixelMapfvDelegate(uint map, IntPtr values);
        [Require(MinVersion = "1.1")]
        private static glGetPixelMapfvDelegate glGetPixelMapfv = null;

        public static void GetPixelMapfv(uint map, float* values) { glGetPixelMapfv(map, (IntPtr)values); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetPixelMapuivDelegate(uint map, IntPtr values);
        [Require(MinVersion = "1.1")]
        private static glGetPixelMapuivDelegate glGetPixelMapuiv = null;

        public static void GetPixelMapuiv(uint map, uint* values) { glGetPixelMapuiv(map, (IntPtr)values); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetPixelMapusvDelegate(uint map, IntPtr values);
        [Require(MinVersion = "1.1")]
        private static glGetPixelMapusvDelegate glGetPixelMapusv = null;

        public static void GetPixelMapusv(uint map, ushort* values) { glGetPixelMapusv(map, (IntPtr)values); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetPointervDelegate(uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glGetPointervDelegate glGetPointerv = null;

        public static void GetPointerv(uint pname, void** @params) { glGetPointerv(pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetPolygonStippleDelegate(IntPtr mask);
        [Require(MinVersion = "1.1")]
        private static glGetPolygonStippleDelegate glGetPolygonStipple = null;

        public static void GetPolygonStipple(byte* mask) { glGetPolygonStipple((IntPtr)mask); }

        [MonoNativeFunctionWrapper]
        private delegate IntPtr glGetStringDelegate(uint name);
        [Require(MinVersion = "1.1")]
        private static glGetStringDelegate glGetString = null;

        public static String GetString(uint name)
        {
            return Marshal.PtrToStringAnsi((IntPtr)glGetString(name));
        }

        [MonoNativeFunctionWrapper]
        private delegate void glGetTexEnvfvDelegate(uint target, uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glGetTexEnvfvDelegate glGetTexEnvfv = null;

        public static void GetTexEnvfv(uint target, uint pname, float* @params) { glGetTexEnvfv(target, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetTexEnvivDelegate(uint target, uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glGetTexEnvivDelegate glGetTexEnviv = null;

        public static void GetTexEnviv(uint target, uint pname, int* @params) { glGetTexEnviv(target, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetTexGendvDelegate(uint coord, uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glGetTexGendvDelegate glGetTexGendv = null;

        public static void GetTexGendv(uint coord, uint pname, double* @params) { glGetTexGendv(coord, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetTexGenfvDelegate(uint coord, uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glGetTexGenfvDelegate glGetTexGenfv = null;

        public static void GetTexGenfv(uint coord, uint pname, float* @params) { glGetTexGenfv(coord, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetTexGenivDelegate(uint coord, uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glGetTexGenivDelegate glGetTexGeniv = null;

        public static void GetTexGeniv(uint coord, uint pname, int* @params) { glGetTexGeniv(coord, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetTexImageDelegate(uint target, int level, uint format, uint type, IntPtr pixels);
        [Require(MinVersion = "1.1")]
        private static glGetTexImageDelegate glGetTexImage = null;

        public static void GetTexImage(uint target, int level, uint format, uint type, void* pixels) { glGetTexImage(target, level, format, type, (IntPtr)pixels); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetTexLevelParameterfvDelegate(uint target, int level, uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glGetTexLevelParameterfvDelegate glGetTexLevelParameterfv = null;

        public static void GetTexLevelParameterfv(uint target, int level, uint pname, float* @params) { glGetTexLevelParameterfv(target, level, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetTexLevelParameterivDelegate(uint target, int level, uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glGetTexLevelParameterivDelegate glGetTexLevelParameteriv = null;

        public static void GetTexLevelParameteriv(uint target, int level, uint pname, int* @params) { glGetTexLevelParameteriv(target, level, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetTexParameterfvDelegate(uint target, uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glGetTexParameterfvDelegate glGetTexParameterfv = null;

        public static void GetTexParameterfv(uint target, uint pname, float* @params) { glGetTexParameterfv(target, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetTexParameterivDelegate(uint target, uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glGetTexParameterivDelegate glGetTexParameteriv = null;

        public static void GetTexParameteriv(uint target, uint pname, int* @params) { glGetTexParameteriv(target, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glHintDelegate(uint target, uint mode);
        [Require(MinVersion = "1.1")]
        private static glHintDelegate glHint = null;

        public static void Hint(uint target, uint mode) { glHint(target, mode); }

        [MonoNativeFunctionWrapper]
        private delegate void glIndexMaskDelegate(uint mask);
        [Require(MinVersion = "1.1")]
        private static glIndexMaskDelegate glIndexMask = null;

        public static void IndexMask(uint mask) { glIndexMask(mask); }

        [MonoNativeFunctionWrapper]
        private delegate void glIndexPointerDelegate(uint type, int stride, IntPtr pointer);
        [Require(MinVersion = "1.1")]
        private static glIndexPointerDelegate glIndexPointer = null;

        public static void IndexPointer(uint type, int stride, void* pointer) { glIndexPointer(type, stride, (IntPtr)pointer); }

        [MonoNativeFunctionWrapper]
        private delegate void glIndexdDelegate(double c);
        [Require(MinVersion = "1.1")]
        private static glIndexdDelegate glIndexd = null;

        public static void Indexd(double c) { glIndexd(c); }

        [MonoNativeFunctionWrapper]
        private delegate void glIndexdvDelegate(IntPtr c);
        [Require(MinVersion = "1.1")]
        private static glIndexdvDelegate glIndexdv = null;

        public static void Indexdv(double* c) { glIndexdv((IntPtr)c); }

        [MonoNativeFunctionWrapper]
        private delegate void glIndexfDelegate(float c);
        [Require(MinVersion = "1.1")]
        private static glIndexfDelegate glIndexf = null;

        public static void Indexf(float c) { glIndexf(c); }

        [MonoNativeFunctionWrapper]
        private delegate void glIndexfvDelegate(IntPtr c);
        [Require(MinVersion = "1.1")]
        private static glIndexfvDelegate glIndexfv = null;

        public static void Indexfv(float* c) { glIndexfv((IntPtr)c); }

        [MonoNativeFunctionWrapper]
        private delegate void glIndexiDelegate(int c);
        [Require(MinVersion = "1.1")]
        private static glIndexiDelegate glIndexi = null;

        public static void Indexi(int c) { glIndexi(c); }

        [MonoNativeFunctionWrapper]
        private delegate void glIndexivDelegate(IntPtr c);
        [Require(MinVersion = "1.1")]
        private static glIndexivDelegate glIndexiv = null;

        public static void Indexiv(int* c) { glIndexiv((IntPtr)c); }

        [MonoNativeFunctionWrapper]
        private delegate void glIndexsDelegate(short c);
        [Require(MinVersion = "1.1")]
        private static glIndexsDelegate glIndexs = null;

        public static void Indexs(short c) { glIndexs(c); }

        [MonoNativeFunctionWrapper]
        private delegate void glIndexsvDelegate(IntPtr c);
        [Require(MinVersion = "1.1")]
        private static glIndexsvDelegate glIndexsv = null;

        public static void Indexsv(short* c) { glIndexsv((IntPtr)c); }

        [MonoNativeFunctionWrapper]
        private delegate void glIndexubDelegate(byte c);
        [Require(MinVersion = "1.1")]
        private static glIndexubDelegate glIndexub = null;

        public static void Indexub(byte c) { glIndexub(c); }

        [MonoNativeFunctionWrapper]
        private delegate void glIndexubvDelegate(IntPtr c);
        [Require(MinVersion = "1.1")]
        private static glIndexubvDelegate glIndexubv = null;

        public static void Indexubv(byte* c) { glIndexubv((IntPtr)c); }

        [MonoNativeFunctionWrapper]
        private delegate void glInitNamesDelegate();
        [Require(MinVersion = "1.1")]
        private static glInitNamesDelegate glInitNames = null;

        public static void InitNames() { glInitNames(); }

        [MonoNativeFunctionWrapper]
        private delegate void glInterleavedArraysDelegate(uint format, int stride, IntPtr pointer);
        [Require(MinVersion = "1.1")]
        private static glInterleavedArraysDelegate glInterleavedArrays = null;

        public static void InterleavedArrays(uint format, int stride, void* pointer) { glInterleavedArrays(format, stride, (IntPtr)pointer); }

        [MonoNativeFunctionWrapper]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool glIsEnabledDelegate(uint cap);
        [Require(MinVersion = "1.1")]
        private static glIsEnabledDelegate glIsEnabled = null;

        public static bool IsEnabled(uint cap) { return glIsEnabled(cap); }

        [MonoNativeFunctionWrapper]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool glIsListDelegate(uint list);
        [Require(MinVersion = "1.1")]
        private static glIsListDelegate glIsList = null;

        public static bool IsList(uint list) { return glIsList(list); }

        [MonoNativeFunctionWrapper]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool glIsTextureDelegate(uint texture);
        [Require(MinVersion = "1.1")]
        private static glIsTextureDelegate glIsTexture = null;

        public static bool IsTexture(uint texture) { return glIsTexture(texture); }

        [MonoNativeFunctionWrapper]
        private delegate void glLightModelfDelegate(uint pname, float param);
        [Require(MinVersion = "1.1")]
        private static glLightModelfDelegate glLightModelf = null;

        public static void LightModelf(uint pname, float param) { glLightModelf(pname, param); }

        [MonoNativeFunctionWrapper]
        private delegate void glLightModelfvDelegate(uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glLightModelfvDelegate glLightModelfv = null;

        public static void LightModelfv(uint pname, float* @params) { glLightModelfv(pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glLightModeliDelegate(uint pname, int param);
        [Require(MinVersion = "1.1")]
        private static glLightModeliDelegate glLightModeli = null;

        public static void LightModeli(uint pname, int param) { glLightModeli(pname, param); }

        [MonoNativeFunctionWrapper]
        private delegate void glLightModelivDelegate(uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glLightModelivDelegate glLightModeliv = null;

        public static void LightModeliv(uint pname, int* @params) { glLightModeliv(pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glLightfDelegate(uint light, uint pname, float param);
        [Require(MinVersion = "1.1")]
        private static glLightfDelegate glLightf = null;

        public static void Lightf(uint light, uint pname, float param) { glLightf(light, pname, param); }

        [MonoNativeFunctionWrapper]
        private delegate void glLightfvDelegate(uint light, uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glLightfvDelegate glLightfv = null;

        public static void Lightfv(uint light, uint pname, float* @params) { glLightfv(light, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glLightiDelegate(uint light, uint pname, int param);
        [Require(MinVersion = "1.1")]
        private static glLightiDelegate glLighti = null;

        public static void Lighti(uint light, uint pname, int param) { glLighti(light, pname, param); }

        [MonoNativeFunctionWrapper]
        private delegate void glLightivDelegate(uint light, uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glLightivDelegate glLightiv = null;

        public static void Lightiv(uint light, uint pname, int* @params) { glLightiv(light, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glLineStippleDelegate(int factor, ushort pattern);
        [Require(MinVersion = "1.1")]
        private static glLineStippleDelegate glLineStipple = null;

        public static void LineStipple(int factor, ushort pattern) { glLineStipple(factor, pattern); }

        [MonoNativeFunctionWrapper]
        private delegate void glLineWidthDelegate(float width);
        [Require(MinVersion = "1.1")]
        private static glLineWidthDelegate glLineWidth = null;

        public static void LineWidth(float width) { glLineWidth(width); }

        [MonoNativeFunctionWrapper]
        private delegate void glListBaseDelegate(uint @base);
        [Require(MinVersion = "1.1")]
        private static glListBaseDelegate glListBase = null;

        public static void ListBase(uint @base) { glListBase(@base); }

        [MonoNativeFunctionWrapper]
        private delegate void glLoadIdentityDelegate();
        [MonoNativeFunctionWrapper]
        private delegate void glLoadMatrixdDelegate(IntPtr m);
        [Require(MinVersion = "1.1")]
        private static glLoadMatrixdDelegate glLoadMatrixd = null;

        public static void LoadMatrixd(double* m) { glLoadMatrixd((IntPtr)m); }

        [MonoNativeFunctionWrapper]
        private delegate void glLoadMatrixfDelegate(IntPtr m);
        [Require(MinVersion = "1.1")]
        private static glLoadMatrixfDelegate glLoadMatrixf = null;

        public static void LoadMatrixf(float* m) { glLoadMatrixf((IntPtr)m); }

        [MonoNativeFunctionWrapper]
        private delegate void glLoadNameDelegate(uint name);
        [Require(MinVersion = "1.1")]
        private static glLoadNameDelegate glLoadName = null;

        public static void LoadName(uint name) { glLoadName(name); }

        [MonoNativeFunctionWrapper]
        private delegate void glLogicOpDelegate(uint opcode);
        [Require(MinVersion = "1.1")]
        private static glLogicOpDelegate glLogicOp = null;

        public static void LogicOp(uint opcode) { glLogicOp(opcode); }

        [MonoNativeFunctionWrapper]
        private delegate void glMap1dDelegate(uint target, double u1, double u2, int stride, int order, IntPtr points);
        [Require(MinVersion = "1.1")]
        private static glMap1dDelegate glMap1d = null;

        public static void Map1d(uint target, double u1, double u2, int stride, int order, double* points) { glMap1d(target, u1, u2, stride, order, (IntPtr)points); }

        [MonoNativeFunctionWrapper]
        private delegate void glMap1fDelegate(uint target, float u1, float u2, int stride, int order, IntPtr points);
        [Require(MinVersion = "1.1")]
        private static glMap1fDelegate glMap1f = null;

        public static void Map1f(uint target, float u1, float u2, int stride, int order, float* points) { glMap1f(target, u1, u2, stride, order, (IntPtr)points); }

        [MonoNativeFunctionWrapper]
        private delegate void glMap2dDelegate(uint target, double u1, double u2, int ustride, int uorder, double v1, double v2, int vstride, int vorder, IntPtr points);
        [Require(MinVersion = "1.1")]
        private static glMap2dDelegate glMap2d = null;

        public static void Map2d(uint target, double u1, double u2, int ustride, int uorder, double v1, double v2, int vstride, int vorder, double* points) { glMap2d(target, u1, u2, ustride, uorder, v1, v2, vstride, vorder, (IntPtr)points); }

        [MonoNativeFunctionWrapper]
        private delegate void glMap2fDelegate(uint target, float u1, float u2, int ustride, int uorder, float v1, float v2, int vstride, int vorder, IntPtr points);
        [Require(MinVersion = "1.1")]
        private static glMap2fDelegate glMap2f = null;

        public static void Map2f(uint target, float u1, float u2, int ustride, int uorder, float v1, float v2, int vstride, int vorder, float* points) { glMap2f(target, u1, u2, ustride, uorder, v1, v2, vstride, vorder, (IntPtr)points); }

        [MonoNativeFunctionWrapper]
        private delegate void glMapGrid1dDelegate(int un, double u1, double u2);
        [Require(MinVersion = "1.1")]
        private static glMapGrid1dDelegate glMapGrid1d = null;

        public static void MapGrid1d(int un, double u1, double u2) { glMapGrid1d(un, u1, u2); }

        [MonoNativeFunctionWrapper]
        private delegate void glMapGrid1fDelegate(int un, float u1, float u2);
        [Require(MinVersion = "1.1")]
        private static glMapGrid1fDelegate glMapGrid1f = null;

        public static void MapGrid1f(int un, float u1, float u2) { glMapGrid1f(un, u1, u2); }

        [MonoNativeFunctionWrapper]
        private delegate void glMapGrid2dDelegate(int un, double u1, double u2, int vn, double v1, double v2);
        [Require(MinVersion = "1.1")]
        private static glMapGrid2dDelegate glMapGrid2d = null;

        public static void MapGrid2d(int un, double u1, double u2, int vn, double v1, double v2) { glMapGrid2d(un, u1, u2, vn, v1, v2); }

        [MonoNativeFunctionWrapper]
        private delegate void glMapGrid2fDelegate(int un, float u1, float u2, int vn, float v1, float v2);
        [Require(MinVersion = "1.1")]
        private static glMapGrid2fDelegate glMapGrid2f = null;

        public static void MapGrid2f(int un, float u1, float u2, int vn, float v1, float v2) { glMapGrid2f(un, u1, u2, vn, v1, v2); }

        [MonoNativeFunctionWrapper]
        private delegate void glMaterialfDelegate(uint face, uint pname, float param);
        [Require(MinVersion = "1.1")]
        private static glMaterialfDelegate glMaterialf = null;

        public static void Materialf(uint face, uint pname, float param) { glMaterialf(face, pname, param); }

        [MonoNativeFunctionWrapper]
        private delegate void glMaterialfvDelegate(uint face, uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glMaterialfvDelegate glMaterialfv = null;

        public static void Materialfv(uint face, uint pname, float* @params) { glMaterialfv(face, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glMaterialiDelegate(uint face, uint pname, int param);
        [Require(MinVersion = "1.1")]
        private static glMaterialiDelegate glMateriali = null;

        public static void Materiali(uint face, uint pname, int param) { glMateriali(face, pname, param); }

        [MonoNativeFunctionWrapper]
        private delegate void glMaterialivDelegate(uint face, uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glMaterialivDelegate glMaterialiv = null;

        public static void Materialiv(uint face, uint pname, int* @params) { glMaterialiv(face, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glMatrixModeDelegate(uint mode);
        [Require(MinVersion = "1.1")]
        private static glMatrixModeDelegate glMatrixMode = null;

        public static void MatrixMode(uint mode) { glMatrixMode(mode); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultMatrixdDelegate(IntPtr m);
        [Require(MinVersion = "1.1")]
        private static glMultMatrixdDelegate glMultMatrixd = null;

        public static void MultMatrixd(double* m) { glMultMatrixd((IntPtr)m); }

        [MonoNativeFunctionWrapper]
        private delegate void glMultMatrixfDelegate(IntPtr m);
        [Require(MinVersion = "1.1")]
        private static glMultMatrixfDelegate glMultMatrixf = null;

        public static void MultMatrixf(float* m) { glMultMatrixf((IntPtr)m); }

        [MonoNativeFunctionWrapper]
        private delegate void glNewListDelegate(uint list, uint mode);
        [Require(MinVersion = "1.1")]
        private static glNewListDelegate glNewList = null;

        public static void NewList(uint list, uint mode) { glNewList(list, mode); }

        [MonoNativeFunctionWrapper]
        private delegate void glNormal3bDelegate(sbyte nx, sbyte ny, sbyte nz);
        [Require(MinVersion = "1.1")]
        private static glNormal3bDelegate glNormal3b = null;

        public static void Normal3b(sbyte nx, sbyte ny, sbyte nz) { glNormal3b(nx, ny, nz); }

        [MonoNativeFunctionWrapper]
        private delegate void glNormal3bvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glNormal3bvDelegate glNormal3bv = null;

        public static void Normal3bv(sbyte* v) { glNormal3bv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glNormal3dDelegate(double nx, double ny, double nz);
        [Require(MinVersion = "1.1")]
        private static glNormal3dDelegate glNormal3d = null;

        public static void Normal3d(double nx, double ny, double nz) { glNormal3d(nx, ny, nz); }

        [MonoNativeFunctionWrapper]
        private delegate void glNormal3dvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glNormal3dvDelegate glNormal3dv = null;

        public static void Normal3dv(double* v) { glNormal3dv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glNormal3fDelegate(float nx, float ny, float nz);
        [Require(MinVersion = "1.1")]
        private static glNormal3fDelegate glNormal3f = null;

        public static void Normal3f(float nx, float ny, float nz) { glNormal3f(nx, ny, nz); }

        [MonoNativeFunctionWrapper]
        private delegate void glNormal3fvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glNormal3fvDelegate glNormal3fv = null;

        public static void Normal3fv(float* v) { glNormal3fv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glNormal3iDelegate(int nx, int ny, int nz);
        [Require(MinVersion = "1.1")]
        private static glNormal3iDelegate glNormal3i = null;

        public static void Normal3i(int nx, int ny, int nz) { glNormal3i(nx, ny, nz); }

        [MonoNativeFunctionWrapper]
        private delegate void glNormal3ivDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glNormal3ivDelegate glNormal3iv = null;

        public static void Normal3iv(int* v) { glNormal3iv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glNormal3sDelegate(short nx, short ny, short nz);
        [Require(MinVersion = "1.1")]
        private static glNormal3sDelegate glNormal3s = null;

        public static void Normal3s(short nx, short ny, short nz) { glNormal3s(nx, ny, nz); }

        [MonoNativeFunctionWrapper]
        private delegate void glNormal3svDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glNormal3svDelegate glNormal3sv = null;

        public static void Normal3sv(short* v) { glNormal3sv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glNormalPointerDelegate(uint type, int stride, IntPtr pointer);
        [Require(MinVersion = "1.1")]
        private static glNormalPointerDelegate glNormalPointer = null;

        public static void NormalPointer(uint type, int stride, void* pointer) { glNormalPointer(type, stride, (IntPtr)pointer); }

        [MonoNativeFunctionWrapper]
        private delegate void glOrthoDelegate(double left, double right, double bottom, double top, double zNear, double zFar);
        [Require(MinVersion = "1.1")]
        private static glOrthoDelegate glOrtho = null;

        public static void Ortho(double left, double right, double bottom, double top, double zNear, double zFar) { glOrtho(left, right, bottom, top, zNear, zFar); }

        [MonoNativeFunctionWrapper]
        private delegate void glPassThroughDelegate(float token);
        [Require(MinVersion = "1.1")]
        private static glPassThroughDelegate glPassThrough = null;

        public static void PassThrough(float token) { glPassThrough(token); }

        [MonoNativeFunctionWrapper]
        private delegate void glPixelMapfvDelegate(uint map, int mapsize, IntPtr values);
        [Require(MinVersion = "1.1")]
        private static glPixelMapfvDelegate glPixelMapfv = null;

        public static void PixelMapfv(uint map, int mapsize, float* values) { glPixelMapfv(map, mapsize, (IntPtr)values); }

        [MonoNativeFunctionWrapper]
        private delegate void glPixelMapuivDelegate(uint map, int mapsize, IntPtr values);
        [Require(MinVersion = "1.1")]
        private static glPixelMapuivDelegate glPixelMapuiv = null;

        public static void PixelMapuiv(uint map, int mapsize, uint* values) { glPixelMapuiv(map, mapsize, (IntPtr)values); }

        [MonoNativeFunctionWrapper]
        private delegate void glPixelMapusvDelegate(uint map, int mapsize, IntPtr values);
        [Require(MinVersion = "1.1")]
        private static glPixelMapusvDelegate glPixelMapusv = null;

        public static void PixelMapusv(uint map, int mapsize, ushort* values) { glPixelMapusv(map, mapsize, (IntPtr)values); }

        [MonoNativeFunctionWrapper]
        private delegate void glPixelStorefDelegate(uint pname, float param);
        [Require(MinVersion = "1.1")]
        private static glPixelStorefDelegate glPixelStoref = null;

        public static void PixelStoref(uint pname, float param) { glPixelStoref(pname, param); }

        [MonoNativeFunctionWrapper]
        private delegate void glPixelStoreiDelegate(uint pname, int param);
        [Require(MinVersion = "1.1")]
        private static glPixelStoreiDelegate glPixelStorei = null;

        public static void PixelStorei(uint pname, int param) { glPixelStorei(pname, param); }

        [MonoNativeFunctionWrapper]
        private delegate void glPixelTransferfDelegate(uint pname, float param);
        [Require(MinVersion = "1.1")]
        private static glPixelTransferfDelegate glPixelTransferf = null;

        public static void PixelTransferf(uint pname, float param) { glPixelTransferf(pname, param); }

        [MonoNativeFunctionWrapper]
        private delegate void glPixelTransferiDelegate(uint pname, int param);
        [Require(MinVersion = "1.1")]
        private static glPixelTransferiDelegate glPixelTransferi = null;

        public static void PixelTransferi(uint pname, int param) { glPixelTransferi(pname, param); }

        [MonoNativeFunctionWrapper]
        private delegate void glPixelZoomDelegate(float xfactor, float yfactor);
        [Require(MinVersion = "1.1")]
        private static glPixelZoomDelegate glPixelZoom = null;

        public static void PixelZoom(float xfactor, float yfactor) { glPixelZoom(xfactor, yfactor); }

        [MonoNativeFunctionWrapper]
        private delegate void glPointSizeDelegate(float size);
        [Require(MinVersion = "1.1")]
        private static glPointSizeDelegate glPointSize = null;

        public static void PointSize(float size) { glPointSize(size); }

        [MonoNativeFunctionWrapper]
        private delegate void glPolygonModeDelegate(uint face, uint mode);
        [Require(MinVersion = "1.1")]
        private static glPolygonModeDelegate glPolygonMode = null;

        public static void PolygonMode(uint face, uint mode) { glPolygonMode(face, mode); }

        [MonoNativeFunctionWrapper]
        private delegate void glPolygonOffsetDelegate(float factor, float units);
        [Require(MinVersion = "1.1")]
        private static glPolygonOffsetDelegate glPolygonOffset = null;

        public static void PolygonOffset(float factor, float units) { glPolygonOffset(factor, units); }

        [MonoNativeFunctionWrapper]
        private delegate void glPolygonStippleDelegate(IntPtr mask);
        [Require(MinVersion = "1.1")]
        private static glPolygonStippleDelegate glPolygonStipple = null;

        public static void PolygonStipple(byte* mask) { glPolygonStipple((IntPtr)mask); }

        [MonoNativeFunctionWrapper]
        private delegate void glPopAttribDelegate();
        [Require(MinVersion = "1.1")]
        private static glPopAttribDelegate glPopAttrib = null;

        public static void PopAttrib() { glPopAttrib(); }

        [MonoNativeFunctionWrapper]
        private delegate void glPopClientAttribDelegate();
        [Require(MinVersion = "1.1")]
        private static glPopClientAttribDelegate glPopClientAttrib = null;

        public static void PopClientAttrib() { glPopClientAttrib(); }

        [MonoNativeFunctionWrapper]
        private delegate void glPopMatrixDelegate();
        [Require(MinVersion = "1.1")]
        private static glPopMatrixDelegate glPopMatrix = null;

        public static void PopMatrix() { glPopMatrix(); }

        [MonoNativeFunctionWrapper]
        private delegate void glPopNameDelegate();
        [Require(MinVersion = "1.1")]
        private static glPopNameDelegate glPopName = null;

        public static void PopName() { glPopName(); }

        [MonoNativeFunctionWrapper]
        private delegate void glPrioritizeTexturesDelegate(int n, IntPtr textures, IntPtr priorities);
        [Require(MinVersion = "1.1")]
        private static glPrioritizeTexturesDelegate glPrioritizeTextures = null;

        public static void PrioritizeTextures(int n, uint* textures, float* priorities) { glPrioritizeTextures(n, (IntPtr)textures, (IntPtr)priorities); }

        [MonoNativeFunctionWrapper]
        private delegate void glPushAttribDelegate(uint mask);
        [Require(MinVersion = "1.1")]
        private static glPushAttribDelegate glPushAttrib = null;

        public static void PushAttrib(uint mask) { glPushAttrib(mask); }

        [MonoNativeFunctionWrapper]
        private delegate void glPushClientAttribDelegate(uint mask);
        [Require(MinVersion = "1.1")]
        private static glPushClientAttribDelegate glPushClientAttrib = null;

        public static void PushClientAttrib(uint mask) { glPushClientAttrib(mask); }

        [MonoNativeFunctionWrapper]
        private delegate void glPushMatrixDelegate();
        [MonoNativeFunctionWrapper]
        private delegate void glPushNameDelegate(uint name);
        [Require(MinVersion = "1.1")]
        private static glPushNameDelegate glPushName = null;

        public static void PushName(uint name) { glPushName(name); }

        [MonoNativeFunctionWrapper]
        private delegate void glRasterPos2dDelegate(double x, double y);
        [Require(MinVersion = "1.1")]
        private static glRasterPos2dDelegate glRasterPos2d = null;

        public static void RasterPos2d(double x, double y) { glRasterPos2d(x, y); }

        [MonoNativeFunctionWrapper]
        private delegate void glRasterPos2dvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glRasterPos2dvDelegate glRasterPos2dv = null;

        public static void RasterPos2dv(double* v) { glRasterPos2dv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glRasterPos2fDelegate(float x, float y);
        [Require(MinVersion = "1.1")]
        private static glRasterPos2fDelegate glRasterPos2f = null;

        public static void RasterPos2f(float x, float y) { glRasterPos2f(x, y); }

        [MonoNativeFunctionWrapper]
        private delegate void glRasterPos2fvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glRasterPos2fvDelegate glRasterPos2fv = null;

        public static void RasterPos2fv(float* v) { glRasterPos2fv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glRasterPos2iDelegate(int x, int y);
        [Require(MinVersion = "1.1")]
        private static glRasterPos2iDelegate glRasterPos2i = null;

        public static void RasterPos2i(int x, int y) { glRasterPos2i(x, y); }

        [MonoNativeFunctionWrapper]
        private delegate void glRasterPos2ivDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glRasterPos2ivDelegate glRasterPos2iv = null;

        public static void RasterPos2iv(int* v) { glRasterPos2iv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glRasterPos2sDelegate(short x, short y);
        [Require(MinVersion = "1.1")]
        private static glRasterPos2sDelegate glRasterPos2s = null;

        public static void RasterPos2s(short x, short y) { glRasterPos2s(x, y); }

        [MonoNativeFunctionWrapper]
        private delegate void glRasterPos2svDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glRasterPos2svDelegate glRasterPos2sv = null;

        public static void RasterPos2sv(short* v) { glRasterPos2sv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glRasterPos3dDelegate(double x, double y, double z);
        [Require(MinVersion = "1.1")]
        private static glRasterPos3dDelegate glRasterPos3d = null;

        public static void RasterPos3d(double x, double y, double z) { glRasterPos3d(x, y, z); }

        [MonoNativeFunctionWrapper]
        private delegate void glRasterPos3dvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glRasterPos3dvDelegate glRasterPos3dv = null;

        public static void RasterPos3dv(double* v) { glRasterPos3dv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glRasterPos3fDelegate(float x, float y, float z);
        [Require(MinVersion = "1.1")]
        private static glRasterPos3fDelegate glRasterPos3f = null;

        public static void RasterPos3f(float x, float y, float z) { glRasterPos3f(x, y, z); }

        [MonoNativeFunctionWrapper]
        private delegate void glRasterPos3fvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glRasterPos3fvDelegate glRasterPos3fv = null;

        public static void RasterPos3fv(float* v) { glRasterPos3fv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glRasterPos3iDelegate(int x, int y, int z);
        [Require(MinVersion = "1.1")]
        private static glRasterPos3iDelegate glRasterPos3i = null;

        public static void RasterPos3i(int x, int y, int z) { glRasterPos3i(x, y, z); }

        [MonoNativeFunctionWrapper]
        private delegate void glRasterPos3ivDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glRasterPos3ivDelegate glRasterPos3iv = null;

        public static void RasterPos3iv(int* v) { glRasterPos3iv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glRasterPos3sDelegate(short x, short y, short z);
        [Require(MinVersion = "1.1")]
        private static glRasterPos3sDelegate glRasterPos3s = null;

        public static void RasterPos3s(short x, short y, short z) { glRasterPos3s(x, y, z); }

        [MonoNativeFunctionWrapper]
        private delegate void glRasterPos3svDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glRasterPos3svDelegate glRasterPos3sv = null;

        public static void RasterPos3sv(short* v) { glRasterPos3sv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glRasterPos4dDelegate(double x, double y, double z, double w);
        [Require(MinVersion = "1.1")]
        private static glRasterPos4dDelegate glRasterPos4d = null;

        public static void RasterPos4d(double x, double y, double z, double w) { glRasterPos4d(x, y, z, w); }

        [MonoNativeFunctionWrapper]
        private delegate void glRasterPos4dvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glRasterPos4dvDelegate glRasterPos4dv = null;

        public static void RasterPos4dv(double* v) { glRasterPos4dv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glRasterPos4fDelegate(float x, float y, float z, float w);
        [Require(MinVersion = "1.1")]
        private static glRasterPos4fDelegate glRasterPos4f = null;

        public static void RasterPos4f(float x, float y, float z, float w) { glRasterPos4f(x, y, z, w); }

        [MonoNativeFunctionWrapper]
        private delegate void glRasterPos4fvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glRasterPos4fvDelegate glRasterPos4fv = null;

        public static void RasterPos4fv(float* v) { glRasterPos4fv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glRasterPos4iDelegate(int x, int y, int z, int w);
        [Require(MinVersion = "1.1")]
        private static glRasterPos4iDelegate glRasterPos4i = null;

        public static void RasterPos4i(int x, int y, int z, int w) { glRasterPos4i(x, y, z, w); }

        [MonoNativeFunctionWrapper]
        private delegate void glRasterPos4ivDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glRasterPos4ivDelegate glRasterPos4iv = null;

        public static void RasterPos4iv(int* v) { glRasterPos4iv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glRasterPos4sDelegate(short x, short y, short z, short w);
        [Require(MinVersion = "1.1")]
        private static glRasterPos4sDelegate glRasterPos4s = null;

        public static void RasterPos4s(short x, short y, short z, short w) { glRasterPos4s(x, y, z, w); }

        [MonoNativeFunctionWrapper]
        private delegate void glRasterPos4svDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glRasterPos4svDelegate glRasterPos4sv = null;

        public static void RasterPos4sv(short* v) { glRasterPos4sv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glReadBufferDelegate(uint mode);
        [Require(MinVersion = "1.1")]
        private static glReadBufferDelegate glReadBuffer = null;

        public static void ReadBuffer(uint mode) { glReadBuffer(mode); }

        [MonoNativeFunctionWrapper]
        private delegate void glReadPixelsDelegate(int x, int y, int width, int height, uint format, uint type, IntPtr pixels);
        [Require(MinVersion = "1.1")]
        private static glReadPixelsDelegate glReadPixels = null;

        public static void ReadPixels(int x, int y, int width, int height, uint format, uint type, void* pixels) { glReadPixels(x, y, width, height, format, type, (IntPtr)pixels); }

        [MonoNativeFunctionWrapper]
        private delegate void glRectdDelegate(double x1, double y1, double x2, double y2);
        [Require(MinVersion = "1.1")]
        private static glRectdDelegate glRectd = null;

        public static void Rectd(double x1, double y1, double x2, double y2) { glRectd(x1, y1, x2, x2); }

        [MonoNativeFunctionWrapper]
        private delegate void glRectdvDelegate(IntPtr v1, IntPtr v2);
        [Require(MinVersion = "1.1")]
        private static glRectdvDelegate glRectdv = null;

        public static void Rectdv(double* v1, double* v2) { glRectdv((IntPtr)v1, (IntPtr)v2); }

        [MonoNativeFunctionWrapper]
        private delegate void glRectfDelegate(float x1, float y1, float x2, float y2);
        [Require(MinVersion = "1.1")]
        private static glRectfDelegate glRectf = null;

        public static void Rectf(float x1, float y1, float x2, float y2) { glRectf(x1, y1, x2, y2); }

        [MonoNativeFunctionWrapper]
        private delegate void glRectfvDelegate(IntPtr v1, IntPtr v2);
        [Require(MinVersion = "1.1")]
        private static glRectfvDelegate glRectfv = null;

        public static void Rectfv(float* v1, float* v2) { glRectfv((IntPtr)v1, (IntPtr)v2); }

        [MonoNativeFunctionWrapper]
        private delegate void glRectiDelegate(int x1, int y1, int x2, int y2);
        [Require(MinVersion = "1.1")]
        private static glRectiDelegate glRecti = null;

        public static void Recti(int x1, int y1, int x2, int y2) { glRecti(x1, y1, x2, y2); }

        [MonoNativeFunctionWrapper]
        private delegate void glRectivDelegate(IntPtr v1, IntPtr v2);
        [Require(MinVersion = "1.1")]
        private static glRectivDelegate glRectiv = null;

        public static void Rectiv(int* v1, int* v2) { glRectiv((IntPtr)v1, (IntPtr)v2); }

        [MonoNativeFunctionWrapper]
        private delegate void glRectsDelegate(short x1, short y1, short x2, short y2);
        [Require(MinVersion = "1.1")]
        private static glRectsDelegate glRects = null;

        public static void Rects(short x1, short y1, short x2, short y2) { glRects(x1, y1, x2, y2); }

        [MonoNativeFunctionWrapper]
        private delegate void glRectsvDelegate(IntPtr v1, IntPtr v2);
        [Require(MinVersion = "1.1")]
        private static glRectsvDelegate glRectsv = null;

        public static void Rectsv(short* v1, short* v2) { glRectsv((IntPtr)v1, (IntPtr)v2); }

        [MonoNativeFunctionWrapper]
        private delegate int glRenderModeDelegate(uint mode);
        [Require(MinVersion = "1.1")]
        private static glRenderModeDelegate glRenderMode = null;

        public static int RenderMode(uint mode) { return glRenderMode(mode); }

        [MonoNativeFunctionWrapper]
        private delegate void glRotatedDelegate(double angle, double x, double y, double z);
        [Require(MinVersion = "1.1")]
        private static glRotatedDelegate glRotated = null;

        public static void Rotated(double angle, double x, double y, double z) { glRotated(angle, x, y, z); }

        [MonoNativeFunctionWrapper]
        private delegate void glRotatefDelegate(float angle, float x, float y, float z);
        [Require(MinVersion = "1.1")]
        private static glRotatefDelegate glRotatef = null;

        public static void Rotatef(float angle, float x, float y, float z) { glRotatef(angle, x, y, z); }

        [MonoNativeFunctionWrapper]
        private delegate void glScaledDelegate(double x, double y, double z);
        [Require(MinVersion = "1.1")]
        private static glScaledDelegate glScaled = null;

        public static void Scaled(double x, double y, double z) { glScaled(x, y, z); }

        [MonoNativeFunctionWrapper]
        private delegate void glScalefDelegate(float x, float y, float z);
        [Require(MinVersion = "1.1")]
        private static glScalefDelegate glScalef = null;

        public static void Scalef(float x, float y, float z) { glScalef(x, y, z); }

        [MonoNativeFunctionWrapper]
        private delegate void glScissorDelegate(int x, int y, int width, int height);
        [Require(MinVersion = "1.1")]
        private static glScissorDelegate glScissor = null;

        public static void Scissor(int x, int y, int width, int height) { glScissor(x, y, width, height); }

        [MonoNativeFunctionWrapper]
        private delegate void glSelectBufferDelegate(int size, IntPtr buffer);
        [Require(MinVersion = "1.1")]
        private static glSelectBufferDelegate glSelectBuffer = null;

        public static void SelectBuffer(int size, uint* buffer) { glSelectBuffer(size, (IntPtr)buffer); }

        [MonoNativeFunctionWrapper]
        private delegate void glShadeModelDelegate(uint mode);
        [Require(MinVersion = "1.1")]
        private static glShadeModelDelegate glShadeModel = null;

        public static void ShadeModel(uint mode) { glShadeModel(mode); }

        [MonoNativeFunctionWrapper]
        private delegate void glStencilFuncDelegate(uint func, int @ref, uint mask);
        [Require(MinVersion = "1.1")]
        private static glStencilFuncDelegate glStencilFunc = null;

        public static void StencilFunc(uint func, int @ref, uint mask) { glStencilFunc(func, @ref, mask); }

        [MonoNativeFunctionWrapper]
        private delegate void glStencilMaskDelegate(uint mask);
        [Require(MinVersion = "1.1")]
        private static glStencilMaskDelegate glStencilMask = null;

        public static void StencilMask(uint mask) { glStencilMask(mask); }

        [MonoNativeFunctionWrapper]
        private delegate void glStencilOpDelegate(uint fail, uint zfail, uint zpass);
        [Require(MinVersion = "1.1")]
        private static glStencilOpDelegate glStencilOp = null;

        public static void StencilOp(uint fail, uint zfail, uint zpass) { glStencilOp(fail, zfail, zpass); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord1dDelegate(double s);
        [Require(MinVersion = "1.1")]
        private static glTexCoord1dDelegate glTexCoord1d = null;

        public static void TexCoord1d(double s) { glTexCoord1d(s); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord1dvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glTexCoord1dvDelegate glTexCoord1dv = null;

        public static void TexCoord1dv(double* v) { glTexCoord1dv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord1fDelegate(float s);
        [Require(MinVersion = "1.1")]
        private static glTexCoord1fDelegate glTexCoord1f = null;

        public static void TexCoord1f(float s) { glTexCoord1f(s); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord1fvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glTexCoord1fvDelegate glTexCoord1fv = null;

        public static void TexCoord1fv(float* v) { glTexCoord1fv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord1iDelegate(int s);
        [Require(MinVersion = "1.1")]
        private static glTexCoord1iDelegate glTexCoord1i = null;

        public static void TexCoord1i(int s) { glTexCoord1i(s); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord1ivDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glTexCoord1ivDelegate glTexCoord1iv = null;

        public static void TexCoord1iv(int* v) { glTexCoord1iv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord1sDelegate(short s);
        [Require(MinVersion = "1.1")]
        private static glTexCoord1sDelegate glTexCoord1s = null;

        public static void TexCoord1s(short s) { glTexCoord1s(s); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord1svDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glTexCoord1svDelegate glTexCoord1sv = null;

        public static void TexCoord1sv(short* v) { glTexCoord1sv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord2dDelegate(double s, double t);
        [Require(MinVersion = "1.1")]
        private static glTexCoord2dDelegate glTexCoord2d = null;

        public static void TexCoord2d(double s, double t) { glTexCoord2d(s, t); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord2dvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glTexCoord2dvDelegate glTexCoord2dv = null;

        public static void TexCoord2dv(double* v) { glTexCoord2dv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord2fDelegate(float s, float t);
        [Require(MinVersion = "1.1")]
        private static glTexCoord2fDelegate glTexCoord2f = null;

        public static void TexCoord2f(float s, float t) { glTexCoord2f(s, t); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord2fvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glTexCoord2fvDelegate glTexCoord2fv = null;

        public static void TexCoord2fv(float* v) { glTexCoord2fv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord2iDelegate(int s, int t);
        [Require(MinVersion = "1.1")]
        private static glTexCoord2iDelegate glTexCoord2i = null;

        public static void TexCoord2i(int s, int t) { glTexCoord2i(s, t); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord2ivDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glTexCoord2ivDelegate glTexCoord2iv = null;

        public static void TexCoord2iv(int* v) { glTexCoord2iv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord2sDelegate(short s, short t);
        [Require(MinVersion = "1.1")]
        private static glTexCoord2sDelegate glTexCoord2s = null;

        public static void TexCoord2s(short s, short t) { glTexCoord2s(s, t); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord2svDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glTexCoord2svDelegate glTexCoord2sv = null;

        public static void TexCoord2sv(short* v) { glTexCoord2sv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord3dDelegate(double s, double t, double r);
        [Require(MinVersion = "1.1")]
        private static glTexCoord3dDelegate glTexCoord3d = null;

        public static void TexCoord3d(double s, double t, double r) { glTexCoord3d(s, t, r); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord3dvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glTexCoord3dvDelegate glTexCoord3dv = null;

        public static void TexCoord3dv(double* v) { glTexCoord3dv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord3fDelegate(float s, float t, float r);
        [Require(MinVersion = "1.1")]
        private static glTexCoord3fDelegate glTexCoord3f = null;

        public static void TexCoord3f(float s, float t, float r) { glTexCoord3f(s, t, r); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord3fvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glTexCoord3fvDelegate glTexCoord3fv = null;

        public static void TexCoord3fv(float* v) { glTexCoord3fv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord3iDelegate(int s, int t, int r);
        [Require(MinVersion = "1.1")]
        private static glTexCoord3iDelegate glTexCoord3i = null;

        public static void TexCoord3i(int s, int t, int r) { glTexCoord3i(s, t, r); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord3ivDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glTexCoord3ivDelegate glTexCoord3iv = null;

        public static void TexCoord3iv(int* v) { glTexCoord3iv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord3sDelegate(short s, short t, short r);
        [Require(MinVersion = "1.1")]
        private static glTexCoord3sDelegate glTexCoord3s = null;

        public static void TexCoord3s(short s, short t, short r) { glTexCoord3s(s, t, r); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord3svDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glTexCoord3svDelegate glTexCoord3sv = null;

        public static void TexCoord3sv(short* v) { glTexCoord3sv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord4dDelegate(double s, double t, double r, double q);
        [Require(MinVersion = "1.1")]
        private static glTexCoord4dDelegate glTexCoord4d = null;

        public static void TexCoord4d(double s, double t, double r, double q) { glTexCoord4d(s, t, r, q); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord4dvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glTexCoord4dvDelegate glTexCoord4dv = null;

        public static void TexCoord4dv(double* v) { glTexCoord4dv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord4fDelegate(float s, float t, float r, float q);
        [Require(MinVersion = "1.1")]
        private static glTexCoord4fDelegate glTexCoord4f = null;

        public static void TexCoord4f(float s, float t, float r, float q) { glTexCoord4f(s, t, r, q); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord4fvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glTexCoord4fvDelegate glTexCoord4fv = null;

        public static void TexCoord4fv(float* v) { glTexCoord4fv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord4iDelegate(int s, int t, int r, int q);
        [Require(MinVersion = "1.1")]
        private static glTexCoord4iDelegate glTexCoord4i = null;

        public static void TexCoord4i(int s, int t, int r, int q) { glTexCoord4i(s, t, r, q); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord4ivDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glTexCoord4ivDelegate glTexCoord4iv = null;

        public static void TexCoord4iv(int* v) { glTexCoord4iv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord4sDelegate(short s, short t, short r, short q);
        [Require(MinVersion = "1.1")]
        private static glTexCoord4sDelegate glTexCoord4s = null;

        public static void TexCoord4s(short s, short t, short r, short q) { glTexCoord4s(s, t, r, q); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoord4svDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glTexCoord4svDelegate glTexCoord4sv = null;

        public static void TexCoord4sv(short* v) { glTexCoord4sv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexCoordPointerDelegate(int size, uint type, int stride, IntPtr pointer);
        [Require(MinVersion = "1.1")]
        private static glTexCoordPointerDelegate glTexCoordPointer = null;

        public static void TexCoordPointer(int size, uint type, int stride, void* pointer) { glTexCoordPointer(size, type, stride, (IntPtr)pointer); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexEnvfDelegate(uint target, uint pname, float param);
        [Require(MinVersion = "1.1")]
        private static glTexEnvfDelegate glTexEnvf = null;

        public static void TexEnvf(uint target, uint pname, float param) { glTexEnvf(target, pname, param); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexEnvfvDelegate(uint target, uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glTexEnvfvDelegate glTexEnvfv = null;

        public static void TexEnvfv(uint target, uint pname, float* @params) { glTexEnvfv(target, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexEnviDelegate(uint target, uint pname, int param);
        [Require(MinVersion = "1.1")]
        private static glTexEnviDelegate glTexEnvi = null;

        public static void TexEnvi(uint target, uint pname, int param) { glTexEnvi(target, pname, param); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexEnvivDelegate(uint target, uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glTexEnvivDelegate glTexEnviv = null;

        public static void TexEnviv(uint target, uint pname, int* @params) { glTexEnviv(target, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexGendDelegate(uint coord, uint pname, double param);
        [Require(MinVersion = "1.1")]
        private static glTexGendDelegate glTexGend = null;

        public static void TexGend(uint coord, uint pname, double param) { glTexGend(coord, pname, param); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexGendvDelegate(uint coord, uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glTexGendvDelegate glTexGendv = null;

        public static void TexGendv(uint coord, uint pname, double* @params) { glTexGendv(coord, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexGenfDelegate(uint coord, uint pname, float param);
        [Require(MinVersion = "1.1")]
        private static glTexGenfDelegate glTexGenf = null;

        public static void TexGenf(uint coord, uint pname, float param) { glTexGenf(coord, pname, param); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexGenfvDelegate(uint coord, uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glTexGenfvDelegate glTexGenfv = null;

        public static void TexGenfv(uint coord, uint pname, float* @params) { glTexGenfv(coord, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexGeniDelegate(uint coord, uint pname, int param);
        [Require(MinVersion = "1.1")]
        private static glTexGeniDelegate glTexGeni = null;

        public static void TexGeni(uint coord, uint pname, int param) { glTexGeni(coord, pname, param); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexGenivDelegate(uint coord, uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glTexGenivDelegate glTexGeniv = null;

        public static void TexGeniv(uint coord, uint pname, int* @params) { glTexGeniv(coord, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexImage1DDelegate(uint target, int level, int internalformat, int width, int border, uint format, uint type, IntPtr pixels);
        [Require(MinVersion = "1.1")]
        private static glTexImage1DDelegate glTexImage1D = null;

        public static void TexImage1D(uint target, int level, int internalformat, int width, int border, uint format, uint type, void* pixels) { glTexImage1D(target, level, internalformat, width, border, format, type, (IntPtr)pixels); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexImage2DDelegate(uint target, int level ,int internalformat, int width, int height, int border, uint format, uint type, IntPtr pixels);
        [Require(MinVersion = "1.1")]
        private static glTexImage2DDelegate glTexImage2D = null;

        public static void TexImage2D(uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, void* pixels) { glTexImage2D(target, level, internalformat, width, height, border, format, type, (IntPtr)pixels); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexParameterfDelegate(uint target, uint pname, float param);
        [Require(MinVersion = "1.1")]
        private static glTexParameterfDelegate glTexParameterf = null;

        public static void TexParameterf(uint target, uint pname, float param) { glTexParameterf(target, pname, param); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexParameterfvDelegate(uint target, uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glTexParameterfvDelegate glTexParameterfv = null;

        public static void TexParameterfv(uint target, uint pname, float* @params) { glTexParameterfv(target, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexParameteriDelegate(uint target, uint pname, int param);
        [Require(MinVersion = "1.1")]
        private static glTexParameteriDelegate glTexParameteri = null;

        public static void TexParameteri(uint target, uint pname, int param) { glTexParameteri(target, pname, param); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexParameterivDelegate(uint target, uint pname, IntPtr @params);
        [Require(MinVersion = "1.1")]
        private static glTexParameterivDelegate glTexParameteriv = null;

        public static void TexParameteriv(uint target, uint pname, int* @params) { glTexParameteriv(target, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexSubImage1DDelegate(uint target, int level, int xoffset, int width, uint format, uint type, IntPtr pixels);
        [Require(MinVersion = "1.1")]
        private static glTexSubImage1DDelegate glTexSubImage1D = null;

        public static void TexSubImage1D(uint target, int level, int xoffset, int width, uint format, uint type, void* pixels) { glTexSubImage1D(target, level, xoffset, width, format, type, (IntPtr)pixels); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexSubImage2DDelegate(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, IntPtr pixels);
        [Require(MinVersion = "1.1")]
        private static glTexSubImage2DDelegate glTexSubImage2D = null;

        public static void TexSubImage2D(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, void* pixels) { glTexSubImage2D(target, level, xoffset, yoffset, width, height, format, type, (IntPtr)pixels); }

        [MonoNativeFunctionWrapper]
        private delegate void glTranslatedDelegate(double x, double y, double z);
        [Require(MinVersion = "1.1")]
        private static glTranslatedDelegate glTranslated = null;

        public static void Translated(double x, double y, double z) { glTranslated(x, y, z); }

        [MonoNativeFunctionWrapper]
        private delegate void glTranslatefDelegate(float x, float y, float z);
        [Require(MinVersion = "1.1")]
        private static glTranslatefDelegate glTranslatef = null;

        public static void Translatef(float x, float y, float z) { glTranslatef(x, y, z); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertex2dDelegate(double x, double y);
        [Require(MinVersion = "1.1")]
        private static glVertex2dDelegate glVertex2d = null;

        public static void Vertex2d(double x, double y) { glVertex2d(x, y); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertex2dvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glVertex2dvDelegate glVertex2dv = null;

        public static void Vertex2dv(double* v) { glVertex2dv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertex2fDelegate(float x, float y);
        [Require(MinVersion = "1.1")]
        private static glVertex2fDelegate glVertex2f = null;

        public static void Vertex2f(float x, float y) { glVertex2f(x, y); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertex2fvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glVertex2fvDelegate glVertex2fv = null;

        public static void Vertex2fv(float* v) { glVertex2fv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertex2iDelegate(int x, int y);
        [Require(MinVersion = "1.1")]
        private static glVertex2iDelegate glVertex2i = null;

        public static void Vertex2i(int x, int y) { glVertex2i(x, y); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertex2ivDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glVertex2ivDelegate glVertex2iv = null;

        public static void Vertex2iv(int* v) { glVertex2iv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertex2sDelegate(short x, short y);
        [Require(MinVersion = "1.1")]
        private static glVertex2sDelegate glVertex2s = null;

        public static void Vertex2s(short x, short y) { glVertex2s(x, y); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertex2svDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glVertex2svDelegate glVertex2sv = null;

        public static void Vertex2sv(short* v) { glVertex2sv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertex3dDelegate(double x, double y, double z);
        [Require(MinVersion = "1.1")]
        private static glVertex3dDelegate glVertex3d = null;

        public static void Vertex3d(double x, double y, double z) { glVertex3d(x, y, z); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertex3dvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glVertex3dvDelegate glVertex3dv = null;

        public static void Vertex3dv(double* v) { glVertex3dv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertex3fDelegate(float x, float y, float z);
        [Require(MinVersion = "1.1")]
        private static glVertex3fDelegate glVertex3f = null;

        public static void Vertex3f(float x, float y, float z) { glVertex3f(x, y, z); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertex3fvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glVertex3fvDelegate glVertex3fv = null;

        public static void Vertex3fv(float* v) { glVertex3fv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertex3iDelegate(int x, int y, int z);
        [Require(MinVersion = "1.1")]
        private static glVertex3iDelegate glVertex3i = null;

        public static void Vertex3i(int x, int y, int z) { glVertex3i(x, y, z); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertex3ivDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glVertex3ivDelegate glVertex3iv = null;

        public static void Vertex3iv(int* v) { glVertex3iv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertex3sDelegate(short x, short y, short z);
        [Require(MinVersion = "1.1")]
        private static glVertex3sDelegate glVertex3s = null;

        public static void Vertex3s(short x, short y, short z) { glVertex3s(x, y, z); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertex3svDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glVertex3svDelegate glVertex3sv = null;

        public static void Vertex3sv(short* v) { glVertex3sv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertex4dDelegate(double x, double y, double z, double w);
        [Require(MinVersion = "1.1")]
        private static glVertex4dDelegate glVertex4d = null;

        public static void Vertex4d(double x, double y, double z, double w) { glVertex4d(x, y, z, w); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertex4dvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glVertex4dvDelegate glVertex4dv = null;

        public static void Vertex4dv(double* v) { glVertex4dv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertex4fDelegate(float x, float y, float z, float w);
        [Require(MinVersion = "1.1")]
        private static glVertex4fDelegate glVertex4f = null;

        public static void Vertex4f(float x, float y, float z, float w) { glVertex4f(x, y, z, w); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertex4fvDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glVertex4fvDelegate glVertex4fv = null;

        public static void Vertex4fv(float* v) { glVertex4fv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertex4iDelegate(int x, int y, int z, int w);
        [Require(MinVersion = "1.1")]
        private static glVertex4iDelegate glVertex4i = null;

        public static void Vertex4i(int x, int y, int z, int w) { glVertex4i(x, y, z, w); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertex4ivDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glVertex4ivDelegate glVertex4iv = null;

        public static void Vertex4iv(int* v) { glVertex4iv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertex4sDelegate(short x, short y, short z, short w);
        [Require(MinVersion = "1.1")]
        private static glVertex4sDelegate glVertex4s = null;

        public static void Vertex4s(short x, short y, short z, short w) { glVertex4s(x, y, z, w); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertex4svDelegate(IntPtr v);
        [Require(MinVersion = "1.1")]
        private static glVertex4svDelegate glVertex4sv = null;

        public static void Vertex4sv(short* v) { glVertex4sv((IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexPointerDelegate(int size, uint type, int stride, IntPtr pointer);
        [Require(MinVersion = "1.1")]
        private static glVertexPointerDelegate glVertexPointer = null;

        public static void VertexPointer(int size, uint type, int stride, void* pointer) { glVertexPointer(size, type, stride, (IntPtr)pointer); }

        [MonoNativeFunctionWrapper]
        private delegate void glViewportDelegate(int x, int y, int width, int height);
        [Require(MinVersion = "1.1")]
        private static glViewportDelegate glViewport = null;

        public static void Viewport(int x, int y, int width, int height) { glViewport(x, y, width, height); }

        public const UInt32 GL_ACCUM                          = 0x0100;
        public const UInt32 GL_LOAD                           = 0x0101;
        public const UInt32 GL_RETURN                         = 0x0102;
        public const UInt32 GL_MULT                           = 0x0103;
        public const UInt32 GL_ADD                            = 0x0104;
        public const UInt32 GL_NEVER                          = 0x0200;
        public const UInt32 GL_LESS                           = 0x0201;
        public const UInt32 GL_EQUAL                          = 0x0202;
        public const UInt32 GL_LEQUAL                         = 0x0203;
        public const UInt32 GL_GREATER                        = 0x0204;
        public const UInt32 GL_NOTEQUAL                       = 0x0205;
        public const UInt32 GL_GEQUAL                         = 0x0206;
        public const UInt32 GL_ALWAYS                         = 0x0207;
        public const UInt32 GL_CURRENT_BIT                    = 0x00000001;
        public const UInt32 GL_POINT_BIT                      = 0x00000002;
        public const UInt32 GL_LINE_BIT                       = 0x00000004;
        public const UInt32 GL_POLYGON_BIT                    = 0x00000008;
        public const UInt32 GL_POLYGON_STIPPLE_BIT            = 0x00000010;
        public const UInt32 GL_PIXEL_MODE_BIT                 = 0x00000020;
        public const UInt32 GL_LIGHTING_BIT                   = 0x00000040;
        public const UInt32 GL_FOG_BIT                        = 0x00000080;
        public const UInt32 GL_DEPTH_BUFFER_BIT               = 0x00000100;
        public const UInt32 GL_ACCUM_BUFFER_BIT               = 0x00000200;
        public const UInt32 GL_STENCIL_BUFFER_BIT             = 0x00000400;
        public const UInt32 GL_VIEWPORT_BIT                   = 0x00000800;
        public const UInt32 GL_TRANSFORM_BIT                  = 0x00001000;
        public const UInt32 GL_ENABLE_BIT                     = 0x00002000;
        public const UInt32 GL_COLOR_BUFFER_BIT               = 0x00004000;
        public const UInt32 GL_HINT_BIT                       = 0x00008000;
        public const UInt32 GL_EVAL_BIT                       = 0x00010000;
        public const UInt32 GL_LIST_BIT                       = 0x00020000;
        public const UInt32 GL_TEXTURE_BIT                    = 0x00040000;
        public const UInt32 GL_SCISSOR_BIT                    = 0x00080000;
        public const UInt32 GL_ALL_ATTRIB_BITS                = 0x000fffff;
        public const UInt32 GL_POINTS                         = 0x0000;
        public const UInt32 GL_LINES                          = 0x0001;
        public const UInt32 GL_LINE_LOOP                      = 0x0002;
        public const UInt32 GL_LINE_STRIP                     = 0x0003;
        public const UInt32 GL_TRIANGLES                      = 0x0004;
        public const UInt32 GL_TRIANGLE_STRIP                 = 0x0005;
        public const UInt32 GL_TRIANGLE_FAN                   = 0x0006;
        public const UInt32 GL_QUADS                          = 0x0007;
        public const UInt32 GL_QUAD_STRIP                     = 0x0008;
        public const UInt32 GL_POLYGON                        = 0x0009;
        public const UInt32 GL_ZERO                           = 0;
        public const UInt32 GL_ONE                            = 1;
        public const UInt32 GL_SRC_COLOR                      = 0x0300;
        public const UInt32 GL_ONE_MINUS_SRC_COLOR            = 0x0301;
        public const UInt32 GL_SRC_ALPHA                      = 0x0302;
        public const UInt32 GL_ONE_MINUS_SRC_ALPHA            = 0x0303;
        public const UInt32 GL_DST_ALPHA                      = 0x0304;
        public const UInt32 GL_ONE_MINUS_DST_ALPHA            = 0x0305;
        public const UInt32 GL_DST_COLOR                      = 0x0306;
        public const UInt32 GL_ONE_MINUS_DST_COLOR            = 0x0307;
        public const UInt32 GL_SRC_ALPHA_SATURATE             = 0x0308;
        public const UInt32 GL_TRUE                           = 1;
        public const UInt32 GL_FALSE                          = 0;
        public const UInt32 GL_CLIP_PLANE0                    = 0x3000;
        public const UInt32 GL_CLIP_PLANE1                    = 0x3001;
        public const UInt32 GL_CLIP_PLANE2                    = 0x3002;
        public const UInt32 GL_CLIP_PLANE3                    = 0x3003;
        public const UInt32 GL_CLIP_PLANE4                    = 0x3004;
        public const UInt32 GL_CLIP_PLANE5                    = 0x3005;
        public const UInt32 GL_BYTE                           = 0x1400;
        public const UInt32 GL_UNSIGNED_BYTE                  = 0x1401;
        public const UInt32 GL_SHORT                          = 0x1402;
        public const UInt32 GL_UNSIGNED_SHORT                 = 0x1403;
        public const UInt32 GL_INT                            = 0x1404;
        public const UInt32 GL_UNSIGNED_INT                   = 0x1405;
        public const UInt32 GL_FLOAT                          = 0x1406;
        public const UInt32 GL_2_BYTES                        = 0x1407;
        public const UInt32 GL_3_BYTES                        = 0x1408;
        public const UInt32 GL_4_BYTES                        = 0x1409;
        public const UInt32 GL_DOUBLE                         = 0x140A;
        public const UInt32 GL_NONE                           = 0;
        public const UInt32 GL_FRONT_LEFT                     = 0x0400;
        public const UInt32 GL_FRONT_RIGHT                    = 0x0401;
        public const UInt32 GL_BACK_LEFT                      = 0x0402;
        public const UInt32 GL_BACK_RIGHT                     = 0x0403;
        public const UInt32 GL_FRONT                          = 0x0404;
        public const UInt32 GL_BACK                           = 0x0405;
        public const UInt32 GL_LEFT                           = 0x0406;
        public const UInt32 GL_RIGHT                          = 0x0407;
        public const UInt32 GL_FRONT_AND_BACK                 = 0x0408;
        public const UInt32 GL_AUX0                           = 0x0409;
        public const UInt32 GL_AUX1                           = 0x040A;
        public const UInt32 GL_AUX2                           = 0x040B;
        public const UInt32 GL_AUX3                           = 0x040C;
        public const UInt32 GL_NO_ERROR                       = 0;
        public const UInt32 GL_INVALID_ENUM                   = 0x0500;
        public const UInt32 GL_INVALID_VALUE                  = 0x0501;
        public const UInt32 GL_INVALID_OPERATION              = 0x0502;
        public const UInt32 GL_STACK_OVERFLOW                 = 0x0503;
        public const UInt32 GL_STACK_UNDERFLOW                = 0x0504;
        public const UInt32 GL_OUT_OF_MEMORY                  = 0x0505;
        public const UInt32 GL_2D                             = 0x0600;
        public const UInt32 GL_3D                             = 0x0601;
        public const UInt32 GL_3D_COLOR                       = 0x0602;
        public const UInt32 GL_3D_COLOR_TEXTURE               = 0x0603;
        public const UInt32 GL_4D_COLOR_TEXTURE               = 0x0604;
        public const UInt32 GL_PASS_THROUGH_TOKEN             = 0x0700;
        public const UInt32 GL_POINT_TOKEN                    = 0x0701;
        public const UInt32 GL_LINE_TOKEN                     = 0x0702;
        public const UInt32 GL_POLYGON_TOKEN                  = 0x0703;
        public const UInt32 GL_BITMAP_TOKEN                   = 0x0704;
        public const UInt32 GL_DRAW_PIXEL_TOKEN               = 0x0705;
        public const UInt32 GL_COPY_PIXEL_TOKEN               = 0x0706;
        public const UInt32 GL_LINE_RESET_TOKEN               = 0x0707;
        public const UInt32 GL_EXP                            = 0x0800;
        public const UInt32 GL_EXP2                           = 0x0801;
        public const UInt32 GL_CW                             = 0x0900;
        public const UInt32 GL_CCW                            = 0x0901;
        public const UInt32 GL_COEFF                          = 0x0A00;
        public const UInt32 GL_ORDER                          = 0x0A01;
        public const UInt32 GL_DOMAIN                         = 0x0A02;
        public const UInt32 GL_CURRENT_COLOR                  = 0x0B00;
        public const UInt32 GL_CURRENT_INDEX                  = 0x0B01;
        public const UInt32 GL_CURRENT_NORMAL                 = 0x0B02;
        public const UInt32 GL_CURRENT_TEXTURE_COORDS         = 0x0B03;
        public const UInt32 GL_CURRENT_RASTER_COLOR           = 0x0B04;
        public const UInt32 GL_CURRENT_RASTER_INDEX           = 0x0B05;
        public const UInt32 GL_CURRENT_RASTER_TEXTURE_COORDS  = 0x0B06;
        public const UInt32 GL_CURRENT_RASTER_POSITION        = 0x0B07;
        public const UInt32 GL_CURRENT_RASTER_POSITION_VALID  = 0x0B08;
        public const UInt32 GL_CURRENT_RASTER_DISTANCE        = 0x0B09;
        public const UInt32 GL_POINT_SMOOTH                   = 0x0B10;
        public const UInt32 GL_POINT_SIZE                     = 0x0B11;
        public const UInt32 GL_POINT_SIZE_RANGE               = 0x0B12;
        public const UInt32 GL_POINT_SIZE_GRANULARITY         = 0x0B13;
        public const UInt32 GL_LINE_SMOOTH                    = 0x0B20;
        public const UInt32 GL_LINE_WIDTH                     = 0x0B21;
        public const UInt32 GL_LINE_WIDTH_RANGE               = 0x0B22;
        public const UInt32 GL_LINE_WIDTH_GRANULARITY         = 0x0B23;
        public const UInt32 GL_LINE_STIPPLE                   = 0x0B24;
        public const UInt32 GL_LINE_STIPPLE_PATTERN           = 0x0B25;
        public const UInt32 GL_LINE_STIPPLE_REPEAT            = 0x0B26;
        public const UInt32 GL_LIST_MODE                      = 0x0B30;
        public const UInt32 GL_MAX_LIST_NESTING               = 0x0B31;
        public const UInt32 GL_LIST_BASE                      = 0x0B32;
        public const UInt32 GL_LIST_INDEX                     = 0x0B33;
        public const UInt32 GL_POLYGON_MODE                   = 0x0B40;
        public const UInt32 GL_POLYGON_SMOOTH                 = 0x0B41;
        public const UInt32 GL_POLYGON_STIPPLE                = 0x0B42;
        public const UInt32 GL_EDGE_FLAG                      = 0x0B43;
        public const UInt32 GL_CULL_FACE                      = 0x0B44;
        public const UInt32 GL_CULL_FACE_MODE                 = 0x0B45;
        public const UInt32 GL_FRONT_FACE                     = 0x0B46;
        public const UInt32 GL_LIGHTING                       = 0x0B50;
        public const UInt32 GL_LIGHT_MODEL_LOCAL_VIEWER       = 0x0B51;
        public const UInt32 GL_LIGHT_MODEL_TWO_SIDE           = 0x0B52;
        public const UInt32 GL_LIGHT_MODEL_AMBIENT            = 0x0B53;
        public const UInt32 GL_SHADE_MODEL                    = 0x0B54;
        public const UInt32 GL_COLOR_MATERIAL_FACE            = 0x0B55;
        public const UInt32 GL_COLOR_MATERIAL_PARAMETER       = 0x0B56;
        public const UInt32 GL_COLOR_MATERIAL                 = 0x0B57;
        public const UInt32 GL_FOG                            = 0x0B60;
        public const UInt32 GL_FOG_INDEX                      = 0x0B61;
        public const UInt32 GL_FOG_DENSITY                    = 0x0B62;
        public const UInt32 GL_FOG_START                      = 0x0B63;
        public const UInt32 GL_FOG_END                        = 0x0B64;
        public const UInt32 GL_FOG_MODE                       = 0x0B65;
        public const UInt32 GL_FOG_COLOR                      = 0x0B66;
        public const UInt32 GL_DEPTH_RANGE                    = 0x0B70;
        public const UInt32 GL_DEPTH_TEST                     = 0x0B71;
        public const UInt32 GL_DEPTH_WRITEMASK                = 0x0B72;
        public const UInt32 GL_DEPTH_CLEAR_VALUE              = 0x0B73;
        public const UInt32 GL_DEPTH_FUNC                     = 0x0B74;
        public const UInt32 GL_ACCUM_CLEAR_VALUE              = 0x0B80;
        public const UInt32 GL_STENCIL_TEST                   = 0x0B90;
        public const UInt32 GL_STENCIL_CLEAR_VALUE            = 0x0B91;
        public const UInt32 GL_STENCIL_FUNC                   = 0x0B92;
        public const UInt32 GL_STENCIL_VALUE_MASK             = 0x0B93;
        public const UInt32 GL_STENCIL_FAIL                   = 0x0B94;
        public const UInt32 GL_STENCIL_PASS_DEPTH_FAIL        = 0x0B95;
        public const UInt32 GL_STENCIL_PASS_DEPTH_PASS        = 0x0B96;
        public const UInt32 GL_STENCIL_REF                    = 0x0B97;
        public const UInt32 GL_STENCIL_WRITEMASK              = 0x0B98;
        public const UInt32 GL_MATRIX_MODE                    = 0x0BA0;
        public const UInt32 GL_NORMALIZE                      = 0x0BA1;
        public const UInt32 GL_VIEWPORT                       = 0x0BA2;
        public const UInt32 GL_MODELVIEW_STACK_DEPTH          = 0x0BA3;
        public const UInt32 GL_PROJECTION_STACK_DEPTH         = 0x0BA4;
        public const UInt32 GL_TEXTURE_STACK_DEPTH            = 0x0BA5;
        public const UInt32 GL_MODELVIEW_MATRIX               = 0x0BA6;
        public const UInt32 GL_PROJECTION_MATRIX              = 0x0BA7;
        public const UInt32 GL_TEXTURE_MATRIX                 = 0x0BA8;
        public const UInt32 GL_ATTRIB_STACK_DEPTH             = 0x0BB0;
        public const UInt32 GL_CLIENT_ATTRIB_STACK_DEPTH      = 0x0BB1;
        public const UInt32 GL_ALPHA_TEST                     = 0x0BC0;
        public const UInt32 GL_ALPHA_TEST_FUNC                = 0x0BC1;
        public const UInt32 GL_ALPHA_TEST_REF                 = 0x0BC2;
        public const UInt32 GL_DITHER                         = 0x0BD0;
        public const UInt32 GL_BLEND_DST                      = 0x0BE0;
        public const UInt32 GL_BLEND_SRC                      = 0x0BE1;
        public const UInt32 GL_BLEND                          = 0x0BE2;
        public const UInt32 GL_LOGIC_OP_MODE                  = 0x0BF0;
        public const UInt32 GL_INDEX_LOGIC_OP                 = 0x0BF1;
        public const UInt32 GL_COLOR_LOGIC_OP                 = 0x0BF2;
        public const UInt32 GL_AUX_BUFFERS                    = 0x0C00;
        public const UInt32 GL_DRAW_BUFFER                    = 0x0C01;
        public const UInt32 GL_READ_BUFFER                    = 0x0C02;
        public const UInt32 GL_SCISSOR_BOX                    = 0x0C10;
        public const UInt32 GL_SCISSOR_TEST                   = 0x0C11;
        public const UInt32 GL_INDEX_CLEAR_VALUE              = 0x0C20;
        public const UInt32 GL_INDEX_WRITEMASK                = 0x0C21;
        public const UInt32 GL_COLOR_CLEAR_VALUE              = 0x0C22;
        public const UInt32 GL_COLOR_WRITEMASK                = 0x0C23;
        public const UInt32 GL_INDEX_MODE                     = 0x0C30;
        public const UInt32 GL_RGBA_MODE                      = 0x0C31;
        public const UInt32 GL_DOUBLEBUFFER                   = 0x0C32;
        public const UInt32 GL_STEREO                         = 0x0C33;
        public const UInt32 GL_RENDER_MODE                    = 0x0C40;
        public const UInt32 GL_PERSPECTIVE_CORRECTION_HINT    = 0x0C50;
        public const UInt32 GL_POINT_SMOOTH_HINT              = 0x0C51;
        public const UInt32 GL_LINE_SMOOTH_HINT               = 0x0C52;
        public const UInt32 GL_POLYGON_SMOOTH_HINT            = 0x0C53;
        public const UInt32 GL_FOG_HINT                       = 0x0C54;
        public const UInt32 GL_TEXTURE_GEN_S                  = 0x0C60;
        public const UInt32 GL_TEXTURE_GEN_T                  = 0x0C61;
        public const UInt32 GL_TEXTURE_GEN_R                  = 0x0C62;
        public const UInt32 GL_TEXTURE_GEN_Q                  = 0x0C63;
        public const UInt32 GL_PIXEL_MAP_I_TO_I               = 0x0C70;
        public const UInt32 GL_PIXEL_MAP_S_TO_S               = 0x0C71;
        public const UInt32 GL_PIXEL_MAP_I_TO_R               = 0x0C72;
        public const UInt32 GL_PIXEL_MAP_I_TO_G               = 0x0C73;
        public const UInt32 GL_PIXEL_MAP_I_TO_B               = 0x0C74;
        public const UInt32 GL_PIXEL_MAP_I_TO_A               = 0x0C75;
        public const UInt32 GL_PIXEL_MAP_R_TO_R               = 0x0C76;
        public const UInt32 GL_PIXEL_MAP_G_TO_G               = 0x0C77;
        public const UInt32 GL_PIXEL_MAP_B_TO_B               = 0x0C78;
        public const UInt32 GL_PIXEL_MAP_A_TO_A               = 0x0C79;
        public const UInt32 GL_PIXEL_MAP_I_TO_I_SIZE          = 0x0CB0;
        public const UInt32 GL_PIXEL_MAP_S_TO_S_SIZE          = 0x0CB1;
        public const UInt32 GL_PIXEL_MAP_I_TO_R_SIZE          = 0x0CB2;
        public const UInt32 GL_PIXEL_MAP_I_TO_G_SIZE          = 0x0CB3;
        public const UInt32 GL_PIXEL_MAP_I_TO_B_SIZE          = 0x0CB4;
        public const UInt32 GL_PIXEL_MAP_I_TO_A_SIZE          = 0x0CB5;
        public const UInt32 GL_PIXEL_MAP_R_TO_R_SIZE          = 0x0CB6;
        public const UInt32 GL_PIXEL_MAP_G_TO_G_SIZE          = 0x0CB7;
        public const UInt32 GL_PIXEL_MAP_B_TO_B_SIZE          = 0x0CB8;
        public const UInt32 GL_PIXEL_MAP_A_TO_A_SIZE          = 0x0CB9;
        public const UInt32 GL_UNPACK_SWAP_BYTES              = 0x0CF0;
        public const UInt32 GL_UNPACK_LSB_FIRST               = 0x0CF1;
        public const UInt32 GL_UNPACK_ROW_LENGTH              = 0x0CF2;
        public const UInt32 GL_UNPACK_SKIP_ROWS               = 0x0CF3;
        public const UInt32 GL_UNPACK_SKIP_PIXELS             = 0x0CF4;
        public const UInt32 GL_UNPACK_ALIGNMENT               = 0x0CF5;
        public const UInt32 GL_PACK_SWAP_BYTES                = 0x0D00;
        public const UInt32 GL_PACK_LSB_FIRST                 = 0x0D01;
        public const UInt32 GL_PACK_ROW_LENGTH                = 0x0D02;
        public const UInt32 GL_PACK_SKIP_ROWS                 = 0x0D03;
        public const UInt32 GL_PACK_SKIP_PIXELS               = 0x0D04;
        public const UInt32 GL_PACK_ALIGNMENT                 = 0x0D05;
        public const UInt32 GL_MAP_COLOR                      = 0x0D10;
        public const UInt32 GL_MAP_STENCIL                    = 0x0D11;
        public const UInt32 GL_INDEX_SHIFT                    = 0x0D12;
        public const UInt32 GL_INDEX_OFFSET                   = 0x0D13;
        public const UInt32 GL_RED_SCALE                      = 0x0D14;
        public const UInt32 GL_RED_BIAS                       = 0x0D15;
        public const UInt32 GL_ZOOM_X                         = 0x0D16;
        public const UInt32 GL_ZOOM_Y                         = 0x0D17;
        public const UInt32 GL_GREEN_SCALE                    = 0x0D18;
        public const UInt32 GL_GREEN_BIAS                     = 0x0D19;
        public const UInt32 GL_BLUE_SCALE                     = 0x0D1A;
        public const UInt32 GL_BLUE_BIAS                      = 0x0D1B;
        public const UInt32 GL_ALPHA_SCALE                    = 0x0D1C;
        public const UInt32 GL_ALPHA_BIAS                     = 0x0D1D;
        public const UInt32 GL_DEPTH_SCALE                    = 0x0D1E;
        public const UInt32 GL_DEPTH_BIAS                     = 0x0D1F;
        public const UInt32 GL_MAX_EVAL_ORDER                 = 0x0D30;
        public const UInt32 GL_MAX_LIGHTS                     = 0x0D31;
        public const UInt32 GL_MAX_CLIP_PLANES                = 0x0D32;
        public const UInt32 GL_MAX_TEXTURE_SIZE               = 0x0D33;
        public const UInt32 GL_MAX_PIXEL_MAP_TABLE            = 0x0D34;
        public const UInt32 GL_MAX_ATTRIB_STACK_DEPTH         = 0x0D35;
        public const UInt32 GL_MAX_MODELVIEW_STACK_DEPTH      = 0x0D36;
        public const UInt32 GL_MAX_NAME_STACK_DEPTH           = 0x0D37;
        public const UInt32 GL_MAX_PROJECTION_STACK_DEPTH     = 0x0D38;
        public const UInt32 GL_MAX_TEXTURE_STACK_DEPTH        = 0x0D39;
        public const UInt32 GL_MAX_VIEWPORT_DIMS              = 0x0D3A;
        public const UInt32 GL_MAX_CLIENT_ATTRIB_STACK_DEPTH  = 0x0D3B;
        public const UInt32 GL_SUBPIXEL_BITS                  = 0x0D50;
        public const UInt32 GL_INDEX_BITS                     = 0x0D51;
        public const UInt32 GL_RED_BITS                       = 0x0D52;
        public const UInt32 GL_GREEN_BITS                     = 0x0D53;
        public const UInt32 GL_BLUE_BITS                      = 0x0D54;
        public const UInt32 GL_ALPHA_BITS                     = 0x0D55;
        public const UInt32 GL_DEPTH_BITS                     = 0x0D56;
        public const UInt32 GL_STENCIL_BITS                   = 0x0D57;
        public const UInt32 GL_ACCUM_RED_BITS                 = 0x0D58;
        public const UInt32 GL_ACCUM_GREEN_BITS               = 0x0D59;
        public const UInt32 GL_ACCUM_BLUE_BITS                = 0x0D5A;
        public const UInt32 GL_ACCUM_ALPHA_BITS               = 0x0D5B;
        public const UInt32 GL_NAME_STACK_DEPTH               = 0x0D70;
        public const UInt32 GL_AUTO_NORMAL                    = 0x0D80;
        public const UInt32 GL_MAP1_COLOR_4                   = 0x0D90;
        public const UInt32 GL_MAP1_INDEX                     = 0x0D91;
        public const UInt32 GL_MAP1_NORMAL                    = 0x0D92;
        public const UInt32 GL_MAP1_TEXTURE_COORD_1           = 0x0D93;
        public const UInt32 GL_MAP1_TEXTURE_COORD_2           = 0x0D94;
        public const UInt32 GL_MAP1_TEXTURE_COORD_3           = 0x0D95;
        public const UInt32 GL_MAP1_TEXTURE_COORD_4           = 0x0D96;
        public const UInt32 GL_MAP1_VERTEX_3                  = 0x0D97;
        public const UInt32 GL_MAP1_VERTEX_4                  = 0x0D98;
        public const UInt32 GL_MAP2_COLOR_4                   = 0x0DB0;
        public const UInt32 GL_MAP2_INDEX                     = 0x0DB1;
        public const UInt32 GL_MAP2_NORMAL                    = 0x0DB2;
        public const UInt32 GL_MAP2_TEXTURE_COORD_1           = 0x0DB3;
        public const UInt32 GL_MAP2_TEXTURE_COORD_2           = 0x0DB4;
        public const UInt32 GL_MAP2_TEXTURE_COORD_3           = 0x0DB5;
        public const UInt32 GL_MAP2_TEXTURE_COORD_4           = 0x0DB6;
        public const UInt32 GL_MAP2_VERTEX_3                  = 0x0DB7;
        public const UInt32 GL_MAP2_VERTEX_4                  = 0x0DB8;
        public const UInt32 GL_MAP1_GRID_DOMAIN               = 0x0DD0;
        public const UInt32 GL_MAP1_GRID_SEGMENTS             = 0x0DD1;
        public const UInt32 GL_MAP2_GRID_DOMAIN               = 0x0DD2;
        public const UInt32 GL_MAP2_GRID_SEGMENTS             = 0x0DD3;
        public const UInt32 GL_TEXTURE_1D                     = 0x0DE0;
        public const UInt32 GL_TEXTURE_2D                     = 0x0DE1;
        public const UInt32 GL_FEEDBACK_BUFFER_POINTER        = 0x0DF0;
        public const UInt32 GL_FEEDBACK_BUFFER_SIZE           = 0x0DF1;
        public const UInt32 GL_FEEDBACK_BUFFER_TYPE           = 0x0DF2;
        public const UInt32 GL_SELECTION_BUFFER_POINTER       = 0x0DF3;
        public const UInt32 GL_SELECTION_BUFFER_SIZE          = 0x0DF4;
        public const UInt32 GL_TEXTURE_WIDTH                  = 0x1000;
        public const UInt32 GL_TEXTURE_HEIGHT                 = 0x1001;
        public const UInt32 GL_TEXTURE_INTERNAL_FORMAT        = 0x1003;
        public const UInt32 GL_TEXTURE_BORDER_COLOR           = 0x1004;
        public const UInt32 GL_TEXTURE_BORDER                 = 0x1005;
        public const UInt32 GL_DONT_CARE                      = 0x1100;
        public const UInt32 GL_FASTEST                        = 0x1101;
        public const UInt32 GL_NICEST                         = 0x1102;
        public const UInt32 GL_LIGHT0                         = 0x4000;
        public const UInt32 GL_LIGHT1                         = 0x4001;
        public const UInt32 GL_LIGHT2                         = 0x4002;
        public const UInt32 GL_LIGHT3                         = 0x4003;
        public const UInt32 GL_LIGHT4                         = 0x4004;
        public const UInt32 GL_LIGHT5                         = 0x4005;
        public const UInt32 GL_LIGHT6                         = 0x4006;
        public const UInt32 GL_LIGHT7                         = 0x4007;
        public const UInt32 GL_AMBIENT                        = 0x1200;
        public const UInt32 GL_DIFFUSE                        = 0x1201;
        public const UInt32 GL_SPECULAR                       = 0x1202;
        public const UInt32 GL_POSITION                       = 0x1203;
        public const UInt32 GL_SPOT_DIRECTION                 = 0x1204;
        public const UInt32 GL_SPOT_EXPONENT                  = 0x1205;
        public const UInt32 GL_SPOT_CUTOFF                    = 0x1206;
        public const UInt32 GL_CONSTANT_ATTENUATION           = 0x1207;
        public const UInt32 GL_LINEAR_ATTENUATION             = 0x1208;
        public const UInt32 GL_QUADRATIC_ATTENUATION          = 0x1209;
        public const UInt32 GL_COMPILE                        = 0x1300;
        public const UInt32 GL_COMPILE_AND_EXECUTE            = 0x1301;
        public const UInt32 GL_CLEAR                          = 0x1500;
        public const UInt32 GL_AND                            = 0x1501;
        public const UInt32 GL_AND_REVERSE                    = 0x1502;
        public const UInt32 GL_COPY                           = 0x1503;
        public const UInt32 GL_AND_INVERTED                   = 0x1504;
        public const UInt32 GL_NOOP                           = 0x1505;
        public const UInt32 GL_XOR                            = 0x1506;
        public const UInt32 GL_OR                             = 0x1507;
        public const UInt32 GL_NOR                            = 0x1508;
        public const UInt32 GL_EQUIV                          = 0x1509;
        public const UInt32 GL_INVERT                         = 0x150A;
        public const UInt32 GL_OR_REVERSE                     = 0x150B;
        public const UInt32 GL_COPY_INVERTED                  = 0x150C;
        public const UInt32 GL_OR_INVERTED                    = 0x150D;
        public const UInt32 GL_NAND                           = 0x150E;
        public const UInt32 GL_SET                            = 0x150F;
        public const UInt32 GL_EMISSION                       = 0x1600;
        public const UInt32 GL_SHININESS                      = 0x1601;
        public const UInt32 GL_AMBIENT_AND_DIFFUSE            = 0x1602;
        public const UInt32 GL_COLOR_INDEXES                  = 0x1603;
        public const UInt32 GL_MODELVIEW                      = 0x1700;
        public const UInt32 GL_PROJECTION                     = 0x1701;
        public const UInt32 GL_TEXTURE                        = 0x1702;
        public const UInt32 GL_COLOR                          = 0x1800;
        public const UInt32 GL_DEPTH                          = 0x1801;
        public const UInt32 GL_STENCIL                        = 0x1802;
        public const UInt32 GL_COLOR_INDEX                    = 0x1900;
        public const UInt32 GL_STENCIL_INDEX                  = 0x1901;
        public const UInt32 GL_DEPTH_COMPONENT                = 0x1902;
        public const UInt32 GL_RED                            = 0x1903;
        public const UInt32 GL_GREEN                          = 0x1904;
        public const UInt32 GL_BLUE                           = 0x1905;
        public const UInt32 GL_ALPHA                          = 0x1906;
        public const UInt32 GL_RGB                            = 0x1907;
        public const UInt32 GL_RGBA                           = 0x1908;
        public const UInt32 GL_LUMINANCE                      = 0x1909;
        public const UInt32 GL_LUMINANCE_ALPHA                = 0x190A;
        public const UInt32 GL_BITMAP                         = 0x1A00;
        public const UInt32 GL_POINT                          = 0x1B00;
        public const UInt32 GL_LINE                           = 0x1B01;
        public const UInt32 GL_FILL                           = 0x1B02;
        public const UInt32 GL_RENDER                         = 0x1C00;
        public const UInt32 GL_FEEDBACK                       = 0x1C01;
        public const UInt32 GL_SELECT                         = 0x1C02;
        public const UInt32 GL_FLAT                           = 0x1D00;
        public const UInt32 GL_SMOOTH                         = 0x1D01;
        public const UInt32 GL_KEEP                           = 0x1E00;
        public const UInt32 GL_REPLACE                        = 0x1E01;
        public const UInt32 GL_INCR                           = 0x1E02;
        public const UInt32 GL_DECR                           = 0x1E03;
        public const UInt32 GL_VENDOR                         = 0x1F00;
        public const UInt32 GL_RENDERER                       = 0x1F01;
        public const UInt32 GL_VERSION                        = 0x1F02;
        public const UInt32 GL_EXTENSIONS                     = 0x1F03;
        public const UInt32 GL_S                              = 0x2000;
        public const UInt32 GL_T                              = 0x2001;
        public const UInt32 GL_R                              = 0x2002;
        public const UInt32 GL_Q                              = 0x2003;
        public const UInt32 GL_MODULATE                       = 0x2100;
        public const UInt32 GL_DECAL                          = 0x2101;
        public const UInt32 GL_TEXTURE_ENV_MODE               = 0x2200;
        public const UInt32 GL_TEXTURE_ENV_COLOR              = 0x2201;
        public const UInt32 GL_TEXTURE_ENV                    = 0x2300;
        public const UInt32 GL_EYE_LINEAR                     = 0x2400;
        public const UInt32 GL_OBJECT_LINEAR                  = 0x2401;
        public const UInt32 GL_SPHERE_MAP                     = 0x2402;
        public const UInt32 GL_TEXTURE_GEN_MODE               = 0x2500;
        public const UInt32 GL_OBJECT_PLANE                   = 0x2501;
        public const UInt32 GL_EYE_PLANE                      = 0x2502;
        public const UInt32 GL_NEAREST                        = 0x2600;
        public const UInt32 GL_LINEAR                         = 0x2601;
        public const UInt32 GL_NEAREST_MIPMAP_NEAREST         = 0x2700;
        public const UInt32 GL_LINEAR_MIPMAP_NEAREST          = 0x2701;
        public const UInt32 GL_NEAREST_MIPMAP_LINEAR          = 0x2702;
        public const UInt32 GL_LINEAR_MIPMAP_LINEAR           = 0x2703;
        public const UInt32 GL_TEXTURE_MAG_FILTER             = 0x2800;
        public const UInt32 GL_TEXTURE_MIN_FILTER             = 0x2801;
        public const UInt32 GL_TEXTURE_WRAP_S                 = 0x2802;
        public const UInt32 GL_TEXTURE_WRAP_T                 = 0x2803;
        public const UInt32 GL_CLAMP                          = 0x2900;
        public const UInt32 GL_REPEAT                         = 0x2901;
        public const UInt32 GL_CLIENT_PIXEL_STORE_BIT         = 0x00000001;
        public const UInt32 GL_CLIENT_VERTEX_ARRAY_BIT        = 0x00000002;
        public const UInt32 GL_CLIENT_ALL_ATTRIB_BITS         = 0xffffffff;
        public const UInt32 GL_POLYGON_OFFSET_FACTOR          = 0x8038;
        public const UInt32 GL_POLYGON_OFFSET_UNITS           = 0x2A00;
        public const UInt32 GL_POLYGON_OFFSET_POINT           = 0x2A01;
        public const UInt32 GL_POLYGON_OFFSET_LINE            = 0x2A02;
        public const UInt32 GL_POLYGON_OFFSET_FILL            = 0x8037;
        public const UInt32 GL_ALPHA4                         = 0x803B;
        public const UInt32 GL_ALPHA8                         = 0x803C;
        public const UInt32 GL_ALPHA12                        = 0x803D;
        public const UInt32 GL_ALPHA16                        = 0x803E;
        public const UInt32 GL_LUMINANCE4                     = 0x803F;
        public const UInt32 GL_LUMINANCE8                     = 0x8040;
        public const UInt32 GL_LUMINANCE12                    = 0x8041;
        public const UInt32 GL_LUMINANCE16                    = 0x8042;
        public const UInt32 GL_LUMINANCE4_ALPHA4              = 0x8043;
        public const UInt32 GL_LUMINANCE6_ALPHA2              = 0x8044;
        public const UInt32 GL_LUMINANCE8_ALPHA8              = 0x8045;
        public const UInt32 GL_LUMINANCE12_ALPHA4             = 0x8046;
        public const UInt32 GL_LUMINANCE12_ALPHA12            = 0x8047;
        public const UInt32 GL_LUMINANCE16_ALPHA16            = 0x8048;
        public const UInt32 GL_INTENSITY                      = 0x8049;
        public const UInt32 GL_INTENSITY4                     = 0x804A;
        public const UInt32 GL_INTENSITY8                     = 0x804B;
        public const UInt32 GL_INTENSITY12                    = 0x804C;
        public const UInt32 GL_INTENSITY16                    = 0x804D;
        public const UInt32 GL_R3_G3_B2                       = 0x2A10;
        public const UInt32 GL_RGB4                           = 0x804F;
        public const UInt32 GL_RGB5                           = 0x8050;
        public const UInt32 GL_RGB8                           = 0x8051;
        public const UInt32 GL_RGB10                          = 0x8052;
        public const UInt32 GL_RGB12                          = 0x8053;
        public const UInt32 GL_RGB16                          = 0x8054;
        public const UInt32 GL_RGBA2                          = 0x8055;
        public const UInt32 GL_RGBA4                          = 0x8056;
        public const UInt32 GL_RGB5_A1                        = 0x8057;
        public const UInt32 GL_RGBA8                          = 0x8058;
        public const UInt32 GL_RGB10_A2                       = 0x8059;
        public const UInt32 GL_RGBA12                         = 0x805A;
        public const UInt32 GL_RGBA16                         = 0x805B;
        public const UInt32 GL_TEXTURE_RED_SIZE               = 0x805C;
        public const UInt32 GL_TEXTURE_GREEN_SIZE             = 0x805D;
        public const UInt32 GL_TEXTURE_BLUE_SIZE              = 0x805E;
        public const UInt32 GL_TEXTURE_ALPHA_SIZE             = 0x805F;
        public const UInt32 GL_TEXTURE_LUMINANCE_SIZE         = 0x8060;
        public const UInt32 GL_TEXTURE_INTENSITY_SIZE         = 0x8061;
        public const UInt32 GL_PROXY_TEXTURE_1D               = 0x8063;
        public const UInt32 GL_PROXY_TEXTURE_2D               = 0x8064;
        public const UInt32 GL_TEXTURE_PRIORITY               = 0x8066;
        public const UInt32 GL_TEXTURE_RESIDENT               = 0x8067;
        public const UInt32 GL_TEXTURE_BINDING_1D             = 0x8068;
        public const UInt32 GL_TEXTURE_BINDING_2D             = 0x8069;
        public const UInt32 GL_VERTEX_ARRAY                   = 0x8074;
        public const UInt32 GL_NORMAL_ARRAY                   = 0x8075;
        public const UInt32 GL_COLOR_ARRAY                    = 0x8076;
        public const UInt32 GL_INDEX_ARRAY                    = 0x8077;
        public const UInt32 GL_TEXTURE_COORD_ARRAY            = 0x8078;
        public const UInt32 GL_EDGE_FLAG_ARRAY                = 0x8079;
        public const UInt32 GL_VERTEX_ARRAY_SIZE              = 0x807A;
        public const UInt32 GL_VERTEX_ARRAY_TYPE              = 0x807B;
        public const UInt32 GL_VERTEX_ARRAY_STRIDE            = 0x807C;
        public const UInt32 GL_NORMAL_ARRAY_TYPE              = 0x807E;
        public const UInt32 GL_NORMAL_ARRAY_STRIDE            = 0x807F;
        public const UInt32 GL_COLOR_ARRAY_SIZE               = 0x8081;
        public const UInt32 GL_COLOR_ARRAY_TYPE               = 0x8082;
        public const UInt32 GL_COLOR_ARRAY_STRIDE             = 0x8083;
        public const UInt32 GL_INDEX_ARRAY_TYPE               = 0x8085;
        public const UInt32 GL_INDEX_ARRAY_STRIDE             = 0x8086;
        public const UInt32 GL_TEXTURE_COORD_ARRAY_SIZE       = 0x8088;
        public const UInt32 GL_TEXTURE_COORD_ARRAY_TYPE       = 0x8089;
        public const UInt32 GL_TEXTURE_COORD_ARRAY_STRIDE     = 0x808A;
        public const UInt32 GL_EDGE_FLAG_ARRAY_STRIDE         = 0x808C;
        public const UInt32 GL_VERTEX_ARRAY_POINTER           = 0x808E;
        public const UInt32 GL_NORMAL_ARRAY_POINTER           = 0x808F;
        public const UInt32 GL_COLOR_ARRAY_POINTER            = 0x8090;
        public const UInt32 GL_INDEX_ARRAY_POINTER            = 0x8091;
        public const UInt32 GL_TEXTURE_COORD_ARRAY_POINTER    = 0x8092;
        public const UInt32 GL_EDGE_FLAG_ARRAY_POINTER        = 0x8093;
        public const UInt32 GL_V2F                            = 0x2A20;
        public const UInt32 GL_V3F                            = 0x2A21;
        public const UInt32 GL_C4UB_V2F                       = 0x2A22;
        public const UInt32 GL_C4UB_V3F                       = 0x2A23;
        public const UInt32 GL_C3F_V3F                        = 0x2A24;
        public const UInt32 GL_N3F_V3F                        = 0x2A25;
        public const UInt32 GL_C4F_N3F_V3F                    = 0x2A26;
        public const UInt32 GL_T2F_V3F                        = 0x2A27;
        public const UInt32 GL_T4F_V4F                        = 0x2A28;
        public const UInt32 GL_T2F_C4UB_V3F                   = 0x2A29;
        public const UInt32 GL_T2F_C3F_V3F                    = 0x2A2A;
        public const UInt32 GL_T2F_N3F_V3F                    = 0x2A2B;
        public const UInt32 GL_T2F_C4F_N3F_V3F                = 0x2A2C;
        public const UInt32 GL_T4F_C4F_N3F_V4F                = 0x2A2D;
        public const UInt32 GL_EXT_vertex_array               = 1;
        public const UInt32 GL_EXT_bgra                       = 1;
        public const UInt32 GL_EXT_paletted_texture           = 1;
        public const UInt32 GL_WIN_swap_hint                  = 1;
        public const UInt32 GL_WIN_draw_range_elements        = 1;
        public const UInt32 GL_VERTEX_ARRAY_EXT               = 0x8074;
        public const UInt32 GL_NORMAL_ARRAY_EXT               = 0x8075;
        public const UInt32 GL_COLOR_ARRAY_EXT                = 0x8076;
        public const UInt32 GL_INDEX_ARRAY_EXT                = 0x8077;
        public const UInt32 GL_TEXTURE_COORD_ARRAY_EXT        = 0x8078;
        public const UInt32 GL_EDGE_FLAG_ARRAY_EXT            = 0x8079;
        public const UInt32 GL_VERTEX_ARRAY_SIZE_EXT          = 0x807A;
        public const UInt32 GL_VERTEX_ARRAY_TYPE_EXT          = 0x807B;
        public const UInt32 GL_VERTEX_ARRAY_STRIDE_EXT        = 0x807C;
        public const UInt32 GL_VERTEX_ARRAY_COUNT_EXT         = 0x807D;
        public const UInt32 GL_NORMAL_ARRAY_TYPE_EXT          = 0x807E;
        public const UInt32 GL_NORMAL_ARRAY_STRIDE_EXT        = 0x807F;
        public const UInt32 GL_NORMAL_ARRAY_COUNT_EXT         = 0x8080;
        public const UInt32 GL_COLOR_ARRAY_SIZE_EXT           = 0x8081;
        public const UInt32 GL_COLOR_ARRAY_TYPE_EXT           = 0x8082;
        public const UInt32 GL_COLOR_ARRAY_STRIDE_EXT         = 0x8083;
        public const UInt32 GL_COLOR_ARRAY_COUNT_EXT          = 0x8084;
        public const UInt32 GL_INDEX_ARRAY_TYPE_EXT           = 0x8085;
        public const UInt32 GL_INDEX_ARRAY_STRIDE_EXT         = 0x8086;
        public const UInt32 GL_INDEX_ARRAY_COUNT_EXT          = 0x8087;
        public const UInt32 GL_TEXTURE_COORD_ARRAY_SIZE_EXT   = 0x8088;
        public const UInt32 GL_TEXTURE_COORD_ARRAY_TYPE_EXT   = 0x8089;
        public const UInt32 GL_TEXTURE_COORD_ARRAY_STRIDE_EXT = 0x808A;
        public const UInt32 GL_TEXTURE_COORD_ARRAY_COUNT_EXT  = 0x808B;
        public const UInt32 GL_EDGE_FLAG_ARRAY_STRIDE_EXT     = 0x808C;
        public const UInt32 GL_EDGE_FLAG_ARRAY_COUNT_EXT      = 0x808D;
        public const UInt32 GL_VERTEX_ARRAY_POINTER_EXT       = 0x808E;
        public const UInt32 GL_NORMAL_ARRAY_POINTER_EXT       = 0x808F;
        public const UInt32 GL_COLOR_ARRAY_POINTER_EXT        = 0x8090;
        public const UInt32 GL_INDEX_ARRAY_POINTER_EXT        = 0x8091;
        public const UInt32 GL_TEXTURE_COORD_ARRAY_POINTER_EXT = 0x8092;
        public const UInt32 GL_EDGE_FLAG_ARRAY_POINTER_EXT    = 0x8093;
        public const UInt32 GL_DOUBLE_EXT                     = GL_DOUBLE;
        public const UInt32 GL_BGR_EXT                        = 0x80E0;
        public const UInt32 GL_BGRA_EXT                       = 0x80E1;
        public const UInt32 GL_COLOR_TABLE_FORMAT_EXT         = 0x80D8;
        public const UInt32 GL_COLOR_TABLE_WIDTH_EXT          = 0x80D9;
        public const UInt32 GL_COLOR_TABLE_RED_SIZE_EXT       = 0x80DA;
        public const UInt32 GL_COLOR_TABLE_GREEN_SIZE_EXT     = 0x80DB;
        public const UInt32 GL_COLOR_TABLE_BLUE_SIZE_EXT      = 0x80DC;
        public const UInt32 GL_COLOR_TABLE_ALPHA_SIZE_EXT     = 0x80DD;
        public const UInt32 GL_COLOR_TABLE_LUMINANCE_SIZE_EXT = 0x80DE;
        public const UInt32 GL_COLOR_TABLE_INTENSITY_SIZE_EXT = 0x80DF;
        public const UInt32 GL_COLOR_INDEX1_EXT               = 0x80E2;
        public const UInt32 GL_COLOR_INDEX2_EXT               = 0x80E3;
        public const UInt32 GL_COLOR_INDEX4_EXT               = 0x80E4;
        public const UInt32 GL_COLOR_INDEX8_EXT               = 0x80E5;
        public const UInt32 GL_COLOR_INDEX12_EXT              = 0x80E6;
        public const UInt32 GL_COLOR_INDEX16_EXT              = 0x80E7;
        public const UInt32 GL_MAX_ELEMENTS_VERTICES_WIN      = 0x80E8;
        public const UInt32 GL_MAX_ELEMENTS_INDICES_WIN       = 0x80E9;
        public const UInt32 GL_PHONG_WIN                      = 0x80EA;
        public const UInt32 GL_PHONG_HINT_WIN                 = 0x80EB;
        public const UInt32 GL_FOG_SPECULAR_TEXTURE_WIN       = 0x80EC;
        public const UInt32 GL_LOGIC_OP = GL_INDEX_LOGIC_OP;
        public const UInt32 GL_TEXTURE_COMPONENTS = GL_TEXTURE_INTERNAL_FORMAT;
    }
}
