using System;

namespace Ultraviolet.Core
{
    /// <summary>
    /// Prevents the Xamarin linker from linking the target.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Assembly |
        AttributeTargets.Class | 
        AttributeTargets.Struct | 
        AttributeTargets.Enum | 
        AttributeTargets.Constructor | 
        AttributeTargets.Method |
        AttributeTargets.Property | 
        AttributeTargets.Field | 
        AttributeTargets.Event | 
        AttributeTargets.Interface | 
        AttributeTargets.Delegate, AllowMultiple = true)]
    public sealed class PreserveAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PreserveAttribute"/> class.
        /// </summary>
        public PreserveAttribute() { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="PreserveAttribute"/> class.
        /// </summary>
        public PreserveAttribute(Type type) { }

        /// <summary>
        /// Ensures that all members of this type are preserved.
        /// </summary>
        public Boolean AllMembers;

        /// <summary>
        /// Flags the method as a method to preserve during linking if the container class is pulled in.
        /// </summary>
        public Boolean Conditional;
    }
}
