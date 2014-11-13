using System;
using System.Collections.Generic;
using Android.Content.Res;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Android.Platform
{
    /// <summary>
    /// Represents either a file or a directory in the "file system" that represents the Android package's assets.
    /// </summary>
    internal sealed class FileSystemNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemNode"/> class.
        /// </summary>
        /// <param name="assets">The asset manager for the application.</param>
        /// <param name="parent">The node's parent node.</param>
        /// <param name="path">The path to the node.</param>
        public FileSystemNode(AssetManager assets, FileSystemNode parent, String path)
        {
            Contract.Require(assets, "assets");
            Contract.Require(path, "path");

            this.assets = assets;
            this.parent = parent;
            this.name   = System.IO.Path.GetFileName(path);
            this.path   = path;
            this.isFile = (assets.List(path).Length == 0);
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            return path;
        }

        /// <summary>
        /// Finds the file system node with the specified relative path.
        /// </summary>
        /// <param name="path">The relative path for which to search.</param>
        /// <param name="throwIfNotFound">A value indicating whether to throw an exception if the file is not found.</param>
        /// <returns>The file system node that corresponds to the specified path.</returns>
        public FileSystemNode Find(String path, Boolean throwIfNotFound)
        {
            Contract.RequireNotEmpty(path, "path");

            var components = path.Split(DirectorySeparators);
            var current    = this;

            foreach (var component in components)
            {
                if (component == ".")
                    continue;

                if (component == "..")
                {
                    current = current.Parent;
                    continue;
                }

                var found = false;

                foreach (var child in current.Children)
                {
                    if (String.Equals(child.Name, component, StringComparison.InvariantCultureIgnoreCase))
                    {
                        current = child;
                        found   = true;
                        break;
                    }
                }

                if (!found)
                {
                    if (throwIfNotFound)
                    {
                        throw new System.IO.FileNotFoundException(path);
                    }
                    return null;
                }
            }

            return current;
        }

        /// <summary>
        /// Gets the asset manager for the application.
        /// </summary>
        public AssetManager Assets
        {
            get { return assets; }
        }

        /// <summary>
        /// Gets the node's parent node.
        /// </summary>
        public FileSystemNode Parent
        {
            get { return parent; }
        }

        /// <summary>
        /// Gets the node's name.
        /// </summary>
        public String Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the node's full path.
        /// </summary>
        public String Path
        {
            get { return path; }
        }

        /// <summary>
        /// Gets a value indicating whether the node represents a file.
        /// </summary>
        public Boolean IsFile
        {
            get { return isFile; }
        }

        /// <summary>
        /// Gets a value indicating whether the node represents a directory.
        /// </summary>
        public Boolean IsDirectory
        {
            get { return !isFile; }
        }

        /// <summary>
        /// Gets the node's collection of child nodes.
        /// </summary>
        public IEnumerable<FileSystemNode> Children
        {
            get
            {
                if (children == null)
                {
                    LoadChildren();
                }
                return children;
            }
        }

        /// <summary>
        /// Loads the node's child nodes.
        /// </summary>
        private void LoadChildren()
        {
            var list     = new List<FileSystemNode>();
            var children = Assets.List(path);

            foreach (var child in children)
            {
                var node = new FileSystemNode(assets, this, System.IO.Path.Combine(Path, child));
                list.Add(node);
            }

            this.children = list;
        }

        // Property values.
        private readonly AssetManager assets;
        private readonly FileSystemNode parent;
        private readonly String name;
        private readonly String path;
        private readonly Boolean isFile;
        private IEnumerable<FileSystemNode> children;

        // The list of characters used to separate directories in paths.
        private static readonly Char[] DirectorySeparators = new[] { 
            System.IO.Path.DirectorySeparatorChar, 
            System.IO.Path.AltDirectorySeparatorChar };
    }
}