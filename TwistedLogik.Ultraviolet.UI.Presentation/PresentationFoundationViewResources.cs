using System.Linq;
using System.Text;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Text;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.UI.Presentation
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
            Contract.Require(view, "view");

            this.view = view;
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
        /// Gets the view's global string formatter.
        /// </summary>
        public StringFormatter StringFormatter
        {
            get { return stringFormatter; }
        }

        /// <summary>
        /// Gets the view's global string buffer.
        /// </summary>
        public StringBuilder StringBuffer
        {
            get { return stringBuffer; }
        }

        /// <summary>
        /// Gets the view's global text renderer.
        /// </summary>
        public TextRenderer TextRenderer
        {
            get { return textRenderer; }
        }

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
        /// Reloads the view's resources.
        /// </summary>
        internal void Reload()
        {
            ReloadBlankImage();
            ReloadCursor();
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

        /// <inheritdoc/>
        protected internal sealed override void ApplyStyles(UvssDocument document)
        {
            var rule = document.RuleSets.Where(x => x.IsViewResourceRule()).LastOrDefault();
            if (rule != null)
            {
                foreach (var style in rule.Rules)
                {
                    var dp = DependencyProperty.FindByStylingName(style.Name, GetType());
                    if (dp != null)
                    {
                        var selector = rule.Selectors[0];
                        var navexp = NavigationExpression.FromUvssNavigationExpression(
                            view.Ultraviolet, selector.NavigationExpression);
                        base.ApplyStyle(style, selector, navexp, dp);
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

        // Property values.
        private readonly StringFormatter stringFormatter = new StringFormatter();
        private readonly StringBuilder stringBuffer = new StringBuilder();
        private readonly TextRenderer textRenderer = new TextRenderer();

        // State values.
        private readonly PresentationFoundationView view;
    }
}
