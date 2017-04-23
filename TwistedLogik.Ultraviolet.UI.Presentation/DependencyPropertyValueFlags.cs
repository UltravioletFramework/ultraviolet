using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    partial class DependencyObject
    {
        /// <summary>
        /// Represents the state of a <see cref="DependencyObject.DependencyPropertyValue{T}"/> object.
        /// </summary>
        [Flags]
        internal enum DependencyPropertyValueFlags : byte
        {
            /// <summary>
            /// No flags.
            /// </summary>
            None = 0x00,

            /// <summary>
            /// Indicates that the value encapsulates a reference type.
            /// </summary>
            IsReferenceType = 0x01,

            /// <summary>
            /// Indicates that the value encapsulated a value type.
            /// </summary>
            IsValueType = 0x02,
            
            /// <summary>
            /// Indicates that the value is bound to another value.
            /// </summary>
            IsDataBound = 0x04,

            /// <summary>
            /// Indicates that a change event is going to be raised for the value.
            /// </summary>
            IsPendingChangeEvent = 0x08,

            /// <summary>
            /// Indicates that the value has a local value defined.
            /// </summary>
            HasLocalValue = 0x10,

            /// <summary>
            /// Indicates that the value has a styled value defined.
            /// </summary>
            HasStyledValue = 0x20,

            /// <summary>
            /// Indicates that the value needs to be digested.
            /// </summary>
            RequiresDigest = 0x40,
        }
    }
}