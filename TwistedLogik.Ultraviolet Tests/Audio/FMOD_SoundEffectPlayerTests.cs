using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Ultraviolet.Audio;
using TwistedLogik.Ultraviolet.FMOD;
using TwistedLogik.Ultraviolet.Testing;

namespace TwistedLogik.Ultraviolet.Tests.Audio
{
    [Ignore]
    [TestClass]
    [DeploymentItem(@"Resources")]
    [DeploymentItem(@"Dependencies")]
    public class FMOD_SoundEffectPlayerTests : UltravioletApplicationTestFramework
    {
        [TestMethod]
        [TestCategory("Audio")]
        public void FMOD_SoundEffectPlayer_PlaySetsVolumePitchAndPan()
        {
            var sfxPlayer = default(SoundEffectPlayer);
            var sfx       = default(SoundEffect);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithAudioSubsystem(typeof(FMODUltravioletAudio).Assembly.FullName)
                .WithInitialization(uv => uv.GetAudio().AudioMuted = true)
                .WithContent(content =>
                {
                    sfxPlayer = SoundEffectPlayer.Create();
                    sfx       = content.Load<SoundEffect>("SoundEffects/grenade");

                    sfxPlayer.Play(sfx, 0.25f, 0.50f, 0.75f, false);

                    TheResultingValue(sfxPlayer.Volume).ShouldBe(0.25f);
                    TheResultingValue(sfxPlayer.Pitch).ShouldBe(0.50f);
                    TheResultingValue(sfxPlayer.Pan).ShouldBe(0.75f);
                })
                .RunForOneFrame();
        }

        [TestMethod]
        [TestCategory("Audio")]
        public void FMOD_SoundEffectPlayer_PlayResetsVolumePitchAndPanWhenNotSpecified()
        {
            var sfxPlayer = default(SoundEffectPlayer);
            var sfx       = default(SoundEffect);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithAudioSubsystem(typeof(FMODUltravioletAudio).Assembly.FullName)
                .WithInitialization(uv => uv.GetAudio().AudioMuted = true)
                .WithContent(content =>
                {
                    sfxPlayer = SoundEffectPlayer.Create();
                    sfx       = content.Load<SoundEffect>("SoundEffects/grenade");

                    sfxPlayer.Play(sfx, 0.25f, 0.50f, 0.75f, false);
                    sfxPlayer.Stop();
                    sfxPlayer.Play(sfx, false);

                    TheResultingValue(sfxPlayer.Volume).ShouldBe(1f);
                    TheResultingValue(sfxPlayer.Pitch).ShouldBe(0f);
                    TheResultingValue(sfxPlayer.Pan).ShouldBe(0f);
                })
                .RunForOneFrame();
        }

        [TestMethod]
        [TestCategory("Audio")]
        public void FMOD_SoundEffectPlayer_SlidesVolumeCorrectly()
        {
            var sfxPlayer = default(SoundEffectPlayer);
            var sfx       = default(SoundEffect);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithAudioSubsystem(typeof(FMODUltravioletAudio).Assembly.FullName)
                .WithInitialization(uv => uv.GetAudio().AudioMuted = true)
                .WithContent(content =>
                {
                    sfxPlayer = SoundEffectPlayer.Create();
                    sfx       = content.Load<SoundEffect>("SoundEffects/grenade");

                    sfxPlayer.Play(sfx, false);

                    TheResultingValue(sfxPlayer.Volume).ShouldBe(1f);

                    sfxPlayer.SlideVolume(0f, TimeSpan.FromSeconds(1));
                })
                .RunFor(TimeSpan.FromSeconds(2));

            TheResultingValue(sfxPlayer.Volume).ShouldBe(0f);
        }

        [TestMethod]
        [TestCategory("Audio")]
        public void FMOD_SoundEffectPlayer_SlidesPitchCorrectly()
        {
            var sfxPlayer = default(SoundEffectPlayer);
            var sfx       = default(SoundEffect);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithAudioSubsystem(typeof(FMODUltravioletAudio).Assembly.FullName)
                .WithInitialization(uv => uv.GetAudio().AudioMuted = true)
                .WithContent(content =>
                {
                    sfxPlayer = SoundEffectPlayer.Create();
                    sfx       = content.Load<SoundEffect>("SoundEffects/grenade");

                    sfxPlayer.Play(sfx, false);

                    TheResultingValue(sfxPlayer.Pitch).ShouldBe(0f);

                    sfxPlayer.SlidePitch(-1f, TimeSpan.FromSeconds(1));
                })
                .RunFor(TimeSpan.FromSeconds(2));

            TheResultingValue(sfxPlayer.Pitch).ShouldBe(-1f);
        }

        [TestMethod]
        [TestCategory("Audio")]
        public void FMOD_SoundEffectPlayer_SlidesPanCorrectly()
        {
            var sfxPlayer = default(SoundEffectPlayer);
            var sfx       = default(SoundEffect);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithAudioSubsystem(typeof(FMODUltravioletAudio).Assembly.FullName)
                .WithInitialization(uv => uv.GetAudio().AudioMuted = true)
                .WithContent(content =>
                {
                    sfxPlayer = SoundEffectPlayer.Create();
                    sfx       = content.Load<SoundEffect>("SoundEffects/grenade");

                    sfxPlayer.Play(sfx, false);

                    TheResultingValue(sfxPlayer.Pan).ShouldBe(0f);

                    sfxPlayer.SlidePan(-1f, TimeSpan.FromSeconds(1));
                })
                .RunFor(TimeSpan.FromSeconds(2));

            TheResultingValue(sfxPlayer.Pan).ShouldBe(-1f);
        }

        [TestMethod]
        [TestCategory("Audio")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void FMOD_SoundEffectPlayer_ThrowsExceptionIfVolumeSetWhileNotPlaying()
        {
            var sfxPlayer = default(SoundEffectPlayer);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithAudioSubsystem(typeof(FMODUltravioletAudio).Assembly.FullName)
                .WithInitialization(uv => uv.GetAudio().AudioMuted = true)
                .WithContent(content =>
                {
                    sfxPlayer = SoundEffectPlayer.Create();
                    sfxPlayer.Volume = 0f;
                })
                .RunFor(TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        [TestCategory("Audio")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void FMOD_SoundEffectPlayer_ThrowsExceptionIfPitchSetWhileNotPlaying()
        {
            var sfxPlayer = default(SoundEffectPlayer);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithAudioSubsystem(typeof(FMODUltravioletAudio).Assembly.FullName)
                .WithInitialization(uv => uv.GetAudio().AudioMuted = true)
                .WithContent(content =>
                {
                    sfxPlayer = SoundEffectPlayer.Create();
                    sfxPlayer.Pitch = -1f;
                })
                .RunFor(TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        [TestCategory("Audio")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void FMOD_SoundEffectPlayer_ThrowsExceptionIfPanSetWhileNotPlaying()
        {
            var sfxPlayer = default(SoundEffectPlayer);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithAudioSubsystem(typeof(FMODUltravioletAudio).Assembly.FullName)
                .WithInitialization(uv => uv.GetAudio().AudioMuted = true)
                .WithContent(content =>
                {
                    sfxPlayer = SoundEffectPlayer.Create();
                    sfxPlayer.Pan = 0f;
                })
                .RunFor(TimeSpan.FromSeconds(1));
        }
    }
}
