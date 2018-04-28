using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Core
{
    /// <summary>
    /// Contains methods for enforcing code contracts and establishing invariants.
    /// </summary>
    public static class Contract
    {
        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if the specified condition is false.
        /// </summary>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="message">An optional message to pass to the thrown exception.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EnsureRange(Boolean condition, String message)
        {
            if (!condition)
                throw new ArgumentOutOfRangeException(message);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if the specified condition is false.
        /// </summary>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="message">An optional message to pass to the thrown exception.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EnsureRange(Boolean condition, StringResource message)
        {
            if (!condition)
                throw new ArgumentOutOfRangeException(message);
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if the specified condition is false.
        /// </summary>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="message">An optional message to pass to the thrown exception.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Ensure(Boolean condition, String message)
        {
            if (!condition)
            {
                if (message != null)
                {
                    throw new InvalidOperationException(message);
                }
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if the specified condition is false.
        /// </summary>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="message">An optional message to pass to the thrown exception.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Ensure(Boolean condition, StringResource message)
        {
            if (!condition)
            {
                if (message != null)
                {
                    throw new InvalidOperationException(message);
                }
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Throws an exception if the specified condition is false.
        /// </summary>
        /// <typeparam name="T">The type of exception to throw.</typeparam>
        /// <param name="condition">The condition to evaluate.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Ensure<T>(Boolean condition) where T : Exception, new()
        {
            if (!condition)
            {
                throw new T();
            }
        }

        /// <summary>
        /// Throws an exception if the specified condition is false.
        /// </summary>
        /// <typeparam name="T">The type of exception to throw.</typeparam>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="message">An optional message to pass to the thrown exception.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Ensure<T>(Boolean condition, String message) where T : Exception, new()
        {
            if (!condition)
            {
                if (message != null)
                {
                    throw CreateExceptionWithMessage<T>(message);
                }
                throw new T();
            }
        }

        /// <summary>
        /// Throws an exception if the specified condition is false.
        /// </summary>
        /// <typeparam name="T">The type of exception to throw.</typeparam>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="message">An optional message to pass to the thrown exception.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Ensure<T>(Boolean condition, StringResource message) where T : Exception, new()
        {
            if (!condition)
            {
                if (message != null)
                {
                    throw CreateExceptionWithMessage<T>(message);
                }
                throw new T();
            }
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if the specified condition is true.
        /// </summary>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="message">An optional message to pass to the thrown exception.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EnsureNot(Boolean condition, String message)
        {
            if (condition)
            {
                if (message != null)
                {
                    throw new InvalidOperationException(message);
                }
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if the specified condition is true.
        /// </summary>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="message">An optional message to pass to the thrown exception.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EnsureNot(Boolean condition, StringResource message)
        {
            if (condition)
            {
                if (message != null)
                {
                    throw new InvalidOperationException(message);
                }
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if the specified condition is true.
        /// </summary>
        /// <typeparam name="T">The type of exception to throw.</typeparam>
        /// <param name="condition">The condition to evaluate.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EnsureNot<T>(Boolean condition) where T : Exception, new()
        {
            if (condition)
            {
                throw new T();
            }
        }

        /// <summary>
        /// Throws an exception if the specified condition is true.
        /// </summary>
        /// <typeparam name="T">The type of exception to throw.</typeparam>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="message">An optional message to pass to the thrown exception.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EnsureNot<T>(Boolean condition, String message) where T : Exception, new()
        {
            if (condition)
            {
                if (message != null)
                {
                    throw CreateExceptionWithMessage<T>(message);
                }
                throw new T();
            }
        }

        /// <summary>
        /// Throws an exception if the specified condition is true.
        /// </summary>
        /// <typeparam name="T">The type of exception to throw.</typeparam>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="message">An optional message to pass to the thrown exception.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EnsureNot<T>(Boolean condition, StringResource message) where T : Exception, new()
        {
            if (condition)
            {
                if (message != null)
                {
                    throw CreateExceptionWithMessage<T>(message);
                }
                throw new T();
            }
        }

        /// <summary>
        /// Throws an <see cref="ObjectDisposedException"/> if the specified object has been disposed.
        /// </summary>
        /// <param name="obj">The object to evaluate.</param>
        /// <param name="disposed">A value indicating whether the object is disposed.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EnsureNotDisposed(Object obj, Boolean disposed)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            if (disposed)
                throw new ObjectDisposedException(obj.GetType().Name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the specified pointer is <see cref="IntPtr.Zero"/>.
        /// </summary>
        /// <param name="argument">The object to evaluate for nullity.</param>
        /// <param name="message">The exception message.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Require(IntPtr argument, String message)
        {
            if (argument == IntPtr.Zero)
                throw new ArgumentNullException(message);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the specified pointer is <see cref="IntPtr.Zero"/>.
        /// </summary>
        /// <param name="argument">The object to evaluate for nullity.</param>
        /// <param name="message">The exception message.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Require(IntPtr argument, StringResource message)
        {
            if (argument == IntPtr.Zero)
                throw new ArgumentNullException(message);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the specified object is <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to evaluate for nullity.</typeparam>
        /// <param name="argument">The object to evaluate for nullity.</param>
        /// <param name="message">The exception message.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Require<T>(T argument, String message) where T : class
        {
            if (argument == null)
                throw new ArgumentNullException(message);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the specified object is null.
        /// </summary>
        /// <typeparam name="T">The type of object to evaluate for nullity.</typeparam>
        /// <param name="argument">The object to evaluate for nullity.</param>
        /// <param name="message">The exception message.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Require<T>(T argument, StringResource message) where T : class
        {
            if (argument == null)
                throw new ArgumentNullException(message);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the specified string is null, 
        /// or an <see cref="ArgumentException"/> if the string is empty.
        /// </summary>
        /// <param name="argument">The string to evaluate for nullity or emptiness.</param>
        /// <param name="message">The exception message.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequireNotEmpty(String argument, String message)
        {
            if (argument == null)
                throw new ArgumentNullException(message);
            if (argument == String.Empty)
                throw new ArgumentException(message);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the specified string is null, 
        /// or an <see cref="ArgumentException"/> if the string is empty.
        /// </summary>
        /// <param name="argument">The string to evaluate for nullity or emptiness.</param>
        /// <param name="message">The exception message.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequireNotEmpty(String argument, StringResource message)
        {
            if (argument == null)
                throw new ArgumentNullException(message);
            if (argument == String.Empty)
                throw new ArgumentException(message);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the specified collection is null, 
        /// or an <see cref="ArgumentException"/> if the collection is empty.
        /// </summary>
        /// <param name="collection">The collection to evaluate for nullity or emptiness.</param>
        /// <param name="message">The exception message.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequireNotEmpty(ICollection collection, String message)
        {
            if (collection == null)
                throw new ArgumentNullException(message);
            if (collection.Count == 0)
                throw new ArgumentException(message);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the specified collection is null, 
        /// or an <see cref="ArgumentException"/> if the collection is empty.
        /// </summary>
        /// <param name="collection">The collection to evaluate for nullity or emptiness.</param>
        /// <param name="message">The exception message.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequireNotEmpty(ICollection collection, StringResource message)
        {
            if (collection == null)
                throw new ArgumentNullException(message);
            if (collection.Count == 0)
                throw new ArgumentException(message);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the specified collection is null, 
        /// or an <see cref="ArgumentException"/> if the collection is empty.
        /// </summary>
        /// <typeparam name="T">The type of item contained by the collection.</typeparam>
        /// <param name="collection">The collection to evaluate for nullity or emptiness.</param>
        /// <param name="message">The exception message.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequireNotEmpty<T>(ICollection<T> collection, String message)
        {
            if (collection == null)
                throw new ArgumentNullException(message);
            if (collection.Count == 0)
                throw new ArgumentException(message);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the specified collection is null, 
        /// or an <see cref="ArgumentException"/> if the collection is empty.
        /// </summary>
        /// <typeparam name="T">The type of item contained by the collection.</typeparam>
        /// <param name="collection">The collection to evaluate for nullity or emptiness.</param>
        /// <param name="message">The exception message.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequireNotEmpty<T>(ICollection<T> collection, StringResource message)
        {
            if (collection == null)
                throw new ArgumentNullException(message);
            if (collection.Count == 0)
                throw new ArgumentException(message);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the specified collection is null, 
        /// or an <see cref="ArgumentException"/> if the collection is empty.
        /// </summary>
        /// <param name="collection">The collection to evaluate for nullity or emptiness.</param>
        /// <param name="message">The exception message.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequireNotEmpty(IEnumerable collection, String message)
        {
            if (collection == null)
                throw new ArgumentNullException(message);
            foreach (var item in collection)
            {
                return;
            }
            throw new ArgumentException(message);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the specified collection is null, 
        /// or an <see cref="ArgumentException"/> if the collection is empty.
        /// </summary>
        /// <param name="collection">The collection to evaluate for nullity or emptiness.</param>
        /// <param name="message">The exception message.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequireNotEmpty(IEnumerable collection, StringResource message)
        {
            if (collection == null)
                throw new ArgumentNullException(message);
            foreach (var item in collection)
            {
                return;
            }
            throw new ArgumentException(message);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the specified collection is null, 
        /// or an <see cref="ArgumentException"/> if the collection is empty.
        /// </summary>
        /// <typeparam name="T">The type of item contained by the collection.</typeparam>
        /// <param name="collection">The collection to evaluate for nullity or emptiness.</param>
        /// <param name="message">The exception message.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequireNotEmpty<T>(IEnumerable<T> collection, String message)
        {
            if (collection == null)
                throw new ArgumentNullException(message);
            foreach (var item in collection)
            {
                return;
            }
            throw new ArgumentException(message);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the specified collection is null, 
        /// or an <see cref="ArgumentException"/> if the collection is empty.
        /// </summary>
        /// <typeparam name="T">The type of item contained by the collection.</typeparam>
        /// <param name="collection">The collection to evaluate for nullity or emptiness.</param>
        /// <param name="message">The exception message.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequireNotEmpty<T>(IEnumerable<T> collection, StringResource message)
        {
            if (collection == null)
                throw new ArgumentNullException(message);
            foreach (var item in collection)
            {
                return;
            }
            throw new ArgumentException(message);
        }

        /// <summary>
        /// Creates a new exception object with the specified message as an argument.
        /// </summary>
        /// <param name="message">The message to pass to the exception as an argument.</param>
        /// <returns>The exception object that was created.</returns>
        private static T CreateExceptionWithMessage<T>(String message) where T : Exception, new()
        {
            var ctor = typeof(T).GetConstructor(new[] { typeof(String) });
            if (ctor != null)
            {
                return (T)ctor.Invoke(new[] { message });
            }
            return new T();
        }
    }
}
