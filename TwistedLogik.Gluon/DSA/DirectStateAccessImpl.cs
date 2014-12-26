using System;

namespace TwistedLogik.Gluon
{
    partial class gl
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

            public abstract uint CheckNamedFramebufferStatus(uint framebuffer, uint target);

            public abstract void TextureParameteri(uint texture, uint target, uint pname, int param);

            public abstract void TextureImage2D(uint texture, uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, void* pixels);

            public abstract void TextureSubImage2D(uint texture, uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, void* pixels);

            public abstract void TextureStorage1D(uint texture, uint target, int levels, uint internalformat, int width);

            public abstract void TextureStorage2D(uint texture, uint target, int levels, uint internalformat, int width, int height);

            public abstract void TextureStorage3D(uint texture, uint target, int levels, uint internalformat, int width, int height, int depth);
        }
    }
}
