using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a framework element which displays a particular sprite animation.
    /// </summary>
    [Preserve(AllMembers = true)]
    [UvmlKnownType]
    [UvmlDefaultProperty("Source")]
    public class Sprite : FrameworkElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public Sprite(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets or sets the image source.
        /// </summary>
        /// <value>A <see cref="SourcedResource{T}"/> which represents the sprite that is drawn
        /// by this control. The default value is an invalid sprite.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="SourceProperty"/></dpropField>
        ///     <dpropStylingName>source-sprite</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public SourcedResource<Graphics.Graphics2D.Sprite> Source
        {
            get { return GetValue<SourcedResource<Graphics.Graphics2D.Sprite>>(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color with which the sprite is drawn.
        /// </summary>
        /// <value>A <see cref="Color"/> value that specifies the color with which
        /// the control draws its sprite. The default value is <see cref="Color.White"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="SourceColorProperty"/></dpropField>
        ///     <dpropStylingName>source-color</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Color SourceColor
        {
            get { return GetValue<Color>(SourceColorProperty); }
            set { SetValue(SourceColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the name of the animation which is drawn.
        /// </summary>
        /// <value>A <see cref="SpriteAnimationName"/> value that specifies which of the loaded
        /// sprite's animations will be drawn by the control.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="SourceAnimationProperty"/></dpropField>
        ///     <dpropStylingName>source-animation</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public SpriteAnimationName SourceAnimation
        {
            get { return GetValue<SpriteAnimationName>(SourceAnimationProperty); }
            set { SetValue(SourceAnimationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the name of the animation which is drawn.
        /// </summary>
        /// <value>A <see cref="Boolean"/> value that specifies whether to use the display animation's
        /// global controller (if <see langword="true"/>) or a controller which is local to the control (if <see langword="false"/>).</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="UseGlobalAnimationControllerProperty"/></dpropField>
        ///     <dpropStylingName>use-global-animation-controller</dpropStylingName>
        ///     <dpropMetadata>None</dpropMetadata>
        /// </dprop>
        /// </remarks>
        public Boolean UseGlobalAnimationController
        {
            get { return GetValue<Boolean>(UseGlobalAnimationControllerProperty); }
            set { SetValue(UseGlobalAnimationControllerProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Source"/> property.
        /// </summary>
        /// <value>The identifier for the <see cref="Source"/> dependency property.</value>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", "source-sprite",
            typeof(SourcedResource<Graphics.Graphics2D.Sprite>), typeof(Sprite), new PropertyMetadata<SourcedResource<Graphics.Graphics2D.Sprite>>(HandleSourceChanged));

        /// <summary>
        /// Identifies the <see cref="SourceColor"/> property.
        /// </summary>
        /// <value>The identifier for the <see cref="SourceColor"/> dependency property.</value>
        public static readonly DependencyProperty SourceColorProperty = DependencyProperty.Register("SourceColor", typeof(Color), typeof(Sprite),
            new PropertyMetadata<Color>(UltravioletBoxedValues.Color.White));

        /// <summary>
        /// Identifies the <see cref="SourceAnimation"/> property.
        /// </summary>
        /// <value>The identifier of the <see cref="SourceAnimation"/> dependency property.</value>
        public static readonly DependencyProperty SourceAnimationProperty = DependencyProperty.Register("SourceAnimation", typeof(SpriteAnimationName), typeof(Sprite),
            new PropertyMetadata<SpriteAnimationName>(HandleSourceAnimationChanged));

        /// <summary>
        /// Identifies the <see cref="UseGlobalAnimationController"/> property.
        /// </summary>
        /// <value>The identifier of the <see cref="UseGlobalAnimationController"/> dependency property.</value>
        public static readonly DependencyProperty UseGlobalAnimationControllerProperty = DependencyProperty.Register("UseGlobalAnimationController", typeof(Boolean), typeof(Sprite),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, HandleUseGlobalAnimationControllerChanged));

        /// <inheritdoc/>
        protected override void ReloadContentOverride(Boolean recursive)
        {
            ReloadSource();
            ResetAnimationController();

            base.ReloadContentOverride(recursive);
        }

        /// <inheritdoc/>
        protected override void DrawOverride(UltravioletTime time, DrawingContext dc)
        {
            if (localAnimationController != null)
            {
                DrawSprite(dc, localAnimationController, Point2D.Zero, null, null, SourceColor, 0f);
            }
            else
            {
                var sprite = Source;
                var spriteAnimation = SourceAnimation;
                if (sprite.IsLoaded && sprite.Resource.Value != null && sprite.Resource.Value.IsValidAnimationName(spriteAnimation))
                {
                    var globalAnimationController = sprite.Resource.Value[spriteAnimation].Controller;
                    DrawSprite(dc, globalAnimationController, Point2D.Zero, null, null, SourceColor, 0f);
                }
            }
            base.DrawOverride(time, dc);
        }

        /// <summary>
        /// Reloads the element's source image.
        /// </summary>
        protected void ReloadSource()
        {
            LoadResource(Source);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Source"/> dependency property changes.
        /// </summary>
        private static void HandleSourceChanged(DependencyObject dobj, SourcedResource<Graphics.Graphics2D.Sprite> oldValue, SourcedResource<Graphics.Graphics2D.Sprite> newValue)
        {
            var sprite = (Sprite)dobj;
            sprite.ReloadSource();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="SourceAnimation"/> dependency property changes.
        /// </summary>
        private static void HandleSourceAnimationChanged(DependencyObject dobj, SpriteAnimationName oldValue, SpriteAnimationName newValue)
        {
            var sprite = (Sprite)dobj;
            sprite.ResetAnimationController();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="UseGlobalAnimationController"/> dependency property changes.
        /// </summary>
        private static void HandleUseGlobalAnimationControllerChanged(DependencyObject dobj, Boolean oldValue, Boolean newValue)
        {
            var sprite = (Sprite)dobj;
            sprite.localAnimationController = newValue ? new SpriteAnimationController() : null;
            sprite.ResetAnimationController();
        }

        /// <summary>
        /// Resets the state of the control's local animation controller, if it has one.
        /// </summary>
        private void ResetAnimationController()
        {
            if (localAnimationController == null)
                return;

            var sprite = Source;
            var spriteAnimationName = SourceAnimation;
            if (!sprite.IsLoaded || sprite.Resource.Value == null || !sprite.Resource.Value.IsValidAnimationName(spriteAnimationName))
            {
                localAnimationController.StopAnimation();
            }
            else
            {
                var animation = sprite.Resource.Value[spriteAnimationName];
                localAnimationController.PlayAnimation(animation);
            }
        }

        // State values.
        private SpriteAnimationController localAnimationController;
    }
}
