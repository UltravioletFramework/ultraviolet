using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.UI;

namespace TwistedLogik.Ultraviolet.Tests.UI.Presentation
{
    public class DockPanelTestScreenViewModel { }

    public class DockPanelTestScreen : UIScreen
    {
        public DockPanelTestScreen(ContentManager globalContent)
            : base("Content/UI/Screens/DockPanelTestScreen", "DockPanelTestScreen", globalContent)
        {

        }
    }
}
