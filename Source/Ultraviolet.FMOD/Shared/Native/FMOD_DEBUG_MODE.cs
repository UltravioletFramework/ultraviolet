namespace Ultraviolet.FMOD.Native
{
#pragma warning disable 1591
    public enum FMOD_DEBUG_MODE
    {
        FMOD_DEBUG_MODE_TTY,             /* Default log location per platform, i.e. Visual Studio output window, stderr, LogCat, etc */
        FMOD_DEBUG_MODE_FILE,            /* Write log to specified file path */
        FMOD_DEBUG_MODE_CALLBACK,        /* Call specified callback with log information */
    }
#pragma warning restore 1591
}
