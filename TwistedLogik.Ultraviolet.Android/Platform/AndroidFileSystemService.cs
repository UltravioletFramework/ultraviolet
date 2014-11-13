using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Android.Content.Res;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.Android.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="FileSystemService"/> class for the Android platform.
    /// </summary>
    public sealed class AndroidFileSystemService : FileSystemService
    {
        /// <inheritdoc/>
        public override Boolean FileExists(String path)
        {
            Contract.RequireNotEmpty(path, "path");

            var node = Find(path, false);
            return node != null && node.IsFile;
        }

        /// <inheritdoc/>
        public override Boolean DirectoryExists(String path)
        {
            Contract.RequireNotEmpty(path, "path");

            var node = Find(path, false);
            return node != null && node.IsDirectory;
        }

        /// <inheritdoc/>
        public override IEnumerable<String> ListFiles(String path, String searchPattern)
        {
            Contract.RequireNotEmpty(path, "path");

            var candidates = Find(path).Children.Where(x => x.IsFile);
            return PatternMatch(candidates, searchPattern).Select(x => x.Path).ToList();
        }

        /// <inheritdoc/>
        public override IEnumerable<String> ListDirectories(String path, String searchPattern)
        {
            Contract.RequireNotEmpty(path, "path");

            var candidates = Find(path).Children.Where(x => x.IsDirectory);
            return PatternMatch(candidates, searchPattern).Select(x => x.Path).ToList();
        }

        /// <inheritdoc/>
        public override Stream OpenRead(String path)
        {
            Contract.RequireNotEmpty(path, "path");

            return Assets.Open(path);
        }

        /// <summary>
        /// Gets the application's asset manager.
        /// </summary>
        public static AssetManager Assets
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets a value indicating whether the specified asset path represents a file.
        /// </summary>
        /// <param name="path">The path to evaluate.</param>
        /// <returns><c>true</c> if the specified path represents a file; otherwise, <c>false</c>.</returns>
        private Boolean IsFile(String path)
        {
            return Find(path).IsFile;
        }

        /// <summary>
        /// Gets a value indicating whether the specified asset path represents a directory.
        /// </summary>
        /// <param name="path">The path to evaluate.</param>
        /// <returns><c>true</c> if the specified path represents a directory; otherwise, <c>false</c>.</returns>
        private Boolean IsDirectory(String path)
        {
            return Find(path).IsDirectory;
        }

        /// <summary>
        /// Performs pattern matching on the specified collection of file system nodes.
        /// </summary>
        /// <param name="nodes">The collection of file system nodes on which to perform pattern matching.</param>
        /// <param name="searchPattern">The search string to match against the names of nodes in <paramref name="nodes"/>.</param>
        /// <returns>The collection of nodes which match the specified pattern.</returns>
        private IEnumerable<FileSystemNode> PatternMatch(IEnumerable<FileSystemNode> nodes, String searchPattern)
        {
            if (searchPattern == "*")
            {
                return nodes;
            }
            if (searchPattern.StartsWith("*.") && searchPattern.Count(x => x == '*') == 1)
            {
                var extension = searchPattern.Substring(1);
                return nodes.Where(x => System.IO.Path.GetExtension(x.Name) == extension);
            }
            var regex = new Regex(searchPattern.Replace("*", ".*"));
            return nodes.Where(x => regex.IsMatch(x.Name));
        }

        /// <summary>
        /// Finds the file system node with the specified relative path.
        /// </summary>
        /// <param name="path">The relative path for which to search.</param>
        /// <param name="throwIfNotFound">A value indicating whether to throw an exception if the file is not found.</param>
        /// <returns>The file system node that corresponds to the specified path.</returns>
        private FileSystemNode Find(String path, Boolean throwIfNotFound = true)
        {
            FileSystemNode node;
            if (!fileSystemCache.TryGetValue(path, out node))
            {
                node = FileSystemRoot.Find(path, throwIfNotFound);
                fileSystemCache[path] = node;
            }
            return node;
        }

        /// <summary>
        /// Gets the root node of the "file system" that represents the Android package's assets.
        /// </summary>
        private FileSystemNode FileSystemRoot
        {
            get
            {
                if (fileSystemRoot == null)
                {
                    if (Assets == null)
                    {
                        // NOTE: No need to localize this because we can't access our localization files!
                        throw new InvalidOperationException("No valid AssetManager instance.");
                    }
                    fileSystemRoot = new FileSystemNode(Assets, null, String.Empty);
                }
                return fileSystemRoot;
            }
        }

        // State values.
        private FileSystemNode fileSystemRoot;
        private static readonly Dictionary<String, FileSystemNode> fileSystemCache = 
            new Dictionary<String, FileSystemNode>();
    }
}
