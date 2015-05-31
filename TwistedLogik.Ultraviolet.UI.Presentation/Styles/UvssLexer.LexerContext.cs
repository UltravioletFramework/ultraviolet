using System;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
    partial class UvssLexer
    {
        /// <summary>
        /// Represents the base class for UVSS lexer contexts.
        /// </summary>
        private abstract class LexerContext
        {
            /// <summary>
            /// Initializes the lexer context after it has been switched to.
            /// </summary>
            public virtual void Initialize() { }

            /// <summary>
            /// Lexes the input stream.
            /// </summary>
            public abstract Boolean Lex(String input, IList<UvssLexerToken> output, ref Int32 line, ref Int32 ix);
        }

        /// <summary>
        /// Represents the default lexer context.
        /// </summary>
        private sealed class LexerContext_Default : LexerContext
        {
            /// <inhertidoc/>
            public override void Initialize()
            {
                storyboard = false;

                base.Initialize();
            }

            /// <inheritdoc/>
            public override Boolean Lex(String input, IList<UvssLexerToken> output, ref Int32 line, ref Int32 ix)
            {
                var isStoryboardIdentifier = false;

                if (ConsumeWhiteSpaceAndComments(input, output, ref line, ref ix))
                    return true;

                if (ConsumeChildSelector(input, output, line, ref ix))
                    return true;
                if (ConsumeUniversalSelector(input, output, line, ref ix))
                    return true;
                if (ConsumeIdentifier(input, output, line, ref ix, ref isStoryboardIdentifier))
                {
                    if (!storyboard && isStoryboardIdentifier)
                    {
                        storyboard = isStoryboardIdentifier;
                        braces     = 0;
                    }
                    return true;
                }

                if (ConsumeNumber(input, output, line, ref ix))
                    return true;
                if (ConsumeString(input, output, line, ref ix))
                    return true;
                if (ConsumeOpenParenthesis(input, output, line, ref ix))
                    return true;
                if (ConsumeCloseParenthesis(input, output, line, ref ix))
                    return true;
                if (ConsumeOpenCurlyBrace(input, output, line, ref ix))
                {
                    if (!storyboard)
                    {
                        ChangeContext(cachedLexerContext_StyleList);
                    }
                    else
                    {
                        braces++;
                    }
                    return true;
                }
                if (ConsumeCloseCurlyBrace(input, output, line, ref ix))
                {
                    if (storyboard)
                    {
                        if (--braces <= 0)
                        {
                            storyboard = false;
                        }
                    }
                    return true;
                }
                if (ConsumeColon(input, output, line, ref ix))
                    return true;
                if (ConsumeSemicolon(input, output, line, ref ix))
                    return true;
                if (ConsumeComma(input, output, line, ref ix))
                    return true;

                return false;
            }

            // State values.
            private Boolean storyboard;
            private Int32 braces;
        }

        /// <summary>
        /// Represents the lexer context which is responsible for lexing style lists.
        /// </summary>
        private sealed class LexerContext_StyleList : LexerContext
        {
            /// <inheritdoc/>
            public override void Initialize()
            {
                parens  = 0;
                braces  = 1;
                arglist = false;

                base.Initialize();
            }

            /// <inheritdoc/>
            public override Boolean Lex(String input, IList<UvssLexerToken> output, ref Int32 line, ref Int32 ix)
            {
                var storyboard = false;

                if (ConsumeWhiteSpaceAndComments(input, output, ref line, ref ix))
                    return true;

                if (!arglist)
                {
                    if (ConsumeStyleName(input, output, line, ref ix))
                        return true;
                    if (ConsumeStyleQualifier(input, output, line, ref ix))
                        return true;
                }

                if (ConsumeChildSelector(input, output, line, ref ix))
                    return true;
                if (ConsumeUniversalSelector(input, output, line, ref ix))
                    return true;
                if (ConsumeIdentifier(input, output, line, ref ix, ref storyboard))
                    return true;
                if (ConsumeNumber(input, output, line, ref ix))
                    return true;
                if (ConsumeString(input, output, line, ref ix))
                    return true;
                if (ConsumeOpenParenthesis(input, output, line, ref ix))
                {
                    if (++parens > 0)
                    {
                        arglist = true;
                    }
                    return true;
                }
                if (ConsumeCloseParenthesis(input, output, line, ref ix))
                {
                    if (--parens <= 0)
                    {
                        arglist = false;
                    }
                    return true;
                }
                if (ConsumeOpenCurlyBrace(input, output, line, ref ix))
                {
                    braces++;
                    return true;
                }
                if (ConsumeCloseCurlyBrace(input, output, line, ref ix))
                {
                    if (--braces <= 0)
                    {
                        ChangeContext(cachedLexerContext_Default);
                    }
                    return true;
                }

                if (ConsumePunctuationAndSymbols(input, output, line, ref ix))
                    return true;

                return false;
            }

            // State values.
            private Int32 parens;
            private Int32 braces;
            private Boolean arglist;
        }

        /// <summary>
        /// Represents the lexer context which is switched to after encountering the "trigger" keyword.
        /// This context assumes that the keyword is immediately followed by an identifier which specifies
        /// whether the trigger is a property or event trigger.
        /// </summary>
        private sealed class LexerContext_Trigger : LexerContext
        {
            /// <inheritdoc/>
            public override Boolean Lex(String input, IList<UvssLexerToken> output, ref Int32 line, ref Int32 ix)
            {
                if (ConsumeWhiteSpaceAndComments(input, output, ref line, ref ix))
                    return true;

                var storyboard = false;

                if (ConsumeIdentifier(input, output, line, ref ix, ref storyboard))
                {
                    if (LastTokenHasValue(output, "property"))
                    {
                        ChangeContext(cachedLexerContext_Trigger_Property);
                        return true;
                    }

                    if (LastTokenHasValue(output, "event"))
                    {
                        ChangeContext(cachedLexerContext_Trigger_Event);
                        return true;
                    }

                    throw new UvssException(PresentationStrings.StyleSheetInvalidTriggerType.Format(line, GetLastTokenValue(output)));
                }

                return false;
            }
        }

        /// <summary>
        /// Represents the lexer context which is switched to in order to process a property trigger.
        /// The trigger type identifier is immediately followed by a comma-delimited list of trigger conditions, which 
        /// is followed by a braced list of trigger actions.
        /// </summary>
        private sealed class LexerContext_Trigger_Property : LexerContext
        {
            /// <inheritdoc/>
            public override Boolean Lex(String input, IList<UvssLexerToken> output, ref Int32 line, ref Int32 ix)
            {
                while (true)
                {
                    ConsumeAllWhiteSpaceAndComments(input, output, ref line, ref ix);

                    if (!ConsumeStyleName(input, output, line, ref ix))
                        return false;

                    ConsumeAllWhiteSpaceAndComments(input, output, ref line, ref ix);

                    if (!ConsumeComparisonOperator(input, output, line, ref ix))
                        return false;

                    ConsumeAllWhiteSpaceAndComments(input, output, ref line, ref ix);

                    if (!ConsumeBracedValue(input, output, ref line, ref ix))
                        return false;

                    ConsumeAllWhiteSpaceAndComments(input, output, ref line, ref ix);

                    if (!ConsumeComma(input, output, line, ref ix))
                        break;
                }

                ChangeContext(cachedLexerContext_TriggerActionList);

                return true;
            }
        }

        /// <summary>
        /// Represents the lexer context which is switched to in order to process an event trigger.
        /// The trigger type identifier is immediately followed by a single event name, which is followed 
        /// by a braced list of trigger actions.
        /// </summary>
        private sealed class LexerContext_Trigger_Event : LexerContext
        {
            /// <inheritdoc/>
            public override Boolean Lex(String input, IList<UvssLexerToken> output, ref Int32 line, ref Int32 ix)
            {
                if (ConsumeWhiteSpaceAndComments(input, output, ref line, ref ix))
                    return true;

                if (!ConsumeStyleName(input, output, line, ref ix))
                    return false;

                ConsumeAllWhiteSpaceAndComments(input, output, ref line, ref ix);

                var storyboard = false;
                if (ConsumeOpenParenthesis(input, output, line, ref ix))
                {
                    while (true)
                    {
                        ConsumeAllWhiteSpaceAndComments(input, output, ref line, ref ix);

                        if (ConsumeIdentifier(input, output, line, ref ix, ref storyboard))
                            continue;
                        if (ConsumeComma(input, output, line, ref ix))
                            continue;

                        if (ConsumeCloseParenthesis(input, output, line, ref ix))
                            break;

                        return false;
                    }
                }

                ChangeContext(cachedLexerContext_TriggerActionList);
                return true;
            }
        }

        /// <summary>
        /// Represents the lexer context which is switched to when lexing a list of trigger actions.
        /// </summary>
        private sealed class LexerContext_TriggerActionList : LexerContext
        {
            /// <inheritdoc/>
            public override Boolean Lex(String input, IList<UvssLexerToken> output, ref Int32 line, ref Int32 ix)
            {
                var storyboard = false;

                ConsumeAllWhiteSpaceAndComments(input, output, ref line, ref ix);

                if (!ConsumeOpenCurlyBrace(input, output, line, ref ix))
                    return false;

                while (true)
                {
                    ConsumeAllWhiteSpaceAndComments(input, output, ref line, ref ix);

                    if (ConsumeCloseCurlyBrace(input, output, line, ref ix))
                    {
                        ChangeContext(cachedLexerContext_StyleList);
                        return true;
                    }

                    if (!ConsumeIdentifier(input, output, line, ref ix, ref storyboard))
                        return false;

                    var type = GetLastTokenValue(output);

                    if (String.Equals("set", type, StringComparison.InvariantCultureIgnoreCase))
                    {
                        ConsumeAllWhiteSpaceAndComments(input, output, ref line, ref ix);

                        if (!ConsumeStyleName(input, output, line, ref ix))
                            return false;

                        ConsumeAllWhiteSpaceAndComments(input, output, ref line, ref ix);

                        if (ConsumeOpenParenthesis(input, output, line, ref ix))
                        {
                            if (!ConsumeSelector(input, output, ref line, ref ix))
                                return false;

                            if (!ConsumeCloseParenthesis(input, output, line, ref ix))
                                return false;
                        }
                    }

                    ConsumeAllWhiteSpaceAndComments(input, output, ref line, ref ix);

                    if (!ConsumeBracedValue(input, output, ref line, ref ix))
                        return false;
                }
            }
        }

        /// <summary>
        /// Changes the lexer's current context.
        /// </summary>
        /// <param name="context">The lexer's new context.</param>
        private static void ChangeContext(LexerContext context)
        {
            currentContext = context ?? cachedLexerContext_Default;
            currentContext.Initialize();
        }

        // Lexer context state.
        private static readonly LexerContext cachedLexerContext_Default;
        private static readonly LexerContext cachedLexerContext_StyleList;
        private static readonly LexerContext cachedLexerContext_Trigger;
        private static readonly LexerContext cachedLexerContext_Trigger_Property;
        private static readonly LexerContext cachedLexerContext_Trigger_Event;
        private static readonly LexerContext cachedLexerContext_TriggerActionList;
        private static LexerContext currentContext;
    }
}
