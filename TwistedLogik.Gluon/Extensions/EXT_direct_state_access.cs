using System;

namespace TwistedLogik.Gluon
{
    public static unsafe partial class gl
    {
        private delegate void glTextureParameteriDelegate(uint texture, uint target, uint pname, int param);
        [Require(MinVersion = "4.5", Extension = "GL_EXT_direct_state_access", ExtensionFunction = "glTextureParameteriEXT")]
        private static readonly glTextureParameteriDelegate glTextureParameteri = null;

        public static void TextureParameteri(uint texture, uint target, uint pname, int param) { glTextureParameteri(texture, target, pname, param); }

        private delegate void glTextureImage2DDelegate(uint texture, uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, void* pixels);
        [Require(MinVersion = "4.5", Extension = "GL_EXT_direct_state_access", ExtensionFunction = "glTextureImage2DEXT")]
        private static readonly glTextureImage2DDelegate glTextureImage2D = null;

        public static void TextureImage2D(uint texture, uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, void* pixels)
        { glTextureImage2D(texture, target, level, internalformat, width, height, border, format, type, pixels); }

        public static Boolean IsDirectStateAccessAvailable
        {
            get;
            private set;
        }
    }
}
