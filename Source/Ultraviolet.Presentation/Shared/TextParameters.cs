using System;
using Ultraviolet.Core;
using Ultraviolet.Graphics.Graphics2D.Text;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents the set of parameters which specify how a UI element should render text.
    /// </summary>
    [UvmlKnownType]
    public class TextParameters : DependencyObject
    {
        public Boolean Shaped
        {
            get { return GetValue<Boolean>(ShapedProperty); }
            set { SetValue(ShapedProperty, value); }
        }

        public TextScript Script
        {
            get { return GetValue<TextScript>(ScriptProperty); }
            set { SetValue(ScriptProperty, value); }
        }

        public String Language
        {
            get { return GetValue<String>(LanguageProperty); }
            set { SetValue(LanguageProperty, value); }
        }

        public static readonly DependencyProperty ShapedProperty = DependencyProperty.Register("Shaped", typeof(Boolean), typeof(TextParameters),
            new PropertyMetadata<Boolean>(CommonBoxedValues.Boolean.False, PropertyMetadataOptions.AffectsMeasure | PropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty ScriptProperty = DependencyProperty.Register("Script", typeof(TextScript), typeof(TextParameters),
            new PropertyMetadata<TextScript>(TextScript.Latin, PropertyMetadataOptions.AffectsMeasure | PropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty LanguageProperty = DependencyProperty.Register("Language", typeof(String), typeof(TextParameters),
            new PropertyMetadata<String>("en", PropertyMetadataOptions.AffectsMeasure | PropertyMetadataOptions.AffectsArrange));
    }
}
