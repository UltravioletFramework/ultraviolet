using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using System.Windows.Media;

namespace Ultraviolet.VisualStudio.Uvss.Classification
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
            this.ForegroundColor = Colors.Green;
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
            this.ForegroundColor = Colors.Blue;
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
            this.ForegroundColor = Colors.Purple;
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
            this.ForegroundColor = Colors.Maroon;
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
            this.ForegroundColor = Colors.Red;
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
            this.ForegroundColor = Colors.Blue;
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
            this.ForegroundColor = Colors.Maroon;
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
            this.ForegroundColor = Colors.Red;
        }
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = "UvssDirective")]
	[Name("UvssDirective")]
	[UserVisible(true)]
	[Order(Before = Priority.Default)]
	internal sealed class UvssDirectiveFormat : ClassificationFormatDefinition
	{
		public UvssDirectiveFormat()
		{
			this.DisplayName = "UVSS Directive";
            this.ForegroundColor = Colors.Maroon;
		}
	}
}
