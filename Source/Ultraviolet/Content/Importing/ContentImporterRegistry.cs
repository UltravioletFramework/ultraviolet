using System;
using System.Collections.Generic;
using System.IO;
using Ultraviolet.Core;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents an Ultraviolet context's registry of content importers.
    /// </summary>
    public sealed partial class ContentImporterRegistry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentImporterRegistry"/> class.
        /// </summary>
        internal ContentImporterRegistry()
        {
            this.internalByteArrayImporter = new InternalByteArrayImporter();
            this.internalMemoryStreamImporter = new InternalMemoryStreamImporter();
        }
        
        /// <summary>
        /// Finds the appropriate importer for the specified file extension.
        /// </summary>
        /// <param name="extension">The file extension for which to find a content importer.</param>
        /// <returns>The content importer for the specified file extension, or <see langword="null"/> if no appropriate importer could be found.</returns>
        public IContentImporter FindImporter(String extension)
        {
            var outputType = default(Type);
            return FindImporter(extension, ref outputType);
        }

        /// <summary>
        /// Finds the appropriate importer for the specified file extension.
        /// </summary>
        /// <param name="extension">The file extension for which to find a content importer.</param>
        /// <param name="outputType">The importer's output type.</param>
        /// <returns>The content importer for the specified file extension, or <see langword="null"/> if no appropriate importer could be found.</returns>
        public IContentImporter FindImporter(String extension, ref Type outputType)
        {
            Contract.RequireNotEmpty(extension, nameof(extension));

            if (outputType == typeof(Byte[]))
                return internalByteArrayImporter;

            if (outputType == typeof(MemoryStream))
                return internalMemoryStreamImporter;

            if (registeredImporters.TryGetValue(extension, out var entry))
            {
                outputType = entry.OutputType;
            }
            else
            {
                outputType = null;
            }
            return entry.Importer;
        }

        /// <summary>
        /// Registers a content importer for the specified file extensions.
        /// </summary>
        /// <typeparam name="T">The type of content importer to register.</typeparam>
        /// <param name="extensions">The file extensions for which to register the importer.</param>
        public void RegisterImporter<T>(params String[] extensions) where T : IContentImporter
        {
            Contract.Require(extensions, nameof(extensions));

            var baseImporterType = GetBaseContentImporterType(typeof(T));
            if (baseImporterType == null)
                throw new InvalidOperationException(UltravioletStrings.ImporterInvalidBaseClass.Format(typeof(T).FullName));

            foreach (var extension in extensions)
            {
                Contract.RequireNotEmpty(extension, nameof(extension));

                if (registeredImporters.ContainsKey(extension))
                {
                    throw new InvalidOperationException(
                        UltravioletStrings.ImporterAlreadyRegistered.Format(typeof(T).FullName, extensions));
                }
                registeredImporters[extension] = new RegistryEntry(CreateImporterInstance(typeof(T)));
            }
        }

        /// <summary>
        /// Unregisters the content importer for the specified file extension.
        /// </summary>
        /// <param name="extension">The file extension for which to unregister an importer.</param>
        public void UnregisterImporter(String extension)
        {
            Contract.RequireNotEmpty(extension, nameof(extension));

            registeredImporters.Remove(extension);
        }

        /// <summary>
        /// Gets the ContentImporter type from which the specified type is derived.
        /// </summary>
        /// <param name="type">The type to evaluate.</param>
        /// <returns>The ContentImporter type from which the specified type is derived.</returns>
        internal static Type GetBaseContentImporterType(Type type)
        {
            var current = type;
            while (current != null)
            {
                if (current.IsGenericType && current.GetGenericTypeDefinition() == typeof(ContentImporter<>))
                {
                    return current;
                }
                current = current.BaseType;
            }
            return null;
        }

        /// <summary>
        /// Creates an instance of the specified importer type.
        /// </summary>
        /// <param name="type">The type of importer to instantiate.</param>
        /// <returns>The importer instance that was created.</returns>
        private IContentImporter CreateImporterInstance(Type type)
        {
            var ctor = type.GetConstructor(Type.EmptyTypes);
            if (ctor == null)
            {
                throw new InvalidOperationException(UltravioletStrings.ImporterRequiresCtor.Format(type.FullName));
            }
            return (IContentImporter)ctor.Invoke(null);
        }

        // Internal importers.
        private readonly InternalByteArrayImporter internalByteArrayImporter;
        private readonly InternalMemoryStreamImporter internalMemoryStreamImporter;

        // The content importer registry.
        private readonly Dictionary<String, RegistryEntry> registeredImporters = 
            new Dictionary<String, RegistryEntry>(StringComparer.OrdinalIgnoreCase);
    }
}
