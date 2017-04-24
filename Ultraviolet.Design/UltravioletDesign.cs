using System;
using Ultraviolet.Core;
using Ultraviolet.Core.Design;

namespace Ultraviolet.Design
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
            Contract.EnsureNot(Initialized, CoreStrings.TypeMetadataAlreadyLoaded);

            if (!CoreDesign.Initialized)
            {
                CoreDesign.Initialize();
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
            "Ultraviolet.Design.Circle.xml",
            "Ultraviolet.Design.CircleF.xml",
            "Ultraviolet.Design.Color.xml",
            "Ultraviolet.Design.Matrix.xml",
            "Ultraviolet.Design.Radians.xml",
            "Ultraviolet.Design.Rectangle.xml",
            "Ultraviolet.Design.RectangleF.xml",
            "Ultraviolet.Design.Size2.xml",
            "Ultraviolet.Design.Size2F.xml",
            "Ultraviolet.Design.Size3.xml",
            "Ultraviolet.Design.Size3F.xml",
            "Ultraviolet.Design.Vector2.xml",
            "Ultraviolet.Design.Vector3.xml",
            "Ultraviolet.Design.Vector4.xml",
        };
    }
}
