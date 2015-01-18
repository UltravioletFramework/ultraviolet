using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents a resource used by the Presentation Framework.
    /// </summary>
    public class FrameworkResource<T> where T : class
    {
        /// <summary>
        /// Loads the resource from the specified content manager.
        /// </summary>
        /// <param name="content">The <see cref="ContentManager"/> with which to load the resource.</param>
        /// <param name="asset">The asset identifier that identifies the resource to load.</param>
        public void Load(ContentManager content, AssetID asset)
        {
            Contract.Require(content, "content");

            resource = content.Load<T>(asset);
        }
        
        /// <summary>
        /// Implicitly converts an instance of <see cref="Resource{T}"/> to its underlying resource.
        /// </summary>
        /// <param name="resource">The object to convert.</param>
        /// <returns>The converted object.</returns>
        public static implicit operator T(FrameworkResource<T> resource)
        {
            return (resource == null) ? null : resource.Resource;
        }

        /// <summary>
        /// Gets the resource that this object represents.
        /// </summary>
        public T Resource
        {
            get { return Resource; }
        }

        // Property values.
        private T resource;
    }
}
