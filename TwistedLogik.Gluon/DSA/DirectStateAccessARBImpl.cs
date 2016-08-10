using System;

namespace TwistedLogik.Gluon
{
    partial class gl
    {
        /// <summary>
        /// Implements DSA functions for OpenGL contexts with version 4.5 or greater.
        /// </summary>
        internal sealed unsafe class DirectStateAccessARBImpl : DirectStateAccessImpl
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
                glTextureParameteri(texture, pname, param);
            }

            public override void TextureImage2D(uint texture, uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, void* pixels)
            {
                // NOTE: Should be using glTextureStorage2D() instead
                throw new NotSupportedException();
            }

            public override void TextureSubImage2D(uint texture, uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, void* pixels)
            {
                glTextureSubImage2D(texture, level, xoffset, yoffset, width, height, format, type, (IntPtr)pixels);
            }

            public override void TextureStorage1D(uint texture, uint target, int levels, uint internalformat, int width)
            {
                glTextureStorage1D(texture, levels, internalformat, width);
            }

            public override void TextureStorage2D(uint texture, uint target, int levels, uint internalformat, int width, int height)
            {
                glTextureStorage2D(texture, levels, internalformat, width, height);
            }

            public override void TextureStorage3D(uint texture, uint target, int levels, uint internalformat, int width, int height, int depth)
            {
                glTextureStorage3D(texture, levels, internalformat, width, height, depth);
            }

            public override void* MapNamedBuffer(uint buffer, uint target, uint access)
            {
                return glMapNamedBuffer(buffer, access).ToPointer();
            }

            public override void* MapNamedBufferRange(uint buffer, uint target, int* offset, uint* length, uint access)
            {
                return glMapNamedBufferRange(buffer, (IntPtr)offset, (IntPtr)length, access).ToPointer();
            }

            public override bool UnmapNamedBuffer(uint buffer, uint target)
            {
                return glUnmapNamedBuffer(buffer);
            }

            public override void FlushMappedNamedBufferRange(uint buffer, uint target, int* offset, uint* length)
            {
                glFlushMappedNamedBufferRange(buffer, (IntPtr)offset, (IntPtr)length);
            }

            public override void GetNamedBufferParameteriv(uint buffer, uint target, uint pname, int* @params)
            {
                glGetNamedBufferParameteriv(buffer, pname, (IntPtr)@params);
            }

            public override void GetNamedBufferPointerv(uint buffer, uint target, uint pname, void** @params)
            {
                glGetNamedBufferPointerv(buffer, pname, (IntPtr)@params);
            }

            public override void GetNamedBufferSubData(uint buffer, uint target, int* offset, uint* size, void* data)
            {
                glGetNamedBufferSubData(buffer, (IntPtr)offset, (IntPtr)size, (IntPtr)data);
            }
        }
    }
}
