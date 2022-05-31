namespace Ultraviolet.ImGuiViewProvider.Bindings
{
    [System.Flags]
    public enum ImDrawCornerFlags
    {
        TopLeft = 1 << 0,
        TopRight = 1 << 1,
        BotLeft = 1 << 2,
        BotRight = 1 << 3,
        Top = TopLeft | TopRight,
        Bot = BotLeft | BotRight,
        Left = TopLeft | BotLeft,
        Right = TopRight | BotRight,
        All = 0xF,
    }
}
