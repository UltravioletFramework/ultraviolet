using System;

namespace TwistedLogik.Ultraviolet.Layout.XiliumCef
{
    /// <summary>
    /// Represents the exception that is thrown when an API call cannot be bound.
    /// </summary>
    [Serializable]
    internal class ApiBindingFailureException : Exception
    {
        public ApiBindingFailureException() : base() { }
        public ApiBindingFailureException(String message) : base(message) { }
    }
}
