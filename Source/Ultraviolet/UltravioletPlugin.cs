using System;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a plugin which extends the functionality of the Ultraviolet Framework.
    /// </summary>
    public abstract class UltravioletPlugin
    {
        /// <summary>
        /// Registers the plugin and allows it to modify the Ultraviolet configuration prior to context creation.
        /// </summary>
        /// <param name="configuration">The Ultraviolet configuration.</param>
        public virtual void Register(UltravioletConfiguration configuration) { }

        /// <summary>
        /// Configures the plugin for the specified Ultraviolet context.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="factory">The Ultraviolet factory.</param>
        public virtual void Configure(UltravioletContext uv, UltravioletFactory factory) { }

        /// <summary>
        /// Initializes the plugin for the specified Ultraviolet context.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="factory">The Ultraviolet factory.</param>
        public virtual void Initialize(UltravioletContext uv, UltravioletFactory factory) { }

        /// <summary>
        /// Gets a value indicating whether this plugin has been configured.
        /// </summary>
        public Boolean Configured { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether this plugin has been initialized.
        /// </summary>
        public Boolean Initialized { get; internal set; }
    }
}
