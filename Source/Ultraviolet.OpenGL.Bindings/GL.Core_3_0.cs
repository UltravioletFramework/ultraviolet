using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;

namespace Ultraviolet.OpenGL.Bindings
{
    public static unsafe partial class GL
    {
        [MonoNativeFunctionWrapper]
        private delegate void glColorMaskiDelegate(uint buf, [MarshalAs(UnmanagedType.I1)] bool red, [MarshalAs(UnmanagedType.I1)] bool green, [MarshalAs(UnmanagedType.I1)] bool blue, [MarshalAs(UnmanagedType.I1)] bool alpha);
        [Require(MinVersion = "3.0")]
        private static glColorMaskiDelegate glColorMaski = null;

        public static void ColorMaski(uint buf, bool red, bool green, bool blue, bool alpha) { glColorMaski(buf, red, green, blue, alpha); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetBooleani_vDelegate(uint pname, uint index, IntPtr data);
        [Require(MinVersion = "3.0")]
        private static glGetBooleani_vDelegate glGetBooleani_v = null;

        public static void GetBooleani_v(uint pname, uint index, bool* data) { glGetBooleani_v(pname, index, (IntPtr)data); }

        public static bool GetBooleani(uint pname, uint index)
        {
            bool value;
            glGetBooleani_v(pname, index, (IntPtr)(&value));
            return value;
        }

        [MonoNativeFunctionWrapper]
        private delegate void glEnableiDelegate(uint cap, uint index);
        [Require(MinVersion = "3.0")]
        private static glEnableiDelegate glEnablei = null;

        public static void Enablei(uint cap, uint index) { glEnablei(cap, index); }

        [MonoNativeFunctionWrapper]
        private delegate void glDisableiDelegate(uint cap, uint index);
        [Require(MinVersion = "3.0")]
        private static glDisableiDelegate glDisablei = null;

        public static void Disablei(uint cap, uint index) { glDisablei(cap, index); }

        [MonoNativeFunctionWrapper]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool glIsEnablediDelegate(uint cap, uint index);
        [Require(MinVersion = "3.0")]
        private static glIsEnablediDelegate glIsEnabledi = null;

        public static bool IsEnabledi(uint cap, uint index) { return glIsEnabledi(cap, index); }

        [MonoNativeFunctionWrapper]
        private delegate void glBeginTransformFeedbackDelegate(uint primitiveMode);
        [Require(MinVersion = "3.0")]
        private static glBeginTransformFeedbackDelegate glBeginTransformFeedback = null;

        public static void BeginTransformFeedback(uint primitiveMode) { glBeginTransformFeedback(primitiveMode); }

        [MonoNativeFunctionWrapper]
        private delegate void glEndTransformFeedbackDelegate();
        [MonoNativeFunctionWrapper]
        private delegate void glTransformFeedbackVaryingsDelegate(uint program, int count, IntPtr varyings, uint bufferMode);
        [Require(MinVersion = "3.0")]
        private static glTransformFeedbackVaryingsDelegate glTransformFeedbackVaryings = null;

        public static void TransformFeedbackVaryings(uint program, int count, sbyte** varyings, uint bufferMode) =>
            glTransformFeedbackVaryings(program, count, (IntPtr)varyings, bufferMode);

        [MonoNativeFunctionWrapper]
        private delegate void glGetTransformFeedbackVaryingDelegate(uint program, uint index, int bufSize, IntPtr length, IntPtr size, IntPtr type, IntPtr name);
        [Require(MinVersion = "3.0")]
        private static glGetTransformFeedbackVaryingDelegate glGetTransformFeedbackVarying = null;

        public static void GetTransformFeedbackVarying(uint program, uint index, int bufSize, int* length, int* size, uint* type, sbyte* name) =>
            glGetTransformFeedbackVarying(program, index, bufSize, (IntPtr)length, (IntPtr)size, (IntPtr)type, (IntPtr)name);

        [MonoNativeFunctionWrapper]
        private delegate void glClampColorDelegate(uint target, uint clamp);
        [Require(MinVersion = "3.0")]
        private static glClampColorDelegate glClampColor = null;

        public static void ClampColor(uint target, uint clamp) { glClampColor(target, clamp); }

        [MonoNativeFunctionWrapper]
        private delegate void glBeginConditionalRenderDelegate(uint id, uint mode);
        [Require(MinVersion = "3.0")]
        private static glBeginConditionalRenderDelegate glBeginConditionalRender = null;

        public static void BeginConditionalRender(uint id, uint mode) { glBeginConditionalRender(id, mode); }

        [MonoNativeFunctionWrapper]
        private delegate void glEndConditionalRenderDelegate();
        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttribI1iDelegate(uint index, int v0);
        [Require(MinVersion = "3.0")]
        private static glVertexAttribI1iDelegate glVertexAttribI1i = null;

        public static void VertexAttribI1i(uint index, int v0) { glVertexAttribI1i(index, v0); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttribI2iDelegate(uint index, int v0, int v1);
        [Require(MinVersion = "3.0")]
        private static glVertexAttribI2iDelegate glVertexAttribI2i = null;

        public static void VertexAttribI2i(uint index, int v0, int v1) { glVertexAttribI2i(index, v0, v1); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttribI3iDelegate(uint index, int v0, int v1, int v2);
        [Require(MinVersion = "3.0")]
        private static glVertexAttribI3iDelegate glVertexAttribI3i = null;

        public static void VertexAttribI3i(uint index, int v0, int v1, int v2) { glVertexAttribI3i(index, v0, v1, v2); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttribI4iDelegate(uint index, int v0, int v1, int v2, int v3);
        [Require(MinVersion = "3.0")]
        private static glVertexAttribI4iDelegate glVertexAttribI4i = null;

        public static void VertexAttribI4i(uint index, int v0, int v1, int v2, int v3) { glVertexAttribI4i(index, v0, v1, v2, v3); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttribI1uiDelegate(uint index, uint v0);
        [Require(MinVersion = "3.0")]
        private static glVertexAttribI1uiDelegate glVertexAttribI1ui = null;

        public static void VertexAttribI1ui(uint index, uint v0) { glVertexAttribI1ui(index, v0); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttribI2uiDelegate(uint index, uint v0, uint v1);
        [Require(MinVersion = "3.0")]
        private static glVertexAttribI2uiDelegate glVertexAttribI2ui = null;

        public static void VertexAttribI2ui(uint index, uint v0, uint v1) { glVertexAttribI2ui(index, v0, v1); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttribI3uiDelegate(uint index, uint v0, uint v1, uint v2);
        [Require(MinVersion = "3.0")]
        private static glVertexAttribI3uiDelegate glVertexAttribI3ui = null;

        public static void VertexAttribI3ui(uint index, uint v0, uint v1, uint v2) { glVertexAttribI3ui(index, v0, v1, v2); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttribI4uiDelegate(uint index, uint v0, uint v1, uint v2, uint v3);
        [Require(MinVersion = "3.0")]
        private static glVertexAttribI4uiDelegate glVertexAttribI4ui = null;

        public static void VertexAttribI4ui(uint index, uint v0, uint v1, uint v2, uint v3) { glVertexAttribI4ui(index, v0, v1, v2, v3); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttribI1ivDelegate(uint index, IntPtr v);
        [Require(MinVersion = "3.0")]
        private static glVertexAttribI1ivDelegate glVertexAttribI1iv = null;

        public static void VertexAttribI1iv(uint index, int* v) { glVertexAttribI1iv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttribI2ivDelegate(uint index, IntPtr v);
        [Require(MinVersion = "3.0")]
        private static glVertexAttribI2ivDelegate glVertexAttribI2iv = null;

        public static void VertexAttribI2iv(uint index, int* v) { glVertexAttribI2iv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttribI3ivDelegate(uint index, IntPtr v);
        [Require(MinVersion = "3.0")]
        private static glVertexAttribI3ivDelegate glVertexAttribI3iv = null;

        public static void VertexAttribI3iv(uint index, int* v) { glVertexAttribI3iv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttribI4ivDelegate(uint index, IntPtr v);
        [Require(MinVersion = "3.0")]
        private static glVertexAttribI4ivDelegate glVertexAttribI4iv = null;

        public static void VertexAttribI4iv(uint index, int* v) { glVertexAttribI4iv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttribI1uivDelegate(uint index, IntPtr v);
        [Require(MinVersion = "3.0")]
        private static glVertexAttribI1uivDelegate glVertexAttribI1uiv = null;

        public static void VertexAttribI1uiv(uint index, uint* v) { glVertexAttribI1uiv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttribI2uivDelegate(uint index, IntPtr v);
        [Require(MinVersion = "3.0")]
        private static glVertexAttribI2uivDelegate glVertexAttribI2uiv = null;

        public static void VertexAttribI2uiv(uint index, uint* v) { glVertexAttribI2uiv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttribI3uivDelegate(uint index, IntPtr v);
        [Require(MinVersion = "3.0")]
        private static glVertexAttribI3uivDelegate glVertexAttribI3uiv = null;

        public static void VertexAttribI3uiv(uint index, uint* v) { glVertexAttribI3uiv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttribI4uivDelegate(uint index, IntPtr v);
        [Require(MinVersion = "3.0")]
        private static glVertexAttribI4uivDelegate glVertexAttribI4uiv = null;

        public static void VertexAttribI4uiv(uint index, uint* v) { glVertexAttribI4uiv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttribI4bvDelegate(uint index, IntPtr v);
        [Require(MinVersion = "3.0")]
        private static glVertexAttribI4bvDelegate glVertexAttribI4bv = null;

        public static void VertexAttribI4bv(uint index, sbyte* v) { glVertexAttribI4bv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttribI4svDelegate(uint index, IntPtr v);
        [Require(MinVersion = "3.0")]
        private static glVertexAttribI4svDelegate glVertexAttribI4sv = null;

        public static void VertexAttribI4sv(uint index, short* v) { glVertexAttribI4sv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttribI4ubvDelegate(uint index, IntPtr v);
        [Require(MinVersion = "3.0")]
        private static glVertexAttribI4ubvDelegate glVertexAttribI4ubv = null;

        public static void VertexAttribI4ubv(uint index, byte* v) { glVertexAttribI4ubv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttribI4usvDelegate(uint index, IntPtr v);
        [Require(MinVersion = "3.0")]
        private static glVertexAttribI4usvDelegate glVertexAttribI4usv = null;

        public static void VertexAttribI4usv(uint index, ushort* v) { glVertexAttribI4usv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttribIPointerDelegate(uint index, int size, uint type, int stride, IntPtr pointer);
        [Require(MinVersion = "3.0")]
        private static glVertexAttribIPointerDelegate glVertexAttribIPointer = null;

        public static void VertexAttribIPointer(uint index, int size, uint type, int stride, void* pointer) { glVertexAttribIPointer(index, size, type, stride, (IntPtr)pointer); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetVertexAttribIivDelegate(uint index, uint pname, IntPtr @params);
        [Require(MinVersion = "3.0")]
        private static glGetVertexAttribIivDelegate glGetVertexAttribIiv = null;

        public static void GetVertexAttribIiv(uint index, uint pname, int* @params) { glGetVertexAttribIiv(index, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetVertexAttribIuivDelegate(uint index, uint pname, IntPtr @params);
        [Require(MinVersion = "3.0")]
        private static glGetVertexAttribIuivDelegate glGetVertexAttribIuiv = null;

        public static void GetVertexAttribIuiv(uint index, uint pname, uint* @params) { glGetVertexAttribIuiv(index, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetUniformuivDelegate(uint program, int location, IntPtr @params);
        [Require(MinVersion = "3.0")]
        private static glGetUniformuivDelegate glGetUniformuiv = null;

        public static void GetUniformuiv(uint program, int location, uint* @params) { glGetUniformuiv(program, location, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glBindFragDataLocationDelegate(uint program, uint colorNumber, IntPtr name);
        [Require(MinVersion = "3.0")]
        private static glBindFragDataLocationDelegate glBindFragDataLocation = null;

        public static void BindFragDataLocation(uint program, uint colorNumber, sbyte* name) { glBindFragDataLocation(program, colorNumber, (IntPtr)name); }

        [MonoNativeFunctionWrapper]
        private delegate int glGetFragDataLocationDelegate(uint program, IntPtr name);
        [Require(MinVersion = "3.0")]
        private static glGetFragDataLocationDelegate glGetFragDataLocation = null;

        public static int GetFragDataLocation(uint program, sbyte* name) { return glGetFragDataLocation(program, (IntPtr)name); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniform1uiDelegate(int location, uint v0);
        [Require(MinVersion = "3.0")]
        private static glUniform1uiDelegate glUniform1ui = null;

        public static void Uniform1ui(int location, uint v0) { glUniform1ui(location, v0); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniform2uiDelegate(int location, uint v0, uint v1);
        [Require(MinVersion = "3.0")]
        private static glUniform2uiDelegate glUniform2ui = null;

        public static void Uniform2ui(int location, uint v0, uint v1) { glUniform2ui(location, v0, v1); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniform3uiDelegate(int location, uint v0, uint v1, uint v2);
        [Require(MinVersion = "3.0")]
        private static glUniform3uiDelegate glUniform3ui = null;

        public static void Uniform3ui(int location, uint v0, uint v1, uint v2) { glUniform3ui(location, v0, v1, v2); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniform4uiDelegate(int location, uint v0, uint v1, uint v2, uint v3);
        [Require(MinVersion = "3.0")]
        private static glUniform4uiDelegate glUniform4ui = null;

        public static void Uniform4ui(int location, uint v0, uint v1, uint v2, uint v3) { glUniform4ui(location, v0, v1, v2, v3); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniform1uivDelegate(int location, int count, IntPtr value);
        [Require(MinVersion = "3.0")]
        private static glUniform1uivDelegate glUniform1uiv = null;

        public static void Uniform1uiv(int location, int count, uint* value) { glUniform1uiv(location, count, (IntPtr)value); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniform2uivDelegate(int location, int count, IntPtr value);
        [Require(MinVersion = "3.0")]
        private static glUniform2uivDelegate glUniform2uiv = null;

        public static void Uniform2uiv(int location, int count, uint* value) { glUniform2uiv(location, count, (IntPtr)value); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniform3uivDelegate(int location, int count, IntPtr value);
        [Require(MinVersion = "3.0")]
        private static glUniform3uivDelegate glUniform3uiv = null;

        public static void Uniform3uiv(int location, int count, uint* value) { glUniform3uiv(location, count, (IntPtr)value); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniform4uivDelegate(int location, int count, IntPtr value);
        [Require(MinVersion = "3.0")]
        private static glUniform4uivDelegate glUniform4uiv = null;

        public static void Uniform4uiv(int location, int count, uint* value) { glUniform4uiv(location, count, (IntPtr)value); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexParameterIivDelegate(uint target, uint pname, IntPtr @params);
        [Require(MinVersion = "3.0")]
        private static glTexParameterIivDelegate glTexParameterIiv = null;

        public static void TexParameterIiv(uint target, uint pname, int* @params) { glTexParameterIiv(target, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glTexParameterIuivDelegate(uint target, uint pname, IntPtr @params);
        [Require(MinVersion = "3.0")]
        private static glTexParameterIuivDelegate glTexParameterIuiv = null;

        public static void TexParameterIuiv(uint target, uint pname, uint* @params) { glTexParameterIuiv(target, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetTexParameterIivDelegate(uint target, uint pname, IntPtr @params);
        [Require(MinVersion = "3.0")]
        private static glGetTexParameterIivDelegate glGetTexParameterIiv = null;

        public static void GetTexParameterIiv(uint target, uint pname, int* @params) { glGetTexParameterIiv(target, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetTexParameterIuivDelegate(uint target, uint pname, IntPtr @params);
        [Require(MinVersion = "3.0")]
        private static glGetTexParameterIuivDelegate glGetTexParameterIuiv = null;

        public static void GetTexParameterIuiv(uint target, uint pname, uint* @params) { glGetTexParameterIuiv(target, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glClearBufferivDelegate(uint buffer, int drawBuffer, IntPtr value);
        [Require(MinVersion = "3.0")]
        private static glClearBufferivDelegate glClearBufferiv = null;

        public static void ClearBufferiv(uint buffer, int drawBuffer, int* value) { glClearBufferiv(buffer, drawBuffer, (IntPtr)value); }

        [MonoNativeFunctionWrapper]
        private delegate void glClearBufferuivDelegate(uint buffer, int drawBuffer, IntPtr value);
        [Require(MinVersion = "3.0")]
        private static glClearBufferuivDelegate glClearBufferuiv = null;

        public static void ClearBufferuiv(uint buffer, int drawBuffer, uint* value) { glClearBufferuiv(buffer, drawBuffer, (IntPtr)value); }

        [MonoNativeFunctionWrapper]
        private delegate void glClearBufferfvDelegate(uint buffer, int drawBuffer, IntPtr value);
        [Require(MinVersion = "3.0")]
        private static glClearBufferfvDelegate glClearBufferfv = null;

        public static void ClearBufferfv(uint buffer, int drawBuffer, float* value) { glClearBufferfv(buffer, drawBuffer, (IntPtr)value); }

        [MonoNativeFunctionWrapper]
        private delegate void glClearBufferfiDelegate(uint buffer, int drawBuffer, float depth, int stencil);
        [Require(MinVersion = "3.0")]
        private static glClearBufferfiDelegate glClearBufferfi = null;

        public static void ClearBufferfi(uint buffer, int drawBuffer, float depth, int stencil) { glClearBufferfi(buffer, drawBuffer, depth, stencil); }

        [MonoNativeFunctionWrapper]
        private delegate IntPtr glGetStringiDelegate(uint name, uint index);
        [Require(MinVersion = "3.0")]
        private static glGetStringiDelegate glGetStringi = null;

        public static String GetStringi(uint name, uint index)
        {
            return Marshal.PtrToStringAnsi((IntPtr)glGetStringi(name, index));
        }

        public const UInt32 GL_MAX_CLIP_DISTANCES = GL_MAX_CLIP_PLANES;
        public const UInt32 GL_CLIP_DISTANCE5 = GL_CLIP_PLANE5;
        public const UInt32 GL_CLIP_DISTANCE1 = GL_CLIP_PLANE1;
        public const UInt32 GL_CLIP_DISTANCE3 = GL_CLIP_PLANE3;
        public const UInt32 GL_COMPARE_REF_TO_TEXTURE = GL_COMPARE_R_TO_TEXTURE_ARB;
        public const UInt32 GL_CLIP_DISTANCE0 = GL_CLIP_PLANE0;
        public const UInt32 GL_CLIP_DISTANCE4 = GL_CLIP_PLANE4;
        public const UInt32 GL_CLIP_DISTANCE2 = GL_CLIP_PLANE2;
        public const UInt32 GL_MAX_VARYING_COMPONENTS = GL_MAX_VARYING_FLOATS;
        public const UInt32 GL_CONTEXT_FLAG_FORWARD_COMPATIBLE_BIT = 0x0001;
        public const UInt32 GL_MAJOR_VERSION = 0x821B;
        public const UInt32 GL_MINOR_VERSION = 0x821C;
        public const UInt32 GL_NUM_EXTENSIONS = 0x821D;
        public const UInt32 GL_CONTEXT_FLAGS = 0x821E;
        public const UInt32 GL_DEPTH_BUFFER = 0x8223;
        public const UInt32 GL_STENCIL_BUFFER = 0x8224;
        public const UInt32 GL_RGBA32F = 0x8814;
        public const UInt32 GL_RGB32F = 0x8815;
        public const UInt32 GL_RGBA16F = 0x881A;
        public const UInt32 GL_RGB16F = 0x881B;
        public const UInt32 GL_VERTEX_ATTRIB_ARRAY_INTEGER = 0x88FD;
        public const UInt32 GL_MAX_ARRAY_TEXTURE_LAYERS = 0x88FF;
        public const UInt32 GL_MIN_PROGRAM_TEXEL_OFFSET = 0x8904;
        public const UInt32 GL_MAX_PROGRAM_TEXEL_OFFSET = 0x8905;
        public const UInt32 GL_CLAMP_VERTEX_COLOR = 0x891A;
        public const UInt32 GL_CLAMP_FRAGMENT_COLOR = 0x891B;
        public const UInt32 GL_CLAMP_READ_COLOR = 0x891C;
        public const UInt32 GL_FIXED_ONLY = 0x891D;
        public const UInt32 GL_TEXTURE_RED_TYPE = 0x8C10;
        public const UInt32 GL_TEXTURE_GREEN_TYPE = 0x8C11;
        public const UInt32 GL_TEXTURE_BLUE_TYPE = 0x8C12;
        public const UInt32 GL_TEXTURE_ALPHA_TYPE = 0x8C13;
        public const UInt32 GL_TEXTURE_LUMINANCE_TYPE = 0x8C14;
        public const UInt32 GL_TEXTURE_INTENSITY_TYPE = 0x8C15;
        public const UInt32 GL_TEXTURE_DEPTH_TYPE = 0x8C16;
        public const UInt32 GL_TEXTURE_1D_ARRAY = 0x8C18;
        public const UInt32 GL_PROXY_TEXTURE_1D_ARRAY = 0x8C19;
        public const UInt32 GL_TEXTURE_2D_ARRAY = 0x8C1A;
        public const UInt32 GL_PROXY_TEXTURE_2D_ARRAY = 0x8C1B;
        public const UInt32 GL_TEXTURE_BINDING_1D_ARRAY = 0x8C1C;
        public const UInt32 GL_TEXTURE_BINDING_2D_ARRAY = 0x8C1D;
        public const UInt32 GL_R11F_G11F_B10F = 0x8C3A;
        public const UInt32 GL_UNSIGNED_INT_10F_11F_11F_REV = 0x8C3B;
        public const UInt32 GL_RGB9_E5 = 0x8C3D;
        public const UInt32 GL_UNSIGNED_INT_5_9_9_9_REV = 0x8C3E;
        public const UInt32 GL_TEXTURE_SHARED_SIZE = 0x8C3F;
        public const UInt32 GL_TRANSFORM_FEEDBACK_VARYING_MAX_LENGTH = 0x8C76;
        public const UInt32 GL_TRANSFORM_FEEDBACK_BUFFER_MODE = 0x8C7F;
        public const UInt32 GL_MAX_TRANSFORM_FEEDBACK_SEPARATE_COMPONENTS = 0x8C80;
        public const UInt32 GL_TRANSFORM_FEEDBACK_VARYINGS = 0x8C83;
        public const UInt32 GL_TRANSFORM_FEEDBACK_BUFFER_START = 0x8C84;
        public const UInt32 GL_TRANSFORM_FEEDBACK_BUFFER_SIZE = 0x8C85;
        public const UInt32 GL_PRIMITIVES_GENERATED = 0x8C87;
        public const UInt32 GL_TRANSFORM_FEEDBACK_PRIMITIVES_WRITTEN = 0x8C88;
        public const UInt32 GL_RASTERIZER_DISCARD = 0x8C89;
        public const UInt32 GL_MAX_TRANSFORM_FEEDBACK_INTERLEAVED_COMPONENTS = 0x8C8A;
        public const UInt32 GL_MAX_TRANSFORM_FEEDBACK_SEPARATE_ATTRIBS = 0x8C8B;
        public const UInt32 GL_INTERLEAVED_ATTRIBS = 0x8C8C;
        public const UInt32 GL_SEPARATE_ATTRIBS = 0x8C8D;
        public const UInt32 GL_TRANSFORM_FEEDBACK_BUFFER = 0x8C8E;
        public const UInt32 GL_TRANSFORM_FEEDBACK_BUFFER_BINDING = 0x8C8F;
        public const UInt32 GL_RGBA32UI = 0x8D70;
        public const UInt32 GL_RGB32UI = 0x8D71;
        public const UInt32 GL_RGBA16UI = 0x8D76;
        public const UInt32 GL_RGB16UI = 0x8D77;
        public const UInt32 GL_RGBA8UI = 0x8D7C;
        public const UInt32 GL_RGB8UI = 0x8D7D;
        public const UInt32 GL_RGBA32I = 0x8D82;
        public const UInt32 GL_RGB32I = 0x8D83;
        public const UInt32 GL_RGBA16I = 0x8D88;
        public const UInt32 GL_RGB16I = 0x8D89;
        public const UInt32 GL_RGBA8I = 0x8D8E;
        public const UInt32 GL_RGB8I = 0x8D8F;
        public const UInt32 GL_RED_INTEGER = 0x8D94;
        public const UInt32 GL_GREEN_INTEGER = 0x8D95;
        public const UInt32 GL_BLUE_INTEGER = 0x8D96;
        public const UInt32 GL_ALPHA_INTEGER = 0x8D97;
        public const UInt32 GL_RGB_INTEGER = 0x8D98;
        public const UInt32 GL_RGBA_INTEGER = 0x8D99;
        public const UInt32 GL_BGR_INTEGER = 0x8D9A;
        public const UInt32 GL_BGRA_INTEGER = 0x8D9B;
        public const UInt32 GL_SAMPLER_1D_ARRAY = 0x8DC0;
        public const UInt32 GL_SAMPLER_2D_ARRAY = 0x8DC1;
        public const UInt32 GL_SAMPLER_1D_ARRAY_SHADOW = 0x8DC3;
        public const UInt32 GL_SAMPLER_2D_ARRAY_SHADOW = 0x8DC4;
        public const UInt32 GL_SAMPLER_CUBE_SHADOW = 0x8DC5;
        public const UInt32 GL_UNSIGNED_INT_VEC2 = 0x8DC6;
        public const UInt32 GL_UNSIGNED_INT_VEC3 = 0x8DC7;
        public const UInt32 GL_UNSIGNED_INT_VEC4 = 0x8DC8;
        public const UInt32 GL_INT_SAMPLER_1D = 0x8DC9;
        public const UInt32 GL_INT_SAMPLER_2D = 0x8DCA;
        public const UInt32 GL_INT_SAMPLER_3D = 0x8DCB;
        public const UInt32 GL_INT_SAMPLER_CUBE = 0x8DCC;
        public const UInt32 GL_INT_SAMPLER_1D_ARRAY = 0x8DCE;
        public const UInt32 GL_INT_SAMPLER_2D_ARRAY = 0x8DCF;
        public const UInt32 GL_UNSIGNED_INT_SAMPLER_1D = 0x8DD1;
        public const UInt32 GL_UNSIGNED_INT_SAMPLER_2D = 0x8DD2;
        public const UInt32 GL_UNSIGNED_INT_SAMPLER_3D = 0x8DD3;
        public const UInt32 GL_UNSIGNED_INT_SAMPLER_CUBE = 0x8DD4;
        public const UInt32 GL_UNSIGNED_INT_SAMPLER_1D_ARRAY = 0x8DD6;
        public const UInt32 GL_UNSIGNED_INT_SAMPLER_2D_ARRAY = 0x8DD7;
        public const UInt32 GL_QUERY_WAIT = 0x8E13;
        public const UInt32 GL_QUERY_NO_WAIT = 0x8E14;
        public const UInt32 GL_QUERY_BY_REGION_WAIT = 0x8E15;
        public const UInt32 GL_QUERY_BY_REGION_NO_WAIT = 0x8E16;
    }
}