using System;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents a reference that can either point to a <see cref="WatchedAsset{T}"/> or directly
    /// to an instance of <typeparamref name="T"/> if asset watching is not required.
    /// </summary>
    public partial struct WatchableAssetReference<T> : IEquatable<WatchableAssetReference<T>>, IEquatable<T>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WatchableAssetReference{T}"/> structure.
        /// </summary>
        /// <param name="asset">The watched asset which this reference represents.</param>
        public WatchableAssetReference(WatchedAsset<T> asset) => this.reference = asset;

        /// <summary>
        /// Initializes a new instance of the <see cref="WatchableAssetReference{T}"/> structure.
        /// </summary>
        /// <param name="asset">The asset which this reference represents.</param>
        public WatchableAssetReference(T asset) => this.reference = asset;

        /// <summary>
        /// Implicitly converts a <see cref="WatchableAssetReference{T}"/> to its underlying asset value.
        /// </summary>
        /// <param name="war">The <see cref="WatchableAssetReference{T}"/> to convert.</param>
        public static implicit operator T(WatchableAssetReference<T> war) => war.Value;

        /// <summary>
        /// Implicitly converts a <see cref="WatchedAsset{T}"/> to a <see cref="WatchableAssetReference{T}"/> structure.
        /// </summary>
        /// <param name="wa">The <see cref="WatchedAsset{T}"/> to convert.</param>
        public static implicit operator WatchableAssetReference<T>(WatchedAsset<T> wa) => new WatchableAssetReference<T>(wa);

        /// <summary>
        /// Implicitly converts an asset to a <see cref="WatchableAssetReference{T}"/> structure.
        /// </summary>
        /// <param name="asset">The asset to convert.</param>
        public static implicit operator WatchableAssetReference<T>(T asset) => new WatchableAssetReference<T>(asset);

        /// <summary>
        /// Retrieves a <see cref="WatchableAssetReference{T}"/> which is equivalent to a null reference.
        /// </summary>
        public static WatchableAssetReference<T> Null => new WatchableAssetReference<T>(null);

        /// <summary>
        /// Gets the asset which is referenced by this structure.
        /// </summary>
        public T Value => reference is WatchedAsset<T> wa ? wa.Value : (T)reference;

        /// <summary>
        /// Gets a value indicating whether this structure represents a watched asset.
        /// </summary>
        public Boolean IsWatchedAsset => reference is WatchedAsset<T>;

        /// <summary>
        /// Gets a value indicating whether this structure is a direct reference to an asset.
        /// </summary>
        public Boolean IsDirectReference => reference is T;

        /// <summary>
        /// Gets a value indicating whether this structure is a null reference.
        /// </summary>
        public Boolean IsNullReference => reference == null;

        // State values.
        private readonly Object reference;
    }
}
