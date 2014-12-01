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

            Object value;
            if (!DependencyPropertyValuesRef.TryGetValue(dp.ID, out value))
            {
                if (dp.Metadata.DefaultCallback != null)
                {
                    T @default = (T)dp.Metadata.DefaultCallback();
                    SetValueRef<T>(dp, @default);
                    value = @default;
                }
            }

            return (T)value;
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

            DependencyPropertyValuesRef[dp.ID] = value;
            if (dp.Metadata.ChangedCallback != null)
            {
                dp.Metadata.ChangedCallback(this);
            }
        }

        /// <summary>
        /// Clears the value associated with the specified reference typed dependency property.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property to clear.</param>
        public void ClearValueRef<T>(DependencyProperty dp) where T : class
        {
            Contract.Require(dp, "dp");

            if (DependencyPropertyValuesRef.Remove(dp.ID))
            {
                if (dp.Metadata.ChangedCallback != null)
                {
                    dp.Metadata.ChangedCallback(this);
                }
            }
        }

        /// <summary>
        /// Gets the value typed value of the specified dependency property.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="property">The name of the dependency property for which to retrieve a value.</param>
        /// <returns>The value of the specified dependency property.</returns>
        public T GetValue<T>(String property) where T : struct
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
        public T GetValue<T>(DependencyProperty dp) where T : struct
        {
            Contract.Require(dp, "dp");

            if (!typeof(T).TypeHandle.Equals(dp.PropertyType))
                throw new InvalidCastException();

            var wrapper = GetValueTypeWrapper<T>(dp);
            return wrapper.Value;
        }

        /// <summary>
        /// Sets the local value of the specified value typed dependency property.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property for which to set a value.</param>
        /// <param name="value">The value to set on the specified dependency property.</param>
        public void SetValue<T>(DependencyProperty dp, T value) where T : struct
        {
            Contract.Require(dp, "dp");

            if (!typeof(T).TypeHandle.Equals(dp.PropertyType))
                throw new InvalidCastException();

            var wrapper = GetValueTypeWrapper<T>(dp);
            wrapper.Value = value;
            if (dp.Metadata.ChangedCallback != null)
            {
                dp.Metadata.ChangedCallback(this);
            }
        }

        /// <summary>
        /// Clears the value associated with the specified value typed dependency property.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="dp">A <see cref="DependencyProperty"/> instance which identifies the dependency property to clear.</param>
        public void ClearValue<T>(DependencyProperty dp) where T : struct
        {
            Contract.Require(dp, "dp");

            if (DependencyPropertyValues.Remove(dp.ID))
            {
                if (dp.Metadata.ChangedCallback != null)
                {
                    dp.Metadata.ChangedCallback(this);
                }
            }
        }

        /// <summary>
        /// Gets an instance of <see cref="DependencyPropertyValueWrapper{T}"/> for the specified dependency property.
        /// </summary>
        /// <typeparam name="T">The type of value contained by the dependency property.</typeparam>
        /// <param name="dp">The dependency property for which to create a value wrapper.</param>
        /// <returns>The <see cref="DependencyPropertyValueWrapper{T}"/> instance which was retrieved.</returns>
        private DependencyPropertyValueWrapper<T> GetValueTypeWrapper<T>(DependencyProperty dp) where T : struct
        {
            Object wrapperObject;
            DependencyPropertyValuesRef.TryGetValue(dp.ID, out wrapperObject);

            var wrapper = (DependencyPropertyValueWrapper<T>)wrapperObject;
            if (wrapper == null)
            {
                wrapper = new DependencyPropertyValueWrapper<T>();
                if (dp.Metadata.DefaultCallback != null)
                {
                    wrapper.Value = (T)dp.Metadata.DefaultCallback();
                }
                DependencyPropertyValuesRef[dp.ID] = wrapper;
            }
            return wrapper;
        }

        // The object's reference type dependency property values.
        private Dictionary<Int64, Object> DependencyPropertyValuesRef =
            new Dictionary<Int64, Object>();

        // The object's value type dependency property values.
        private Dictionary<Int64, Object> DependencyPropertyValues =
            new Dictionary<Int64, Object>();
    }
}
