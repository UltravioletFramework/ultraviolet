using System;

namespace TwistedLogik.Gluon
{
    public static unsafe partial class gl
    {
        public const UInt32 GL_TEXTURE_MAX_ANISOTROPY_EXT = 0x84FE;
        public const UInt32 GL_MAX_TEXTURE_MAX_ANISOTROPY_EXT = 0x84FF;

        public static Boolean IsAnisotropicFilteringAvailable
        {
            get;
            private set;
        }
    }
}
