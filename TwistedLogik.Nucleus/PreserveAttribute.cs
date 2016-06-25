using System;

namespace TwistedLogik.Nucleus
{
    /// <summary>
    /// Prevents the Xamarin linker from linking the target.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class PreserveAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PreserveAttribute"/> class.
        /// </summary>
        public PreserveAttribute() { }

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
