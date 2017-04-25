using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Animations
{
    /// <summary>
    /// Represents a particular instance of a <see cref="Storyboard"/> which is playing on a <see cref="UIElement"/>.
    /// </summary>
    public partial class StoryboardInstance
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardInstance"/> class.
        /// </summary>
        internal StoryboardInstance() { }

        /// <summary>
        /// Gets the storyboard which is associated with this storyboard instance.
        /// </summary>
        public Storyboard Storyboard
        {
            get { return storyboard; }
        }

        /// <summary>
        /// Gets the storyboard clock which is associated with this storyboard instance.
        /// </summary>
        public StoryboardClock StoryboardClock
        {
            get { return clock.Value; }
        }

        /// <summary>
        /// Gets the element which is targeted by this storyboard instance.
        /// </summary>
        public UIElement Target
        {
            get { return target; }
        }
        
        /// <summary>
        /// Enlists a dependency property value into this storyboard instance.
        /// </summary>
        /// <param name="dpValue">The dependency property value to enlist.</param>
        /// <param name="animation">The animation to apply to the dependency property.</param>
        internal void Enlist(IDependencyPropertyValue dpValue, AnimationBase animation)
        {
            Contract.Require(dpValue, nameof(dpValue));

            var enlistment = new Enlistment(dpValue, animation);
            enlistedDependencyProperties.Add(enlistment);
        }

        /// <summary>
        /// Associates the instance with the specified storyboard and element.
        /// </summary>
        /// <param name="storyboard">The <see cref="Storyboard"/> with which to associate the instance.</param>
        /// <param name="clock">The <see cref="StoryboardClock"/> with which to associate the instance.</param>
        /// <param name="target">The <see cref="UIElement"/> with which is targeted by the instance.</param>
        internal void AssociateWith(Storyboard storyboard, UpfPool<StoryboardClock>.PooledObject clock, UIElement target)
        {
            Contract.Require(storyboard, nameof(storyboard));
            Contract.Require(clock, nameof(clock));
            Contract.Require(target, nameof(target));

            if (this.storyboard != null)
                throw new InvalidOperationException();

            this.storyboard = storyboard;
            this.clock = clock;
            this.target = target;
        }

        /// <summary>
        /// Disassociates the instance from its current storyboard and element.
        /// </summary>
        internal void Disassociate()
        {
            if (this.storyboard == null)
                return;

            Stop();

            if (this.clock != null)
                StoryboardClockPool.Instance.Release(this.clock);

            this.storyboard = null;
            this.clock = null;
            this.target = null;

            this.enlistedDependencyProperties.Clear();
        }

        /// <summary>
        /// Starts the storyboard, if it is not running.
        /// </summary>
        internal void Start()
        {
            if (this.storyboard == null)
                throw new InvalidOperationException();

            if (this.clock.Value.State == ClockState.Stopped)
            {
                foreach (var enlistment in enlistedDependencyProperties)
                {
                    enlistment.DependencyPropertyValue.BeginStoryboard(enlistment.Animation, this);
                }

                this.clock.Value.Start();
            }
        }

        /// <summary>
        /// Stops the storyboard, if it is running.
        /// </summary>
        internal void Stop()
        {
            if (this.storyboard == null)
                throw new InvalidOperationException();

            if (this.clock.Value.State != ClockState.Stopped)
            {
                foreach (var enlistment in enlistedDependencyProperties)
                {
                    enlistment.DependencyPropertyValue.StopStoryboard(enlistment.Animation, this);
                }

                this.clock.Value.Stop();
            }
        }

        /// <summary>
        /// Gets the pooled object that represents the storyboard clock for this instance.
        /// </summary>
        internal UpfPool<StoryboardClock>.PooledObject ClockPooledObject
        {
            get { return clock; }
        }

        // Property values.
        private Storyboard storyboard;
        private UpfPool<StoryboardClock>.PooledObject clock;
        private UIElement target;

        // State values.
        private readonly List<Enlistment> enlistedDependencyProperties = new List<Enlistment>(16);
    }
}
