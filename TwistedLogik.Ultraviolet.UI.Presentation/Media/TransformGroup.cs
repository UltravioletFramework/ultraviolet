using System.ComponentModel;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Media
{
    /// <summary>
    /// Represents a group of transformations.
    /// </summary>
    [UvmlKnownType]
    [DefaultProperty("Children")]
    public sealed class TransformGroup : Transform
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransformGroup"/> class.
        /// </summary>
        public TransformGroup()
        {
            Children = new TransformCollection();
        }

        /// <inheritdoc/>
        public override Matrix GetValue()
        {
            var matrix   = Matrix.Identity;
            var children = Children;

            if (children != null && children.Count > 0)
            {
                foreach (var child in children)
                {
                    matrix *= child.GetValue();
                }
            }

            return matrix;
        }

        /// <inheritdoc/>
        public override Matrix GetValueForDisplay(IUltravioletDisplay display)
        {
            var matrix   = Matrix.Identity;
            var children = Children;

            if (children != null && children.Count > 0)
            {
                foreach (var child in children)
                {
                    matrix *= child.GetValueForDisplay(display);
                }
            }

            return matrix;
        }

        /// <summary>
        /// Gets or sets the transform group's list of child transformations.
        /// </summary>
        public TransformCollection Children
        {
            get { return GetValue<TransformCollection>(ChildrenProperty); }
            set { SetValue<TransformCollection>(ChildrenProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Children"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ChildrenProperty = DependencyProperty.Register("Children", typeof(TransformCollection), typeof(TransformGroup),
            new PropertyMetadata<TransformCollection>(null, PropertyMetadataOptions.None));
    }
}
