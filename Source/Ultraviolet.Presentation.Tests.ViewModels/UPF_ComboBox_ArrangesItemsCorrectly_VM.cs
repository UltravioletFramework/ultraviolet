using System;
using System.Collections.Generic;
using Ultraviolet.Presentation.Controls;

namespace Ultraviolet.Presentation.Tests.ViewModels
{
    public class UPF_ComboBox_ArrangesItemsCorrectly_VM
    {
        public class CustomItemModel
        {
            public String Name { get; set; }
            public Color Color { get; set; }
        }

        public void ComboBoxInitialized(DependencyObject dobj)
        {
            var cb = (ComboBox)dobj;
            cb.Items.Add("Item #1");
            cb.Items.Add("Item #2");
            cb.Items.Add("Item #3");
            cb.Items.Add("Item #4");
            cb.SelectedIndex = 2;
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
