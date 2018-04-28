using System;
using System.IO;

namespace Ultraviolet.Presentation.Uvss.Syntax
{
    /// <summary>
    /// Represents a UVSS keyword token.
    /// </summary>
    [SyntaxNodeTypeID((Byte)SyntaxNodeType.Keyword)]
    public class UvssKeyword : SyntaxToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvssKeyword"/> class.
        /// </summary>
        /// <param name="kind">A <see cref="SyntaxKind"/> value that specifies the keyword's kind.</param>
        public UvssKeyword(SyntaxKind kind)
            : this(kind, null, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssKeyword"/> class.
        /// </summary>
        /// <param name="kind">A <see cref="SyntaxKind"/> value that specifies the keyword's kind.</param>
        /// <param name="leadingTrivia">The keyword's leading trivia.</param>
        /// <param name="trailingTrivia">The keyword's trailing trivia.</param>
        public UvssKeyword(SyntaxKind kind, SyntaxNode leadingTrivia, SyntaxNode trailingTrivia)
            : base(kind, GetKeywordText(kind), leadingTrivia, trailingTrivia)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvssKeyword"/> class from
        /// the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        public UvssKeyword(BinaryReader reader, Int32 version)
            : base(reader, version)
        {

        }

        /// <summary>
        /// Gets a value indicating whether the specified string is a UVSS keyword.
        /// </summary>
        /// <param name="text">The string to evaluate.</param>
        /// <returns>true if the specified string is a UVSS keyword; otherwise, false.</returns>
        public static Boolean IsKeyword(String text)
        {
            return GetKeywordKindFromText(text) != SyntaxKind.None;
        }

        /// <summary>
        /// Gets the <see cref="SyntaxKind"/> value that corresponds to the specified keyword text.
        /// </summary>
        /// <param name="text">The keyword text to evaluate.</param>
        /// <returns>The <see cref="SyntaxKind"/> value that represents the specified keyword, 
        /// or <see cref="SyntaxKind.None"/> if the text is not a keyword.</returns>
        public static SyntaxKind GetKeywordKindFromText(String text)
        {
            switch (text)
            {
                case "play-storyboard":
                    return SyntaxKind.PlayStoryboardKeyword;

                case "set-handled":
                    return SyntaxKind.SetHandledKeyword;

                case "transition":
                    return SyntaxKind.TransitionKeyword;

                case "!important":
                    return SyntaxKind.ImportantKeyword;

                case "animation":
                    return SyntaxKind.AnimationKeyword;

                case "play-sfx":
                    return SyntaxKind.PlaySfxKeyword;

                case "property":
                    return SyntaxKind.PropertyKeyword;

                case "keyframe":
                    return SyntaxKind.KeyframeKeyword;

                case "trigger":
                    return SyntaxKind.TriggerKeyword;

                case "handled":
                    return SyntaxKind.HandledKeyword;

                case "target":
                    return SyntaxKind.TargetKeyword;

                case "event":
                    return SyntaxKind.EventKeyword;

                case "set":
                    return SyntaxKind.SetKeyword;

                case "as":
                    return SyntaxKind.AsKeyword;

                default:
                    return SyntaxKind.None;
            }
        }

        /// <summary>
        /// Gets the text associated with the specified keyword kind.
        /// </summary>
        /// <param name="kind">The kind of keyword for which to retrieve text.</param>
        /// <returns>The text associated with the specified keyword kind.</returns>
        private static String GetKeywordText(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.PlayStoryboardKeyword:
                    return "play-storyboard";
                case SyntaxKind.SetHandledKeyword:
                    return "set-handled";
                case SyntaxKind.TransitionKeyword:
                    return "transition";
                case SyntaxKind.ImportantKeyword:
                    return "!important";
                case SyntaxKind.AnimationKeyword:
                    return "animation";
                case SyntaxKind.PlaySfxKeyword:
                    return "play-sfx";
                case SyntaxKind.PropertyKeyword:
                    return "property";
                case SyntaxKind.KeyframeKeyword:
                    return "keyframe";
                case SyntaxKind.TriggerKeyword:
                    return "trigger";
                case SyntaxKind.HandledKeyword:
                    return "handled";
                case SyntaxKind.TargetKeyword:
                    return "target";
                case SyntaxKind.EventKeyword:
                    return "event";
                case SyntaxKind.SetKeyword:
                    return "set";
                case SyntaxKind.AsKeyword:
                    return "as";
            }
            throw new InvalidOperationException();
        }
    }
}
