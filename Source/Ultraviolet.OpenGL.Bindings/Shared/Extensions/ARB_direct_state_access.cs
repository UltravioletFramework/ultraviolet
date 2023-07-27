using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;

namespace Ultraviolet.OpenGL.Bindings
{
    public static unsafe partial class GL
    {
        [MonoNativeFunctionWrapper]
        private delegate void glNamedBufferDataDelegate(uint buffer, IntPtr size, IntPtr data, uint usage);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        [Require(Extension = "GL_EXT_direct_state_access", ExtensionFunction = "glNamedBufferDataEXT")]
        private static glNamedBufferDataDelegate glNamedBufferData = null;

        [MonoNativeFunctionWrapper]
        private delegate void glNamedBufferSubDataDelegate(uint buffer, IntPtr offset, IntPtr size, IntPtr data);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        [Require(Extension = "GL_EXT_direct_state_access", ExtensionFunction = "glNamedBufferSubDataEXT")]
        private static glNamedBufferSubDataDelegate glNamedBufferSubData = null;

        [MonoNativeFunctionWrapper]
        private delegate uint glCheckNamedFramebufferStatusDelegate(uint framebuffer, uint target);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        [Require(Extension = "GL_EXT_direct_state_access", ExtensionFunction = "glCheckNamedFramebufferStatusEXT")]
        private static glCheckNamedFramebufferStatusDelegate glCheckNamedFramebufferStatus = null;

        [MonoNativeFunctionWrapper]
        private delegate void glNamedFramebufferTextureDelegate(uint framebuffer, uint attachment, uint texture, int level);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        [Require(Extension = "GL_EXT_direct_state_access", ExtensionFunction = "glNamedFramebufferTextureEXT")]
        private static glNamedFramebufferTextureDelegate glNamedFramebufferTexture = null;

        [MonoNativeFunctionWrapper]
        private delegate void glNamedFramebufferRenderbufferDelegate(uint framebuffer, uint attachment, uint renderbuffertarget, uint renderbuffer);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        [Require(Extension = "GL_EXT_direct_state_access", ExtensionFunction = "glNamedFramebufferRenderbufferEXT")]
        private static glNamedFramebufferRenderbufferDelegate glNamedFramebufferRenderbuffer = null;

        [MonoNativeFunctionWrapper]
        private delegate void glNamedFramebufferDrawBufferDelegate(uint framebuffer, uint mode);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        [Require(Extension = "GL_EXT_direct_state_access", ExtensionFunction = "glFramebufferDrawBufferEXT")]
        private static glNamedFramebufferDrawBufferDelegate glNamedFramebufferDrawBuffer = null;

        [MonoNativeFunctionWrapper]
        private delegate void glNamedFramebufferDrawBuffersDelegate(uint framebuffer, int size, IntPtr bufs);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        [Require(Extension = "GL_EXT_direct_state_access", ExtensionFunction = "glFramebufferDrawBuffersEXT")]
        private static glNamedFramebufferDrawBuffersDelegate glNamedFramebufferDrawBuffers = null;

        [MonoNativeFunctionWrapper]
        private delegate void glTextureParameteriDelegate(uint texture, uint pname, int param);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        /* NOTE: Different signature in GL_EXT_direct_state_access */
        private static glTextureParameteriDelegate glTextureParameteri = null;

        [MonoNativeFunctionWrapper]
        private delegate void glTextureSubImage2DDelegate(uint texture, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, IntPtr pixels);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        /* NOTE: Different signature in GL_EXT_direct_state_access */
        private static glTextureSubImage2DDelegate glTextureSubImage2D = null;

        [MonoNativeFunctionWrapper]
        private delegate void glTextureSubImage3DDelegate(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr pixels);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        /* NOTE: Different signature in GL_EXT_direct_state_access */
        private static glTextureSubImage3DDelegate glTextureSubImage3D = null;

        [MonoNativeFunctionWrapper]
        private delegate void glTextureStorage1DDelegate(uint texture, int levels, uint internalformat, int width);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        /* NOTE: Different signature in GL_EXT_direct_state_access */
        private static glTextureStorage1DDelegate glTextureStorage1D = null;

        [MonoNativeFunctionWrapper]
        private delegate void glTextureStorage2DDelegate(uint texture, int levels, uint internalformat, int width, int height);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        /* NOTE: Different signature in GL_EXT_direct_state_access */
        private static glTextureStorage2DDelegate glTextureStorage2D = null;
                
        [MonoNativeFunctionWrapper]
        private delegate void glTextureStorage3DDelegate(uint texture, int levels, uint internalformat, int width, int height, int depth);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        /* NOTE: Different signature in GL_EXT_direct_state_access */
        private static glTextureStorage3DDelegate glTextureStorage3D = null;

        [MonoNativeFunctionWrapper]
        private delegate void glCreateBuffersDelegate(int n, IntPtr buffers);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        private static glCreateBuffersDelegate glCreateBuffers = null;

        public static void CreateBuffers(int n, uint* buffers)
        {
            glCreateBuffers(n, (IntPtr)buffers);
        }

        public static uint CreateBuffer()
        {
            unsafe
            {
                uint buffers;
                glCreateBuffers(1, (IntPtr)(&buffers));
                return buffers;
            }
        }

        [MonoNativeFunctionWrapper]
        private delegate void glCreateTexturesDelegate(uint target, int n, IntPtr textures);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        private static glCreateTexturesDelegate glCreateTextures = null;

        public static void CreateTextures(uint target, int n, uint* textures)
        {
            glCreateTextures(target, n, (IntPtr)textures);
        }

        public static uint CreateTexture(uint target)
        {
            unsafe
            {
                uint textures;
                glCreateTextures(target, 1, (IntPtr)(&textures));
                return textures;
            }
        }

        [MonoNativeFunctionWrapper]
        private delegate void glCreateFramebuffersDelegate(int n, IntPtr ids);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        private static glCreateFramebuffersDelegate glCreateFramebuffers = null;

        public static void CreateFramebuffers(int n, uint* ids)
        {
            glCreateFramebuffers(n, (IntPtr)ids);
        }

        public static uint CreateFramebuffer()
        {
            unsafe
            {
                uint framebuffers;
                glCreateFramebuffers(1, (IntPtr)(&framebuffers));
                return framebuffers;
            }
        }

        [MonoNativeFunctionWrapper]
        private delegate void glCreateRenderbuffersDelegate(int n, IntPtr renderbuffers);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        private static glCreateRenderbuffersDelegate glCreateRenderbuffers = null;

        public static void CreateRenderbuffers(int n, uint* renderbuffers)
        {
            glCreateRenderbuffers(n, (IntPtr)renderbuffers);
        }

        public static uint CreateRenderbuffer()
        {
            unsafe
            {
                uint renderbuffers;
                glCreateRenderbuffers(1, (IntPtr)(&renderbuffers));
                return renderbuffers;
            }
        }

        [MonoNativeFunctionWrapper]
        private delegate IntPtr glMapNamedBufferDelegate(uint buffer, uint access);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        [Require(Extension = "GL_EXT_direct_state_access", ExtensionFunction = "glMapNamedBufferEXT")]
        private static glMapNamedBufferDelegate glMapNamedBuffer = null;

        [MonoNativeFunctionWrapper]
        private delegate IntPtr glMapNamedBufferRangeDelegate(uint buffer, IntPtr offset, IntPtr length, uint access);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        [Require(Extension = "GL_EXT_direct_state_access", ExtensionFunction = "glMapNamedBufferRangeEXT")]
        private static glMapNamedBufferRangeDelegate glMapNamedBufferRange = null;

        [MonoNativeFunctionWrapper]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool glUnmapNamedBufferDelegate(uint buffer);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        [Require(Extension = "GL_EXT_direct_state_access", ExtensionFunction = "glUnmapNamedBufferEXT")]
        private static glUnmapNamedBufferDelegate glUnmapNamedBuffer = null;

        [MonoNativeFunctionWrapper]
        private delegate void glFlushMappedNamedBufferRangeDelegate(uint buffer, IntPtr offset, IntPtr length);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        [Require(Extension = "GL_EXT_direct_state_access", ExtensionFunction = "glFlushMappedNamedBufferRangeEXT")]
        private static glFlushMappedNamedBufferRangeDelegate glFlushMappedNamedBufferRange = null;

        [MonoNativeFunctionWrapper]
        private delegate void glGetNamedBufferParameterivDelegate(uint buffer, uint pname, IntPtr @params);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        [Require(Extension = "GL_EXT_direct_state_access", ExtensionFunction = "glGetNamedBufferParameterivEXT")]
        private static glGetNamedBufferParameterivDelegate glGetNamedBufferParameteriv = null;
        
        [MonoNativeFunctionWrapper]
        private delegate void glGetNamedBufferPointervDelegate(uint buffer, uint pname, IntPtr @params);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        [Require(Extension = "GL_EXT_direct_state_access", ExtensionFunction = "glGetNamedBufferPointervEXT")]
        private static glGetNamedBufferPointervDelegate glGetNamedBufferPointerv = null;

        [MonoNativeFunctionWrapper]
        private delegate void glGetNamedBufferSubDataDelegate(uint buffer, IntPtr offset, IntPtr size, IntPtr data);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        [Require(Extension = "GL_EXT_direct_state_access", ExtensionFunction = "glGetNamedBufferSubDataEXT")]
        private static glGetNamedBufferSubDataDelegate glGetNamedBufferSubData = null;

        [MonoNativeFunctionWrapper]
        private delegate void glCreateVertexArraysDelegate(int n, IntPtr arrays);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        private static glCreateVertexArraysDelegate glCreateVertexArrays = null;

        public static void CreateVertexArrays(int n, uint* arrays)
        {
            glCreateVertexArrays(n, (IntPtr)arrays);
        }

        public static uint CreateVertexArray()
        {
            unsafe
            {
                uint arrays;
                glCreateVertexArrays(1, (IntPtr)(&arrays));
                return arrays;
            }
        }

        [MonoNativeFunctionWrapper]
        private delegate void glEnableVertexArrayAttribDelegate(uint vaobj, uint index);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        [Require(Extension = "GL_EXT_direct_state_access", ExtensionFunction = "glEnableVertexArrayAttribEXT")]
        private static glEnableVertexArrayAttribDelegate glEnableVertexArrayAttrib = null;

        [MonoNativeFunctionWrapper]
        private delegate void glDisableVertexArrayAttribDelegate(uint vaobj, uint index);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        [Require(Extension = "GL_EXT_direct_state_access", ExtensionFunction = "glDisableVertexArrayAttribEXT")]
        private static glDisableVertexArrayAttribDelegate glDisableVertexArrayAttrib = null;

        [MonoNativeFunctionWrapper]
        private delegate void glInvalidateNamedFramebufferDataDelegate(UInt32 framebuffer, Int32 numAttachments, IntPtr attachments);
        [Require(MinVersion = "4.5")]
        [Require(Extension = "GL_ARB_direct_state_access")]
        private static glInvalidateNamedFramebufferDataDelegate glInvalidateNamedFramebufferData = null;
    }
}
