using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.OpenGL;

namespace UltravioletGame
{
    /// <summary>
    /// Represents the main application object.
    /// </summary>
    public class Game : UltravioletApplication
    {
        public Game() : base("YOUR_ORGANIZATION", "PROJECT_NAME") { }

        /// <summary>
        /// The application's entry point.
        /// </summary>
        /// <param name="args">An array containing the application's command line arguments.</param>
        public static void Main(string[] args)
        {
            using (var game = new Game())
            {
                game.Run();
            }
        }  

        /// <inheritdoc/>
        protected override UltravioletContext OnCreatingUltravioletContext()
        {
            return new OpenGLUltravioletContext(this);
        }
    }
}