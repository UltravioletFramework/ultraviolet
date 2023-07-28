using System;
using NUnit.Framework;
using Ultraviolet.Audio;
using Ultraviolet.TestApplication;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests.Audio
{
    [TestFixture]
    public class SongTests : UltravioletApplicationTestFramework
    {
        [Test]
        [TestCase(AudioImplementation.BASS)]
        [TestCase(AudioImplementation.FMOD)]
        [Category("Audio")]
        public void Song_LoadsTags_FromOggVorbisFile(AudioImplementation audioImplementation)
        {
            var song = default(Song);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithAudioImplementation(audioImplementation)
                .WithInitialization(uv => uv.GetAudio().AudioMuted = true)
                .WithContent(content =>
                {
                    song = content.Load<Song>("Songs/Tone");

                    TheResultingString(song.Tags["CUSTOM_TAG1"].Value)
                        .ShouldBe("Hello, world!");

                    TheResultingString(song.Tags["CUSTOM_TAG2"].Value)
                        .ShouldBe("1234");

                    TheResultingValue(song.Tags["CUSTOM_TAG2"].As<Int32>())
                        .ShouldBe(1234);
                })
                .RunForOneFrame();
        }
    }
}
