using System;
using Ultraviolet.Content;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents a <see cref="CompositeUvssDocument"/> which may be used as a locally-applied style sheet.
    /// </summary>
    public sealed class LocalStyleSheet : CompositeUvssDocument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalStyleSheet"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        private LocalStyleSheet(UltravioletContext uv)
            : base(uv)
        { }

        /// <summary>
        /// Creates a new <see cref="LocalStyleSheet"/> instance.
        /// </summary>
        /// <param name="validating">A delegate which implements the <see cref="DelegateAssetWatcher{T}.OnValidating(String, T)"/> method.</param>
        /// <param name="validationComplete">A delegate which implements the <see cref="DelegateAssetWatcher{T}.OnValidationComplete(String, T, Boolean)"/> method.</param>
        /// <returns>The <see cref="LocalStyleSheet"/> which was created.</returns>
        public static LocalStyleSheet Create(AssetWatcherValidatingHandler<UvssDocument> validating = null, AssetWatcherValidationCompleteHandler<UvssDocument> validationComplete = null)
        {
            var uv = UltravioletContext.DemandCurrent();
            return new LocalStyleSheet(uv);
        }
    }
}
