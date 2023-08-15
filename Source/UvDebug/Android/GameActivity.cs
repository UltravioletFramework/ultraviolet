using Android.App;
using Android.Content.PM;
using System;
using Ultraviolet;

namespace UvDebug
{
    [Activity(Label = "UvDebug", MainLauncher = true, ConfigurationChanges =
        ConfigChanges.Orientation |
        ConfigChanges.ScreenSize |
        ConfigChanges.KeyboardHidden)]
    public class GameActivity : UltravioletApplication
    {
        public GameActivity() 
            : base("Ultraviolet", "UvDebug")
        {
        }

        protected override UltravioletApplicationAdapter OnCreatingApplicationAdapter()
        {
            var game = new Game(Array.Empty<String>(), this);
            return game;
        }

        /// <summary>
        /// Called after the application has been initialized.
        /// </summary>
        protected override void OnInitialized()
        {
            if (!SetFileSourceFromManifestIfExists("UvDebug.Content.uvarc"))
                UsePlatformSpecificFileSource();

            base.OnInitialized();
        }
    }
}