using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Layout
{
    /// <summary>
    /// Represents an object that participates in the dependency property system.
    /// </summary>
    public class DependencyObject
    {
        /// <summary>
        /// Evaluates whether any of the object's dependency property values have changed and, if so, invokes the appropriate callbacks.
        /// </summary>
        public void Digest()
        {
            foreach (var kvp in dependencyPropertyValues)
            {
                kvp.Value.Digest();
            }
        }

        /// <summary>
        /// Clears the local values on all of the object's dependency properties.
        /// </summary>
        public void ClearLocalValues()
        {
            foreach (var kvp in dependencyPropertyValues)
            {
                kvp.Value.ClearLocalValue();
            }
        }

        /// <summary>
        /// Gets the reference typed value of the specified dependency property.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="property">The name of the dependency property for which to retrieve a value.</param>
        /// <returns>The value of the specified dependency property.</returns>
        public T GetValueRef<T>(String property) where T : class
        {
            Contract.RequireNotEmpty(property, "property");

            var dp = DependencyProperty.FindByName(property, GetType());
            if (dp == null)
            {
                throw new ArgumentException(LayoutStrings.DependencyPropertyDoesNotExist);
            }

            return GetValueRef<T>(dp);
        }

        /// <summary>
        /// Gets the reference typed value of the specified dependency property.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property for which to retrieve a value.</param>
        /// <returns>The value of the specified dependency property.</returns>
        public T GetValueRef<T>(DependencyProperty dp) where T : class
        {
            Contract.Require(dp, "dp");

            if (!typeof(T).TypeHandle.Equals(dp.PropertyType))
                throw new InvalidCastException();

            var wrapper = GetDependencyPropertyValueRef<T>(dp);
            return wrapper.GetValue();
        }

        /// <summary>
        /// Sets the local value of the specified reference typed dependency property.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property for which to set a value.</param>
        /// <param name="value">The value to set on the specified dependency property.</param>
        public void SetValueRef<T>(DependencyProperty dp, T value) where T : class
        {
            Contract.Require(dp, "dp");

            if (!typeof(T).TypeHandle.Equals(dp.PropertyType))
                throw new InvalidCastException();

            var wrapper = GetDependencyPropertyValueRef<T>(dp);
            wrapper.SetValue(value);
        }

        /// <summary>
        /// Clears the value associated with the specified reference typed dependency property.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property to clear.</param>
        public void ClearValueRef<T>(DependencyProperty dp) where T : class
        {
            Contract.Require(dp, "dp");

            IDependencyPropertyValue valueWrapper;
            if (dependencyPropertyValues.TryGetValue(dp.ID, out valueWrapper))
            {
                valueWrapper.ClearLocalValue();
            }
        }

        /// <summary>
        /// Gets the value typed value of the specified dependency property.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="property">The name of the dependency property for which to retrieve a value.</param>
        /// <returns>The value of the specified dependency property.</returns>
        public T GetValue<T>(String property) where T : struct, IEquatable<T>
        {
            Contract.Require(property, "property");

            var dp = DependencyProperty.FindByName(property, GetType());
            if (dp == null)
            {
                throw new ArgumentException(LayoutStrings.DependencyPropertyDoesNotExist);
            }
            
            return GetValue<T>(dp);
        }

        /// <summary>
        /// Gets the value typed value of the specified dependency property.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="dp">The name of the dependency property for which to retrieve a value.</param>
        /// <returns>The value of the specified dependency property.</returns>
        public T GetValue<T>(DependencyProperty dp) where T : struct, IEquatable<T>
        {
            Contract.Require(dp, "dp");

            if (!typeof(T).TypeHandle.Equals(dp.PropertyType))
                throw new InvalidCastException();

            var wrapper = GetDependencyPropertyValue<T>(dp);
            return wrapper.GetValue();
        }

        /// <summary>
        /// Sets the local value of the specified value typed dependency property.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property for which to set a value.</param>
        /// <param name="value">The value to set on the specified dependency property.</param>
        public void SetValue<T>(DependencyProperty dp, T value) where T : struct, IEquatable<T>
        {
            Contract.Require(dp, "dp");

            if (!typeof(T).TypeHandle.Equals(dp.PropertyType))
                throw new InvalidCastException();

            var wrapper = GetDependencyPropertyValue<T>(dp);
            wrapper.SetValue(value);
        }

        /// <summary>
        /// Clears the value associated with the specified value typed dependency property.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property to clear.</param>
        public void ClearValue<T>(DependencyProperty dp) where T : struct, IEquatable<T>
        {
            Contract.Require(dp, "dp");

            if (dependencyPropertyValues.Remove(dp.ID))
            {
                if (dp.Metadata.ChangedCallback != null)
                {
                    dp.Metadata.ChangedCallback(this);
                }
            }
        }

        /// <summary>
        /// Gets the object's containing object.
        /// </summary>
        public DependencyObject DependencyContainer
        {
            get { return dependencyContainer; }
            protected set { dependencyContainer = value; }
        }

        /// <summary>
        /// Gets an instance of <see cref="DependencyPropertyValue{T}"/> for the specified reference typed dependency property.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="dp">The dependency property for which to create a value wrapper.</param>
        /// <returns>The <see cref="DependencyPropertyValue{T}"/> instance which was retrieved.</returns>
        private DependencyPropertyValueRef<T> GetDependencyPropertyValueRef<T>(DependencyProperty dp) where T : class
        {
            IDependencyPropertyValue valueWrapper;
            dependencyPropertyValues.TryGetValue(dp.ID, out valueWrapper);

            var wrapper = (DependencyPropertyValueRef<T>)valueWrapper;
            if (wrapper == null)
            {
                wrapper = new DependencyPropertyValueRef<T>(this, dp);
                if (dp.Metadata.DefaultCallback != null)
                {
                    wrapper.DefaultValue = (T)dp.Metadata.DefaultCallback();
                }
                dependencyPropertyValues[dp.ID] = wrapper;
            }
            return wrapper;
        }

        /// <summary>
        /// Gets an instance of <see cref="DependencyPropertyValue{T}"/> for the specified value typed dependency property.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="dp">The dependency property for which to create a value wrapper.</param>
        /// <returns>The <see cref="DependencyPropertyValue{T}"/> instance which was retrieved.</returns>
        private DependencyPropertyValue<T> GetDependencyPropertyValue<T>(DependencyProperty dp) where T : struct, IEquatable<T>
        {
            IDependencyPropertyValue valueWrapper;
            dependencyPropertyValues.TryGetValue(dp.ID, out valueWrapper);

            var wrapper = (DependencyPropertyValue<T>)valueWrapper;
            if (wrapper == null)
            {
                wrapper = new DependencyPropertyValue<T>(this, dp);
                if (dp.Metadata.DefaultCallback != null)
                {
                    wrapper.DefaultValue = (T)dp.Metadata.DefaultCallback();
                }
                dependencyPropertyValues[dp.ID] = wrapper;
            }
            return wrapper;
        }

        // Property values.
        private DependencyObject dependencyContainer;

        // State values.
        private readonly Dictionary<Int64, IDependencyPropertyValue> dependencyPropertyValues =
            new Dictionary<Int64, IDependencyPropertyValue>();
    }
}
