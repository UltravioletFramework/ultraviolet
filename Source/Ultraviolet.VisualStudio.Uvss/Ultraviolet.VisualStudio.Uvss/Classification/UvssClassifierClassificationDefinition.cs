using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Ultraviolet.VisualStudio.Uvss.Classification
{
    /// <summary>
    /// Defines the classification types used by the UVSS classifier.
    /// </summary>
    internal static class UvssClassifierClassificationDefinition
    {
#pragma warning disable 169

        [Export(typeof(ContentTypeDefinition))]
        [Name("uvss")]
        [BaseDefinition("text")]
        private static ContentTypeDefinition uvssContentType;
        
        [Export(typeof(FileExtensionToContentTypeDefinition))]
        [FileExtension(".uvss")]
        [ContentType("uvss")]
        private static FileExtensionToContentTypeDefinition uvssFileExtensionToContentType;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("UvssComment")]
        private static ClassificationTypeDefinition uvssComment;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("UvssNumber")]
        private static ClassificationTypeDefinition uvssNumber;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("UvssKeyword")]
        private static ClassificationTypeDefinition uvssKeyword;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("UvssSelector")]
        private static ClassificationTypeDefinition uvssSelector;
        
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("UvssPropertyName")]
        private static ClassificationTypeDefinition uvssPropertyName;
        
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("UvssPropertyValue")]
        private static ClassificationTypeDefinition uvssPropertyValue;
        
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("UvssStoryboard")]
        private static ClassificationTypeDefinition uvssStoryboard;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("UvssTypeName")]
        private static ClassificationTypeDefinition uvssTypeName;

		[Export(typeof(ClassificationTypeDefinition))]
		[Name("UvssDirective")]
		private static ClassificationTypeDefinition uvssDirective;

#pragma warning restore 169
	}
}
