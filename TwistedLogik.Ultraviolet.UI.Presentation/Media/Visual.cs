using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Media
{
    /// <summary>
    /// Represents the base class for all visually rendered objects
    /// in the Ultraviolet Presentation Foundation.
    /// </summary>
    public abstract class Visual : DependencyObject
    {
        /// <summary>
        /// Returns a transformation matrix which can be used to transform coordinates from this visual to
        /// the specified ancestor of this visual.
        /// </summary>
        /// <param name="ancestor">The ancestor to which coordinates will be transformed.</param>
        /// <returns>A <see cref="Matrix"/> which represents the specified transformation.</returns>
        public Matrix GetTransformToAncestorMatrix(Visual ancestor)
        {
            Contract.Require(ancestor, "ancestor");

            return ancestor.MatrixTransformToDescendantInternal(this, false);
        }

        /// <summary>
        /// Returns a transformation matrix which can be used to transform coordinates from this visual to
        /// the specified descendant of this visual.
        /// </summary>
        /// <param name="descendant">The descendnat to which coordinates will be transformed.</param>
        /// <returns>A <see cref="Matrix"/> which represents the specified transformation.</returns>
        public Matrix GetTransformToDescendantMatrix(Visual descendant)
        {
            Contract.Require(descendant, "descendant");

            return MatrixTransformToDescendantInternal(descendant, true);
        }

        /// <summary>
        /// Retrieves a transform which can be used to transform coordinates from this visual
        /// to the specified ancestor visual.
        /// </summary>
        /// <param name="ancestor">The ancestor to which coordinates will be transformed.</param>
        /// <returns>A <see cref="Transform"/> which represents the specified transformation.</returns>
        public Transform TransformToAncestor(Visual ancestor)
        {
            return new MatrixTransform(GetTransformToAncestorMatrix(ancestor));
        }

        /// <summary>
        /// Retrieves a transform which can be used to transform coordinates from this visual
        /// to the specified descendant visual.
        /// </summary>
        /// <param name="descendant">The descendant to which coordinates will be transformed.</param>
        /// <returns>A <see cref="Transform"/> which represents the specified transformation.</returns>
        public Transform TranformToDescendant(Visual descendant)
        {
            return new MatrixTransform(GetTransformToDescendantMatrix(descendant));
        }

        /// <summary>
        /// Transforms a vector from the coordinate space of this visual to the coordinate space
        /// of the specified ancestor visual.
        /// </summary>
        /// <param name="ancestor">The ancestor to which coordinates will be transformed.</param>
        /// <param name="vector">The vector to transform.</param>
        /// <returns>The transformed vector.</returns>
        public Vector2 TransformToAncestor(Visual ancestor, Vector2 vector)
        {
            var matrix = GetTransformToAncestorMatrix(ancestor);

            Vector2 result;
            Vector2.Transform(ref vector, ref matrix, out result);

            return result;
        }

        /// <summary>
        /// Transforms a point from the coordinate space of this visual to the coordinate space
        /// of the specified ancestor visual.
        /// </summary>
        /// <param name="ancestor">The ancestor to which coordinates will be transformed.</param>
        /// <param name="point">The point to transform.</param>
        /// <returns>The transformed point.</returns>
        public Point2D TransformToAncestor(Visual ancestor, Point2D point)
        {
            var matrix = GetTransformToAncestorMatrix(ancestor);

            Point2D result;
            Point2D.Transform(ref point, ref matrix, out result);

            return result;
        }

        /// <summary>
        /// Transforms a vector from the coordinate space of this visual to the coordinate space
        /// of the specified descendant visual.
        /// </summary>
        /// <param name="descendant">The descendant to which coordinates will be transformed.</param>
        /// <param name="vector">The vector to transform.</param>
        /// <returns>The transformed vector.</returns>
        public Vector2 TransformToDescendant(Visual descendant, Vector2 vector)
        {
            var matrix = GetTransformToDescendantMatrix(descendant);

            Vector2 result;
            Vector2.Transform(ref vector, ref matrix, out result);

            return result;
        }

        /// <summary>
        /// Transforms a point from the coordinate space of this visual to the coordinate space
        /// of the specified descendant visual.
        /// </summary>
        /// <param name="descendant">The descendant to which coordinates will be transformed.</param>
        /// <param name="point">The point to transform.</param>
        /// <returns>The transformed point.</returns>
        public Point2D TransformToDescendant(Visual descendant, Point2D point)
        {
            var matrix = GetTransformToDescendantMatrix(descendant);

            Point2D result;
            Point2D.Transform(ref point, ref matrix, out result);

            return result;
        }

        /// <summary>
        /// Performs a hit test against this and returns the topmost descendant
        /// which contains the specified point.
        /// </summary>
        /// <param name="point">The point to evaluate.</param>
        /// <returns>The topmost <see cref="Visual"/> which contains the specified point, or <c>null</c>.</returns>
        public Visual HitTest(Point2D point)
        {
            return HitTestCore(point);
        }

        /// <summary>
        /// Invokes the <see cref="OnVisualParentChanged(Visual, Visual)"/> method.
        /// </summary>
        /// <param name="oldParent">The visual's old visual parent.</param>
        /// <param name="newParent">The visual's new visual parent.</param>
        internal virtual void OnVisualParentChangedInternal(Visual oldParent, Visual newParent)
        {
            OnVisualParentChanged(oldParent, newParent);
        }

        /// <summary>
        /// Gets the object's visual parent.
        /// </summary>
        internal Visual VisualParent
        {
            get { return visualParent; }
        }

        /// <summary>
        /// Adds a visual as a child of this object.
        /// </summary>
        /// <param name="child">The child object to add to this object.</param>
        protected internal void AddVisualChild(Visual child)
        {
            Contract.Require(child, "child");

            if (child.visualParent != null)
                throw new InvalidOperationException(PresentationStrings.VisualAlreadyHasAParent);

            if (child.visualParent != this)
            {
                var oldParent = child.visualParent;
                var newParent = this;

                child.visualParent = newParent;
                child.OnVisualParentChangedInternal(oldParent, newParent);
            }
        }

        /// <summary>
        /// Removes a visual child from this object.
        /// </summary>
        /// <param name="child">The child object to remove from this object.</param>
        protected internal void RemoveVisualChild(Visual child)
        {
            Contract.Require(child, "child");

            if (child.visualParent == this)
            {
                child.visualParent = null;
                child.OnVisualParentChangedInternal(this, null);
            }
        }

        /// <summary>
        /// Gets the specified visual child of this element.
        /// </summary>
        /// <param name="childIndex">The index of the visual child to retrieve.</param>
        /// <returns>The visual child of this element with the specified index.</returns>
        protected internal virtual UIElement GetVisualChild(Int32 childIndex)
        {
            throw new ArgumentOutOfRangeException("childIndex");
        }

        /// <summary>
        /// Gets the number of visual children which belong to this element.
        /// </summary>
        protected internal virtual Int32 VisualChildrenCount
        {
            get { return 0; }
        }

        /// <summary>
        /// Occurs when the object's visual parent is changed.
        /// </summary>
        /// <param name="oldParent">The visual's old visual parent.</param>
        /// <param name="newParent">The visual's new visual parent.</param>
        protected virtual void OnVisualParentChanged(Visual oldParent, Visual newParent)
        {

        }

        /// <summary>
        /// When overridden in a derived class, performs a hit test against this and returns the topmost descendant
        /// which contains the specified point.
        /// </summary>
        /// <param name="point">The point to evaluate.</param>
        /// <returns>The topmost <see cref="Visual"/> which contains the specified point, or <c>null</c>.</returns>
        protected virtual Visual HitTestCore(Point2D point)
        {
            return null;
        }

        /// <summary>
        /// Gets the visual's transformation matrix.
        /// </summary>
        /// <returns>The visual's transformation matrix.</returns>
        protected virtual Matrix GetTransformMatrix()
        {
            return Matrix.Identity;
        }

        /// <summary>
        /// Returns a transformation matrix which can be used to transform coordinates from this visual to
        /// the specified descendant of this visual.
        /// </summary>
        /// <param name="descendant">The descendnat to which coordinates will be transformed.</param>
        /// <param name="invert">A value indicating whether to invert the resulting matrix.</param>
        /// <returns>A <see cref="Matrix"/> which represents the specified transformation.</returns>
        private Matrix MatrixTransformToDescendantInternal(Visual descendant, Boolean invert)
        {
            var mtxFinal = Matrix.Identity;

            DependencyObject current;
            for (current = descendant; current != null && current != this; current = VisualTreeHelper.GetParent(current))
            {
                var uiElement = current as UIElement;
                if (uiElement != null)
                {
                    var mtxTransform = uiElement.GetTransformMatrix();
                    var mtxTranslateToClientSpace = Matrix.CreateTranslation(
                        (Single)uiElement.RelativeBounds.X, 
                        (Single)uiElement.RelativeBounds.Y, 0f);

                    Matrix mtxResult;
                    Matrix.Concat(ref mtxFinal, ref mtxTransform, out mtxResult);
                    Matrix.Concat(ref mtxResult, ref mtxTranslateToClientSpace, out mtxFinal);
                }
            }

            if (current != this)
            {
                var paramName = invert ? "ancestor" : "descendant";
                var message = invert ? PresentationStrings.ElementIsNotAnAncestor : PresentationStrings.ElementIsNotADescendant;
                throw new ArgumentException(message, paramName);
            }

            if (invert)
            {
                if (Matrix.TryInvert(mtxFinal, out mtxFinal))
                {
                    return mtxFinal;
                }
                return Matrix.Identity;
            }
            return mtxFinal;
        }

        // State values.
        private Visual visualParent;
    }
}
