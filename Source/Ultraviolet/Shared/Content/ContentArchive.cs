using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ultraviolet.Core;
using Ultraviolet.Platform;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents an archive containing multiple content files.
    /// </summary>
    public sealed class ContentArchive : FileSource, IEnumerable<ContentArchiveNode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentArchive"/> class.
        /// </summary>
        /// <param name="roots">A collection containing the archive's root nodes.</param>
        private ContentArchive(IEnumerable<ContentArchiveNode> roots)
        {
            this.canSave    = true;
            this.canExtract = false;
            this.roots      = new List<ContentArchiveNode>(roots);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentArchive"/> class.
        /// </summary>
        /// <param name="loader">A function which opens a stream into the archive data.</param>
        private ContentArchive(Func<Stream> loader)
        {
            this.canSave    = false;
            this.canExtract = true;
            this.loader     = loader;

            using (var stream = loader())
            {
                using (var reader = new BinaryReader(stream))
                {
                    var fileHeader = reader.ReadString();
                    if (fileHeader != "UVARC0")
                        throw new InvalidDataException(UltravioletStrings.InvalidContentArchive);

                    var rootCount = reader.ReadInt32();
                    for (int i = 0; i < rootCount; i++)
                    {
                        var root = ContentArchiveNode.FromArchive(reader);
                        roots.Add(root);
                    }

                    dataBlockPosition = reader.BaseStream.Position;
                }
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="ContentArchive"/> from the specified set of directories.
        /// </summary>
        /// <param name="directories">A collection of directories from which to create the content archive.</param>
        /// <returns>The <see cref="ContentArchive"/> that was created.</returns>
        public static ContentArchive FromFileSystem(IEnumerable<String> directories)
        {
            Contract.Require(directories, nameof(directories));

            return new ContentArchive(directories.Select(x => ContentArchiveNode.FromFileSystem(x)));
        }

        /// <summary>
        /// Creates a new instance of <see cref="ContentArchive"/> from the specified stream.
        /// </summary>
        /// <param name="loader">A function which opens a stream into the archive data.</param>
        /// <returns>The <see cref="ContentArchive"/> that was created.</returns>
        public static ContentArchive FromArchiveFile(Func<Stream> loader)
        {
            Contract.Require(loader, nameof(loader));

            return new ContentArchive(loader);
        }

        /// <summary>
        /// Writes the content archive to the specified stream.
        /// </summary>
        /// <param name="writer">A <see cref="BinaryWriter"/> on the stream to which to save the archive.</param>
        public void Save(BinaryWriter writer)
        {
            Contract.Require(writer, nameof(writer));
            Contract.Ensure<NotSupportedException>(canSave);

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

        /// <inheritdoc/>
        public override FileSourceNode Find(String path, Boolean throwIfNotFound = true)
        {
            Contract.Require(path, nameof(path));

            if (path.StartsWith("/"))
            {
                path = path.Substring(1);
            }

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
                    current = (ContentArchiveNode)current.Parent;
                    if (current == null)
                    {
                        if (throwIfNotFound)
                        {
                            throw new FileNotFoundException(path);
                        }
                    }
                    continue;
                }

                var match = (ContentArchiveNode)current.Children.SingleOrDefault(x => 
                    String.Equals(x.Name, component, StringComparison.InvariantCultureIgnoreCase));

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

        /// <inheritdoc/>
        public override Stream Extract(String path)
        {
            Contract.Require(path, nameof(path));
            Contract.Ensure<NotSupportedException>(canExtract);

            var node = Find(path);

            var stream = loader();
            stream.Seek(node.Position, SeekOrigin.Begin);

            return new ContentArchiveStream(stream, dataBlockPosition + node.Position, node.SizeInBytes);
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

        // State values.
        private Func<Stream> loader;
        private Boolean canSave;
        private Boolean canExtract;
        private Int64 dataBlockPosition;

        // The archive's root nodes.
        private readonly List<ContentArchiveNode> roots = 
            new List<ContentArchiveNode>();

        // The archive's path cache.
        private readonly ConcurrentDictionary<String, ContentArchiveNode> pathCache = 
            new ConcurrentDictionary<String, ContentArchiveNode>();

        // The set of characters used to delimit directories in a path.
        private static readonly Char[] DirectorySeparators = new[] { 
            Path.DirectorySeparatorChar, 
            Path.AltDirectorySeparatorChar };
    }
}
