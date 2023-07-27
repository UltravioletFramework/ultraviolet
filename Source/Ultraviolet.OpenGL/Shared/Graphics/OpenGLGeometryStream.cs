using System;
using System.Collections.Generic;
using System.Linq;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL implementation of the GeometryStream class.
    /// </summary>
    public sealed partial class OpenGLGeometryStream : GeometryStream, IOpenGLResource
    {
        /// <summary>
        /// Initializes the OpenGLGeometryStream type.
        /// </summary>
        static OpenGLGeometryStream()
        {
            vertexAttributeNames =
                (from name in Enum.GetNames(typeof(VertexElementUsage))
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

            if (GL.IsVertexArrayObjectAvailable)
            {
                uv.QueueWorkItem(state =>
                {
                    vao = GL.GenVertexArray();
                    GL.ThrowIfError();
                }).Wait();
            }

            this.vao = vao;
        }

        /// <inheritdoc/>
        public override void Attach(VertexBuffer vbuffer)
        {
            Contract.Require(vbuffer, nameof(vbuffer));

            Ultraviolet.ValidateResource(vbuffer);

            AttachInternal(vbuffer, 0);
        }

        /// <inheritdoc/>
        public override void Attach(VertexBuffer vbuffer, Int32 instanceFrequency)
        {
            Contract.Require(vbuffer, nameof(vbuffer));
            Contract.EnsureRange(instanceFrequency >= 0, nameof(instanceFrequency));
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure<NotSupportedException>(SupportsInstancedRendering || instanceFrequency == 0);

            Ultraviolet.ValidateResource(vbuffer);

            AttachInternal(vbuffer, instanceFrequency);
        }

        /// <inheritdoc/>
        public override void Attach(IndexBuffer ibuffer)
        {
            Contract.Require(ibuffer, nameof(ibuffer));
            Contract.EnsureNot(HasIndices, UltravioletStrings.GeometryStreamAlreadyHasIndices);
            Contract.EnsureNotDisposed(this, Disposed);

            Ultraviolet.ValidateResource(ibuffer);

            var oglIndexBuffer = (OpenGLIndexBuffer)ibuffer;
            var oglIndexBufferName = oglIndexBuffer.OpenGLName;

            this.ibuffer = oglIndexBuffer;

            if (IsUsingVertexArrayObject)
            {
                using (OpenGLState.ScopedBindVertexArrayObject(vao, 0, force: true))
                {
                    OpenGLState.BindElementArrayBuffer(oglIndexBufferName);
                }
            }

            this.glElementArrayBufferBinding = oglIndexBufferName;
            this.indexBufferElementType = ibuffer.IndexElementType;
        }

        /// <summary>
        /// Applies the geometry stream to the graphics device.
        /// </summary>
        public void Apply()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (IsUsingVertexArrayObject)
            {
                OpenGLState.BindVertexArrayObject(vao, glElementArrayBufferBinding ?? 0);
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
        /// <param name="offset">The offset within the vertex buffer, in bytes, at which vertex begins.</param>
        public void ApplyAttributes(UInt32 program, UInt32 offset = 0)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Ensure(IsValid, OpenGLStrings.InvalidGeometryStream);

            var changedProgram = SwitchCachedProgram(program);
            var changedOffset = (this.offset != offset);
            if (changedProgram || changedOffset)
            {
                BindBuffers(
                    changedProgram ? program : (UInt32?)null,
                    changedOffset ? offset : (UInt32?)null);

                this.program = program;
                this.offset = offset;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this geometry stream is implemented using a vertex array object (VAO).
        /// </summary>
        public Boolean IsUsingVertexArrayObject
        {
            get { return vao != 0; }
        }

        /// <summary>
        /// Gets a value indicating whether the geometry stream supports instanced rendering.
        /// </summary>
        public Boolean SupportsInstancedRendering
        {
            get { return Ultraviolet.GetGraphics().Capabilities.SupportsInstancedRendering; }
        }

        /// <inheritdoc/>
        public UInt32 OpenGLName => vao;

        /// <inheritdoc/>
        public override Boolean HasMultipleSources => (vbuffers?.Count ?? 0) > 0;

        /// <inheritdoc/>
        public override Boolean HasVertices => vbuffers != null;

        /// <inheritdoc/>
        public override Boolean HasVertexPosition => hasVertexPosition;

        /// <inheritdoc/>
        public override Boolean HasVertexColor => hasVertexColor;

        /// <inheritdoc/>
        public override Boolean HasVertexTexture => hasVertexTexture;

        /// <inheritdoc/>
        public override Boolean HasVertexNormal => hasVertexNormal;

        /// <inheritdoc/>
        public override Boolean HasVertexTangent => hasVertexTangent;

        /// <inheritdoc/>
        public override Boolean HasVertexBlend => hasVertexBlend;

        /// <inheritdoc/>
        public override Boolean HasIndices => ibuffer != null;

        /// <inheritdoc/>
        public override IndexBufferElementType IndexBufferElementType => indexBufferElementType;

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                if (!Ultraviolet.Disposed)
                {
                    var gfx = Ultraviolet.GetGraphics();
                    if (gfx.GetGeometryStream() == this)
                        gfx.SetGeometryStream(null);

                    var glname = vao;
                    if (glname != 0)
                    {
                        Ultraviolet.QueueWorkItem((state) =>
                        {
                            GL.DeleteVertexArray(glname);
                            GL.ThrowIfError();

                            OpenGLState.DeleteVertexArrayObject(glname, glElementArrayBufferBinding ?? 0);
                        }, null, WorkItemOptions.ReturnNullOnSynchronousExecution);
                    }

                    vao = 0;
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
        /// <param name="normalize">A value indicating whether to normalize the attribute's values.</param>
        /// <returns>The converted vertex format.</returns>
        private static UInt32 GetVertexFormatGL(VertexElementFormat format, out Int32 size, out Int32 stride, out Boolean normalize)
        {
            switch (format)
            {
                case VertexElementFormat.Color:
                    size = 4;
                    stride = size * sizeof(Byte);
                    normalize = true;
                    return GL.GL_UNSIGNED_BYTE;

                case VertexElementFormat.SByte:
                    size = 1;
                    stride = size * sizeof(SByte);
                    normalize = false;
                    return GL.GL_BYTE;

                case VertexElementFormat.SByte2:
                    size = 2;
                    stride = size * sizeof(SByte);
                    normalize = false;
                    return GL.GL_BYTE;

                case VertexElementFormat.SByte3:
                    size = 3;
                    stride = size * sizeof(SByte);
                    normalize = false;
                    return GL.GL_BYTE;

                case VertexElementFormat.SByte4:
                    size = 4;
                    stride = size * sizeof(SByte);
                    normalize = false;
                    return GL.GL_BYTE;

                case VertexElementFormat.NormalizedSByte:
                    size = 1;
                    stride = size * sizeof(SByte);
                    normalize = true;
                    return GL.GL_BYTE;

                case VertexElementFormat.NormalizedSByte2:
                    size = 2;
                    stride = size * sizeof(SByte);
                    normalize = true;
                    return GL.GL_BYTE;

                case VertexElementFormat.NormalizedSByte3:
                    size = 3;
                    stride = size * sizeof(SByte);
                    normalize = true;
                    return GL.GL_BYTE;

                case VertexElementFormat.NormalizedSByte4:
                    size = 4;
                    stride = size * sizeof(SByte);
                    normalize = true;
                    return GL.GL_BYTE;

                case VertexElementFormat.Byte:
                    size = 1;
                    stride = size * sizeof(Byte);
                    normalize = false;
                    return GL.GL_UNSIGNED_BYTE;

                case VertexElementFormat.Byte2:
                    size = 2;
                    stride = size * sizeof(Byte);
                    normalize = false;
                    return GL.GL_UNSIGNED_BYTE;

                case VertexElementFormat.Byte3:
                    size = 3;
                    stride = size * sizeof(Byte);
                    normalize = false;
                    return GL.GL_UNSIGNED_BYTE;

                case VertexElementFormat.Byte4:
                    size = 4;
                    stride = size * sizeof(Byte);
                    normalize = false;
                    return GL.GL_UNSIGNED_BYTE;

                case VertexElementFormat.NormalizedByte:
                    size = 1;
                    stride = size * sizeof(Byte);
                    normalize = true;
                    return GL.GL_UNSIGNED_BYTE;

                case VertexElementFormat.NormalizedByte2:
                    size = 2;
                    stride = size * sizeof(Byte);
                    normalize = true;
                    return GL.GL_UNSIGNED_BYTE;

                case VertexElementFormat.NormalizedByte3:
                    size = 3;
                    stride = size * sizeof(Byte);
                    normalize = true;
                    return GL.GL_UNSIGNED_BYTE;

                case VertexElementFormat.NormalizedByte4:
                    size = 4;
                    stride = size * sizeof(Byte);
                    normalize = true;
                    return GL.GL_UNSIGNED_BYTE;

                case VertexElementFormat.Short:
                    size = 1;
                    stride = size * sizeof(Int16);
                    normalize = false;
                    return GL.GL_SHORT;

                case VertexElementFormat.Short2:
                    size = 2;
                    stride = size * sizeof(Int16);
                    normalize = false;
                    return GL.GL_SHORT;

                case VertexElementFormat.Short3:
                    size = 3;
                    stride = size * sizeof(Int16);
                    normalize = false;
                    return GL.GL_SHORT;

                case VertexElementFormat.Short4:
                    size = 4;
                    stride = size * sizeof(Int16);
                    normalize = false;
                    return GL.GL_SHORT;

                case VertexElementFormat.NormalizedShort:
                    size = 1;
                    stride = size * sizeof(Int16);
                    normalize = true;
                    return GL.GL_SHORT;

                case VertexElementFormat.NormalizedShort2:
                    size = 2;
                    stride = size * sizeof(Int16);
                    normalize = true;
                    return GL.GL_SHORT;

                case VertexElementFormat.NormalizedShort3:
                    size = 3;
                    stride = size * sizeof(Int16);
                    normalize = true;
                    return GL.GL_SHORT;

                case VertexElementFormat.NormalizedShort4:
                    size = 4;
                    stride = size * sizeof(Int16);
                    normalize = true;
                    return GL.GL_SHORT;

                case VertexElementFormat.UnsignedShort:
                    size = 1;
                    stride = size * sizeof(UInt16);
                    normalize = false;
                    return GL.GL_UNSIGNED_SHORT;

                case VertexElementFormat.UnsignedShort2:
                    size = 2;
                    stride = size * sizeof(UInt16);
                    normalize = false;
                    return GL.GL_UNSIGNED_SHORT;

                case VertexElementFormat.UnsignedShort3:
                    size = 3;
                    stride = size * sizeof(UInt16);
                    normalize = false;
                    return GL.GL_UNSIGNED_SHORT;

                case VertexElementFormat.UnsignedShort4:
                    size = 4;
                    stride = size * sizeof(UInt16);
                    normalize = false;
                    return GL.GL_UNSIGNED_SHORT;

                case VertexElementFormat.NormalizedUnsignedShort:
                    size = 1;
                    stride = size * sizeof(UInt16);
                    normalize = true;
                    return GL.GL_UNSIGNED_SHORT;

                case VertexElementFormat.NormalizedUnsignedShort2:
                    size = 2;
                    stride = size * sizeof(UInt16);
                    normalize = true;
                    return GL.GL_UNSIGNED_SHORT;

                case VertexElementFormat.NormalizedUnsignedShort3:
                    size = 3;
                    stride = size * sizeof(UInt16);
                    normalize = true;
                    return GL.GL_UNSIGNED_SHORT;

                case VertexElementFormat.NormalizedUnsignedShort4:
                    size = 4;
                    stride = size * sizeof(ushort);
                    normalize = true;
                    return GL.GL_UNSIGNED_SHORT;

                case VertexElementFormat.Int:
                    size = 1;
                    stride = size * sizeof(Int32);
                    normalize = false;
                    return GL.GL_INT;

                case VertexElementFormat.Int2:
                    size = 2;
                    stride = size * sizeof(Int32);
                    normalize = false;
                    return GL.GL_INT;

                case VertexElementFormat.Int3:
                    size = 3;
                    stride = size * sizeof(Int32);
                    normalize = false;
                    return GL.GL_INT;

                case VertexElementFormat.Int4:
                    size = 4;
                    stride = size * sizeof(Int32);
                    normalize = false;
                    return GL.GL_INT;

                case VertexElementFormat.NormalizedInt:
                    size = 1;
                    stride = size * sizeof(Int32);
                    normalize = true;
                    return GL.GL_INT;

                case VertexElementFormat.NormalizedInt2:
                    size = 2;
                    stride = size * sizeof(Int32);
                    normalize = true;
                    return GL.GL_INT;

                case VertexElementFormat.NormalizedInt3:
                    size = 3;
                    stride = size * sizeof(Int32);
                    normalize = true;
                    return GL.GL_INT;

                case VertexElementFormat.NormalizedInt4:
                    size = 4;
                    stride = size * sizeof(Int32);
                    normalize = true;
                    return GL.GL_INT;

                case VertexElementFormat.UnsignedInt:
                    size = 1;
                    stride = size * sizeof(UInt32);
                    normalize = false;
                    return GL.GL_UNSIGNED_INT;

                case VertexElementFormat.UnsignedInt2:
                    size = 2;
                    stride = size * sizeof(UInt32);
                    normalize = false;
                    return GL.GL_UNSIGNED_INT;

                case VertexElementFormat.UnsignedInt3:
                    size = 3;
                    stride = size * sizeof(UInt32);
                    normalize = false;
                    return GL.GL_UNSIGNED_INT;

                case VertexElementFormat.UnsignedInt4:
                    size = 4;
                    stride = size * sizeof(UInt32);
                    normalize = false;
                    return GL.GL_UNSIGNED_INT;

                case VertexElementFormat.NormalizedUnsignedInt:
                    size = 1;
                    stride = size * sizeof(UInt32);
                    normalize = true;
                    return GL.GL_UNSIGNED_INT;

                case VertexElementFormat.NormalizedUnsignedInt2:
                    size = 2;
                    stride = size * sizeof(UInt32);
                    normalize = true;
                    return GL.GL_UNSIGNED_INT;

                case VertexElementFormat.NormalizedUnsignedInt3:
                    size = 3;
                    stride = size * sizeof(UInt32);
                    normalize = true;
                    return GL.GL_UNSIGNED_INT;

                case VertexElementFormat.NormalizedUnsignedInt4:
                    size = 4;
                    stride = size * sizeof(UInt32);
                    normalize = true;
                    return GL.GL_UNSIGNED_INT;

                case VertexElementFormat.Single:
                    size = 1;
                    stride = size * sizeof(Single);
                    normalize = false;
                    return GL.GL_FLOAT;

                case VertexElementFormat.Vector2:
                    size = 2;
                    stride = size * sizeof(Single);
                    normalize = false;
                    return GL.GL_FLOAT;

                case VertexElementFormat.Vector3:
                    size = 3;
                    stride = size * sizeof(Single);
                    normalize = false;
                    return GL.GL_FLOAT;

                case VertexElementFormat.Vector4:
                    size = 4;
                    stride = size * sizeof(Single);
                    normalize = false;
                    return GL.GL_FLOAT;

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
        private static String GetVertexAttributeNameFromUsage(VertexElementUsage usage, Int32 index)
        {
            return vertexAttributeNames[((int)usage * VertexElement.UsageIndexCount) + index];
        }

        /// <summary>
        /// Attempts to associate this geometry stream with the specified OpenGL program.
        /// </summary>
        /// <param name="program">The OpenGL name of the program with which to associate this stream.</param>
        /// <returns><see langword="true"/> if the stream's cached program was changed; otherwise, <see langword="false"/>.</returns>
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
        private unsafe void DisableVertexAttributesOnCachedProgram()
        {
            var attributes = 0;

            GL.GetProgramiv(program, GL.GL_ACTIVE_ATTRIBUTES, &attributes);
            GL.ThrowIfError();

            for (var i = 0u; i < attributes; i++)
            {
                GL.DisableVertexArrayAttrib(vao, i);
                GL.ThrowIfError();
            }
        }

        /// <summary>
        /// Binds the geometry stream's buffers to the device in preparation for rendering.
        /// </summary>
        private void BindBuffers(UInt32? program, UInt32? offset)
        {
            unsafe
            {
                var previousBuffer = (uint)OpenGLState.GL_ARRAY_BUFFER_BINDING;

                if (IsUsingVertexArrayObject)
                {
                    using (OpenGLState.ScopedBindVertexArrayObject(vao, glElementArrayBufferBinding ?? 0, force: !GL.IsVertexAttribBindingAvailable))
                    {
                        if (program.HasValue)
                            DisableVertexAttributesOnCachedProgram();

                        if (GL.IsVertexAttribBindingAvailable)
                        {
                            for (int i = 0; i < vbuffers.Count; i++)
                            {
                                var binding = vbuffers[i];
                                BindVertexAttributesForBuffer_NewAPI(binding.VertexBuffer,
                                    (UInt32)i, (UInt32)binding.InstanceFrequency, program, offset);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < vbuffers.Count; i++)
                            {
                                var binding = vbuffers[i];
                                BindVertexAttributesForBuffer_OldAPI(binding.VertexBuffer,
                                    (UInt32)i, (UInt32)binding.InstanceFrequency, program, offset);
                            }
                        }
                    }
                }
                else
                {
                    if (program.HasValue)
                        DisableVertexAttributesOnCachedProgram();

                    for (int i = 0; i < vbuffers.Count; i++)
                    {
                        var binding = vbuffers[i];
                        BindVertexAttributesForBuffer_OldAPI(binding.VertexBuffer,
                            (UInt32)i, (UInt32)binding.InstanceFrequency, program, offset);
                    }

                    OpenGLState.BindArrayBuffer(previousBuffer);
                }
            }
        }

        /// <summary>
        /// Binds the specified buffer's vertex attributes to the currently cached program using the old API.
        /// </summary>
        private unsafe void BindVertexAttributesForBuffer_OldAPI(OpenGLVertexBuffer vbuffer, UInt32 binding, UInt32 frequency, UInt32? program, UInt32? offset)
        {
            OpenGLState.BindArrayBuffer(vbuffer.OpenGLName);

            var position = offset ?? this.offset;
            foreach (var element in vbuffer.VertexDeclaration)
            {
                var name = element.Name ?? GetVertexAttributeNameFromUsage(element.Usage, element.Index);
                var size = 0;
                var stride = 0;
                var normalize = false;
                var type = GetVertexFormatGL(element.Format, out size, out stride, out normalize);

                var category = OpenGLAttribCategory.Single;
                var location = OpenGLState.CurrentProgram.GetAttribLocation(name, out category);
                if (location >= 0)
                {
                    var unsignedLocation = (UInt32)location;

                    if (program.HasValue)
                    {
                        if (GL.IsInstancedRenderingAvailable)
                        {
                            GL.VertexAttribDivisor(unsignedLocation, frequency);
                            GL.ThrowIfError();
                        }
                        else
                        {
                            if (frequency != 0)
                                throw new NotSupportedException(OpenGLStrings.InstancedRenderingNotSupported);
                        }

                        GL.EnableVertexAttribArray(unsignedLocation);
                        GL.ThrowIfError();
                    }

                    switch (category)
                    {
                        case OpenGLAttribCategory.Single:
                            {
                                GL.VertexAttribPointer(unsignedLocation, size, type, normalize, vbuffer.VertexDeclaration.VertexStride, (void*)(position));
                                GL.ThrowIfError();
                            }
                            break;

                        case OpenGLAttribCategory.Double:
                            {
                                if (!GL.IsDoublePrecisionVertexAttribAvailable)
                                    throw new NotSupportedException(OpenGLStrings.DoublePrecisionVAttribsNotSupported);

                                GL.VertexAttribLPointer(unsignedLocation, size, type, vbuffer.VertexDeclaration.VertexStride, (void*)(position));
                                GL.ThrowIfError();
                            }
                            break;

                        case OpenGLAttribCategory.Integer:
                            {
                                if (!GL.IsIntegerVertexAttribAvailable)
                                    throw new NotSupportedException(OpenGLStrings.IntegerVAttribsNotSupported);

                                GL.VertexAttribIPointer(unsignedLocation, size, type, vbuffer.VertexDeclaration.VertexStride, (void*)(position));
                                GL.ThrowIfError();
                            }
                            break;
                    }
                }

                position += (uint)stride;
            }
        }

        /// <summary>
        /// Binds the specified buffer's vertex attributes to the currently cached program using the new API.
        /// </summary>
        private unsafe void BindVertexAttributesForBuffer_NewAPI(OpenGLVertexBuffer vbuffer, UInt32 binding, UInt32 frequency, UInt32? program, UInt32? offset)
        {
            using (OpenGLState.ScopedBindVertexArrayObject(vao, glElementArrayBufferBinding ?? 0))
            {
                if (program.HasValue || offset.HasValue)
                {
                    GL.VertexArrayVertexBuffer(vao, binding, vbuffer.OpenGLName, (IntPtr)(offset ?? 0), vbuffer.VertexDeclaration.VertexStride);
                    GL.ThrowIfError();
                }

                if (program.HasValue)
                {
                    GL.VertexArrayBindingDivisor(vao, binding, frequency);
                    GL.ThrowIfError();

                    var position = 0u;
                    foreach (var element in vbuffer.VertexDeclaration)
                    {
                        var name = element.Name ?? GetVertexAttributeNameFromUsage(element.Usage, element.Index);
                        var size = 0;
                        var stride = 0;
                        var normalize = false;
                        var type = GetVertexFormatGL(element.Format, out size, out stride, out normalize);

                        var category = OpenGLAttribCategory.Single;
                        var location = OpenGLState.CurrentProgram.GetAttribLocation(name, out category);
                        if (location >= 0)
                        {
                            var unsignedLocation = (UInt32)location;

                            GL.VertexArrayAttribBinding(vao, unsignedLocation, binding);
                            GL.ThrowIfError();

                            GL.EnableVertexArrayAttrib(vao, unsignedLocation);
                            GL.ThrowIfError();

                            unsafe
                            {
                                switch (category)
                                {
                                    case OpenGLAttribCategory.Single:
                                        {
                                            GL.VertexArrayAttribFormat(vao, unsignedLocation, size, type, normalize, position);
                                            GL.ThrowIfError();
                                        }
                                        break;

                                    case OpenGLAttribCategory.Double:
                                        {
                                            if (!GL.IsDoublePrecisionVertexAttribAvailable)
                                                throw new NotSupportedException(OpenGLStrings.DoublePrecisionVAttribsNotSupported);

                                            GL.VertexArrayAttribLFormat(vao, unsignedLocation, size, type, position);
                                            GL.ThrowIfError();
                                        }
                                        break;

                                    case OpenGLAttribCategory.Integer:
                                        {
                                            if (!GL.IsIntegerVertexAttribAvailable)
                                                throw new NotSupportedException(OpenGLStrings.IntegerVAttribsNotSupported);

                                            GL.VertexArrayAttribIFormat(vao, unsignedLocation, size, type, position);
                                            GL.ThrowIfError();
                                        }
                                        break;
                                }
                            }
                        }

                        position += (uint)stride;
                    }
                }
            }
        }

        /// <summary>
        /// Attaches the specified vertex buffer to the geometry stream.
        /// </summary>
        private void AttachInternal(VertexBuffer vbuffer, Int32 instanceFrequency)
        {
            if (vbuffers == null)
                vbuffers = new List<VertexBufferBinding>(1);

            var oglVertexBuffer = (OpenGLVertexBuffer)vbuffer;
            var binding = new VertexBufferBinding(oglVertexBuffer, instanceFrequency);
            vbuffers.Add(binding);

            var vdecl = vbuffer.VertexDeclaration;

            if (vdecl.HasPosition)
                this.hasVertexPosition = true;

            if (vdecl.HasColor)
                this.hasVertexColor = true;

            if (vdecl.HasTexture)
                this.hasVertexTexture = true;

            if (vdecl.HasNormal)
                this.hasVertexNormal = true;

            if (vdecl.HasTangent)
                this.hasVertexTangent = true;

            if (vdecl.HasBlend)
                this.hasVertexBlend = true;
        }

        // The names of vertex attributes which correspond to Ultraviolet vertex usages.
        private static readonly String[] vertexAttributeNames;

        // Underlying vertex and index buffers.
        private List<VertexBufferBinding> vbuffers;
        private OpenGLIndexBuffer ibuffer;
        private IndexBufferElementType indexBufferElementType = IndexBufferElementType.Int16;

        // The Vertex Array Object (VAO) that this buffer represents.
        private UInt32 vao;
        private UInt32 program;
        private UInt32 offset;

        // Vertex attributes
        private Boolean hasVertexPosition;
        private Boolean hasVertexColor;
        private Boolean hasVertexTexture;
        private Boolean hasVertexNormal;
        private Boolean hasVertexTangent;
        private Boolean hasVertexBlend;

        // GL state values
        private UInt32? glElementArrayBufferBinding;
    }
}
