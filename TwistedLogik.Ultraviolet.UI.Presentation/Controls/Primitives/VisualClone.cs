using System;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives
{
    /// <summary>
    /// Represents a UI element which is a visual clone of another UI element.
    /// </summary>
    internal sealed class VisualClone : UIElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisualClone"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public VisualClone(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Gets or sets the element which is being cloned by this element.
        /// </summary>
        public UIElement ClonedElement
        {
            get { return clonedElement; }
            set
            {
                if (clonedElement == value)
                    return;

                clonedElement = value;
                if (clonedElement != null)
                {
                    InvalidateStyle();
                }
            }
        }

        /// <summary>
        /// Called when the Presentation Foundation updates the layout.
        /// </summary>
        internal void HandleLayoutUpdated()
        {
            if (clonedElement == null)
                return;

            if (clonedRenderWidth != clonedElement.RenderSize.Width ||
                clonedRenderHeight != clonedElement.RenderSize.Height)
            {
                InvalidateMeasure();
            }
        }

        /// <inheritdoc/>
        protected override void DrawCore(UltravioletTime time, DrawingContext dc)
        {
            if (clonedElement != null)
            {
                var dcState = dc.GetCurrentState();
                dc.End();

                var offset = (Vector2)Display.DipsToPixels(clonedElement.UntransformedAbsoluteBounds.Location - clonedElement.UntransformedRelativeBounds.Location);

                dc.IsOutOfBandRenderingSuppressed = true;
                dc.GlobalTransform = GetTransformToViewMatrix() * Matrix.CreateTranslation(-offset.X, -offset.Y, 0);

                dc.Begin(Graphics.Graphics2D.SpriteSortMode.Deferred, null, Matrix.Identity);
                clonedElement.Draw(null, dc);
                dc.End();

                dc.IsOutOfBandRenderingSuppressed = false;
                dc.Begin(dcState);
            }

            base.DrawCore(time, dc);
        }

        /// <inheritdoc/>
        protected override void StyleCore(UvssDocument styleSheet)
        {
            if (clonedElement != null && !clonedElement.IsVisuallyConnectedToViewRoot)
            {
                clonedElement.Style(styleSheet);
            }
            base.StyleCore(styleSheet);
        }

        /// <inheritdoc/>
        protected override Size2D MeasureCore(Size2D availableSize)
        {
            if (clonedElement != null)
            {
                if (!clonedElement.IsVisuallyConnectedToViewRoot)
                {
                    clonedElement.Measure(new Size2D(Double.PositiveInfinity, Double.PositiveInfinity));
                }
                return clonedElement.DesiredSize;
            }
            return base.MeasureCore(availableSize);
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeCore(RectangleD finalRect, ArrangeOptions options)
        {
            if (clonedElement != null)
            {
                if (!clonedElement.IsVisuallyConnectedToViewRoot)
                {
                    clonedElement.Arrange(new RectangleD(0, 0, clonedElement.DesiredSize.Width, clonedElement.DesiredSize.Height));
                }
                clonedRenderWidth = clonedElement.RenderSize.Width;
                clonedRenderHeight = clonedElement.RenderSize.Height;
                return clonedElement.RenderSize;
            }
            return base.ArrangeCore(finalRect, options);
        }

        /// <inheritdoc/>
        protected override void CleanupCore()
        {
            ClonedElement = null;
            base.CleanupCore();
        }

        // Property values.
        private UIElement clonedElement;
        private Double clonedRenderWidth;
        private Double clonedRenderHeight;        
    }
}
