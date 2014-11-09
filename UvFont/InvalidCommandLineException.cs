using System;

namespace TwistedLogik.UvFont
{
    public class InvalidCommandLineException : Exception
    {
        public InvalidCommandLineException() { }
        public InvalidCommandLineException(String error) { Error = error; }

        public String Error
        {
            get;
            private set;
        }
    }
}
