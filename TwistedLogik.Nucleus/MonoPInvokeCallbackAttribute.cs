using System;

namespace TwistedLogik.Nucleus
{
    /// <summary>
    /// Attribute used to annotate functions that will be called back from the unmanaged world.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class MonoPInvokeCallbackAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TwistedLogik.Nucleus.MonoPInvokeCallbackAttribute"/> class.
        /// </summary>
        /// <param name="t">The type of the delegate that will be calling us back.</param>
        public MonoPInvokeCallbackAttribute(Type t) { }
    }
}

