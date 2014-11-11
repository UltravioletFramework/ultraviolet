using System;

namespace TwistedLogik.Gluon
{
	public static unsafe partial class gl
	{
		private delegate void glBindFramebufferDelegate(uint target, uint framebuffer);
		[Require(MinVersion = "3.0", Extension = "GL_ARB_framebuffer_object")]
		private static readonly glBindFramebufferDelegate glBindFramebuffer = null;

		public static void BindFramebuffer(uint target, uint framebuffer) { glBindFramebuffer(target, framebuffer); }

		private delegate void glBindRenderbufferDelegate(uint target, uint renderbuffer);
		[Require(MinVersion = "3.0", Extension = "GL_ARB_framebuffer_object")]
		private static readonly glBindRenderbufferDelegate glBindRenderbuffer = null;

		public static void BindRenderbuffer(uint target, uint renderbuffer) { glBindRenderbuffer(target, renderbuffer); }

		private delegate void glBlitFramebufferDelegate(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, uint filter);
		[Require(MinVersion = "3.0", Extension = "GL_ARB_framebuffer_object")]
		private static readonly glBlitFramebufferDelegate glBlitFramebuffer = null;

		public static void BlitFramebuffer(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, uint filter) { glBlitFramebuffer(srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter); }

		private delegate uint glCheckFramebufferStatusDelegate(uint target);
		[Require(MinVersion = "3.0", Extension = "GL_ARB_framebuffer_object")]
		private static readonly glCheckFramebufferStatusDelegate glCheckFramebufferStatus = null;

		public static uint CheckFramebufferStatus(uint target) { return glCheckFramebufferStatus(target); }
		
		private delegate void glDeleteFramebuffersDelegate(int n, uint* framebuffers);
		[Require(MinVersion = "3.0", Extension = "GL_ARB_framebuffer_object")]
		private static readonly glDeleteFramebuffersDelegate glDeleteFramebuffers = null;

		public static void DeleteFramebuffers(int n, uint* framebuffers) { glDeleteFramebuffers(n, framebuffers); }

        public static void DeleteFramebuffers(uint[] framebuffers)
        {
            fixed (uint* pframebuffers = framebuffers)
            {
                glDeleteFramebuffers(framebuffers.Length, pframebuffers);
            }
        }

        public static void DeleteFramebuffer(uint framebuffer)
        {
            glDeleteFramebuffers(1, &framebuffer);
        }

		private delegate void glDeleteRenderbuffersDelegate(int n, uint* renderbuffers);
        [Require(MinVersion = "3.0", Extension = "GL_ARB_framebuffer_object")]
        private static readonly glDeleteRenderbuffersDelegate glDeleteRenderbuffers = null;

        public static void DeleteRenderBuffers(int n, uint* renderbuffers) { glDeleteRenderbuffers(n, renderbuffers); }

        public static void DeleteRenderBuffers(uint[] renderbuffers)
        {
            fixed (uint* prenderbuffers = renderbuffers)
            {
                glDeleteRenderbuffers(renderbuffers.Length, prenderbuffers);
            }
        }

        public static void DeleteRenderBuffers(uint renderbuffer)
        {
            glDeleteRenderbuffers(1, &renderbuffer);
        }

		private delegate void glFramebufferRenderbufferDelegate(uint target, uint attachment, uint renderbuffertarget, uint renderbuffer);
		[Require(MinVersion = "3.0", Extension = "GL_ARB_framebuffer_object")]
		private static readonly glFramebufferRenderbufferDelegate glFramebufferRenderbuffer = null;

		public static void FramebufferRenderbuffer(uint target, uint attachment, uint renderbuffertarget, uint renderbuffer) { glFramebufferRenderbuffer(target, attachment, renderbuffertarget, renderbuffer); }

		private delegate void glFramebufferTexture1DDelegate(uint target, uint attachment, uint textarget, uint texture, int level);
		[Require(MinVersion = "3.0", Extension = "GL_ARB_framebuffer_object")]
		private static readonly glFramebufferTexture1DDelegate glFramebufferTexture1D = null;

		public static void FramebufferTexture1D(uint target, uint attachment, uint textarget, uint texture, int level) { glFramebufferTexture1D(target, attachment, textarget, texture, level); }

		private delegate void glFramebufferTexture2DDelegate(uint target, uint attachment, uint textarget, uint texture, int level);
		[Require(MinVersion = "3.0", Extension = "GL_ARB_framebuffer_object")]
		private static readonly glFramebufferTexture2DDelegate glFramebufferTexture2D = null;

		public static void FramebufferTexture2D(uint target, uint attachment, uint textarget, uint texture, int level) { glFramebufferTexture2D(target, attachment, textarget, texture, level); }

		private delegate void glFramebufferTexture3DDelegate(uint target, uint attachment, uint textarget, uint texture, int level, int layer);
		[Require(MinVersion = "3.0", Extension = "GL_ARB_framebuffer_object")]
		private static readonly glFramebufferTexture3DDelegate glFramebufferTexture3D = null;

		public static void FramebufferTexture3D(uint target, uint attachment, uint textarget, uint texture, int level, int layer) { glFramebufferTexture3D(target, attachment, textarget, texture, level, layer); }

		private delegate void glFramebufferTextureLayerDelegate(uint target, uint attachment, uint texture, int level, int layer);
		[Require(MinVersion = "3.0", Extension = "GL_ARB_framebuffer_object")]
		private static readonly glFramebufferTextureLayerDelegate glFramebufferTextureLayer = null;

		public static void FramebufferTextureLayer(uint target, uint attachment, uint texture, int level, int layer) { glFramebufferTextureLayer(target, attachment, texture, level, layer); }

		private delegate void glGenFramebuffersDelegate(int n, uint* framebuffers);
		[Require(MinVersion = "3.0", Extension = "GL_ARB_framebuffer_object")]
		private static readonly glGenFramebuffersDelegate glGenFramebuffers = null;

		public static void GenFramebuffers(int n, uint* framebuffers) { glGenFramebuffers(n, framebuffers); }

        public static void GenFramebuffers(uint[] framebuffers)
        {
            fixed (uint* ptextures = framebuffers)
            {
                glGenFramebuffers(framebuffers.Length, ptextures);
            }
        }

        public static uint GenFramebuffer()
        {
            uint value;
            glGenFramebuffers(1, &value);
            return value;
        }

		private delegate void glGenRenderbuffersDelegate(int n, uint* renderbuffers);
		[Require(MinVersion = "3.0", Extension = "GL_ARB_framebuffer_object")]
		private static readonly glGenRenderbuffersDelegate glGenRenderbuffers = null;

		public static void GenRenderbuffers(int n, uint* renderbuffers) { glGenRenderbuffers(n, renderbuffers); }

        public static void GenRenderbuffers(uint[] renderbuffers)
        {
            fixed (uint* ptextures = renderbuffers)
            {
                glGenRenderbuffers(renderbuffers.Length, ptextures);
            }
        }

        public static uint GenRenderbuffer()
        {
            uint value;
            glGenRenderbuffers(1, &value);
            return value;
        }

		private delegate void glGenerateMipmapDelegate(uint target);
		[Require(MinVersion = "3.0", Extension = "GL_ARB_framebuffer_object")]
		private static readonly glGenerateMipmapDelegate glGenerateMipmap = null;

		public static void GenerateMipmap(uint target) { glGenerateMipmap(target); }

		private delegate void glGetFramebufferAttachmentParameterivDelegate(uint target, uint attachment, uint pname, int* @params);
		[Require(MinVersion = "3.0", Extension = "GL_ARB_framebuffer_object")]
		private static readonly glGetFramebufferAttachmentParameterivDelegate glGetFramebufferAttachmentParameteriv = null;

		public static void GetFramebufferAttachmentParameteriv(uint target, uint attachment, uint pname, int* @params) { glGetFramebufferAttachmentParameteriv(target, attachment, pname, @params); }

		private delegate void glGetRenderbufferParameterivDelegate(uint target, uint pname, int* @params);
		[Require(MinVersion = "3.0", Extension = "GL_ARB_framebuffer_object")]
		private static readonly glGetRenderbufferParameterivDelegate glGetRenderbufferParameteriv = null;

		public static void GetRenderbufferParameteriv(uint target, uint pname, int* @params) { glGetRenderbufferParameteriv(target, pname, @params); }

		private delegate bool glIsFramebufferDelegate(uint framebuffer);
		[Require(MinVersion = "3.0", Extension = "GL_ARB_framebuffer_object")]
		private static readonly glIsFramebufferDelegate glIsFramebuffer = null;

		public static bool IsFramebuffer(uint framebuffer) { return glIsFramebuffer(framebuffer); }

		private delegate bool glIsRenderbufferDelegate(uint renderbuffer);
		[Require(MinVersion = "3.0", Extension = "GL_ARB_framebuffer_object")]
		private static readonly glIsRenderbufferDelegate glIsRenderbuffer = null;

		public static bool IsRenderbuffer(uint renderbuffer) { return glIsRenderbuffer(renderbuffer); }

		private delegate void glRenderbufferStorageDelegate(uint target, uint internalformat, int width, int height);
		[Require(MinVersion = "3.0", Extension = "GL_ARB_framebuffer_object")]
		private static readonly glRenderbufferStorageDelegate glRenderbufferStorage = null;

		public static void RenderbufferStorage(uint target, uint internalformat, int width, int height) { glRenderbufferStorage(target, internalformat, width, height); }

		private delegate void glRenderbufferStorageMultisampleDelegate(uint target, int samples, uint internalformat, int width, int height);
		[Require(MinVersion = "3.0", Extension = "GL_ARB_framebuffer_object")]
		private static readonly glRenderbufferStorageMultisampleDelegate glRenderbufferStorageMultisample = null;

		public static void RenderbufferStorageMultisample(uint target, int samples, uint internalformat, int width, int height) { glRenderbufferStorageMultisample(target, samples, internalformat, width, height); }

		public const UInt32 GL_INVALID_FRAMEBUFFER_OPERATION = 0x0506;
		public const UInt32 GL_FRAMEBUFFER_ATTACHMENT_COLOR_ENCODING = 0x8210;
		public const UInt32 GL_FRAMEBUFFER_ATTACHMENT_COMPONENT_TYPE = 0x8211;
		public const UInt32 GL_FRAMEBUFFER_ATTACHMENT_RED_SIZE = 0x8212;
		public const UInt32 GL_FRAMEBUFFER_ATTACHMENT_GREEN_SIZE = 0x8213;
		public const UInt32 GL_FRAMEBUFFER_ATTACHMENT_BLUE_SIZE = 0x8214;
		public const UInt32 GL_FRAMEBUFFER_ATTACHMENT_ALPHA_SIZE = 0x8215;
		public const UInt32 GL_FRAMEBUFFER_ATTACHMENT_DEPTH_SIZE = 0x8216;
		public const UInt32 GL_FRAMEBUFFER_ATTACHMENT_STENCIL_SIZE = 0x8217;
		public const UInt32 GL_FRAMEBUFFER_DEFAULT = 0x8218;
		public const UInt32 GL_FRAMEBUFFER_UNDEFINED = 0x8219;
		public const UInt32 GL_DEPTH_STENCIL_ATTACHMENT = 0x821A;
		public const UInt32 GL_INDEX = 0x8222;
		public const UInt32 GL_MAX_RENDERBUFFER_SIZE = 0x84E8;
		public const UInt32 GL_DEPTH_STENCIL = 0x84F9;
		public const UInt32 GL_UNSIGNED_INT_24_8 = 0x84FA;
		public const UInt32 GL_DEPTH24_STENCIL8 = 0x88F0;
		public const UInt32 GL_TEXTURE_STENCIL_SIZE = 0x88F1;
		public const UInt32 GL_UNSIGNED_NORMALIZED = 0x8C17;
		// Defined in gl.Core_2_1.cs | public const UInt32 GL_SRGB = 0x8C40;
		public const UInt32 GL_DRAW_FRAMEBUFFER_BINDING = 0x8CA6;
		public const UInt32 GL_FRAMEBUFFER_BINDING = 0x8CA6;
		public const UInt32 GL_RENDERBUFFER_BINDING = 0x8CA7;
		public const UInt32 GL_READ_FRAMEBUFFER = 0x8CA8;
		public const UInt32 GL_DRAW_FRAMEBUFFER = 0x8CA9;
		public const UInt32 GL_READ_FRAMEBUFFER_BINDING = 0x8CAA;
		public const UInt32 GL_RENDERBUFFER_SAMPLES = 0x8CAB;
		public const UInt32 GL_FRAMEBUFFER_ATTACHMENT_OBJECT_TYPE = 0x8CD0;
		public const UInt32 GL_FRAMEBUFFER_ATTACHMENT_OBJECT_NAME = 0x8CD1;
		public const UInt32 GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_LEVEL = 0x8CD2;
		public const UInt32 GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_CUBE_MAP_FACE = 0x8CD3;
		public const UInt32 GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_LAYER = 0x8CD4;
		public const UInt32 GL_FRAMEBUFFER_COMPLETE = 0x8CD5;
		public const UInt32 GL_FRAMEBUFFER_INCOMPLETE_ATTACHMENT = 0x8CD6;
		public const UInt32 GL_FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT = 0x8CD7;
		public const UInt32 GL_FRAMEBUFFER_INCOMPLETE_DRAW_BUFFER = 0x8CDB;
		public const UInt32 GL_FRAMEBUFFER_INCOMPLETE_READ_BUFFER = 0x8CDC;
		public const UInt32 GL_FRAMEBUFFER_UNSUPPORTED = 0x8CDD;
		public const UInt32 GL_MAX_COLOR_ATTACHMENTS = 0x8CDF;
		public const UInt32 GL_COLOR_ATTACHMENT0 = 0x8CE0;
		public const UInt32 GL_COLOR_ATTACHMENT1 = 0x8CE1;
		public const UInt32 GL_COLOR_ATTACHMENT2 = 0x8CE2;
		public const UInt32 GL_COLOR_ATTACHMENT3 = 0x8CE3;
		public const UInt32 GL_COLOR_ATTACHMENT4 = 0x8CE4;
		public const UInt32 GL_COLOR_ATTACHMENT5 = 0x8CE5;
		public const UInt32 GL_COLOR_ATTACHMENT6 = 0x8CE6;
		public const UInt32 GL_COLOR_ATTACHMENT7 = 0x8CE7;
		public const UInt32 GL_COLOR_ATTACHMENT8 = 0x8CE8;
		public const UInt32 GL_COLOR_ATTACHMENT9 = 0x8CE9;
		public const UInt32 GL_COLOR_ATTACHMENT10 = 0x8CEA;
		public const UInt32 GL_COLOR_ATTACHMENT11 = 0x8CEB;
		public const UInt32 GL_COLOR_ATTACHMENT12 = 0x8CEC;
		public const UInt32 GL_COLOR_ATTACHMENT13 = 0x8CED;
		public const UInt32 GL_COLOR_ATTACHMENT14 = 0x8CEE;
		public const UInt32 GL_COLOR_ATTACHMENT15 = 0x8CEF;
		public const UInt32 GL_DEPTH_ATTACHMENT = 0x8D00;
		public const UInt32 GL_STENCIL_ATTACHMENT = 0x8D20;
		public const UInt32 GL_FRAMEBUFFER = 0x8D40;
		public const UInt32 GL_RENDERBUFFER = 0x8D41;
		public const UInt32 GL_RENDERBUFFER_WIDTH = 0x8D42;
		public const UInt32 GL_RENDERBUFFER_HEIGHT = 0x8D43;
		public const UInt32 GL_RENDERBUFFER_INTERNAL_FORMAT = 0x8D44;
		public const UInt32 GL_STENCIL_INDEX1 = 0x8D46;
		public const UInt32 GL_STENCIL_INDEX4 = 0x8D47;
		public const UInt32 GL_STENCIL_INDEX8 = 0x8D48;
		public const UInt32 GL_STENCIL_INDEX16 = 0x8D49;
		public const UInt32 GL_RENDERBUFFER_RED_SIZE = 0x8D50;
		public const UInt32 GL_RENDERBUFFER_GREEN_SIZE = 0x8D51;
		public const UInt32 GL_RENDERBUFFER_BLUE_SIZE = 0x8D52;
		public const UInt32 GL_RENDERBUFFER_ALPHA_SIZE = 0x8D53;
		public const UInt32 GL_RENDERBUFFER_DEPTH_SIZE = 0x8D54;
		public const UInt32 GL_RENDERBUFFER_STENCIL_SIZE = 0x8D55;
		public const UInt32 GL_FRAMEBUFFER_INCOMPLETE_MULTISAMPLE = 0x8D56;
		public const UInt32 GL_MAX_SAMPLES = 0x8D57;
	}
}
