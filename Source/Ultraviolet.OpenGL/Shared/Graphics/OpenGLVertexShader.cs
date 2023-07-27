using System;
using Ultraviolet.Core;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents a vertex shader.
    /// </summary>
    public unsafe sealed class OpenGLVertexShader : UltravioletResource, IOpenGLResource
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLVertexShader class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="source">The shader source.</param>
        public OpenGLVertexShader(UltravioletContext uv, ShaderSource[] source)
            : base(uv)
        {
            Contract.Require(source, nameof(source));

            var shader = 0u;
            var ssmd = default(ShaderSourceMetadata);

            uv.QueueWorkItem(state =>
            {
                shader = GL.CreateShader(GL.GL_VERTEX_SHADER);
                GL.ThrowIfError();
                
                if (!ShaderCompiler.Compile(shader, source, out var log, out ssmd))
                    throw new InvalidOperationException(log);                
            }).Wait();

            this.OpenGLName = shader;
            this.ShaderSourceMetadata = ssmd;
        }

        /// <summary>
        /// Initializes a new instance of the OpenGLVertexShader class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="source">The shader source.</param>
        public OpenGLVertexShader(UltravioletContext uv, ShaderSource source)
            : this(uv, new[] { source })
        {

        }

        /// <summary>
        /// Gets the OpenGL shader handle.
        /// </summary>
        public UInt32 OpenGLName { get; private set; }

        /// <summary>
        /// Gets the shader source metadata for this shader.
        /// </summary>
        public ShaderSourceMetadata ShaderSourceMetadata { get; }

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
                var glname = OpenGLName;
                if (glname != 0 && !Ultraviolet.Disposed)
                {
                    Ultraviolet.QueueWorkItem((state) =>
                    {
                        GL.DeleteShader(glname);
                        GL.ThrowIfError();
                    }, this, WorkItemOptions.ReturnNullOnSynchronousExecution);
                }

                OpenGLName = 0;
            }

            base.Dispose(disposing);
        }
    }
}
