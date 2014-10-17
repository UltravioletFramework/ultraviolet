using System;

namespace TwistedLogik.Ultraviolet.FMOD
{
    /// <summary>
    /// Contains utility methods for working with the FMOD API.
    /// </summary>
    public static class FMODUtil
    {
        /// <summary>
        /// Checks the result of an FMOD API call.
        /// </summary>
        /// <param name="result">The result value to check.</param>
        public static void CheckResult(FMODNative.RESULT result)
        {
            if (result != FMODNative.RESULT.OK)
            {
                throw new FMODException(result);
            }
        }

        /// <summary>
        /// Sets the loop mode of the specified channel.
        /// </summary>
        /// <param name="channel">The channel to update.</param>
        /// <param name="looping">A value indicating whether the channel should loop.</param>
        public static void SetLooping(FMODNative.Channel channel, Boolean looping)
        {
            FMODNative.RESULT result;
            FMODNative.MODE mode;

            result = channel.getMode(out mode);
            FMODUtil.CheckResult(result);

            if (looping)
            {
                mode = (mode & ~FMODNative.MODE.LOOP_OFF) | FMODNative.MODE.LOOP_NORMAL;
            }
            else
            {
                mode = (mode & ~FMODNative.MODE.LOOP_NORMAL) | FMODNative.MODE.LOOP_OFF;
            }

            result = channel.setMode(mode);
            FMODUtil.CheckResult(result);
        }

        /// <summary>
        /// Sets the volume of the specified channel.
        /// </summary>
        /// <param name="channel">The channel to update.</param>
        /// <param name="volume">The volume value to set.</param>
        public static void SetVolume(FMODNative.Channel channel, Single volume)
        {
            var result = channel.setVolume(volume);
            FMODUtil.CheckResult(result);
        }

        /// <summary>
        /// Sets the pitch of the specified channel.
        /// </summary>
        /// <param name="channel">The channel to update.</param>
        /// <param name="frequency">The original frequency of the sound.</param>
        /// <param name="pitch">The pitch value to set.</param>
        public static void SetPitch(FMODNative.Channel channel, Single frequency, Single pitch)
        {
            var frequencyPercentAdjustment = 1f + (Math.Pow(2.0, pitch) - 1.0);
            var result = channel.setFrequency((float)(frequency * frequencyPercentAdjustment));
            FMODUtil.CheckResult(result);
        }

        /// <summary>
        /// Sets the pan of the specified channel.
        /// </summary>
        /// <param name="channel">The channel to update.</param>
        /// <param name="pan">The pan value to set.</param>
        public static void SetPan(FMODNative.Channel channel, Single pan)
        {
            var result = channel.setPan(pan);
            FMODUtil.CheckResult(result);
        }
    }
}
