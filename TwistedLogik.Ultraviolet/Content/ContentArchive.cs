using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// Represents an archive containing multiple content files.
    /// </summary>
    public sealed class ContentArchive : IDisposable, IEnumerable<ContentArchiveNode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentArchive"/> class.
        /// </summary>
        /// <param name="roots">A collection containing the archive's root nodes.</param>
        private ContentArchive(IEnumerable<ContentArchiveNode> roots)
        {
            this.isReadOnly = false;
            this.stream     = null;
            this.reader     = null;
            this.roots      = new List<ContentArchiveNode>(roots);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentArchive"/> class.
        /// </summary>
        /// <param name="stream">The stream from which to load the content archive.</param>
        private ContentArchive(Stream stream)
        {
            this.isReadOnly = true;
            this.stream     = stream;
            this.reader     = new BinaryReader(stream);

            var fileHeader = reader.ReadString();
            if (fileHeader != "UVARC0")
                throw new InvalidDataException("TODO");

            var rootCount = reader.ReadInt32();
            for (int i = 0; i < rootCount; i++)
            {
                var root = ContentArchiveNode.FromArchive(reader);
                roots.Add(root);
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="ContentArchive"/> from the specified set of directories.
        /// </summary>
        /// <param name="directories">A collection of directories from which to create the content archive.</param>
        /// <returns>The <see cref="ContentArchive"/> that was created.</returns>
        public static ContentArchive FromFileSystem(IEnumerable<String> directories)
        {
            Contract.Require(directories, "directories");

            return new ContentArchive(directories.Select(x => ContentArchiveNode.FromFileSystem(x)));
        }

        /// <summary>
        /// Creates a new instance of <see cref="ContentArchive"/> from the specified stream.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> that contains the archive data.</param>
        /// <returns>The <see cref="ContentArchive"/> that was created.</returns>
        public static ContentArchive FromArchiveFile(Stream stream)
        {
            Contract.Require(stream, "stream");

            return new ContentArchive(stream);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Writes the content archive to the specified stream.
        /// </summary>
        /// <param name="writer">A <see cref="BinaryWriter"/> on the stream to which to save the archive.</param>
        public void Save(BinaryWriter writer)
        {
            Contract.Require(writer, "writer");
            Contract.EnsureNot(isReadOnly, "TODO");

            writer.Write("UVARC0");
            writer.Write(roots.Count());

            var position = 0L;

            foreach (var root in roots)
            {
                root.WriteIndex(writer, ref position);
            }

            foreach (var root in roots)
            {
                root.WriteData(writer);
            }
        }

        /// <summary>
        /// Finds the archive node with the specified path.
        /// </summary>
        /// <param name="path">The relative path of the asset to load.</param>
        /// <param name="throwIfNotFound">A value indicating whether to throw a <see cref="FileNotFoundException"/> if the path does not exist.</param>
        /// <returns>The archive node with the specified path, or null if no such node exists and <paramref name="throwIfNotFound"/> is <c>false</c>.</returns>
        public ContentArchiveNode Find(String path, Boolean throwIfNotFound = true)
        {
            Contract.Require(path, "path");

            ContentArchiveNode cachedNode;
            if (pathCache.TryGetValue(path, out cachedNode))
            {
                return cachedNode;
            }

            var components = path.Split(DirectorySeparators);
            var rootname   = components.First();
            var root       = roots.SingleOrDefault(x => String.Equals(x.Name, rootname, StringComparison.InvariantCultureIgnoreCase));

            if (root == null)
            {
                if (throwIfNotFound)
                {
                    throw new DirectoryNotFoundException(path);
                }
                return null;
            }

            var current = root;
            for (var i = 1; i < components.Length; i++)
            {
                var component = components[i];

                if (component == ".")
                    continue;

                if (component == "..")
                {
                    current = current.Parent;
                    if (current == null)
                    {
                        if (throwIfNotFound)
                        {
                            throw new FileNotFoundException(path);
                        }
                    }
                    continue;
                }

                var match = current.Children.SingleOrDefault(x => String.Equals(x.Name, component, StringComparison.InvariantCultureIgnoreCase));
                if (match == null)
                {
                    if (throwIfNotFound)
                    {
                        throw new FileNotFoundException(path);
                    }
                    return null;
                }
                current = match;
            }

            pathCache[path] = current;
            return current;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="ContentArchive"/>.
        /// </summary>
        /// <returns>An enumerator that iterates through the <see cref="ContentArchive"/>.</returns>
        List<ContentArchiveNode>.Enumerator GetEnumerator()
        {
            return roots.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator<ContentArchiveNode> IEnumerable<ContentArchiveNode>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing"><c>true</c> if the object is being disposed; <c>false</c> if the object is being finalized.</param>
        private void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                SafeDispose.Dispose(reader);
                SafeDispose.Dispose(stream);
            }
        }

        // Property values.
        private Boolean isReadOnly;

        // The archive's data stream.
        private readonly Stream stream;
        private readonly BinaryReader reader;

        // The archive's root nodes.
        private readonly List<ContentArchiveNode> roots = 
            new List<ContentArchiveNode>();

        // The archive's path cache.
        private readonly Dictionary<String, ContentArchiveNode> pathCache = 
            new Dictionary<String, ContentArchiveNode>();

        // The set of characters used to delimit directories in a path.
        private static readonly Char[] DirectorySeparators = new[] { 
            Path.DirectorySeparatorChar, 
            Path.AltDirectorySeparatorChar };
    }
}
