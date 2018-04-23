using System;
using static Ultraviolet.FreeType2.Native.FreeTypeNative;
using static Ultraviolet.FreeType2.Native.FT_Error;

namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Encapsulates the native FreeType2 library object.
    /// </summary>
    internal sealed unsafe class FreeTypeLibrary : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FreeTypeLibrary"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public FreeTypeLibrary(UltravioletContext uv)
            : base(uv)
        {
            var lib = default(IntPtr);
            var err = FT_Init_FreeType((IntPtr)(&lib));
            if (err != FT_Err_Ok)
                throw new FreeTypeException(err);

            Native = lib;
        }

        /// <summary>
        /// Gets the native pointer to the FreeType2 library object.
        /// </summary>
        public IntPtr Native { get; private set; }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Native != IntPtr.Zero)
            {
                var err = FT_Done_FreeType(Native);
                if (err != FT_Err_Ok)
                    throw new FreeTypeException(err);

                Native = IntPtr.Zero;
            }
            base.Dispose(disposing);
        }
    }
}
