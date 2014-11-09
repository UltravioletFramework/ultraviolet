using System;
using System.ComponentModel;
using System.Linq;
using TwistedLogik.Nucleus.Data;

namespace TwistedLogik.Nucleus.Design.Data
{
    /// <summary>
    /// Represents a type converter which produces a list of standard values corresponding to
    /// the contents of the specified data object registry.
    /// </summary>
    [CLSCompliant(false)]
    public abstract class DataObjectTypeConverter<T> : ObjectResolverTypeConverter<T> where T : DataObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataObjectTypeConverter{T}"/> class.
        /// </summary>
        /// <param name="includeInvalid">A value indicating whether to include an invalid object reference as a possible selection.</param>
        protected DataObjectTypeConverter(Boolean allowInvalid)
        {
            values = GetStandardValuesForRegistry(allowInvalid);
        }

        /// <inheritdoc/>
        public override Boolean GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <inheritdoc/>
        public override Boolean GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <inheritdoc/>
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return values;
        }

        /// <summary>
        /// Gets a <see cref="StandardValuesCollection"/> containing the object references defined by the specified data object registry.
        /// </summary>
        /// <param name="includeInvalid">A value indicating whether to include an invalid object reference as a possible selection.</param>
        /// <returns>The <see cref="StandardValuesCollection"/> which was created.</returns>
        private static StandardValuesCollection GetStandardValuesForRegistry(Boolean allowInvalid)
        {
            var registry   = DataObjectRegistries.Get<T>();
            var keys       = registry.Select(x => String.Format("@{0}:{1}", registry.ReferenceResolutionName, x.Key));
            var references = (from k in keys 
                              select (ResolvedDataObjectReference)ObjectResolver.FromString(k, typeof(ResolvedDataObjectReference))).ToList();

            if (allowInvalid)
            {
                references.Insert(0, ResolvedDataObjectReference.Invalid);
            }

            return new StandardValuesCollection(references);
        }

        // The standard values for this converter.
        private StandardValuesCollection values;
    }
}
