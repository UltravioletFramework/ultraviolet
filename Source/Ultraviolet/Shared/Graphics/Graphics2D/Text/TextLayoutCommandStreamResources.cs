using System;
using System.Collections.Generic;
using System.Text;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Encapsulates the named resources used by an instance of the <see cref="TextLayoutCommandStream"/> class.
    /// </summary>
    internal class TextLayoutCommandStreamResources
    {
        /// <summary>
        /// Removes all items from the resource registry.
        /// </summary>
        public void Clear()
        {
            if (sources != null)
                sources.Clear();

            if (sourcesByReference != null)
                sourcesByReference.Clear();

            if (styles != null)
                styles.Clear();

            if (stylesByName != null)
                stylesByName.Clear();

            if (icons != null)
                icons.Clear();

            if (iconsByName != null)
                iconsByName.Clear();

            if (fonts != null)
                fonts.Clear();

            if (fontsByName != null)
                fontsByName.Clear();

            if (glyphShaders != null)
                glyphShaders.Clear();

            if (glyphShadersByName != null)
                glyphShadersByName.Clear();

            if (linkTargets != null)
                linkTargets.Clear();
        }

        /// <summary>
        /// Retrieves the registered source string at the specified index within the command stream's internal registry.
        /// </summary>
        /// <param name="index">The index of the registered source string to retrieve.</param>
        /// <returns>The registered source string at the specified index within the command stream's internal registry.</returns>
        public String GetSourceString(Int16 index)
        {
            if (sources == null)
                throw new IndexOutOfRangeException(nameof(index));

            return (String)sources[index];
        }

        /// <summary>
        /// Retrieves the registered source string builder at the specified index within the command stream's internal registry.
        /// </summary>
        /// <param name="index">The index of the registered source string builder to retrieve.</param>
        /// <returns>The registered source string builder at the specified index within the command stream's internal registry.</returns>
        public StringBuilder GetSourceStringBuilder(Int16 index)
        {
            if (sources == null)
                throw new IndexOutOfRangeException(nameof(index));

            return (StringBuilder)sources[index];
        }

        /// <summary>
        /// Retrieves the registered source shaped string at the specified index within the command stream's internal registry.
        /// </summary>
        /// <param name="index">The index of the registered source shaped string to retrieve.</param>
        /// <returns>The registered source shaped string at the specified index within the command stream's internal registry.</returns>
        public ShapedString GetSourceShapedString(Int16 index)
        {
            if (sources == null)
                throw new IndexOutOfRangeException(nameof(index));

            return (ShapedString)sources[index];
        }

        /// <summary>
        /// Retrieves the registered source shaped string builder at the specified index within the command stream's internal registry.
        /// </summary>
        /// <param name="index">The index of the registered source shaped string builder to retrieve.</param>
        /// <returns>The registered source shaped string builder at the specified index within the command stream's internal registry.</returns>
        public ShapedStringBuilder GetSourceShapedStringBuilder(Int16 index)
        {
            if (sources == null)
                throw new IndexOutOfRangeException(nameof(index));

            return (ShapedStringBuilder)sources[index];
        }

        /// <summary>
        /// Retrieves the registered style with the specified name.
        /// </summary>
        /// <param name="name">The name of the style to retrieve.</param>
        /// <returns>The registered style with the specified name.</returns>
        public TextStyle GetStyle(StringSegment name)
        {
            if (stylesByName == null)
                return null;

            Int16 index;
            if (!stylesByName.TryGetValue(name, out index))
                return null;

            return styles[index];
        }

        /// <summary>
        /// Retrieves the registered style at the specified index within the command stream's internal registry.
        /// </summary>
        /// <param name="index">The index of the registered style to retrieve.</param>
        /// <returns>The registered style at the specified index within the command stream's internal registry.</returns>
        public TextStyle GetStyle(Int16 index)
        {
            if (styles == null)
                throw new IndexOutOfRangeException(nameof(index));

            return styles[index];
        }

        /// <summary>
        /// Retrieves the registered icon with the specified name.
        /// </summary>
        /// <param name="name">The name of the icon to retrieve.</param>
        /// <returns>The registered icon with the specified name.</returns>
        public TextIconInfo? GetIcon(StringSegment name)
        {
            if (iconsByName == null)
                return null;

            Int16 index;
            if (!iconsByName.TryGetValue(name, out index))
                return null;

            return icons[index];
        }

        /// <summary>
        /// Retrieves the registered icon at the specified index within the command stream's internal registry.
        /// </summary>
        /// <param name="index">The index of the registered icon to retrieve.</param>
        /// <returns>The registered icon at the specified index within the command stream's internal registry.</returns>
        public TextIconInfo GetIcon(Int16 index)
        {
            if (icons == null)
                throw new IndexOutOfRangeException(nameof(index));

            return icons[index];
        }

        /// <summary>
        /// Retrieves the registered font with the specified name.
        /// </summary>
        /// <param name="name">The name of the font to retrieve.</param>
        /// <returns>The registered font with the specified name.</returns>
        public UltravioletFont GetFont(StringSegment name)
        {
            if (fontsByName == null)
                return null;

            Int16 index;
            if (!fontsByName.TryGetValue(name, out index))
                return null;

            return fonts[index];
        }

        /// <summary>
        /// Retrieves the registered font at the specified index within the command stream's internal registry.
        /// </summary>
        /// <param name="index">The index of the registered font to retrieve.</param>
        /// <returns>The registered font at the specified index within the command stream's internal registry.</returns>
        public UltravioletFont GetFont(Int16 index)
        {
            if (fonts == null)
                throw new IndexOutOfRangeException(nameof(index));

            return fonts[index];
        }

        /// <summary>
        /// Retrieves the registered glyph shader with the specified name.
        /// </summary>
        /// <param name="name">The name of the glyph shader to retrieve.</param>
        /// <returns>The registered glyph shader with the specified name.</returns>
        public GlyphShader GetGlyphShader(StringSegment name)
        {
            if (glyphShadersByName == null)
                return null;

            Int16 index;
            if (!glyphShadersByName.TryGetValue(name, out index))
                return null;

            return glyphShaders[index];
        }

        /// <summary>
        /// Retrieves the registered glyph shader at the specified index within the command stream's internal registry.
        /// </summary>
        /// <param name="index">The index of the registered glyph shader to retrieve.</param>
        /// <returns>The registered glyph shader at the specified index within the command stream's internal registry.</returns>
        public GlyphShader GetGlyphShader(Int16 index)
        {
            if (glyphShaders == null)
                throw new IndexOutOfRangeException(nameof(index));

            return glyphShaders[index];
        }

        /// <summary>
        /// Retrieves the registered link target at the specified index within the command stream's internal registry.
        /// </summary>
        /// <param name="index">The index of the registered link target to retrieve.</param>
        /// <returns>The registered link target at the specified index within the command stream's internal registry.</returns>
        public String GetLinkTarget(Int16 index)
        {
            if (linkTargets == null)
                throw new IndexOutOfRangeException(nameof(index));

            return linkTargets[index];
        }

        /// <summary>
        /// Registers a source string or string builder with the command stream.
        /// </summary>
        /// <param name="source">The source string or string builder to register.</param>
        /// <returns>The index of the source string or string builder within the command stream's internal registry.</returns>
        public Int16 RegisterSource(Object source)
        {
            Contract.Require(source, nameof(source));

            if (sources == null)
                sources = new List<Object>();

            if (sourcesByReference == null)
                sourcesByReference = new Dictionary<Object, Int16>();

            var index = default(Int16);
            if (sourcesByReference.TryGetValue(source, out index))
                return index;

            index = (Int16)sources.Count;

            if (index > Int16.MaxValue)
                throw new InvalidOperationException(UltravioletStrings.LayoutEngineHasTooManyStringSources);

            sources.Add(source);
            sourcesByReference[source] = index;

            return index;
        }

        /// <summary>
        /// Registers a style preset with the command stream.
        /// </summary>
        /// <param name="name">The name that identifies the style preset.</param>
        /// <param name="style">The style preset to register.</param>
        /// <returns>The index of the style preset within the command stream's internal registry.</returns>
        public Int16 RegisterStyle(StringSegment name, TextStyle style)
        {
            Contract.Require(style, nameof(style));

            if (styles == null)
                styles = new List<TextStyle>();

            if (stylesByName == null)
                stylesByName = new Dictionary<StringSegmentKey, Int16>();

            return RegisterResource(name, style, styles, stylesByName);
        }

        /// <summary>
        /// Registers an icon with the command stream.
        /// </summary>
        /// <param name="name">The name that identifies the icon.</param>
        /// <param name="icon">The icon to register.</param>
        /// <returns>The index of the icon within the command stream's internal registry.</returns>
        public Int16 RegisterIcon(StringSegment name, TextIconInfo icon)
        {
            if (icons == null)
                icons = new List<TextIconInfo>();

            if (iconsByName == null)
                iconsByName = new Dictionary<StringSegmentKey, Int16>();

            return RegisterResource(name, icon, icons, iconsByName);
        }

        /// <summary>
        /// Registers a font with the command stream.
        /// </summary>
        /// <param name="name">The name that identifies the font.</param>
        /// <param name="font">The font to register.</param>
        /// <returns>The index of the font within the command stream's internal registry.</returns>
        public Int16 RegisterFont(StringSegment name, UltravioletFont font)
        {
            Contract.Require(font, nameof(font));

            if (fonts == null)
                fonts = new List<UltravioletFont>();

            if (fontsByName == null)
                fontsByName = new Dictionary<StringSegmentKey, Int16>();

            return RegisterResource(name, font, fonts, fontsByName);
        }

        /// <summary>
        /// Registers a glyph shader with the command stream.
        /// </summary>
        /// <param name="name">The name that identifies the glyph shader.</param>
        /// <param name="glyphShader">The glyph shader to register.</param>
        /// <returns>The index of the glyph shader within the command stream's internal registry.</returns>
        public Int16 RegisterGlyphShader(StringSegment name, GlyphShader glyphShader)
        {
            Contract.Require(glyphShader, nameof(glyphShader));

            if (glyphShaders == null)
                glyphShaders = new List<GlyphShader>();

            if (glyphShadersByName == null)
                glyphShadersByName = new Dictionary<StringSegmentKey, Int16>();

            return RegisterResource(name, glyphShader, glyphShaders, glyphShadersByName);
        }

        /// <summary>
        /// Registers a link target with the command stream.
        /// </summary>
        /// <param name="target">The link target to register.</param>
        /// <returns>The index of the link target within the command stream's internal registry.</returns>
        public Int16 RegisterLinkTarget(String target)
        {
            Contract.Require(target, nameof(target));

            if (linkTargets == null)
                linkTargets = new List<String>();

            return RegisterResource(new StringSegment(target), target, linkTargets, null);
        }

        /// <summary>
        /// Registers a resource with the command stream.
        /// </summary>
        private Int16 RegisterResource<TResource>(StringSegment name, TResource resource, List<TResource> resourcesList, Dictionary<StringSegmentKey, Int16> resourcesByName)
        {
            Int16 index;
            if (resourcesByName != null && resourcesByName.TryGetValue(name, out index))
                return index;

            index = (Int16)resourcesList.Count;

            if (index > Int16.MaxValue)
                throw new InvalidOperationException(UltravioletStrings.LayoutEngineHasTooManyResources);

            resourcesList.Add(resource);

            if (resourcesByName != null)
                resourcesByName[name] = index;

            return index;
        }

        // String source registry
        private List<Object> sources = new List<Object>();
        private Dictionary<Object, Int16> sourcesByReference;

        // Style preset registry
        private List<TextStyle> styles = new List<TextStyle>();
        private Dictionary<StringSegmentKey, Int16> stylesByName;

        // Icon registry
        private List<TextIconInfo> icons = new List<TextIconInfo>();
        private Dictionary<StringSegmentKey, Int16> iconsByName;

        // Font registry
        private List<UltravioletFont> fonts = new List<UltravioletFont>();
        private Dictionary<StringSegmentKey, Int16> fontsByName;

        // Glyph shader registry
        private List<GlyphShader> glyphShaders = new List<GlyphShader>();
        private Dictionary<StringSegmentKey, Int16> glyphShadersByName;
        
        // Link target registry
        private List<String> linkTargets = new List<String>(0);
    }
}
