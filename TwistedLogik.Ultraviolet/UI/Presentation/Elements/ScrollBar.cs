using System;
using System.ComponentModel;
using System.Xml.Linq;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a scroll bar control.
    /// </summary>
    [UIElement("ScrollBar")]
    [DefaultProperty("Value")]
    public class ScrollBar : Control
    {
        /// <summary>
        /// Initializes the <see cref="ScrollBar"/> type.
        /// </summary>
        static ScrollBar()
        {
            ComponentTemplate = LoadComponentTemplateFromManifestResourceStream(typeof(ScrollBar).Assembly,
                "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.ScrollBar.xml");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollBar"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        /// <param name="viewModelType">The type of view model to which the element will be bound.</param>
        /// <param name="bindingContext">The binding context to apply to the element which is instantiated.</param>
        public ScrollBar(UltravioletContext uv, String id, Type viewModelType, String bindingContext = null)
            : base(uv, id)
        {
            if (ComponentTemplate != null)
            {
                LoadComponentRoot(ComponentTemplate, viewModelType, bindingContext);
                ReorientComponents();
            }

            if (Thumb != null)
            {
                Thumb.MouseMotion += Thumb_MouseMotion;
            }
        }

        void Thumb_MouseMotion(UIElement element, Input.MouseDevice device, int x, int y, int dx, int dy)
        {
            if (!Thumb.Depressed)
                return;

            Canvas.SetLeft(element, dx + Canvas.GetLeft(element));
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
        /// Gets or sets the scroll bar's orientation.
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
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(ScrollBar),
            new DependencyPropertyMetadata(HandleOrientationChanged, () => Orientation.Horizontal, DependencyPropertyOptions.None));

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
        /// Occurs when the value of the <see cref="Orientation"/> property is changed.
        /// </summary>
        /// <param name="dobj">The dependency object for which the value was changed.</param>
        private static void HandleOrientationChanged(DependencyObject dobj)
        {
            var scrollBar = (ScrollBar)dobj;
            scrollBar.ReorientComponents();
            scrollBar.OnOrientationChanged();
        }

        /// <summary>
        /// Reorients the control's components depending on the current value of its
        /// <see cref="Orientation"/> property.
        /// </summary>
        private void ReorientComponents()
        {
            if (Orientation == Orientation.Horizontal)
            {
                ButtonDecrease.ClearLocalValue<Double>(Canvas.RightProperty);
                Canvas.SetLeft(ButtonDecrease, 0.0);
                Canvas.SetTop(ButtonDecrease, 0.0);
                Canvas.SetBottom(ButtonDecrease, 0.0);
                ButtonDecrease.Width = 32; // TODO
                ButtonDecrease.Height = Double.NaN;

                ButtonIncrease.ClearLocalValue<Double>(Canvas.LeftProperty);
                Canvas.SetRight(ButtonIncrease, 0.0);
                Canvas.SetTop(ButtonIncrease, 0.0);
                Canvas.SetBottom(ButtonIncrease, 0.0);
                ButtonIncrease.Width = 32; // TODO
                ButtonIncrease.Height = Double.NaN;

                Canvas.SetLeft(Thumb, 32);
                Canvas.SetTop(Thumb, 0);
                Canvas.SetBottom(Thumb, 0);
                Thumb.Width = 32;
            }
            else
            {
                ButtonDecrease.ClearLocalValue<Double>(Canvas.TopProperty);
                Canvas.SetBottom(ButtonDecrease, 0.0);
                Canvas.SetLeft(ButtonDecrease, 0.0);
                Canvas.SetRight(ButtonDecrease, 0.0);
                ButtonDecrease.Width = Double.NaN;
                ButtonDecrease.Height = 32;

                ButtonIncrease.ClearLocalValue<Double>(Canvas.BottomProperty);
                Canvas.SetTop(ButtonIncrease, 0.0);
                Canvas.SetLeft(ButtonIncrease, 0.0);
                Canvas.SetRight(ButtonIncrease, 0.0);
                ButtonIncrease.Width = Double.NaN;
                ButtonIncrease.Height = 32;
            }
        }

        // Control components.
        //private readonly Canvas LayoutRoot = null;
        private readonly Button ButtonIncrease = null;
        private readonly Button ButtonDecrease = null;
        private readonly Button Thumb = null;
    }
}
