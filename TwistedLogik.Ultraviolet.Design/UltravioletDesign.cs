using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Design;
using System.Reflection;

namespace TwistedLogik.Ultraviolet.Design
{
    /// <summary>
    /// Contains methods for loading type metadata for Ultraviolet's basic data types.
    /// </summary>
    public static class UltravioletDesign
    {
        /// <summary>
        /// Loads the type metadata for Ultraviolet's basic data types.
        /// </summary>
        public static void Initialize()
        {
            Contract.EnsureNot(Initialized, NucleusStrings.TypeMetadataAlreadyLoaded);

            if (!NucleusDesign.Initialized)
            {
                NucleusDesign.Initialize();
            }

            foreach (var m in metadata)
            {
                using (var stream = typeof(UltravioletDesign).Assembly.GetManifestResourceStream(m))
                {
                    TypeDescriptionMetadataLoader.Load(stream);
                }
            }

            Initialized = true;
        }

        /// <summary>
        /// Gets a value indicating whether the type metadata for this assembly has been initialized.
        /// </summary>
        public static Boolean Initialized
        {
            get;
            private set;
        }

        // State values.
        private static readonly String[] metadata = new[]
        {
            "TwistedLogik.Ultraviolet.Design.Circle.xml",
            "TwistedLogik.Ultraviolet.Design.CircleF.xml",
            "TwistedLogik.Ultraviolet.Design.Color.xml",
            "TwistedLogik.Ultraviolet.Design.Matrix.xml",
            "TwistedLogik.Ultraviolet.Design.Radians.xml",
            "TwistedLogik.Ultraviolet.Design.Rectangle.xml",
            "TwistedLogik.Ultraviolet.Design.RectangleF.xml",
            "TwistedLogik.Ultraviolet.Design.Size2.xml",
            "TwistedLogik.Ultraviolet.Design.Size2F.xml",
            "TwistedLogik.Ultraviolet.Design.Size3.xml",
            "TwistedLogik.Ultraviolet.Design.Size3F.xml",
            "TwistedLogik.Ultraviolet.Design.Vector2.xml",
            "TwistedLogik.Ultraviolet.Design.Vector3.xml",
            "TwistedLogik.Ultraviolet.Design.Vector4.xml",
        };
    }
}
