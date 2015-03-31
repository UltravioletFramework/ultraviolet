using System;
using System.Threading;
using Microsoft.Owin.Hosting;

namespace UvTestRunner
{
    public class Program
    {
        private static Boolean running;

        private static void Main(String[] args)
        {
            Console.CancelKeyPress += Console_CancelKeyPress;
            Console.WriteLine("Starting UvTestRunner...");

            running = true;

            using (WebApp.Start<Startup>(url: Settings.Default.WebApiUrl))
            {
                while (running) { Thread.Sleep(1000); }
            }

            Console.WriteLine("Goodbye.");
        }

        private static void Console_CancelKeyPress(Object sender, ConsoleCancelEventArgs e)
        {
            running = false;
        }
    }
}
