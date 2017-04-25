using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;

namespace Ultraviolet.Presentation
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
        /// <param name="blendState">The drawing context's blend state.</param>
        /// <param name="samplerState">The drawing context's sampler state.</param>
        /// <param name="customEffect">The drawing context's custom effect.</param>
        /// <param name="localTransform">The drawing context's local transformation matrix.</param>
        /// <param name="globalTransform">The drawing context's global transformation matrix.</param>
        /// <param name="combinedTransform">The drawing context's combined transformation matrix.</param>
        internal DrawingContextState(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, Effect customEffect, ref Matrix localTransform, ref Matrix globalTransform, ref Matrix combinedTransform)
        {
            this.sortMode = sortMode;
            this.blendState = blendState;
            this.samplerState = samplerState;
            this.customEffect = customEffect;
            this.localTransform = localTransform;
            this.globalTransform = globalTransform;
            this.combinedTransform = combinedTransform;
        }

        /// <summary>
        /// Gets the <see cref="SpriteSortMode"/> which is in effect for the context.
        /// </summary>
        public SpriteSortMode SortMode
        {
            get { return sortMode; }
        }

        /// <summary>
        /// Gets the <see cref="BlendState"/> which is in effect for the context.
        /// </summary>
        public BlendState BlendState
        {
            get { return blendState; }
        }

        /// <summary>
        /// Gets the <see cref="SamplerState"/> which is in effect for the context.
        /// </summary>
        public SamplerState SamplerState
        {
            get { return samplerState; }
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

        /// <summary>
        /// Gets the combined transform matrix which is in effect for the context.
        /// </summary>
        public Matrix CombinedTransform
        {
            get { return combinedTransform; }
        }

        // Property values.
        private readonly SpriteSortMode sortMode;
        private readonly BlendState blendState;
        private readonly SamplerState samplerState;
        private readonly Effect customEffect;
        private readonly Matrix localTransform;
        private readonly Matrix globalTransform;
        private readonly Matrix combinedTransform;
    }
}
