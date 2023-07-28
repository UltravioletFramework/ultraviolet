using Ultraviolet.Graphics;

namespace Ultraviolet.Presentation
{
    partial class DrawingContext
    {
        // The texture which is used to render the clipping region to the stencil buffer.
        private static readonly UltravioletSingleton<Texture2D> StencilTexture = 
            new UltravioletSingleton<Texture2D>(UltravioletSingletonFlags.DisabledInServiceMode, uv =>
            {
                var texture = Texture2D.CreateTexture(1, 1);
                texture.SetData(new[] { Color.White });
                return texture;
            });

        // The depth/stencil state which is used to write to the stencil buffer.
        private static readonly UltravioletSingleton<DepthStencilState> StencilWriteDepthState = 
            new UltravioletSingleton<DepthStencilState>(UltravioletSingletonFlags.DisabledInServiceMode, uv =>
            {
                var state = DepthStencilState.Create();
                state.StencilEnable = true;
                state.StencilFunction = CompareFunction.Always;
                state.StencilPass = StencilOperation.Replace;
                state.ReferenceStencil = 0;
                state.DepthBufferEnable = false;
                return state;
            });

        // The depth/stencil state which is used to read from the stencil buffer.
        private static readonly UltravioletSingleton<DepthStencilState> StencilReadDepthState = 
            new UltravioletSingleton<DepthStencilState>(UltravioletSingletonFlags.DisabledInServiceMode, uv =>
            {
                var state = DepthStencilState.Create();
                state.StencilEnable = true;
                state.StencilFunction = CompareFunction.Equal;
                state.ReferenceStencil = 0;
                state.DepthBufferEnable = false;
                return state;
            });

        // The blend state which is used to disable color writes during stenciling.
        private static readonly UltravioletSingleton<BlendState> StencilBlendState = 
            new UltravioletSingleton<BlendState>(UltravioletSingletonFlags.DisabledInServiceMode, uv =>
            {
                var state = BlendState.Create();
                state.AlphaSourceBlend = Blend.One;
                state.AlphaDestinationBlend = Blend.InverseSourceAlpha;
                state.ColorSourceBlend = Blend.One;
                state.ColorDestinationBlend = Blend.InverseSourceAlpha;
                state.ColorWriteChannels = ColorWriteChannels.None;
                return state;
            });
    }
}
