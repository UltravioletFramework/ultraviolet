using System;
using System.Collections;
using System.Collections.Generic;
using TwistedLogik.Ultraviolet.UI.Presentation;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;

namespace TwistedLogik.Ultraviolet.Tests.UI.Presentation.Screens
{
    public class UPF_ListBox_ArrangesItemsCorrectly_VM
    {
        public void ListBoxInitialized(DependencyObject dobj)
        {
            var lb = (ListBox)dobj;
            lb.Items.Add("Item #1");
            lb.Items.Add("Item #2");
            lb.Items.Add("Item #3");
            lb.Items.Add("Item #4");
            lb.SelectedIndex = 2;
        }

        public static IEnumerable TestItemsSource
        {
            get { return testItemsSource; }
        }

        private static readonly List<String> testItemsSource = new List<String> { "Hello", "world!", "This is a test", "of the ItemsSource property" };
    }
}
