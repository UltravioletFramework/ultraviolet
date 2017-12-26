using System;
using Ultraviolet.Core;
using Ultraviolet.Presentation.Controls;
using Ultraviolet.Presentation.Controls.Primitives;
using Ultraviolet.Presentation.Input;
using Ultraviolet.Presentation.Media;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents the root layout element for a <see cref="PresentationFoundationView"/>.
    /// </summary>
    public sealed class PresentationFoundationViewRoot : FrameworkElement
    {
        /// <summary>
        /// Initializes the <see cref="PresentationFoundationViewRoot"/> type.
        /// </summary>
        static PresentationFoundationViewRoot()
        {
            FocusManager.IsFocusScopeProperty.OverrideMetadata(
                typeof(PresentationFoundationView), new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.True, PropertyMetadataOptions.None));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationFoundationViewRoot"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public PresentationFoundationViewRoot(UltravioletContext uv, String name) 
            : base(uv, name)
        {
            this.children = new VisualCollection(this);

            this.toolTipPopup = new Popup(uv, null);
            this.toolTipPopup.IsHitTestVisible = false;
            this.children.Add(this.toolTipPopup);

            this.toolTip = new ToolTip(uv, null);
            this.toolTipPopup.Child = this.toolTip;
        }

        /// <summary>
        /// Gets the view root's child element.
        /// </summary>
        public UIElement Child
        {
            get { return child; }
            internal set
            {
                if (child == value)
                    return;

                var oldChild = this.child;
                var newChild = value;

                if (oldChild != null)
                    oldChild.ChangeLogicalAndVisualParents(null, null);

                this.child = newChild;

                if (newChild != null)
                    newChild.ChangeLogicalAndVisualParents(this, this);

                InvalidateMeasure();
            }
        }

        /// <summary>
        /// Gets the popup that contains the view's tooltips.
        /// </summary>
        internal Popup ToolTipPopup
        {
            get { return toolTipPopup; }
        }

        /// <summary>
        /// Gets the view's tooltip control.
        /// </summary>
        internal ToolTip ToolTip
        {
            get { return toolTip; }
        }

        /// <inheritdoc/>
        protected internal override UIElement GetLogicalChild(Int32 childIndex)
        {
            switch (childIndex)
            {
                case 0:
                    return child ?? toolTipPopup;

                case 1:
                    if (child != null)
                    {
                        return toolTipPopup;
                    }
                    break;
            }
            throw new ArgumentOutOfRangeException("childIndex");
        }

        /// <inheritdoc/>
        protected internal override UIElement GetVisualChild(Int32 childIndex)
        {
            switch (childIndex)
            {
                case 0:
                    return child ?? toolTipPopup;

                case 1:
                    if (child != null)
                    {
                        return toolTipPopup;
                    }
                    break;
            }
            throw new ArgumentOutOfRangeException("childIndex");
        }

        /// <inheritdoc/>
        protected internal override Int32 LogicalChildrenCount
        {
            get { return (child == null) ? 1 : 2; }
        }

        /// <inheritdoc/>
        protected internal override Int32 VisualChildrenCount
        {
            get { return (child == null) ? 1 : 2; }
        }

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            if (child != null)
            {
                child.Measure(availableSize);
                return child.DesiredSize;
            }
            toolTipPopup.Measure(availableSize);
            return Size2D.Zero;
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            if (child != null)
            {
                child.Arrange(new RectangleD(Point2D.Zero, finalSize), options);
            }
            toolTipPopup.Arrange(new RectangleD(Point2D.Zero, finalSize), options);
            return finalSize;
        }

        // State values.
        private readonly VisualCollection children;
        private UIElement child;

        // Tooltips.
        private readonly Popup toolTipPopup;
        private readonly ToolTip toolTip;
    }
}
