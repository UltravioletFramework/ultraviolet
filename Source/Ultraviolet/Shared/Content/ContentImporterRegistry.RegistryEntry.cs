using System;
using System.Linq;
using Ultraviolet.Core;

namespace Ultraviolet.Content
{
    partial class ContentImporterRegistry
    {
        /// <summary>
        /// Represents an entry in a content importer registry.
        /// </summary>
        private struct RegistryEntry
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="RegistryEntry"/> structure.
            /// </summary>
            /// <param name="importer">The registered importer instance.</param>
            public RegistryEntry(IContentImporter importer)
            {
                Contract.Require(importer, nameof(importer));

                var baseImporterType = ContentImporterRegistry.GetBaseContentImporterType(importer.GetType());
                var outputType = baseImporterType.GetGenericArguments().Single();

                this.importer = importer;
                this.outputType = outputType;
            }

            /// <summary>
            /// Get the registered importer instance.
            /// </summary>
            public IContentImporter Importer
            {
                get { return importer; }
            }

            /// <summary>
            /// Gets the registered importer's output type.
            /// </summary>
            public Type OutputType
            {
                get { return outputType; }
            }

            // Property values.
            private readonly IContentImporter importer;
            private readonly Type outputType;
        }
    }
}
