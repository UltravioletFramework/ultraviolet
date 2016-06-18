using Foundation;
using TwistedLogik.Ultraviolet.iOS.Bindings;
using UIKit;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents the Ultraviolet Framework's application delegate.
    /// </summary>
    [Register(nameof(UltravioletAppDelegate))]
    internal sealed class UltravioletAppDelegate : SDLUIKitDelegate
    {
        /// <inheritdoc/>
        public override void DidEnterBackground(UIApplication application)
        {
            var uv = UltravioletContext.DemandCurrent();

            uv.Messages.Publish(UltravioletMessages.ApplicationSuspended, null);
            uv.ProcessMessages();
        }

        /// <inheritdoc/>
        public override void WillEnterForeground(UIApplication application)
        {
            var uv = UltravioletContext.DemandCurrent();

            uv.Messages.Publish(UltravioletMessages.ApplicationResumed, null);
            uv.ProcessMessages();
        }
    }
}