using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.OpenGL;

namespace $RootNamespace$
{
    public class Game : UltravioletApplication
    {
        public Game() 
            : base("YOUR_ORGANIZATION", "PROJECT_NAME") 
        { 
            // This ensures that the Xamarin linker includes the BASS assembly, 
            // which needs to be dynamically loaded by the Ultraviolet context.
            EnsureAssemblyIsLinked<TwistedLogik.Ultraviolet.BASS.BASSUltravioletAudio>();
        }

        protected override UltravioletContext OnCreatingUltravioletContext()
        {
            var configuration = new OpenGLUltravioletConfiguration();
            PopulateConfiguration(configuration);

#if DEBUG
            configuration.Debug = true;
            configuration.DebugLevels = DebugLevels.Error | DebugLevels.Warning;
            configuration.DebugCallback = (uv, level, message) =>
            {
                System.Diagnostics.Debug.WriteLine(message);
            };
#endif

            return new OpenGLUltravioletContext(this, configuration);
        }

        protected override void OnLoadingContent()
        {
            this.content = ContentManager.Create("Content");

            // TODO: Load content here

            base.OnLoadingContent();
        }

        protected override void OnUpdating(UltravioletTime time)
        {
            // TODO: Update the game state
            
            base.OnUpdating(time);
        }
        
        protected override void OnDrawing(UltravioletTime time)
        {
            // TODO: Draw the game
            
            base.OnDrawing(time);
        }
        
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                SafeDispose.DisposeRef(ref content);
            }
            base.Dispose(disposing);
        }

        private ContentManager content;
    }
}