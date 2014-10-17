using System;

namespace TwistedLogik.Ultraviolet.FMOD
{
    /// <summary>
    /// Represents an exception which is thrown as a result of an FMOD API error.
    /// </summary>
    [Serializable]
    public sealed class FMODException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the FMODException class.
        /// </summary>
        public FMODException(FMODNative.RESULT result)
            : base(FMODNative.Error.String(result))
        {

        }
    }
}
