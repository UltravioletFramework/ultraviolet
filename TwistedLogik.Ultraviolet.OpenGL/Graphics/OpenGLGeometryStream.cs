using System;
using System.Collections.Generic;
using System.Linq;
using TwistedLogik.Gluon;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL/SDL2 implementation of the GeometryStream class.
    /// </summary>
    public sealed class OpenGLGeometryStream : GeometryStream, IOpenGLResource
    {
        /// <summary>
        /// Initializes the OpenGLGeometryStream type.
        /// </summary>
        static OpenGLGeometryStream()
        {
            vertexAttributeNames = 
                (from name in Enum.GetNames(typeof(VertexUsage))
                 from index in Enumerable.Range(0, VertexElement.UsageIndexCount)
                 select String.Format("uv_{0}{1}", name, index)).ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the OpenGLGeometryStream class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public OpenGLGeometryStream(UltravioletContext uv)
            : base(uv)
        {
            var vao = 0u;

            if (OpenGLState.SupportsVertexArrayObjects)
            {
                uv.QueueWorkItemAndWait(() =>
                {
                    vao = gl.GenVertexArray();
                    gl.ThrowIfError();
                });
            }

            this.vao = vao;
        }

        /// <inheritdoc/>
        public override void Attach(VertexBuffer vbuffer)
        {
            Contract.Require(vbuffer, "vbuffer");
            Contract.EnsureNotDisposed(this, Disposed);

            Ultraviolet.ValidateResource(vbuffer);

            if (vbuffers == null)
                vbuffers = new List<OpenGLVertexBuffer>(1);

            var sdlVertexBuffer = (OpenGLVertexBuffer)vbuffer;
            var sdlVertexBufferName = sdlVertexBuffer.OpenGLName;

            this.vbuffers.Add(sdlVertexBuffer);
        }

        /// <inheritdoc/>
        public override void Attach(IndexBuffer ibuffer)
        {
            Contract.Require(ibuffer, "ibuffer");
            Contract.EnsureNot(HasIndices, UltravioletStrings.GeometryStreamAlreadyHasIndices);
            Contract.EnsureNotDisposed(this, Disposed);

            Ultraviolet.ValidateResource(ibuffer);

            var sdlIndexBuffer = (OpenGLIndexBuffer)ibuffer;
            var sdlIndexBufferName = sdlIndexBuffer.OpenGLName;

            this.ibuffer = sdlIndexBuffer;

            if (IsUsingVertexArrayObject)
            {
                using (OpenGLState.ScopedBindVertexArrayObject(vao, 0, 0, true))
                {
                    gl.BindBuffer(gl.GL_ELEMENT_ARRAY_BUFFER, sdlIndexBufferName);
                    gl.ThrowIfError();
                }
            }

            this.glElementArrayBufferBinding = sdlIndexBufferName;
            this.indexBufferElementType      = ibuffer.IndexElementType;
        }

        /// <summary>
        /// Applies the geometry stream to the graphics device.
        /// </summary>
        public void Apply()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (IsUsingVertexArrayObject)
            {
                OpenGLState.BindVertexArrayObject(vao, 0, glElementArrayBufferBinding ?? 0);
            }
            else
            {
                if (ibuffer != null)
                {
                    OpenGLState.BindElementArrayBuffer(ibuffer.OpenGLName);
                }
            }
        }

        /// <summary>
        /// Applies the geometry stream's vertex attributes to the specified program.
        /// </summary>
        /// <param name="program">The OpenGL identifier of the shader program being used to render.</param>
        public void ApplyAttributes(UInt32 program)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(IsValid, OpenGLStrings.InvalidGeometryStream);

            if (!SwitchCachedProgram(program))
                return;

            unsafe
            {
                var previousBuffer = (uint)OpenGLState.GL_ARRAY_BUFFER_BINDING;

                DisableVertexAttributesOnCachedProgram();

                foreach (var vbuffer in vbuffers)
                {
                    BindVertexAttributesForBuffer(vbuffer);
                }

                gl.BindBuffer(gl.GL_ARRAY_BUFFER, previousBuffer);
                gl.ThrowIfError();
            }

            this.program = program;
        }

        /// <summary>
        /// Gets a value indicating whether this geometry stream is implemented using a vertex array object (VAO).
        /// </summary>
        public Boolean IsUsingVertexArrayObject
        {
            get { return vao != 0; }
        }

        /// <inheritdoc/>
        public UInt32 OpenGLName
        {
            get 
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return vao; 
            }
        }

        /// <inheritdoc/>
        public override Boolean HasMultipleSources
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return vbuffers != null && vbuffers.Count > 0;
            }
        }

        /// <inheritdoc/>
        public override Boolean HasVertices
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return vbuffers != null;
            }
        }

        /// <inheritdoc/>
        public override Boolean HasIndices
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return ibuffer != null;
            }
        }

        /// <inheritdoc/>
        public override IndexBufferElementType IndexBufferElementType
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return indexBufferElementType;
            }
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                if (!Ultraviolet.Disposed)
                {
                    if (vao != 0)
                    {
                        Ultraviolet.QueueWorkItem((state) =>
                        {
                            var vaoName = ((OpenGLGeometryStream)state).vao;

                            gl.DeleteVertexArray(vaoName);
                            gl.ThrowIfError();

                            OpenGLState.DeleteVertexArrayObject(vaoName, 0, glElementArrayBufferBinding ?? 0);
                        }, this);
                    }
                }
                vbuffers.Clear();
                ibuffer = null;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets the OpenGL vertex format that corresponds to the specified Ultraviolet vertex format.
        /// </summary>
        /// <param name="format">The vertex format to convert.</param>
        /// <param name="size">The number of components in the element.</param>
        /// <param name="stride">The vertex stride in bytes.</param>
        /// <returns>The converted vertex format.</returns>
        private static UInt32 GetVertexFormatGL(VertexFormat format, out Int32 size, out Int32 stride)
        {
            switch (format)
            {
                case VertexFormat.Single:
                    size = 1;
                    stride = size * sizeof(float);
                    return gl.GL_FLOAT;

                case VertexFormat.Vector2:
                    size = 2;
                    stride = size * sizeof(float);
                    return gl.GL_FLOAT;

                case VertexFormat.Vector3:
                    size = 3;
                    stride = size * sizeof(float);
                    return gl.GL_FLOAT;

                case VertexFormat.Vector4:
                    size = 4;
                    stride = size * sizeof(float);
                    return gl.GL_FLOAT;

                case VertexFormat.Color:
                    size = 4;
                    stride = size * sizeof(byte);
                    return gl.GL_UNSIGNED_BYTE;

                default:
                    throw new NotSupportedException(OpenGLStrings.UnsupportedVertexFormat);
            }
        }

        /// <summary>
        /// Gets the name of the shader attribute which corresponds to the specified Ultraviolet vertex usage.
        /// </summary>
        /// <param name="usage">The vertex usage for which to retrieve a name.</param>
        /// <param name="index">The vertex usage index for which to retrieve a name.</param>
        /// <returns>The name of the specified shader attribute.</returns>
        private static String GetVertexAttributeNameFromUsage(VertexUsage usage, Int32 index)
        {
            return vertexAttributeNames[((int)usage * VertexElement.UsageIndexCount) + index];
        }

        /// <summary>
        /// Attempts to associate this geometry stream with the specified OpenGL program.
        /// </summary>
        /// <param name="program">The OpenGL name of the program with which to associate this stream.</param>
        /// <returns><c>true</c> if the stream's cached program was changed; otherwise, <c>false</c>.</returns>
        private Boolean SwitchCachedProgram(UInt32 program)
        {
            if (IsUsingVertexArrayObject && this.program == program)
                return false;

            this.program = program;

            return true;
        }

        /// <summary>
        /// Disables all of the active vertex attributes for the currently cached program.
        /// </summary>
        private void DisableVertexAttributesOnCachedProgram()
        {
            var attributes = 0;

            unsafe
            {
                gl.GetProgramiv(program, gl.GL_ACTIVE_ATTRIBUTES, &attributes);
                gl.ThrowIfError();
            }

            for (int i = 0; i < attributes; i++)
            {
                gl.DisableVertexAttribArray((uint)i);
                gl.ThrowIfError();
            }
        }

        /// <summary>
        /// Binds the specified buffer's vertex attributes to the currently cached program.
        /// </summary>
        /// <param name="vbuffer">The vertex buffer to bind to the cached program.</param>
        private void BindVertexAttributesForBuffer(OpenGLVertexBuffer vbuffer)
        {
            gl.BindBuffer(gl.GL_ARRAY_BUFFER, vbuffer.OpenGLName);
            gl.ThrowIfError();

            var position = 0u;

            foreach (var element in vbuffer.VertexDeclaration)
            {
                var name = GetVertexAttributeNameFromUsage(element.Usage, element.Index);
                var size = 0;
                var stride = 0;
                var type = GetVertexFormatGL(element.Format, out size, out stride);

                var location = gl.GetAttribLocation(program, name);
                if (location >= 0)
                {
                    unsafe
                    {
                        gl.VertexAttribPointer((uint)location, size, type, true, vbuffer.VertexDeclaration.VertexStride, (void*)(position));
                        gl.ThrowIfError();
                    }

                    gl.EnableVertexAttribArray((uint)location);
                    gl.ThrowIfError();
                }

                position += (uint)stride;
            }
        }

        // The names of vertex attributes which correspond to Ultraviolet vertex usages.
        private static readonly String[] vertexAttributeNames;

        // Underlying vertex and index buffers.
        private List<OpenGLVertexBuffer> vbuffers;
        private OpenGLIndexBuffer ibuffer;
        private IndexBufferElementType indexBufferElementType = IndexBufferElementType.Int16;

        // The Vertex Array Object (VAO) that this buffer represents.
        private readonly UInt32 vao;
        private UInt32 program;

        // GL state values
        private UInt32? glElementArrayBufferBinding;
    }
}
