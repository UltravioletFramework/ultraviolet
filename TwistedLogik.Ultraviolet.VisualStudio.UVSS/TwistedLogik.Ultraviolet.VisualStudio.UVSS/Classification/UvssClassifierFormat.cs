using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace TwistedLogik.Ultraviolet.VisualStudio.Uvss.Classification
{
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "UvssComment")]
    [Name("UvssComment")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class UvssCommentFormat : ClassificationFormatDefinition
    {
        public UvssCommentFormat()
        {
            this.DisplayName = "UVSS Comment";
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "UvssNumber")]
    [Name("UvssNumber")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class UvssNumberFormat : ClassificationFormatDefinition
    {
        public UvssNumberFormat()
        {
            this.DisplayName = "UVSS Numeric Literal";
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "UvssKeyword")]
    [Name("UvssKeyword")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class UvssKeywordFormat : ClassificationFormatDefinition
    {
        public UvssKeywordFormat()
        {
            this.DisplayName = "UVSS Keyword";
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "UvssSelector")]
    [Name("UvssSelector")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class UvssSelectorFormat : ClassificationFormatDefinition
    {
        public UvssSelectorFormat()
        {
            this.DisplayName = "UVSS Selector";
        }
    }
    
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "UvssPropertyName")]
    [Name("UvssPropertyName")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class UvssPropertyNameFormat : ClassificationFormatDefinition
    {
        public UvssPropertyNameFormat()
        {
            this.DisplayName = "UVSS Property Name";
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "UvssPropertyValue")]
    [Name("UvssPropertyValue")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class UvssPropertyValueFormat : ClassificationFormatDefinition
    {
        public UvssPropertyValueFormat()
        {
            this.DisplayName = "UVSS Property Value";
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "UvssStoryboard")]
    [Name("UvssStoryboard")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class UvssStoryboardFormat : ClassificationFormatDefinition
    {
        public UvssStoryboardFormat()
        {
            this.DisplayName = "UVSS Storyboard";
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "UvssTypeName")]
    [Name("UvssTypeName")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class UvssTypeNameFormat : ClassificationFormatDefinition
    {
        public UvssTypeNameFormat()
        {
            this.DisplayName = "UVSS Type Name";
        }
    }
}
