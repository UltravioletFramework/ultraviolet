using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using Ultraviolet.Core;
using Ultraviolet.Presentation.Styles;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents a method which sets the value of a styled property on a dependency object.
    /// </summary>
    /// <param name="dobj">The dependency object on which to set the style.</param>
    /// <param name="style">The style to set on this dependency property.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    public delegate void DependencyPropertyStyleSetter(DependencyObject dobj, UvssRule style, IFormatProvider provider);

    /// <summary>
    /// Represents a dependency property.
    /// </summary>
    public partial class DependencyProperty
    {
        /// <summary>
        /// Initializes the <see cref="DependencyProperty"/> type.
        /// </summary>
        static DependencyProperty()
        {
            var dobjMethods = typeof(DependencyObject).GetMethods(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            foreach (var dobjMethod in dobjMethods)
            {
                if (miResolveStyledValue != null && miSetStyledValue != null)
                    break;

                if (miResolveStyledValue == null && String.Equals(dobjMethod.Name, "ResolveStyledValue", StringComparison.Ordinal))
                {
                    miResolveStyledValue = dobjMethod;
                    continue;
                }

                if (miSetStyledValue == null && string.Equals(dobjMethod.Name, "SetStyledValue", StringComparison.Ordinal))
                {
                    if (dobjMethod.GetParameters()[0].ParameterType == typeof(DependencyProperty))
                    {
                        miSetStyledValue = dobjMethod;
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyProperty"/> class.
        /// </summary>
        /// <param name="id">The dependency property's unique identifier.</param>
        /// <param name="name">The dependency property's name.</param>
        /// <param name="uvssName">The dependency property's name within the UVSS styling system.</param>
        /// <param name="propertyType">The dependency property's value type.</param>
        /// <param name="ownerType">The dependency property's owner type.</param>
        /// <param name="metadata">The dependency property's metadata.</param>
        /// <param name="isReadOnly">A value indicating whether this is a read-only dependency property.</param>
        /// <param name="isAttached">A value indicating whether this is an attached property.</param>
        internal DependencyProperty(Int64 id, String name, String uvssName, Type propertyType, Type ownerType, PropertyMetadata metadata, Boolean isReadOnly = false, Boolean isAttached = false)
        {
            this.id              = id;
            this.name            = name;
            this.uvssName        = uvssName ?? UvssNameGenerator.GenerateUvssName(name);
            this.propertyType    = propertyType;
            this.underlyingType  = Nullable.GetUnderlyingType(propertyType);
            this.ownerType       = ownerType;
            this.defaultMetadata = metadata ?? (PropertyMetadata)typeof(PropertyMetadata<>).MakeGenericType(propertyType).GetField("Empty").GetValue(null);
            this.isReadOnly      = isReadOnly;
            this.isAttached      = isAttached;
            this.styleSetter     = CreateStyleSetter();

            this.changeNotificationServer = new DependencyPropertyChangeNotificationServer(this);
        }

        /// <summary>
        /// Registers a new dependency property.
        /// </summary>
        /// <param name="name">The dependency property's name.</param>
        /// <param name="propertyType">The dependency property's value type.</param>
        /// <param name="ownerType">The dependency property's owner type.</param>
        /// <param name="metadata">The dependency property's metadata.</param>
        /// <returns>A <see cref="DependencyProperty"/> instance which represents the registered dependency property.</returns>
        public static DependencyProperty Register(String name, Type propertyType, Type ownerType, PropertyMetadata metadata = null)
        {
            return DependencyPropertySystem.Register(name, null, propertyType, ownerType, metadata);
        }

        /// <summary>
        /// Registers a new dependency property.
        /// </summary>
        /// <param name="name">The dependency property's name.</param>
        /// <param name="uvssName">The dependency property's name within the UVSS styling system.</param>
        /// <param name="propertyType">The dependency property's value type.</param>
        /// <param name="ownerType">The dependency property's owner type.</param>
        /// <param name="metadata">The dependency property's metadata.</param>
        /// <returns>A <see cref="DependencyProperty"/> instance which represents the registered dependency property.</returns>
        public static DependencyProperty Register(String name, String uvssName, Type propertyType, Type ownerType, PropertyMetadata metadata = null)
        {
            return DependencyPropertySystem.Register(name, uvssName, propertyType, ownerType, metadata);
        }

        /// <summary>
        /// Registers a new read-only dependency property.
        /// </summary>
        /// <param name="name">The dependency property's name.</param>
        /// <param name="propertyType">The dependency property's value type.</param>
        /// <param name="ownerType">The dependency property's owner type.</param>
        /// <param name="metadata">The dependency property's metadata.</param>
        /// <returns>A <see cref="DependencyPropertyKey"/> instance which provides access to the read-only dependency property.</returns>
        public static DependencyPropertyKey RegisterReadOnly(String name, Type propertyType, Type ownerType, PropertyMetadata metadata = null)
        {
            return DependencyPropertySystem.RegisterReadOnly(name, null, propertyType, ownerType, metadata);
        }

        /// <summary>
        /// Registers a new read-only dependency property.
        /// </summary>
        /// <param name="name">The dependency property's name.</param>
        /// <param name="uvssName">The dependency property's name within the UVSS styling system.</param>
        /// <param name="propertyType">The dependency property's value type.</param>
        /// <param name="ownerType">The dependency property's owner type.</param>
        /// <param name="metadata">The dependency property's metadata.</param>
        /// <returns>A <see cref="DependencyPropertyKey"/> instance which provides access to the read-only dependency property.</returns>
        public static DependencyPropertyKey RegisterReadOnly(String name, String uvssName, Type propertyType, Type ownerType, PropertyMetadata metadata = null)
        {
            return DependencyPropertySystem.RegisterReadOnly(name, uvssName, propertyType, ownerType, metadata);
        }

        /// <summary>
        /// Registers a new attached property.
        /// </summary>
        /// <param name="name">The attached property's name.</param>
        /// <param name="propertyType">The attached property's value type.</param>
        /// <param name="ownerType">The attached property's owner type.</param>
        /// <param name="metadata">The attached property's metadata.</param>
        /// <returns>A <see cref="DependencyProperty"/> instance which represents the registered attached property.</returns>
        public static DependencyProperty RegisterAttached(String name, Type propertyType, Type ownerType, PropertyMetadata metadata = null)
        {
            return DependencyPropertySystem.RegisterAttached(name, null, propertyType, ownerType, metadata);
        }

        /// <summary>
        /// Registers a new attached property.
        /// </summary>
        /// <param name="name">The attached property's name.</param>
        /// <param name="uvssName">The attached property's name within the UVSS styling system.</param>
        /// <param name="propertyType">The attached property's value type.</param>
        /// <param name="ownerType">The attached property's owner type.</param>
        /// <param name="metadata">The attached property's metadata.</param>
        /// <returns>A <see cref="DependencyProperty"/> instance which represents the registered attached property.</returns>
        public static DependencyProperty RegisterAttached(String name, String uvssName, Type propertyType, Type ownerType, PropertyMetadata metadata = null)
        {
            return DependencyPropertySystem.RegisterAttached(name, uvssName, propertyType, ownerType, metadata);
        }

        /// <summary>
        /// Registers a new read-only attached property.
        /// </summary>
        /// <param name="name">The attached property's name.</param>
        /// <param name="propertyType">The attached property's value type.</param>
        /// <param name="ownerType">The attached property's owner type.</param>
        /// <param name="metadata">The attached property's metadata.</param>
        /// <returns>A <see cref="DependencyPropertyKey"/> instance which provides access to the read-only attached property.</returns>
        public static DependencyPropertyKey RegisterAttachedReadOnly(String name, Type propertyType, Type ownerType, PropertyMetadata metadata = null)
        {
            return DependencyPropertySystem.RegisterAttachedReadOnly(name, null, propertyType, ownerType, metadata);
        }

        /// <summary>
        /// Registers a new read-only attached property.
        /// </summary>
        /// <param name="name">The attached property's name.</param>
        /// <param name="uvssName">The attached property's name within the UVSS styling system.</param>
        /// <param name="propertyType">The attached property's value type.</param>
        /// <param name="ownerType">The attached property's owner type.</param>
        /// <param name="metadata">The attached property's metadata.</param>
        /// <returns>A <see cref="DependencyPropertyKey"/> instance which provides access to the read-only attached property.</returns>
        public static DependencyPropertyKey RegisterAttachedReadOnly(String name, String uvssName, Type propertyType, Type ownerType, PropertyMetadata metadata = null)
        {
            return DependencyPropertySystem.RegisterAttachedReadOnly(name, uvssName, propertyType, ownerType, metadata);
        }

        /// <summary>
        /// Finds the dependency property with the specified name.
        /// </summary>
        /// <param name="name">The name of the dependency property for which to search.</param>
        /// <param name="ownerType">The dependency property's owner type.</param>
        /// <returns>A <see cref="DependencyProperty"/> instance which represents the specified dependency property, 
        /// or <see langword="null"/> if no such dependency property exists.</returns>
        public static DependencyProperty FindByName(String name, Type ownerType)
        {
            return DependencyPropertySystem.FindByName(name, ownerType);
        }

        /// <summary>
        /// Finds the dependency property with the specified name.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="dobj">The dependency object which is searching for a dependency property.</param>
        /// <param name="owner">The name of the dependency property's containing type.</param>
        /// <param name="name">The name of the dependency property.</param>
        /// <returns>A <see cref="DependencyProperty"/> instance which represents the specified dependency property, 
        /// or <see langword="null"/> if no such dependency property exists.</returns>
        public static DependencyProperty FindByName(UltravioletContext uv, DependencyObject dobj, String owner, String name)
        {
            Contract.Require(uv, nameof(uv));
            Contract.Require(dobj, nameof(dobj));
            Contract.RequireNotEmpty(name, nameof(name));

            var type = String.IsNullOrEmpty(owner) ? dobj.GetType() : null;
            if (type == null)
            {
                if (!uv.GetUI().GetPresentationFoundation().GetKnownType(owner, false, out type))
                    throw new InvalidOperationException(PresentationStrings.UnrecognizedType.Format(owner));
            }

            return FindByName(name, type);
        }

        /// <summary>
        /// Finds the dependency property with the specified styling name.
        /// </summary>
        /// <param name="name">The styling name of the dependency property for which to search.</param>
        /// <param name="ownerType">The dependency property's owner type.</param>
        /// <returns>A <see cref="DependencyProperty"/> instance which represents the specified dependency property, 
        /// or <see langword="null"/> if no such dependency property exists.</returns>
        public static DependencyProperty FindByStylingName(String name, Type ownerType)
        {
            return DependencyPropertySystem.FindByStylingName(name, ownerType);
        }

        /// <summary>
        /// Finds the dependency property with the specified styling name.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="dobj">The dependency object which is searching for a dependency property.</param>
        /// <param name="owner">The name of the dependency property's containing type.</param>
        /// <param name="name">The styling name of the dependency property.</param>
        /// <returns>A <see cref="DependencyProperty"/> instance which represents the specified dependency property, 
        /// or <see langword="null"/> if no such dependency property exists.</returns>
        public static DependencyProperty FindByStylingName(UltravioletContext uv, DependencyObject dobj, String owner, String name)
        {
            Contract.Require(uv, nameof(uv));
            Contract.Require(dobj, nameof(dobj));
            Contract.RequireNotEmpty(name, nameof(name));

            var type = String.IsNullOrEmpty(owner) ? dobj.GetType() : null;
            if (type == null)
            {
                if (!uv.GetUI().GetPresentationFoundation().GetKnownType(owner, false, out type))
                    throw new InvalidOperationException(PresentationStrings.UnrecognizedType.Format(owner));
            }

            return FindByStylingName(name, type);
        }

        /// <summary>
        /// Adds a new owning type to this dependency property.
        /// </summary>
        /// <param name="ownerType">The owner type to add to this dependency property.</param>
        /// <returns>A reference to this dependency property instance.</returns>
        public DependencyProperty AddOwner(Type ownerType)
        {
            return AddOwner(ownerType, null);
        }

        /// <summary>
        /// Adds a new owning type to this dependency property.
        /// </summary>
        /// <param name="ownerType">The owner type to add to this dependency property.</param>
        /// <param name="typeMetadata">The property metadata for this owning type, which will override the default metadata.</param>
        /// <returns>A reference to this dependency property instance.</returns>
        public DependencyProperty AddOwner(Type ownerType, PropertyMetadata typeMetadata)
        {
            Contract.Require(ownerType, nameof(ownerType));

            DependencyPropertySystem.AddOwner(this, ownerType);

            OverrideMetadata(ownerType, typeMetadata);

            return this;
        }

        /// <summary>
        /// Overrides the property's metadata for the specified type.
        /// </summary>
        /// <param name="forType">The type for which to override property metadata.</param>
        /// <param name="typeMetadata">The property metadata for the specified type.</param>
        public void OverrideMetadata(Type forType, PropertyMetadata typeMetadata)
        {
            Contract.Require(ownerType, nameof(ownerType));

            if (metadataOverrides.ContainsKey(forType))
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyAlreadyRegistered);

            var merged = false;

            var currentType = forType.BaseType;
            while (currentType != null)
            {
                PropertyMetadata currentTypeMetadata;
                if (metadataOverrides.TryGetValue(currentType, out currentTypeMetadata))
                {
                    if (typeMetadata == null)
                    {
                        typeMetadata = currentTypeMetadata;
                    }
                    else
                    {
                        typeMetadata.Merge(currentTypeMetadata, this);
                    }
                    merged = true;
                    break;
                }
                currentType = currentType.BaseType;
            }

            if (!merged)
            {
                var baseMetadata = GetMetadataForOwner(ownerType);
                if (typeMetadata == null)
                {
                    typeMetadata = baseMetadata;
                }
                else
                {
                    typeMetadata.Merge(baseMetadata, this);
                }
                merged = true;
            }

            metadataOverrides[forType] = typeMetadata;
        }

        /// <summary>
        /// Registers the specified subscriber to receive change notifications for the specified dependency property.
        /// </summary>
        /// <param name="dobj">The dependency object to monitor for changes.</param>
        /// <param name="dprop">The dependency property for which to receive change notifications.</param>
        /// <param name="subscriber">The subscriber that wishes to receive change notifications for the specified dependency property.</param>
        internal static void RegisterChangeNotification(DependencyObject dobj, DependencyProperty dprop, IDependencyPropertyChangeNotificationSubscriber subscriber)
        {
            Contract.Require(dobj, nameof(dobj));
            Contract.Require(dprop, nameof(dprop));
            Contract.Require(subscriber, nameof(subscriber));

            dprop.changeNotificationServer.Subscribe(dobj, subscriber);
        }

        /// <summary>
        /// Unregisters the specified subscriber from receiving change notifications for the specified dependency property.
        /// </summary>
        /// <param name="dobj">The dependency object to monitor for changes.</param>
        /// <param name="dprop">The dependency property for which to stop receiving change notifications.</param>
        /// <param name="subscriber">The subscriber that wishes to stop receiving change notifications for the specified dependency property.</param>
        internal static void UnregisterChangeNotification(DependencyObject dobj, DependencyProperty dprop, IDependencyPropertyChangeNotificationSubscriber subscriber)
        {
            Contract.Require(dobj, nameof(dobj));
            Contract.Require(dprop, nameof(dprop));
            Contract.Require(subscriber, nameof(subscriber));

            dprop.changeNotificationServer.Unsubscribe(dobj, subscriber);
        }

        /// <summary>
        /// Raises a change notification for this dependency property.
        /// </summary>
        /// <param name="dobj">The object that was changed.</param>
        internal void RaiseChangeNotification(DependencyObject dobj)
        {
            changeNotificationServer.Notify(dobj);
        }

        /// <summary>
        /// Applies the specified style to the dependency property.
        /// </summary>
        /// <param name="dobj">The dependency object on which to set the style.</param>
        /// <param name="style">The style to set on this dependency property.</param>
        internal void ApplyStyle(DependencyObject dobj, UvssRule style)
        {
            if (styleSetter == null)
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyIsReadOnly.Format(Name));

            styleSetter(dobj, style, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Applies the specified style to the dependency property.
        /// </summary>
        /// <param name="dobj">The dependency object on which to set the style.</param>
        /// <param name="style">The style to set on this dependency property.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        internal void ApplyStyle(DependencyObject dobj, UvssRule style, IFormatProvider provider)
        {
            if (styleSetter == null)
                throw new InvalidOperationException(PresentationStrings.DependencyPropertyIsReadOnly.Format(Name));

            styleSetter(dobj, style, provider);
        }

        /// <summary>
        /// Gets the dependency property's metadata for the specified owning type.
        /// </summary>
        /// <param name="type">The owning type for which to retrieve metadata.</param>
        internal PropertyMetadata GetMetadataForOwner(Type type)
        {
            if (metadataOverrides.Count > 0)
            {
                var currentType = type;
                while (currentType != null)
                {
                    PropertyMetadata metadata;
                    if (metadataOverrides.TryGetValue(currentType, out metadata))
                        return metadata;

                    currentType = currentType.BaseType;
                }
            }
            return defaultMetadata;
        }

        /// <summary>
        /// Gets a value indicating whether the specified type (or one of its ancestors) is one of this property's owner types.
        /// </summary>
        /// <param name="type">The type to evaluate.</param>
        /// <returns><see langword="true"/> if the specified type is an owner type; otherwise, <see langword="false"/>.</returns>
        internal Boolean IsOwner(Type type)
        {
            Contract.Require(type, nameof(type));

            var current = type;
            while (current != null)
            {
                if (ownerType == current || metadataOverrides.ContainsKey(current))
                    return true;

                current = current.BaseType;
            }
            return false;
        }

        /// <summary>
        /// Gets the dependency property's unique identifier.
        /// </summary>
        internal Int64 ID
        {
            get { return id; }
        }

        /// <summary>
        /// Gets the dependency property's name.
        /// </summary>
        internal String Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the dependency property's name within the UVSS styling system.
        /// </summary>
        internal String UvssName
        {
            get { return uvssName; }
        }

        /// <summary>
        /// Gets the dependency property's value type.
        /// </summary>
        internal Type PropertyType
        {
            get { return propertyType; }
        }

        /// <summary>
        /// If the dependency property's type is nullable, this property retrieves its underlying type.
        /// Otherwise, this property returns <see langword="null"/>.
        /// </summary>
        internal Type UnderlyingType
        {
            get { return underlyingType; }
        }

        /// <summary>
        /// Gets the dependency property's owner type.
        /// </summary>
        internal Type OwnerType
        {
            get { return ownerType; }
        }

        /// <summary>
        /// Gets a value indicating whether this is a read-only dependency property.
        /// </summary>
        internal Boolean IsReadOnly
        {
            get { return isReadOnly; }
        }

        /// <summary>
        /// Gets a value indicating whether this dependency property is an attached property.
        /// </summary>
        internal Boolean IsAttached
        {
            get { return isAttached; }
        }

        /// <summary>
        /// Dynamically compiles a collection of lambda methods which can be used to apply styles
        /// to the object's properties.
        /// </summary>
        private DependencyPropertyStyleSetter CreateStyleSetter()
        {
            if (this.IsReadOnly)
                return null;

            var dpType              = this.PropertyType;

            var setStyledValue      = miSetStyledValue.MakeGenericMethod(dpType);

            var expParameterDObj    = Expression.Parameter(typeof(DependencyObject), "dobj");
            var expParameterStyle   = Expression.Parameter(typeof(UvssRule), "style");
            var expParameterFmtProv = Expression.Parameter(typeof(IFormatProvider), "provider");
            var expResolveValue     = Expression.Convert(Expression.Call(miResolveStyledValue, expParameterStyle, Expression.Constant(dpType), expParameterFmtProv), dpType);
            var expCallMethod       = Expression.Call(expParameterDObj, setStyledValue, Expression.Constant(this), expResolveValue);

            return Expression.Lambda<DependencyPropertyStyleSetter>(expCallMethod, expParameterDObj, expParameterStyle, expParameterFmtProv).Compile();
        }

        // Methods on DependencyObject used to style dependency properties.
        private static readonly MethodInfo miResolveStyledValue;
        private static readonly MethodInfo miSetStyledValue;

        // Property values.
        private readonly Int64 id;
        private readonly String name;
        private readonly String uvssName;
        private readonly Type propertyType;
        private readonly Type underlyingType;
        private readonly Type ownerType;
        private readonly PropertyMetadata defaultMetadata;
        private readonly Boolean isReadOnly;
        private readonly Boolean isAttached;
        private readonly Dictionary<Type, PropertyMetadata> metadataOverrides = 
            new Dictionary<Type, PropertyMetadata>();

        // State values.
        private readonly DependencyPropertyStyleSetter styleSetter;
        private readonly DependencyPropertyChangeNotificationServer changeNotificationServer;
    }
}
