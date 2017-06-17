using Ultraviolet.Content;
using Ultraviolet.Platform;
using GraphicsEffect = Ultraviolet.Graphics.Effect;

namespace Ultraviolet.Presentation.Media.Effects
{
    /// <summary>
    /// Represents the base class for <see cref="Effect"/> types which use a graphical shader effect to provide their effect.
    /// </summary>
    public abstract class ShaderEffect : Effect
    {
        /// <summary>
        /// Gets or sets the effect resource.
        /// </summary>
        public SourcedResource<GraphicsEffect> Shader
        {
            get { return GetValue<SourcedResource<GraphicsEffect>>(ShaderProperty); }
            set { SetValue(ShaderProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Shader"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShaderProperty = DependencyProperty.Register("Shader", typeof(SourcedResource<GraphicsEffect>), typeof(ShaderEffect),
            new PropertyMetadata<SourcedResource<GraphicsEffect>>(null, PropertyMetadataOptions.None));

        /// <inheritdoc/>
        protected internal override void Draw(DrawingContext dc, UIElement element, OutOfBandRenderTarget target)
        {
            if (Shader.IsValid && Shader.IsLoaded)
            {
                DrawRenderTargetAtVisualBounds(dc, element, target, Shader);
            }
        }

        /// <inheritdoc/>
        protected internal override void ReloadResources(ContentManager global, ContentManager local)
        {
            var shader = Shader;
            if (shader.Source == AssetSource.Global)
            {
                shader.Load(global, ScreenDensityBucket.Desktop);
            }
            else
            {
                shader.Load(local, ScreenDensityBucket.Desktop);
            }

            base.ReloadResources(global, local);
        }
    }
}
