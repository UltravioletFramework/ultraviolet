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
            UltravioletContext.DemandCurrent().Messages.Publish(
                UltravioletMessages.ApplicationSuspended, null);
        }

        /// <inheritdoc/>
        public override void WillEnterForeground(UIApplication application)
        {
            UltravioletContext.DemandCurrent().Messages.Publish(
                UltravioletMessages.ApplicationResumed, null);
        }        
    }
}