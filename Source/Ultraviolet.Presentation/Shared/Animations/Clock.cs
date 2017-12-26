using System;
using System.Collections.Generic;
using Ultraviolet.Core;
using Ultraviolet.Core.Collections;

namespace Ultraviolet.Presentation.Animations
{
    /// <summary>
    /// Represents the method that is called when a clock's state changes.
    /// </summary>
    /// <param name="clock">The clock that raised the event.</param>
    public delegate void ClockEventHandler(Clock clock);

    /// <summary>
    /// Represents a clock which tracks the playback state of an animation.
    /// </summary>
    public abstract class Clock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Clock"/> class.
        /// </summary>
        internal Clock() { }

        /// <summary>
        /// Updates the clock's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        public void Update(UltravioletTime time)
        {
            if (!IsValid || state != ClockState.Playing)
                return;

            total += time.ElapsedTime.TotalMilliseconds;

            var duration       = Duration.TotalMilliseconds;
            var elapsedUpdated = elapsed + (delta * time.ElapsedTime.TotalMilliseconds);
            var elapsedClamped = (elapsedUpdated < 0) ? 0 : elapsedUpdated > duration ? duration : elapsedUpdated;

            if (elapsedUpdated < 0 || elapsedUpdated >= duration)
            {
                switch (LoopBehavior)
                {
                    case LoopBehavior.None:
                        elapsed = elapsedClamped;
                        delta   = 1.0;
                        break;

                    case LoopBehavior.Loop:
                        elapsed = elapsedClamped % duration;
                        delta   = 1.0;
                        break;

                    case LoopBehavior.Reverse:
                        {
                            var remaining   = elapsedUpdated < 0 ? Math.Abs(elapsedUpdated) : elapsedUpdated - duration;
                            var distributed = 0.0;

                            while (remaining > 0)
                            {
                                distributed     = Math.Min(remaining, duration);
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
            if (this.state != ClockState.Stopped)
                Stop();

            this.state   = ClockState.Playing;
            this.elapsed = 0;

            OnStarted();
        }

        /// <summary>
        /// Stops the clock.
        /// </summary>
        public void Stop()
        {
            if (this.state != ClockState.Stopped)
            {
                this.state   = ClockState.Stopped;
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
            if (this.state == ClockState.Playing)
            {
                this.state = ClockState.Paused;

                OnPaused();
            }
        }

        /// <summary>
        /// Resumes the clock after being paused.
        /// </summary>
        public void Resume()
        {
            if (this.state == ClockState.Paused)
            {
                this.state = ClockState.Playing;

                OnResumed();
            }
        }

        /// <summary>
        /// Gets the clock's current playback state.
        /// </summary>
        public ClockState State
        {
            get { return state; }
        }

        /// <summary>
        /// Gets the clock's loop behavior.
        /// </summary>
        public abstract LoopBehavior LoopBehavior
        {
            get;
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
        /// Gets the clock's total duration.
        /// </summary>
        public abstract TimeSpan Duration
        {
            get;
        }

        /// <summary>
        /// Occurs when the clock is started.
        /// </summary>
        public event ClockEventHandler Started;

        /// <summary>
        /// Occurs when the clock is stopped.
        /// </summary>
        public event ClockEventHandler Stopped;

        /// <summary>
        /// Occurs when the clock is paused.
        /// </summary>
        public event ClockEventHandler Paused;

        /// <summary>
        /// Occurs when the clock is resumed.
        /// </summary>
        public event ClockEventHandler Resumed;

        /// <summary>
        /// Adds a dependency property value to the clock's subscriber list.
        /// </summary>
        /// <param name="value">The dependency property value to add to the subscriber list.</param>
        internal void Subscribe(IDependencyPropertyValue value)
        {
            Contract.Require(value, nameof(value));

            subscribers.AddLast(value);
        }

        /// <summary>
        /// Removes a dependency property value from the clock's subscriber list.
        /// </summary>
        /// <param name="value">The dependency property value to remove from the subscriber list.</param>
        internal void Unsubscribe(IDependencyPropertyValue value)
        {
            Contract.Require(value, nameof(value));

            subscribers.Remove(value);
        }

        /// <summary>
        /// Gets a value indicating whether the clock is in a valid playback state.
        /// </summary>
        protected abstract Boolean IsValid
        {
            get;
        }

        /// <summary>
        /// Raises the <see cref="Started"/> event.
        /// </summary>
        private void OnStarted()
        {
            foreach (var subscriber in subscribers)
                subscriber.ClockStarted();

            Started?.Invoke(this);
        }

        /// <summary>
        /// Raises the <see cref="Stopped"/> event.
        /// </summary>
        private void OnStopped()
        {
            var current = default(LinkedListNode<IDependencyPropertyValue>);
            var next    = default(LinkedListNode<IDependencyPropertyValue>);

            for (current = subscribers.First; current != null; current = next)
            {
                current.Value.ClockStopped();
                next = current.Next;
            }

            Stopped?.Invoke(this);
        }

        /// <summary>
        /// Raises the <see cref="Paused"/> event.
        /// </summary>
        private void OnPaused()
        {
            foreach (var subscriber in subscribers)
                subscriber.ClockPaused();

            Paused?.Invoke(this);
        }

        /// <summary>
        /// Raises the <see cref="Resumed"/> event.
        /// </summary>
        private void OnResumed()
        {
            foreach (var subscriber in subscribers)
                subscriber.ClockResumed();

            Resumed?.Invoke(this);
        }

        // Property values.
        private ClockState state = ClockState.Stopped;
        private Double elapsed;
        private Double total;
        private Double delta = 1;
        private readonly PooledLinkedList<IDependencyPropertyValue> subscribers = 
            new PooledLinkedList<IDependencyPropertyValue>(8);
    }
}
