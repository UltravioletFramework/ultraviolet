using System.Runtime.InteropServices;

namespace Ultraviolet
{
    partial class UltravioletTimingLogic
    {
        private static class Win32Native
        {
            [DllImport("winmm")]
            public static extern uint timeBeginPeriod(uint period);

            [DllImport("winmm")]
            public static extern uint timeEndPeriod(uint period);
        }
    }
}
