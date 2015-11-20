using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Owin.Hosting;

namespace UvTestRunner
{
    public class Program
    {
        private static Boolean running;

        private static void Main(String[] args)
        {
            CreateEventLog();

            LogInfo("UvTestRunner was started.");

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Console.CancelKeyPress += Console_CancelKeyPress;
            Console.WriteLine("Starting UvTestRunner...");

            running = true;

            using (WebApp.Start<Startup>(url: Settings.Default.WebApiUrl))
            {
                while (running) { Thread.Sleep(1000); }
            }

            LogInfo("UvTestRunner was closed.");

            Console.WriteLine("Goodbye.");
        }

        private static void CreateEventLog()
        {
            if (EventLog.SourceExists("UvTestRunner"))
                return;

            try
            {
                EventLog.CreateEventSource("UvTestRunner", "Application");
            }
            catch (ArgumentException) { }
        }

        private static void LogInfo(String message)
        {
            using (var log = new EventLog("Application"))
            {
                log.Source = "UvTestRunner";
                log.WriteEntry(message, EventLogEntryType.Information);
            }
        }

        private static void LogWarning(String message)
        {
            using (var log = new EventLog("Application"))
            {
                log.Source = "UvTestRunner";
                log.WriteEntry(message, EventLogEntryType.Warning);
            }
        }

        private static void LogError(String message)
        {
            using (var log = new EventLog("Application"))
            {
                log.Source = "UvTestRunner";
                log.WriteEntry(message, EventLogEntryType.Error);
            }
        }
        
        private static void CurrentDomain_UnhandledException(Object sender, UnhandledExceptionEventArgs e)
        {
            LogError(e.ExceptionObject.ToString());

            var color = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine("UNHANDLED EXCEPTION");
            Console.WriteLine("===================");
            Console.WriteLine(e.ExceptionObject);
            Console.WriteLine("===================");

            Console.ForegroundColor = color;
        }

        private static void Console_CancelKeyPress(Object sender, ConsoleCancelEventArgs e)
        {
            running = false;
            e.Cancel = true;
        }
    }
}
