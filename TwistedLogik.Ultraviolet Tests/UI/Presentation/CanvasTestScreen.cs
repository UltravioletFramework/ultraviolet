using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.UI;

namespace TwistedLogik.Ultraviolet.Tests.UI.Presentation
{
    public class CanvasTestScreenViewModel { }

    public class CanvasTestScreen : UIScreen
    {
        public CanvasTestScreen(ContentManager globalContent)
            : base("Content/UI/Screens/CanvasTestScreen", "CanvasTestScreen", globalContent)
        {

        }
    }
}
