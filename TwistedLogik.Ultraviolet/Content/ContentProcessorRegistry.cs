using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Content
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
        /// Registers any content processors which are defined in the specified assembly.
        /// </summary>
        /// <param name="asm">The assembly that contains the content processors to register.</param>
        public void RegisterAssembly(Assembly asm)
        {
            Contract.Require(asm, "asm");

            var processors = from type in asm.GetTypes()
                             let attrs = type.GetCustomAttributes(typeof(ContentProcessorAttribute), false).Cast<ContentProcessorAttribute>()
                             where attrs != null && attrs.Count() > 0
                             select new { Type = type, Attribute = attrs.First() };

            foreach (var processor in processors)
            {
                var instance = CreateProcessorInstance(processor.Type);

                var baseProcessorType = GetBaseContentProcessorType(processor.Type);
                if (baseProcessorType == null)
                    throw new InvalidOperationException(UltravioletStrings.ProcessorInvalidBaseClass.Format(processor.Type.FullName));
                
                var args = baseProcessorType.GetGenericArguments();
                var input = args[0];
                var output = args[1];

                var key = new RegistryKey(input, output);
                if (registeredProcessors.ContainsKey(key))
                {
                    throw new InvalidOperationException(
                        UltravioletStrings.ProcessorAlreadyRegistered.Format(processor.Type.FullName, input.FullName, output.FullName));
                }
                registeredProcessors[key] = instance;
            }
        }

        /// <summary>
        /// Finds the content processor that takes the specified input type and produces the specified output type.
        /// </summary>
        /// <param name="input">The processor's input type.</param>
        /// <param name="output">The processor's output type.</param>
        /// <returns>The content processor that takes the specified types, or <c>null</c> if no such processor exists.</returns>
        public IContentProcessor FindProcessor(Type input, Type output)
        {
            Contract.Require(input, "input");

            var key = new RegistryKey(input, output);
            var instance = default(IContentProcessor);
            registeredProcessors.TryGetValue(key, out instance);
            return instance;
        }

        /// <summary>
        /// Finds the content processor that takes the specified input type and produces the specified output type.
        /// </summary>
        /// <typeparam name="TInput">The processor's input type.</typeparam>
        /// <typeparam name="TOutput">The processor's output type.</typeparam>
        /// <returns>The content processor that takes the specified types, or <c>null</c> if no such processor exists.</returns>
        public IContentProcessor FindProcessor<TInput, TOutput>()
        {
            return FindProcessor(typeof(TInput), typeof(TOutput));
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
        /// <typeparam name="T">The type of content processor to unregister.</typeparam>
        public void UnregisterProcessor<T>() where T : IContentProcessor
        {
            var baseProcessorType = GetBaseContentProcessorType(typeof(T));
            if (baseProcessorType == null)
                throw new InvalidOperationException(UltravioletStrings.ProcessorInvalidBaseClass.Format(typeof(T).FullName));

            var args = baseProcessorType.GetGenericArguments();
            var key = new RegistryKey(args[0], args[1]);

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
    }
}
