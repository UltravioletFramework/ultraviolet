using System;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.UI
{
    /// <summary>
    /// Represents a <see cref="UIPanelDefinition"/> asset which has been loaded for use by a <see cref="UIPanel"/>.
    /// </summary>
    public sealed class UIPanelDefinitionWrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIPanelDefinitionWrapper"/> class.
        /// </summary>
        /// <param name="asset">The <see cref="UIPanelDefinition"/> which is represented by this wrapper.</param>
        public UIPanelDefinitionWrapper(UIPanelDefinition asset)
        {
            this.asset = asset;
            this.lastKnownGoodVersion = asset;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIPanelDefinitionWrapper"/> class.
        /// </summary>
        /// <param name="asset">The <see cref="WatchedAsset{UIPanelDefinitionAsset}"/> which is represented by this wrapper.</param>
        public UIPanelDefinitionWrapper(WatchedAsset<UIPanelDefinition> asset)
        {
            this.asset = asset;
            this.lastKnownGoodVersion = asset;
        }

        /// <summary>
        /// Implicitly converts a <see cref="UIPanelDefinitionWrapper"/> to its underlying value.
        /// </summary>
        /// <param name="wrapper">The <see cref="UIPanelDefinition"/> which is represented by the specified wrapper.</param>
        public static implicit operator UIPanelDefinition(UIPanelDefinitionWrapper wrapper) => wrapper.KnownGoodValue;

        /// <summary>
        /// Retrieves the <see cref="UIPanelDefinition"/> which is represented by this wrapper.
        /// </summary>
        public UIPanelDefinition Value
        {
            get { return KnownGoodValue; }
        }

        /// <summary>
        /// Gets a value indicating whether this wrapper contains a watched asset.
        /// </summary>
        public Boolean IsWatched
        {
            get { return asset is WatchedAsset<UIPanelDefinition>; }
        }

        /// <summary>
        /// Gets a value indicating whether this wrapper contains a valid asset.
        /// </summary>
        public Boolean IsValid
        {
            get { return asset != null; }
        }

        /// <summary>
        /// Marks the current state of as the asset as "known good."
        /// </summary>
        internal void MarkAsKnownGood()
        {
            if (!IsValid)
                return;

            if (asset is UIPanelDefinition)
            {
                lastKnownGoodVersion = (UIPanelDefinition)asset;
            }
            else
            {
                lastKnownGoodVersion = (WatchedAsset<UIPanelDefinition>)asset;
            }
        }

        /// <summary>
        /// Retrieves the <see cref="UIPanelDefinition"/> which is represented by this wrapper.
        /// </summary>
        internal UIPanelDefinition CurrentValue
        {
            get { return (asset is WatchedAsset<UIPanelDefinition>) ? ((WatchedAsset<UIPanelDefinition>)asset).Value : (UIPanelDefinition)asset; }
        }

        /// <summary>
        /// Retrieves the last version of the wrapper's <see cref="UIPanelDefinition"/> which is known to be a valid definition.
        /// </summary>
        internal UIPanelDefinition KnownGoodValue
        {
            get { return lastKnownGoodVersion; }
        }

        // State values.
        private Object asset;
        private UIPanelDefinition lastKnownGoodVersion;
    }
}
