using System;
using System.Collections.Generic;
using System.Linq;
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

            var concatenatedSourceMetadata = new ShaderSourceMetadata();
            concatenatedSourceMetadata.Concat(vertexShader.ShaderSourceMetadata);
            concatenatedSourceMetadata.Concat(fragmentShader.ShaderSourceMetadata);
            
            var program = 0u;

            uv.QueueWorkItem(state =>
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
            }).Wait();

            this.program = program;
            this.uniforms = CreateUniformCollection(concatenatedSourceMetadata);            
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

            if (attributeLocations.TryGetValue(name, out var location))
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
            else
            {
                return -1;
            }

            return location;
        }

        /// <summary>
        /// Gets the resource's OpenGL name.
        /// </summary>
        public UInt32 OpenGLName => program;

        /// <summary>
        /// Gets the program's vertex shader.
        /// </summary>
        public OpenGLVertexShader VertexShader => vertexShader;

        /// <summary>
        /// Gets the program's fragment shader.
        /// </summary>
        public OpenGLFragmentShader FragmentShader => fragmentShader;

        /// <summary>
        /// Gets the program's collection of uniforms.
        /// </summary>
        public OpenGLShaderUniformCollection Uniforms => uniforms;

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
                var glname = program;
                if (glname != 0 && !Ultraviolet.Disposed)
                {
                    Ultraviolet.QueueWorkItem((state) =>
                    {
                        gl.DeleteProgram(glname);
                        gl.ThrowIfError();
                    }, this, WorkItemOptions.ReturnNullOnSynchronousExecution);
                }

                if (programOwnsShaders)
                {
                    SafeDispose.Dispose(vertexShader);
                    SafeDispose.Dispose(fragmentShader);
                }

                program = 0;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Creates the effect pass' collection of uniforms.
        /// </summary>
        /// <param name="ssmd">The source metadata for this program.</param>
        /// <returns>The collection of uniforms that was created.</returns>
        private OpenGLShaderUniformCollection CreateUniformCollection(ShaderSourceMetadata ssmd)
        {
            var result = Ultraviolet.QueueWorkItem(state =>
            {
                var programObject = ((OpenGLShaderProgram)state);
                var program = programObject.program;
                var uniforms = new List<OpenGLShaderUniform>();
                var samplerCount = 0;

                var count = gl.GetProgrami(program, gl.GL_ACTIVE_UNIFORMS);
                gl.ThrowIfError();

                for (uint i = 0; i < count; i++)
                {
                    var name = gl.GetActiveUniform(program, i, out var type, out var elements);
                    gl.ThrowIfError();

                    var location = gl.GetUniformLocation(program, name);
                    gl.ThrowIfError();

                    var isSampler = false;
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
                            isSampler = true;
                            break;
                    }

                    var sampler = isSampler ? samplerCount++ : -1;
                    if (isSampler && ssmd.PreferredSamplerIndices.ContainsKey(name))
                    {
                        samplerCount = ssmd.PreferredSamplerIndices[name];
                        sampler = samplerCount++;
                    }

                    uniforms.Add(new OpenGLShaderUniform(programObject.Ultraviolet, name, type, elements, program, location, sampler));                
                }

                return uniforms;
            }, this).Result;

            // Validation: make sure all preferred samplers correspond to an actual uniform
            var missingUniform = ssmd.PreferredSamplerIndices.Keys.Where(x => !result.Where(y => String.Equals(y.Name, x, StringComparison.Ordinal)).Any()).FirstOrDefault();
            if (missingUniform != null)
                throw new ArgumentException(OpenGLStrings.SamplerDirectiveInvalidUniform.Format(missingUniform));

            return new OpenGLShaderUniformCollection(result);
        }

        // Property values.
        private UInt32 program;
        private readonly OpenGLVertexShader vertexShader;
        private readonly OpenGLFragmentShader fragmentShader;
        private readonly OpenGLShaderUniformCollection uniforms;
        private readonly Boolean programOwnsShaders;

        // Attrib cache.
        private readonly Dictionary<String, Int32> attributeLocations = new Dictionary<String, Int32>();
        private readonly Dictionary<String, UInt32> attributeTypes = new Dictionary<String, UInt32>();
    }
}
