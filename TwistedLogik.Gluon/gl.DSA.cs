using System;

namespace TwistedLogik.Gluon
{
    public static unsafe partial class gl
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

        public static void TextureSubImage2D(uint texture, uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, void* pixels)
        {
            dsaimpl.TextureSubImage2D(texture, target, level, xoffset, yoffset, width, height, format, type, pixels);
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

        public static Boolean IsTextureStorageAvailable
        {
            get;
            private set;
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
