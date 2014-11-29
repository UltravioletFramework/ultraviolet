﻿using System;

namespace TwistedLogik.Ultraviolet.Layout.Stylesheets
{
    /// <summary>
    /// Represents the exception that is thrown when an error is encountered during Ultraviolet Stylesheet (UVSS) interpretation.
    /// </summary>
    [Serializable]
    public sealed class UvssException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public UvssException(String message) : base(message) { }
    }
}
