using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TwistedLogik.Nucleus.Data;

namespace TwistedLogik.Nucleus.Design.Data
{
    /// <summary>
    ///  
    /// </summary>
    [CLSCompliant(false)]
    public abstract class DataObjectTypeConverter<T, U> : ObjectResolverTypeConverter<T> where T : DataObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataObjectTypeConverter{T, U}"/> class.
        /// </summary>
        /// <param name="allowNone">A value indicating whether (none) is a valid selection.</param>
        protected DataObjectTypeConverter(Boolean allowNone)
        {
            values = GetStandardValuesForRegistry(allowNone);
        }

        /// <summary>
        /// Gets a value indicating whether this type converter supports standard values.
        /// </summary>
        public override Boolean GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Gets a value indicating whether only standard values are allowed.
        /// </summary>
        public override Boolean GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Gets the type converter's collection of standard values.
        /// </summary>
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return values;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowNone"></param>
        /// <returns></returns>
        private static StandardValuesCollection GetStandardValuesForRegistry(Boolean allowNone)
        {
            var cache = allowNone ? StandardValuesPlusNoneCache : StandardValuesCache;

            StandardValuesCollection values;
            if (!cache.TryGetValue(typeof(T), out values))
            {
                var registry = DataObjectRegistries.Get<T>();
                var keys = registry.Select(x => String.Format("@{0}:{1}", registry.ReferenceResolutionName, x.Key));
                var references = keys.Select(x => (U)ObjectResolver.FromString(x, typeof(U))).ToList();
                var referencesPlusNone = references.ToList();
                referencesPlusNone.Insert(0, default(U));

                StandardValuesCache[typeof(T)] = new StandardValuesCollection(references);
                StandardValuesPlusNoneCache[typeof(T)] = new StandardValuesCollection(referencesPlusNone);

                values = allowNone ? StandardValuesPlusNoneCache[typeof(T)] : StandardValuesCache[typeof(T)];
            }

            return values;
        }

        // The cache of standard values associated with each asset manifest.
        private static readonly Dictionary<Type, StandardValuesCollection> StandardValuesPlusNoneCache =
            new Dictionary<Type, StandardValuesCollection>();
        private static readonly Dictionary<Type, StandardValuesCollection> StandardValuesCache =
            new Dictionary<Type, StandardValuesCollection>();

        // The standard values for this converter.
        private StandardValuesCollection values;
    }
}
