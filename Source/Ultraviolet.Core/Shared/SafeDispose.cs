using System;

namespace Ultraviolet.Core
{
    /// <summary>
    /// Contains methods for safely disposing of objects without needing to check them for nullity.
    /// </summary>
    public static class SafeDispose
    {
        /// <summary>
        /// Safely disposes the specified object, if it implements <see cref="IDisposable"/>.
        /// </summary>
        /// <param name="obj">The object to dispose.</param>
        public static void Dispose(Object obj)
        {
            var disposable = obj as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        /// <summary>
        /// Safely disposes the specified object, if it implements <see cref="IDisposable"/>, and sets the passed reference to <see langword="null"/>.
        /// </summary>
        /// <param name="obj">The object to dispose.</param>
        public static void DisposeRef(ref Object obj)
        {
            var disposable = obj as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
            obj = null;
        }

        /// <summary>
        /// Safely disposes the specified object.
        /// </summary>
        /// <typeparam name="T">The type of object being disposed.</typeparam>
        /// <param name="obj">The object to dispose.</param>
        public static void Dispose<T>(T obj) where T : class, IDisposable
        {
            if (obj != null)
            {
                obj.Dispose();
            }
        }

        /// <summary>
        /// Safely disposes the specified object and sets the passed reference to <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of object being disposed.</typeparam>
        /// <param name="obj">The object to dispose.</param>
        /// <returns>A <see langword="null"/> reference.</returns>
        public static T DisposeRef<T>(ref T obj) where T : class, IDisposable
        {
            if (obj != null)
            {
                obj.Dispose();
                obj = null;
            }
            return null;
        }
    }
}
