using System;
using Ultraviolet.Core;
using Ultraviolet.Graphics.Graphics2D.Text;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Contains methods for handling formatted text links within a Presentation Foundation control.
    /// </summary>
    internal static class LinkUtil
    {
        /// <summary>
        /// Updates the position of the cursor within the specified command stream.
        /// </summary>
        /// <param name="stream">The command stream to update.</param>
        /// <param name="element">The element that owns the command stream.</param>
        /// <param name="position">The position of the input device.</param>
        public static void UpdateLinkCursor(TextLayoutCommandStream stream, UIElement element, Point2D? position)
        {
            Contract.Require(element, nameof(element));

            if (stream == null || element.View == null)
                return;
            
            var positionDips = position;
            var positionPixs = positionDips.HasValue ? (Point2)element.View.Display.DipsToPixels(positionDips.Value) : (Point2?)null;

            if (positionDips.HasValue && (element.IsMouseOver || element.IsMouseCaptured))
            {
                element.View.Resources
                    .TextRenderer.UpdateCursor(stream, positionPixs);
            }
            else
            {
                element.View.Resources
                    .TextRenderer.UpdateCursor(stream, null);
            }
        }

        /// <summary>
        /// Activates any link at the current cursor position within the specified command stream.
        /// </summary>
        /// <param name="stream">The command stream to update.</param>
        /// <param name="element">The element that owns the command stream.</param>
        /// <param name="data">The event metadata for the routed event which prompted the link activation.</param>
        /// <returns><see langword="true"/> if the command stream's link was deactivated; otherwise, <see langword="false"/>.</returns>
        public static Boolean ActivateTextLink(TextLayoutCommandStream stream, UIElement element, RoutedEventData data)
        {
            Contract.Require(element, nameof(element));

            if (stream == null || element.View == null || !element.View.Resources.TextRenderer.ActivateLinkAtCursor(stream))
                return false;

            element.Focus();
            element.CaptureMouse();

            data.Handled = true;
            return true;
        }

        /// <summary>
        /// Deactivates the specified command stream's activated link, if it has one.
        /// </summary>
        /// <param name="stream">The command stream to update.</param>
        /// <param name="element">The element that owns the command stream.</param>
        /// <returns><see langword="true"/> if the command stream's link was deactivated; otherwise, <see langword="false"/>.</returns>
        public static Boolean DeactivateTextLink(TextLayoutCommandStream stream, UIElement element)
        {
            Contract.Require(element, nameof(element));

            if (stream == null || element.View == null)
                return false;

            return element.View.Resources.TextRenderer.DeactivateLink(stream);
        }

        /// <summary>
        /// Executes any link at the current cursor position within the specified command stream.
        /// </summary>
        /// <param name="stream">The command stream to update.</param>
        /// <param name="element">The element that owns the command stream.</param>
        /// <param name="data">The event metadata for the routed event which prompted the link execution.</param>
        public static Boolean ExecuteTextLink(TextLayoutCommandStream stream, UIElement element, RoutedEventData data)
        {
            Contract.Require(element, nameof(element));

            if (stream == null || element.View == null)
                return false;

            if (stream.ActiveLinkIndex.HasValue)
                element.ReleaseMouseCapture();

            if (!element.View.Resources.TextRenderer.ExecuteActivatedLink(stream))
                return false;

            data.Handled = true;
            return true;
        }
    }
}
