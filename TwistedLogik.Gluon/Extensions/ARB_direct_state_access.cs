using System;

namespace TwistedLogik.Gluon
{
    public static unsafe partial class gl
    {
        private delegate void glNamedBufferDataDelegate(uint buffer, IntPtr size, void* data, uint usage);
        [Require(MinVersion = "4.5")]
        private static readonly glNamedBufferDataDelegate glNamedBufferData = null;

        private delegate void glNamedBufferSubDataDelegate(uint buffer, IntPtr offset, IntPtr size, void* data);
        [Require(MinVersion = "4.5")]
        private static readonly glNamedBufferSubDataDelegate glNamedBufferSubData = null;

        private delegate uint glCheckNamedFramebufferStatusDelegate(uint framebuffer, uint target);
        [Require(MinVersion = "4.5")]
        private static readonly glCheckNamedFramebufferStatusDelegate glCheckNamedFramebufferStatus = null;

        private delegate void glNamedFramebufferTextureDelegate(uint framebuffer, uint attachment, uint texture, int level);
        [Require(MinVersion = "4.5")]
        private static readonly glNamedFramebufferTextureDelegate glNamedFramebufferTexture = null;

        private delegate void glTextureParameteriDelegate(uint texture, uint pname, int param);
        [Require(MinVersion = "4.5")]
        private static readonly glTextureParameteriDelegate glTextureParameteri = null;

        private delegate void glTextureSubImage2DDelegate(uint texture, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, void* pixels);
        [Require(MinVersion = "4.5")]
        private static readonly glTextureSubImage2DDelegate glTextureSubImage2D = null;

        private delegate void glTextureStorage1DDelegate(uint texture, int levels, uint internalformat, int width);
        [Require(MinVersion = "4.5")]
        private static readonly glTextureStorage1DDelegate glTextureStorage1D = null;

        private delegate void glTextureStorage2DDelegate(uint texture, int levels, uint internalformat, int width, int height);
        [Require(MinVersion = "4.5")]
        private static readonly glTextureStorage2DDelegate glTextureStorage2D = null;
                
        private delegate void glTextureStorage3DDelegate(uint texture, int levels, uint internalformat, int width, int height, int depth);
        [Require(MinVersion = "4.5")]
        private static readonly glTextureStorage3DDelegate glTextureStorage3D = null;

        private delegate void glCreateBuffersDelegate(int n, uint* buffers);
        [Require(MinVersion = "4.5")]
        private static readonly glCreateBuffersDelegate glCreateBuffers = null;

        public static void CreateBuffers(int n, uint* buffers)
        {
            glCreateBuffers(n, buffers);
        }

        public static uint CreateBuffer()
        {
            unsafe
            {
                uint buffers;
                glCreateBuffers(1, &buffers);
                return buffers;
            }
        }

        private delegate void glCreateTexturesDelegate(uint target, int n, uint* textures);
        [Require(MinVersion = "4.5")]
        private static readonly glCreateTexturesDelegate glCreateTextures = null;

        public static void CreateTextures(uint target, int n, uint* textures)
        {
            glCreateTextures(target, n, textures);
        }

        public static uint CreateTexture(uint target)
        {
            unsafe
            {
                uint textures;
                glCreateTextures(target, 1, &textures);
                return textures;
            }
        }

        private delegate void glCreateFramebuffersDelegate(int n, uint* textures);
        [Require(MinVersion = "4.5")]
        private static readonly glCreateFramebuffersDelegate glCreateFramebuffers = null;

        public static void CreateFramebuffers(int n, uint* textures)
        {
            glCreateFramebuffers(n, textures);
        }

        public static uint CreateFramebuffer()
        {
            unsafe
            {
                uint framebuffers;
                glCreateFramebuffers(1, &framebuffers);
                return framebuffers;
            }
        }
    }
}
