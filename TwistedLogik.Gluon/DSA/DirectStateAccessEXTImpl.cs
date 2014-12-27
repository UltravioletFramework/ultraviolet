using System;

namespace TwistedLogik.Gluon
{
    partial class gl
    {
        /// <summary>
        /// Implements DSA functions for OpenGL contexts which support the GL_EXT_direct_state_access extension.
        /// </summary>
        internal sealed unsafe class DirectStateAccessEXTImpl : DirectStateAccessImpl
        {
            public override void NamedBufferData(uint buffer, uint target, IntPtr size, void* data, uint usage)
            {
                glNamedBufferDataEXT(buffer, size, data, usage);
            }

            public override void NamedBufferSubData(uint buffer, uint target, IntPtr offset, IntPtr size, void* data)
            {
                glNamedBufferSubDataEXT(buffer, offset, size, data);
            }

            public override void NamedFramebufferTexture(uint framebuffer, uint target, uint attachment, uint texture, int level)
            {
                glNamedFramebufferTextureEXT(framebuffer, attachment, texture, level);
            }

            public override uint CheckNamedFramebufferStatus(uint framebuffer, uint target)
            {
                return glCheckNamedFramebufferStatusEXT(framebuffer, target);
            }

            public override void TextureParameteri(uint texture, uint target, uint pname, int param)
            {
                glTextureParameteriEXT(texture, target, pname, param);
            }

            public override void TextureImage2D(uint texture, uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, void* pixels)
            {
                glTextureImage2DEXT(texture, target, level, internalformat, width, height, border, format, type, pixels);
            }

            public override void TextureSubImage2D(uint texture, uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, void* pixels)
            {
                glTextureSubImage2DEXT(texture, target, level, xoffset, yoffset, width, height, format, type, pixels);
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
        }
    }
}
