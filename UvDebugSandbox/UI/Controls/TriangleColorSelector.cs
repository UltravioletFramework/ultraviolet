using System;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.UI.Presentation;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;

namespace UvDebugSandbox.UI.Controls
{
    /// <summary>
    /// Represents a control which allows the user to pick a color for one of a displayed triangle's vertices.
    /// </summary>
    [UvmlKnownType(null, "UvDebugSandbox.UI.Controls.Templates.TriangleColorSelector.xml")]
    public sealed class TriangleColorSelector : ContentControl
    {
        /// <summary>
        /// Initializes the <see cref="TriangleColorSelector"/> type.
        /// </summary>
        static TriangleColorSelector()
        {
            FocusableProperty.OverrideMetadata(typeof(TriangleColorSelector), new PropertyMetadata<Boolean>(false));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TriangleColorSelector"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public TriangleColorSelector(UltravioletContext uv, String name)
            : base(uv, name)
        {

        }

        /// <summary>
        /// Gets or sets the selected vertex color.
        /// </summary>
        public Color VertexColor
        {
            get { return GetValue<Color>(VertexColorProperty); }
            set { SetValue(VertexColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the index of the selected vertex color within the selector's combo box.
        /// </summary>
        public Int32 VertexColorIndex
        {
            get
            {
                var vertexColor = VertexColor;
                for (int i = 0; i < AvailableColors.Length; i++)
                {
                    if (vertexColor.Equals(AvailableColors[i]))
                        return i;
                }
                return -1;
            }
            set
            {
                if (value >= 0 && value < AvailableColors.Length)
                {
                    VertexColor = AvailableColors[value];
                }
                else
                {
                    VertexColor = Color.White;
                }
            }
        }

        /// <summary>
        /// Identifies the <see cref="VertexColor"/> property.
        /// </summary>
        public static readonly DependencyProperty VertexColorProperty = DependencyProperty.Register("VertexColor", typeof(Color), typeof(TriangleColorSelector),
            new PropertyMetadata<Color>(Color.Red, PropertyMetadataOptions.None));

        // State values.
        private static readonly Color[] AvailableColors = new[] { Color.Red, Color.Lime, Color.Blue };
    }
}
