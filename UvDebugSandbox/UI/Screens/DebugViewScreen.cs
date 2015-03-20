using System;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.UI.Presentation;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;
using TwistedLogik.Ultraviolet.UI;
using TwistedLogik.Nucleus.Collections;

namespace UvDebugSandbox.UI.Screens
{
    public class DebugViewModel
    {
        private PresentationFoundationView view;

        public DebugViewModel(PresentationFoundationView view)
        {
            this.view = view;
        }

        public void PrintVisualTree(UIElement element)
        {
            var root = view.LayoutRoot;
            PrintVisualTreeNode(root, 0);
        }

        private void PrintVisualTreeNode(DependencyObject dobj, Int32 indentLayer)
        {
            var indent = new String(' ', indentLayer * 2);
            System.Diagnostics.Debug.WriteLine(indent + dobj.GetType().Name);

            var children = VisualTreeHelper.GetChildrenCount(dobj);
            for (int i = 0; i < children; i++)
            {
                PrintVisualTreeNode(VisualTreeHelper.GetChild(dobj, i), indentLayer + 1);
            }
        }

        public String Username { get; set; }
        public String Password { get; set; }
    }

    public class DebugViewScreen : UvDebugScreen
    {
        public DebugViewScreen(ContentManager globalContent, UIScreenService uiScreenService)
            : base("Content/UI/Screens/DebugViewScreen", "DebugViewScreen", globalContent, uiScreenService)
        {

        }

        protected override void OnViewLoaded()
        {
            if (View != null)
            {
                View.SetViewModel(new DebugViewModel((PresentationFoundationView)View));
            }
            base.OnViewLoaded();
        }
    }
}
