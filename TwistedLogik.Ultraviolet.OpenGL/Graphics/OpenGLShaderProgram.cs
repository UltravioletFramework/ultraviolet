using System;
using System.Collections.Generic;
using TwistedLogik.Gluon;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents an OpenGL shader program.
    /// </summary>
    public sealed class OpenGLShaderProgram : UltravioletResource, IOpenGLResource
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLShaderProgram class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="vertexShader">The program's vertex shader.</param>
        /// <param name="fragmentShader">The program's fragment shader.</param>
        /// <param name="programOwnsShaders">A value indicating whether the program owns the shader objects.</param>
        public OpenGLShaderProgram(UltravioletContext uv, OpenGLVertexShader vertexShader, OpenGLFragmentShader fragmentShader, Boolean programOwnsShaders)
            : base(uv)
        {
            Contract.Require(vertexShader, "vertexShader");
            Contract.Require(fragmentShader, "fragmentShader");

            Ultraviolet.ValidateResource((UltravioletResource)vertexShader);
            Ultraviolet.ValidateResource((UltravioletResource)fragmentShader);

            this.vertexShader = vertexShader;
            this.fragmentShader = fragmentShader;
            this.programOwnsShaders = programOwnsShaders;

            var program = 0u;

            uv.QueueWorkItemAndWait(() =>
            {
                program = gl.CreateProgram();
                gl.ThrowIfError();

                gl.AttachShader(program, vertexShader.OpenGLName);
                gl.ThrowIfError();

                gl.AttachShader(program, fragmentShader.OpenGLName);
                gl.ThrowIfError();

                gl.LinkProgram(program);
                gl.ThrowIfError();

                var log = gl.GetProgramInfoLog(program);
                gl.ThrowIfError();

                var status = gl.GetProgrami(program, gl.GL_LINK_STATUS);
                gl.ThrowIfError();

                if (status == 0)
                    throw new InvalidOperationException(log);
            });

            this.program = program;
            this.uniforms = CreateUniformCollection();
        }

        /// <summary>
        /// Gets the resource's OpenGL name.
        /// </summary>
        public UInt32 OpenGLName
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return program;
            }
        }

        /// <summary>
        /// Gets the program's vertex shader.
        /// </summary>
        public IOpenGLResource VertexShader
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return vertexShader;
            }
        }

        /// <summary>
        /// Gets the program's fragment shader.
        /// </summary>
        public IOpenGLResource FragmentShader
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return fragmentShader;
            }
        }

        /// <summary>
        /// Gets the program's collection of uniforms.
        /// </summary>
        public OpenGLShaderUniformCollection Uniforms
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return uniforms;
            }
        }

        /// <summary>
        /// Releases resources associated with the object.
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
                        gl.DeleteProgram(((OpenGLShaderProgram)state).program);
                        gl.ThrowIfError();
                    }, this);
                }

                if (programOwnsShaders)
                {
                    SafeDispose.Dispose(vertexShader);
                    SafeDispose.Dispose(fragmentShader);
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Creates the effect pass' collection of uniforms.
        /// </summary>
        /// <returns>The collection of uniforms that was created.</returns>
        private OpenGLShaderUniformCollection CreateUniformCollection()
        {
            return Ultraviolet.QueueWorkItemAndWait((state) =>
            {
                var programObject = ((OpenGLShaderProgram)state);
                var program = programObject.program;
                var uniforms = new List<OpenGLShaderUniform>();
                var sampler = 0;

                var count = gl.GetProgrami(program, gl.GL_ACTIVE_UNIFORMS);
                gl.ThrowIfError();

                for (uint i = 0; i < count; i++)
                {
                    var type = 0u;
                    var name = gl.GetActiveUniform(program, i, out type);
                    gl.ThrowIfError();

                    var location = gl.GetUniformLocation(program, name);
                    gl.ThrowIfError();

                    uniforms.Add(new OpenGLShaderUniform(programObject.Ultraviolet, name, type, program, location, sampler));

                    if (type == gl.GL_SAMPLER_2D)
                    {
                        sampler++;
                    }
                }

                return new OpenGLShaderUniformCollection(uniforms);
            }, this);
        }

        // Property values.
        private readonly UInt32 program;
        private readonly OpenGLVertexShader vertexShader;
        private readonly OpenGLFragmentShader fragmentShader;
        private readonly OpenGLShaderUniformCollection uniforms;
        private readonly Boolean programOwnsShaders;
    }
}
