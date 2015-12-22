using System;
using System.Collections;
using System.Collections.Generic;
using TwistedLogik.Ultraviolet.UI.Presentation;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;

namespace TwistedLogik.Ultraviolet.Tests.UI.Presentation.Screens
{
    public class UPF_ComboBox_ArrangesItemsCorrectly_VM
    {
        public void ComboBoxInitialized(DependencyObject dobj)
        {
            var cb = (ComboBox)dobj;
            cb.Items.Add("Item #1");
            cb.Items.Add("Item #2");
            cb.Items.Add("Item #3");
            cb.Items.Add("Item #4");
            cb.SelectedIndex = 2;
        }

        public static IEnumerable TestItemsSource
        {
            get { return testItemsSource; }
        }

        private static readonly List<String> testItemsSource = new List<String> { "Hello", "world!", "Testing", "ItemsSource" };
    }
}
