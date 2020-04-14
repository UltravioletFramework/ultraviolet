using System;
using System.ComponentModel;
using System.Linq;

namespace Ultraviolet.Core.Data
{
    /// <summary>
    /// Represents a type converter which produces a list of standard values corresponding to
    /// the contents of the specified data object registry.
    /// </summary>
    [CLSCompliant(false)]
    public sealed class DataObjectTypeConverter<T> : ObjectResolverTypeConverter<T> where T : DataObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataObjectTypeConverter{T}"/> class.
        /// </summary>
        public DataObjectTypeConverter()
        {
            values = GetStandardValuesForRegistry();
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
        /// Gets a <see cref="System.ComponentModel.TypeConverter.StandardValuesCollection"/> containing the object references defined by the specified data object registry.
        /// </summary>
        /// <returns>The <see cref="System.ComponentModel.TypeConverter.StandardValuesCollection"/> which was created.</returns>
        private static StandardValuesCollection GetStandardValuesForRegistry()
        {
            var registry = DataObjectRegistries.Get<T>();
            var keys = registry.Select(x => String.Format("@{0}:{1}", registry.ReferenceResolutionName, x.Key));
            var references = (from k in keys
                              select (ResolvedDataObjectReference)ObjectResolver.FromString(k, typeof(ResolvedDataObjectReference))).ToList();

            references.Insert(0, ResolvedDataObjectReference.Invalid);

            return new StandardValuesCollection(references);
        }

        // The standard values for this converter.
        private StandardValuesCollection values;
    }
}
