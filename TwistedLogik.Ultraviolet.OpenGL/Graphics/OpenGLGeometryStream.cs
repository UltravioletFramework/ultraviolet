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

            uv.QueueWorkItemAndWait(() =>
            {
                vao = gl.GenVertexArray();
                gl.ThrowIfError();
            });

            this.vao = vao;
        }

        /// <summary>
        /// Attaches a vertex buffer to the geometry buffer.
        /// </summary>
        /// <param name="vbuffer">The vertex buffer to attach to the geometry buffer.</param>
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

        /// <summary>
        /// Attaches an index buffer to the geometry buffer.
        /// </summary>
        /// <param name="ibuffer">The index buffer to attach to the geometry buffer.</param>
        public override void Attach(IndexBuffer ibuffer)
        {
            Contract.Require(ibuffer, "ibuffer");
            Contract.EnsureNot(HasIndices, UltravioletStrings.GeometryStreamAlreadyHasIndices);
            Contract.EnsureNotDisposed(this, Disposed);

            Ultraviolet.ValidateResource(ibuffer);

            if (ibuffers == null)
                ibuffers = new List<OpenGLIndexBuffer>(1);

            var sdlIndexBuffer     = (OpenGLIndexBuffer)ibuffer;
            var sdlIndexBufferName = sdlIndexBuffer.OpenGLName;

            this.ibuffers.Add(sdlIndexBuffer);

            using (OpenGLState.ScopedBindVertexArrayObject(vao, 0, 0, true))
            {
                gl.BindBuffer(gl.GL_ELEMENT_ARRAY_BUFFER, sdlIndexBufferName);
                gl.ThrowIfError();
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

            OpenGLState.BindVertexArrayObject(vao, 0, glElementArrayBufferBinding ?? 0);
        }

        /// <summary>
        /// Applies the geometry stream's vertex attributes to the specified program.
        /// </summary>
        /// <param name="program">The OpenGL identifier of the shader program being used to render.</param>
        public void ApplyAttributes(UInt32 program)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(IsValid, OpenGLStrings.InvalidGeometryStream);

            if (this.program == program)
                return;

            unsafe
            {
                var attributes = 0;
                gl.GetProgramiv(program, gl.GL_ACTIVE_ATTRIBUTES, &attributes);
                gl.ThrowIfError();

                for (int i = 0; i < attributes; i++)
                {
                    gl.DisableVertexAttribArray((uint)i);
                    gl.ThrowIfError();
                }

                var position = 0u;
                foreach (var vbuffer in vbuffers)
                {
                    gl.BindBuffer(gl.GL_ARRAY_BUFFER, vbuffer.OpenGLName);
                    gl.ThrowIfError();

                    foreach (var element in vbuffer.VertexDeclaration)
                    {
                        var name = GetVertexAttributeNameFromUsage(element.Usage, element.Index);
                        var size = 0;
                        var stride = 0;
                        var type = GetVertexFormatGL(element.Format, out size, out stride);

                        var location = gl.GetAttribLocation(program, name);
                        if (location >= 0)
                        {
                            gl.VertexAttribPointer((uint)location, size, type, true, vbuffer.VertexDeclaration.VertexStride, (void*)(position));
                            gl.ThrowIfError();

                            gl.EnableVertexAttribArray((uint)location);
                            gl.ThrowIfError();
                        }

                        position += (uint)stride;
                    }
                }

                gl.BindBuffer(gl.GL_ARRAY_BUFFER, 0);
                gl.ThrowIfError();
            }

            this.program = program;
        }

        /// <summary>
        /// Gets the resource's OpenGL name.
        /// </summary>
        public UInt32 OpenGLName
        {
            get 
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return vao; 
            }
        }

        /// <summary>
        /// Gets a value indicating whether the geometry stream reads data from multiple vertex buffers.
        /// </summary>
        public override Boolean HasMultipleSources
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return vbuffers != null && vbuffers.Count > 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the geometry buffer has any vertex buffers attached to it.
        /// </summary>
        public override Boolean HasVertices
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return vbuffers != null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the geometry buffer has any index buffers attached to it.
        /// </summary>
        public override Boolean HasIndices
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return ibuffers != null;
            }
        }

        /// <summary>
        /// Gets the type of the elements in the stream's index buffer, if it has an index buffer.
        /// </summary>
        public override IndexBufferElementType IndexBufferElementType
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return indexBufferElementType;
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
                        var vao = ((OpenGLGeometryStream)state).vao;

                        gl.DeleteVertexArray(vao);
                        gl.ThrowIfError();

                        OpenGLState.DeleteVertexArrayObject(vao, 0, glElementArrayBufferBinding ?? 0);
                    }, this);
                }
                vbuffers.Clear();
                ibuffers.Clear();
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

        // The names of vertex attributes which correspond to Ultraviolet vertex usages.
        private static readonly String[] vertexAttributeNames;

        // Underlying vertex and index buffers.
        private List<OpenGLVertexBuffer> vbuffers;
        private List<OpenGLIndexBuffer> ibuffers;
        private IndexBufferElementType indexBufferElementType = IndexBufferElementType.Int16;

        // The Vertex Array Object (VAO) that this buffer represents.
        private readonly UInt32 vao;
        private UInt32 program;

        // GL state values
        private UInt32? glElementArrayBufferBinding;
    }
}
