using System;
using NUnit.Framework;
using Ultraviolet.Audio;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests.Audio
{
    [TestFixture]
    public class SoundEffectPlayerTests : UltravioletApplicationTestFramework
    {
        [Test]
        [Category("Audio")]
        public void BASS_SoundEffectPlayer_PlaySetsVolumePitchAndPan()
        {
            var sfxPlayer = default(SoundEffectPlayer);
            var sfx       = default(SoundEffect);

            GivenAnUltravioletApplicationWithNoWindow()
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

        [Test]
        [Category("Audio")]
        public void BASS_SoundEffectPlayer_PlayResetsVolumePitchAndPanWhenNotSpecified()
        {
            var sfxPlayer = default(SoundEffectPlayer);
            var sfx       = default(SoundEffect);

            GivenAnUltravioletApplicationWithNoWindow()
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

        [Test]
        [Category("Audio")]
        public void BASS_SoundEffectPlayer_SlidesVolumeCorrectly()
        {
            var sfxPlayer = default(SoundEffectPlayer);
            var sfx       = default(SoundEffect);

            GivenAnUltravioletApplicationWithNoWindow()
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

        [Test]
        [Category("Audio")]
        public void BASS_SoundEffectPlayer_SlidesPitchCorrectly()
        {
            var sfxPlayer = default(SoundEffectPlayer);
            var sfx       = default(SoundEffect);

            GivenAnUltravioletApplicationWithNoWindow()
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

        [Test]
        [Category("Audio")]
        public void BASS_SoundEffectPlayer_SlidesPanCorrectly()
        {
            var sfxPlayer = default(SoundEffectPlayer);
            var sfx       = default(SoundEffect);

            GivenAnUltravioletApplicationWithNoWindow()
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

        [Test]
        [Category("Audio")]
        public void BASS_SoundEffectPlayer_ThrowsExceptionIfVolumeSetWhileNotPlaying()
        {
            var sfxPlayer = default(SoundEffectPlayer);

            Assert.That(() =>
            {
                GivenAnUltravioletApplicationWithNoWindow()
                    .WithInitialization(uv => uv.GetAudio().AudioMuted = true)
                    .WithContent(content =>
                    {
                        sfxPlayer = SoundEffectPlayer.Create();
                        sfxPlayer.Volume = 0f;
                    })
                    .RunFor(TimeSpan.FromSeconds(1));
            },
            Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        [Category("Audio")]
        public void BASS_SoundEffectPlayer_ThrowsExceptionIfPitchSetWhileNotPlaying()
        {
            var sfxPlayer = default(SoundEffectPlayer);

            Assert.That(() =>
            {
                GivenAnUltravioletApplicationWithNoWindow()
                    .WithInitialization(uv => uv.GetAudio().AudioMuted = true)
                    .WithContent(content =>
                    {
                        sfxPlayer = SoundEffectPlayer.Create();
                        sfxPlayer.Pitch = -1f;
                    })
                    .RunFor(TimeSpan.FromSeconds(1));
            },
            Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        [Category("Audio")]
        public void BASS_SoundEffectPlayer_ThrowsExceptionIfPanSetWhileNotPlaying()
        {
            var sfxPlayer = default(SoundEffectPlayer);

            Assert.That(() =>
            {
                GivenAnUltravioletApplicationWithNoWindow()
                    .WithInitialization(uv => uv.GetAudio().AudioMuted = true)
                    .WithContent(content =>
                    {
                        sfxPlayer = SoundEffectPlayer.Create();
                        sfxPlayer.Pan = 0f;
                    })
                    .RunFor(TimeSpan.FromSeconds(1));
            },
            Throws.TypeOf<InvalidOperationException>());
        }
    }
}
