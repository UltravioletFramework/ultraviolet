using System;
using TwistedLogik.Nucleus.Data;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents an asset which can be loaded from either the global or local content source.
    /// </summary>
    public struct SourcedImage : IEquatable<SourcedImage>, IInterpolatable<SourcedImage>
    {
        /// <summary>
        /// Initializes the <see cref="SourcedImage"/> type.
        /// </summary>
        static SourcedImage()
        {
            Tweening.Interpolators.RegisterDefault<SourcedImage>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SourcedImage"/> structure.
        /// </summary>
        /// <param name="image">The asset identifier of the resource.</param>
        /// <param name="source">An <see cref="AssetSource"/> value describing how to load the resource.</param>
        public SourcedImage(TextureImage image, AssetSource source)
        {
            this.resource  = image;
            this.source = source;
        }

        /// <summary>
        /// Implicitly converts a sourced image to its underlying image object.
        /// </summary>
        /// <param name="sourced">The <see cref="SourcedImage"/> to convert.</param>
        /// <returns>The underlying value of the sourced asset.</returns>
        public static implicit operator TextureImage(SourcedImage sourced)
        {
            return sourced.Resource;
        }

        /// <summary>
        /// Returns <c>true</c> if the specified sourced assets are equal.
        /// </summary>
        /// <param name="id1">The first <see cref="SourcedImage"/> to compare.</param>
        /// <param name="id2">The second <see cref="SourcedImage"/> to compare.</param>
        /// <returns><c>true</c> if the specified sourced assets are equal; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(SourcedImage id1, SourcedImage id2)
        {
            return id1.Equals(id2);
        }

        /// <summary>
        /// Returns <c>true</c> if the specified sourced assets are not equal.
        /// </summary>
        /// <param name="id1">The first <see cref="SourcedImage"/> to compare.</param>
        /// <param name="id2">The second <see cref="SourcedImage"/> to compare.</param>
        /// <returns><c>true</c> if the specified sourced assets are unequal; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(SourcedImage id1, SourcedImage id2)
        {
            return !id1.Equals(id2);
        }

        /// <summary>
        /// Parses a string into an instance of <see cref="SourcedImage"/>.
        /// </summary>
        /// <param name="str">The string to parse.</param>
        /// <returns>The <see cref="SourcedImage"/> instance that was created from the specified string.</returns>
        public static SourcedImage Parse(String str)
        {
            return Parse(str, null);
        }

        /// <summary>
        /// Parses a string into an instance of <see cref="SourcedImage"/>.
        /// </summary>
        /// <param name="str">The string to parse.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>The <see cref="SourcedImage"/> instance that was created from the specified string.</returns>
        public static SourcedImage Parse(String str, IFormatProvider provider)
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

            var asset = (TextureImage)ObjectResolver.FromString(str.Trim(), typeof(TextureImage), provider);
            return new SourcedImage(asset, source);
        }

        /// <inheritdoc/>
        public SourcedImage Interpolate(SourcedImage target, Single t)
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
            return obj is SourcedImage && Equals((SourcedImage)obj);
        }

        /// <inheritdoc/>
        public Boolean Equals(SourcedImage other)
        {
            return
                this.resource  == other.resource &&
                this.source == other.source;
        }

        /// <summary>
        /// Gets the sourced resource.
        /// </summary>
        public TextureImage Resource
        {
            get { return resource; }
        }

        /// <summary>
        /// Gets a value indicating whether this object represents a valid resource.
        /// </summary>
        public Boolean IsValid
        {
            get { return resource != null && resource.IsValid; }
        }

        /// <summary>
        /// Gets a value indicating whether the resource has been loaded.
        /// </summary>
        public Boolean IsLoaded
        {
            get { return resource != null && resource.IsLoaded; }
        }

        /// <summary>
        /// Gets a <see cref="AssetSource"/> value indicating how to load the asset.
        /// </summary>
        public AssetSource Source
        {
            get { return source; }
        }

        // Property values.
        private readonly TextureImage resource;
        private readonly AssetSource source;
    }
}
