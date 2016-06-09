using System;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;

namespace TwistedLogik.Ultraviolet.UI.Presentation
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
        public static void UpdateLinkCursor(TextLayoutCommandStream stream, UIElement element)
        {
            Contract.Require(element, nameof(element));

            if (stream == null)
                return;
            
            var positionDips = Mouse.GetPosition(element);
            var positionPixs = (Point2)element.View.Display.DipsToPixels(positionDips);

            if (element.Bounds.Contains(positionDips) || element.IsMouseCaptured)
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
        /// Activates any link at the current cursor position within the content presenter's text block.
        /// </summary>
        /// <param name="stream">The command stream to update.</param>
        /// <param name="element">The element that owns the command stream.</param>
        /// <param name="data">The event metadata for the routed event which prompted the link activation.</param>
        public static Boolean ActivateTextLink(TextLayoutCommandStream stream, UIElement element, ref RoutedEventData data)
        {
            Contract.Require(element, nameof(element));

            if (stream == null || !element.View.Resources.TextRenderer.ActivateLinkAtCursor(stream))
                return false;

            element.Focus();
            element.CaptureMouse();

            data.Handled = true;
            return true;
        }

        /// <summary>
        /// Executes any link at the current cursor position within the content presenter's text block/
        /// </summary>
        /// <param name="stream">The command stream to update.</param>
        /// <param name="element">The element that owns the command stream.</param>
        /// <param name="data">The event metadata for the routed event which prompted the link execution.</param>
        public static Boolean ExecuteTextLink(TextLayoutCommandStream stream, UIElement element, ref RoutedEventData data)
        {
            Contract.Require(element, nameof(element));

            if (stream == null)
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
