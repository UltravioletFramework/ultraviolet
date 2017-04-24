using System;
using Ultraviolet.Core;
using Ultraviolet.Graphics.Graphics2D.Text;
using Ultraviolet.Presentation.Controls.Primitives;
using Ultraviolet.Presentation.Documents;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Contains utility methods useful for laying out framework elements.
    /// </summary>
    public static class LayoutUtil
    {
        /// <summary>
        /// Gets a value indicating whether the specified element is currently being drawn.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><see langword="true"/> if the specified element is being drawn; otherwise, false.</returns>
        public static Boolean IsDrawn(UIElement element)
        {
            Contract.Require(element, nameof(element));

            if (element.Visibility != Visibility.Visible)
                return false;

            if (element is Popup || element is Adorner)
                return true;

            return (element.RenderSize.Width > 0 && element.RenderSize.Height > 0);
        }

        /// <summary>
        /// Gets a value indicating whether the specified element is currently filling space in the layout.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><see langword="true"/> if the specified element is filling space; otherwise, <see langword="false"/>.</returns>
        public static Boolean IsSpaceFilling(UIElement element)
        {
            Contract.Require(element, nameof(element));

            return element.Visibility != Visibility.Collapsed;
        }

        /// <summary>
        /// Gets a measure value, substituting a second value if the first value is not a number.
        /// </summary>
        /// <param name="measure">The measure value on which to perform substitutions.</param>
        /// <param name="nan">The value to return if <paramref name="measure"/> is not a number.</param>
        /// <returns>A valid measure value.</returns>
        public static Double GetValidMeasure(Double measure, Double nan)
        {
            if (Double.IsNaN(measure))
            {
                return nan;
            }
            return measure;
        }

        /// <summary>
        /// Gets a measure value, substituting other values if the first value is not a number, negative infinity, or positive infinity.
        /// </summary>
        /// <param name="measure">The measure value on which to perform substitutions.</param>
        /// <param name="nan">The value to return if <paramref name="measure"/> is not a number.</param>
        /// <param name="neginf">The value to return if <paramref name="measure"/> is negative infinity.</param>
        /// <param name="posinf">The value to return if <paramref name="measure"/> is positive infinity.</param>
        /// <returns>A valid measure value.</returns>
        public static Double GetValidMeasure(Double measure, Double nan, Double neginf, Double posinf)
        {
            if (Double.IsNaN(measure))
            {
                return nan;
            }
            if (Double.IsNegativeInfinity(measure))
            {
                return neginf;
            }
            if (Double.IsPositiveInfinity(measure))
            {
                return posinf;
            }
            return measure;
        }

        /// <summary>
        /// Horizontally aligns content within the specified space.
        /// </summary>
        /// <param name="space">The space in which to align the content.</param>
        /// <param name="content">The size of the content to align within <paramref name="space"/>.</param>
        /// <param name="alignment">The type of alignment to perform on the specified content.</param>
        /// <returns>The x-offset of the content within the specified space.</returns>
        public static Double PerformHorizontalAlignment(Size2D space, Size2D content, HorizontalAlignment alignment)
        {
            var xOffset = 0.0;

            switch (alignment)
            {
                case HorizontalAlignment.Stretch:
                    if (content.Width >= space.Width)
                    {
                        xOffset = 0;
                    }
                    else
                    {
                        xOffset = (space.Width - content.Width) / 2.0;
                    }
                    break;

                case HorizontalAlignment.Center:
                    xOffset = (space.Width - content.Width) / 2.0;
                    break;

                case HorizontalAlignment.Right:
                    xOffset = space.Width - content.Width;
                    break;
            }

            return xOffset;
        }

        /// <summary>
        /// Vertically aligns content within the specified space.
        /// </summary>
        /// <param name="space">The space in which to align the content.</param>
        /// <param name="content">The size of the content to align within <paramref name="space"/>.</param>
        /// <param name="alignment">The type of alignment to perform on the specified content.</param>
        /// <returns>The y-offset of the content within the specified space.</returns>
        public static Double PerformVerticalAlignment(Size2D space, Size2D content, VerticalAlignment alignment)
        {
            var yOffset = 0.0;

            switch (alignment)
            {
                case VerticalAlignment.Stretch:
                    if (content.Height >= space.Height)
                    {
                        yOffset = 0;
                    }
                    else
                    {
                        yOffset = (space.Height - content.Height) / 2.0;
                    }
                    break;

                case VerticalAlignment.Center:
                    yOffset = (space.Height - content.Height) / 2.0;
                    break;

                case VerticalAlignment.Bottom:
                    yOffset = space.Height - content.Height;
                    break;
            }

            return yOffset;
        }

        /// <summary>
        /// Converts a <see cref="HorizontalAlignment"/> value and a <see cref="VerticalAlignment"/> value 
        /// to the corresponding <see cref="TextFlags"/> value.
        /// </summary>
        /// <param name="horizontal">The <see cref="HorizontalAlignment"/> value to convert.</param>
        /// <param name="vertical">The <see cref="VerticalAlignment"/> value to convert.</param>
        /// <returns>The <see cref="TextFlags"/> value that corresponds to the specified set of alignments.</returns>
        public static TextFlags ConvertAlignmentsToTextFlags(HorizontalAlignment horizontal, VerticalAlignment vertical)
        {
            var flags = (TextFlags)0;

            switch (horizontal)
            {
                case HorizontalAlignment.Left:
                case HorizontalAlignment.Stretch:
                    flags |= TextFlags.AlignLeft;
                    break;

                case HorizontalAlignment.Right:
                    flags |= TextFlags.AlignRight;
                    break;

                case HorizontalAlignment.Center:
                    flags |= TextFlags.AlignCenter;
                    break;
            }

            switch (vertical)
            {
                case VerticalAlignment.Top:
                case VerticalAlignment.Stretch:
                    flags |= TextFlags.AlignTop;
                    break;

                case VerticalAlignment.Bottom:
                    flags |= TextFlags.AlignBottom;
                    break;

                case VerticalAlignment.Center:
                    flags |= TextFlags.AlignMiddle;
                    break;
            }

            return flags;
        }

        /// <summary>
        /// Gets the upper and lower bounds of the specified measure value, taking its minimum and
        /// maximum values into account.
        /// </summary>
        /// <param name="measure">The measure value to evaluate.</param>
        /// <param name="min">The measure value's minimum allowed value.</param>
        /// <param name="max">The measure value's maximum allowed value.</param>
        /// <param name="lower">The lower bound of possible values for the specified measure.</param>
        /// <param name="upper">The upper bound of possible values for the specified measure.</param>
        public static void GetBoundedMeasure(Double measure, Double min, Double max, out Double lower, out Double upper)
        {
            upper = Math.Max(Math.Min(Double.IsNaN(measure) ? Double.PositiveInfinity : measure, max), min);
            lower = Math.Max(Math.Min(max, Double.IsNaN(measure) ? 0.0 : measure), min);
        }
    }
}
