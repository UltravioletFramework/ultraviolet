using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;

namespace Ultraviolet.OpenGL.Bindings
{
    public static unsafe partial class GL
    {
        [MonoNativeFunctionWrapper]
        private delegate void glBlendEquationSeparateDelegate(uint modeRGB, uint modeAlpha);
        [Require(MinVersion = "2.0")]
        private static glBlendEquationSeparateDelegate glBlendEquationSeparate = null;

        public static void BlendEquationSeparate(uint modeRGB, uint modeAlpha) { glBlendEquationSeparate(modeRGB, modeAlpha); }

        [MonoNativeFunctionWrapper]
        private delegate void glDrawBuffersDelegate(int n, IntPtr bufs);
        [Require(MinVersion = "2.0", MinVersionES = "3.0", Extension = "GL_ARB_draw_buffers", ExtensionFunction = "DrawBuffersARB")]
        private static glDrawBuffersDelegate glDrawBuffers = null;

        public static void DrawBuffers(int n, uint* bufs) { glDrawBuffers(n, (IntPtr)bufs); }

        [MonoNativeFunctionWrapper]
        private delegate void glStencilOpSeparateDelegate(uint face, uint sfail, uint dpfail, uint dppass);
        [Require(MinVersion = "2.0")]
        private static glStencilOpSeparateDelegate glStencilOpSeparate = null;

        public static void StencilOpSeparate(uint face, uint sfail, uint dpfail, uint dppass) { glStencilOpSeparate(face, sfail, dpfail, dppass); }

        [MonoNativeFunctionWrapper]
        private delegate void glStencilFuncSeparateDelegate(uint face, uint func, int @ref, uint mask);
        [Require(MinVersion = "2.0")]
        private static glStencilFuncSeparateDelegate glStencilFuncSeparate = null;

        public static void StencilFuncSeparate(uint face, uint func, int @ref, uint mask) { glStencilFuncSeparate(face, func, @ref, mask); }

        [MonoNativeFunctionWrapper]
        private delegate void glStencilMaskSeparateDelegate(uint face, uint mask);
        [Require(MinVersion = "2.0")]
        private static glStencilMaskSeparateDelegate glStencilMaskSeparate = null;

        public static void StencilMaskSeparate(uint face, uint mask) { glStencilMaskSeparate(face, mask); }

        [MonoNativeFunctionWrapper]
        private delegate void glAttachShaderDelegate(uint program, uint shader);
        [Require(MinVersion = "2.0")]
        private static glAttachShaderDelegate glAttachShader = null;

        public static void AttachShader(uint program, uint shader) { glAttachShader(program, shader); }

        [MonoNativeFunctionWrapper]
        private delegate void glBindAttribLocationDelegate(uint program, uint index, IntPtr name);
        [Require(MinVersion = "2.0")]
        private static glBindAttribLocationDelegate glBindAttribLocation = null;

        public static void BindAttribLocation(uint program, uint index, sbyte* name) { glBindAttribLocation(program, index, (IntPtr)name); }

        [MonoNativeFunctionWrapper]
        private delegate void glCompileShaderDelegate(uint shader);
        [Require(MinVersion = "2.0")]
        private static glCompileShaderDelegate glCompileShader = null;

        public static void CompileShader(uint shader) { glCompileShader(shader); }

        [MonoNativeFunctionWrapper]
        private delegate uint glCreateProgramDelegate();
        [Require(MinVersion = "2.0")]
        private static glCreateProgramDelegate glCreateProgram = null;

        public static uint CreateProgram() { return glCreateProgram(); }

        [MonoNativeFunctionWrapper]
        private delegate uint glCreateShaderDelegate(uint type);
        [Require(MinVersion = "2.0")]
        private static glCreateShaderDelegate glCreateShader = null;

        public static uint CreateShader(uint type) { return glCreateShader(type); }

        [MonoNativeFunctionWrapper]
        private delegate void glDeleteProgramDelegate(uint program);
        [Require(MinVersion = "2.0")]
        private static glDeleteProgramDelegate glDeleteProgram = null;

        public static void DeleteProgram(uint program) { glDeleteProgram(program); }

        [MonoNativeFunctionWrapper]
        private delegate void glDeleteShaderDelegate(uint shader);
        [Require(MinVersion = "2.0")]
        private static glDeleteShaderDelegate glDeleteShader = null;

        public static void DeleteShader(uint shader) { glDeleteShader(shader); }

        [MonoNativeFunctionWrapper]
        private delegate void glDetachShaderDelegate(uint program, uint shader);
        [Require(MinVersion = "2.0")]
        private static glDetachShaderDelegate glDetachShader = null;

        public static void DetachShader(uint program, uint shader) { glDetachShader(program, shader); }

        [MonoNativeFunctionWrapper]
        private delegate void glDisableVertexAttribArrayDelegate(uint index);
        [Require(MinVersion = "2.0")]
        private static glDisableVertexAttribArrayDelegate glDisableVertexAttribArray = null;

        public static void DisableVertexAttribArray(uint index) { glDisableVertexAttribArray(index); }

        [MonoNativeFunctionWrapper]
        private delegate void glEnableVertexAttribArrayDelegate(uint index);
        [Require(MinVersion = "2.0")]
        private static glEnableVertexAttribArrayDelegate glEnableVertexAttribArray = null;

        public static void EnableVertexAttribArray(uint index) { glEnableVertexAttribArray(index); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetActiveAttribDelegate(uint program, uint index, int maxLength, IntPtr length, IntPtr size, IntPtr type, IntPtr name);
        [Require(MinVersion = "2.0")]
        private static glGetActiveAttribDelegate glGetActiveAttrib = null;

        public static void GetActiveAttrib(uint program, uint index, int maxLength, int* length, int* size, uint* type, sbyte* name) =>
            glGetActiveAttrib(program, index, maxLength, (IntPtr)length, (IntPtr)size, (IntPtr)type, (IntPtr)name);

        [MonoNativeFunctionWrapper]
        private delegate void glGetActiveUniformDelegate(uint program, uint index, int maxLength, IntPtr length, IntPtr size, IntPtr type, IntPtr name);
        [Require(MinVersion = "2.0")]
        private static glGetActiveUniformDelegate glGetActiveUniform = null;

        public static void GetActiveUniform(uint program, uint index, int maxLength, int* length, int* size, uint* type, sbyte* name) =>
            glGetActiveUniform(program, index, maxLength, (IntPtr)length, (IntPtr)size, (IntPtr)type, (IntPtr)name);

        public static string GetActiveUniform(uint program, uint index, out uint type, out uint elements)
        {
            var nameBufferSize = 256;
            var nameBuffer = Marshal.AllocHGlobal(nameBufferSize);
            try
            {
                int length;
                int size;
                fixed (uint* pType = &type)
                {
                    glGetActiveUniform(program, index, nameBufferSize, (IntPtr)(&length), (IntPtr)(&size), (IntPtr)pType, nameBuffer);
                }
                elements = (UInt32)size;
                return Marshal.PtrToStringAnsi(nameBuffer);
            }
            finally
            {
                Marshal.FreeHGlobal(nameBuffer);
            }
        }

        [MonoNativeFunctionWrapper]
        private delegate void glGetAttachedShadersDelegate(uint program, int maxCount, IntPtr count, IntPtr shaders);
        [Require(MinVersion = "2.0")]
        private static glGetAttachedShadersDelegate glGetAttachedShaders = null;

        public static void GetAttachedShaders(uint program, int maxCount, int* count, uint* shaders) { glGetAttachedShaders(program, maxCount, (IntPtr)count, (IntPtr)shaders); }

        [MonoNativeFunctionWrapper]
        private delegate int glGetAttribLocationDelegate(uint program, IntPtr name);
        [Require(MinVersion = "2.0")]
        private static glGetAttribLocationDelegate glGetAttribLocation = null;

        public static int GetAttribLocation(uint program, string name) 
        {
            var nameptr = Marshal.StringToHGlobalAnsi(name);
            try
            {
                return glGetAttribLocation(program, nameptr);
            }
            finally
            {
                Marshal.FreeHGlobal(nameptr);
            }
        }

        [MonoNativeFunctionWrapper]
        private delegate void glGetProgramivDelegate(uint program, uint pname, IntPtr param);
        [Require(MinVersion = "2.0")]
        private static glGetProgramivDelegate glGetProgramiv = null;

        public static void GetProgramiv(uint program, uint pname, int* param) { glGetProgramiv(program, pname, (IntPtr)param); }

        public static int GetProgrami(uint program, uint pname)
        {
            int value;
            glGetProgramiv(program, pname, (IntPtr)(&value));
            return value;
        }

        [MonoNativeFunctionWrapper]
        private delegate void glGetProgramInfoLogDelegate(uint program, int bufSize, IntPtr length, IntPtr infoLog);
        [Require(MinVersion = "2.0")]
        private static glGetProgramInfoLogDelegate glGetProgramInfoLog = null;

        public static void GetProgramInfoLog(uint program, int bufSize, int* length, sbyte* infoLog) { glGetProgramInfoLog(program, bufSize, (IntPtr)length, (IntPtr)infoLog); }

        public static String GetProgramInfoLog(uint program)
        {
            var infoLog = default(String);
            var infoLogLength = GetProgrami(program, GL_INFO_LOG_LENGTH);
            var infoLogPtr = Marshal.AllocHGlobal(infoLogLength);
            try
            {
                if (infoLogLength > 0)
                {
                    glGetProgramInfoLog(program, infoLogLength, IntPtr.Zero, infoLogPtr);
                    infoLog = Marshal.PtrToStringAnsi(infoLogPtr);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(infoLogPtr);
            }
            return infoLog;
        }

        [MonoNativeFunctionWrapper]
        private delegate void glGetShaderivDelegate(uint shader, uint pname, IntPtr param);
        [Require(MinVersion = "2.0")]
        private static glGetShaderivDelegate glGetShaderiv = null;

        public static void GetShaderiv(uint shader, uint pname, int* param) { glGetShaderiv(shader, pname, (IntPtr)param); }

        public static int GetShaderi(uint shader, uint pname)
        {
            int value;
            glGetShaderiv(shader, pname, (IntPtr)(&value));
            return value;
        }

        [MonoNativeFunctionWrapper]
        private delegate void glGetShaderInfoLogDelegate(uint shader, int bufSize, IntPtr length, IntPtr infoLog);
        [Require(MinVersion = "2.0")]
        private static glGetShaderInfoLogDelegate glGetShaderInfoLog = null;

        public static void GetShaderInfoLog(uint shader, int bufSize, int* length, sbyte* infoLog) { glGetShaderInfoLog(shader, bufSize, (IntPtr)length, (IntPtr)infoLog); }

        public static void GetShaderInfoLog(uint shader, out String infoLog)
        {
            var infoLogLength = GetShaderi(shader, GL_INFO_LOG_LENGTH);
            if (infoLogLength == 0)
            {
                infoLog = String.Empty;
            }
            else
            {
                var infoLogPtr = Marshal.AllocHGlobal(infoLogLength);
                try
                {
                    glGetShaderInfoLog(shader, infoLogLength, IntPtr.Zero, infoLogPtr);
                    infoLog = Marshal.PtrToStringAnsi(infoLogPtr);
                }
                finally
                {
                    Marshal.FreeHGlobal(infoLogPtr);
                }
            }
        }

        [MonoNativeFunctionWrapper]
        private delegate void glShaderSourceDelegate(uint shader, int count, IntPtr @string, IntPtr length);
        [Require(MinVersion = "2.0")]
        private static glShaderSourceDelegate glShaderSource = null;

        public static void ShaderSource(uint shader, int count, sbyte** @string, int* length) { glShaderSource(shader, count, (IntPtr)@string, (IntPtr)length); }

        [MonoNativeFunctionWrapper]
        private delegate int glGetUniformLocationDelegate(uint program, IntPtr name);
        [Require(MinVersion = "2.0")]
        private static glGetUniformLocationDelegate glGetUniformLocation = null;

        public static int GetUniformLocation(uint program, sbyte* name) { return glGetUniformLocation(program, (IntPtr)name); }

        public static int GetUniformLocation(uint program, String name)
        {
            var nameptr = Marshal.StringToHGlobalAnsi(name);
            try
            {
                return glGetUniformLocation(program, nameptr);
            }
            finally
            {
                Marshal.FreeHGlobal(nameptr);
            }
        }

        [MonoNativeFunctionWrapper]
        private delegate void glGetUniformfvDelegate(uint program, int location, IntPtr @params);
        [Require(MinVersion = "2.0")]
        private static glGetUniformfvDelegate glGetUniformfv = null;

        public static void GetUniformfv(uint program, int location, float* @params) { glGetUniformfv(program, location, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetUniformivDelegate(uint program, int location, IntPtr @params);
        [Require(MinVersion = "2.0")]
        private static glGetUniformivDelegate glGetUniformiv = null;

        public static void GetUniformiv(uint program, int location, int* @params) { glGetUniformiv(program, location, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetVertexAttribdvDelegate(uint index, uint pname, IntPtr @params);
        [Require(MinVersion = "2.0")]
        private static glGetVertexAttribdvDelegate glGetVertexAttribdv = null;

        public static void GetVertexAttribdv(uint index, uint pname, double* @params) { glGetVertexAttribdv(index, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetVertexAttribfvDelegate(uint index, uint pname, IntPtr @params);
        [Require(MinVersion = "2.0")]
        private static glGetVertexAttribfvDelegate glGetVertexAttribfv = null;

        public static void GetVertexAttribfv(uint index, uint pname, float* @params) { glGetVertexAttribfv(index, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetVertexAttribivDelegate(uint index, uint pname, IntPtr @params);
        [Require(MinVersion = "2.0")]
        private static glGetVertexAttribivDelegate glGetVertexAttribiv = null;

        public static void GetVertexAttribiv(uint index, uint pname, int* @params) { glGetVertexAttribiv(index, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetVertexAttribPointervDelegate(uint index, uint pname, IntPtr pointer);
        [Require(MinVersion = "2.0")]
        private static glGetVertexAttribPointervDelegate glGetVertexAttribPointerv = null;

        public static void GetVertexAttribPointerv(uint index, uint pname, void** pointer) { glGetVertexAttribPointerv(index, pname, (IntPtr)pointer); }

        [MonoNativeFunctionWrapper]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool glIsProgramDelegate(uint program);
        [Require(MinVersion = "2.0")]
        private static glIsProgramDelegate glIsProgram = null;

        public static bool IsProgram(uint program) { return glIsProgram(program); }

        [MonoNativeFunctionWrapper]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool glIsShaderDelegate(uint shader);
        [Require(MinVersion = "2.0")]
        private static glIsShaderDelegate glIsShader = null;

        public static bool IsShader(uint shader) { return glIsShader(shader); }

        [MonoNativeFunctionWrapper]
        private delegate void glLinkProgramDelegate(uint program);
        [Require(MinVersion = "2.0")]
        private static glLinkProgramDelegate glLinkProgram = null;

        public static void LinkProgram(uint program) { glLinkProgram(program); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetShaderSourceDelegate(uint obj, int maxLength, IntPtr length, IntPtr source);
        [Require(MinVersion = "2.0")]
        private static glGetShaderSourceDelegate glGetShaderSource = null;

        public static void GetShaderSource(uint obj, int maxLength, int* length, sbyte* source) { glGetShaderSource(obj, maxLength, (IntPtr)length, (IntPtr)source); }

        [MonoNativeFunctionWrapper]
        private delegate void glUseProgramDelegate(uint program);
        [Require(MinVersion = "2.0")]
        private static glUseProgramDelegate glUseProgram = null;

        public static void UseProgram(uint program) { glUseProgram(program); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniform1fDelegate(int location, float v0);
        [Require(MinVersion = "2.0")]
        private static glUniform1fDelegate glUniform1f = null;

        public static void Uniform1f(int location, float v0) { glUniform1f(location, v0); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniform1fvDelegate(int location, int count, IntPtr value);
        [Require(MinVersion = "2.0")]
        private static glUniform1fvDelegate glUniform1fv = null;

        public static void Uniform1fv(int location, int count, float* value) { glUniform1fv(location, count, (IntPtr)value); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniform1iDelegate(int location, int v0);
        [Require(MinVersion = "2.0")]
        private static glUniform1iDelegate glUniform1i = null;

        public static void Uniform1i(int location, int v0) { glUniform1i(location, v0); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniform1ivDelegate(int location, int count, IntPtr value);
        [Require(MinVersion = "2.0")]
        private static glUniform1ivDelegate glUniform1iv = null;

        public static void Uniform1iv(int location, int count, int* value) { glUniform1iv(location, count, (IntPtr)value); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniform2fDelegate(int location, float v0, float v1);
        [Require(MinVersion = "2.0")]
        private static glUniform2fDelegate glUniform2f = null;

        public static void Uniform2f(int location, float v0, float v1) { glUniform2f(location, v0, v1); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniform2fvDelegate(int location, int count, IntPtr value);
        [Require(MinVersion = "2.0")]
        private static glUniform2fvDelegate glUniform2fv = null;

        public static void Uniform2fv(int location, int count, float* value) { glUniform2fv(location, count, (IntPtr)value); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniform2iDelegate(int location, int v0, int v1);
        [Require(MinVersion = "2.0")]
        private static glUniform2iDelegate glUniform2i = null;

        public static void Uniform2i(int location, int v0, int v1) { glUniform2i(location, v0, v1); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniform2ivDelegate(int location, int count, IntPtr value);
        [Require(MinVersion = "2.0")]
        private static glUniform2ivDelegate glUniform2iv = null;

        public static void Uniform2iv(int location, int count, int* value) { glUniform2iv(location, count, (IntPtr)value); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniform3fDelegate(int location, float v0, float v1, float v2);
        [Require(MinVersion = "2.0")]
        private static glUniform3fDelegate glUniform3f = null;

        public static void Uniform3f(int location, float v0, float v1, float v2) { glUniform3f(location, v0, v1, v2); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniform3fvDelegate(int location, int count, IntPtr value);
        [Require(MinVersion = "2.0")]
        private static glUniform3fvDelegate glUniform3fv = null;

        public static void Uniform3fv(int location, int count, float* value) { glUniform3fv(location, count, (IntPtr)value); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniform3iDelegate(int location, int v0, int v1, int v2);
        [Require(MinVersion = "2.0")]
        private static glUniform3iDelegate glUniform3i = null;

        public static void Uniform3i(int location, int v0, int v1, int v2) { glUniform3i(location, v0, v1, v2); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniform3ivDelegate(int location, int count, IntPtr value);
        [Require(MinVersion = "2.0")]
        private static glUniform3ivDelegate glUniform3iv = null;

        public static void Uniform3iv(int location, int count, int* value) { glUniform3iv(location, count, (IntPtr)value); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniform4fDelegate(int location, float v0, float v1, float v2, float v3);
        [Require(MinVersion = "2.0")]
        private static glUniform4fDelegate glUniform4f = null;

        public static void Uniform4f(int location, float v0, float v1, float v2, float v3) { glUniform4f(location, v0, v1, v2, v3); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniform4fvDelegate(int location, int count, IntPtr value);
        [Require(MinVersion = "2.0")]
        private static glUniform4fvDelegate glUniform4fv = null;

        public static void Uniform4fv(int location, int count, float* value) { glUniform4fv(location, count, (IntPtr)value); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniform4iDelegate(int location, int v0, int v1, int v2, int v3);
        [Require(MinVersion = "2.0")]
        private static glUniform4iDelegate glUniform4i = null;

        public static void Uniform4i(int location, int v0, int v1, int v2, int v3) { glUniform4i(location, v0, v1, v2, v3); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniform4ivDelegate(int location, int count, IntPtr value);
        [Require(MinVersion = "2.0")]
        private static glUniform4ivDelegate glUniform4iv = null;

        public static void Uniform4iv(int location, int count, int* value) { glUniform4iv(location, count, (IntPtr)value); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniformMatrix2fvDelegate(int location, int count, [MarshalAs(UnmanagedType.I1)] bool transpose, IntPtr value);
        [Require(MinVersion = "2.0")]
        private static glUniformMatrix2fvDelegate glUniformMatrix2fv = null;

        public static void UniformMatrix2fv(int location, int count, bool transpose, float* value) { glUniformMatrix2fv(location, count, transpose, (IntPtr)value); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniformMatrix3fvDelegate(int location, int count, [MarshalAs(UnmanagedType.I1)] bool transpose, IntPtr value);
        [Require(MinVersion = "2.0")]
        private static glUniformMatrix3fvDelegate glUniformMatrix3fv = null;

        public static void UniformMatrix3fv(int location, int count, bool transpose, float* value) { glUniformMatrix3fv(location, count, transpose, (IntPtr)value); }

        [MonoNativeFunctionWrapper]
        private delegate void glUniformMatrix4fvDelegate(int location, int count, [MarshalAs(UnmanagedType.I1)] bool transpose, IntPtr value);
        [Require(MinVersion = "2.0")]
        private static glUniformMatrix4fvDelegate glUniformMatrix4fv = null;

        public static void UniformMatrix4fv(int location, int count, bool transpose, float* value) { glUniformMatrix4fv(location, count, transpose, (IntPtr)value); }

        [MonoNativeFunctionWrapper]
        private delegate void glValidateProgramDelegate(uint program);
        [Require(MinVersion = "2.0")]
        private static glValidateProgramDelegate glValidateProgram = null;

        public static void ValidateProgram(uint program) { glValidateProgram(program); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib1dDelegate(uint index, double x);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib1dDelegate glVertexAttrib1d = null;

        public static void VertexAttrib1d(uint index, double x) { glVertexAttrib1d(index, x); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib1dvDelegate(uint index, IntPtr v);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib1dvDelegate glVertexAttrib1dv = null;

        public static void VertexAttrib1dv(uint index, double* v) { glVertexAttrib1dv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib1fDelegate(uint index, float x);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib1fDelegate glVertexAttrib1f = null;

        public static void VertexAttrib1f(uint index, float x) { glVertexAttrib1f(index, x); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib1fvDelegate(uint index, IntPtr v);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib1fvDelegate glVertexAttrib1fv = null;

        public static void VertexAttrib1fv(uint index, float* v) { glVertexAttrib1fv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib1sDelegate(uint index, short x);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib1sDelegate glVertexAttrib1s = null;

        public static void VertexAttrib1s(uint index, short x) { glVertexAttrib1s(index, x); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib1svDelegate(uint index, IntPtr v);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib1svDelegate glVertexAttrib1sv = null;

        public static void VertexAttrib1sv(uint index, short* v) { glVertexAttrib1sv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib2dDelegate(uint index, double x, double y);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib2dDelegate glVertexAttrib2d = null;

        public static void VertexAttrib2d(uint index, double x, double y) { glVertexAttrib2d(index, x, y); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib2dvDelegate(uint index, IntPtr v);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib2dvDelegate glVertexAttrib2dv = null;

        public static void VertexAttrib2dv(uint index, double* v) { glVertexAttrib2dv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib2fDelegate(uint index, float x, float y);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib2fDelegate glVertexAttrib2f = null;

        public static void VertexAttrib2f(uint index, float x, float y) { glVertexAttrib2f(index, x, y); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib2fvDelegate(uint index, IntPtr v);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib2fvDelegate glVertexAttrib2fv = null;

        public static void VertexAttrib2fv(uint index, float* v) { glVertexAttrib2fv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib2sDelegate(uint index, short x, short y);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib2sDelegate glVertexAttrib2s = null;

        public static void VertexAttrib2s(uint index, short x, short y) { glVertexAttrib2s(index, x, y); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib2svDelegate(uint index, IntPtr v);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib2svDelegate glVertexAttrib2sv = null;

        public static void VertexAttrib2sv(uint index, short* v) { glVertexAttrib2sv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib3dDelegate(uint index, double x, double y, double z);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib3dDelegate glVertexAttrib3d = null;

        public static void VertexAttrib3d(uint index, double x, double y, double z) { glVertexAttrib3d(index, x, y, z); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib3dvDelegate(uint index, IntPtr v);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib3dvDelegate glVertexAttrib3dv = null;

        public static void VertexAttrib3dv(uint index, double* v) { glVertexAttrib3dv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib3fDelegate(uint index, float x, float y, float z);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib3fDelegate glVertexAttrib3f = null;

        public static void VertexAttrib3f(uint index, float x, float y, float z) { glVertexAttrib3f(index, x, y, z); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib3fvDelegate(uint index, IntPtr v);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib3fvDelegate glVertexAttrib3fv = null;

        public static void VertexAttrib3fv(uint index, float* v) { glVertexAttrib3fv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib3sDelegate(uint index, short x, short y, short z);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib3sDelegate glVertexAttrib3s = null;

        public static void VertexAttrib3s(uint index, short x, short y, short z) { glVertexAttrib3s(index, x, y, z); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib3svDelegate(uint index, IntPtr v);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib3svDelegate glVertexAttrib3sv = null;

        public static void VertexAttrib3sv(uint index, short* v) { glVertexAttrib3sv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib4NbvDelegate(uint index, IntPtr v);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib4NbvDelegate glVertexAttrib4Nbv = null;

        public static void VertexAttrib4Nbv(uint index, sbyte* v) { glVertexAttrib4Nbv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib4NivDelegate(uint index, IntPtr v);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib4NivDelegate glVertexAttrib4Niv = null;

        public static void VertexAttrib4Niv(uint index, int* v) { glVertexAttrib4Niv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib4NsvDelegate(uint index, IntPtr v);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib4NsvDelegate glVertexAttrib4Nsv = null;

        public static void VertexAttrib4Nsv(uint index, short* v) { glVertexAttrib4Nsv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib4NubDelegate(uint index, byte x, byte y, byte z, byte w);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib4NubDelegate glVertexAttrib4Nub = null;

        public static void VertexAttrib4Nub(uint index, byte x, byte y, byte z, byte w) { glVertexAttrib4Nub(index, x, y, z, w); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib4NubvDelegate(uint index, IntPtr v);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib4NubvDelegate glVertexAttrib4Nubv = null;

        public static void VertexAttrib4Nubv(uint index, byte* v) { glVertexAttrib4Nubv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib4NuivDelegate(uint index, IntPtr v);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib4NuivDelegate glVertexAttrib4Nuiv = null;

        public static void VertexAttrib4Nuiv(uint index, uint* v) { glVertexAttrib4Nuiv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib4NusvDelegate(uint index, IntPtr v);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib4NusvDelegate glVertexAttrib4Nusv = null;

        public static void VertexAttrib4Nusv(uint index, ushort* v) { glVertexAttrib4Nusv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib4bvDelegate(uint index, IntPtr v);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib4bvDelegate glVertexAttrib4bv = null;

        public static void VertexAttrib4bv(uint index, sbyte* v) { glVertexAttrib4bv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib4dDelegate(uint index, double x, double y, double z, double w);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib4dDelegate glVertexAttrib4d = null;

        public static void VertexAttrib4d(uint index, double x, double y, double z, double w) { glVertexAttrib4d(index, x, y, z, w); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib4dvDelegate(uint index, IntPtr v);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib4dvDelegate glVertexAttrib4dv = null;

        public static void VertexAttrib4dv(uint index, double* v) { glVertexAttrib4dv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib4fDelegate(uint index, float x, float y, float z, float w);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib4fDelegate glVertexAttrib4f = null;

        public static void VertexAttrib4f(uint index, float x, float y, float z, float w) { glVertexAttrib4f(index, x, y, z, w); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib4fvDelegate(uint index, IntPtr v);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib4fvDelegate glVertexAttrib4fv = null;

        public static void VertexAttrib4fv(uint index, float* v) { glVertexAttrib4fv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib4ivDelegate(uint index, IntPtr v);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib4ivDelegate glVertexAttrib4iv = null;

        public static void VertexAttrib4iv(uint index, int* v) { glVertexAttrib4iv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib4sDelegate(uint index, short x, short y, short z, short w);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib4sDelegate glVertexAttrib4s = null;

        public static void VertexAttrib4s(uint index, short x, short y, short z, short w) { glVertexAttrib4s(index, x, y, z, w); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib4svDelegate(uint index, IntPtr v);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib4svDelegate glVertexAttrib4sv = null;

        public static void VertexAttrib4sv(uint index, short* v) { glVertexAttrib4sv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib4ubvDelegate(uint index, IntPtr v);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib4ubvDelegate glVertexAttrib4ubv = null;

        public static void VertexAttrib4ubv(uint index, byte* v) { glVertexAttrib4ubv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib4uivDelegate(uint index, IntPtr v);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib4uivDelegate glVertexAttrib4uiv = null;

        public static void VertexAttrib4uiv(uint index, uint* v) { glVertexAttrib4uiv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttrib4usvDelegate(uint index, IntPtr v);
        [Require(MinVersion = "2.0")]
        private static glVertexAttrib4usvDelegate glVertexAttrib4usv = null;

        public static void VertexAttrib4usv(uint index, ushort* v) { glVertexAttrib4usv(index, (IntPtr)v); }

        [MonoNativeFunctionWrapper]
        private delegate void glVertexAttribPointerDelegate(uint index, int size, uint type, [MarshalAs(UnmanagedType.I1)] bool normalized, int stride, IntPtr pointer);
        [Require(MinVersion = "2.0")]
        private static glVertexAttribPointerDelegate glVertexAttribPointer = null;

        public static void VertexAttribPointer(uint index, int size, uint type, bool normalized, int stride, void* pointer) { glVertexAttribPointer(index, size, type, normalized, stride, (IntPtr)pointer); }

        public const UInt32 GL_BLEND_EQUATION_RGB = GL_BLEND_EQUATION;
        public const UInt32 GL_VERTEX_ATTRIB_ARRAY_ENABLED = 0x8622;
        public const UInt32 GL_VERTEX_ATTRIB_ARRAY_SIZE = 0x8623;
        public const UInt32 GL_VERTEX_ATTRIB_ARRAY_STRIDE = 0x8624;
        public const UInt32 GL_VERTEX_ATTRIB_ARRAY_TYPE = 0x8625;
        public const UInt32 GL_CURRENT_VERTEX_ATTRIB = 0x8626;
        public const UInt32 GL_VERTEX_PROGRAM_POINT_SIZE = 0x8642;
        public const UInt32 GL_VERTEX_PROGRAM_TWO_SIDE = 0x8643;
        public const UInt32 GL_VERTEX_ATTRIB_ARRAY_POINTER = 0x8645;
        public const UInt32 GL_STENCIL_BACK_FUNC = 0x8800;
        public const UInt32 GL_STENCIL_BACK_FAIL = 0x8801;
        public const UInt32 GL_STENCIL_BACK_PASS_DEPTH_FAIL = 0x8802;
        public const UInt32 GL_STENCIL_BACK_PASS_DEPTH_PASS = 0x8803;
        public const UInt32 GL_MAX_DRAW_BUFFERS = 0x8824;
        public const UInt32 GL_DRAW_BUFFER0 = 0x8825;
        public const UInt32 GL_DRAW_BUFFER1 = 0x8826;
        public const UInt32 GL_DRAW_BUFFER2 = 0x8827;
        public const UInt32 GL_DRAW_BUFFER3 = 0x8828;
        public const UInt32 GL_DRAW_BUFFER4 = 0x8829;
        public const UInt32 GL_DRAW_BUFFER5 = 0x882A;
        public const UInt32 GL_DRAW_BUFFER6 = 0x882B;
        public const UInt32 GL_DRAW_BUFFER7 = 0x882C;
        public const UInt32 GL_DRAW_BUFFER8 = 0x882D;
        public const UInt32 GL_DRAW_BUFFER9 = 0x882E;
        public const UInt32 GL_DRAW_BUFFER10 = 0x882F;
        public const UInt32 GL_DRAW_BUFFER11 = 0x8830;
        public const UInt32 GL_DRAW_BUFFER12 = 0x8831;
        public const UInt32 GL_DRAW_BUFFER13 = 0x8832;
        public const UInt32 GL_DRAW_BUFFER14 = 0x8833;
        public const UInt32 GL_DRAW_BUFFER15 = 0x8834;
        public const UInt32 GL_BLEND_EQUATION_ALPHA = 0x883D;
        public const UInt32 GL_POINT_SPRITE = 0x8861;
        public const UInt32 GL_COORD_REPLACE = 0x8862;
        public const UInt32 GL_MAX_VERTEX_ATTRIBS = 0x8869;
        public const UInt32 GL_VERTEX_ATTRIB_ARRAY_NORMALIZED = 0x886A;
        public const UInt32 GL_MAX_TEXTURE_COORDS = 0x8871;
        public const UInt32 GL_MAX_TEXTURE_IMAGE_UNITS = 0x8872;
        public const UInt32 GL_FRAGMENT_SHADER = 0x8B30;
        public const UInt32 GL_VERTEX_SHADER = 0x8B31;
        public const UInt32 GL_MAX_FRAGMENT_UNIFORM_COMPONENTS = 0x8B49;
        public const UInt32 GL_MAX_VERTEX_UNIFORM_COMPONENTS = 0x8B4A;
        public const UInt32 GL_MAX_VARYING_FLOATS = 0x8B4B;
        public const UInt32 GL_MAX_VERTEX_TEXTURE_IMAGE_UNITS = 0x8B4C;
        public const UInt32 GL_MAX_COMBINED_TEXTURE_IMAGE_UNITS = 0x8B4D;
        public const UInt32 GL_SHADER_TYPE = 0x8B4F;
        public const UInt32 GL_FLOAT_VEC2 = 0x8B50;
        public const UInt32 GL_FLOAT_VEC3 = 0x8B51;
        public const UInt32 GL_FLOAT_VEC4 = 0x8B52;
        public const UInt32 GL_INT_VEC2 = 0x8B53;
        public const UInt32 GL_INT_VEC3 = 0x8B54;
        public const UInt32 GL_INT_VEC4 = 0x8B55;
        public const UInt32 GL_BOOL = 0x8B56;
        public const UInt32 GL_BOOL_VEC2 = 0x8B57;
        public const UInt32 GL_BOOL_VEC3 = 0x8B58;
        public const UInt32 GL_BOOL_VEC4 = 0x8B59;
        public const UInt32 GL_FLOAT_MAT2 = 0x8B5A;
        public const UInt32 GL_FLOAT_MAT3 = 0x8B5B;
        public const UInt32 GL_FLOAT_MAT4 = 0x8B5C;
        public const UInt32 GL_SAMPLER_1D = 0x8B5D;
        public const UInt32 GL_SAMPLER_2D = 0x8B5E;
        public const UInt32 GL_SAMPLER_3D = 0x8B5F;
        public const UInt32 GL_SAMPLER_CUBE = 0x8B60;
        public const UInt32 GL_SAMPLER_1D_SHADOW = 0x8B61;
        public const UInt32 GL_SAMPLER_2D_SHADOW = 0x8B62;
        public const UInt32 GL_DELETE_STATUS = 0x8B80;
        public const UInt32 GL_COMPILE_STATUS = 0x8B81;
        public const UInt32 GL_LINK_STATUS = 0x8B82;
        public const UInt32 GL_VALIDATE_STATUS = 0x8B83;
        public const UInt32 GL_INFO_LOG_LENGTH = 0x8B84;
        public const UInt32 GL_ATTACHED_SHADERS = 0x8B85;
        public const UInt32 GL_ACTIVE_UNIFORMS = 0x8B86;
        public const UInt32 GL_ACTIVE_UNIFORM_MAX_LENGTH = 0x8B87;
        public const UInt32 GL_SHADER_SOURCE_LENGTH = 0x8B88;
        public const UInt32 GL_ACTIVE_ATTRIBUTES = 0x8B89;
        public const UInt32 GL_ACTIVE_ATTRIBUTE_MAX_LENGTH = 0x8B8A;
        public const UInt32 GL_FRAGMENT_SHADER_DERIVATIVE_HINT = 0x8B8B;
        public const UInt32 GL_SHADING_LANGUAGE_VERSION = 0x8B8C;
        public const UInt32 GL_CURRENT_PROGRAM = 0x8B8D;
        public const UInt32 GL_POINT_SPRITE_COORD_ORIGIN = 0x8CA0;
        public const UInt32 GL_LOWER_LEFT = 0x8CA1;
        public const UInt32 GL_UPPER_LEFT = 0x8CA2;
        public const UInt32 GL_STENCIL_BACK_REF = 0x8CA3;
        public const UInt32 GL_STENCIL_BACK_VALUE_MASK = 0x8CA4;
        public const UInt32 GL_STENCIL_BACK_WRITEMASK = 0x8CA5;
    }
}
