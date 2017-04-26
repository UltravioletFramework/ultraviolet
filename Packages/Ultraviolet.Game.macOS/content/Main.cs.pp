using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppKit;

namespace $RootNamespace$
{
    public class Application
    {
        static void Main(string[] args)
        {
            NSApplication.Init();

            using (var game = new Game())
            {
                game.Run();
            }
        }
    }
}
