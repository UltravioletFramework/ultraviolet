using System;
using System.Xml.Linq;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Elements
{
    /// <summary>
    /// Represents a control which provides a scrollable view of its content.
    /// </summary>
    [UIElement("ScrollViewer")]
    public class ScrollViewer : ContentControl
    {
        /// <summary>
        /// Initializes the <see cref="Canvas"/> type.
        /// </summary>
        static ScrollViewer()
        {
            ComponentTemplate = LoadComponentTemplateFromManifestResourceStream(typeof(ScrollViewer).Assembly,
                "TwistedLogik.Ultraviolet.UI.Presentation.Elements.Templates.ScrollViewer.xml");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollViewer"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its view.</param>
        public ScrollViewer(UltravioletContext uv, String id)
            : base(uv, id)
        {
            LoadComponentRoot(ComponentTemplate);
            PerformLayout();
        }

        /// <inheritdoc/>
        public override void PerformLayout()
        {
            base.PerformLayout();

            contentWidth  = 0;
            contentHeight = 0;

            if (Content != null)
            {
                var margin = ConvertThicknessToPixels(Content.Margin, 0);

                contentWidth  = (Int32)(margin.Left + margin.Right + Content.ActualWidth);
                contentHeight = (Int32)(margin.Top + margin.Bottom + Content.ActualHeight);
            }

            hScrollNeeded = contentWidth > ContentPanel.ActualWidth;
            vScrollNeeded = contentHeight > ContentPanel.ActualHeight;
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
        /// Gets or sets a value indicating whether the viewer's horizontal scroll bar is visible.
        /// </summary>
        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get { return GetValue<ScrollBarVisibility>(HorizontalScrollBarVisibilityProperty); }
            set { SetValue<ScrollBarVisibility>(HorizontalScrollBarVisibilityProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the viewer's vertical scroll bar is visible.
        /// </summary>
        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get { return GetValue<ScrollBarVisibility>(VerticalScrollBarVisibilityProperty); }
            set { SetValue<ScrollBarVisibility>(VerticalScrollBarVisibilityProperty, value); }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="HorizontalScrollBarVisibility"/> property changes.
        /// </summary>
        public event UIElementEventHandler HorizontalScrollBarVisibilityChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="VerticalScrollBarVisibility"/> property changes.
        /// </summary>
        public event UIElementEventHandler VerticalScrollBarVisibilityChanged;

        /// <summary>
        /// Identifies the <see cref="HorizontalScrollBarVisibility"/> dependency property.
        /// </summary>
        public static DependencyProperty HorizontalScrollBarVisibilityProperty = DependencyProperty.Register("HorizontalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(ScrollViewer),
            new DependencyPropertyMetadata(HandleHorizontalScrollBarVisibilityChanged, () => ScrollBarVisibility.Disabled, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the <see cref="VerticalScrollBarVisibility"/> dependency property.
        /// </summary>
        public static DependencyProperty VerticalScrollBarVisibilityProperty = DependencyProperty.Register("VerticalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(ScrollViewer),
            new DependencyPropertyMetadata(HandleVerticalScrollBarVisibilityChanged, () => ScrollBarVisibility.Visible, DependencyPropertyOptions.None));

        /// <summary>
        /// Gets the <see cref="Visibility"/> value of the horizontal scroll bar.
        /// </summary>
        private Visibility HScrollVisibility
        {
            get
            {
                switch (HorizontalScrollBarVisibility)
                {
                    case ScrollBarVisibility.Auto:
                        return hScrollNeeded ? Visibility.Visible : Visibility.Collapsed;

                    case ScrollBarVisibility.Disabled:
                    case ScrollBarVisibility.Hidden:
                        return Visibility.Collapsed;

                    case ScrollBarVisibility.Visible:
                        return Visibility.Visible;
                }
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the horizontal scroll bar is enabled.
        /// </summary>
        private Boolean HScrollEnabled
        {
            get
            {
                switch (HorizontalScrollBarVisibility)
                {
                    case ScrollBarVisibility.Auto:
                    case ScrollBarVisibility.Visible:
                    case ScrollBarVisibility.Hidden:
                        return hScrollNeeded;

                    case ScrollBarVisibility.Disabled:
                        return false;
                }
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets the maximum value of the viewer's horizontal scroll bar.
        /// </summary>
        private Double HScrollMax
        {
            get { return Math.Max(0, (Content == null) ? 0 : contentWidth - ContentPanel.ActualWidth); }
        }

        /// <summary>
        /// Gets the value of the viewer's horizontal scroll bar.
        /// </summary>
        private Double HScrollValue
        {
            get { return hScrollValue; }
            set
            {
                if (hScrollValue != value)
                {
                    hScrollValue = value;
                    ContentScrollX = (Int32)value;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="Visibility"/> value of the vertical scroll bar.
        /// </summary>
        private Visibility VScrollVisibility
        {
            get
            {
                switch (VerticalScrollBarVisibility)
                {
                    case ScrollBarVisibility.Auto:
                        return hScrollNeeded ? Visibility.Visible : Visibility.Collapsed;

                    case ScrollBarVisibility.Disabled:
                    case ScrollBarVisibility.Hidden:
                        return Visibility.Collapsed;

                    case ScrollBarVisibility.Visible:
                        return Visibility.Visible;
                }
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the vertical scroll bar is enabled.
        /// </summary>
        private Boolean VScrollEnabled
        {
            get
            {
                switch (VerticalScrollBarVisibility)
                {
                    case ScrollBarVisibility.Auto:
                    case ScrollBarVisibility.Visible:
                    case ScrollBarVisibility.Hidden:
                        return hScrollNeeded;

                    case ScrollBarVisibility.Disabled:
                        return false;
                }
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets the maximum value of the viewer's vertical scroll bar.
        /// </summary>
        private Double VScrollMax
        {
            get { return Math.Max(0, (Content == null) ? 0 : contentHeight - ContentPanel.ActualHeight); }
        }

        /// <summary>
        /// Gets the value of the viewer's vertical scroll bar.
        /// </summary>
        private Double VScrollValue
        {
            get { return vScrollValue; }
            set
            {
                if (vScrollValue != value)
                {
                    vScrollValue = value;
                    ContentScrollY = (Int32)value;
                }
            }
        }

        /// <summary>
        /// Gets the width of the viewer's content panel.
        /// </summary>
        private Double ContentWidth
        {
            get { return ContentPanel.ActualWidth; }
        }

        /// <summary>
        /// Gets the height of the viewer's content panel.
        /// </summary>
        private Double ContentHeight
        {
            get { return ContentPanel.ActualHeight; }
        }

        /// <summary>
        /// Raises the <see cref="HorizontalScrollBarVisibilityChanged"/> event.
        /// </summary>
        protected virtual void OnHorizontalScrollBarVisibilityChanged()
        {
            var temp = HorizontalScrollBarVisibilityChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="VerticalScrollBarVisibilityChanged"/> event.
        /// </summary>
        protected virtual void OnVerticalScrollBarVisibilityChanged()
        {
            var temp = VerticalScrollBarVisibilityChanged;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="HorizontalScrollBarVisibility"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleHorizontalScrollBarVisibilityChanged(DependencyObject dobj)
        {
            var viewer = (ScrollViewer)dobj;
            viewer.OnHorizontalScrollBarVisibilityChanged();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="VerticalScrollBarVisibility"/> dependency property changes.
        /// </summary>
        /// <param name="dobj">The object that raised the event.</param>
        private static void HandleVerticalScrollBarVisibilityChanged(DependencyObject dobj)
        {
            var viewer = (ScrollViewer)dobj;
            viewer.OnVerticalScrollBarVisibilityChanged();
        }

        // State values.
        private Double hScrollValue;
        private Double vScrollValue;
        private Int32 contentWidth;
        private Int32 contentHeight;
        private Boolean hScrollNeeded;
        private Boolean vScrollNeeded;
    }
}
