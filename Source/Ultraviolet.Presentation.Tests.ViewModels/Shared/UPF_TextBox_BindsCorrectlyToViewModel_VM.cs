using System;
using System.Text;

namespace Ultraviolet.Presentation.Tests.ViewModels
{
    public class UPF_TextBox_BindsCorrectlyToViewModel_VM
    {
        public String BoundString { get; set; } = "Hello, world!";

        public StringBuilder BoundStringBuilder { get; set; } = new StringBuilder("Goodbye, world!");
    }
}
