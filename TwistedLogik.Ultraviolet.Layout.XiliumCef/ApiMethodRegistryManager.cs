using System;
using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.Layout.XiliumCef
{
    /// <summary>
    /// Manages the API registries for all extant layout instances.
    /// </summary>
    internal static class ApiMethodRegistryManager
    {
        /// <summary>
        /// Gets the API registry for the browser instance with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the browser for which to retrieve an API registry.</param>
        /// <returns>The API registry for the specified browser, or null if no registry has been created.</returns>
        public static ApiMethodRegistry GetByBrowserID(Int32 id)
        {
            lock (registries)
            {
                ApiMethodRegistry registry;
                registries.TryGetValue(id, out registry);
                return registry;
            }
        }

        /// <summary>
        /// Creates an API registry for the specified browser.
        /// </summary>
        /// <param name="id">The unique identifier of the browser for which to create an API registry.</param>
        public static void Create(Int32 id)
        {
            lock (registries)
            {
                if (registries.ContainsKey(id))
                {
                    throw new InvalidOperationException(XiliumStrings.ApiRegistryAlreadyCreated);
                }
                registries[id] = new ApiMethodRegistry();
            }
        }

        /// <summary>
        /// Destroys the API registry belonging to the specified browser.
        /// </summary>
        /// <param name="id">The unique identifier of the browser that owns the API registry to destroy.</param>
        public static void Destroy(Int32 id)
        {
            lock (registries)
            {
                if (!registries.ContainsKey(id))
                {
                    throw new InvalidOperationException(XiliumStrings.ApiRegistryDoesNotExist);
                }
                registries.Remove(id);
            }
        }

        // The API registries for the active layouts.
        private static readonly Dictionary<Int32, ApiMethodRegistry> registries = 
            new Dictionary<Int32, ApiMethodRegistry>();
    }
}
