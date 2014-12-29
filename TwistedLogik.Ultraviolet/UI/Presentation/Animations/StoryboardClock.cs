using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Collections;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Animations
{
    /// <summary>
    /// Represents the method that is called when a storyboard clock's state changes.
    /// </summary>
    /// <param name="clock">The clock that raised the event.</param>
    public delegate void StoryboardClockEventHandler(StoryboardClock clock);

    /// <summary>
    /// Represents a clock which tracks the playback state of a <see cref="Storyboard"/>.
    /// </summary>
    public sealed class StoryboardClock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardClock"/> class.
        /// </summary>
        internal StoryboardClock()
        {

        }

        /// <summary>
        /// Updates the clock's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        public void Update(UltravioletTime time)
        {
            if (storyboard == null || state != StoryboardClockState.Playing)
                return;

            total += time.ElapsedTime.TotalMilliseconds;

            var storyboardDuration     = storyboard.Duration.TotalMilliseconds;
            var elapsedUpdated   = elapsed + (delta * time.ElapsedTime.TotalMilliseconds);
            var elapsedClamped   = (elapsedUpdated < 0) ? 0 : elapsedUpdated > storyboardDuration ? storyboardDuration : elapsedUpdated;
            if (elapsedUpdated < 0 || elapsedUpdated >= storyboardDuration)
            {
                switch (storyboard.LoopBehavior)
                {
                    case LoopBehavior.None:
                        elapsed = elapsedClamped;
                        delta   = 1.0;
                        break;

                    case LoopBehavior.Loop:
                        elapsed = elapsedClamped % storyboardDuration;
                        delta   = 1.0;
                        break;

                    case LoopBehavior.Reverse:
                        {
                            var remaining   = elapsedUpdated < 0 ? Math.Abs(elapsedUpdated) : elapsedUpdated - storyboardDuration;
                            var distributed = 0.0;

                            while (remaining > 0)
                            {
                                distributed     = Math.Min(remaining, storyboardDuration);
                                delta           = -delta;
                                elapsedClamped += (delta * distributed);
                                remaining      -= distributed;
                            }
                        }
                        elapsed  = elapsedClamped;
                        break;
                }
            }
            else
            {
                elapsed = elapsedUpdated;
            }
        }

        /// <summary>
        /// Starts the clock.
        /// </summary>
        public void Start()
        {
            this.state   = StoryboardClockState.Playing;
            this.elapsed = 0;

            OnStarted();
        }

        /// <summary>
        /// Stops the clock.
        /// </summary>
        public void Stop()
        {
            if (this.state != StoryboardClockState.Stopped)
            {
                this.state   = StoryboardClockState.Stopped;
                this.elapsed = 0;
                this.total   = 0;

                OnStopped();
            }
        }

        /// <summary>
        /// Pauses the clock.
        /// </summary>
        public void Pause()
        {
            if (this.state == StoryboardClockState.Playing)
            {
                this.state = StoryboardClockState.Paused;

                OnPaused();
            }
        }

        /// <summary>
        /// Resumes the clock after being paused.
        /// </summary>
        public void Resume()
        {
            if (this.state == StoryboardClockState.Paused)
            {
                this.state = StoryboardClockState.Playing;

                OnResumed();
            }
        }

        /// <summary>
        /// Gets the clock's current playback state.
        /// </summary>
        public StoryboardClockState State
        {
            get { return state; }
        }

        /// <summary>
        /// Gets the clock's elapsed time.
        /// </summary>
        public TimeSpan ElapsedTime
        {
            get { return TimeSpan.FromMilliseconds(elapsed); }
        }

        /// <summary>
        /// Gets the clock's total running time.
        /// </summary>
        public TimeSpan TotalTime
        {
            get { return TimeSpan.FromMilliseconds(total); }
        }

        /// <summary>
        /// Gets the clock's associated storyboard.
        /// </summary>
        public Storyboard Storyboard
        {
            get { return storyboard; }
        }

        /// <summary>
        /// Occurs when the clock is started.
        /// </summary>
        public event StoryboardClockEventHandler Started;

        /// <summary>
        /// Occurs when the clock is stopped.
        /// </summary>
        public event StoryboardClockEventHandler Stopped;

        /// <summary>
        /// Occurs when the clock is paused.
        /// </summary>
        public event StoryboardClockEventHandler Paused;

        /// <summary>
        /// Occurs when the clock is resumed.
        /// </summary>
        public event StoryboardClockEventHandler Resumed;

        /// <summary>
        /// Changes the clock's associated storyboard.
        /// </summary>
        /// <param name="storyboard">The storyboard to associate with the clock.</param>
        internal void ChangeStoryboard(Storyboard storyboard)
        {
            this.storyboard = storyboard;
        }

        /// <summary>
        /// Adds a dependency property value to the clock's subscriber list.
        /// </summary>
        /// <param name="value">The dependency property value to add to the subscriber list.</param>
        internal void Subscribe(IDependencyPropertyValue value)
        {
            Contract.Require(value, "value");

            subscribers.AddLast(value);
        }

        /// <summary>
        /// Removes a dependency property value from the clock's subscriber list.
        /// </summary>
        /// <param name="value">The dependency property value to remove from the subscriber list.</param>
        internal void Unsubscribe(IDependencyPropertyValue value)
        {
            Contract.Require(value, "value");

            subscribers.Remove(value);
        }

        /// <summary>
        /// Raises the <see cref="Started"/> event.
        /// </summary>
        private void OnStarted()
        {
            foreach (var subscriber in subscribers)
                subscriber.StoryboardClockStarted();

            var temp = Started;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="Stopped"/> event.
        /// </summary>
        private void OnStopped()
        {
            foreach (var subscriber in subscribers)
                subscriber.StoryboardClockStopped();

            var temp = Stopped;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="Paused"/> event.
        /// </summary>
        private void OnPaused()
        {
            foreach (var subscriber in subscribers)
                subscriber.StoryboardClockPaused();

            var temp = Paused;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="Resumed"/> event.
        /// </summary>
        private void OnResumed()
        {
            foreach (var subscriber in subscribers)
                subscriber.StoryboardClockResumed();

            var temp = Resumed;
            if (temp != null)
            {
                temp(this);
            }
        }

        // Property values.
        private StoryboardClockState state = StoryboardClockState.Stopped;
        private Double elapsed;
        private Double total;
        private Double delta = 1;
        private Storyboard storyboard;
        private readonly PooledLinkedList<IDependencyPropertyValue> subscribers = 
            new PooledLinkedList<IDependencyPropertyValue>();
    }
}
