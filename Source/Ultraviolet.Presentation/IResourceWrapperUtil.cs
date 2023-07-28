using System;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Contains utility methods relating to the <see cref="IResourceWrapper"/> interface.
    /// </summary>
    internal static class IResourceWrapperUtil
    {
        /// <summary>
        /// Retrieves the wrapped resource from the specified resource wrapper.
        /// </summary>
        public static Object GetResourceFromResourceWrapper<T>(T wrapper) where T : IResourceWrapper => wrapper.Resource;
    }
}
