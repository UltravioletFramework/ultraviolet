using System;

namespace Ultraviolet.OpenGL.Bindings
{
    partial class GL
    {
        /// <summary>
        /// Implements DSA functions for OpenGL contexts which do not support DSA. Unlike true DSA implementations,
        /// this class will modify the global binding state of the OpenGL context.
        /// </summary>
        internal sealed unsafe class DirectStateAccessNullImpl : DirectStateAccessImpl
        {
            public override void NamedBufferData(uint buffer, uint target, IntPtr size, void* data, uint usage)
            {
                glBufferData(target, size, (IntPtr)data, usage);
            }

            public override void NamedBufferSubData(uint buffer, uint target, IntPtr offset, IntPtr size, void* data)
            {
                glBufferSubData(target, offset, size, (IntPtr)data);
            }

            public override void NamedFramebufferTexture(uint framebuffer, uint target, uint attachment, uint texture, int level)
            {
                glFramebufferTexture(target, attachment, texture, level);
            }

            public override void NamedFramebufferRenderbuffer(uint framebuffer, uint target, uint attachment, uint renderbuffertarget, uint renderbuffer)
            {
                glFramebufferRenderbuffer(target, attachment, renderbuffertarget, renderbuffer);
            }

            public override void NamedFramebufferDrawBuffer(uint framebuffer, uint mode)
            {
                glDrawBuffer(mode);
            }

            public override unsafe void NamedFramebufferDrawBuffers(uint framebuffer, int n, uint* bufs)
            {
                glDrawBuffers(n, (IntPtr)bufs);
            }

            public override uint CheckNamedFramebufferStatus(uint framebuffer, uint target)
            {
                return glCheckFramebufferStatus(target);
            }

            public override void TextureParameteri(uint texture, uint target, uint pname, int param)
            {
                glTexParameteri(target, pname, param);
            }

            public override void TextureImage2D(uint texture, uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, void* pixels)
            {
                glTexImage2D(target, level, internalformat, width, height, border, format, type, (IntPtr)pixels);
            }

            public override void TextureImage3D(uint texture, uint target, int level, int internalformat, int width, int height, int depth, int border, uint format, uint type, void* pixels)
            {
                glTexImage3D(target, level, internalformat, width, height, depth, border, format, type, (IntPtr)pixels);
            }

            public override void TextureSubImage2D(uint texture, uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, void* pixels)
            {
                glTexSubImage2D(target, level, xoffset, yoffset, width, height, format, type, (IntPtr)pixels);
            }

            public override void TextureSubImage3D(uint texture, uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, void* pixels)
            {
                glTexSubImage3D(target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, (IntPtr)pixels);
            }

            public override void TextureStorage1D(uint texture, uint target, int levels, uint internalformat, int width)
            {
                glTexStorage1D(target, levels, internalformat, width);
            }

            public override void TextureStorage2D(uint texture, uint target, int levels, uint internalformat, int width, int height)
            {
                glTexStorage2D(target, levels, internalformat, width, height);
            }

            public override void TextureStorage3D(uint texture, uint target, int levels, uint internalformat, int width, int height, int depth)
            {
                glTexStorage3D(target, levels, internalformat, width, height, depth);
            }

            public override void* MapNamedBuffer(uint buffer, uint target, uint access)
            {
                return glMapBuffer(target, access).ToPointer();
            }

            public override void* MapNamedBufferRange(uint buffer, uint target, IntPtr offset, IntPtr length, uint access)
            {
                return glMapBufferRange(target, offset, length, access).ToPointer();
            }

            public override bool UnmapNamedBuffer(uint buffer, uint target)
            {
                return glUnmapBuffer(target);
            }

            public override void FlushMappedNamedBufferRange(uint buffer, uint target, IntPtr offset, IntPtr length)
            {
                glFlushMappedBufferRange(target, offset, length);
            }

            public override void GetNamedBufferParameteriv(uint buffer, uint target, uint pname, int* @params)
            {
                glGetBufferParameteriv(target, pname, (IntPtr)@params);
            }

            public override void GetNamedBufferPointerv(uint buffer, uint target, uint pname, void** @params)
            {
                glGetBufferPointerv(target, pname, (IntPtr)@params);
            }

            public override void GetNamedBufferSubData(uint buffer, uint target, IntPtr offset, IntPtr size, void* data)
            {
                glGetBufferSubData(target, offset, size, (IntPtr)data);
            }

            public override void VertexArrayVertexBuffer(uint vaobj, uint bindingindex, uint buffer, IntPtr offset, int stride)
            {
                glBindVertexBuffer(bindingindex, buffer, offset, stride);
            }

            public override void VertexArrayAttribFormat(uint vaobj, uint attribindex, int size, uint type, bool normalized, uint relativesize)
            {
                glVertexAttribFormat(attribindex, size, type, normalized, relativesize);
            }

            public override void VertexArrayAttribIFormat(uint vaobj, uint attribindex, int size, uint type, uint relativesize)
            {
                glVertexAttribIFormat(attribindex, size, type, relativesize);
            }

            public override void VertexArrayAttribLFormat(uint vaobj, uint attribindex, int size, uint type, uint relativesize)
            {
                glVertexAttribLFormat(attribindex, size, type, relativesize);
            }

            public override void VertexArrayAttribBinding(uint vaobj, uint attribindex, uint bindingindex)
            {
                glVertexAttribBinding(attribindex, bindingindex);
            }

            public override void VertexArrayBindingDivisor(uint vaobj, uint bindingindex, uint divisor)
            {
                glVertexBindingDivisor(bindingindex, divisor);
            }

            public override void EnableVertexArrayAttrib(uint vaobj, uint index)
            {
                glEnableVertexAttribArray(index);
            }

            public override void DisableVertexArrayAttrib(uint vaobj, uint index)
            {
                glDisableVertexAttribArray(index);
            }

            public override void InvalidateNamedFramebufferData(uint target, uint framebuffer, int numAttachments, IntPtr attachments)
            {
                glInvalidateFramebuffer(target, numAttachments, attachments);
            }
        }
    }
}
