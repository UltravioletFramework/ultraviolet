using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Data;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// Represents an instance of <see cref="DataObjectRegistry{T}"/> which is designed for use with the Ultraviolet Framework.
    /// </summary>
    /// <inheritdoc/>
    [CLSCompliant(false)]
    public abstract class UltravioletDataObjectRegistry<T> : DataObjectRegistry<T> where T : DataObject
    {
        /// <inheritdoc/>
        protected override void OnRegistered()
        {
            UltravioletContext.ContextInvalidated += UltravioletContext_ContextInvalidated;

            base.OnRegistered();
        }

        /// <inheritdoc/>
        protected override void OnUnregistered()
        {
            UltravioletContext.ContextInvalidated -= UltravioletContext_ContextInvalidated;

            base.OnUnregistered();
        }

        /// <summary>
        /// Handles Ultraviolet context invalidation.
        /// </summary>
        private void UltravioletContext_ContextInvalidated(Object sender, EventArgs e)
        {
            Clear();
        }
    }
}
