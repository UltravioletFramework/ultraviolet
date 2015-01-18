using System;
using System.Xml.Linq;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents an element container which stacks its children either directly on top of each
    /// other (if <see cref="StackPanel.Orientation"/> is <see cref="F:Orientation.Vertical"/>) or
    /// side-by-side if (see <see cref="StackPanel.Orientation"/> is <see cref="F:Orientation.Horizontal"/>).
    /// </summary>
    [UIElement("StackPanel")]
    public class StackPanel : Panel
    {
        /// <summary>
        /// Initializes the <see cref="StackPanel"/> type.
        /// </summary>
        static StackPanel()
        {
            ComponentTemplate = LoadComponentTemplateFromManifestResourceStream(typeof(StackPanel).Assembly,
                "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.StackPanel.xml");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StackPanel"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public StackPanel(UltravioletContext uv, String id)
            : base(uv, id)
        {
            LoadComponentRoot(ComponentTemplate);
        }

        /// <summary>
        /// Gets or sets the template used to create the control's component tree.
        /// </summary>
        public static XDocument ComponentTemplate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the panel's orientation.
        /// </summary>
        public Orientation Orientation
        {
            get { return GetValue<Orientation>(OrientationProperty); }
            set { SetValue<Orientation>(OrientationProperty, value); }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Orientation"/> property changes.
        /// </summary>
        public event UIElementEventHandler OrientationChanged;

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(StackPanel),
            new DependencyPropertyMetadata(HandleOrientationChanged, () => Orientation.Vertical, DependencyPropertyOptions.AffectsMeasure));

        /// <inheritdoc/>
        protected override Size2D MeasureOverride(Size2D availableSize)
        {
            var contentWidth  = 0.0;
            var contentHeight = 0.0;

            foreach (var child in Children)
                child.Measure(availableSize);

            if (Orientation == Orientation.Vertical)
            {
                foreach (var child in Children)
                {
                    contentWidth  = Math.Max(contentWidth, child.DesiredSize.Width);
                    contentHeight = contentHeight + child.DesiredSize.Height;
                }
            }
            else
            {
                foreach (var child in Children)
                {
                    contentWidth  = contentWidth + child.DesiredSize.Width;
                    contentHeight = Math.Max(contentHeight, child.DesiredSize.Height);
                }
            }

            contentSize = new Size2D(contentWidth, contentHeight);
            return MeasureComponents(availableSize, contentSize);
        }

        /// <inheritdoc/>
        protected override Size2D ArrangeOverride(Size2D finalSize, ArrangeOptions options)
        {
            ArrangeComponents(finalSize);

            var positionX = 0.0;
            var positionY = 0.0;

            if (Orientation == Orientation.Vertical)
            {
                foreach (var child in Children)
                {
                    child.Arrange(new RectangleD(positionX, positionY, contentSize.Width, child.DesiredSize.Height));
                    positionY += child.RenderSize.Height;
                }
            }
            else
            {
                foreach (var child in Children)
                {
                    child.Arrange(new RectangleD(positionX, positionY, child.DesiredSize.Width, contentSize.Height));
                    positionX += child.DesiredSize.Width;
                }
            }

            return finalSize;
        }

        /// <summary>
        /// Raises the <see cref="OrientationChanged"/> event.
        /// </summary>
        protected virtual void OnOrientationChanged()
        {
            var temp = OrientationChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Orientation"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The dependency object that raised the event.</param>
        private static void HandleOrientationChanged(DependencyObject dobj)
        {
            var stackPanel = (StackPanel)dobj;
            stackPanel.OnOrientationChanged();
        }

        // State values.
        private Size2D contentSize;
    }
}
