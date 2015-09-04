using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.UI;

namespace TwistedLogik.Ultraviolet.Tests.UI.Presentation.Screens
{
    public class AdornersTestScreen : UIScreen
    {
        public AdornersTestScreen(ContentManager globalContent)
            : base("Content/UI/Screens/AdornersTestScreen", "AdornersTestScreen", globalContent)
        {

        }

        /// <inheritdoc/>
        protected override void OnViewLoaded()
        {
            if (View != null)
            {
                View.SetViewModel(new AdornersTestScreenViewModel());
            }
            base.OnViewLoaded();
        }
    }
}
