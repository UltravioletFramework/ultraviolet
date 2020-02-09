using System;
using Ultraviolet.Presentation;
using Ultraviolet.Presentation.Documents;

namespace Ultraviolet.Presentation.Tests.ViewModels
{
    /// <summary>
    /// Represents an example adorner which renders colored boxes on top of the adorned element.
    /// </summary>
    public class ExampleBoxesAdorner : Adorner
    {
        const Double FullBoxSize = 16;
        const Double HalfBoxSize = FullBoxSize / 2;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleBoxesAdorner"/> class.
        /// </summary>
        /// <param name="adornedElement">The element being adorned.</param>
        public ExampleBoxesAdorner(UIElement adornedElement)
          : base(adornedElement)
        {

        }

        /// <inheritdoc/>
        protected override void OnDrawing(UltravioletTime time, DrawingContext dc)
        {
            DrawBlank(dc, new RectangleD(-HalfBoxSize, -HalfBoxSize, FullBoxSize, FullBoxSize), Color.Red);
            DrawBlank(dc, new RectangleD(AdornedElement.RenderSize.Width - HalfBoxSize, -HalfBoxSize, FullBoxSize, FullBoxSize), Color.Lime);
            DrawBlank(dc, new RectangleD(-8, AdornedElement.RenderSize.Height - HalfBoxSize, FullBoxSize, FullBoxSize), Color.Blue);
            DrawBlank(dc, new RectangleD(AdornedElement.RenderSize.Width - HalfBoxSize, AdornedElement.RenderSize.Height - HalfBoxSize, FullBoxSize, FullBoxSize), Color.Magenta);

            base.OnDrawing(time, dc);
        }

        /// <inheritdoc/>
        protected override RectangleD CalculateVisualBounds()
        {
            var bounds = AdornedElement.VisualBounds;
            RectangleD.Inflate(ref bounds, HalfBoxSize, HalfBoxSize, out bounds);
            return bounds;
        }
    }

}
