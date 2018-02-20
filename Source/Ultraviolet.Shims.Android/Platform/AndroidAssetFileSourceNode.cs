using System;
using System.Collections.Generic;
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
        internal AndroidAssetFileSourceNode(AndroidAssetFileSourceNode parent, AssetManager assets, String path)
        {
            Contract.Require(assets, nameof(assets));

            this.parent = parent;
            this.assets = assets;

            this.name = System.IO.Path.GetFileName(path);
            this.path = path;

            this.isFile = assets.List(path).Length == 0;
            this.isDirectory = !isFile;            
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
        public override IEnumerable<FileSourceNode> Children
        {
            get
            {
                var children = assets.List(path);
                foreach (var child in children)
                    yield return new AndroidAssetFileSourceNode(this, assets, System.IO.Path.Combine(path, child));
            }
        }

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