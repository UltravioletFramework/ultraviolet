using System;
using Ultraviolet.Core;

namespace Ultraviolet.OpenGL.Bindings
{
    public static unsafe partial class gl
    {
        /* NOTE: Functions which are shared with the ARB extension are defined in GL_ARB_direct_state_access.cs */

        [MonoNativeFunctionWrapper]
        private delegate void glTextureParameteriEXTDelegate(uint texture, uint target, uint pname, int param);
        [Require(Extension = "GL_EXT_direct_state_access")]
        private static readonly glTextureParameteriEXTDelegate glTextureParameteriEXT = null;

        [MonoNativeFunctionWrapper]
        private delegate void glTextureImage2DEXTDelegate(uint texture, uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, IntPtr pixels);
        [Require(Extension = "GL_EXT_direct_state_access")]
        private static readonly glTextureImage2DEXTDelegate glTextureImage2DEXT = null;

        [MonoNativeFunctionWrapper]
        private delegate void glTextureSubImage2DEXTDelegate(uint texture, uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, IntPtr pixels);
        [Require(Extension = "GL_EXT_direct_state_access")]
        private static readonly glTextureSubImage2DEXTDelegate glTextureSubImage2DEXT = null;        
    }
}
