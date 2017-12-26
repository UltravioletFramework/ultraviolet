using System;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents an object which encapsulates the playback state of a <see cref="SpriteAnimation"/>.
    /// </summary>
    public sealed class SpriteAnimationController
    {
        /// <summary>
        /// Updates the controller.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        public void Update(UltravioletTime time)
        {
            if (IsPlaying)
            {
                // If the current animation has no frames, there's no need to update.
                if (animation.Frames.Count < 1)
                {
                    if (defaultAnimation != null)
                        PlayAnimation(defaultAnimation);
                    return;
                }

                // Calculate the duration of the current frame.  If we have a specified playback time
                // for this animation, then we normalize the frame duration and multiply by the playback time.
                var duration = playbackTime ?? 0.0;
                if (animation.Frames.Count > 1)
                {
                    duration = (playbackTime.HasValue) ?
                        (frame.Duration / (float)animation.Frames.Duration) * playbackTime.GetValueOrDefault() :
                        (frame.Duration);
                }

                // Update the animation timer and advance to the next frame if necessary.
                timer += time.ElapsedTime.TotalMilliseconds;
                if (timer >= duration)
                {
                    timer -= duration;

                    // Have we reached the end of our frame list?
                    if (frameIndex + 1 == animation.Frames.Count)
                    {
                        // If this is a fire-and-forget animation, play the default.
                        if (defaultAnimation != null)
                        {
                            PlayAnimation(defaultAnimation);
                            return;
                        }

                        // If this is a non-repeating animation, stop it.
                        if (animation.Repeat == SpriteAnimationRepeat.None)
                        {
                            StopAnimation();
                            return;
                        }

                        // Othewise, return to the first frame of the animation.
                        frameIndex = 0;
                        frame = animation.Frames[frameIndex];
                    }
                    else
                    {
                        // Advance to the next frame of the animation.
                        frameIndex++;
                        frame = animation.Frames[frameIndex];
                    }
                }
            }
        }

        /// <summary>
        /// Plays a fire-and-forget animation.  The animation will play once (regardless of its repeat mode),
        /// then the controller will return to the currently playing animation.
        /// </summary>
        /// <param name="animation">The <see cref="SpriteAnimation"/> to play.</param>
        /// <param name="playbackTime">The desired playback time in milliseconds, or <see langword="null"/> to use the standard playback time.</param>
        public void FireAndForget(SpriteAnimation animation, Double? playbackTime = null)
        {
            FireAndForget(animation, this.animation, playbackTime);
        }

        /// <summary>
        /// Plays a fire-and-forget animation.  The animation will play once (regardless of its repeat mode),
        /// then the controller will return to the specified default animation.
        /// </summary>
        /// <param name="animation">The animation to play.</param>
        /// <param name="defaultAnimation">The <see cref="SpriteAnimation"/> to play once the fire-and-forget animation has completed.</param>
        /// <param name="playbackTime">The desired playback time in milliseconds, or <see langword="null"/> to use the standard playback time.</param>
        public void FireAndForget(SpriteAnimation animation, SpriteAnimation defaultAnimation, Double? playbackTime = null)
        {
            this.animation = animation;
            this.defaultAnimation = defaultAnimation;
            this.playbackTime = playbackTime;
            ResetAnimation();
        }

        /// <summary>
        /// Plays the specified animation.
        /// </summary>
        /// <param name="animation">The <see cref="SpriteAnimation"/> to play.</param>
        public void PlayAnimation(SpriteAnimation animation)
        {
            this.animation = animation;
            this.defaultAnimation = null;
            this.playbackTime = null;
            ResetAnimation();
        }

        /// <summary>
        /// Stops the controller's current animation.
        /// </summary>
        public void StopAnimation()
        {
            this.animation = null;
            this.defaultAnimation = null;
            this.playbackTime = null;
            this.timer = 0.0;
            this.frame = null;
        }

        /// <summary>
        /// Resets the controller's current animation to its default state.
        /// </summary>
        public void ResetAnimation()
        {
            this.timer = 0.0;
            this.frame = (animation == null || animation.Frames.Count == 0) ? null : animation.Frames[0];
            this.frameIndex = 0;
        }

        /// <summary>
        /// Gets the playing animation's current frame.
        /// </summary>
        /// <returns>The playing animation's current frame, or <see langword="null"/> if no animation is playing.</returns>
        public SpriteFrame GetFrame()
        {
            return frame;
        }
        
        /// <summary>
        /// Gets the current animation.
        /// </summary>
        public SpriteAnimation GetAnimation()
        {
            return animation;
        }

        /// <summary>
        /// Gets a value indicating whether the controller is currently playing a fire-and-forget animation.
        /// </summary>
        public Boolean IsPlayingFireAndForget
        {
            get { return defaultAnimation != null; }
        }

        /// <summary>
        /// Gets a value indicating whether the controller is currently playing an animation.
        /// </summary>
        public Boolean IsPlaying
        {
            get { return animation != null && frame != null; }
        }

        /// <summary>
        /// Gets a value indicating whether the controller is currently playing a static (i.e. single-frame) animation.
        /// </summary>
        public Boolean IsStatic
        {
            get { return animation != null && animation.Frames.Count == 1; }
        }

        /// <summary>
        /// Gets the width of the controller's currently displayed frame.
        /// </summary>
        public Int32 Width
        {
            get
            {
                var frame = GetFrame();
                if (frame != null)
                    return frame.Width;
                return 0;
            }
        }

        /// <summary>
        /// Gets the height of the controller's currently displayed frame.
        /// </summary>
        public Int32 Height
        {
            get
            {
                var frame = GetFrame();
                if (frame != null)
                    return frame.Height;
                return 0;
            }
        }        

        // Animation properties.
        private Double timer;
        private Double? playbackTime;
        private SpriteAnimation animation;
        private SpriteAnimation defaultAnimation;
        private SpriteFrame frame;
        private Int32 frameIndex;
    }
}
