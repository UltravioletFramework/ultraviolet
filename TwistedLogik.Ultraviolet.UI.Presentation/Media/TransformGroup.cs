using System.ComponentModel;

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
        public override Matrix Value
        {
            get
            {
                var matrix   = Matrix.Identity;
                var children = Children;

                if (children != null && children.Count > 0)
                {
                    foreach (var child in children)
                    {
                        matrix *= child.Value;
                    }
                }

                return matrix;
            }
        }

        /// <inheritdoc/>
        public override Matrix? Inverse
        {
            get
            {
                Matrix value = Value;
                Matrix inverse;
                if (Matrix.TryInvert(value, out inverse))
                {
                    return inverse;
                }
                return null;
            }
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
