using Java.Lang;
using Ultraviolet.Core.Messages;
using Ultraviolet.Messages;
using Ultraviolet.Platform;

namespace Ultraviolet.FMOD
{
    partial class FMODUltravioletAudio
    {
        partial void PlatformSpecificInitialization()
        {
            JavaSystem.LoadLibrary("fmod");

            var activity = AndroidActivityService.Create().Activity;
            if (activity != null)
                Org.Fmod.FMOD.Init(activity);
        }

        partial void PlatformSpecificMessageSubscriptions(UltravioletContext uv)
        {
            uv.Messages.Subscribe(this, UltravioletMessages.AndroidActivityCreate);
            uv.Messages.Subscribe(this, UltravioletMessages.AndroidActivityDestroy);
        }

        partial void PlatformSpecificMessageHandling(UltravioletMessageID type, MessageData data)
        {
            if (type == UltravioletMessages.AndroidActivityCreate)
            {
                var activity = ((AndroidLifecycleMessageData)data).Activity;
                Org.Fmod.FMOD.Init(activity);
                return;
            }

            if (type == UltravioletMessages.AndroidActivityDestroy)
            {
                Org.Fmod.FMOD.Close();
                return;
            }
        }
    }
}