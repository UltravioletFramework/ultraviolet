using System;

namespace Ultraviolet.OpenGL.Bindings
{
    partial class GL
    {
        /// <summary>
        /// Implements DSA functions for OpenGL contexts which support the GL_EXT_direct_state_access extension.
        /// </summary>
        internal sealed unsafe class DirectStateAccessEXTImpl : DirectStateAccessImpl
        {
            public override void NamedBufferData(uint buffer, uint target, IntPtr size, void* data, uint usage)
            {
                glNamedBufferData(buffer, size, (IntPtr)data, usage);
            }

            public override void NamedBufferSubData(uint buffer, uint target, IntPtr offset, IntPtr size, void* data)
            {
                glNamedBufferSubData(buffer, offset, size, (IntPtr)data);
            }

            public override void NamedFramebufferTexture(uint framebuffer, uint target, uint attachment, uint texture, int level)
            {
                glNamedFramebufferTexture(framebuffer, attachment, texture, level);
            }

            public override void NamedFramebufferRenderbuffer(uint framebuffer, uint target, uint attachment, uint renderbuffertarget, uint renderbuffer)
            {
                glNamedFramebufferRenderbuffer(framebuffer, attachment, renderbuffertarget, renderbuffer);
            }

            public override void NamedFramebufferDrawBuffer(uint framebuffer, uint mode)
            {
                glNamedFramebufferDrawBuffer(framebuffer, mode);
            }

            public override unsafe void NamedFramebufferDrawBuffers(uint framebuffer, int n, uint* bufs)
            {
                glNamedFramebufferDrawBuffers(framebuffer, n, (IntPtr)bufs);
            }

            public override uint CheckNamedFramebufferStatus(uint framebuffer, uint target)
            {
                return glCheckNamedFramebufferStatus(framebuffer, target);
            }

            public override void TextureParameteri(uint texture, uint target, uint pname, int param)
            {
                glTextureParameteriEXT(texture, target, pname, param);
            }

            public override void TextureImage2D(uint texture, uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, void* pixels)
            {
                glTextureImage2DEXT(texture, target, level, internalformat, width, height, border, format, type, (IntPtr)pixels);
            }

            public override void TextureImage3D(uint texture, uint target, int level, int internalformat, int width, int height, int depth, int border, uint format, uint type, void* pixels)
            {
                glTextureImage3DEXT(texture, target, level, internalformat, width, height, depth, border, format, type, (IntPtr)pixels);
            }

            public override void TextureSubImage2D(uint texture, uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, void* pixels)
            {
                glTextureSubImage2DEXT(texture, target, level, xoffset, yoffset, width, height, format, type, (IntPtr)pixels);
            }

            public override void TextureSubImage3D(uint texture, uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, void* pixels)
            {
                glTextureSubImage3DEXT(texture, target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, (IntPtr)pixels);
            }

            public override void TextureStorage1D(uint texture, uint target, int levels, uint internalformat, int width)
            {
                glTextureStorage1DEXT(texture, target, levels, internalformat, width);
            }

            public override void TextureStorage2D(uint texture, uint target, int levels, uint internalformat, int width, int height)
            {
                glTextureStorage2DEXT(texture, target, levels, internalformat, width, height);
            }

            public override void TextureStorage3D(uint texture, uint target, int levels, uint internalformat, int width, int height, int depth)
            {
                glTextureStorage3DEXT(texture, target, levels, internalformat, width, height, depth);
            }

            public override void* MapNamedBuffer(uint buffer, uint target, uint access)
            {
                return glMapNamedBuffer(buffer, access).ToPointer();
            }

            public override void* MapNamedBufferRange(uint buffer, uint target, IntPtr offset, IntPtr length, uint access)
            {
                return glMapNamedBufferRange(buffer, offset, length, access).ToPointer();
            }

            public override bool UnmapNamedBuffer(uint buffer, uint target)
            {
                return glUnmapNamedBuffer(buffer);
            }

            public override void FlushMappedNamedBufferRange(uint buffer, uint target, IntPtr offset, IntPtr length)
            {
                glFlushMappedNamedBufferRange(buffer, offset, length);
            }

            public override void GetNamedBufferParameteriv(uint buffer, uint target, uint pname, int* @params)
            {
                glGetNamedBufferParameteriv(buffer, pname, (IntPtr)@params);
            }

            public override void GetNamedBufferPointerv(uint buffer, uint target, uint pname, void** @params)
            {
                glGetNamedBufferPointerv(buffer, pname, (IntPtr)@params);
            }

            public override void GetNamedBufferSubData(uint buffer, uint target, IntPtr offset, IntPtr size, void* data)
            {
                glGetNamedBufferSubData(buffer, offset, size, (IntPtr)data);
            }

            public override void VertexArrayVertexBuffer(uint vaobj, uint bindingindex, uint buffer, IntPtr offset, int stride)
            {
                glVertexArrayVertexBuffer(vaobj, bindingindex, buffer, offset, stride);
            }

            public override void VertexArrayAttribFormat(uint vaobj, uint attribindex, int size, uint type, bool normalized, uint relativesize)
            {
                glVertexArrayAttribFormat(vaobj, attribindex, size, type, normalized, relativesize);
            }

            public override void VertexArrayAttribIFormat(uint vaobj, uint attribindex, int size, uint type, uint relativesize)
            {
                glVertexArrayAttribIFormat(vaobj, attribindex, size, type, relativesize);
            }

            public override void VertexArrayAttribLFormat(uint vaobj, uint attribindex, int size, uint type, uint relativesize)
            {
                glVertexArrayAttribLFormat(vaobj, attribindex, size, type, relativesize);
            }

            public override void VertexArrayAttribBinding(uint vaobj, uint attribindex, uint bindingindex)
            {
                glVertexArrayAttribBinding(vaobj, attribindex, bindingindex);
            }

            public override void VertexArrayBindingDivisor(uint vaobj, uint bindingindex, uint divisor)
            {
                glVertexArrayBindingDivisor(vaobj, bindingindex, divisor);
            }

            public override void EnableVertexArrayAttrib(uint vaobj, uint index)
            {
                glEnableVertexArrayAttrib(vaobj, index);
            }

            public override void DisableVertexArrayAttrib(uint vaobj, uint index)
            {
                glDisableVertexArrayAttrib(vaobj, index);
            }

            public override void InvalidateNamedFramebufferData(uint target, uint framebuffer, int numAttachments, IntPtr attachments)
            {
                throw new NotSupportedException(BindingsStrings.FunctionNotProvidedByDriver.Format("glInvalidateNamedFramebuffer"));
            }
        }
    }
}
