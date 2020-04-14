using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a collection of Ultraviolet plugins.
    /// </summary>
    public sealed class UltravioletPluginCollection : UltravioletCollection<UltravioletPlugin>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletPluginCollection"/> class.
        /// </summary>
        /// <param name="owner">The configuration object which owns this collection.</param>
        public UltravioletPluginCollection(UltravioletConfiguration owner)
        {
            Contract.Require(owner, nameof(owner));

            this.owner = owner;
        }

        /// <summary>
        /// Adds a plugin to the collection.
        /// </summary>
        /// <param name="plugin">The plugin to add to the collection.</param>
        public void Add(UltravioletPlugin plugin)
        {
            Contract.Require(plugin, nameof(plugin));

            plugin.Register(owner);

            AddInternal(plugin);
        }

        // State values.
        private readonly UltravioletConfiguration owner;
    }
}
