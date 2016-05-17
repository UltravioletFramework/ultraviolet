using System;

namespace TwistedLogik.Gluon
{
    public static unsafe partial class gl
    {
        public const UInt32 GL_SAMPLER_BINDING = 0x8919;

        private delegate void glGenSamplersDelegate(int count, uint* samplers);
        [Require(MinVersion = "3.3", MinVersionES = "3.0", Extension = "GL_ARB_sampler_objects")]
        private static readonly glGenSamplersDelegate glGenSamplers = null;

        public static void GenSamplers(int n, uint* samplers) { glGenSamplers(n, samplers); }

        public static void GenSamplers(uint[] samplers)
        {
            fixed (uint* psamplers = samplers)
                glGenSamplers(samplers.Length, psamplers);
        }

        public static uint GenSampler()
        {
            uint value;
            glGenSamplers(1, &value);
            return value;
        }
        
        private delegate void glDeleteSamplersDelegate(int count, uint* samplers);
        [Require(MinVersion = "3.3", MinVersionES = "3.0", Extension = "GL_ARB_sampler_objects")]
        private static readonly glDeleteSamplersDelegate glDeleteSamplers = null;

        public static void DeleteSamplers(int n, uint* samplers) { glDeleteSamplers(n, samplers); }

        public static void DeleteSamplers(uint[] samplers)
        {
            fixed (uint* psamplers = samplers)
                glDeleteSamplers(samplers.Length, psamplers);
        }

        public static void DeleteSampler(uint sampler)
        {
            glDeleteSamplers(1, &sampler);
        }

        private delegate void glBindSamplerDelegate(uint unit, uint sampler);
        [Require(MinVersion = "3.3", MinVersionES = "3.0", Extension = "GL_ARB_sampler_objects")]
        private static readonly glBindSamplerDelegate glBindSampler = null;

        public static void BindSampler(uint unit, uint sampler) { glBindSampler(unit, sampler); }
        
        private delegate void glSamplerParameteriDelegate(uint sampler, uint pname, int param);
        [Require(MinVersion = "3.3", MinVersionES = "3.0", Extension = "GL_ARB_sampler_objects")]
        private static readonly glSamplerParameteriDelegate glSamplerParameteri = null;

        public static void SamplerParameteri(uint sampler, uint pname, int param) { glSamplerParameteri(sampler, pname, param); }

        private delegate void glSamplerParameterfDelegate(uint sampler, uint pname, float param);
        [Require(MinVersion = "3.3", MinVersionES = "3.0", Extension = "GL_ARB_sampler_objects")]
        private static readonly glSamplerParameterfDelegate glSamplerParameterf = null;

        public static void SamplerParameterf(uint sampler, uint pname, float param) { glSamplerParameterf(sampler, pname, param); }

        private delegate void glSamplerParameterivDelegate(uint sampler, uint pname, int* @params);
        [Require(MinVersion = "3.3", MinVersionES = "3.0", Extension = "GL_ARB_sampler_objects")]
        private static readonly glSamplerParameterivDelegate glSamplerParameteriv = null;

        public static void SamplerParameteriv(uint sampler, uint pname, int* @params) { glSamplerParameteriv(sampler, pname, @params); }

        private delegate void glSamplerParameterfvDelegate(uint sampler, uint pname, float* @params);
        [Require(MinVersion = "3.3", MinVersionES = "3.0", Extension = "GL_ARB_sampler_objects")]
        private static readonly glSamplerParameterfvDelegate glSamplerParameterfv = null;

        public static void SamplerParameterfv(uint sampler, uint pname, float* @params) { glSamplerParameterfv(sampler, pname, @params); }

        private delegate void glSamplerParameterIivDelegate(uint sampler, uint pname, int* @params);
        [Require(MinVersion = "3.3", MinVersionES = "3.0", Extension = "GL_ARB_sampler_objects")]
        private static readonly glSamplerParameterIivDelegate glSamplerParameterIiv = null;

        public static void SamplerParameterIiv(uint sampler, uint pname, int* @params) { glSamplerParameterIiv(sampler, pname, @params); }

        private delegate void glSamplerParameterIuivDelegate(uint sampler, uint pname, uint* @params);
        [Require(MinVersion = "3.3", MinVersionES = "3.0", Extension = "GL_ARB_sampler_objects")]
        private static readonly glSamplerParameterIuivDelegate glSamplerParameterIuiv = null;

        public static void SamplerParameterIuiv(uint sampler, uint pname, uint* @params) { glSamplerParameterIuiv(sampler, pname, @params); }

        private delegate void glGetSamplerParameterivDelegate(uint sampler, uint pname, int* @params);
        [Require(MinVersion = "3.3", MinVersionES = "3.0", Extension = "GL_ARB_sampler_objects")]
        private static readonly glGetSamplerParameterivDelegate glGetSamplerParameteriv = null;

        public static void GetSamplerParameteriv(uint sampler, uint pname, int* @params) { glGetSamplerParameteriv(sampler, pname, @params); }

        private delegate void glGetSamplerParameterfvDelegate(uint sampler, uint pname, float* @params);
        [Require(MinVersion = "3.3", MinVersionES = "3.0", Extension = "GL_ARB_sampler_objects")]
        private static readonly glGetSamplerParameterfvDelegate glGetSamplerParameterfv = null;

        public static void GetSamplerParameterfv(uint sampler, uint pname, float* @params) { glGetSamplerParameterfv(sampler, pname, @params); }

        private delegate void glGetSamplerParameterIivDelegate(uint sampler, uint pname, int* @params);
        [Require(MinVersion = "3.3", MinVersionES = "3.0", Extension = "GL_ARB_sampler_objects")]
        private static readonly glGetSamplerParameterIivDelegate glGetSamplerParameterIiv = null;

        public static void GetSamplerParameterIiv(uint sampler, uint pname, int* @params) { glGetSamplerParameterIiv(sampler, pname, @params); }

        private delegate void glGetSamplerParameterIuivDelegate(uint sampler, uint pname, uint* @params);
        [Require(MinVersion = "3.3", MinVersionES = "3.0", Extension = "GL_ARB_sampler_objects")]
        private static readonly glGetSamplerParameterIuivDelegate glGetSamplerParameterIuiv = null;

        public static void GetSamplerParameterIuiv(uint sampler, uint pname, uint* @params) { glGetSamplerParameterIuiv(sampler, pname, @params); }
    }
}
