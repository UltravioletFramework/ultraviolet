using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Content
{
    partial class ContentProcessorRegistry
    {
        /// <summary>
        /// Represents a content processor's key within the registry.
        /// </summary>
        private struct RegistryKey : IEquatable<RegistryKey>
        {
            /// <summary>
            /// Initializes a new instance of the ContentProcessorKey structure.
            /// </summary>
            /// <param name="input">The input type.</param>
            /// <param name="output">The output type.</param>
            public RegistryKey(Type input, Type output)
            {
                Contract.Require(input, "input");
                Contract.Require(output, "output");

                this.Input = input;
                this.Output = output;
            }

            /// <summary>
            /// Checks two objects for equality.
            /// </summary>
            /// <param name="other">The object to compare against this object for equality.</param>
            /// <returns>true if this object is equal to the specified object; otherwise, false.</returns>
            public Boolean Equals(RegistryKey other)
            {
                return Input == other.Input && Output == other.Output;
            }

            /// <summary>
            /// The input type.
            /// </summary>
            public readonly Type Input;

            /// <summary>
            /// The output type.
            /// </summary>
            public readonly Type Output;
        }
    }
}
