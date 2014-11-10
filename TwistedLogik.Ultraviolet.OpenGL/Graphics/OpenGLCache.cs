using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using TwistedLogik.Gluon;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Contains cached values associated with the OpenGL context.
    /// The implementation caches these values in this class in order to avoid having
    /// to retrieve them through calls to glGet().
    /// </summary>
    /// <remarks>If objects are bound to the OpenGL context outside of Ultraviolet, it is possible that this cache
    /// will fall out of sync with the actual state of the device.  When the VERIFY_OPENGL_CACHE compilation symbol is defined,
    /// the state of the cache can be verified against the device by calling the <see cref="TwistedLogik.Ultraviolet.OpenGL.Graphics.OpenGLCache.Verify()"/> 
    /// method.  Verification is compiled out if this flag is not defined.</remarks>
    internal static class OpenGLCache
    {
        static OpenGLCache()
        {
            EXT_direct_state_access = gl.IsExtensionSupported("GL_EXT_direct_state_access");

            glCachedIntegers = typeof(OpenGLCache).GetFields(BindingFlags.NonPublic | BindingFlags.Static)
                .Where(x => x.FieldType == typeof(OpenGLCachedInteger))
                .Select(x => (OpenGLCachedInteger)x.GetValue(null)).ToArray();
        }

        /// <summary>
        /// Resets the state of the cache.
        /// </summary>
        public static void Reset()
        {
            foreach (var glCachedInteger in glCachedIntegers)
            {
                glCachedInteger.Reset();
            }
        }

        [Conditional("VERIFY_OPENGL_CACHE")]
        public static void Verify()
        {
            foreach (var value in glCachedIntegers)
            {
                value.Verify();
            }
        }

        public static Boolean EXT_direct_state_access { get; private set; }

        public static OpenGLCachedInteger GL_CURRENT_PROGRAM              { get { return glCurrentProgram; } }
        public static OpenGLCachedInteger GL_TEXTURE_BINDING_1D           { get { return glTextureBinding1D; } }
        public static OpenGLCachedInteger GL_TEXTURE_BINDING_2D           { get { return glTextureBinding2D; } }
        public static OpenGLCachedInteger GL_TEXTURE_BINDING_3D           { get { return glTextureBinding3D; } }
        public static OpenGLCachedInteger GL_VERTEX_ARRAY_BINDING         { get { return glVertexArrayBinding; } }
        public static OpenGLCachedInteger GL_ARRAY_BUFFER_BINDING         { get { return glArrayBufferBinding; } }
        public static OpenGLCachedInteger GL_ELEMENT_ARRAY_BUFFER_BINDING { get { return glElementArrayBufferBinding; } }
        public static OpenGLCachedInteger GL_FRAMEBUFFER_BINDING          { get { return glFramebufferBinding; } }

        private static readonly OpenGLCachedInteger glCurrentProgram            = new OpenGLCachedInteger("GL_CURRENT_PROGRAM", gl.GL_CURRENT_PROGRAM);
        private static readonly OpenGLCachedInteger glTextureBinding1D          = new OpenGLCachedInteger("GL_TEXTURE_BINDING_1D", gl.GL_TEXTURE_BINDING_1D);
        private static readonly OpenGLCachedInteger glTextureBinding2D          = new OpenGLCachedInteger("GL_TEXTURE_BINDING_2D", gl.GL_TEXTURE_BINDING_2D);
        private static readonly OpenGLCachedInteger glTextureBinding3D          = new OpenGLCachedInteger("GL_TEXTURE_BINDING_3D", gl.GL_TEXTURE_BINDING_3D);
        private static readonly OpenGLCachedInteger glVertexArrayBinding        = new OpenGLCachedInteger("GL_VERTEX_ARRAY_BINDING", gl.GL_VERTEX_ARRAY_BINDING);
        private static readonly OpenGLCachedInteger glArrayBufferBinding        = new OpenGLCachedInteger("GL_ARRAY_BUFFER_BINDING", gl.GL_ARRAY_BUFFER_BINDING);
        private static readonly OpenGLCachedInteger glElementArrayBufferBinding = new OpenGLCachedInteger("GL_ELEMENT_ARRAY_BUFFER_BINDING", gl.GL_ELEMENT_ARRAY_BUFFER_BINDING);
        private static readonly OpenGLCachedInteger glFramebufferBinding        = new OpenGLCachedInteger("GL_FRAMEBUFFER_BINDING", gl.GL_FRAMEBUFFER_BINDING);

        private static readonly OpenGLCachedInteger[] glCachedIntegers;
    }
}
