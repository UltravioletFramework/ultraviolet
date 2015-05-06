using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.UI;

namespace TwistedLogik.Ultraviolet.Tests.UI.Presentation
{
    public class GridTestScreenViewModel { }

    public class GridTestScreen : UIScreen
    {
        public GridTestScreen(ContentManager globalContent)
            : base("Content/UI/Screens/GridTestScreen", "GridTestScreen", globalContent)
        {

        }
    }
}
