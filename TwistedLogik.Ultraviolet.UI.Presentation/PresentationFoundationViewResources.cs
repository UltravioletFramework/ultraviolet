using System;
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
            internal set { SetValue<SourcedImage>(BlankImageProperty, value); }
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
            new PropertyMetadata<SourcedImage>(HandleBlankImagePropertyChanged));
        
        /// <summary>
        /// Reloads the view's resources.
        /// </summary>
        internal void Reload()
        {
            ReloadBlankImage();
        }

        /// <summary>
        /// Reloads the image exposed by the <see cref="BlankImage"/> property.
        /// </summary>
        internal void ReloadBlankImage()
        {
            view.LoadImage(BlankImage);
        }

        /// <inheritdoc/>
        internal sealed override Object DependencyDataSource
        {
            get { return null; }
        }

        /// <inheritdoc/>
        internal sealed override DependencyObject DependencyContainer
        {
            get { return null; }
        }

        /// <inheritdoc/>
        protected internal sealed override void ApplyStyles(UvssDocument document)
        {
            var rule = document.Rules.Where(x => x.IsViewResourceRule()).LastOrDefault();
            if (rule != null)
            {
                foreach (var style in rule.Styles)
                {
                    var dp = DependencyProperty.FindByStylingName(style.Name, GetType());
                    if (dp != null)
                    {
                        base.ApplyStyle(style, rule.Selectors[0], dp);
                    }
                }
            }
            base.ApplyStyles(document);
        }

        /// <inheritdoc/>
        protected internal sealed override void ApplyStyle(UvssStyle style, UvssSelector selector, DependencyProperty dp)
        {
            base.ApplyStyle(style, selector, dp);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="BlankImageProperty"/> dependency property changes.
        /// </summary>
        private static void HandleBlankImagePropertyChanged(DependencyObject dobj, SourcedImage oldValue, SourcedImage newValue)
        {
            var resources = (PresentationFoundationViewResources)dobj;
            resources.ReloadBlankImage();
        }

        // Property values.
        private readonly StringFormatter stringFormatter = new StringFormatter();
        private readonly StringBuilder stringBuffer = new StringBuilder();
        private readonly TextRenderer textRenderer = new TextRenderer();

        // State values.
        private readonly PresentationFoundationView view;
    }
}
