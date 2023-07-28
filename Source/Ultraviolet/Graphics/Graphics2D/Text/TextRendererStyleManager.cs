using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    partial class TextRenderer
    {
        /// <summary>
        /// Contains methods for managing the styles used to render a block of text.
        /// </summary>
        private class TextRendererStyleManager
        {
            /// <summary>
            /// Clears all of the renderer's layout parameter stacks.
            /// </summary>
            public void ClearLayoutStacks()
            {
                StyleStack.Clear();
                FontStack.Clear();
                ColorStack.Clear();
                GlyphShaderStack.Clear();
                LinkStack.Clear();
            }

            /// <summary>
            /// Pushes a value onto a style-scoped stack.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void PushScopedStack<T>(Stack<TextStyleScoped<T>> stack, T value)
            {
                var scope = StyleStack.Count;
                stack.Push(new TextStyleScoped<T>(value, scope));
            }

            /// <summary>
            /// Pushes a style onto the style stack.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void PushStyle(TextStyle style, ref Boolean bold, ref Boolean italic)
            {
                var instance = new TextStyleInstance(style, bold, italic);
                StyleStack.Push(instance);

                if (style.Font != null)
                    PushFont(style.Font);

                if (style.Color.HasValue)
                    PushColor(style.Color.Value);

                if (style.GlyphShaders.Count > 0)
                {
                    foreach (var glyphShader in style.GlyphShaders)
                        PushGlyphShader(glyphShader);
                }

                if (style.Bold.HasValue)
                    bold = style.Bold.Value;

                if (style.Italic.HasValue)
                    italic = style.Italic.Value;
            }

            /// <summary>
            /// Pushes a font onto the font stack.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void PushFont(UltravioletFont font)
            {
                PushScopedStack(FontStack, font);
            }

            /// <summary>
            /// Pushes a color onto the color stack.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void PushColor(Color color)
            {
                PushScopedStack(ColorStack, color);
            }

            /// <summary>
            /// Pushes a glyph shader onto the glyph shader stack.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void PushGlyphShader(GlyphShader glyphShader)
            {
                PushScopedStack(GlyphShaderStack, glyphShader);
            }

            /// <summary>
            /// Pushes a link onto the link stack.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void PushLink(Int16 linkIndex)
            {
                LinkStack.Push(linkIndex);
            }

            /// <summary>
            /// Pops a value off of a style-scoped stack.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void PopScopedStack<T>(Stack<TextStyleScoped<T>> stack)
            {
                if (stack.Count == 0)
                    return;

                var scope = StyleStack.Count;
                if (stack.Peek().Scope != scope)
                    return;

                stack.Pop();
            }

            /// <summary>
            /// Pops a style off of the style stack.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void PopStyle(ref Boolean bold, ref Boolean italic)
            {
                if (StyleStack.Count > 0)
                {
                    PopStyleScope();

                    var instance = StyleStack.Pop();
                    bold = instance.Bold;
                    italic = instance.Italic;
                }
            }

            /// <summary>
            /// Pops a font off of the font stack.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void PopFont()
            {
                PopScopedStack(FontStack);
            }

            /// <summary>
            /// Pops a color off of the color stack.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void PopColor()
            {
                PopScopedStack(ColorStack);
            }

            /// <summary>
            /// Pops a glyph shader off of the glyph shader stack.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void PopGlyphShader()
            {
                PopScopedStack(GlyphShaderStack);
            }

            /// <summary>
            /// Pops a link off of the link stack.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void PopLink()
            {
                if (LinkStack.Count == 0)
                    return;

                LinkStack.Pop();
            }

            /// <summary>
            /// Pops the current style scope off of the stacks.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void PopStyleScope()
            {
                var scope = StyleStack.Count;

                while (FontStack.Count > 0 && FontStack.Peek().Scope == scope)
                    FontStack.Pop();

                while (ColorStack.Count > 0 && ColorStack.Peek().Scope == scope)
                    ColorStack.Pop();

                while (GlyphShaderStack.Count > 0 && GlyphShaderStack.Peek().Scope == scope)
                    GlyphShaderStack.Pop();
            }

            /// <summary>
            /// Gets the stack that tracks text styles.
            /// </summary>
            public Stack<TextStyleInstance> StyleStack { get; } = new Stack<TextStyleInstance>();

            /// <summary>
            /// Gets the stack that tracks fonts.
            /// </summary>
            public Stack<TextStyleScoped<UltravioletFont>> FontStack { get; } = new Stack<TextStyleScoped<UltravioletFont>>();

            /// <summary>
            /// Gets the stack that tracks colors.
            /// </summary>
            public Stack<TextStyleScoped<Color>> ColorStack { get; } = new Stack<TextStyleScoped<Color>>();

            /// <summary>
            /// Gets the stack that tracks glyph shaders.
            /// </summary>
            public Stack<TextStyleScoped<GlyphShader>> GlyphShaderStack { get; } = new Stack<TextStyleScoped<GlyphShader>>();

            /// <summary>
            /// Gets the stack that tracks links.
            /// </summary>
            public Stack<Int16> LinkStack { get; } = new Stack<Int16>();
        }
    }
}
