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
    public class Game : UltravioletActivity
#else
    public class Game : UltravioletApplication
#endif
    {
        public Game()
            : base("TwistedLogik", "Sample 1 - Creating an Application")
        {
#if IOS
            EnsureAssemblyIsLinked<TwistedLogik.Ultraviolet.BASS.BASSUltravioletAudio>();
#endif
        }

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
