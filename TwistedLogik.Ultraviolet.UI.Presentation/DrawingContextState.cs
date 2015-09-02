using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the state of a <see cref="DrawingContext"/> instance.
    /// </summary>
    public struct DrawingContextState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingContextState"/> structure.
        /// </summary>
        /// <param name="sortMode">The drawing context's sort mode.</param>
        /// <param name="customEffect">The drawing context's custom effect.</param>
        /// <param name="localTransform">The drawing context's local transformation matrix.</param>
        /// <param name="globalTransform">The drawing context's global transformation matrix.</param>
        internal DrawingContextState(SpriteSortMode sortMode, Effect customEffect, Matrix localTransform, Matrix globalTransform)
        {
            this.sortMode = sortMode;
            this.customEffect = customEffect;
            this.localTransform = localTransform;
            this.globalTransform = globalTransform;
        }

        /// <summary>
        /// Gets the <see cref="SpriteSortMode"/> which is in effect for the context.
        /// </summary>
        public SpriteSortMode SortMode
        {
            get { return sortMode; }
        }

        /// <summary>
        /// Gets the <see cref="Effect"/> which is in effect for the context.
        /// </summary>
        public Effect Effect
        {
            get { return customEffect; }
        }

        /// <summary>
        /// Gets the local transformation matrix which is in effect for the context.
        /// </summary>
        public Matrix LocalTransform
        {
            get { return localTransform; }
        }

        /// <summary>
        /// Gets the global transform matrix which is in effect for the context.
        /// </summary>
        public Matrix GlobalTransform
        {
            get { return globalTransform; }
        }

        // Property values.
        private readonly SpriteSortMode sortMode;
        private readonly Effect customEffect;
        private readonly Matrix localTransform;
        private readonly Matrix globalTransform;
    }
}
