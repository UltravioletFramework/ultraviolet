using System;
using TwistedLogik.Gluon;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents a fragment shader.
    /// </summary>
    public sealed class OpenGLFragmentShader : UltravioletResource, IOpenGLResource
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLFragmentShader class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="source">The shader source.</param>
        public OpenGLFragmentShader(UltravioletContext uv, String[] source)
            : base(uv)
        {
            Contract.Require(source, "source");

            var shader = 0u;

            uv.QueueWorkItemAndWait(() =>
            {
                shader = gl.CreateShader(gl.GL_FRAGMENT_SHADER);
                gl.ThrowIfError();

                var log = String.Empty;
                if (!ShaderCompiler.Compile(shader, source, out log))
                    throw new InvalidOperationException(log);
            });

            this.shader = shader;
        }
        
        /// <summary>
        /// Initializes a new instance of the OpenGLFragmentShader class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="source">The shader source.</param>
        public OpenGLFragmentShader(UltravioletContext uv, String source)
            : this(uv, new[] { source })
        {

        }

        /// <summary>
        /// Gets the OpenGL shader handle.
        /// </summary>
        public UInt32 OpenGLName
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);
                
                return shader;
            }
        }

        /// <summary>
        /// Releases resources associated with this object.
        /// </summary>
        /// <param name="disposing">true if the object is being disposed; false if the object is being finalized.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                if (!Ultraviolet.Disposed)
                {
                    Ultraviolet.QueueWorkItem((state) =>
                    {
                        gl.DeleteShader(((OpenGLFragmentShader)state).shader);
                        gl.ThrowIfError();
                    }, this);
                }
            }

            base.Dispose(disposing);
        }

        // State values.
        private readonly UInt32 shader;
    }
}
