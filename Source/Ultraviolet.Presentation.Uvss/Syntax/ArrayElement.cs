using System;
using System.IO;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Uvss
{
    /// <summary>
    /// Represents an array element.
    /// </summary>
    /// <typeparam name="T">The type of the array element's value.</typeparam>
    public struct ArrayElement<T> where T : SyntaxNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayElement{T}"/> structure
        /// from the specified binary reader.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        public ArrayElement(BinaryReader reader, Int32 version)
        {
            Contract.Require(reader, nameof(reader));

            this.Value = (T)reader.ReadSyntaxNode(version);
        }

        /// <summary>
        /// Serializes the object's instance data to the specified stream.
        /// </summary>
        /// <param name="writer">The binary writer with which to serialize the object.</param>
        /// <param name="version">The file version of the data being written.</param>
        public void Serialize(BinaryWriter writer, Int32 version)
        {
            Contract.Require(writer, nameof(writer));

            writer.Write(this.Value, version);
        }

        /// <summary>
        /// Implicitly converts an array element to its underlying value.
        /// </summary>
        /// <param name="element">The array element to convert.</param>
        /// <returns>The underlying value of the specified array element.</returns>
        public static implicit operator T(ArrayElement<T> element)
        {
            return element.Value;
        }

        /// <summary>
        /// Creates an array of array elements from the specified array of instances.
        /// </summary>
        /// <param name="items">The array of instances.</param>
        /// <returns>The array of array elements.</returns>
        public static ArrayElement<T>[] MakeElementArray(T[] items)
        {
            if (items == null)
                return null;

            var array = new ArrayElement<T>[items.Length];
            for (int i = 0; i < items.Length; i++)
                array[i].Value = items[i];

            return array;
        }

        /// <summary>
        /// Creates an array of instances from the specified array of array elements.
        /// </summary>
        /// <param name="items">The array of array elements.</param>
        /// <returns>The array of instances.</returns>
        public static T[] MakeArray(ArrayElement<T>[] items)
        {
            if (items == null)
                return null;

            var array = new T[items.Length];
            for (int i = 0; i < items.Length; i++)
                array[i] = items[i].Value;

            return array;
        }

        /// <summary>
        /// The array element's value.
        /// </summary>
        public T Value;
    }
}
