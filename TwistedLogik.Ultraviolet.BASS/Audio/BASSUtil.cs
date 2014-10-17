using System;
using TwistedLogik.Ultraviolet.BASS.Native;

namespace TwistedLogik.Ultraviolet.BASS.Audio
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
        public static Boolean IsValidHandle(UInt32 handle)
        {
            return handle != 0;
        }

        /// <summary>
        /// Gets a value indicating whether the specified value is a valid return value.
        /// </summary>
        /// <param name="value">The value to evaluate.</param>
        /// <returns>true if the specified value is a valid return value; otherwise, false.</returns>
        public static Boolean IsValidValue(UInt32 value)
        {
            return value != unchecked((UInt32)(-1));
        }

        /// <summary>
        /// Gets a value indicating whether the specified value is a valid return value.
        /// </summary>
        /// <param name="value">The value to evaluate.</param>
        /// <returns>true if the specified value is a valid return value; otherwise, false.</returns>
        public static Boolean IsValidValue(UInt64 value)
        {
            return value != unchecked((UInt64)(-1));
        }

        /// <summary>
        /// Gets the duration of the specified channel in seconds.
        /// </summary>
        /// <param name="handle">The handle of the channel to evaluate.</param>
        /// <returns>The duration of the specified channel in seconds.</returns>
        public static Double GetDurationInSeconds(UInt32 handle)
        {
            var length = BASSNative.ChannelGetLength(handle, 0);
            if (!BASSUtil.IsValidValue(length))
                throw new BASSException();

            var seconds = BASSNative.ChannelBytes2Seconds(handle, length);
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
            var position = BASSNative.ChannelGetPosition(handle, 0);
            if (!BASSUtil.IsValidValue(position))
                throw new BASSException();

            var seconds = BASSNative.ChannelBytes2Seconds(handle, position);
            if (seconds < 0)
                throw new BASSException();

            return seconds;
        }

        /// <summary>
        /// Sets the position of the specified channel.
        /// </summary>
        /// <param name="handle">The handle of the channel to modify.</param>
        /// <param name="position">The position in seconds to which to set the channel.</param>
        public static void SetPositionInSeconds(UInt32 handle, Double position)
        {
            var positionInBytes = BASSNative.ChannelSeconds2Bytes(handle, position);
            if (!BASSUtil.IsValidValue(positionInBytes))
                throw new BASSException();

            if (!BASSNative.ChannelSetPosition(handle, positionInBytes, 0))
                throw new BASSException();
        }

        /// <summary>
        /// Gets a value indicating whether the specified channel is looping.
        /// </summary>
        /// <param name="handle">The handle of the channel to evaluate.</param>
        /// <returns>true if the channel is looping; otherwise, false.</returns>
        public static Boolean GetIsLooping(UInt32 handle)
        {
            var flags = BASSNative.ChannelFlags(handle, 0, 0);
            if (!BASSUtil.IsValidValue(flags))
                throw new BASSException();

            return (flags & BASSNative.BASS_SAMPLE_LOOP) == BASSNative.BASS_SAMPLE_LOOP;
        }

        /// <summary>
        /// Sets a value indicating whether the specified channel is looping.
        /// </summary>
        /// <param name="handle">The handle of the channel to modify.</param>
        /// <param name="looping">A value indicating whether the channel is looping.</param>
        public static void SetIsLooping(UInt32 handle, Boolean looping)
        {
            var flags = looping ?
                    BASSNative.ChannelFlags(handle, BASSNative.BASS_SAMPLE_LOOP, BASSNative.BASS_SAMPLE_LOOP) :
                    BASSNative.ChannelFlags(handle, 0, BASSNative.BASS_SAMPLE_LOOP);

            if (!BASSUtil.IsValidValue(flags))
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
                if (!BASSNative.ChannelGetAttribute(handle, BASSAttrib.ATTRIB_VOL, &value))
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
            if (BASSNative.ChannelIsSliding(handle, BASSAttrib.ATTRIB_VOL))
            {
                if (!BASSNative.ChannelSlideAttribute(handle, BASSAttrib.ATTRIB_VOL, volume, 0))
                    throw new BASSException();
            }
            else
            {
                if (!BASSNative.ChannelSetAttribute(handle, BASSAttrib.ATTRIB_VOL, volume))
                    throw new BASSException();
            }
        }

        /// <summary>
        /// Gets the pitch of the specified channel.
        /// </summary>
        /// <param name="handle">The handle that represents the channel to evaluate.</param>
        /// <returns>The channel's pitch.</returns>
        public static Single GetPitch(UInt32 handle)
        {
            unsafe
            {
                Single value;
                if (!BASSNative.ChannelGetAttribute(handle, BASSAttrib.ATTRIB_TEMPO_PITCH, &value))
                    throw new BASSException();
                return value / SemitonesPerOctave;
            }
        }

        /// <summary>
        /// Sets the pitch of the specified channel.
        /// </summary>
        /// <param name="handle">The handle that represents the channel to adjust.</param>
        /// <param name="pitch">The channel's new pitch.</param>
        public static void SetPitch(UInt32 handle, Single pitch)
        {
            if (BASSNative.ChannelIsSliding(handle, BASSAttrib.ATTRIB_TEMPO_PITCH))
            {
                if (!BASSNative.ChannelSlideAttribute(handle, BASSAttrib.ATTRIB_TEMPO_PITCH, pitch * SemitonesPerOctave, 0))
                    throw new BASSException();
            }
            else
            {
                if (!BASSNative.ChannelSetAttribute(handle, BASSAttrib.ATTRIB_TEMPO_PITCH, pitch * SemitonesPerOctave))
                    throw new BASSException();
            }

            var tempo = (Math.Pow(2.0, pitch) - 1.0) * 100.0;
            if (BASSNative.ChannelIsSliding(handle, BASSAttrib.ATTRIB_TEMPO))
            {
                if (!BASSNative.ChannelSlideAttribute(handle, BASSAttrib.ATTRIB_TEMPO, (float)tempo, 0))
                    throw new BASSException();
            }
            else
            {
                if (!BASSNative.ChannelSetAttribute(handle, BASSAttrib.ATTRIB_TEMPO, (float)tempo))
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
                if (!BASSNative.ChannelGetAttribute(handle, BASSAttrib.ATTRIB_PAN, &value))
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
            if (BASSNative.ChannelIsSliding(handle, BASSAttrib.ATTRIB_PAN))
            {
                if (!BASSNative.ChannelSlideAttribute(handle, BASSAttrib.ATTRIB_PAN, pan, 0))
                    throw new BASSException();
            }
            else
            {
                if (!BASSNative.ChannelSetAttribute(handle, BASSAttrib.ATTRIB_PAN, pan))
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
            if (!BASSNative.ChannelSlideAttribute(handle, BASSAttrib.ATTRIB_VOL, volume, (uint)time.TotalMilliseconds))
                throw new BASSException();
        }

        /// <summary>
        /// Slides the pitch of the specified channel.
        /// </summary>
        /// <param name="handle">The handle that represents the channel to slide.</param>
        /// <param name="pitch">The channel's new pitch.</param>
        /// <param name="time">The time over which to perform the slide.</param>
        public static void SlidePitch(UInt32 handle, Single pitch, TimeSpan time)
        {
            if (!BASSNative.ChannelSlideAttribute(handle, BASSAttrib.ATTRIB_TEMPO_PITCH, pitch * SemitonesPerOctave, (uint)time.TotalMilliseconds))
                throw new BASSException();

            var tempo = (Math.Pow(2.0, pitch) - 1.0) * 100.0;
            if (!BASSNative.ChannelSlideAttribute(handle, BASSAttrib.ATTRIB_TEMPO, (float)tempo, (uint)time.TotalMilliseconds))
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
            if (!BASSNative.ChannelSlideAttribute(handle, BASSAttrib.ATTRIB_PAN, pan, (uint)time.TotalMilliseconds))
                throw new BASSException();
        }

        /// <summary>
        /// The number of semitones in one octave.
        /// </summary>
        public const Single SemitonesPerOctave = 12f;
    }
}
