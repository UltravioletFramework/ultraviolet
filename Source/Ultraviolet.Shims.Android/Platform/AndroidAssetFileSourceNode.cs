using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content.Res;
using Ultraviolet.Core;
using Ultraviolet.Platform;

namespace Ultraviolet.Shims.Android.Platform
{
    /// <summary>
    /// Represents a <see cref="FileSourceNode"/> which encapsulates an Android asset.
    /// </summary>
    public class AndroidAssetFileSourceNode : FileSourceNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidAssetFileSourceNode"/> class.
        /// </summary>
        /// <param name="parent">The node's parent node.</param>
        /// <param name="assets">The asset manager.</param>
        /// <param name="path">The path to the asset which this node represents.</param>
        /// <param name="isFile">A value indicating whether this node represents a file.</param>
        /// <param name="aalist">The precompiled list of Android assets which should be used to determine which assets are available.</param>
        internal AndroidAssetFileSourceNode(AndroidAssetFileSourceNode parent, AssetManager assets, String path, Boolean? isFile, Dictionary<String, List<AndroidAssetInfo>> aalist = null)
        {
            Contract.Require(assets, nameof(assets));

            this.parent = parent;
            this.assets = assets;

            this.name = System.IO.Path.GetFileName(path);
            this.path = path;

            if (aalist != null)
            {
                if (aalist.TryGetValue(path, out var aalistEntry))
                {
                    this.Children = aalistEntry.Select(x =>
                        new AndroidAssetFileSourceNode(this, assets, System.IO.Path.Combine(path, x.Name), x.IsFile, aalist)).ToList();
                }
                else
                {
                    this.Children = Enumerable.Empty<AndroidAssetFileSourceNode>();
                }
            }
            else
            {
                var childAssets = assets.List(path);
                isFile = (childAssets.Length == 0);

                this.Children = childAssets.Select(x =>
                    new AndroidAssetFileSourceNode(this, assets, System.IO.Path.Combine(path, x), null, aalist)).ToList();
            }

            this.isFile = isFile ?? false;
            this.isDirectory = !this.isFile;
        }

        /// <inheritdoc/>
        public override FileSourceNode Parent => parent;

        /// <inheritdoc/>
        public override String Path => path;

        /// <inheritdoc/>
        public override String Name => name;

        /// <inheritdoc/>
        public override Boolean IsFile => isFile;

        /// <inheritdoc/>
        public override Boolean IsDirectory => isDirectory;

        /// <inheritdoc/>
        public override Int64 Position => throw new NotSupportedException();

        /// <inheritdoc/>
        public override Int64 SizeInBytes => throw new NotSupportedException();

        /// <inheritdoc/>
        public override IEnumerable<FileSourceNode> Children { get; }

        // Property values.
        private readonly AndroidAssetFileSourceNode parent;
        private readonly String name;
        private readonly String path;
        private readonly Boolean isFile;
        private readonly Boolean isDirectory;

        // The asset manager that contains this asset.
        private readonly AssetManager assets;
    }
}