using Android.App;
using Android.Content.PM;

namespace UvDebug
{
    [Activity(Label = "UvDebug", MainLauncher = true, ConfigurationChanges =
        ConfigChanges.Orientation |
        ConfigChanges.ScreenSize |
        ConfigChanges.KeyboardHidden)]
    partial class Game
    { }
}