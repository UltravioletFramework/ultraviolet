using System;

namespace Ultraviolet.Core.Design
{
    /// <summary>
    /// Contains methods for loading type metadata for the Ultraviolet core library's basic data types.
    /// </summary>
    public static class CoreDesign
    {
        /// <summary>
        /// Loads the type metadata for the Ultraviolet core library's basic data types.
        /// </summary>
        public static void Initialize()
        {
            Contract.EnsureNot(Initialized, CoreStrings.TypeMetadataAlreadyLoaded);

            foreach (var m in metadata)
            {
                using (var stream = typeof(CoreDesign).Assembly.GetManifestResourceStream(m))
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
            "Ultraviolet.Core.Design.MaskedUInt32.xml",
            "Ultraviolet.Core.Design.MaskedUInt64.xml",
            "Ultraviolet.Core.Design.Text.StringResource.xml",
        };
    }
}
