using System;
using System.Runtime.InteropServices;

namespace TwistedLogik.Gluon
{
	public static unsafe partial class gl
	{
        public delegate void DebugProc(uint source, uint type, uint id, uint severity, int length, IntPtr message, IntPtr userParam);

        private delegate void glDebugMessageCallbackDelegate(DebugProc callback, IntPtr userParam);
        [Require(Extension = "GL_ARB_debug_output", ExtensionFunction = "glDebugMessageCallbackARB")]
        private static readonly glDebugMessageCallbackDelegate glDebugMessageCallback = null;

        public static void DebugMessageCallback(DebugProc callback, IntPtr userParam) { glDebugMessageCallback(callback, userParam); }

		private delegate void glDebugMessageControlDelegate(uint source, uint type, uint severity, int count, IntPtr ids, bool enabled);
		[Require(Extension = "GL_ARB_debug_output", ExtensionFunction = "glDebugMessageControlARB")]
        private static readonly glDebugMessageControlDelegate glDebugMessageControl = null;

        public static void DebugMessageControl(uint source, uint type, uint severity, int count, IntPtr ids, bool enabled) { glDebugMessageControl(source, type, severity, count, ids, enabled); }

        private delegate void glDebugMessageInsertDelegate(uint source, uint type, uint id, uint severity, int length, IntPtr buf);
        [Require(Extension = "GL_ARB_debug_output", ExtensionFunction = "glDebugMessageInsertARB")]
        private static readonly glDebugMessageInsertDelegate glDebugMessageInsert = null;

        public static void DebugMessageInsert(uint source, uint type, uint id, uint severity, string message)
        {
            var buffer = Marshal.StringToHGlobalAnsi(message);
            try
            {
                glDebugMessageInsert(source, type, id, severity, message.Length, buffer);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        public const UInt32 DEBUG_SEVERITY_HIGH = 0x9146;
        public const UInt32 DEBUG_SEVERITY_MEDIUM = 0x9147;
        public const UInt32 DEBUG_SEVERITY_LOW = 0x9148;
        public const UInt32 DEBUG_SEVERITY_NOTIFICATION = 0x826B;
    }
}
