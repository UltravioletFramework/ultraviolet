using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Platform
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="FileSystemService"/> class.
    /// </summary>
    /// <returns>The instance of <see cref="FileSystemService"/> that was created.</returns>
    public delegate FileSystemService FileSystemServiceFactory();

    /// <summary>
    /// Contains methods for interacting with the file system.
    /// </summary>
    public class FileSystemService
    {
        /// <summary>
        /// Creates a new instance of the <see cref="FileSystemService"/> class.
        /// </summary>
        /// <returns>The instance of <see cref="FileSystemService"/> that was created.</returns>
        public static FileSystemService Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<FileSystemServiceFactory>()();
        }

        /// <summary>
        /// Gets the current working directory.
        /// </summary>
        /// <returns>The current working directory.</returns>
        public virtual String GetCurrentDirectory()
        {
            if (Source != null)
            {
                return String.Empty;
            }
            return Directory.GetCurrentDirectory();
        }

        /// <summary>
        /// Gets a value indicating whether the specified path exists and is a file.
        /// </summary>
        /// <param name="path">The path to evaluate.</param>
        /// <returns><c>true</c> if the specified path exists and is a file; otherwise, <c>false</c>.</returns>
        public virtual Boolean FileExists(String path)
        {
            Contract.Require(path, "path");

            if (source != null)
            {
                var node = source.Find(path, false);
                return node != null && node.IsFile;
            }
            return File.Exists(path);
        }

        /// <summary>
        /// Gets a value indicating whether the specified path exists and is a directory.
        /// </summary>
        /// <param name="path">The path to evaluate.</param>
        /// <returns><c>true</c> if the specified path exists and is a directory; otherwise, <c>false</c>.</returns>
        public virtual Boolean DirectoryExists(String path)
        {
            Contract.Require(path, "path");

            if (source != null)
            {
                var node = source.Find(path, false);
                return node != null && node.IsDirectory;
            }
            return Directory.Exists(path);
        }

        /// <summary>
        /// Lists the files at the specified path.
        /// </summary>
        /// <param name="path">The path to evaluate.</param>
        /// <param name="searchPattern">The search string to match against the names of files in <paramref name="path"/>.</param>
        /// <returns>A list of directories at the specified path.</returns>
        public virtual IEnumerable<String> ListFiles(String path, String searchPattern = "*")
        {
            Contract.Require(path, "path");
            Contract.Require(searchPattern, "searchPattern");

            if (source != null)
            {
                var node = source.Find(path, false);
                var files = node.Children.Where(x => x.IsFile);
                return PatternMatch(files, searchPattern).Select(x => x.Path);
            }
            return Directory.GetFiles(path, searchPattern);
        }

        /// <summary>
        /// Lists the directories at the specified path.
        /// </summary>
        /// <param name="path">The path to evaluate.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>.</param>
        /// <returns>A list of directories at the specified path.</returns>
        public virtual IEnumerable<String> ListDirectories(String path, String searchPattern = "*")
        {
            Contract.Require(path, "path");
            Contract.Require(searchPattern, "searchPattern");

            if (source != null)
            {
                var node = source.Find(path, false);
                var dirs = node.Children.Where(x => x.IsDirectory);
                return PatternMatch(dirs, searchPattern).Select(x => x.Path);
            }
            return Directory.GetDirectories(path, searchPattern);
        }

        /// <summary>
        /// Opens the specified file for reading.
        /// </summary>
        /// <param name="path">The path of the file to open.</param>
        /// <returns>A <see cref="Stream"/> on the file at <paramref name="path"/>.</returns>
        public virtual Stream OpenRead(String path)
        {
            Contract.Require(path, "path");

            if (source != null)
            {
                return source.Extract(path);
            }
            return File.OpenRead(path);
        }

        /// <summary>
        /// Gets or sets the file source. If no source is set, Ultraviolet will attempt to read
        /// files directly from the underlying file system where possible.
        /// </summary>
        public static FileSource Source
        {
            get { return source; }
            set { source = value; }
        }

        /// <summary>
        /// Performs pattern matching on the specified collection of file source nodes.
        /// </summary>
        /// <param name="nodes">The collection of file source nodes on which to perform pattern matching.</param>
        /// <param name="searchPattern">The search string to match against the names of nodes in <paramref name="nodes"/>.</param>
        /// <returns>The collection of nodes which match the specified pattern.</returns>
        protected IEnumerable<FileSourceNode> PatternMatch(IEnumerable<FileSourceNode> nodes, String searchPattern)
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
            var regex = new Regex(searchPattern
                .Replace(".", "\\.")
                .Replace("*", ".*"));
            return nodes.Where(x => regex.IsMatch(x.Name));
        }

        // State values.
        private static FileSource source;
    }
}
