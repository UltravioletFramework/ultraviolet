using System;
using TwistedLogik.Nucleus.Data;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents an asset which can be loaded from either the global or local content source.
    /// </summary>
    /// <typeparam name="T">The type of asset which this object represents.</typeparam>
    public struct SourcedResource<T> : IEquatable<SourcedResource<T>>, IInterpolatable<SourcedResource<T>> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SourcedResource{T}"/> structure.
        /// </summary>
        /// <param name="asset">The asset identifier of the resource.</param>
        /// <param name="source">An <see cref="AssetSource"/> value describing how to load the resource.</param>
        public SourcedResource(AssetID asset, AssetSource source)
        {
            this.resource = new FrameworkResource<T>();
            this.asset    = asset;
            this.source   = source;
        }

        /// <summary>
        /// Implicitly converts a sourced asset to its underlying value.
        /// </summary>
        /// <param name="sourced">The <see cref="SourcedResource{T}"/> to convert.</param>
        /// <returns>The underlying value of the sourced asset.</returns>
        public static implicit operator T(SourcedResource<T> sourced)
        {
            return sourced.Resource;
        }

        /// <summary>
        /// Returns <c>true</c> if the specified sourced assets are equal.
        /// </summary>
        /// <param name="id1">The first <see cref="SourcedResource{T}"/> to compare.</param>
        /// <param name="id2">The second <see cref="SourcedResource{T}"/> to compare.</param>
        /// <returns><c>true</c> if the specified sourced assets are equal; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(SourcedResource<T> id1, SourcedResource<T> id2)
        {
            return id1.Equals(id2);
        }

        /// <summary>
        /// Returns <c>true</c> if the specified sourced assets are not equal.
        /// </summary>
        /// <param name="id1">The first <see cref="SourcedResource{T}"/> to compare.</param>
        /// <param name="id2">The second <see cref="SourcedResource{T}"/> to compare.</param>
        /// <returns><c>true</c> if the specified sourced assets are unequal; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(SourcedResource<T> id1, SourcedResource<T> id2)
        {
            return !id1.Equals(id2);
        }

        /// <summary>
        /// Parses a string into an instance of <see cref="SourcedResource{T}"/>.
        /// </summary>
        /// <param name="str">The string to parse.</param>
        /// <returns>The <see cref="SourcedResource{T}"/> instance that was created from the specified string.</returns>
        public static SourcedResource<T> Parse(String str)
        {
            return Parse(str, null);
        }

        /// <summary>
        /// Parses a string into an instance of <see cref="SourcedResource{T}"/>.
        /// </summary>
        /// <param name="str">The string to parse.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>The <see cref="SourcedResource{T}"/> instance that was created from the specified string.</returns>
        public static SourcedResource<T> Parse(String str, IFormatProvider provider)
        {
            var source = AssetSource.Global;

            if (str.EndsWith(" local"))
            {
                source = AssetSource.Local;
                str    = str.Substring(0, str.Length - " local".Length);
            }
            else if (str.EndsWith(" global"))
            {
                source = AssetSource.Global;
                str    = str.Substring(0, str.Length - " global".Length);
            }

            var asset = (AssetID)ObjectResolver.FromString(str.Trim(), typeof(AssetID), provider);
            return new SourcedResource<T>(asset, source);
        }

        /// <summary>
        /// Loads the resource using the specified content manager.
        /// </summary>
        /// <param name="contentManager">The <see cref="ContentManager"/> with which to load the resource.</param>
        public void Load(ContentManager contentManager)
        {
            resource.Load(contentManager, asset);
        }

        /// <inheritdoc/>
        public SourcedResource<T> Interpolate(SourcedResource<T> target, Single t)
        {
            return (t >= 1) ? target : this;
        }

        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + resource.GetHashCode();
                hash = hash * 23 + source.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            return String.Format("{0} {1}", resource, source.ToString().ToLowerInvariant());
        }

        /// <inheritdoc/>
        public override Boolean Equals(Object obj)
        {
            return obj is SourcedResource<T> && Equals((SourcedResource<T>)obj);
        }

        /// <inheritdoc/>
        public Boolean Equals(SourcedResource<T> other)
        {
            return
                this.resource == other.resource &&
                this.source   == other.source;
        }

        /// <summary>
        /// Gets the sourced resource.
        /// </summary>
        public FrameworkResource<T> Resource
        {
            get { return resource; }
        }

        /// <summary>
        /// Gets a value indicating whether this object represents a valid resource.
        /// </summary>
        public Boolean IsValid
        {
            get { return resource != null && asset.IsValid; }
        }

        /// <summary>
        /// Gets a value indicating whether the resource has been loaded.
        /// </summary>
        public Boolean IsLoaded
        {
            get { return resource != null && resource.Value != null; }
        }

        /// <summary>
        /// Gets a <see cref="AssetSource"/> value indicating how to load the asset.
        /// </summary>
        public AssetSource Source
        {
            get { return source; }
        }

        // Property values.
        private readonly FrameworkResource<T> resource;
        private readonly AssetID asset;
        private readonly AssetSource source;
    }
}
