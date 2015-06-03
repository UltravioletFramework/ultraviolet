using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using Microsoft.Owin.Hosting;

namespace UvTestRunner
{
    public class Program
    {
        private static Boolean running;

        private static void Main(String[] args)
        {
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;

            Console.CancelKeyPress += Console_CancelKeyPress;
            Console.WriteLine("Starting UvTestRunner...");

            running = true;

            using (WebApp.Start<Startup>(url: Settings.Default.WebApiUrl))
            {
                while (running) { Thread.Sleep(1000); }
            }

            Console.WriteLine("Goodbye.");
        }

        private static void CurrentDomain_FirstChanceException(Object sender, FirstChanceExceptionEventArgs e)
        {
            var color = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine("FIRST CHANCE EXCEPTION");
            Console.WriteLine("======================");
            Console.WriteLine(e.Exception);
            Console.WriteLine("======================");

            Console.ForegroundColor = color;
        }

        private static void Console_CancelKeyPress(Object sender, ConsoleCancelEventArgs e)
        {
            running = false;
        }
    }
}
