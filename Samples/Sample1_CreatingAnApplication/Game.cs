using System;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.OpenGL;

namespace UltravioletSample.Sample1_CreatingAnApplication
{
#if ANDROID
    [Android.App.Activity(Label = "Sample 1 - Creating an Application", MainLauncher = true, ConfigurationChanges = 
        Android.Content.PM.ConfigChanges.Orientation | 
        Android.Content.PM.ConfigChanges.ScreenSize | 
        Android.Content.PM.ConfigChanges.KeyboardHidden)]
    public partial class Game : UltravioletActivity
#else
    public partial class Game : UltravioletApplication
#endif
    {
        public Game()
            : base("TwistedLogik", "Sample 1 - Creating an Application")
        {
            PlatformSpecificInitialization();
        }

        partial void PlatformSpecificInitialization();

        public static void Main(String[] args)
        {
            using (var game = new Game())
            {
                game.Run();
            }
        }

        protected override UltravioletContext OnCreatingUltravioletContext()
        {
            return new OpenGLUltravioletContext(this);
        }
    }
}
