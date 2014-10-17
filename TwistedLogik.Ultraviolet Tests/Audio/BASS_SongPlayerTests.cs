using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Ultraviolet.Testing;
using TwistedLogik.Ultraviolet.Audio;

namespace TwistedLogik.Ultraviolet.Tests.Audio
{
    [TestClass]
    [DeploymentItem(@"Resources")]
    [DeploymentItem(@"Dependencies")]
    public class BASS_SongPlayerTests : UltravioletApplicationTestFramework
    {
        [TestMethod]
        [TestCategory("Audio")]
        public void BASS_SongPlayer_PlaySetsVolumePitchAndPan()
        {
            var songPlayer = default(SongPlayer);
            var song       = default(Song);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithInitialization(uv => uv.GetAudio().AudioMuted = true)
                .WithContent(content =>
                {
                    songPlayer = SongPlayer.Create();
                    song       = content.Load<Song>("Songs/Deep Haze");

                    songPlayer.Play(song, 0.25f, 0.50f, 0.75f, false);

                    TheResultingValue(songPlayer.Volume).ShouldBe(0.25f);
                    TheResultingValue(songPlayer.Pitch).ShouldBe(0.50f);
                    TheResultingValue(songPlayer.Pan).ShouldBe(0.75f);
                })
                .RunForOneFrame();
        }

        [TestMethod]
        [TestCategory("Audio")]
        public void BASS_SongPlayer_PlayResetsVolumePitchAndPanWhenNotSpecified()
        {
            var songPlayer = default(SongPlayer);
            var song       = default(Song);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithInitialization(uv => uv.GetAudio().AudioMuted = true)
                .WithContent(content =>
                {
                    songPlayer = SongPlayer.Create();
                    song       = content.Load<Song>("Songs/Deep Haze");

                    songPlayer.Play(song, 0.25f, 0.50f, 0.75f, false);
                    songPlayer.Stop();
                    songPlayer.Play(song, false);

                    TheResultingValue(songPlayer.Volume).ShouldBe(1f);
                    TheResultingValue(songPlayer.Pitch).ShouldBe(0f);
                    TheResultingValue(songPlayer.Pan).ShouldBe(0f);
                })
                .RunForOneFrame();
        }

        [TestMethod]
        [TestCategory("Audio")]
        public void BASS_SongPlayer_SlidesVolumeCorrectly()
        {
            var songPlayer = default(SongPlayer);
            var song       = default(Song);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithInitialization(uv => uv.GetAudio().AudioMuted = true)
                .WithContent(content =>
                {
                    songPlayer = SongPlayer.Create();
                    song       = content.Load<Song>("Songs/Deep Haze");

                    songPlayer.Play(song, false);

                    TheResultingValue(songPlayer.Volume).ShouldBe(1f);

                    songPlayer.SlideVolume(0f, TimeSpan.FromSeconds(1));
                })
                .RunFor(TimeSpan.FromSeconds(2));

            TheResultingValue(songPlayer.Volume).ShouldBe(0f);
        }

        [TestMethod]
        [TestCategory("Audio")]
        public void BASS_SongPlayer_SlidesPitchCorrectly()
        {
            var songPlayer = default(SongPlayer);
            var song       = default(Song);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithInitialization(uv => uv.GetAudio().AudioMuted = true)
                .WithContent(content =>
                {
                    songPlayer = SongPlayer.Create();
                    song       = content.Load<Song>("Songs/Deep Haze");

                    songPlayer.Play(song, false);

                    TheResultingValue(songPlayer.Pitch).ShouldBe(0f);

                    songPlayer.SlidePitch(-1f, TimeSpan.FromSeconds(1));
                })
                .RunFor(TimeSpan.FromSeconds(2));

            TheResultingValue(songPlayer.Pitch).ShouldBe(-1f);
        }

        [TestMethod]
        [TestCategory("Audio")]
        public void BASS_SongPlayer_SlidesPanCorrectly()
        {
            var songPlayer = default(SongPlayer);
            var song       = default(Song);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithInitialization(uv => uv.GetAudio().AudioMuted = true)
                .WithContent(content =>
                {
                    songPlayer = SongPlayer.Create();
                    song       = content.Load<Song>("Songs/Deep Haze");

                    songPlayer.Play(song, false);

                    TheResultingValue(songPlayer.Pan).ShouldBe(0f);

                    songPlayer.SlidePan(-1f, TimeSpan.FromSeconds(1));
                })
                .RunFor(TimeSpan.FromSeconds(2));

            TheResultingValue(songPlayer.Pan).ShouldBe(-1f);
        }

        [TestMethod]
        [TestCategory("Audio")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void BASS_SongPlayer_ThrowsExceptionIfVolumeSetWhileNotPlaying()
        {
            var songPlayer = default(SongPlayer);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithInitialization(uv => uv.GetAudio().AudioMuted = true)
                .WithContent(content =>
                {
                    songPlayer = SongPlayer.Create();
                    songPlayer.Volume = 0f;
                })
                .RunFor(TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        [TestCategory("Audio")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void BASS_SongPlayer_ThrowsExceptionIfPitchSetWhileNotPlaying()
        {
            var songPlayer = default(SongPlayer);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithInitialization(uv => uv.GetAudio().AudioMuted = true)
                .WithContent(content =>
                {
                    songPlayer = SongPlayer.Create();
                    songPlayer.Pitch = -1f;
                })
                .RunFor(TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        [TestCategory("Audio")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void BASS_SongPlayer_ThrowsExceptionIfPanSetWhileNotPlaying()
        {
            var songPlayer = default(SongPlayer);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithInitialization(uv => uv.GetAudio().AudioMuted = true)
                .WithContent(content =>
                {
                    songPlayer = SongPlayer.Create();
                    songPlayer.Pan = 0f;
                })
                .RunFor(TimeSpan.FromSeconds(1));
        }
    }
}
