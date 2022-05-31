using Ultraviolet.Input;

namespace Ultraviolet.ImGuiViewProvider
{
    partial class ImGuiView
    {
        /// <summary>
        /// Maps ImGui key values to Ultraviolet <see cref="Scancode"/> values.
        /// </summary>
        private static readonly Scancode[] ScancodeMap =
        {
            Scancode.Tab,
            Scancode.Left,
            Scancode.Right,
            Scancode.Up,
            Scancode.Down,
            Scancode.PageUp,
            Scancode.PageDown,
            Scancode.Home,
            Scancode.End,
            Scancode.Insert,
            Scancode.Delete,
            Scancode.Backspace,
            Scancode.Space,
            Scancode.Return,
            Scancode.Escape,
            Scancode.A,
            Scancode.C,
            Scancode.V,
            Scancode.X,
            Scancode.Y,
            Scancode.Z,
        };
    }
}
