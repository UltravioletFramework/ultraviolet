using System;
using System.IO;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.OpenGL;
using UltravioletSample.Input;

namespace UltravioletSample
{
#if ANDROID
    [Android.App.Activity(Label = "Ultraviolet Sample 2", MainLauncher = true, ConfigurationChanges = 
        Android.Content.PM.ConfigChanges.Orientation | 
        Android.Content.PM.ConfigChanges.ScreenSize | 
        Android.Content.PM.ConfigChanges.KeyboardHidden)]
    public class Game : UltravioletActivity
#else
    public class Game : UltravioletApplication
#endif
    {
        public Game()
            : base("TwistedLogik", "Ultraviolet Sample 2")
        {

        }

        public static void Main(string[] args)
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

        protected override void OnUpdating(UltravioletTime time)
        {
            if (Ultraviolet.GetInput().GetActions().ExitApplication.IsPressed())
            {
                Exit();
            }
            base.OnUpdating(time);
        }

        protected override void OnLoadingContent()
        {
            LoadInputBindings();

            base.OnLoadingContent();
        }

        protected override void OnShutdown()
        {
            SaveInputBindings();

            base.OnShutdown();
        }

        private String GetInputBindingsPath()
        {
            return Path.Combine(GetRoamingApplicationSettingsDirectory(), "InputBindings.xml");
        }

        private void LoadInputBindings()
        {
            Ultraviolet.GetInput().GetActions().Load(GetInputBindingsPath(), throwIfNotFound: false);
        }

        private void SaveInputBindings()
        {
            Ultraviolet.GetInput().GetActions().Save(GetInputBindingsPath());
        }
    }
}
