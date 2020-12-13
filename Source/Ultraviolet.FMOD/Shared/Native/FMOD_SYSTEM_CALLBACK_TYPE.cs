using System;

namespace Ultraviolet.FMOD.Native
{
#pragma warning disable 1591
    [Flags]
    public enum FMOD_SYSTEM_CALLBACK_TYPE : uint
    {
        DEVICELISTCHANGED      = 0x00000001,  /* Called from System::update when the enumerated list of devices has changed. */
        DEVICELOST             = 0x00000002,  /* Called from System::update when an output device has been lost due to control panel parameter changes and FMOD cannot automatically recover. */
        MEMORYALLOCATIONFAILED = 0x00000004,  /* Called directly when a memory allocation fails somewhere in FMOD.  (NOTE - 'system' will be NULL in this callback type.)*/
        THREADCREATED          = 0x00000008,  /* Called directly when a thread is created. (NOTE - 'system' will be NULL in this callback type.) */
        BADDSPCONNECTION       = 0x00000010,  /* Called when a bad connection was made with DSP::addInput. Usually called from mixer thread because that is where the connections are made.  */
        PREMIX                 = 0x00000020,  /* Called each tick before a mix update happens. */
        POSTMIX                = 0x00000040,  /* Called each tick after a mix update happens. */
        ERROR                  = 0x00000080,  /* Called when each API function returns an error code, including delayed async functions. */
        MIDMIX                 = 0x00000100,  /* Called each tick in mix update after clocks have been updated before the main mix occurs. */
        THREADDESTROYED        = 0x00000200,  /* Called directly when a thread is destroyed. */
        PREUPDATE              = 0x00000400,  /* Called at start of System::update function. */
        POSTUPDATE             = 0x00000800,  /* Called at end of System::update function. */
        RECORDLISTCHANGED      = 0x00001000,  /* Called from System::update when the enumerated list of recording devices has changed. */
        ALL                    = 0xFFFFFFFF,  /* Pass this mask to System::setCallback to receive all callback types.  */
    }
#pragma warning restore 1591
}