using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Collections;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents a pooled collection of <see cref="CursorTracker"/> instances used for
    /// tracking touch cursors.
    /// </summary>
    internal sealed class CursorTrackerTouchCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CursorTrackerTouchCollection"/> class.
        /// </summary>
        /// <param name="view"></param>
        public CursorTrackerTouchCollection(PresentationFoundationView view)
        {
            Contract.Require(view, nameof(view));

            pool = new ExpandingPool<CursorTracker.Touch>(5,
                () => CursorTracker.ForTouch(view),
                (tracker) => tracker.TouchID = 0);
            active = new Dictionary<Int64, CursorTracker.Touch>(5);
        }

        /// <summary>
        /// Clears the state of any active trackers.
        /// </summary>
        public void Clear()
        {
            foreach (var tracker in active)
                pool.Release(tracker);

            active.Clear();
        }

        /// <summary>
        /// Updates all of the trackers in the collection.
        /// </summary>
        /// <param name="forceNullPosition">A value indicating whether to force the tracker's position to <see langword="null"/>,
        /// regardless of the physical device state.</param>
        public void Update(Boolean forceNullPosition = false)
        {
            foreach (var kvp in active)
                kvp.Value.Update(forceNullPosition);
        }

        /// <summary>
        /// Begins tracking the specified touch.
        /// </summary>
        /// <param name="touchID">The unique identifier of the touch to start track.</param>
        /// <returns><see langword="true"/> if tracking started successfully; otherwise, <see langword="false"/>.</returns>
        public Boolean StartTracking(Int64 touchID)
        {
            if (active.ContainsKey(touchID))
                return false;

            var tracker = pool.Retrieve();
            tracker.TouchID = touchID;

            active.Add(touchID, tracker);
            tracker.Update();

            return true;
        }

        /// <summary>
        /// Finishes tracking the specified touch.
        /// </summary>
        /// <param name="touchID">The unique identifier of the touch to finish tracking.</param>
        /// <returns><see langword="true"/> if tracking finished successfully; otherwise, <see langword="false"/>.</returns>
        public Boolean FinishTracking(Int64 touchID)
        {
            CursorTracker.Touch tracker;
            if (!active.TryGetValue(touchID, out tracker))
                return false;

            active.Remove(touchID);

            pool.Release(tracker);
            return true;
        }

        /// <summary>
        /// Gets the tracker for the touch with the specified identifier.
        /// </summary>
        /// <param name="touchID">The unique identifier of the touch for which to retrieve a tracker.</param>
        /// <returns>The tracker for the touch with the specified identifier, or <see langword="null"/> if no such tracker exists.</returns>
        public CursorTracker.Touch GetTrackerByTouchID(Int64 touchID)
        {
            CursorTracker.Touch tracker;
            active.TryGetValue(touchID, out tracker);
            return tracker;
        }
        
        // The pool of tracker objects.
        private readonly Dictionary<Int64, CursorTracker.Touch> active;
        private readonly IPool<CursorTracker.Touch> pool;

    }
}
