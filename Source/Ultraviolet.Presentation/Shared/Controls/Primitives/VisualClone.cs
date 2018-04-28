using System;
using Ultraviolet.Presentation.Media;
using Ultraviolet.Presentation.Styles;

namespace Ultraviolet.Presentation.Controls.Primitives
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

                if (clonedElement == null)
                {
                    this.Effect = null;
                }
                else
                {
                    this.Effect = clonedElement.Effect;
                    this.InvalidateStyle();
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

            if (!clonedBounds.Equals(clonedElement.TransformedVisualBounds))
            {
                InvalidateMeasure();
            }
        }

        /// <inheritdoc/>
        protected override void UpdateCore(UltravioletTime time)
        {
            var desiredEffect = (clonedElement == null) ? null : clonedElement.Effect;
            if (desiredEffect != this.Effect)
            {
                this.Effect = desiredEffect;
            }

            base.UpdateCore(time);
        }

        /// <inheritdoc/>
        protected override void DrawCore(UltravioletTime time, DrawingContext dc)
        {
            if (clonedElement != null)
            {
                var dcState = dc.GetCurrentState();
                dc.End();

                var offsetRaw = Display.DipsToPixels(clonedElement.UntransformedAbsoluteBounds.Location - clonedElement.UntransformedRelativeBounds.Location);
                var offsetX = dc.IsTransformed ? offsetRaw.X : Math.Floor(offsetRaw.X);
                var offsetY = dc.IsTransformed ? offsetRaw.Y : Math.Floor(offsetRaw.Y);
                var offset = new Vector2((Single)offsetX, (Single)offsetY);
                
                var mtxTransform = Matrix.CreateTranslation(-offset.X, -offset.Y, 0);
                var mtxTransformToView = GetTransformToViewMatrix(true);
                var mtxTransformGlobal = dcState.GlobalTransform;
                Matrix.Multiply(ref mtxTransform, ref mtxTransformToView, out mtxTransform);
                Matrix.Multiply(ref mtxTransform, ref mtxTransformGlobal, out mtxTransform);

                if (!dc.IsTransformed && !clonedElement.HasNonIdentityTransform)
                {
                    var floorM41 = (Single)Math.Floor(mtxTransform.M41);
                    var floorM42 = (Single)Math.Floor(mtxTransform.M42);
                    var floorM43 = (Single)Math.Floor(mtxTransform.M43);

                    mtxTransform = new Matrix(
                        mtxTransform.M11, mtxTransform.M12, mtxTransform.M13, mtxTransform.M14,
                        mtxTransform.M21, mtxTransform.M22, mtxTransform.M23, mtxTransform.M24,
                        mtxTransform.M31, mtxTransform.M32, mtxTransform.M33, mtxTransform.M34,
                                floorM41,         floorM42,         floorM43, mtxTransform.M44);
                }

                dc.IsOutOfBandRenderingSuppressed = true;
                dc.GlobalTransform = mtxTransform;

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
                var clonedParent = VisualTreeHelper.GetParent(clonedElement) as UIElement;
                var clonedTransform = (clonedParent == null) ? Matrix.Identity : clonedElement.GetTransformToAncestorMatrix(clonedParent);

                if (!clonedElement.IsVisuallyConnectedToViewRoot)
                {
                    clonedElement.Measure(new Size2D(Double.PositiveInfinity, Double.PositiveInfinity));
                }

                RectangleD bounds = clonedElement.VisualBounds;
                RectangleD.TransformAxisAligned(ref bounds, ref clonedTransform, out bounds);

                return bounds.Size;
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
                clonedBounds = clonedElement.TransformedVisualBounds;
                return finalRect.Size;
            }
            return base.ArrangeCore(finalRect, options);
        }

        /// <inheritdoc/>
        protected override RectangleD? ClipCore()
        {
            if (clonedBounds.Size.Width > RenderSize.Width ||
                clonedBounds.Size.Height > RenderSize.Height)
            {
                return UntransformedAbsoluteBounds;
            }
            return base.ClipCore();
        }

        /// <inheritdoc/>
        protected override void CleanupCore()
        {
            ClonedElement = null;
            base.CleanupCore();
        }

        // Property values.
        private UIElement clonedElement;

        // State values.
        private RectangleD clonedBounds;
    }
}
