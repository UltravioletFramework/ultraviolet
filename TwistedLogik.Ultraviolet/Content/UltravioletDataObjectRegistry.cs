using System;
using System.IO;
using Ultraviolet.Core.Data;
using Ultraviolet.Platform;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents an instance of <see cref="DataObjectRegistry{T}"/> which is designed for use with the Ultraviolet Framework.
    /// </summary>
    /// <inheritdoc/>
    [CLSCompliant(false)]
    public abstract class UltravioletDataObjectRegistry<T> : DataObjectRegistry<T> where T : UltravioletDataObject
    {
        /// <summary>
        /// Gets the Ultraviolet context.
        /// </summary>
        public UltravioletContext Ultraviolet =>
            UltravioletContext.DemandCurrent();

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

        /// <inheritdoc/>
        protected override Stream OpenFileStream(String path)
        {
            var fss = FileSystemService.Create();
            return fss.OpenRead(path);
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