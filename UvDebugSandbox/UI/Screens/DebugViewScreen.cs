using System;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.UI.Presentation;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls.Primitives;
using TwistedLogik.Ultraviolet.UI.Presentation.Media;

namespace UvDebugSandbox.UI.Screens
{
    public class DebugViewModel
    {
        private PresentationFoundationView view;

        public DebugViewModel(PresentationFoundationView view)
        {
            Enable = true;
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

        public Boolean Enable { get; set; }

        public Double SomeValue
        {
            get;
            set;
        }

        public Single Angle { get { return (DateTime.Now.Millisecond / 1000.0f) * 360f; } }
        public Single Angle2 { get; set; }

        public Int32 Index
        {
            get;
            set;
        }

        public String Name
        {
            get 
            {
                var selected = foo.SelectedItem;
                if (selected == null)
                    return null;

                return ((FrameworkElement)selected).Name;
            }
        }

        private readonly ListBox foo = null;

        public void HandleValueChanged(DependencyObject dobj)
        {
            System.Diagnostics.Debug.WriteLine(DateTime.UtcNow.TimeOfDay + " value changed");
        }

        public void HandleSelectionChanged(DependencyObject dobj, ref RoutedEventData data)
        {
            System.Diagnostics.Debug.WriteLine("selection changed:");
            System.Diagnostics.Debug.Write("   ");

            var lbox = (ListBox)dobj;
            foreach (var item in lbox.SelectedItems)
            {
                System.Diagnostics.Debug.Write(((FrameworkElement)item).Name + " ");
            }

            System.Diagnostics.Debug.WriteLine("");
        }

        public void HandleExit(DependencyObject dobj, ref RoutedEventData data)
        {
            view.Ultraviolet.Host.Exit();
        }

        public void HandleClick(DependencyObject dobj, ref RoutedEventData data)
        {
            asdf.IsOpen = !asdf.IsOpen;
        }

        private readonly Popup asdf = null;
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
