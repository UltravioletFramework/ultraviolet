using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Ultraviolet.Core;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics
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
            Contract.Require(vertexShader, nameof(vertexShader));
            Contract.Require(fragmentShader, nameof(fragmentShader));

            Ultraviolet.ValidateResource(vertexShader);
            Ultraviolet.ValidateResource(fragmentShader);

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

                var attributeCount = gl.GetProgrami(program, gl.GL_ACTIVE_ATTRIBUTES);
                gl.ThrowIfError();

                unsafe
                {
                    var namebuf = Marshal.AllocHGlobal(256);
                    try
                    {
                        for (int i = 0; i < attributeCount; i++)
                        {
                            var attrNameLen = 0;
                            var attrName = default(String);
                            var attrSize = 0;
                            var attrType = 0u;
                            gl.GetActiveAttrib(program, (uint)i, 256, &attrNameLen, &attrSize, &attrType, (sbyte*)namebuf);
                            gl.ThrowIfError();

                            attrName = Marshal.PtrToStringAnsi(namebuf);

                            var location = gl.GetAttribLocation(program, attrName);
                            gl.ThrowIfError();

                            attributeLocations[attrName] = location;
                            attributeTypes[attrName] = attrType;
                        }
                    }
                    finally { Marshal.FreeHGlobal(namebuf); }
                }

                if (status == 0)
                    throw new InvalidOperationException(log);
            });

            this.program = program;
            this.uniforms = CreateUniformCollection();
        }

        /// <summary>
        /// Gets the location of the specified attribute within the shader program's list of attributes.
        /// </summary>
        /// <param name="name">The name of the attribute to evaluate.</param>
        /// <returns>The location of the specified attribute, or -1 if the attribute does not exist within the program.</returns>
        public Int32 GetAttribLocation(String name)
        {
            var category = OpenGLAttribCategory.Single;
            return GetAttribLocation(name, out category);
        }

        /// <summary>
        /// Gets the location of the specified attribute within the shader program's list of attributes.
        /// </summary>
        /// <param name="name">The name of the attribute to evaluate.</param>
        /// <param name="category">A value specifying the attribute's type category.</param>
        /// <returns>The location of the specified attribute, or -1 if the attribute does not exist within the program.</returns>
        public Int32 GetAttribLocation(String name, out OpenGLAttribCategory category)
        {
            category = OpenGLAttribCategory.Single;

            var location = -1;
            if (attributeLocations.TryGetValue(name, out location))
            {
                var type = attributeTypes[name];
                switch (type)
                {
                    case gl.GL_INT:
                    case gl.GL_INT_VEC2:
                    case gl.GL_INT_VEC3:
                    case gl.GL_INT_VEC4:
                    case gl.GL_UNSIGNED_INT:
                    case gl.GL_UNSIGNED_INT_VEC2:
                    case gl.GL_UNSIGNED_INT_VEC3:
                    case gl.GL_UNSIGNED_INT_VEC4:
                        category = OpenGLAttribCategory.Integer;
                        break;

                    case gl.GL_DOUBLE:
                        category = OpenGLAttribCategory.Double;
                        break;
                }
            }

            return location;
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

                    switch (type)
                    {
                        case gl.GL_SAMPLER_1D:
                        case gl.GL_SAMPLER_1D_ARRAY:
                        case gl.GL_SAMPLER_1D_ARRAY_SHADOW:
                        case gl.GL_SAMPLER_1D_SHADOW:
                        case gl.GL_SAMPLER_2D:
                        case gl.GL_SAMPLER_2D_ARRAY:
                        case gl.GL_SAMPLER_2D_ARRAY_SHADOW:
                        case gl.GL_SAMPLER_2D_MULTISAMPLE:
                        case gl.GL_SAMPLER_2D_MULTISAMPLE_ARRAY:
                        case gl.GL_SAMPLER_2D_RECT:
                        case gl.GL_SAMPLER_2D_RECT_SHADOW:
                        case gl.GL_SAMPLER_2D_SHADOW:
                        case gl.GL_SAMPLER_3D:
                        case gl.GL_SAMPLER_CUBE:
                        case gl.GL_SAMPLER_CUBE_MAP_ARRAY:
                        case gl.GL_SAMPLER_CUBE_MAP_ARRAY_SHADOW:
                        case gl.GL_SAMPLER_CUBE_SHADOW:
                            sampler++;
                            break;
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

        // Attrib cache.
        private readonly Dictionary<String, Int32> attributeLocations = new Dictionary<String, Int32>();
        private readonly Dictionary<String, UInt32> attributeTypes = new Dictionary<String, UInt32>();
    }
}
