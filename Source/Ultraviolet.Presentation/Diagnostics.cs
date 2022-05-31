using System;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Defines dependency properties related to diagnostic rendering.
    /// </summary>
    [UvmlKnownType]
    public static class Diagnostics
    {
        /// <summary>
        /// Gets a value indicating whether the diagnostics renderer should draw the visual bounds of the specified element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns><see langword="true"/> if the diagnostics renderer should draw the visual bounds of the specified element; otherwise, <see langword="false"/>.</returns>
        public static Boolean GetDrawVisualBounds(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<Boolean>(DrawVisualBoundsProperty);
        }

        /// <summary>
        /// Sets a value indicating whether the diagnostics renderer should draw the visual bounds of the specified element.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">A value specifying whether the diagnostics renderer should draw the visual bounds of the specified element.</param>
        public static void SetDrawVisualBounds(DependencyObject element, Boolean value)
        {
            Contract.Require(element, nameof(element));

            element.SetValue(DrawVisualBoundsProperty, value);
        }

        /// <summary>
        /// Gets a value specifying the color with which the diagnostics renderer will draw the visual bounds of the specified element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The color with which the diagnostics renderer will render the visual bounds of the specified element.</returns>
        public static Color GetDrawVisualBoundsColor(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<Color>(DrawVisualBoundsColorProperty);
        }

        /// <summary>
        /// Sets a value specifying the color with which the diagnostics renderer should draw the visual bounds of the specified element.
        /// </summary>
        /// <param name="element">The element to update.</param>
        /// <param name="value">The color with which the diagnostics renderer should draw the visual bounds of the specified element.</param>
        public static void SetDrawVisualBoundsColor(DependencyObject element, Color value)
        {
            Contract.Require(element, nameof(element));

            element.SetValue(DrawVisualBoundsColorProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether UPF views should render diagnostics visuals.
        /// </summary>
        public static Boolean DrawDiagnosticsVisuals
        {
            get;
            set;
        }

        /// <summary>
        /// Identifies the DrawVisualBounds attached property.
        /// </summary>
        public static readonly DependencyProperty DrawVisualBoundsProperty = DependencyProperty.RegisterAttached("DrawVisualBounds", typeof(Boolean), typeof(Diagnostics),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the DrawVisualBoundsColor attached property.
        /// </summary>
        public static readonly DependencyProperty DrawVisualBoundsColorProperty = DependencyProperty.RegisterAttached("DrawVisualBoundsColor", typeof(Color), typeof(Diagnostics),
            new PropertyMetadata<Color>(Color.Lime, PropertyMetadataOptions.None));

        /// <summary>
        /// Gets the image used by the diagnostics renderer to draw bounding boxes.
        /// </summary>
        internal static TextureImage BoundingBoxImage
        {
            get
            {
                if (boundingBoxImageTexture == null || boundingBoxImage.Texture != boundingBoxImageTexture.Value)
                {
                    if (boundingBoxImageTexture == null)
                    {
                        boundingBoxImageTexture = new UltravioletSingleton<Texture2D>(UltravioletSingletonFlags.DisabledInServiceMode, uv =>
                        {
                            var texture = Texture2D.CreateTexture(3, 3);
                            texture.SetData(new[] { Color.White, Color.White, Color.White, Color.White, Color.Transparent, Color.White, Color.White, Color.White, Color.White });
                            return texture;
                        });
                    }
                    boundingBoxImage = StretchableImage9.Create(boundingBoxImageTexture, new Rectangle(0, 0, 3, 3), 1, 1, 1, 1);
                }
                return boundingBoxImage;
            }
        }

        // Diagnostics resources.
        private static UltravioletSingleton<Texture2D> boundingBoxImageTexture;
        private static TextureImage boundingBoxImage;
    }
}
