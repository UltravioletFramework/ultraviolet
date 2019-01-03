using System;
using Ultraviolet.Core;
using Ultraviolet.Graphics.Graphics2D.Text;

namespace Ultraviolet.Presentation.Media
{
    /// <summary>
    /// Defines attached properties which modify how elements draw text.
    /// </summary>
    [UvmlKnownType]
    public static class TextOptions
    {
        /// <summary>
        /// Gets the rendering mode used to render the element's text.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>A <see cref="TextRenderingMode"/> value which represents the rendering mode used to render the element's text.</returns>
        public static TextRenderingMode GetTextRenderingMode(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<TextRenderingMode>(TextRenderingModeProperty);
        }

        /// <summary>
        /// Sets the rendering mode used to render the element's text.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="value">A <see cref="TextRenderingMode"/> value which represents the rendering mode used to render the element's text.</param>
        public static void SetTextRenderingMode(DependencyObject element, TextRenderingMode value)
        {
            Contract.Require(element, nameof(element));

            element.SetValue(TextRenderingModeProperty, value);
        }

        /// <summary>
        /// Gets the script used to render the element's text.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>A <see cref="TextScript"/> value which represents the script used to render the element's text.</returns>
        public static TextScript GetTextScript(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<TextScript>(TextScriptProperty);
        }

        /// <summary>
        /// Sets the script used to render the element's text.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="value">A <see cref="TextScript"/> value which represents the script used to render the element's text.</param>
        public static void SetTextScript(DependencyObject element, TextScript value)
        {
            Contract.Require(element, nameof(element));

            element.SetValue(TextScriptProperty, value);
        }

        /// <summary>
        /// Gets the ISO 639 language code which represents the language of the element's text.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The ISO 639 language code which represents the language of the element's text.</returns>
        public static String GetTextLanguage(DependencyObject element)
        {
            Contract.Require(element, nameof(element));

            return element.GetValue<String>(TextLanguageProperty);
        }

        /// <summary>
        /// Sets the ISO 639 language code which represents the language of the element's text.
        /// </summary>
        /// <param name="element">The element to modify.</param>
        /// <param name="value">The ISO 639 language code which represents the language of the element's text.</param>
        public static void SetTextLanguage(DependencyObject element, String value)
        {
            Contract.Require(element, nameof(element));

            element.SetValue(TextLanguageProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.FrameworkElement.TextRenderingMode"/>
        /// attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.FrameworkElement.TextRenderingMode"/>
        /// attached property.</value>
        /// <AttachedPropertyComments>
        /// <summary>
        /// Gets or sets a value indicating how the element renders text.
        /// </summary>
        /// <value>A <see cref="TextRenderingMode"/> value indicating how the element renders text.
        /// The default value is <see cref="TextRenderingMode.Auto"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="TextRenderingModeProperty"/></dpropField>
        ///     <dpropStylingName>text-rendering-mode</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsMeasure"/>, <see cref="PropertyMetadataOptions.AffectsArrange"/>, <see cref="PropertyMetadataOptions.Inherits"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        /// </AttachedPropertyComments>
        public static readonly DependencyProperty TextRenderingModeProperty = DependencyProperty.RegisterAttached("TextRenderingMode", typeof(TextRenderingMode), typeof(TextOptions),
            new PropertyMetadata<TextRenderingMode>(TextRenderingMode.Auto, PropertyMetadataOptions.AffectsMeasure | PropertyMetadataOptions.AffectsArrange | PropertyMetadataOptions.Inherits));

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.FrameworkElement.TextScript"/>
        /// attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.FrameworkElement.TextScript"/>
        /// attached property.</value>
        /// <AttachedPropertyComments>
        /// <summary>
        /// Gets or sets a value indicating which script is used to render the element's text.
        /// </summary>
        /// <value>A <see cref="TextRenderingMode"/> value indicating which script is used to render the element's text.
        /// The default value is <see cref="TextScript.Latin"/>.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="TextScriptProperty"/></dpropField>
        ///     <dpropStylingName>text-script</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsMeasure"/>, <see cref="PropertyMetadataOptions.AffectsArrange"/>, <see cref="PropertyMetadataOptions.Inherits"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        /// </AttachedPropertyComments>
        public static readonly DependencyProperty TextScriptProperty = DependencyProperty.RegisterAttached("TextScript", typeof(TextScript), typeof(TextOptions),
            new PropertyMetadata<String>(TextScript.Latin, PropertyMetadataOptions.AffectsMeasure | PropertyMetadataOptions.AffectsArrange | PropertyMetadataOptions.Inherits));

        /// <summary>
        /// Identifies the <see cref="P:Ultraviolet.Presentation.FrameworkElement.TextLanguage"/>
        /// attached property.
        /// </summary>
        /// <value>The identifier for the <see cref="P:Ultraviolet.Presentation.FrameworkElement.TextLanguage"/>
        /// attached property.</value>
        /// <AttachedPropertyComments>
        /// <summary>
        /// Gets or sets the ISO 639 language code which specifies the language of the element's text.
        /// </summary>
        /// <value>The ISO 639 language code which specifies the language of the element's text.
        /// The default value is "en" for English.</value>
        /// <remarks>
        /// <dprop>
        ///     <dpropField><see cref="TextLanguageProperty"/></dpropField>
        ///     <dpropStylingName>text-language</dpropStylingName>
        ///     <dpropMetadata><see cref="PropertyMetadataOptions.AffectsMeasure"/>, <see cref="PropertyMetadataOptions.AffectsArrange"/>, <see cref="PropertyMetadataOptions.Inherits"/></dpropMetadata>
        /// </dprop>
        /// </remarks>
        /// </AttachedPropertyComments>
        public static readonly DependencyProperty TextLanguageProperty = DependencyProperty.RegisterAttached("TextLanguage", typeof(String), typeof(TextOptions),
            new PropertyMetadata<String>("en", PropertyMetadataOptions.AffectsMeasure | PropertyMetadataOptions.AffectsArrange | PropertyMetadataOptions.Inherits));
    }
}
