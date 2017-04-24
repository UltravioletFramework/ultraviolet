using System;
using System.Collections.Generic;
using System.Reflection;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Uvml
{
    /// <summary>
    /// Represents a UVML mutator which sets the items in a collection 
    /// which is represented by a dependency property.
    /// </summary>
    internal sealed class UvmlDependencyPropertyCollectionItemMutator : UvmlCollectionItemMutator
    {
        /// <summary>
        /// Initializes the <see cref="UvmlDependencyPropertyCollectionItemMutator"/> type.
        /// </summary>
        static UvmlDependencyPropertyCollectionItemMutator()
        {
            var dobjMethods = typeof(DependencyObject).GetMethods();

            foreach (var dobjMethod in dobjMethods)
            {
                if (miSetLocalValue != null && miGetValue != null)
                    break;

                if (miSetLocalValue == null && String.Equals(dobjMethod.Name, nameof(DependencyObject.SetLocalValue), StringComparison.Ordinal))
                {
                    if (dobjMethod.GetParameters()[0].ParameterType == typeof(DependencyProperty))
                    {
                        miSetLocalValue = dobjMethod;
                        continue;
                    }
                }

                if (miGetValue == null && String.Equals(dobjMethod.Name, nameof(DependencyObject.GetValue), StringComparison.Ordinal))
                {
                    if (dobjMethod.GetParameters()[0].ParameterType == typeof(DependencyProperty))
                    {
                        miGetValue = dobjMethod;
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UvmlDependencyPropertyCollectionItemMutator"/> class.
        /// </summary>
        /// <param name="dpropID">The property which is being mutated.</param>
        /// <param name="dpropItems">The collection of items to set on the property.</param>
        public UvmlDependencyPropertyCollectionItemMutator(DependencyProperty dpropID, IEnumerable<UvmlNode> dpropItems)
            : base(dpropItems)
        {
            Contract.Require(dpropID, nameof(dpropID));

            this.dpropID = dpropID;
            this.dpropGetter = miGetValue.MakeGenericMethod(dpropID.PropertyType);
            this.dpropSetter = dpropID.IsReadOnly ? null : miSetLocalValue.MakeGenericMethod(dpropID.PropertyType);
        }

        /// <inheritdoc/>
        protected override Boolean CreateCollection(UltravioletContext uv, Object instance, UvmlInstantiationContext context, out Object collection)
        {
            return CreateCollectionOfType(uv, instance, context, dpropID.PropertyType, out collection);
        }

        /// <inheritdoc/>
        protected override Boolean GetCollection(Object instance, out Object collection, out String propname)
        {
            propname = dpropID.Name;
            collection = dpropGetter.Invoke(instance, new[] { dpropID });
            return true;
        }

        /// <inheritdoc/>
        protected override Boolean SetCollection(Object instance, Object collection)
        {
            if (dpropID.IsReadOnly)
                return false;

            dpropSetter.Invoke(instance, new Object[] { dpropID, collection });
            return true;
        }

        // Reflection information for methods on DependencyObject
        private static readonly MethodInfo miSetLocalValue;
        private static readonly MethodInfo miGetValue;

        // State values.
        private readonly DependencyProperty dpropID;
        private readonly MethodInfo dpropGetter;
        private readonly MethodInfo dpropSetter;
    }
}
