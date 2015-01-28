using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents a resource used by the Presentation Foundation.
    /// </summary>
    public sealed class FrameworkResource<T> where T : class
    {
        /// <summary>
        /// Loads the resource from the specified content manager.
        /// </summary>
        /// <param name="content">The <see cref="ContentManager"/> with which to load the resource.</param>
        /// <param name="asset">The asset identifier that identifies the resource to load.</param>
        public void Load(ContentManager content, AssetID asset)
        {
            Contract.Require(content, "content");

            value = content.Load<T>(asset);
        }
        
        /// <summary>
        /// Implicitly converts an instance of <see cref="FrameworkResource{T}"/> to its underlying resource.
        /// </summary>
        /// <param name="resource">The object to convert.</param>
        /// <returns>The converted object.</returns>
        public static implicit operator T(FrameworkResource<T> resource)
        {
            return (resource == null) ? null : resource.Value;
        }

        /// <summary>
        /// Gets the resource value that this object represents.
        /// </summary>
        public T Value
        {
            get { return value; }
        }

        // Property values.
        private T value;
    }
}
