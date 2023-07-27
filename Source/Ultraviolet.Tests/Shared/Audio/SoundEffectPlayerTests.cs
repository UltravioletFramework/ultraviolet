using System;
using NUnit.Framework;
using Ultraviolet.Audio;
using Ultraviolet.TestApplication;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests.Audio
{
    [TestFixture]
    public class SoundEffectPlayerTests : UltravioletApplicationTestFramework
    {
        [Test]
        [TestCase(AudioImplementation.BASS)]
        [TestCase(AudioImplementation.FMOD)]
        [Category("Audio")]
        public void SoundEffectPlayer_PlaySetsVolumePitchAndPan(AudioImplementation audioImplementation)
        {
            var sfxPlayer = default(SoundEffectPlayer);
            var sfx = default(SoundEffect);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithAudioImplementation(audioImplementation)
                .WithInitialization(uv => uv.GetAudio().AudioMuted = true)
                .WithContent(content =>
                {
                    sfxPlayer = SoundEffectPlayer.Create();
                    sfx = content.Load<SoundEffect>("SoundEffects/grenade");

                    sfxPlayer.Play(sfx, 0.25f, 0.50f, 0.75f, false);

                    TheResultingValue(sfxPlayer.Volume).ShouldBe(0.25f);
                    TheResultingValue(sfxPlayer.Pitch).ShouldBe(content.Ultraviolet.GetAudio().Capabilities.SupportsPitchShifting ? 0.50f : 0.00f);
                    TheResultingValue(sfxPlayer.Pan).ShouldBe(0.75f);
                })
                .OnUpdate((app, time) =>
                {
                    sfxPlayer.Update(time);
                })
                .RunForOneFrame();
        }

        [Test]
        [TestCase(AudioImplementation.BASS)]
        [TestCase(AudioImplementation.FMOD)]
        [Category("Audio")]
        public void SoundEffectPlayer_PlayResetsVolumePitchAndPanWhenNotSpecified(AudioImplementation audioImplementation)
        {
            var sfxPlayer = default(SoundEffectPlayer);
            var sfx = default(SoundEffect);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithAudioImplementation(audioImplementation)
                .WithInitialization(uv => uv.GetAudio().AudioMuted = true)
                .WithContent(content =>
                {
                    sfxPlayer = SoundEffectPlayer.Create();
                    sfx = content.Load<SoundEffect>("SoundEffects/grenade");

                    sfxPlayer.Play(sfx, 0.25f, 0.50f, 0.75f, false);
                    sfxPlayer.Stop();
                    sfxPlayer.Play(sfx, false);

                    TheResultingValue(sfxPlayer.Volume).ShouldBe(1f);
                    TheResultingValue(sfxPlayer.Pitch).ShouldBe(0f);
                    TheResultingValue(sfxPlayer.Pan).ShouldBe(0f);
                })
                .OnUpdate((app, time) =>
                {
                    sfxPlayer.Update(time);
                })
                .RunForOneFrame();
        }

        [Test]
        [TestCase(AudioImplementation.BASS)]
        [TestCase(AudioImplementation.FMOD)]
        [Category("Audio")]
        public void SoundEffectPlayer_SlidesVolumeCorrectly(AudioImplementation audioImplementation)
        {
            var sfxPlayer = default(SoundEffectPlayer);
            var sfx = default(SoundEffect);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithAudioImplementation(audioImplementation)
                .WithInitialization(uv => uv.GetAudio().AudioMuted = true)
                .WithContent(content =>
                {
                    sfxPlayer = SoundEffectPlayer.Create();
                    sfx = content.Load<SoundEffect>("SoundEffects/grenade");

                    sfxPlayer.Play(sfx, false);

                    TheResultingValue(sfxPlayer.Volume).ShouldBe(1f);

                    sfxPlayer.SlideVolume(0f, TimeSpan.FromSeconds(1));
                })
                .OnUpdate((app, time) =>
                {
                    sfxPlayer.Update(time);
                })
                .RunFor(TimeSpan.FromSeconds(2));

            TheResultingValue(sfxPlayer.Volume).ShouldBe(0f);
        }

        [Test]
        [TestCase(AudioImplementation.BASS)]
        [TestCase(AudioImplementation.FMOD)]
        [Category("Audio")]
        public void SoundEffectPlayer_SlidesPitchCorrectly(AudioImplementation audioImplementation)
        {
            var sfxPlayer = default(SoundEffectPlayer);
            var sfx = default(SoundEffect);

            var supported = true;

            GivenAnUltravioletApplicationWithNoWindow()
                .WithAudioImplementation(audioImplementation)
                .WithInitialization(uv => uv.GetAudio().AudioMuted = true)
                .WithContent(content =>
                {
                    sfxPlayer = SoundEffectPlayer.Create();
                    sfx = content.Load<SoundEffect>("SoundEffects/grenade");

                    sfxPlayer.Play(sfx, false);

                    TheResultingValue(sfxPlayer.Pitch).ShouldBe(0f);

                    sfxPlayer.SlidePitch(-1f, TimeSpan.FromSeconds(1));

                    supported = content.Ultraviolet.GetAudio().Capabilities.SupportsPitchShifting;
                })
                .OnUpdate((app, time) =>
                {
                    sfxPlayer.Update(time);
                })
                .RunFor(TimeSpan.FromSeconds(2));

            TheResultingValue(sfxPlayer.Pitch).ShouldBe(supported ? -1f : 0f);
        }

        [Test]
        [TestCase(AudioImplementation.BASS)]
        [TestCase(AudioImplementation.FMOD)]
        [Category("Audio")]
        public void SoundEffectPlayer_SlidesPanCorrectly(AudioImplementation audioImplementation)
        {
            var sfxPlayer = default(SoundEffectPlayer);
            var sfx = default(SoundEffect);

            GivenAnUltravioletApplicationWithNoWindow()
                .WithAudioImplementation(audioImplementation)
                .WithInitialization(uv => uv.GetAudio().AudioMuted = true)
                .WithContent(content =>
                {
                    sfxPlayer = SoundEffectPlayer.Create();
                    sfx = content.Load<SoundEffect>("SoundEffects/grenade");

                    sfxPlayer.Play(sfx, false);

                    TheResultingValue(sfxPlayer.Pan).ShouldBe(0f);

                    sfxPlayer.SlidePan(-1f, TimeSpan.FromSeconds(1));
                })
                .OnUpdate((app, time) =>
                {
                    sfxPlayer.Update(time);
                })
                .RunFor(TimeSpan.FromSeconds(2));

            TheResultingValue(sfxPlayer.Pan).ShouldBe(-1f);
        }

        [Test]
        [TestCase(AudioImplementation.BASS)]
        [TestCase(AudioImplementation.FMOD)]
        [Category("Audio")]
        public void SoundEffectPlayer_ThrowsExceptionIfVolumeSetWhileNotPlaying(AudioImplementation audioImplementation)
        {
            var sfxPlayer = default(SoundEffectPlayer);

            Assert.That(() =>
            {
                GivenAnUltravioletApplicationWithNoWindow()
                    .WithAudioImplementation(audioImplementation)
                    .WithInitialization(uv => uv.GetAudio().AudioMuted = true)
                    .WithContent(content =>
                    {
                        sfxPlayer = SoundEffectPlayer.Create();
                        sfxPlayer.Volume = 0f;
                    })
                    .OnUpdate((app, time) =>
                    {
                        sfxPlayer.Update(time);
                    })
                    .RunFor(TimeSpan.FromSeconds(1));
            },
            Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        [TestCase(AudioImplementation.BASS)]
        [TestCase(AudioImplementation.FMOD)]
        [Category("Audio")]
        public void SoundEffectPlayer_ThrowsExceptionIfPitchSetWhileNotPlaying(AudioImplementation audioImplementation)
        {
            var sfxPlayer = default(SoundEffectPlayer);

            Assert.That(() =>
            {
                GivenAnUltravioletApplicationWithNoWindow()
                    .WithAudioImplementation(audioImplementation)
                    .WithInitialization(uv => uv.GetAudio().AudioMuted = true)
                    .WithContent(content =>
                    {
                        sfxPlayer = SoundEffectPlayer.Create();
                        sfxPlayer.Pitch = -1f;
                    })
                    .OnUpdate((app, time) =>
                    {
                        sfxPlayer.Update(time);
                    })
                    .RunFor(TimeSpan.FromSeconds(1));
            },
            Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        [TestCase(AudioImplementation.BASS)]
        [TestCase(AudioImplementation.FMOD)]
        [Category("Audio")]
        public void SoundEffectPlayer_ThrowsExceptionIfPanSetWhileNotPlaying(AudioImplementation audioImplementation)
        {
            var sfxPlayer = default(SoundEffectPlayer);

            Assert.That(() =>
            {
                GivenAnUltravioletApplicationWithNoWindow()
                    .WithAudioImplementation(audioImplementation)
                    .WithInitialization(uv => uv.GetAudio().AudioMuted = true)
                    .WithContent(content =>
                    {
                        sfxPlayer = SoundEffectPlayer.Create();
                        sfxPlayer.Pan = 0f;
                    })
                    .OnUpdate((app, time) =>
                    {
                        sfxPlayer.Update(time);
                    })
                    .RunFor(TimeSpan.FromSeconds(1));
            },
            Throws.TypeOf<InvalidOperationException>());
        }
    }
}