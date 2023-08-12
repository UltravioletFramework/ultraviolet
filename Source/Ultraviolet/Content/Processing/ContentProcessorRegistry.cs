using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents an Ultraviolet context's registry of content processors.
    /// </summary>
    public sealed partial class ContentProcessorRegistry
    {        
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentProcessorRegistry"/> class.
        /// </summary>
        internal ContentProcessorRegistry()
        {

        }

        /// <summary>
        /// Finds the content processor that takes the specified input type and produces the specified output type.
        /// </summary>
        /// <param name="input">The processor's input type.</param>
        /// <param name="output">The processor's output type.</param>
        /// <returns>The content processor that takes the specified types, or <see langword="null"/> if no such processor exists.</returns>
        public IContentProcessor FindProcessor(Type input, Type output)
        {
            Contract.Require(input, nameof(input));

            if (!registeredProcessors.TryGetValue(new RegistryKey(input, output), out var instance))
            {
                if (fallbackTypes.TryGetValue(output.TypeHandle.Value.ToInt64(), out var fallback))
                {
                    registeredProcessors.TryGetValue(new RegistryKey(input, fallback), out instance);
                }
            }
            return instance;
        }

        /// <summary>
        /// Finds the content processor that takes the specified input type and produces the specified output type.
        /// </summary>
        /// <typeparam name="TInput">The processor's input type.</typeparam>
        /// <typeparam name="TOutput">The processor's output type.</typeparam>
        /// <returns>The content processor that takes the specified types, or <see langword="null"/> if no such processor exists.</returns>
        public IContentProcessor FindProcessor<TInput, TOutput>()
        {
            return FindProcessor(typeof(TInput), typeof(TOutput));
        }

        /// <summary>
        /// Gets the fallback type which has been registered for the specified asset type.
        /// </summary>
        /// <param name="original">The asset type to evaluate.</param>
        /// <returns>The fallback type which has been registered for the specified asset type, 
        /// or <see langword="null"/> if no fallback has been registered.</returns>
        public Type GetFallbackType(Type original)
        {
            Contract.Require(original, nameof(original));

            fallbackTypes.TryGetValue(original.TypeHandle.Value.ToInt64(), out var result);
            return result;
        }

        /// <summary>
        /// Gets the fallback type which has been registered for the specified asset type.
        /// </summary>
        /// <typeparam name="T">The asset type to evaluate.</typeparam>
        /// <returns>The fallback type which has been registered for the specified asset type, 
        /// or <see langword="null"/> if no fallback has been registered.</returns>
        public Type GetFallbackType<T>()
        {
            return GetFallbackType(typeof(T));
        }

        /// <summary>
        /// Sets the fallback type for the specified asset type. If no processor can be found for
        /// a given type, the processor for its fallback type will be returned instead.
        /// </summary>
        /// <param name="original">The type for which to specify a fallback type.</param>
        /// <param name="fallback">The fallback type for the specified type, or <see langword="null"/> to clear the fallback type.</param>
        public void SetFallbackType(Type original, Type fallback)
        {
            Contract.Require(original, nameof(original));

            if (fallback == null)
            {
                fallbackTypes.Remove(original.TypeHandle.Value.ToInt64());
            }
            else
            {
                fallbackTypes[original.TypeHandle.Value.ToInt64()] = fallback;
            }
        }

        /// <summary>
        /// Sets the fallback type for the specified asset type. If no processor can be found for
        /// a given type, the processor for its fallback type will be returned instead.
        /// </summary>
        /// <typeparam name="T">The type for which to register a fallback type.</typeparam>
        /// <param name="fallback">The fallback type for the specified type, or <see langword="null"/> to clear the fallback type.</param>
        public void SetFallbackType<T>(Type fallback)
        {
            SetFallbackType(typeof(T), fallback);
        }

        /// <summary>
        /// Registers a content processor.
        /// </summary>
        /// <typeparam name="T">The type of content processor to register.</typeparam>
        public void RegisterProcessor<T>() where T : IContentProcessor
        {
            var processorType = typeof(T);

            var baseProcessorType = GetBaseContentProcessorType(processorType);
            if (baseProcessorType == null)
                throw new InvalidOperationException(UltravioletStrings.ProcessorInvalidBaseClass.Format(processorType.FullName));

            var args = baseProcessorType.GetGenericArguments();
            var input = args[0];
            var output = args[1];
            var key = new RegistryKey(input, output);

            if (registeredProcessors.ContainsKey(key))
            {
                throw new InvalidOperationException(
                    UltravioletStrings.ProcessorAlreadyRegistered.Format(processorType.FullName, input.FullName, output.FullName));
            }
            registeredProcessors[key] = CreateProcessorInstance(processorType);
        }

        /// <summary>
        /// Unregisters a content processor.
        /// </summary>
        /// <typeparam name="TInput">The input type of the processor to unregister.</typeparam>
        /// <typeparam name="TOutput">The output type of the processor to unregister.</typeparam>
        public void UnregisterProcessor<TInput, TOutput>()
        {
            var key = new RegistryKey(typeof(TInput), typeof(TOutput));

            registeredProcessors.Remove(key);
        }

        /// <summary>
        /// Gets the <see cref="IContentProcessor"/> type from which the specified type is derived.
        /// </summary>
        /// <param name="type">The type to evaluate.</param>
        /// <returns>The <see cref="IContentProcessor"/> type from which the specified type is derived.</returns>
        internal static Type GetBaseContentProcessorType(Type type)
        {
            var current = type;
            while (current != null)
            {
                if (current.IsGenericType && current.GetGenericTypeDefinition() == typeof(ContentProcessor<,>))
                {
                    return current;
                }
                current = current.BaseType;
            }
            return null;
        }

        /// <summary>
        /// Creates an instance of the specified processor type.
        /// </summary>
        /// <param name="type">The type of importer to instantiate.</param>
        /// <returns>The importer instance that was created.</returns>
        private IContentProcessor CreateProcessorInstance(Type type)
        {
            var ctor = type.GetConstructor(Type.EmptyTypes);
            if (ctor == null)
                throw new InvalidOperationException(UltravioletStrings.ProcessorRequiresCtor.Format(type.FullName));

            return (IContentProcessor)ctor.Invoke(null);
        }

        // The content processor registry.
        private readonly Dictionary<RegistryKey, IContentProcessor> registeredProcessors = 
            new Dictionary<RegistryKey, IContentProcessor>();

        // The list of fallback types which have been registered.
        private readonly Dictionary<Int64, Type> fallbackTypes =
            new Dictionary<Int64, Type>();
    }
}
