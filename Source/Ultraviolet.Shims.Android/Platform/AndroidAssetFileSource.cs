using System;
using System.IO;
using System.Linq;
using Android.Content.Res;
using Ultraviolet.Core;
using Ultraviolet.Platform;

namespace Ultraviolet.Shims.Android.Platform
{
    /// <summary>
    /// Represents a <see cref="FileSource"/> which uses the Android asset system.
    /// </summary>
    [CLSCompliant(false)]
    public class AndroidAssetFileSource : FileSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidAssetFileSource"/> class.
        /// </summary>
        /// <param name="assets">The asset manager.</param>
        public AndroidAssetFileSource(AssetManager assets)
        {
            Contract.Require(assets, nameof(assets));

            this.assets = assets;
            this.root = new AndroidAssetFileSourceNode(null, assets, String.Empty);
        }

        /// <inheritdoc/>
        public override FileSourceNode Find(String path, Boolean throwIfNotFound = true)
        {
            Contract.Require(path, nameof(path));

            if (path.StartsWith("/"))
                path = path.Substring(1);

            var components = path.Split(DirectorySeparators);
            var current = root;
            for (var i = 0; i < components.Length; i++)
            {
                var component = components[i];

                if (component == ".")
                    continue;

                if (component == "..")
                {
                    current = (AndroidAssetFileSourceNode)current.Parent;
                    if (current == null)
                    {
                        if (throwIfNotFound)
                        {
                            throw new FileNotFoundException(path);
                        }
                    }
                    continue;
                }

                var match = (AndroidAssetFileSourceNode)current.Children.SingleOrDefault(x =>
                    String.Equals(x.Name, component, StringComparison.Ordinal));

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

            return current;
        }

        /// <inheritdoc/>
        public override Stream Extract(String path)
        {
            Contract.Require(path, nameof(path));

            using (var stream = assets.Open(path))
            {
                var memstream = new MemoryStream();
                stream.CopyTo(memstream);
                memstream.Seek(0, SeekOrigin.Begin);
                return memstream;
            }
        }

        // The asset manager.
        private readonly AssetManager assets;
        private readonly AndroidAssetFileSourceNode root;

        // The set of characters used to delimit directories in a path.
        private static readonly Char[] DirectorySeparators = new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };
    }
}