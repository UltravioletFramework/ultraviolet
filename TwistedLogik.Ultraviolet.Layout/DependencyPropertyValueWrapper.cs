
namespace TwistedLogik.Ultraviolet.Layout
{
    /// <summary>
    /// Represents a wrapper around value types which are stored in dependency properties.
    /// </summary>
    internal class DependencyPropertyValueWrapper<T> where T : struct
    {
        /// <summary>
        /// Gets or sets the wrapper's underlying value.
        /// </summary>
        public T Value
        {
            get;
            set;
        }
    }
}
