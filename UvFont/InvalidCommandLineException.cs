using System;

namespace TwistedLogik.UvFont
{
    public class InvalidCommandLineException : Exception
    {
        public InvalidCommandLineException() { }
        public InvalidCommandLineException(String message) : base(message) { }
    }
}
