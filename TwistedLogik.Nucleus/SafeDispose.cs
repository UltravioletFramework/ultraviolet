using System;

namespace TwistedLogik.Nucleus
{
    /// <summary>
    /// Contains methods for safely disposing of an object.
    /// </summary>
    public static class SafeDispose
    {
        /// <summary>
        /// Safely disposes the specified object, if it implements IDisposable.
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
        /// Safely disposes the specified object, if it implements IDisposable, and sets the passed reference to null.
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
        /// <param name="obj">The object to dispose.</param>
        public static void Dispose<T>(T obj) where T : class, IDisposable
        {
            if (obj != null)
            {
                obj.Dispose();
            }
        }

        /// <summary>
        /// Safely disposes the specified object and sets the passed reference to null.
        /// </summary>
        /// <param name="obj">The object to dispose.</param>
        /// <returns>A null reference.</returns>
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
