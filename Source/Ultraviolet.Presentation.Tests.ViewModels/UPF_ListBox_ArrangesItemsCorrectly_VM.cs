using System;
using System.Collections.Generic;
using Ultraviolet.Presentation;
using Ultraviolet.Presentation.Controls;

namespace Ultraviolet.Presentation.Tests.ViewModels
{
    public class UPF_ListBox_ArrangesItemsCorrectly_VM
    {
        public class CustomItemModel
        {
            public String Name { get; set; }
            public Color Color { get; set; }
        }

        public void ListBoxInitialized(DependencyObject dobj)
        {
            var lb = (ListBox)dobj;
            lb.Items.Add("Item #1");
            lb.Items.Add("Item #2");
            lb.Items.Add("Item #3");
            lb.Items.Add("Item #4");
            lb.SelectedIndex = 2;
        }

        public static IEnumerable<CustomItemModel> TestItemsSource { get; } = new List<CustomItemModel>
        {
            new CustomItemModel() { Name = "Red", Color = Color.Red },
            new CustomItemModel() { Name = "Lime", Color = Color.Lime },
            new CustomItemModel() { Name = "Blue", Color = Color.Blue },
            new CustomItemModel() { Name = "Navy", Color = Color.Navy },
            new CustomItemModel() { Name = "Snow", Color = Color.Snow }
        };
    }
}
