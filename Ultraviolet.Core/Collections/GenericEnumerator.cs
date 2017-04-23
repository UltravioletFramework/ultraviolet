using System;
using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Core.Collections
{
    /// <summary>
    /// Represents a method which produces items for a generic enumerator.
    /// Producing a value of <c>default(T)</c> indicates that the enumerator has reached the end of the collection.
    /// </summary>
    /// <typeparam name="T">The type of item contained by the collection being enumerated.</typeparam>
    /// <param name="state">A state object to pass to the generation function.</param>
    /// <param name="index">A value indicating the index of the item that should be generated, respective to its collection.</param>
    /// <param name="result">The item that was generated.</param>
    /// <returns><see langword="true"/> if a valid item was generated; otherwise, <see langword="false"/>.</returns>
    public delegate Boolean GenericEnumeratorGenerator<T>(Object state, Int32 index, out T result);

    /// <summary>
    /// Represents a method which produces a version number associated with
    /// a generically enumerated collection.
    /// </summary>
    /// <param name="state">A state object to pass to the versioning function.</param>
    /// <returns>The current version of the enumerated collection.</returns>
    public delegate Int32 GenericEnumeratorVersionFunction(Object state);

    /// <summary>
    /// Represents a generic list enumerator implemented as a mutable struct.
    /// </summary>
    /// <typeparam name="T">The type of item contained by the collection being enumerated.</typeparam>
    public struct GenericEnumerator<T> : IEnumerator<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericEnumerator{T}"/> structure.
        /// </summary>
        /// <param name="state">A state object to pass to the generation function.</param>
        /// <param name="generator">A function which produces objects from the enumerated collection.</param>
        /// <param name="versioner">A function which produces the collection's version number.</param>
        public GenericEnumerator(Object state, GenericEnumeratorGenerator<T> generator, GenericEnumeratorVersionFunction versioner = null)
        {
            this.state = state;
            this.generator = generator;
            this.versioner = versioner ?? DefaultVersioner;
            this.current = default(T);
            this.index = 0;
            this.version = (versioner == null) ? 0 : versioner(state);
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        public void Dispose()
        {

        }

        /// <summary>
        /// Resets the enumerator to its initial state.
        /// </summary>
        public void Reset()
        {
            if (versioner(state) != version)
            {
                throw new InvalidOperationException(CoreStrings.CollectionChanged);
            }
            index = 0;
            current = default(T);
        }

        /// <summary>
        /// Advances the enumerator to the next item in the collection.
        /// </summary>
        /// <returns><see langword="true"/> if the enumerator advanced to the next item; otherwise, <see langword="false"/>.</returns>
        public Boolean MoveNext()
        {
            if (versioner(state) != version)
            {
                throw new InvalidOperationException(CoreStrings.CollectionChanged);
            }
            if (generator(state, index, out current))
            {
                index++;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the current object.
        /// </summary>
        public T Current
        {
            get { return current; }
        }

        /// <summary>
        /// Gets the current object.
        /// </summary>
        Object IEnumerator.Current
        {
            get { return current; }
        }

        // The default function used to version generic enumerators.  If this is used, versioning is disabled.
        private static readonly GenericEnumeratorVersionFunction DefaultVersioner = new GenericEnumeratorVersionFunction(state => 0);

        // State values.
        private readonly Object state;
        private readonly GenericEnumeratorGenerator<T> generator;
        private readonly GenericEnumeratorVersionFunction versioner;
        private T current;
        private Int32 index;
        private Int32 version;
    }
}
