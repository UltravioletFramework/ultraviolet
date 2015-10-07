using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.UI.Presentation;

namespace UvDebugSandbox.UI.Screens
{
    public class DebugModalViewModel
    {
        private readonly UvDebugScreen owner;

        public DebugModalViewModel(UvDebugScreen owner)
        {
            this.owner = owner;
        }

        public void HandleClickClose(DependencyObject dobj, ref RoutedEventData data)
        {
            var uv = owner.Ultraviolet;
            uv.GetUI().GetScreens().Close(owner);
        }
    }

    public class DebugModal : UvDebugScreen
    {
        public DebugModal(ContentManager globalContent, UIScreenService uiScreenService)
            : base("Content/UI/Screens/DebugModal", "DebugModal", globalContent, uiScreenService)
        {

        }

        protected override void OnViewLoaded()
        {
            if (View != null)
            {
                View.SetViewModel(new DebugModalViewModel(this));
            }
            base.OnViewLoaded();
        }
    }
}
