using System;

namespace TwistedLogik.Gluon
{
    public static unsafe partial class gl
    {
        private delegate void glTextureParameteriDelegate(uint texture, uint target, uint pname, int param);
        [Require(MinVersion = "4.5", Extension = "GL_EXT_direct_state_access", ExtensionFunction = "glTextureParameteriEXT")]
        private static readonly glTextureParameteriDelegate glTextureParameteri = null;

        public static void TextureParameteri(uint texture, uint target, uint pname, int param) 
        {
            if (IsDirectStateAccessAvailable)
            {
                glTextureParameteri(texture, target, pname, param);
            }
            else
            {
                glTexParameteri(target, pname, param);
            }
        }

        private delegate void glTextureImage2DDelegate(uint texture, uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, void* pixels);
        [Require(MinVersion = "4.5", Extension = "GL_EXT_direct_state_access", ExtensionFunction = "glTextureImage2DEXT")]
        private static readonly glTextureImage2DDelegate glTextureImage2D = null;

        public static void TextureImage2D(uint texture, uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, void* pixels)
        {
            if (IsDirectStateAccessAvailable)
            {
                glTextureImage2D(texture, target, level, internalformat, width, height, border, format, type, pixels);
            }
            else
            {
                glTexImage2D(target, level, internalformat, width, height, border, format, type, pixels);
            }
        }

        private delegate void glTextureSubImage2DDelegate(uint texture, uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, void* pixels);
        [Require(MinVersion = "4.5", Extension = "GL_EXT_direct_state_access", ExtensionFunction = "glTextureSubImage2DEXT")]
        private static readonly glTextureSubImage2DDelegate glTextureSubImage2D = null;

        public static void TextureSubImage2D(uint texture, uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, void* pixels)
        {
            if (IsDirectStateAccessAvailable)
            {
                glTextureSubImage2D(texture, target, level, xoffset, yoffset, width, height, format, type, pixels);
            }
            else
            {
                glTexSubImage2D(target, level, xoffset, yoffset, width, height, format, type, pixels);
            }
        }

        public static Boolean IsDirectStateAccessAvailable
        {
            get;
            private set;
        }
    }
}
