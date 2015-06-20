using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.UI;

namespace TwistedLogik.Ultraviolet.Tests.UI.Presentation
{
    public class RenderTransformTestScreenViewModel { }

    public class RenderTransformTestScreen : UIScreen
    {
        public RenderTransformTestScreen(ContentManager globalContent)
            : base("Content/UI/Screens/RenderTransformTestScreen", "RenderTransformTestScreen", globalContent)
        {

        }
    }
}
