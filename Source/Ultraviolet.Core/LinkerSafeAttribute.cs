using System;

namespace Ultraviolet.Core
{
    /// <summary>
    /// Indicates that the tagged assembly can be safely linked by the Xamarin linker.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class LinkerSafeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkerSafeAttribute"/> class.
        /// </summary>
        public LinkerSafeAttribute() { }
    }
}
