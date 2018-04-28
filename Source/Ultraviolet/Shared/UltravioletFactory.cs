using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Represents an Ultraviolet context's object factory.
    /// </summary>
    public sealed class UltravioletFactory
    {
        /// <summary>
        /// Attempts to retrieve the default factory method of the specified delegate type.
        /// </summary>
        /// <typeparam name="T">The delegate type of the factory method to retrieve.</typeparam>
        /// <returns>The default factory method of the specified type, or <see langword="null"/> if no such factory method is registered..</returns>
        public T TryGetFactoryMethod<T>() where T : class
        {
            var value = default(Delegate);
            defaultFactoryMethods.TryGetValue(typeof(T), out value);

            if (value == null)
                return null;

            var typed = value as T;
            if (typed == null)
                throw new InvalidCastException();

            return typed;
        }

        /// <summary>
        /// Attempts to retrieve a named factory method of the specified delegate type.
        /// </summary>
        /// <typeparam name="T">The delegate type of the factory method to retrieve.</typeparam>
        /// <param name="name">The name of the factory method to retrieve.</param>
        /// <returns>The specified named factory method, or <see langword="null"/> if no such factory method is registered.</returns>
        public T TryGetFactoryMethod<T>(String name) where T : class
        {
            Contract.RequireNotEmpty(name, nameof(name));

            var registry = default(Dictionary<String, Delegate>);
            if (!namedFactoryMethods.TryGetValue(typeof(T), out registry))
                return null;

            var value = default(Delegate);
            registry.TryGetValue(name, out value);

            if (value == null)
                return null;

            var typed = value as T;
            if (typed == null)
                throw new InvalidCastException();

            return typed;
        }

        /// <summary>
        /// Gets the default factory method of the specified delegate type.
        /// </summary>
        /// <typeparam name="T">The delegate type of the factory method to retrieve.</typeparam>
        /// <returns>The default factory method of the specified type.</returns>
        public T GetFactoryMethod<T>() where T : class
        {
            var value = default(Delegate);
            defaultFactoryMethods.TryGetValue(typeof(T), out value);

            if (value == null)
                throw new InvalidOperationException(UltravioletStrings.MissingFactoryMethod.Format(typeof(T).FullName));

            var typed = value as T;
            if (typed == null)
                throw new InvalidCastException();

            return typed;
        }

        /// <summary>
        /// Gets a named factory method of the specified delegate type.
        /// </summary>
        /// <typeparam name="T">The delegate type of the factory method to retrieve.</typeparam>
        /// <param name="name">The name of the factory method to retrieve.</param>
        /// <returns>The specified named factory method.</returns>
        public T GetFactoryMethod<T>(String name) where T : class
        {
            Contract.RequireNotEmpty(name, nameof(name));
            
            var registry = default(Dictionary<String, Delegate>);
            if (!namedFactoryMethods.TryGetValue(typeof(T), out registry))
                throw new InvalidOperationException(UltravioletStrings.NoNamedFactoryMethods.Format(typeof(T).FullName));
            
            var value = default(Delegate);
            registry.TryGetValue(name, out value);

            if (value == null)
                throw new InvalidOperationException(UltravioletStrings.MissingNamedFactoryMethod.Format(name));
            
            var typed = value as T;
            if (typed == null)
                throw new InvalidCastException();

            return typed;
        }

        /// <summary>
        /// Registers the default factory method of the specified delegate type.
        /// </summary>
        /// <typeparam name="T">The delegate type of the factory method to register.</typeparam>
        /// <param name="factory">A delegate representing the factory method to register.</param>
        public void SetFactoryMethod<T>(T factory) where T : class
        {
            Contract.Require(factory, nameof(factory));

            var key = typeof(T);
            var del = factory as Delegate;
            if (del == null)
                throw new InvalidOperationException(UltravioletStrings.FactoryMethodInvalidDelegate);
            
            if (defaultFactoryMethods.ContainsKey(key))
                throw new InvalidOperationException(UltravioletStrings.FactoryMethodAlreadyRegistered);
            
            defaultFactoryMethods[key] = del;
        }

        /// <summary>
        /// Registers a named factory method of the specified delegate type.
        /// </summary>
        /// <typeparam name="T">The delegate type of the factory method to register.</typeparam>
        /// <param name="name">The name of the factory method to register.</param>
        /// <param name="factory">A delegate representing the factory method to register.</param>
        public void SetFactoryMethod<T>(String name, T factory) where T : class
        {
            Contract.RequireNotEmpty(name, nameof(name));
            Contract.Require(factory, nameof(factory));

            var key = typeof(T);
            var registry = default(Dictionary<String, Delegate>);
            if (!namedFactoryMethods.TryGetValue(key, out registry))
                namedFactoryMethods[key] = registry = new Dictionary<String, Delegate>();
            
            var del = factory as Delegate;
            if (del == null)
                throw new InvalidOperationException(UltravioletStrings.FactoryMethodInvalidDelegate);
            
            if (registry.ContainsKey(name))
                throw new InvalidOperationException(UltravioletStrings.NamedFactoryMethodAlreadyRegistered);

            registry[name] = del;
        }

        /// <summary>
        /// Unregisters the default factory method of the specified delegate type.
        /// </summary>
        /// <typeparam name="T">The delegate type of the factory method to remove.</typeparam>
        /// <returns><see langword="true"/> if the factory method was unregistered; otherwise, <see langword="false"/>.</returns>
        public Boolean RemoveFactoryMethod<T>() where T : class
        {
            return defaultFactoryMethods.Remove(typeof(T));
        }

        /// <summary>
        /// Unregisters a named factory method of the specified delegate type.
        /// </summary>
        /// <typeparam name="T">The delegate type of the factory method to remove.</typeparam>
        /// <param name="name">The name of the factory method to unregister.</param>
        /// <returns><see langword="true"/> if the factory method was unregistered; otherwise, <see langword="false"/>.</returns>
        public Boolean RemoveFactoryMethod<T>(String name) where T : class
        {
            Contract.RequireNotEmpty(name, nameof(name));

            var key = typeof(T);
            var registry = default(Dictionary<String, Delegate>);
            if (!namedFactoryMethods.TryGetValue(key, out registry))
                return false;

            return registry.Remove(name);
        }

        // The factory method registry.
        private readonly Dictionary<Type, Delegate> defaultFactoryMethods = 
            new Dictionary<Type, Delegate>();
        private readonly Dictionary<Type, Dictionary<String, Delegate>> namedFactoryMethods = 
            new Dictionary<Type, Dictionary<String, Delegate>>();
    }
}
