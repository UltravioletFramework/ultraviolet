using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a collection of Ultraviolet plugins.
    /// </summary>
    public sealed class UltravioletPluginCollection : UltravioletCollection<UltravioletPlugin>
    {
        /// <summary>
        /// Adds a plugin to the collection.
        /// </summary>
        /// <param name="plugin">The plugin to add to the collection.</param>
        public void Add(UltravioletPlugin plugin)
        {
            Contract.Require(plugin, nameof(plugin));

            AddInternal(plugin);
        }
    }
}
