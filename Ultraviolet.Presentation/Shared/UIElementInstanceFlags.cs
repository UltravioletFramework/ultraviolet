using System;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents a collection of values which can be attached to an element instance by
    /// the UVML loader in order to record certain information which is required later
    /// in the loading process.
    /// </summary>
    [Flags]
    internal enum UIElementInstanceFlags
    {
        /// <summary>
        /// The element has no loading flags.
        /// </summary>
        None = 0x0000,

        /// <summary>
        /// The element should be used to replace the component template's ItemsPanel element.
        /// </summary>
        IsItemsPanel = 0x0001,
    }
}
