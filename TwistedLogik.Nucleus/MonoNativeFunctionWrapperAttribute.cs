using System;

namespace TwistedLogik.Nucleus
{
    /// <summary>
    /// Attribute to apply to delegates to flag them as targets that can be used with <see cref="System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Delegate)]
    public sealed class MonoNativeFunctionWrapperAttribute : Attribute
    { }
}

