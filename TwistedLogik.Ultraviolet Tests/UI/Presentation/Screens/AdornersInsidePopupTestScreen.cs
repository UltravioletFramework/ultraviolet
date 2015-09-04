using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.UI;

namespace TwistedLogik.Ultraviolet.Tests.UI.Presentation.Screens
{
    public class AdornersInsidePopupTestScreen : UIScreen
    {
        public AdornersInsidePopupTestScreen(ContentManager globalContent)
            : base("Content/UI/Screens/AdornersInsidePopupTestScreen", "AdornersInsidePopupTestScreen", globalContent)
        {

        }

        /// <inheritdoc/>
        protected override void OnViewLoaded()
        {
            if (View != null)
            {
                View.SetViewModel(new AdornersInsidePopupTestScreenViewModel());
            }
            base.OnViewLoaded();
        }
    }
}
