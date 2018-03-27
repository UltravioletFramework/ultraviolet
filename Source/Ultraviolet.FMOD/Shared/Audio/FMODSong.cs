using System;
using System.Runtime.InteropServices;
using System.Text;
using Ultraviolet.Audio;
using Ultraviolet.Core;
using Ultraviolet.FMOD.Native;
using static Ultraviolet.FMOD.Native.FMOD_MODE;
using static Ultraviolet.FMOD.Native.FMOD_RESULT;
using static Ultraviolet.FMOD.Native.FMOD_TAGDATATYPE;
using static Ultraviolet.FMOD.Native.FMODNative;

namespace Ultraviolet.FMOD.Audio
{
    /// <summary>
    /// Represents the FMOD implementation of the <see cref="Song"/> class.
    /// </summary>
    public sealed unsafe class FMODSong : Song
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FMODSong"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="file">The path to the file from which to stream the song.</param>
        public FMODSong(UltravioletContext uv, String file)
            : base(uv)
        {
            Contract.RequireNotEmpty(file, nameof(file));

            var result = default(FMOD_RESULT);
            var system = ((FMODUltravioletAudio)uv.GetAudio()).System;

            // Load song as a sound
            fixed (FMOD_SOUND** psound = &sound)
            {
                var exinfo = new FMOD_CREATESOUNDEXINFO();
                exinfo.cbsize = Marshal.SizeOf(exinfo);
                                
                result = FMOD_System_CreateStream(system, file, FMOD_LOOP_NORMAL | FMOD_2D | FMOD_3D_WORLDRELATIVE | FMOD_3D_INVERSEROLLOFF, &exinfo, psound);
                if (result != FMOD_OK)
                    throw new FMODException(result);
            }

            this.duration = GetDuration(sound);
            this.tags = GetTags(sound, out name, out artist, out album);
        }

        /// <inheritdoc/>
        public override SongTagCollection Tags => tags;

        /// <inheritdoc/>
        public override String Name => name;

        /// <inheritdoc/>
        public override String Artist => artist;

        /// <inheritdoc/>
        public override String Album => album;

        /// <inheritdoc/>
        public override TimeSpan Duration => duration;

        /// <summary>
        /// Gets the FMOD sound pointer for this object.
        /// </summary>
        internal FMOD_SOUND* Sound => sound;

        /// <summary>
        /// Gets the FMOD channel group for this object.
        /// </summary>
        internal FMOD_CHANNELGROUP* ChannelGroup => ((FMODUltravioletAudio)Ultraviolet.GetAudio()).ChannelGroupSongs;

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            var result = FMOD_Sound_Release(sound);
            if (result != FMOD_OK)
                throw new FMODException(result);

            base.Dispose(disposing);
        }
        
        /// <summary>
        /// Gets the duration of the specified sound.
        /// </summary>
        private static TimeSpan GetDuration(FMOD_SOUND* sound)
        {
            var durationInMilliseconds = 0u;
            var result = FMOD_Sound_GetLength(sound, &durationInMilliseconds, FMOD_TIMEUNIT.FMOD_TIMEUNIT_MS);
            if (result != FMOD_OK)
                throw new FMODException(result);

            return TimeSpan.FromMilliseconds(durationInMilliseconds);
        }

        /// <summary>
        /// Gets the collection of tags associated with the specified sound.
        /// </summary>
        private static SongTagCollection GetTags(FMOD_SOUND* sound, out String name, out String artist, out String album)
        {
            var numtags = 0;
            var result = default(FMOD_RESULT);

            result = FMOD_Sound_GetNumTags(sound, &numtags, null);
            if (result != FMOD_OK)
                throw new FMODException(result);

            var tags = new SongTagCollection();

            for (int i = 0; i < numtags; i++)
            {
                var tag = GetTag(sound, i);
                if (tag != null)
                {
                    tags.Add(tag);
                }
            }

            name =
                tags["name"]?.Value ??
                tags["title"]?.Value;

            artist =
                tags["artist"]?.Value;

            album =
                tags["album"]?.Value;

            return tags;
        }

        /// <summary>
        /// Gets the tag at the specified index within the specified sound.
        /// </summary>
        private static SongTag GetTag(FMOD_SOUND* sound, Int32 index)
        {
            var tag = default(FMOD_TAG);
            var result = default(FMOD_RESULT);

            result = FMOD_Sound_GetTag(sound, null, index, &tag);
            if (result != FMOD_OK)
                throw new FMODException(result);

            var tagname = Marshal.PtrToStringAnsi(tag.name);
            var tagvalue = String.Empty;

            String GetTagString(IntPtr ptr, UInt32 length, Encoding encoding)
            {
                var buffer = new byte[length];
                for (int offset = 0; offset < length; offset++)
                    buffer[offset] = Marshal.ReadByte(ptr, offset);

                var str = encoding.GetString(buffer);
                var nul = str.IndexOf('\0');
                return nul < 0 ? str : str.Substring(0, nul);
            }

            switch (tag.datatype)
            {
                case FMOD_TAGDATATYPE_INT:
                    tagvalue = (*(int*)tag.data).ToString();
                    break;
                case FMOD_TAGDATATYPE_FLOAT:
                    tagvalue = (*(float*)tag.data).ToString();
                    break;
                case FMOD_TAGDATATYPE_STRING:
                    tagvalue = GetTagString(tag.data, tag.datalen, Encoding.ASCII);
                    break;
                case FMOD_TAGDATATYPE_STRING_UTF16:
                    tagvalue = GetTagString(tag.data, tag.datalen, Encoding.Unicode);
                    break;
                case FMOD_TAGDATATYPE_STRING_UTF16BE:
                    tagvalue = GetTagString(tag.data, tag.datalen, Encoding.BigEndianUnicode);
                    break;
                case FMOD_TAGDATATYPE_STRING_UTF8:
                    tagvalue = GetTagString(tag.data, tag.datalen, Encoding.UTF8);
                    break;
                default:
                    return null;
            }

            return new SongTag(tagname, tagvalue);
        }

        // FMOD state variables.
        private readonly FMOD_SOUND* sound;

        // Property values.
        private readonly SongTagCollection tags;
        private readonly String name;
        private readonly String artist;
        private readonly String album;
        private readonly TimeSpan duration;
    }
}
