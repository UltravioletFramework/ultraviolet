using System;
using Ultraviolet.Core;

namespace Ultraviolet.OpenGL.Bindings
{
    public static unsafe partial class GL
    {
        public const UInt32 GL_SAMPLER_BINDING = 0x8919;

        [MonoNativeFunctionWrapper]
        private delegate void glGenSamplersDelegate(int count, IntPtr samplers);
        [Require(MinVersion = "3.3", MinVersionES = "3.0", Extension = "GL_ARB_sampler_objects")]
        private static glGenSamplersDelegate glGenSamplers = null;

        public static void GenSamplers(int n, uint* samplers) { glGenSamplers(n, (IntPtr)samplers); }

        public static void GenSamplers(uint[] samplers)
        {
            fixed (uint* psamplers = samplers)
                glGenSamplers(samplers.Length, (IntPtr)psamplers);
        }

        public static uint GenSampler()
        {
            uint value;
            glGenSamplers(1, (IntPtr)(&value));
            return value;
        }
        
        [MonoNativeFunctionWrapper]
        private delegate void glDeleteSamplersDelegate(int count, IntPtr samplers);
        [Require(MinVersion = "3.3", MinVersionES = "3.0", Extension = "GL_ARB_sampler_objects")]
        private static glDeleteSamplersDelegate glDeleteSamplers = null;

        public static void DeleteSamplers(int n, uint* samplers) { glDeleteSamplers(n, (IntPtr)samplers); }

        public static void DeleteSamplers(uint[] samplers)
        {
            fixed (uint* psamplers = samplers)
                glDeleteSamplers(samplers.Length, (IntPtr)psamplers);
        }

        public static void DeleteSampler(uint sampler)
        {
            glDeleteSamplers(1, (IntPtr)(&sampler));
        }

        [MonoNativeFunctionWrapper]
        private delegate void glBindSamplerDelegate(uint unit, uint sampler);
        [Require(MinVersion = "3.3", MinVersionES = "3.0", Extension = "GL_ARB_sampler_objects")]
        private static glBindSamplerDelegate glBindSampler = null;

        public static void BindSampler(uint unit, uint sampler) { glBindSampler(unit, sampler); }
        
        [MonoNativeFunctionWrapper]
        private delegate void glSamplerParameteriDelegate(uint sampler, uint pname, int param);
        [Require(MinVersion = "3.3", MinVersionES = "3.0", Extension = "GL_ARB_sampler_objects")]
        private static glSamplerParameteriDelegate glSamplerParameteri = null;

        public static void SamplerParameteri(uint sampler, uint pname, int param) { glSamplerParameteri(sampler, pname, param); }

        [MonoNativeFunctionWrapper]
        private delegate void glSamplerParameterfDelegate(uint sampler, uint pname, float param);
        [Require(MinVersion = "3.3", MinVersionES = "3.0", Extension = "GL_ARB_sampler_objects")]
        private static glSamplerParameterfDelegate glSamplerParameterf = null;

        public static void SamplerParameterf(uint sampler, uint pname, float param) { glSamplerParameterf(sampler, pname, param); }

        [MonoNativeFunctionWrapper]
        private delegate void glSamplerParameterivDelegate(uint sampler, uint pname, IntPtr @params);
        [Require(MinVersion = "3.3", MinVersionES = "3.0", Extension = "GL_ARB_sampler_objects")]
        private static glSamplerParameterivDelegate glSamplerParameteriv = null;

        public static void SamplerParameteriv(uint sampler, uint pname, int* @params) { glSamplerParameteriv(sampler, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glSamplerParameterfvDelegate(uint sampler, uint pname, IntPtr @params);
        [Require(MinVersion = "3.3", MinVersionES = "3.0", Extension = "GL_ARB_sampler_objects")]
        private static glSamplerParameterfvDelegate glSamplerParameterfv = null;

        public static void SamplerParameterfv(uint sampler, uint pname, float* @params) { glSamplerParameterfv(sampler, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glSamplerParameterIivDelegate(uint sampler, uint pname, IntPtr @params);
        [Require(MinVersion = "3.3", MinVersionES = "3.0", Extension = "GL_ARB_sampler_objects")]
        private static glSamplerParameterIivDelegate glSamplerParameterIiv = null;

        public static void SamplerParameterIiv(uint sampler, uint pname, int* @params) { glSamplerParameterIiv(sampler, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glSamplerParameterIuivDelegate(uint sampler, uint pname, IntPtr @params);
        [Require(MinVersion = "3.3", MinVersionES = "3.0", Extension = "GL_ARB_sampler_objects")]
        private static glSamplerParameterIuivDelegate glSamplerParameterIuiv = null;

        public static void SamplerParameterIuiv(uint sampler, uint pname, uint* @params) { glSamplerParameterIuiv(sampler, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetSamplerParameterivDelegate(uint sampler, uint pname, IntPtr @params);
        [Require(MinVersion = "3.3", MinVersionES = "3.0", Extension = "GL_ARB_sampler_objects")]
        private static glGetSamplerParameterivDelegate glGetSamplerParameteriv = null;

        public static void GetSamplerParameteriv(uint sampler, uint pname, int* @params) { glGetSamplerParameteriv(sampler, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetSamplerParameterfvDelegate(uint sampler, uint pname, IntPtr @params);
        [Require(MinVersion = "3.3", MinVersionES = "3.0", Extension = "GL_ARB_sampler_objects")]
        private static glGetSamplerParameterfvDelegate glGetSamplerParameterfv = null;

        public static void GetSamplerParameterfv(uint sampler, uint pname, float* @params) { glGetSamplerParameterfv(sampler, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetSamplerParameterIivDelegate(uint sampler, uint pname, IntPtr @params);
        [Require(MinVersion = "3.3", MinVersionES = "3.0", Extension = "GL_ARB_sampler_objects")]
        private static glGetSamplerParameterIivDelegate glGetSamplerParameterIiv = null;

        public static void GetSamplerParameterIiv(uint sampler, uint pname, int* @params) { glGetSamplerParameterIiv(sampler, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetSamplerParameterIuivDelegate(uint sampler, uint pname, IntPtr @params);
        [Require(MinVersion = "3.3", MinVersionES = "3.0", Extension = "GL_ARB_sampler_objects")]
        private static glGetSamplerParameterIuivDelegate glGetSamplerParameterIuiv = null;

        public static void GetSamplerParameterIuiv(uint sampler, uint pname, uint* @params) { glGetSamplerParameterIuiv(sampler, pname, (IntPtr)@params); }
    }
}
