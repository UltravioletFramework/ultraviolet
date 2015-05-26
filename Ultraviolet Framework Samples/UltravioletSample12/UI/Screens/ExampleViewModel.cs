using System;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.UI.Presentation;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;

namespace UltravioletSample.UI.Screens
{
    public class ExampleViewModel
    {
        public ExampleViewModel(UltravioletContext uv)
        {
            this.uv = uv;
        }

        public void Exit(DependencyObject element, ref RoutedEventData data)
        {
            uv.Host.Exit();
        }

        public void Reset(DependencyObject element, ref RoutedEventData data)
        {
            this.Message = "Hello, world!";
        }

        public void ButtonClick(DependencyObject element, ref RoutedEventData data)
        {
            this.Message = "You clicked " + ((Button)element).Content;
        }

        public String Message
        {
            get { return message; }
            set { message = value; }
        }

        private readonly UltravioletContext uv;
        private String message = "Hello, world!";
    }
}
