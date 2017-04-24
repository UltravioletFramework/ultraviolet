
namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents the state values for a particular sprite batch.
    /// </summary>
    public struct SpriteBatchState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteBatchState"/> structure.
        /// </summary>
        /// <param name="sortMode">The sprite batch's sort mode.</param>
        /// <param name="blendState">The sprite batch's blend state.</param>
        /// <param name="samplerState">The sprite batch's sampler state.</param>
        /// <param name="rasterizerState">The sprite batch's rasterizer state.</param>
        /// <param name="depthStencilState">The sprite batch's depth/stencil state.</param>
        /// <param name="effect">The sprite batch's custom effect.</param>
        /// <param name="transformMatrix">The sprite batch's transformation matrix.</param>
        public SpriteBatchState(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, RasterizerState rasterizerState, DepthStencilState depthStencilState, Effect effect, Matrix transformMatrix)
        {
            this.sortMode          = sortMode;
            this.blendState        = blendState;
            this.samplerState      = samplerState;
            this.rasterizerState   = rasterizerState;
            this.depthStencilState = depthStencilState;
            this.customEffect      = effect;
            this.transformMatrix   = transformMatrix;
        }

        /// <summary>
        /// Gets the <see cref="SpriteSortMode"/> which is in effect for the batch.
        /// </summary>
        public SpriteSortMode SortMode
        {
            get { return sortMode; }
        }

        /// <summary>
        /// Gets the <see cref="BlendState"/> which is in effect for the batch.
        /// </summary>
        public BlendState BlendState
        {
            get { return blendState; }
        }

        /// <summary>
        /// Gets the <see cref="SamplerState"/> which is in effect for the batch.
        /// </summary>
        public SamplerState SamplerState
        {
            get { return samplerState; }
        }

        /// <summary>
        /// Gets the <see cref="RasterizerState"/> which is in effect for the batch.
        /// </summary>
        public RasterizerState RasterizerState
        {
            get { return rasterizerState; }
        }

        /// <summary>
        /// Gets the <see cref="DepthStencilState"/> which is in effect for the batch.
        /// </summary>
        public DepthStencilState DepthStencilState
        {
            get { return depthStencilState; }
        }

        /// <summary>
        /// Gets the <see cref="Effect"/> which is in effect for the batch.
        /// </summary>
        public Effect Effect
        {
            get { return customEffect; }
        }

        /// <summary>
        /// Gets the transformation matrix which is in effect for the batch.
        /// </summary>
        public Matrix TransformMatrix
        {
            get { return transformMatrix; }
        }

        // Property values.
        private readonly SpriteSortMode sortMode;
        private readonly BlendState blendState;
        private readonly SamplerState samplerState;
        private readonly DepthStencilState depthStencilState;
        private readonly RasterizerState rasterizerState;
        private readonly Effect customEffect;
        private readonly Matrix transformMatrix;
    }
}
