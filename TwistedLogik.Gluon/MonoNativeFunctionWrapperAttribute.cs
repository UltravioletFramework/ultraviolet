using System;

namespace TwistedLogik.Gluon
{
    /// <summary>
    /// A dummy attribute which replaces Xamarin's MonoNativeFunctionWrapperAttribute
    /// on platforms other than iOS.
    /// </summary>
    [AttributeUsage(AttributeTargets.Delegate)]
    internal sealed class MonoNativeFunctionWrapperAttribute : Attribute
    { }
}
