using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics.Graphics2D.Text;
using Ultraviolet.Presentation.Styles;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Contains the global resources used by a Presentation Foundation view.
    /// </summary>
    public sealed class PresentationFoundationViewResources : DependencyObject
    {
        /// <summary>
        /// Initializes the <see cref="PresentationFoundationViewResources"/> type.
        /// </summary>
        static PresentationFoundationViewResources()
        {
            /* required to correctly initialize static fields in Release configuration */
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationFoundationViewResources"/> class.
        /// </summary>
        /// <param name="view">The view that owns this resource collection.</param>
        internal PresentationFoundationViewResources(PresentationFoundationView view)
        {
            Contract.Require(view, nameof(view));

            this.view = view;

            this.StringFormatter = new StringFormatter();
            this.StringBuffer = new StringBuilder();
            this.TextRenderer = new TextRenderer();

            if (view.TextShaper != null)
                this.TextRenderer.LayoutEngine.RegisterTextShaper(view.TextShaper);
        }

        /// <summary>
        /// Gets or sets the blank image used by this view for rendering elements.
        /// </summary>
        public SourcedImage BlankImage
        {
            get { return GetValue<SourcedImage>(BlankImageProperty); }
            internal set { SetValue(BlankImageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the view's default cursor.
        /// </summary>
        public SourcedCursor Cursor
        {
            get { return GetValue<SourcedCursor>(CursorProperty); }
            internal set { SetValue(CursorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the asset identifier of the file which defines the 
        /// named styles available to the view's text renderer.
        /// </summary>
        public SourcedAssetID TextStyles
        {
            get { return GetValue<SourcedAssetID>(TextStylesProperty); }
            internal set { SetValue(TextStylesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the asset identifier of the file which defines the
        /// named fonts available to the view's text renderer.
        /// </summary>
        public SourcedAssetID TextFonts
        {
            get { return GetValue<SourcedAssetID>(TextFontsProperty); }
            internal set { SetValue(TextFontsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the asset identifier of the file which defines the
        /// named fallback fonts available to the view's text renderer.
        /// </summary>
        public SourcedAssetID TextFallbackFonts
        {
            get { return GetValue<SourcedAssetID>(TextFallbackFontsProperty); }
            internal set { SetValue(TextFallbackFontsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the asset identifier of the file which defines the
        /// named icons available to the view's text renderer.
        /// </summary>
        public SourcedAssetID TextIcons
        {
            get { return GetValue<SourcedAssetID>(TextIconsProperty); }
            internal set { SetValue(TextIconsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the asset identifier of the file which defines the
        /// named glyph shaders available to the view's text renderer.
        /// </summary>
        public SourcedAssetID TextShaders
        {
            get { return GetValue<SourcedAssetID>(TextShadersProperty); }
            internal set { SetValue(TextShadersProperty, value); }
        }

        /// <summary>
        /// Gets or sets the name of the view model method which colorizes links.
        /// </summary>
        public String LinkColorizer
        {
            get { return GetValue<String>(LinkColorizerProperty); }
            internal set { SetValue(LinkColorizerProperty, value); }
        }

        /// <summary>
        /// Gets or sets the name of the view model method which evaluates the state of links.
        /// </summary>
        public String LinkStateEvaluator
        {
            get { return GetValue<String>(LinkStateEvaluatorProperty); }
            internal set { SetValue(LinkStateEvaluatorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the name of the view model method which handles clicked links.
        /// </summary>
        public String LinkClickHandler
        {
            get { return GetValue<String>(LinkClickHandlerProperty); }
            internal set { SetValue(LinkClickHandlerProperty, value); }
        }

        /// <summary>
        /// Gets the view's global string formatter.
        /// </summary>
        public StringFormatter StringFormatter { get; }

        /// <summary>
        /// Gets the view's global string buffer.
        /// </summary>
        public StringBuilder StringBuffer { get; }

        /// <summary>
        /// Gets the view's global text renderer.
        /// </summary>
        public TextRenderer TextRenderer { get; }

        /// <summary>
        /// Identifies the <see cref="BlankImage"/> dependency property.
        /// </summary>
        internal static readonly DependencyProperty BlankImageProperty = DependencyProperty.Register("BlankImage", typeof(SourcedImage), typeof(PresentationFoundationViewResources),
            new PropertyMetadata<SourcedImage>(HandleBlankImageChanged));

        /// <summary>
        /// Identifies the <see cref="Cursor"/> dependency property.
        /// </summary>
        internal static readonly DependencyProperty CursorProperty = DependencyProperty.Register("Cursor", typeof(SourcedCursor), typeof(PresentationFoundationViewResources),
            new PropertyMetadata<SourcedCursor>(HandleCursorChanged));

        /// <summary>
        /// Identifies the <see cref="TextStyles"/> dependency property.
        /// </summary>
        internal static readonly DependencyProperty TextStylesProperty = DependencyProperty.Register("TextStyles", typeof(SourcedAssetID), typeof(PresentationFoundationViewResources),
            new PropertyMetadata<SourcedAssetID>(HandleTextStylesChanged));

        /// <summary>
        /// Identifies the <see cref="TextFonts"/> dependency property.
        /// </summary>
        internal static readonly DependencyProperty TextFontsProperty = DependencyProperty.Register("TextFonts", typeof(SourcedAssetID), typeof(PresentationFoundationViewResources),
            new PropertyMetadata<SourcedAssetID>(HandleTextFontsChanged));

        /// <summary>
        /// Identifies the <see cref="TextFonts"/> dependency property.
        /// </summary>
        internal static readonly DependencyProperty TextFallbackFontsProperty = DependencyProperty.Register("TextFallbackFonts", typeof(SourcedAssetID), typeof(PresentationFoundationViewResources),
            new PropertyMetadata<SourcedAssetID>(HandleTextFallbackFontsChanged));

        /// <summary>
        /// Identifies the <see cref="TextIcons"/> dependency property.
        /// </summary>
        internal static readonly DependencyProperty TextIconsProperty = DependencyProperty.Register("TextIcons", typeof(SourcedAssetID), typeof(PresentationFoundationViewResources),
            new PropertyMetadata<SourcedAssetID>(HandleTextIconsChanged));

        /// <summary>
        /// Identifies the <see cref="TextShaders"/> dependency property.
        /// </summary>
        internal static readonly DependencyProperty TextShadersProperty = DependencyProperty.Register("TextShaders", typeof(SourcedAssetID), typeof(PresentationFoundationViewResources),
            new PropertyMetadata<SourcedAssetID>(HandleTextShadersChanged));

        /// <summary>
        /// Identifies the <see cref="LinkColorizer"/> dependency property.
        /// </summary>
        internal static readonly DependencyProperty LinkColorizerProperty = DependencyProperty.Register("LinkColorizer", typeof(String), typeof(PresentationFoundationViewResources),
            new PropertyMetadata<String>(HandleLinkColorizerChanged));

        /// <summary>
        /// Identifies the <see cref="LinkStateEvaluator"/> dependency property.
        /// </summary>
        internal static readonly DependencyProperty LinkStateEvaluatorProperty = DependencyProperty.Register("LinkStateEvaluator", typeof(String), typeof(PresentationFoundationViewResources),
            new PropertyMetadata<String>(HandleLinkStateEvaluatorChanged));

        /// <summary>
        /// Identifies the <see cref="LinkClickHandler"/> dependency property.
        /// </summary>
        internal static readonly DependencyProperty LinkClickHandlerProperty = DependencyProperty.Register("LinkClickHandler", typeof(String), typeof(PresentationFoundationViewResources),
            new PropertyMetadata<String>(HandleLinkClickHandlerChanged));

        /// <summary>
        /// Reloads the view's resources.
        /// </summary>
        internal void Reload()
        {
            ReloadBlankImage();
            ReloadCursor();
            ReloadTextStyles();
            ReloadTextFonts();
            ReloadTextFallbackFonts();
            ReloadTextIcons();
            ReloadTextShaders();
        }

        /// <summary>
        /// Reloads the image exposed by the <see cref="BlankImage"/> property.
        /// </summary>
        internal void ReloadBlankImage()
        {
            view.LoadImage(BlankImage);
        }

        /// <summary>
        /// Reloads the cursor exposed by the <see cref="Cursor"/> property.
        /// </summary>
        internal void ReloadCursor()
        {
            view.LoadCursor(Cursor);
        }

        /// <summary>
        /// Reloads the named style presets exposed by the <see cref="TextShaders"/> property.
        /// </summary>
        internal void ReloadTextStyles()
        {
            TextRenderer.LayoutEngine.ClearStyles();

            var dictionary = LoadJsonResource<IDictionary<String, SourcedTextStyleDescription>>(TextStyles);
            if (dictionary == null)
                return;

            foreach (var kvp in dictionary)
            {
                var description = kvp.Value;
                var font = default(UltravioletFont);

                if (description.Font != null)
                {
                    font = view.LoadResource<UltravioletFont>(description.Font.Value);
                    if (font == null)
                        throw new InvalidOperationException(PresentationStrings.CollectionContainsInvalidResources);
                }

                var style = new TextStyle(font,
                    description.Bold,
                    description.Italic,
                    description.Color,
                    description.GlyphShaders);
                TextRenderer.LayoutEngine.RegisterStyle(kvp.Key, style);
            }
        }

        /// <summary>
        /// Reloads the named fonts exposed by the <see cref="TextFonts"/> property.
        /// </summary>
        internal void ReloadTextFonts()
        {
            TextRenderer.LayoutEngine.ClearFonts();

            var dictionary = LoadJsonResource<IDictionary<String, SourcedAssetID>>(TextFonts);
            if (dictionary == null)
                return;

            foreach (var kvp in dictionary)
            {
                if (!kvp.Value.AssetID.IsValid)
                    throw new InvalidOperationException(PresentationStrings.CollectionContainsInvalidResources);

                var font = view.LoadResource<UltravioletFont>(kvp.Value);
                TextRenderer.LayoutEngine.RegisterFont(kvp.Key, font);
            }
        }

        /// <summary>
        /// Reloads the named fonts exposed by the <see cref="TextFallbackFonts"/> property.
        /// </summary>
        internal void ReloadTextFallbackFonts()
        {
            TextRenderer.LayoutEngine.ClearFallbackFonts();

            var dictionary = LoadJsonResource<IDictionary<String, FallbackFontInfo>>(TextFallbackFonts);
            if (dictionary == null)
                return;

            foreach (var kvp in dictionary)
            {
                TextRenderer.LayoutEngine.RegisterFallbackFont(kvp.Key, kvp.Value.RangeStart, kvp.Value.RangeEnd, kvp.Value.Font.ToString());
            }
        }    

        /// <summary>
        /// Reloads the named glyph icons exposed by the <see cref="TextShaders"/> property.
        /// </summary>
        internal void ReloadTextIcons()
        {
            TextRenderer.LayoutEngine.ClearIcons();

            var dictionary = LoadJsonResource<IDictionary<String, SourcedTextIconDescription>>(TextIcons);
            if (dictionary == null)
                return;

            foreach (var kvp in dictionary)
            {
                if (!kvp.Value.Icon.SpriteAnimationID.IsValid)
                    throw new InvalidOperationException(PresentationStrings.CollectionContainsInvalidResources);

                var icon = view.LoadResource(kvp.Value.Icon);
                TextRenderer.LayoutEngine.RegisterIcon(kvp.Key, icon, kvp.Value.Width, kvp.Value.Height);
            }
        }

        /// <summary>
        /// Reloads the named glyph shaders exposed by the <see cref="TextShaders"/> property.
        /// </summary>
        internal void ReloadTextShaders()
        {
            TextRenderer.LayoutEngine.ClearGlyphShaders();

            var dictionary = LoadJsonResource<IDictionary<String, GlyphShader>>(TextShaders);
            if (dictionary == null)
                return;

            foreach (var kvp in dictionary)
                TextRenderer.LayoutEngine.RegisterGlyphShader(kvp.Key, kvp.Value);
        }

        /// <inheritdoc/>
        protected internal sealed override void ApplyStyles(UvssDocument document)
        {
            var ruleSets = document.RuleSets.Where(x => x.IsViewResourceRule());
            if (ruleSets?.Any() ?? false)
            {
                foreach (var ruleSet in ruleSets)
                {
                    foreach (var rule in ruleSet.Rules)
                    {
                        var dp = DependencyProperty.FindByStylingName(rule.Name, GetType());
                        if (dp != null)
                        {
                            var selector = ruleSet.Selectors[0];
                            var navexp = NavigationExpression.FromUvssNavigationExpression(
                                view.Ultraviolet, selector.NavigationExpression);
                            base.ApplyStyle(rule, selector, navexp, dp);
                        }
                    }
                }
            }
            base.ApplyStyles(document);
        }

        /// <inheritdoc/>
        protected internal sealed override void ApplyStyle(UvssRule style, UvssSelector selector, NavigationExpression? navigationExpression, DependencyProperty dp)
        {
            base.ApplyStyle(style, selector, navigationExpression, dp);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="BlankImage"/> dependency property changes.
        /// </summary>
        private static void HandleBlankImageChanged(DependencyObject dobj, SourcedImage oldValue, SourcedImage newValue)
        {
            var resources = (PresentationFoundationViewResources)dobj;
            resources.ReloadBlankImage();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Cursor"/> dependency property changes.
        /// </summary>
        private static void HandleCursorChanged(DependencyObject dobj, SourcedCursor oldValue, SourcedCursor newValue)
        {
            var resources = (PresentationFoundationViewResources)dobj;
            resources.ReloadCursor();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="TextStyles"/> dependency property changes.
        /// </summary>
        private static void HandleTextStylesChanged(DependencyObject dobj, SourcedAssetID oldValue, SourcedAssetID newValue)
        {
            var resources = (PresentationFoundationViewResources)dobj;
            resources.ReloadTextStyles();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="TextFonts"/> dependency property changes.
        /// </summary>
        private static void HandleTextFontsChanged(DependencyObject dobj, SourcedAssetID oldValue, SourcedAssetID newValue)
        {
            var resources = (PresentationFoundationViewResources)dobj;
            resources.ReloadTextFonts();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="TextFallbackFonts"/> dependency property changes.
        /// </summary>
        private static void HandleTextFallbackFontsChanged(DependencyObject dobj, SourcedAssetID oldValue, SourcedAssetID newValue)
        {
            var resources = (PresentationFoundationViewResources)dobj;
            resources.ReloadTextFallbackFonts();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="TextIcons"/> dependency property changes.
        /// </summary>
        private static void HandleTextIconsChanged(DependencyObject dobj, SourcedAssetID oldValue, SourcedAssetID newValue)
        {
            var resources = (PresentationFoundationViewResources)dobj;
            resources.ReloadTextIcons();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="TextShaders"/> dependency property changes.
        /// </summary>
        private static void HandleTextShadersChanged(DependencyObject dobj, SourcedAssetID oldValue, SourcedAssetID newValue)
        {
            var resources = (PresentationFoundationViewResources)dobj;
            resources.ReloadTextShaders();
        }

        /// <summary>
        /// Occurs when the value of the <see cref="LinkColorizer"/> dependency property changes.
        /// </summary>
        private static void HandleLinkColorizerChanged(DependencyObject dobj, String oldValue, String newValue)
        {
            var resources = ((PresentationFoundationViewResources)dobj);
            var view = resources.view;

            if (String.IsNullOrEmpty(newValue))
            {
                resources.TextRenderer.LinkColorizer = null;
            }
            else
            {
                var linkColorizerBinding = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                var linkColorizerTypes = new[] { typeof(String), typeof(Boolean), typeof(Boolean), typeof(Boolean), typeof(Color) };
                var linkColorizer = view.ViewModelType.GetMethod(newValue, linkColorizerBinding, null, linkColorizerTypes, null);

                if (linkColorizer?.ReturnType != typeof(Color))
                    throw new InvalidOperationException(PresentationStrings.InvalidLinkColorizer.Format(newValue));
                
                resources.TextRenderer.LinkColorizer =
                    CompileViewModelMethodLambda<LinkColorizer>(view, linkColorizer);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="LinkStateEvaluator"/> dependency property changes.
        /// </summary>
        private static void HandleLinkStateEvaluatorChanged(DependencyObject dobj, String oldValue, String newValue)
        {
            var resources = ((PresentationFoundationViewResources)dobj);
            var view = resources.view;

            if (String.IsNullOrEmpty(newValue))
            {
                resources.TextRenderer.LinkStateEvaluator = null;
            }
            else
            {
                var linkStateBindings = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                var linkStateTypes = new[] { typeof(String) };
                var linkStateEvaluator = view.ViewModelType.GetMethod(newValue, linkStateBindings, null, linkStateTypes, null);

                if (linkStateEvaluator?.ReturnType != typeof(Boolean))
                    throw new InvalidOperationException(PresentationStrings.InvalidLinkStateEvaluator.Format(newValue));
                
                resources.TextRenderer.LinkStateEvaluator = 
                    CompileViewModelMethodLambda<LinkStateEvaluator>(view, linkStateEvaluator);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="LinkClickHandler"/> dependency property changes.
        /// </summary>
        private static void HandleLinkClickHandlerChanged(DependencyObject dobj, String oldValue, String newValue)
        {
            var resources = ((PresentationFoundationViewResources)dobj);
            var view = resources.view;

            if (String.IsNullOrEmpty(newValue))
            {
                resources.TextRenderer.LinkStateEvaluator = null;
            }
            else
            {
                var linkStateBindings = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                var linkStateTypes = new[] { typeof(String) };
                var linkClickHandler = view.ViewModelType.GetMethod(newValue, linkStateBindings, null, linkStateTypes, null);

                if (linkClickHandler?.ReturnType != typeof(Boolean))
                    throw new InvalidOperationException(PresentationStrings.InvalidLinkClickHandler.Format(newValue));
                
                resources.TextRenderer.LinkClickHandler = 
                    CompileViewModelMethodLambda<LinkClickHandler>(view, linkClickHandler);
            }
        }

        /// <summary>
        /// Compiles a delegate which invokes a method on the view model.
        /// </summary>
        private static TLambda CompileViewModelMethodLambda<TLambda>(PresentationFoundationView view, MethodInfo methodInfo)
        {
            var invoke = typeof(TLambda).GetMethod("Invoke");
            var invokeParams = invoke.GetParameters();

            var expParams = invokeParams.Select(x => Expression.Parameter(x.ParameterType, x.Name)).ToList();
            var expView = Expression.Constant(view, typeof(PresentationFoundationView));
            var expViewModel = Expression.Property(expView, nameof(PresentationFoundationView.ViewModel));
            var expViewModelTyped = Expression.Convert(expViewModel, view.ViewModelType);
            var expInvoke = Expression.Call(expViewModelTyped, methodInfo, expParams);

            return Expression.Lambda<TLambda>(expInvoke, expParams).Compile();
        }

        /// <summary>
        /// Loads a dictionary of JSON resource definitions specified by the given asset identifier.
        /// </summary>
        private T LoadJsonResource<T>(SourcedAssetID id)
        {
            if (!id.AssetID.IsValid)
                return default(T);

            var definition = view.LoadResource<JObject>(id);
            if (definition == null)
                return default(T);

            var settings = new UltravioletJsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
            var serializer = JsonSerializer.CreateDefault(settings);
            var resource = definition.ToObject<T>(serializer);

            return resource;
        }

        // State values.
        private readonly PresentationFoundationView view;
    }
}
