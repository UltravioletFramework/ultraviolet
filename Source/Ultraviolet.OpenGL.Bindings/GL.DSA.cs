using System;

namespace Ultraviolet.OpenGL.Bindings
{
    public static unsafe partial class GL
    {
        private static void InitializeDSA()
        {
            if (IsVersionAtLeast(4, 5))
            {
                IsARBDirectStateAccessAvailable = true;
                dsaimpl = new DirectStateAccessARBImpl();
                return;
            }
            if (IsExtensionSupported("GL_EXT_direct_state_access"))
            {
                IsEXTDirectStateAccessAvailable = true;
                dsaimpl = new DirectStateAccessEXTImpl();
                return;
            }
            dsaimpl = new DirectStateAccessNullImpl();
        }

        public static void NamedBufferData(uint buffer, uint target, IntPtr size, void* data, uint usage)
        {
            dsaimpl.NamedBufferData(buffer, target, size, data, usage);
        }

        public static void NamedBufferSubData(uint buffer, uint target, IntPtr offset, IntPtr size, void* data)
        {
            dsaimpl.NamedBufferSubData(buffer, target, offset, size, data);
        }

        public static void NamedFramebufferTexture(uint framebuffer, uint target, uint attachment, uint texture, int level)
        {
            dsaimpl.NamedFramebufferTexture(framebuffer, target, attachment, texture, level);
        }

        public static void NamedFramebufferRenderbuffer(uint framebuffer, uint target, uint attachment, uint renderbuffertarget, uint renderbuffer)
        {
            dsaimpl.NamedFramebufferRenderbuffer(framebuffer, target, attachment, renderbuffertarget, renderbuffer);
        }

        public static void NamedFramebufferDrawBuffer(uint framebuffer, uint mode)
        {
            dsaimpl.NamedFramebufferDrawBuffer(framebuffer, mode);
        }

        public static void NamedFramebufferDrawBuffers(uint framebuffer, int n, uint* bufs)
        {
            dsaimpl.NamedFramebufferDrawBuffers(framebuffer, n, bufs);
        }

        public static uint CheckNamedFramebufferStatus(uint framebuffer, uint target)
        {
            return dsaimpl.CheckNamedFramebufferStatus(framebuffer, target);
        }

        public static void TextureParameteri(uint texture, uint target, uint pname, int param)
        {
            dsaimpl.TextureParameteri(texture, target, pname, param);
        }

        public static void TextureImage2D(uint texture, uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, void* pixels)
        {
            dsaimpl.TextureImage2D(texture, target, level, internalformat, width, height, border, format, type, pixels);
        }

        public static void TextureImage3D(uint texture, uint target, int level, int internalformat, int width, int height, int depth, int border, uint format, uint type, void* pixels)
        {
            dsaimpl.TextureImage3D(texture, target, level, internalformat, width, height, depth, border, format, type, pixels);
        }

        public static void TextureSubImage2D(uint texture, uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, void* pixels)
        {
            dsaimpl.TextureSubImage2D(texture, target, level, xoffset, yoffset, width, height, format, type, pixels);
        }

        public static void TextureSubImage3D(uint texture, uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, void* pixels)
        {
            dsaimpl.TextureSubImage3D(texture, target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels);
        }

        public static void TextureStorage1D(uint texture, uint target, int levels, uint internalformat, int width)
        {
            dsaimpl.TextureStorage1D(texture, target, levels, internalformat, width);
        }

        public static void TextureStorage2D(uint texture, uint target, int levels, uint internalformat, int width, int height)
        {
            dsaimpl.TextureStorage2D(texture, target, levels, internalformat, width, height);
        }

        public static void TextureStorage3D(uint texture, uint target, int levels, uint internalformat, int width, int height, int depth)
        {
            dsaimpl.TextureStorage3D(texture, target, levels, internalformat, width, height, depth);
        }

        public static void* MapNamedBuffer(uint buffer, uint target, uint access)
        {
            return dsaimpl.MapNamedBuffer(buffer, target, access);
        }

        public static void* MapNamedBufferRange(uint buffer, uint target, IntPtr offset, IntPtr length, uint access)
        {
            return dsaimpl.MapNamedBufferRange(buffer, target, offset, length, access);
        }

        public static bool UnmapNamedBuffer(uint buffer, uint target)
        {
            return dsaimpl.UnmapNamedBuffer(buffer, target);
        }

        public static void FlushMappedNamedBufferRange(uint buffer, uint target, IntPtr offset, IntPtr length)
        {
            dsaimpl.FlushMappedNamedBufferRange(buffer, target, offset, length);
        }

        public static void GetNamedBufferParameteriv(uint buffer, uint target, uint pname, int* @params)
        {
            dsaimpl.GetNamedBufferParameteriv(buffer, target, pname, @params);
        }

        public static void GetNamedBufferPointerv(uint buffer, uint target, uint pname, void** @params)
        {
            dsaimpl.GetNamedBufferPointerv(buffer, target, pname, @params);
        }

        public static void GetNamedBufferSubData(uint buffer, uint target, IntPtr offset, IntPtr size, void* data)
        {
            dsaimpl.GetNamedBufferSubData(buffer, target, offset, size, data);
        }

        public static void VertexArrayVertexBuffer(uint vaobj, uint bindingindex, uint buffer, IntPtr offset, int stride)
        {
            dsaimpl.VertexArrayVertexBuffer(vaobj, bindingindex, buffer, offset, stride);
        }

        public static void VertexArrayAttribFormat(uint vaobj, uint attribindex, int size, uint type, bool normalized, uint relativesize)
        {
            dsaimpl.VertexArrayAttribFormat(vaobj, attribindex, size, type, normalized, relativesize);
        }

        public static void VertexArrayAttribIFormat(uint vaobj, uint attribindex, int size, uint type, uint relativesize)
        {
            dsaimpl.VertexArrayAttribIFormat(vaobj, attribindex, size, type, relativesize);
        }

        public static void VertexArrayAttribLFormat(uint vaobj, uint attribindex, int size, uint type, uint relativesize)
        {
            dsaimpl.VertexArrayAttribLFormat(vaobj, attribindex, size, type, relativesize);
        }

        public static void VertexArrayAttribBinding(uint vaobj, uint attribindex, uint bindingindex)
        {
            dsaimpl.VertexArrayAttribBinding(vaobj, attribindex, bindingindex);
        }

        public static void VertexArrayBindingDivisor(uint vaobj, uint bindingindex, uint divisor)
        {
            dsaimpl.VertexArrayBindingDivisor(vaobj, bindingindex, divisor);
        }

        public static void EnableVertexArrayAttrib(uint vaobj, uint index)
        {
            dsaimpl.EnableVertexArrayAttrib(vaobj, index);
        }

        public static void DisableVertexArrayAttrib(uint vaobj, uint index)
        {
            dsaimpl.DisableVertexArrayAttrib(vaobj, index);
        }

        public static void InvalidateNamedFramebufferData(uint target, uint framebuffer, int numAttachments, IntPtr attachments)
        {
            dsaimpl.InvalidateNamedFramebufferData(target, framebuffer, numAttachments, attachments);
        }

        public static Boolean IsEXTDirectStateAccessAvailable
        {
            get;
            private set;
        }

        public static Boolean IsARBDirectStateAccessAvailable
        {
            get;
            private set;
        }

        public static Boolean IsDirectStateAccessAvailable
        {
            get { return IsEXTDirectStateAccessAvailable || IsARBDirectStateAccessAvailable; }
        }

        private static DirectStateAccessImpl dsaimpl;
    }
}
