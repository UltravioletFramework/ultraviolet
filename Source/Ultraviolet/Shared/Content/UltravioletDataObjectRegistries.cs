using System;
using System.Reflection;
using Ultraviolet.Core.Data;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Maintains the application's collection of data object registries.
    /// </summary>
    [CLSCompliant(false)]
    public static class UltravioletDataObjectRegistries
    {
        /// <summary>
        /// Gets the data object registry for the specified object type.
        /// </summary>
        /// <typeparam name="T">The type of object for which to retrieve a registry.</typeparam>
        /// <returns>The <see cref="UltravioletDataObjectRegistry{T}"/> which contains objects of type <typeparamref name="T"/>.</returns>
        public static UltravioletDataObjectRegistry<T> Get<T>() where T : UltravioletDataObject =>
            (UltravioletDataObjectRegistry<T>)DataObjectRegistries.Get<T>();

        /// <summary>
        /// Registers the data object registries in the specified assembly.
        /// </summary>
        /// <param name="asm">The assembly that contains the registries to register.</param>
        public static void Register(Assembly asm) =>
            DataObjectRegistries.Register(asm);

        /// <summary>
        /// Removes the data object registry for the specified type from the collection of registries.
        /// </summary>
        /// <typeparam name="T">The type of data object for which to unregister a registry.</typeparam>
        public static void Unregister<T>() where T : UltravioletDataObject =>
            DataObjectRegistries.Unregister<T>();

        /// <summary>
        /// Unregisters all object registries and completely resets the registry manager.
        /// </summary>
        public static void Reset() =>
            DataObjectRegistries.Reset();

        /// <summary>
        /// Removes all objects from all of the application's data object registries 
        /// and returns them to their default states.
        /// </summary>
        public static void Clear() =>
            DataObjectRegistries.Clear();

        /// <summary>
        /// Loads all of the application's registered data object registries.
        /// </summary>
        public static void Load() =>
            DataObjectRegistries.Load();

        /// <summary>
        /// Loads object keys for all of the application's registered data object registries.
        /// </summary>
        public static void LoadKeys() =>
            DataObjectRegistries.LoadKeys();

        /// <summary>
        /// Loads objects for all of the application's registered data object registries.
        /// </summary>
        public static void LoadObjects() =>
            DataObjectRegistries.LoadObjects();

        /// <summary>
        /// Resolves an object reference. If the reference cannot be resolved, an exception is thrown.
        /// </summary>
        /// <param name="reference">The object reference to resolve.</param>
        /// <returns>The resolved object reference.</returns>
        public static ResolvedDataObjectReference ResolveReference(String reference) =>
            DataObjectRegistries.ResolveReference(reference);        
    }
}
