using System;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Contains common boxed values of the Presentation Foundation's value types.
    /// </summary>
    public static class PresentationBoxedValues
    {
        /// <summary>
        /// Contains boxed <see cref="Ultraviolet.Presentation.Controls.Orientation"/> values.
        /// </summary>
        public static class Orientation
        {
            /// <summary>
            /// The cached box for the value <see cref="Ultraviolet.Presentation.Controls.Orientation.Horizontal"/>.
            /// </summary>
            public static readonly Object Horizontal =
                Ultraviolet.Presentation.Controls.Orientation.Horizontal;

            /// <summary>
            /// The cached box for the value <see cref="Ultraviolet.Presentation.Controls.Orientation.Vertical"/>.
            /// </summary>
            public static readonly Object Vertical =
                Ultraviolet.Presentation.Controls.Orientation.Vertical;
        }

        /// <summary>
        /// Contains boxed <see cref="Ultraviolet.Presentation.Thickness"/> values.
        /// </summary>
        public static class Thickness
        {
            /// <summary>
            /// The cached box for the value <see cref="Ultraviolet.Presentation.Thickness.Zero"/>.
            /// </summary>
            public static readonly Object Zero =
                Ultraviolet.Presentation.Thickness.Zero;

            /// <summary>
            /// The cached box for the value <see cref="Ultraviolet.Presentation.Thickness.One"/>.
            /// </summary>
            public static readonly Object One =
                Ultraviolet.Presentation.Thickness.One;
        }

        /// <summary>
        /// Contains boxed <see cref="Ultraviolet.Presentation.HorizontalAlignment"/> values.
        /// </summary>
        public static class HorizontalAlignment
        {
            /// <summary>
            /// The cached box for the value <see cref="Ultraviolet.Presentation.HorizontalAlignment.Left"/>.
            /// </summary>
            public static readonly Object Left =
                Ultraviolet.Presentation.HorizontalAlignment.Left;

            /// <summary>
            /// The cached box for the value <see cref="Ultraviolet.Presentation.HorizontalAlignment.Center"/>.
            /// </summary>
            public static readonly Object Center =
                Ultraviolet.Presentation.HorizontalAlignment.Center;

            /// <summary>
            /// The cached box for the value <see cref="Ultraviolet.Presentation.HorizontalAlignment.Right"/>.
            /// </summary>
            public static readonly Object Right =
                Ultraviolet.Presentation.HorizontalAlignment.Right;

            /// <summary>
            /// The cached box for the value <see cref="Ultraviolet.Presentation.HorizontalAlignment.Stretch"/>.
            /// </summary>
            public static readonly Object Stretch =
                Ultraviolet.Presentation.HorizontalAlignment.Stretch;
        }

        /// <summary>
        /// Contains boxed <see cref="Ultraviolet.Presentation.VerticalAlignment"/> values.
        /// </summary>
        public static class VerticalAlignment
        {
            /// <summary>
            /// The cached box for the value <see cref="Ultraviolet.Presentation.VerticalAlignment.Top"/>.
            /// </summary>
            public static readonly Object Top =
                Ultraviolet.Presentation.VerticalAlignment.Top;

            /// <summary>
            /// The cached box for the value <see cref="Ultraviolet.Presentation.VerticalAlignment.Center"/>.
            /// </summary>
            public static readonly Object Center =
                Ultraviolet.Presentation.VerticalAlignment.Center;

            /// <summary>
            /// The cached box for the value <see cref="Ultraviolet.Presentation.VerticalAlignment.Bottom"/>.
            /// </summary>
            public static readonly Object Bottom =
                Ultraviolet.Presentation.VerticalAlignment.Bottom;

            /// <summary>
            /// The cached box for the value <see cref="Ultraviolet.Presentation.VerticalAlignment.Stretch"/>.
            /// </summary>
            public static readonly Object Stretch =
                Ultraviolet.Presentation.VerticalAlignment.Stretch;
        }

        /// <summary>
        /// Contains boxed <see cref="Ultraviolet.Presentation.GridLength"/> values.
        /// </summary>
        public static class GridLength
        {
            /// <summary>
            /// The cached box for a <see cref="Ultraviolet.Presentation.GridLength"/> of zero pixels.
            /// </summary>
            public static readonly Object Zero =
                new Ultraviolet.Presentation.GridLength(0);

            /// <summary>
            /// The cached box for a <see cref="Ultraviolet.Presentation.GridLength"/> of one pixel.
            /// </summary>
            public static readonly Object One =
                new Ultraviolet.Presentation.GridLength(1);

            /// <summary>
            /// The cached box for an auto-sized <see cref="Ultraviolet.Presentation.GridLength"/>.
            /// </summary>
            public static readonly Object Auto =
                Ultraviolet.Presentation.GridLength.Auto;
        }

        /// <summary>
        /// Contains boxed <see cref="Ultraviolet.Presentation.Controls.Dock"/> values.
        /// </summary>
        public static class Dock
        {
            /// <summary>
            /// The cached box for the value <see cref="Ultraviolet.Presentation.Controls.Dock.Left"/>.
            /// </summary>
            public static readonly Object Left =
                Ultraviolet.Presentation.Controls.Dock.Left;

            /// <summary>
            /// The cached box for the value <see cref="Ultraviolet.Presentation.Controls.Dock.Top"/>.
            /// </summary>
            public static readonly Object Top =
                Ultraviolet.Presentation.Controls.Dock.Top;

            /// <summary>
            /// The cached box for the value <see cref="Ultraviolet.Presentation.Controls.Dock.Right"/>.
            /// </summary>
            public static readonly Object Right =
                Ultraviolet.Presentation.Controls.Dock.Right;

            /// <summary>
            /// The cached box for the value <see cref="Ultraviolet.Presentation.Controls.Dock.Bottom"/>.
            /// </summary>
            public static readonly Object Bottom =
                Ultraviolet.Presentation.Controls.Dock.Bottom;
        }

        /// <summary>
        /// Contains boxed <see cref="Ultraviolet.Presentation.Controls.ClickMode"/> values.
        /// </summary>
        public static class ClickMode
        {
            /// <summary>
            /// The cached box for the value <see cref="Ultraviolet.Presentation.Controls.ClickMode.Hover"/>.
            /// </summary>
            public static readonly Object Hover =
                Ultraviolet.Presentation.Controls.ClickMode.Hover;

            /// <summary>
            /// The cached box for the value <see cref="Ultraviolet.Presentation.Controls.ClickMode.Press"/>.
            /// </summary>
            public static readonly Object Press =
                Ultraviolet.Presentation.Controls.ClickMode.Press;

            /// <summary>
            /// The cached box for the value <see cref="Ultraviolet.Presentation.Controls.ClickMode.Release"/>.
            /// </summary>
            public static readonly Object Release =
                Ultraviolet.Presentation.Controls.ClickMode.Release;
        }

        /// <summary>
        /// Contains boxed <see cref="Ultraviolet.Presentation.Controls.ScrollBarVisibility"/> values.
        /// </summary>
        public static class ScrollBarVisibility
        {
            /// <summary>
            /// The cached box for the value <see cref="Ultraviolet.Presentation.Controls.ScrollBarVisibility.Auto"/>.
            /// </summary>
            public static readonly Object Auto =
                Ultraviolet.Presentation.Controls.ScrollBarVisibility.Auto;

            /// <summary>
            /// The cached box for the value <see cref="Ultraviolet.Presentation.Controls.ScrollBarVisibility.Disabled"/>.
            /// </summary>
            public static readonly Object Disabled =
                Ultraviolet.Presentation.Controls.ScrollBarVisibility.Disabled;

            /// <summary>
            /// The cached box for the value <see cref="Ultraviolet.Presentation.Controls.ScrollBarVisibility.Hidden"/>.
            /// </summary>
            public static readonly Object Hidden =
                Ultraviolet.Presentation.Controls.ScrollBarVisibility.Hidden;

            /// <summary>
            /// The cached box for the value <see cref="Ultraviolet.Presentation.Controls.ScrollBarVisibility.Visible"/>.
            /// </summary>
            public static readonly Object Visible =
                Ultraviolet.Presentation.Controls.ScrollBarVisibility.Visible;
        }

        /// <summary>
        /// Contains boxed <see cref="Ultraviolet.Presentation.Visibility"/> values.
        /// </summary>
        public static class Visibility
        {
            /// <summary>
            /// The cached box for the value <see cref="Ultraviolet.Presentation.Visibility.Collapsed"/>.
            /// </summary>
            public static readonly Object Collapsed =
                Ultraviolet.Presentation.Visibility.Collapsed;

            /// <summary>
            /// The cached box for the value <see cref="Ultraviolet.Presentation.Visibility.Hidden"/>.
            /// </summary>
            public static readonly Object Hidden =
                Ultraviolet.Presentation.Visibility.Hidden;

            /// <summary>
            /// The cached box for the value <see cref="Ultraviolet.Presentation.Visibility.Visible"/>.
            /// </summary>
            public static readonly Object Visible =
                Ultraviolet.Presentation.Visibility.Visible;
        }
    }
}
