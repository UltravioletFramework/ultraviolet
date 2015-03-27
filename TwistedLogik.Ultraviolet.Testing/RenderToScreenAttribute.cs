using System;

namespace TwistedLogik.Ultraviolet.Testing
{
    /// <summary>
    /// Represents an attribute which is used to indicate that a class' rendering tests should
    /// be rendered to the screen for visual debugging.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class RenderToScreenAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets a value indicating whether debugging is enabled. If set to true,
        /// the test framework will programatically launch and break the debugger after initialization.
        /// This is a work around for the issue that prevents the application window from showing up
        /// if a debugger is attached during initialization.
        /// </summary>
        public Boolean LaunchDebugger
        {
            get;
            set;
        }
    }
}
