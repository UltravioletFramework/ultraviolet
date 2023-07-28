using System;
using System.Collections.Generic;
using static Ultraviolet.BASS.Native.BASSNative;

namespace Ultraviolet.BASS
{
    /// <summary>
    /// Represents an exception thrown as a result of a BASS API error.
    /// </summary>
    [Serializable]
    public sealed class BASSException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the BASSException class.
        /// </summary>
        public BASSException()
            : base(GetExceptionMessage())
        {

        }

        /// <summary>
        /// Initializes a new instance of the BASSException class.
        /// </summary>
        /// <param name="code">The BASS error code.</param>
        public BASSException(Int32 code)
            : base(GetExceptionMessage(code))
        {

        }

        /// <summary>
        /// Gets the exception message for the current error code.
        /// </summary>
        /// <returns>The exception message for the current error code.</returns>
        private static String GetExceptionMessage()
        {
            return GetExceptionMessage(BASS_ErrorGetCode());
        }

        /// <summary>
        /// Gets the exception message for the current error code.
        /// </summary>
        /// <returns>The exception message for the current error code.</returns>
        private static String GetExceptionMessage(Int32 code)
        {
            switch (code)
            {
                default:
                    String symbol;
                    if (BASSErrorCodes.TryGetValue(code, out symbol))
                    {
                        return "BASS error code " + symbol;
                    }
                    return "BASS error code " + code;
            }
        }

        // A table relating BASS error codes to their symbols in code.
        private static readonly Dictionary<Int32, String> BASSErrorCodes = new Dictionary<Int32, String>
        {
           {  0, "BASS_OK" },
           {  1, "BASS_ERROR_MEM" },
           {  2, "BASS_ERROR_FILEOPEN" },
           {  3, "BASS_ERROR_DRIVER" },
           {  4, "BASS_ERROR_BUFLOST" },
           {  5, "BASS_ERROR_HANDLE" }, 
           {  6, "BASS_ERROR_FORMAT" }, 
           {  7, "BASS_ERROR_POSITION" }, 
           {  8, "BASS_ERROR_INIT" },
           {  9, "BASS_ERROR_START" },
           { 14, "BASS_ERROR_ALREADY" },
           { 18, "BASS_ERROR_NOCHAN" },
           { 19, "BASS_ERROR_ILLTYPE" },
           { 20, "BASS_ERROR_ILLPARAM" }, 
           { 21, "BASS_ERROR_NO3D" },
           { 22, "BASS_ERROR_NOEAX" },
           { 23, "BASS_ERROR_DEVICE" },
           { 24, "BASS_ERROR_NOPLAY" },
           { 25, "BASS_ERROR_FREQ" },
           { 27, "BASS_ERROR_NOTFILE" },
           { 29, "BASS_ERROR_NOHW" },
           { 31, "BASS_ERROR_EMPTY" },
           { 32, "BASS_ERROR_NONET" },
           { 33, "BASS_ERROR_CREATE" },
           { 34, "BASS_ERROR_NOFX" },
           { 37, "BASS_ERROR_NOTAVAIL" },
           { 38, "BASS_ERROR_DECODE" },
           { 39, "BASS_ERROR_DX" },
           { 40, "BASS_ERROR_TIMEOUT" },
           { 41, "BASS_ERROR_FILEFORM" },
           { 42, "BASS_ERROR_SPEAKER" },
           { 43, "BASS_ERROR_VERSION" },
           { 44, "BASS_ERROR_CODEC" },
           { 45, "BASS_ERROR_ENDED" },
           { 46, "BASS_ERROR_BUSY" },
           { -1, "BASS_ERROR_UNKNOWN" },
        };
    }
}
