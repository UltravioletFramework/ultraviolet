using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents the base class for all visually rendered objects
    /// in the Ultraviolet Presentation Foundation.
    /// </summary>
    public abstract class Visual : StyledDependencyObject
    {
        /// <summary>
        /// Determines whether the specified point falls within this visual object and, if so,
        /// returns the topmost visual object which contains the point, which may be a descendant of this object.
        /// </summary>
        /// <param name="pt">The point to evaluate.</param>
        /// <returns>The visual object which contains the point, or <c>null</c> if no descendant of this object contains the point.</returns>
        public Visual HitTest(Point2D pt)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// When overridden in a derived class, determines whether the specified point falls within this visual object and, if so,
        /// returns the topmost visual object which contains the point, which may be a descendant of this object.
        /// </summary>
        /// <param name="pt">The point to evaluate.</param>
        /// <returns>The visual object which contains the point, or <c>null</c> if no descendant of this object contains the point.</returns>
        protected virtual Visual HitTestCore(Point2D pt)
        {
            throw new NotImplementedException();
        }
    }
}
