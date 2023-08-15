using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultraviolet;
using Ultraviolet.SDL2;

namespace UvDebug
{
    public class Program : UltravioletApplication
    {
        private static String[] args;

        /// <summary>
        /// The application's entry point.
        /// </summary>
        /// <param name="args">An array containing the application's command line arguments.</param>
        public static void Main(String[] args)
        {
            Program.args = args;

            using (var app = new Program())
            {
                app.Run();
            }
        }

        public Program() 
            : base("Ultraviolet", "UvDebug")
        {
        }

        protected override UltravioletApplicationAdapter OnCreatingApplicationAdapter()
        {
            var game = new Game(Program.args, this);
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
