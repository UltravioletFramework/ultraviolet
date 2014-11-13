using System;

namespace TwistedLogik.Nucleus.Design
{
    /// <summary>
    /// Contains methods for loading type metadata for Nucleus's basic data types.
    /// </summary>
    public static class NucleusDesign
    {
        /// <summary>
        /// Loads the type metadata for Nucleus's basic data types.
        /// </summary>
        public static void Initialize()
        {
            Contract.EnsureNot(Initialized, NucleusStrings.TypeMetadataAlreadyLoaded);

            foreach (var m in metadata)
            {
                using (var stream = typeof(NucleusDesign).Assembly.GetManifestResourceStream(m))
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
            "TwistedLogik.Nucleus.Design.MaskedUInt32.xml",
            "TwistedLogik.Nucleus.Design.MaskedUInt64.xml",
            "TwistedLogik.Nucleus.Design.Text.StringResource.xml",
        };
    }
}
