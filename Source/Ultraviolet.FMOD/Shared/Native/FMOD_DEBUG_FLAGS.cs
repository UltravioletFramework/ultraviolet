using System;

namespace Ultraviolet.FMOD.Native
{
#pragma warning disable 1591
    [Flags]
    public enum FMOD_DEBUG_FLAGS
    {
        FMOD_DEBUG_LEVEL_NONE           = 0x00000000,    /* Disable all messages */
        FMOD_DEBUG_LEVEL_ERROR          = 0x00000001,    /* Enable only error messages. */
        FMOD_DEBUG_LEVEL_WARNING        = 0x00000002,    /* Enable warning and error messages. */
        FMOD_DEBUG_LEVEL_LOG            = 0x00000004,    /* Enable informational, warning and error messages (default). */
        FMOD_DEBUG_TYPE_MEMORY          = 0x00000100,    /* Verbose logging for memory operations, only use this if you are debugging a memory related issue. */
        FMOD_DEBUG_TYPE_FILE            = 0x00000200,    /* Verbose logging for file access, only use this if you are debugging a file related issue. */
        FMOD_DEBUG_TYPE_CODEC           = 0x00000400,    /* Verbose logging for codec initialization, only use this if you are debugging a codec related issue. */
        FMOD_DEBUG_TYPE_TRACE           = 0x00000800,    /* Verbose logging for internal errors, use this for tracking the origin of error codes. */
        FMOD_DEBUG_DISPLAY_TIMESTAMPS   = 0x00010000,    /* Display the time stamp of the log message in milliseconds. */
        FMOD_DEBUG_DISPLAY_LINENUMBERS  = 0x00020000,    /* Display the source code file and line number for where the message originated. */
        FMOD_DEBUG_DISPLAY_THREAD       = 0x00040000,    /* Display the thread ID of the calling function that generated the message. */
    }
#pragma warning restore 1591
}
