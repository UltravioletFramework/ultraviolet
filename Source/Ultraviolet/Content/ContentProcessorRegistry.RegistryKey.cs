using System;
using Ultraviolet.Core;

namespace Ultraviolet.Content
{
    partial class ContentProcessorRegistry
    {
        /// <summary>
        /// Represents a content processor's key within a content processor registry.
        /// </summary>
        private struct RegistryKey : IEquatable<RegistryKey>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="RegistryKey"/> structure.
            /// </summary>
            /// <param name="input">The input type.</param>
            /// <param name="output">The output type.</param>
            public RegistryKey(Type input, Type output)
            {
                Contract.Require(input, nameof(input));
                Contract.Require(output, nameof(output));

                this.Input = input;
                this.Output = output;
            }

            /// <summary>
            /// Checks two objects for equality.
            /// </summary>
            /// <param name="other">The object to compare against this object for equality.</param>
            /// <returns><see langword="true"/> if this object is equal to the specified object; otherwise, <see langword="false"/>.</returns>
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
