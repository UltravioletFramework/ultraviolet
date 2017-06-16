using System;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents a <see cref="CompositeUvssDocument"/> which may be used as a globally-applied style sheet.
    /// </summary>
    public sealed class GlobalStyleSheet : CompositeUvssDocument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalStyleSheet"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        private GlobalStyleSheet(UltravioletContext uv)
            : base(uv)
        { }

        /// <summary>
        /// Creates a new <see cref="GlobalStyleSheet"/> instance.
        /// </summary>
        /// <returns>The <see cref="GlobalStyleSheet"/> which was created.</returns>
        public static GlobalStyleSheet Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            return new GlobalStyleSheet(uv);
        }

        /// <summary>
        /// Handles the validation phase when reloading the global style sheet.
        /// </summary>
        private bool Global_Validating(String path, UvssDocument asset)
        {
            var upf = Ultraviolet.GetUI().GetPresentationFoundation();
            return upf.TrySetGlobalStyleSheet(this);
        }

        /// <summary>
        /// Handles the reloading phase when reloading the global style sheet.
        /// </summary>
        private void Global_ValidationComplete(String path, UvssDocument asset, Boolean validated)
        {
            if (validated)
                return;

            var upf = Ultraviolet.GetUI().GetPresentationFoundation();
            upf.TrySetGlobalStyleSheet(this);
        }
    }
}
