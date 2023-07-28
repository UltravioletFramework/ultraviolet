using System;
using static Ultraviolet.BASS.Native.BASSNative;

namespace Ultraviolet.BASS.Audio
{
    /// <summary>
    /// Contains methods for manipulating BASS resources.
    /// </summary>
    public static class BASSUtil
    {
        /// <summary>
        /// Gets a value indicating whether the specified value is a valid resource handle.
        /// </summary>
        /// <param name="handle">The handle to evaluate.</param>
        /// <returns>true if the specified value is a valid resource handle; otherwise, false.</returns>
        public static Boolean IsValidHandle(UInt32 handle) => handle != 0;

        /// <summary>
        /// Gets a value indicating whether the specified value is a valid return value.
        /// </summary>
        /// <param name="value">The value to evaluate.</param>
        /// <returns>true if the specified value is a valid return value; otherwise, false.</returns>
        public static Boolean IsValidValue(UInt32 value) => value != unchecked((UInt32)(-1));

        /// <summary>
        /// Gets a value indicating whether the specified value is a valid return value.
        /// </summary>
        /// <param name="value">The value to evaluate.</param>
        /// <returns>true if the specified value is a valid return value; otherwise, false.</returns>
        public static Boolean IsValidValue(UInt64 value) => value != unchecked((UInt64)(-1));

        /// <summary>
        /// Gets the duration of the specified channel in seconds.
        /// </summary>
        /// <param name="handle">The handle of the channel to evaluate.</param>
        /// <returns>The duration of the specified channel in seconds.</returns>
        public static Double GetDurationInSeconds(UInt32 handle)
        {
            var length = BASS_ChannelGetLength(handle, 0);
            if (!IsValidValue(length))
                throw new BASSException();

            var seconds = BASS_ChannelBytes2Seconds(handle, length);
            if (seconds < 0)
                throw new BASSException();

            return seconds;
        }

        /// <summary>
        /// Gets the position of the specified channel.
        /// </summary>
        /// <param name="handle">The handle of the channel to evaluate.</param>
        /// <returns>The current position of the channel in seconds.</returns>
        public static Double GetPositionInSeconds(UInt32 handle)
        {
            var position = BASS_ChannelGetPosition(handle, 0);
            if (!IsValidValue(position))
                throw new BASSException();

            var seconds = BASS_ChannelBytes2Seconds(handle, position);
            if (seconds < 0)
                throw new BASSException();

            return seconds;
        }

        /// <summary>
        /// Gets the duration of the specified channel as a <see cref="TimeSpan"/> value.
        /// </summary>
        /// <param name="handle">The handle of the channel to evaluate.</param>
        /// <returns>The duration of the specified channel.</returns>
        public static TimeSpan GetDurationAsTimeSpan(UInt32 handle)
        {
            return TimeSpan.FromSeconds(GetDurationInSeconds(handle));
        }

        /// <summary>
        /// Gets the position of the specified channel as a <see cref="TimeSpan"/> value.
        /// </summary>
        /// <param name="handle">The handle of the channel to evaluate.</param>
        /// <returns>The current position of the channel.</returns>
        public static TimeSpan GetPositionAsTimeSpan(UInt32 handle)
        {
            return TimeSpan.FromSeconds(GetPositionInSeconds(handle));
        }

        /// <summary>
        /// Sets the position of the specified channel.
        /// </summary>
        /// <param name="handle">The handle of the channel to modify.</param>
        /// <param name="position">The position in seconds to which to set the channel.</param>
        public static void SetPositionInSeconds(UInt32 handle, Double position)
        {
            var positionInBytes = BASS_ChannelSeconds2Bytes(handle, position);
            if (!IsValidValue(positionInBytes))
                throw new BASSException();

            if (!BASS_ChannelSetPosition(handle, positionInBytes, 0))
                throw new BASSException();
        }

        /// <summary>
        /// Gets a value indicating whether the specified channel is looping.
        /// </summary>
        /// <param name="handle">The handle of the channel to evaluate.</param>
        /// <returns>true if the channel is looping; otherwise, false.</returns>
        public static Boolean GetIsLooping(UInt32 handle)
        {
            var flags = BASS_ChannelFlags(handle, 0, 0);
            if (!BASSUtil.IsValidValue(flags))
                throw new BASSException();

            return (flags & BASS_SAMPLE_LOOP) == BASS_SAMPLE_LOOP;
        }

        /// <summary>
        /// Sets a value indicating whether the specified channel is looping.
        /// </summary>
        /// <param name="handle">The handle of the channel to modify.</param>
        /// <param name="looping">A value indicating whether the channel is looping.</param>
        public static void SetIsLooping(UInt32 handle, Boolean looping)
        {
            var flags = looping ?
                    BASS_ChannelFlags(handle, BASS_SAMPLE_LOOP, BASS_SAMPLE_LOOP) :
                    BASS_ChannelFlags(handle, 0, BASS_SAMPLE_LOOP);

            if (!IsValidValue(flags))
                throw new BASSException();
        }

        /// <summary>
        /// Gets the volume of the specified channel.
        /// </summary>
        /// <param name="handle">The handle that represents the channel to evaluate.</param>
        /// <returns>The channel's volume.</returns>
        public static Single GetVolume(UInt32 handle)
        {
            unsafe
            {
                Single value;
                if (!BASS_ChannelGetAttribute(handle, BASS_ATTRIB_VOL, &value))
                    throw new BASSException();
                return value;
            }
        }

        /// <summary>
        /// Sets the volume of the specified channel.
        /// </summary>
        /// <param name="handle">The handle that represents the channel to adjust.</param>
        /// <param name="volume">The channel's new volume./</param>
        public static void SetVolume(UInt32 handle, Single volume)
        {
            if (BASS_ChannelIsSliding(handle, BASS_ATTRIB_VOL))
            {
                if (!BASS_ChannelSlideAttribute(handle, BASS_ATTRIB_VOL, volume, 0))
                    throw new BASSException();
            }
            else
            {
                if (!BASS_ChannelSetAttribute(handle, BASS_ATTRIB_VOL, volume))
                    throw new BASSException();
            }
        }

        /// <summary>
        /// Gets the pan of the specified channel.
        /// </summary>
        /// <param name="handle">The handle that represents the channel to evaluate.</param>
        /// <returns>The channel's pan.</returns>
        public static Single GetPan(UInt32 handle)
        {
            unsafe
            {
                Single value;
                if (!BASS_ChannelGetAttribute(handle, BASS_ATTRIB_PAN, &value))
                    throw new BASSException();
                return value;
            }
        }

        /// <summary>
        /// Sets the pan of the specified channel.
        /// </summary>
        /// <param name="handle">The handle that represents the channel to adjust.</param>
        /// <param name="pan">The channel's new pan.</param>
        public static void SetPan(UInt32 handle, Single pan)
        {
            if (BASS_ChannelIsSliding(handle, BASS_ATTRIB_PAN))
            {
                if (!BASS_ChannelSlideAttribute(handle, BASS_ATTRIB_PAN, pan, 0))
                    throw new BASSException();
            }
            else
            {
                if (!BASS_ChannelSetAttribute(handle, BASS_ATTRIB_PAN, pan))
                    throw new BASSException();
            }
        }

        /// <summary>
        /// Slides the volume of the specified channel.
        /// </summary>
        /// <param name="handle">The handle that represents the channel to slide.</param>
        /// <param name="volume">The channel's new volume./</param>
        /// <param name="time">The time over which to perform the slide.</param>
        public static void SlideVolume(UInt32 handle, Single volume, TimeSpan time)
        {
            if (!BASS_ChannelSlideAttribute(handle, BASS_ATTRIB_VOL, volume, (uint)time.TotalMilliseconds))
                throw new BASSException();
        }

        /// <summary>
        /// Slides the pan of the specified channel.
        /// </summary>
        /// <param name="handle">The handle that represents the channel to slide.</param>
        /// <param name="pan">The channel's new pan.</param>
        /// <param name="time">The time over which to perform the slide.</param>
        public static void SlidePan(UInt32 handle, Single pan, TimeSpan time)
        {
            if (!BASS_ChannelSlideAttribute(handle, BASS_ATTRIB_PAN, pan, (uint)time.TotalMilliseconds))
                throw new BASSException();
        }

        /// <summary>
        /// The number of semitones in one octave.
        /// </summary>
        public const Single SemitonesPerOctave = 12f;
    }
}
