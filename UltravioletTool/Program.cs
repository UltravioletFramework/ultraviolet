using System;
using System.Windows.Forms;

namespace SAFE_PROJECT_NAME
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UltravioletToolForm());
        }
    }
}
