using System;
using System.Collections.Generic;

namespace TwistedLogik.Nucleus.Splinq
{
    /// <summary>
    /// Contains SpLINQ extension methods for the <see cref="LinkedList{T}"/> class.
    /// </summary>
    public static class LinkedListExtensions
    {
        /// <summary>
        /// Gets a value indicating whether the specified list contains any elements.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="LinkedList{T}"/> to evaluate.</param>
        /// <returns><see langword="true"/> if the source list contains any elements; otherwise, <see langword="false"/>.</returns>
        public static Boolean Any<T>(this LinkedList<T> source)
        {
            Contract.Require(source, nameof(source));

            return source.Count > 0;
        }

        /// <summary>
        /// Gets a value indicating whether the specified list contains any elements which match the specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="LinkedList{T}"/> to evaluate.</param>
        /// <param name="predicate">The predicate against which to evaluate the items of <paramref name="source"/>.</param>
        /// <returns><see langword="true"/> if the source list contains any elements; otherwise, <see langword="false"/>.</returns>
        public static Boolean Any<T>(this LinkedList<T> source, Predicate<T> predicate)
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
        /// Gets a value indicating whether all of the items in the specified list match the specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="LinkedList{T}"/> to evaluate.</param>
        /// <param name="predicate">The predicate against which to evaluate the items of <paramref name="source"/>.</param>
        /// <returns><see langword="true"/> if all of the items in the source list match the specified predicate; otherwise, <see langword="false"/>.</returns>
        public static Boolean All<T>(this LinkedList<T> source, Predicate<T> predicate)
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
        /// Gets the number of items in the specified list.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="LinkedList{T}"/> to evaluate.</param>
        /// <returns>The number of items in the source list.</returns>
        public static Int32 Count<T>(this LinkedList<T> source)
        {
            Contract.Require(source, nameof(source));

            return source.Count;
        }

        /// <summary>
        /// Gets the number of items in the specified list which match the specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="LinkedList{T}"/> to evaluate.</param>
        /// <param name="predicate">The predicate against which to evaluate the items of <paramref name="source"/>.</param>
        /// <returns>The number of items in the source list.</returns>
        public static Int32 Count<T>(this LinkedList<T> source, Predicate<T> predicate)
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
        /// Gets the first item in the specified list.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="LinkedList{T}"/> to evaluate.</param>
        /// <returns>The first item in the source list.</returns>
        public static T First<T>(this LinkedList<T> source)
        {
            Contract.Require(source, nameof(source));

            if (source.Count == 0)
                throw new InvalidOperationException(NucleusStrings.SequenceHasNoElements);

            return source.First.Value;
        }

        /// <summary>
        /// Gets the first item in the specified list that satisfies the given predicate.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="LinkedList{T}"/> to evaluate.</param>
        /// <param name="predicate">The predicate against which to evaluate the items of <paramref name="source"/>.</param>
        /// <returns>The first item in the source list that satisfies the predicate.</returns>
        public static T First<T>(this LinkedList<T> source, Predicate<T> predicate)
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
        /// Gets the last item in the specified list.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="LinkedList{T}"/> to evaluate.</param>
        /// <returns>The last item in the source list.</returns>
        public static T Last<T>(this LinkedList<T> source)
        {
            Contract.Require(source, nameof(source));

            if (source.Count == 0)
                throw new InvalidOperationException(NucleusStrings.SequenceHasNoElements);

            return source.Last.Value;
        }

        /// <summary>
        /// Gets the last item in the specified list that satisfies the given predicate.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="LinkedList{T}"/> to evaluate.</param>
        /// <param name="predicate">The predicate against which to evaluate the items of <paramref name="source"/>.</param>
        /// <returns>The last item in the source list that satisfies the predicate.</returns>
        public static T Last<T>(this LinkedList<T> source, Predicate<T> predicate)
        {
            Contract.Require(source, nameof(source));
            Contract.Require(predicate, nameof(predicate));

            for (var current = source.Last; current != null; current = current.Previous)
            {
                var item = current.Value;
                if (predicate(item))
                    return item;
            }

            throw new InvalidOperationException(NucleusStrings.SequenceHasNoElements);
        }

        /// <summary>
        /// Returns the only element in the list, and throws an exception if there is not exactly
        /// one item in the list.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="LinkedList{T}"/> to evaluate.</param>
        /// <returns>The single item in the source list.</returns>
        public static T Single<T>(this LinkedList<T> source)
        {
            Contract.Require(source, nameof(source));

            if (source.Count == 0)
                throw new InvalidOperationException(NucleusStrings.SequenceHasNoElements);

            if (source.Count > 1)
                throw new InvalidOperationException(NucleusStrings.SequenceHasMoreThanOneElement);

            return source.First.Value;
        }

        /// <summary>
        /// Returns the only element in the list that satisfies the given predicate, and throws an exception if there 
        /// is not exactly one such item in the list.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="LinkedList{T}"/> to evaluate.</param>
        /// <param name="predicate">The predicate against which to evaluate the items of <paramref name="source"/>.</param>
        /// <returns>The single item in the source list that matches the specified predicate.</returns>
        public static T Single<T>(this LinkedList<T> source, Predicate<T> predicate)
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
        /// Returns the only element in the list, or a default value if no items exist in the list.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="LinkedList{T}"/> to evaluate.</param>
        /// <returns>The single item in the source list, or a default value.</returns>
        public static T SingleOrDefault<T>(this LinkedList<T> source)
        {
            Contract.Require(source, nameof(source));

            if (source.Count < 1)
                return default(T);

            if (source.Count > 1)
                throw new InvalidOperationException(NucleusStrings.SequenceHasMoreThanOneElement);

            return source.First.Value;
        }

        /// <summary>
        /// Returns the only element in the list that satisfies the given predicate, or a default value if 
        /// no such items exist in the list.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="LinkedList{T}"/> to evaluate.</param>
        /// <param name="predicate">The predicate against which to evaluate the items of <paramref name="source"/>.</param>
        /// <returns>The single item in the source list, or a default value.</returns>
        public static T SingleOrDefault<T>(this LinkedList<T> source, Predicate<T> predicate)
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
        /// Gets the maximum item in the list.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="LinkedList{T}"/> to evaluate.</param>
        /// <returns>The maximum item in the list.</returns>
        public static T Max<T>(this LinkedList<T> source)
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
        /// Gets the minimum item in the list.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="LinkedList{T}"/> to evaluate.</param>
        /// <returns>The minimum item in the list.</returns>
        public static T Min<T>(this LinkedList<T> source)
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
