using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ultraviolet.Core;
using Ultraviolet.Platform;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents a file or directory in an Ultraviolet content archive.
    /// </summary>
    public sealed class ContentArchiveNode : FileSourceNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentArchiveNode"/> class from a file or directory in the file system.
        /// </summary>
        /// <param name="parent">The node's parent node.</param>
        /// <param name="path">The path to the file or directory that this node represents.</param>
        private ContentArchiveNode(ContentArchiveNode parent, String path)
        {
            path = System.IO.Path.GetFullPath(path);

            this.parent      = parent;
            this.path        = path;
            this.name        = System.IO.Path.GetFileName(path);
            this.isFile      = File.Exists(path);
            this.isDirectory = Directory.Exists(path);

            if (!isFile && !isDirectory)
                throw new FileNotFoundException(path);

            if (isFile)
            {
                using (var stream = File.OpenRead(path))
                {
                    this.sizeInBytes = stream.Length;
                }
            }
            else
            {
                LoadChildren(path);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentArchiveNode"/> class from an archive stream.
        /// </summary>
        /// <param name="parent">The node's parent node.</param>
        /// <param name="reader">A <see cref="BinaryReader"/> on the stream containing the archive data.</param>
        private ContentArchiveNode(ContentArchiveNode parent, BinaryReader reader)
        {
            this.parent      = parent;
            this.name        = reader.ReadString();
            this.path        = BuildPath(parent, name);
            this.isFile      = reader.ReadBoolean();
            this.isDirectory = !this.isFile;

            if (this.isFile)
            {
                this.position    = reader.ReadInt64();
                this.sizeInBytes = reader.ReadInt64();
            }
            else
            {
                var childList = new List<ContentArchiveNode>();
                var childCount = reader.ReadInt32();

                for (int i = 0; i < childCount; i++)
                {
                    var node = new ContentArchiveNode(this, reader);
                    childList.Add(node);
                }

                this.children = childList;
            }
        }

        /// <summary>
        /// Creates an archive tree that represents the specified path in the file system.
        /// </summary>
        /// <param name="path">The path for which to build an archive tree.</param>
        /// <returns>An <see cref="ContentArchiveNode"/> that represents the root of the constructed archive tree.</returns>
        internal static ContentArchiveNode FromFileSystem(String path)
        {
            Contract.RequireNotEmpty(path, nameof(path));

            return new ContentArchiveNode(null, path);
        }

        /// <summary>
        /// Creates an archive tree that represents the data contained in the specified archive stream.
        /// </summary>
        /// <param name="reader">A <see cref="BinaryReader"/> on the stream that contains the archive data.</param>
        /// <returns>An <see cref="ContentArchiveNode"/> that represents the root of the constructed archive tree.</returns>
        internal static ContentArchiveNode FromArchive(BinaryReader reader)
        {
            Contract.Require(reader, nameof(reader));

            return new ContentArchiveNode(null, reader);
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            return name;
        }

        /// <summary>
        /// Writes the node's index data, and the index data of its descendants, to the specified stream.
        /// </summary>
        /// <param name="writer">A <see cref="BinaryWriter"/> on the stream to which to write the node data.</param>
        /// <param name="position">The position of the first node's data within the stream.</param>
        public void WriteIndex(BinaryWriter writer, ref Int64 position)
        {
            Contract.Require(writer, nameof(writer));

            WriteIndexInternal(writer, ref position);
        }

        /// <summary>
        /// Serializes the node and its descendants to the specified stream.
        /// </summary>
        /// <param name="writer">A <see cref="BinaryWriter"/> on the stream to which to serialize the node.</param>
        public void WriteData(BinaryWriter writer)
        {
            Contract.Require(writer, nameof(writer));
            Contract.Ensure<NotSupportedException>(path != null);

            if (IsFile)
            {
                var buffer = new Byte[1024 * 10];

                using (var stream = File.OpenRead(path))
                {
                    while (true)
                    {
                        var read = stream.Read(buffer, 0, buffer.Length);
                        if (read > 0)
                            writer.Write(buffer, 0, read);

                        if (read < buffer.Length)
                            break;
                    }
                }
            }

            foreach (ContentArchiveNode child in Children)
            {
                child.WriteData(writer);
            }
        }

        /// <inheritdoc/>
        public override FileSourceNode Parent
        {
            get { return parent; }
        }

        /// <summary>
        /// Gets the file's path within its source.
        /// </summary>
        public override String Path
        {
            get
            {
                return path;
            }
        }

        /// <inheritdoc/>
        public override String Name
        {
            get
            {
                return name;
            }
        }

        /// <inheritdoc/>
        public override Boolean IsFile
        {
            get
            {
                return isFile;
            }
        }

        /// <inheritdoc/>
        public override Boolean IsDirectory
        {
            get
            {
                return isDirectory;
            }
        }

        /// <inheritdoc/>
        public override Int64 Position
        {
            get { return position; }
        }

        /// <inheritdoc/>
        public override Int64 SizeInBytes
        {
            get
            {
                return sizeInBytes;
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<FileSourceNode> Children
        {
            get
            {
                return children ?? Enumerable.Empty<ContentArchiveNode>();
            }
        }

        /// <summary>
        /// Builds an asset path for a node.
        /// </summary>
        /// <param name="parent">The node's parent node.</param>
        /// <param name="name">The node's name.</param>
        /// <returns>The asset path for a node with the specified parameters.</returns>
        private static String BuildPath(ContentArchiveNode parent, String name)
        {
            var builder = new StringBuilder(name);
            var current = parent;
            while (current != null)
            {
                builder.Insert(0, '/');
                builder.Insert(0, current.Name);

                current = (ContentArchiveNode)current.Parent;
            }
            return builder.ToString();
        }

        /// <summary>
        /// Loads the node's child nodes from the file system.
        /// </summary>
        /// <param name="path">The full path that the node represents.</param>
        private void LoadChildren(String path)
        {
            var list = new List<ContentArchiveNode>();

            foreach (var directory in Directory.GetDirectories(path))
            {
                var node = new ContentArchiveNode(this, directory);
                list.Add(node);
            }

            foreach (var file in Directory.GetFiles(path))
            {
                var node = new ContentArchiveNode(this, file);
                list.Add(node);
            }
            
            this.children = list;
        }

        /// <summary>
        /// Writes the 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="position"></param>
        private void WriteIndexInternal(BinaryWriter writer, ref Int64 position)
        {
            writer.Write(this.Name);
            writer.Write(this.IsFile);

            if (this.IsFile)
            {
                writer.Write(position);
                writer.Write(this.SizeInBytes);

                position += this.SizeInBytes;
            }
            else
            {
                writer.Write(this.Children.Count());
                foreach (ContentArchiveNode child in children)
                {
                    child.WriteIndexInternal(writer, ref position);
                }
            }
        }

        // Property values.
        private readonly FileSourceNode parent;
        private readonly String path;
        private readonly String name;
        private readonly Boolean isFile;
        private readonly Boolean isDirectory;
        private readonly Int64 position;
        private readonly Int64 sizeInBytes;
        private IEnumerable<FileSourceNode> children;
    }
}
