using System;

namespace Ultraviolet.Content
{
    partial class ContentManager
    {
        /// <summary>
        /// Controls how a <see cref="ContentManager"/> resolves an asset path to a filename.
        /// </summary>
        [Flags]
        private enum AssetResolutionFlags
        {
            /// <summary>
            /// No options.
            /// </summary>
            None = 0x0000,

            /// <summary>
            /// Include preprocessed (UVC) files as candidate matches.
            /// </summary>
            IncludePreprocessed = 0x0001,

            /// <summary>
            /// Perform asset substitutions during resolution.
            /// </summary>
            PerformSubstitution = 0x0002,

            /// <summary>
            /// Attempt to load assets from the solution directory
            /// instead of the content root.
            /// </summary>
            LoadFromSolutionDirectory = 0x0004,

            /// <summary>
            /// The default options for asset resolution.
            /// </summary>
            Default = IncludePreprocessed | PerformSubstitution
        }
    }
}
