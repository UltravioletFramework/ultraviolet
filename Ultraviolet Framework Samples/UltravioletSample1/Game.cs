using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.OpenGL;

namespace UltravioletSample
{
    public class Game : UltravioletApplication
    {
        public Game()
            : base("TwistedLogik", "Ultraviolet Sample 1")
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
    }
}
