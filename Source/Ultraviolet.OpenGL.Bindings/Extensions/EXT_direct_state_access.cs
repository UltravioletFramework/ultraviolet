using System;
using Ultraviolet.Core;

namespace Ultraviolet.OpenGL.Bindings
{
    public static unsafe partial class GL
    {
        /* NOTE: Functions which are shared with the ARB extension are defined in GL_ARB_direct_state_access.cs */

        [MonoNativeFunctionWrapper]
        private delegate void glTextureParameteriEXTDelegate(uint texture, uint target, uint pname, int param);
        [Require(Extension = "GL_EXT_direct_state_access")]
        private static glTextureParameteriEXTDelegate glTextureParameteriEXT = null;

        [MonoNativeFunctionWrapper]
        private delegate void glTextureImage2DEXTDelegate(uint texture, uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, IntPtr pixels);
        [Require(Extension = "GL_EXT_direct_state_access")]
        private static glTextureImage2DEXTDelegate glTextureImage2DEXT = null;

        [MonoNativeFunctionWrapper]
        private delegate void glTextureImage3DEXTDelegate(uint texture, uint target, int level, int internalformat, int width, int height, int depth, int border, uint format, uint type, IntPtr pixels);
        [Require(Extension = "GL_EXT_direct_state_access")]
        private static glTextureImage3DEXTDelegate glTextureImage3DEXT = null;

        [MonoNativeFunctionWrapper]
        private delegate void glTextureSubImage2DEXTDelegate(uint texture, uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, IntPtr pixels);
        [Require(Extension = "GL_EXT_direct_state_access")]
        private static glTextureSubImage2DEXTDelegate glTextureSubImage2DEXT = null;

        [MonoNativeFunctionWrapper]
        private delegate void glTextureSubImage3DEXTDelegate(uint texture, uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr pixels);
        [Require(Extension = "GL_EXT_direct_state_access")]
        private static glTextureSubImage3DEXTDelegate glTextureSubImage3DEXT = null;
    }
}
