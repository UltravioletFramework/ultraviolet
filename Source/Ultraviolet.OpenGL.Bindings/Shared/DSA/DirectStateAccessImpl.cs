using System;

namespace Ultraviolet.OpenGL.Bindings
{
    partial class GL
    {
        /// <summary>
        /// Represents a class which exposes functions that are compatible with DSA (direct state access).
        /// Depending on whether (and how) DSA is supported on the current context, different implementations of
        /// this class can make different OpenGL calls to perform the requested operation.
        /// </summary>
        internal abstract unsafe class DirectStateAccessImpl
        {
            public abstract void NamedBufferData(uint buffer, uint target, IntPtr size, void* data, uint usage);

            public abstract void NamedBufferSubData(uint buffer, uint target, IntPtr offset, IntPtr size, void* data);

            public abstract void NamedFramebufferTexture(uint framebuffer, uint target, uint attachment, uint texture, int level);

            public abstract void NamedFramebufferRenderbuffer(uint framebuffer, uint target, uint attachment, uint renderbuffertarget, uint renderbuffer);

            public abstract void NamedFramebufferDrawBuffer(uint framebuffer, uint mode);

            public abstract void NamedFramebufferDrawBuffers(uint framebuffer, int n, uint* bufs);

            public abstract uint CheckNamedFramebufferStatus(uint framebuffer, uint target);

            public abstract void TextureParameteri(uint texture, uint target, uint pname, int param);

            public abstract void TextureImage2D(uint texture, uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, void* pixels);

            public abstract void TextureImage3D(uint texture, uint target, int level, int internalformat, int width, int height, int depth, int border, uint format, uint type, void* pixels);

            public abstract void TextureSubImage2D(uint texture, uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, void* pixels);

            public abstract void TextureSubImage3D(uint texture, uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, void* pixels);

            public abstract void TextureStorage1D(uint texture, uint target, int levels, uint internalformat, int width);

            public abstract void TextureStorage2D(uint texture, uint target, int levels, uint internalformat, int width, int height);

            public abstract void TextureStorage3D(uint texture, uint target, int levels, uint internalformat, int width, int height, int depth);

            public abstract void* MapNamedBuffer(uint buffer, uint target, uint access);

            public abstract void* MapNamedBufferRange(uint buffer, uint target, IntPtr offset, IntPtr length, uint access);

            public abstract bool UnmapNamedBuffer(uint buffer, uint target);

            public abstract void FlushMappedNamedBufferRange(uint buffer, uint target, IntPtr offset, IntPtr length);

            public abstract void GetNamedBufferParameteriv(uint buffer, uint target, uint pname, int* @params);

            public abstract void GetNamedBufferPointerv(uint buffer, uint target, uint pname, void** @params);

            public abstract void GetNamedBufferSubData(uint buffer, uint target, IntPtr offset, IntPtr size, void* data);

            public abstract void VertexArrayVertexBuffer(uint vaobj, uint bindingindex, uint buffer, IntPtr offset, int stride);

            public abstract void VertexArrayAttribFormat(uint vaobj, uint attribindex, int size, uint type, bool normalized, uint relativesize);

            public abstract void VertexArrayAttribIFormat(uint vaobj, uint attribindex, int size, uint type, uint relativesize);

            public abstract void VertexArrayAttribLFormat(uint vaobj, uint attribindex, int size, uint type, uint relativesize);

            public abstract void VertexArrayAttribBinding(uint vaobj, uint attribindex, uint bindingindex);

            public abstract void VertexArrayBindingDivisor(uint vaobj, uint bindingindex, uint divisor);

            public abstract void EnableVertexArrayAttrib(uint vaobj, uint index);

            public abstract void DisableVertexArrayAttrib(uint vaobj, uint index);

            public abstract void InvalidateNamedFramebufferData(uint target, uint framebuffer, int numAttachments, IntPtr attachments);
        }
    }
}
