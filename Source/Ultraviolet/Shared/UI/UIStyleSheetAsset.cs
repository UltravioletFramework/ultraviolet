using System;
using Ultraviolet.Core;

namespace Ultraviolet.UI
{
    /// <summary>
    /// Represents the source code for a particular style sheet.
    /// </summary>
    public sealed class UIStyleSheetAsset
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIStyleSheetAsset"/> class.
        /// </summary>
        /// <param name="asset">The path to the asset which is dependent on the style sheet.</param>
        public UIStyleSheetAsset(String asset)
        {
            Contract.Require(asset, nameof(asset));

            this.Asset = asset;
        }

        /// <summary>
        /// Gets the path to the asset which is dependent on the style sheet.
        /// </summary>
        public String Asset { get; }
    }
}
