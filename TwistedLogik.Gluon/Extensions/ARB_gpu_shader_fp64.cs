
namespace TwistedLogik.Gluon
{
    public static unsafe partial class gl
    {
        private delegate void glUniform1dDelegate(int location, double v0);
        [Require(Extension = "GL_ARB_gpu_shader_fp64")]
        private static readonly glUniform1dDelegate glUniform1d = null;

        public static void Uniform1d(int location, double v0) { glUniform1d(location, v0); }

        private delegate void glUniform2dDelegate(int location, double v0, double v1);
        [Require(Extension = "GL_ARB_gpu_shader_fp64")]
        private static readonly glUniform2dDelegate glUniform2d = null;

        public static void Uniform2d(int location, double v0, double v1) { glUniform2d(location, v0, v1); }

        private delegate void glUniform3dDelegate(int location, double v0, double v1, double v2);
        [Require(Extension = "GL_ARB_gpu_shader_fp64")]
        private static readonly glUniform3dDelegate glUniform3d = null;

        public static void Uniform3d(int location, double v0, double v1, double v2) { glUniform3d(location, v0, v1, v2); }

        private delegate void glUniform4dDelegate(int location, double v0, double v1, double v2, double v3);
        [Require(Extension = "GL_ARB_gpu_shader_fp64")]
        private static readonly glUniform4dDelegate glUniform4d = null;

        public static void Uniform4d(int location, double v0, double v1, double v2, double v3) { glUniform4d(location, v0, v1, v2, v3); }

        private delegate void glUniform1dvDelegate(int location, int count, double* value);
        [Require(Extension = "GL_ARB_gpu_shader_fp64")]
        private static readonly glUniform1dvDelegate glUniform1dv = null;

        public static void Uniform1dv(int location, int count, double* value) { glUniform1dv(location, count, value); }

        private delegate void glUniform2dvDelegate(int location, int count, double* value);
        [Require(Extension = "GL_ARB_gpu_shader_fp64")]
        private static readonly glUniform2dvDelegate glUniform2dv = null;

        public static void Uniform2dv(int location, int count, double* value) { glUniform2dv(location, count, value); }

        private delegate void glUniform3dvDelegate(int location, int count, double* value);
        [Require(Extension = "GL_ARB_gpu_shader_fp64")]
        private static readonly glUniform3dvDelegate glUniform3dv = null;

        public static void Uniform3dv(int location, int count, double* value) { glUniform3dv(location, count, value); }

        private delegate void glUniform4dvDelegate(int location, int count, double* value);
        [Require(Extension = "GL_ARB_gpu_shader_fp64")]
        private static readonly glUniform4dvDelegate glUniform4dv = null;

        public static void Uniform4dv(int location, int count, double* value) { glUniform4dv(location, count, value); }
   }
}
