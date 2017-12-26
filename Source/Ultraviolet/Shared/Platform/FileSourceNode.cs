using System;
using System.Collections.Generic;

namespace Ultraviolet.Platform
{
    /// <summary>
    /// Represents a file or directory in a file source tree.
    /// </summary>
    public abstract class FileSourceNode
    {
        /// <summary>
        /// Gets the node's parent node.
        /// </summary>
        public abstract FileSourceNode Parent
        {
            get;
        }

        /// <summary>
        /// Gets the file's path within its source.
        /// </summary>
        public abstract String Path
        {
            get;
        }

        /// <summary>
        /// Gets the node's name.
        /// </summary>
        public abstract String Name
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the node represents a file.
        /// </summary>
        public abstract Boolean IsFile
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the node represents a directory.
        /// </summary>
        public abstract Boolean IsDirectory
        {
            get;
        }

        /// <summary>
        /// Gets the position of the node's data within the archive's data block.
        /// </summary>
        public abstract Int64 Position
        {
            get;
        }

        /// <summary>
        /// Gets the node's size in bytes.
        /// </summary>
        public abstract Int64 SizeInBytes
        {
            get;
        }

        /// <summary>
        /// Gets the archive node's child nodes.
        /// </summary>
        public abstract IEnumerable<FileSourceNode> Children
        {
            get;
        }
    }
}
