using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Represents the exception that is thrown when an error is encountered during Ultraviolet Style Sheet (UVSS) interpretation.
    /// </summary>
    [Serializable]
    public sealed class UvssException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="errors">The collection of errors which caused this exception to be raised.</param>
        public UvssException(String message, params UvssError[] errors)
            : this(message, (IEnumerable<UvssError>)errors)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="errors">The collection of errors which caused this exception to be raised.</param>
        public UvssException(String message, IEnumerable<UvssError> errors)
            : base(message)
        {
            this.Errors = new ReadOnlyCollection<UvssError>(
                (errors ?? Enumerable.Empty<UvssError>()).ToList());
        }
        
        /// <summary>
        /// Gets the collection of errors which caused this exception to be raised.
        /// </summary>
        public ReadOnlyCollection<UvssError> Errors { get; }
    }
}
