using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Data;
using TwistedLogik.Ultraviolet.Layout.Stylesheets;

namespace TwistedLogik.Ultraviolet.Layout.Elements
{
    /// <summary>
    /// Represents a method which sets the value of a styled property on a UI element.
    /// </summary>
    /// <param name="element">The UI element on which to set the style.</param>
    /// <param name="value">The string representation of the value to set for the style.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    internal delegate void StyleSetter(UIElement element, String value, IFormatProvider provider);

    /// <summary>
    /// The base class for all UI elements.
    /// </summary>
    public abstract class UIElement : DependencyObject
    {
        /// <summary>
        /// Initialies the <see cref="UIElement"/> type.
        /// </summary>
        static UIElement()
        {
            miFromString = typeof(ObjectResolver).GetMethod("FromString", new Type[] { typeof(String), typeof(Type), typeof(IFormatProvider) });
            miSetStyledValue = typeof(DependencyObject).GetMethod("SetStyledValue");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIElement"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="id">The element's unique identifier within its layout.</param>
        public UIElement(UltravioletContext uv, String id)
        {
            Contract.Require(uv, "uv");

            this.uv   = uv;
            this.id   = id;
            this.name = GetType().Name;

            CreateStyleSetters();
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Updates the element's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        public void Update(UltravioletTime time)
        {
            Digest();
        }

        /// <summary>
        /// Called when the element should reload its content.
        /// </summary>
        public void ReloadContent()
        {
            OnReloadingContent();
        }

        /// <summary>
        /// Gets the Ultraviolet context that created the element.
        /// </summary>
        public UltravioletContext Ultraviolet
        {
            get { return uv; }
        }

        /// <summary>
        /// Gets the element's unique identifier within its layout.
        /// </summary>
        public String ID
        {
            get { return id; }
        }

        /// <summary>
        /// Gets the element's name based on its type.
        /// </summary>
        public String Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the <see cref="UIViewport"/> that is the top-level container for this element.
        /// </summary>
        public UIViewport Viewport
        {
            get { return viewport; }
        }

        /// <summary>
        /// Gets the <see cref="UIContainer"/> that contains this element.
        /// </summary>
        public UIContainer Container
        {
            get { return container; }
        }

        /// <summary>
        /// Gets a value indicating whether this is an anonymous element.
        /// </summary>
        /// <remarks>An anonymous element is one which has no explicit identifier.</remarks>
        public Boolean IsAnonymous
        {
            get { return String.IsNullOrEmpty(id); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the element is enabled.
        /// </summary>
        public Boolean IsEnabled
        {
            get { return GetValue<Boolean>(dpIsEnabled); }
            set { SetValue<Boolean>(dpIsEnabled, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the element is visible.
        /// </summary>
        public Boolean IsVisible
        {
            get { return GetValue<Boolean>(dpIsVisible); }
            set { SetValue<Boolean>(dpIsVisible, value); }
        }

        /// <summary>
        /// Gets the element's area relative to its container after layout has been performed.
        /// </summary>
        protected internal Rectangle ContainerRelativeLayout
        {
            get { return new Rectangle(containerRelativeX, containerRelativeY, calculatedWidth, calculatedHeight); }
            set
            {
                containerRelativeX = value.X;
                containerRelativeY = value.Y;
                calculatedWidth = value.Width;
                calculatedHeight = value.Height;
            }
        }

        /// <summary>
        /// Gets the x-coordinate of the element relative to its container after layout has been performed.
        /// </summary>
        protected internal Int32 ContainerRelativeX
        {
            get { return containerRelativeX; }
            internal set { containerRelativeX = value; }
        }

        /// <summary>
        /// Gets the y-coordinate of the element relative to its container after layout has been performed.
        /// </summary>
        protected internal Int32 ContainerRelativeY
        {
            get { return containerRelativeY; }
            internal set { containerRelativeY = value; }
        }

        /// <summary>
        /// Gets the element's width as calculated during layout.
        /// </summary>
        protected internal Int32 CalculatedWidth
        {
            get { return calculatedWidth; }
            internal set { calculatedWidth = value; }
        }

        /// <summary>
        /// Gets the element's height as calculated during layout.
        /// </summary>
        protected internal Int32 CalculatedHeight
        {
            get { return calculatedHeight; }
            internal set { calculatedHeight = value; }
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing"><c>true</c> if the object is being disposed; <c>false</c> if the object is being finalized.</param>
        protected virtual void Dispose(Boolean disposing)
        {

        }

        /// <summary>
        /// Called when the element should reload its content.
        /// </summary>
        protected virtual void OnReloadingContent()
        {

        }

        /// <summary>
        /// Applies a style to the element.
        /// </summary>
        /// <param name="name">The name of the style.</param>
        /// <param name="value">The value to apply to the style.</param>
        /// <param name="attached">A value indicating whether thie style represents an attached property.</param>
        internal void ApplyStyle(String name, String value, Boolean attached)
        {
            Contract.RequireNotEmpty(name, "name");
            Contract.RequireNotEmpty(value, "value");

            var setter = attached ? Container.GetStyleSetter(name) : GetStyleSetter(name);
            if (setter == null)
                return;

            setter(this, value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Updates the viewport associated with this element.
        /// </summary>
        /// <param name="viewport">The viewport to associate with this element.</param>
        internal virtual void UpdateViewport(UIViewport viewport)
        {
            this.viewport = viewport;
            ReloadContent();
        }

        /// <summary>
        /// Updates the container which holds this element.
        /// </summary>
        /// <param name="container">The container to associate with this element.</param>
        internal virtual void UpdateContainer(UIContainer container)
        {
            this.container = container;

            var viewport = (container == null) ? null : container.Viewport;
            if (viewport != this.viewport)
            {
                UpdateViewport(viewport);
            }
        }

        /// <summary>
        /// Creates the element's style setters.
        /// </summary>
        private void CreateStyleSetters()
        {
            var typeID = GetType().TypeHandle.Value.ToInt64();

            lock (styleSetters)
            {
                Dictionary<String, StyleSetter> styleSettersForCurrentType;
                if (!styleSetters.TryGetValue(typeID, out styleSettersForCurrentType))
                {
                    styleSettersForCurrentType = new Dictionary<String, StyleSetter>(StringComparer.OrdinalIgnoreCase);
                    
                    var styledDependencyProperties = 
                        from field in GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                        let attr = field.GetCustomAttributes(typeof(StyledAttribute), false).SingleOrDefault()
                        let type = field.FieldType
                        let name = field.Name
                        where
                            type == typeof(DependencyProperty)
                        select new { Attribute = (StyledAttribute)attr, FieldInfo = field };

                    foreach (var prop in styledDependencyProperties)
                    {
                        var dp                  = (DependencyProperty)prop.FieldInfo.GetValue(null);
                        var dpType              = Type.GetTypeFromHandle(dp.PropertyType);

                        var setStyledValue      = miSetStyledValue.MakeGenericMethod(dpType);
                        
                        var expParameterElement = Expression.Parameter(typeof(UIElement), "element");
                        var expParameterValue   = Expression.Parameter(typeof(String), "value");
                        var expParameterFmtProv = Expression.Parameter(typeof(IFormatProvider), "provider");
                        var expResolveValue     = Expression.Convert(Expression.Call(miFromString, expParameterValue, Expression.Constant(dpType), expParameterFmtProv), dpType);
                        var expCallMethod       = Expression.Call(expParameterElement, setStyledValue, Expression.Constant(dp), expResolveValue);

                        var lambda = Expression.Lambda<StyleSetter>(expCallMethod, expParameterElement, expParameterValue, expParameterFmtProv).Compile();
                        styleSettersForCurrentType[prop.Attribute.Name] = lambda;
                    }

                    styleSetters[typeID] = styleSettersForCurrentType;
                }
            }
        }

        /// <summary>
        /// Gets the style setter for the style with the specified name.
        /// </summary>
        /// <param name="name">The name of the style for which to retrieve a setter.</param>
        /// <returns>A function to set the value of the specified style.</returns>
        private StyleSetter GetStyleSetter(String name)
        {
            var typeID = GetType().TypeHandle.Value.ToInt64();

            lock (styleSetters)
            {
                Dictionary<String, StyleSetter> styleSettersForCurrentType;
                if (!styleSetters.TryGetValue(typeID, out styleSettersForCurrentType))
                    return null;

                StyleSetter setter;
                styleSettersForCurrentType.TryGetValue(name, out setter);
                return setter;
            }
        }

        // Dependency properties.
        private static readonly DependencyProperty dpIsEnabled = DependencyProperty.Register("IsEnabled", typeof(Boolean), typeof(UIElement));
        private static readonly DependencyProperty dpIsVisible = DependencyProperty.Register("IsVisible", typeof(Boolean), typeof(UIElement));

        // Property values.
        private readonly UltravioletContext uv;
        private readonly String id;
        private readonly String name;
        private UIViewport viewport;
        private UIContainer container;
        private Int32 containerRelativeX;
        private Int32 containerRelativeY;
        private Int32 calculatedWidth;
        private Int32 calculatedHeight;

        // Functions for setting styles on known element types.
        private static readonly MethodInfo miFromString;
        private static readonly MethodInfo miSetStyledValue;
        private static readonly Dictionary<Int64, Dictionary<String, StyleSetter>> styleSetters = 
            new Dictionary<Int64, Dictionary<String, StyleSetter>>();
    }
}
