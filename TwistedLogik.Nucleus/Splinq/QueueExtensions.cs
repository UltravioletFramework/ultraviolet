using System;
using System.Collections.Generic;

namespace TwistedLogik.Nucleus.Splinq
{
    /// <summary>
    /// Contains SpLINQ extension methods for the <see cref="Queue{T}"/> class.
    /// </summary>
    public static class QueueExtensions
    {
        /// <summary>
        /// Gets a value indicating whether the specified queue contains any elements.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="Queue{T}"/> to evaluate.</param>
        /// <returns><see langword="true"/> if the source queue contains any elements; otherwise, <see langword="false"/>.</returns>
        public static Boolean Any<T>(this Queue<T> source)
        {
            Contract.Require(source, nameof(source));

            return source.Count > 0;
        }

        /// <summary>
        /// Gets a value indicating whether the specified queue contains any elements which match the specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="Queue{T}"/> to evaluate.</param>
        /// <param name="predicate">The predicate against which to evaluate the items of <paramref name="source"/>.</param>
        /// <returns><see langword="true"/> if the source queue contains any elements; otherwise, <see langword="false"/>.</returns>
        public static Boolean Any<T>(this Queue<T> source, Predicate<T> predicate)
        {
            Contract.Require(source, nameof(source));
            Contract.Require(predicate, nameof(predicate));

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether all of the items in the specified queue match the specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="Queue{T}"/> to evaluate.</param>
        /// <param name="predicate">The predicate against which to evaluate the items of <paramref name="source"/>.</param>
        /// <returns><see langword="true"/> if all of the items in the source queue match the specified predicate; otherwise, <see langword="false"/>.</returns>
        public static Boolean All<T>(this Queue<T> source, Predicate<T> predicate)
        {
            Contract.Require(source, nameof(source));
            Contract.Require(predicate, nameof(predicate));

            foreach (var item in source)
            {
                if (!predicate(item))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the number of items in the specified queue.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="Queue{T}"/> to evaluate.</param>
        /// <returns>The number of items in the source queue.</returns>
        public static Int32 Count<T>(this Queue<T> source)
        {
            Contract.Require(source, nameof(source));

            return source.Count;
        }

        /// <summary>
        /// Gets the number of items in the specified queue which match the specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="Queue{T}"/> to evaluate.</param>
        /// <param name="predicate">The predicate against which to evaluate the items of <paramref name="source"/>.</param>
        /// <returns>The number of items in the source queue.</returns>
        public static Int32 Count<T>(this Queue<T> source, Predicate<T> predicate)
        {
            Contract.Require(source, nameof(source));
            Contract.Require(predicate, nameof(predicate));

            var count = 0;

            foreach (var item in source)
            {
                if (predicate(item))
                    count++;
            }

            return count;
        }

        /// <summary>
        /// Gets the first item in the specified queue.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="Queue{T}"/> to evaluate.</param>
        /// <returns>The first item in the source queue.</returns>
        public static T First<T>(this Queue<T> source)
        {
            Contract.Require(source, nameof(source));
            
            foreach (var item in source)
                return item;

            throw new InvalidOperationException(NucleusStrings.SequenceHasNoElements);
        }

        /// <summary>
        /// Gets the first item in the specified queue that satisfies the given predicate.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="Queue{T}"/> to evaluate.</param>
        /// <param name="predicate">The predicate against which to evaluate the items of <paramref name="source"/>.</param>
        /// <returns>The first item in the source queue that satisfies the predicate.</returns>
        public static T First<T>(this Queue<T> source, Predicate<T> predicate)
        {
            Contract.Require(source, nameof(source));
            Contract.Require(predicate, nameof(predicate));

            foreach (var item in source)
            {
                if (predicate(item))
                    return item;
            }

            throw new InvalidOperationException(NucleusStrings.SequenceHasNoElements);
        }

        /// <summary>
        /// Gets the last item in the specified queue.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="Queue{T}"/> to evaluate.</param>
        /// <returns>The last item in the source queue.</returns>
        public static T Last<T>(this Queue<T> source)
        {
            Contract.Require(source, nameof(source));

            var value = default(T);
            var valueExists = false;

            foreach (var item in source)
            {
                value = item;
                valueExists = true;
            }

            if (valueExists)
                return value;

            throw new InvalidOperationException(NucleusStrings.SequenceHasNoElements);
        }

        /// <summary>
        /// Gets the last item in the specified queue that satisfies the given predicate.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="Queue{T}"/> to evaluate.</param>
        /// <param name="predicate">The predicate against which to evaluate the items of <paramref name="source"/>.</param>
        /// <returns>The last item in the source queue that satisfies the predicate.</returns>
        public static T Last<T>(this Queue<T> source, Predicate<T> predicate)
        {
            Contract.Require(source, nameof(source));
            Contract.Require(predicate, nameof(predicate));

            var value = default(T);
            var valueExists = false;

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    value = item;
                    valueExists = true;
                }
            }

            if (valueExists)
                return value;

            throw new InvalidOperationException(NucleusStrings.SequenceHasNoElements);
        }

        /// <summary>
        /// Returns the only element in the queue, and throws an exception if there is not exactly
        /// one item in the queue.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="Queue{T}"/> to evaluate.</param>
        /// <returns>The single item in the source queue.</returns>
        public static T Single<T>(this Queue<T> source)
        {
            Contract.Require(source, nameof(source));
            
            if (source.Count > 1)
                throw new InvalidOperationException(NucleusStrings.SequenceHasMoreThanOneElement);

            foreach (var item in source)
                return item;

            throw new InvalidOperationException(NucleusStrings.SequenceHasNoElements);
        }

        /// <summary>
        /// Returns the only element in the queue that satisfies the given predicate, and throws an exception if there 
        /// is not exactly one such item in the queue.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="Queue{T}"/> to evaluate.</param>
        /// <param name="predicate">The predicate against which to evaluate the items of <paramref name="source"/>.</param>
        /// <returns>The single item in the source queue that matches the specified predicate.</returns>
        public static T Single<T>(this Queue<T> source, Predicate<T> predicate)
        {
            Contract.Require(source, nameof(source));
            Contract.Require(predicate, nameof(predicate));

            var count = 0;
            var value = default(T);

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    value = item;
                    count++;
                }
            }

            if (count == 0)
                throw new InvalidOperationException(NucleusStrings.SequenceHasNoElements);

            if (count > 1)
                throw new InvalidOperationException(NucleusStrings.SequenceHasMoreThanOneElement);

            return value;
        }

        /// <summary>
        /// Returns the only element in the queue, or a default value if no items exist in the queue.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="Queue{T}"/> to evaluate.</param>
        /// <returns>The single item in the source queue, or a default value.</returns>
        public static T SingleOrDefault<T>(this Queue<T> source)
        {
            Contract.Require(source, nameof(source));

            if (source.Count > 1)
                throw new InvalidOperationException(NucleusStrings.SequenceHasMoreThanOneElement);

            foreach (var item in source)
                return item;

            return default(T);
        }

        /// <summary>
        /// Returns the only element in the queue that satisfies the given predicate, or a default value if 
        /// no such items exist in the queue.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="Queue{T}"/> to evaluate.</param>
        /// <param name="predicate">The predicate against which to evaluate the items of <paramref name="source"/>.</param>
        /// <returns>The single item in the source queue, or a default value.</returns>
        public static T SingleOrDefault<T>(this Queue<T> source, Predicate<T> predicate)
        {
            Contract.Require(source, nameof(source));
            Contract.Require(predicate, nameof(predicate));

            var count = 0;
            var value = default(T);

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    value = item;
                    count++;
                }
            }

            if (count == 0)
                return default(T);

            if (count > 1)
                throw new InvalidOperationException(NucleusStrings.SequenceHasMoreThanOneElement);

            return value;
        }

        /// <summary>
        /// Gets the maximum item in the queue.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="Queue{T}"/> to evaluate.</param>
        /// <returns>The maximum item in the queue.</returns>
        public static T Max<T>(this Queue<T> source)
        {
            Contract.Require(source, nameof(source));

            var comparer = Comparer<T>.Default;

            var value = default(T);
            var valueFound = false;

            foreach (var item in source)
            {
                if (item == null)
                    continue;

                if (!valueFound)
                {
                    value = item;
                    valueFound = true;
                }
                else
                {
                    if (comparer.Compare(item, value) > 0)
                    {
                        value = item;
                    }
                }
            }

            if (valueFound)
                return value;

            throw new InvalidOperationException(NucleusStrings.SequenceHasNoElements);
        }

        /// <summary>
        /// Gets the minimum item in the queue.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="Queue{T}"/> to evaluate.</param>
        /// <returns>The minimum item in the queue.</returns>
        public static T Min<T>(this Queue<T> source)
        {
            Contract.Require(source, nameof(source));

            var comparer = Comparer<T>.Default;

            var value = default(T);
            var valueFound = false;

            foreach (var item in source)
            {
                if (item == null)
                    continue;

                if (!valueFound)
                {
                    value = item;
                    valueFound = true;
                }
                else
                {
                    if (comparer.Compare(item, value) < 0)
                    {
                        value = item;
                    }
                }
            }

            if (valueFound)
                return value;

            throw new InvalidOperationException(NucleusStrings.SequenceHasNoElements);
        }
    }
}
