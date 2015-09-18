using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus.Collections;

namespace TwistedLogik.Nucleus.Splinq
{
    /// <summary>
    /// Contains SpLINQ extension methods for the <see cref="ObservableList{T}"/> class.
    /// </summary>
    public static class ObservableListExtensions
    {
        /// <summary>
        /// Gets a value indicating whether the specified list contains any elements.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="ObservableList{T}"/> to evaluate.</param>
        /// <returns><c>true</c> if the source list contains any elements; otherwise, <c>false</c>.</returns>
        public static Boolean Any<T>(this ObservableList<T> source)
        {
            Contract.Require(source, "source");

            return source.Count > 0;
        }

        /// <summary>
        /// Gets a value indicating whether the specified list contains any elements which match the specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="ObservableList{T}"/> to evaluate.</param>
        /// <param name="predicate">The predicate against which to evaluate the items of <paramref name="source"/>.</param>
        /// <returns><c>true</c> if the source list contains any elements; otherwise, <c>false</c>.</returns>
        public static Boolean Any<T>(this ObservableList<T> source, Predicate<T> predicate)
        {
            Contract.Require(source, "source");
            Contract.Require(predicate, "predicate");

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
        /// <param name="source">The <see cref="ObservableList{T}"/> to evaluate.</param>
        /// <param name="predicate">The predicate against which to evaluate the items of <paramref name="source"/>.</param>
        /// <returns><c>true</c> if all of the items in the source list match the specified predicate; otherwise, <c>false</c>.</returns>
        public static Boolean All<T>(this ObservableList<T> source, Predicate<T> predicate)
        {
            Contract.Require(source, "source");
            Contract.Require(predicate, "predicate");

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
        /// <param name="source">The <see cref="ObservableList{T}"/> to evaluate.</param>
        /// <returns>The number of items in the source list.</returns>
        public static Int32 Count<T>(this ObservableList<T> source)
        {
            Contract.Require(source, "source");

            return source.Count;
        }

        /// <summary>
        /// Gets the number of items in the specified list which match the specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="ObservableList{T}"/> to evaluate.</param>
        /// <param name="predicate">The predicate against which to evaluate the items of <paramref name="source"/>.</param>
        /// <returns>The number of items in the source list.</returns>
        public static Int32 Count<T>(this ObservableList<T> source, Predicate<T> predicate)
        {
            Contract.Require(source, "source");
            Contract.Require(predicate, "predicate");

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
        /// <param name="source">The <see cref="ObservableList{T}"/> to evaluate.</param>
        /// <returns>The first item in the source list.</returns>
        public static T First<T>(this ObservableList<T> source)
        {
            Contract.Require(source, "source");

            if (source.Count == 0)
                throw new InvalidOperationException(NucleusStrings.SequenceHasNoElements);

            return source[0];
        }

        /// <summary>
        /// Gets the first item in the specified list that satisfies the given predicate.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="ObservableList{T}"/> to evaluate.</param>
        /// <param name="predicate">The predicate against which to evaluate the items of <paramref name="source"/>.</param>
        /// <returns>The first item in the source list that satisfies the predicate.</returns>
        public static T First<T>(this ObservableList<T> source, Predicate<T> predicate)
        {
            Contract.Require(source, "source");
            Contract.Require(predicate, "predicate");

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
        /// <param name="source">The <see cref="ObservableList{T}"/> to evaluate.</param>
        /// <returns>The last item in the source list.</returns>
        public static T Last<T>(this ObservableList<T> source)
        {
            Contract.Require(source, "source");

            if (source.Count == 0)
                throw new InvalidOperationException(NucleusStrings.SequenceHasNoElements);

            return source[source.Count - 1];
        }

        /// <summary>
        /// Gets the last item in the specified list that satisfies the given predicate.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="ObservableList{T}"/> to evaluate.</param>
        /// <param name="predicate">The predicate against which to evaluate the items of <paramref name="source"/>.</param>
        /// <returns>The last item in the source list that satisfies the predicate.</returns>
        public static T Last<T>(this ObservableList<T> source, Predicate<T> predicate)
        {
            Contract.Require(source, "source");
            Contract.Require(predicate, "predicate");

            for (int i = source.Count - 1; i >= 0; i--)
            {
                var item = source[i];
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
        /// <param name="source">The <see cref="ObservableList{T}"/> to evaluate.</param>
        /// <returns>The single item in the source list.</returns>
        public static T Single<T>(this ObservableList<T> source)
        {
            Contract.Require(source, "source");

            if (source.Count == 0)
                throw new InvalidOperationException(NucleusStrings.SequenceHasNoElements);

            if (source.Count > 1)
                throw new InvalidOperationException(NucleusStrings.SequenceHasMoreThanOneElement);

            return source[0];
        }

        /// <summary>
        /// Returns the only element in the list that satisfies the given predicate, and throws an exception if there 
        /// is not exactly one such item in the list.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="ObservableList{T}"/> to evaluate.</param>
        /// <param name="predicate">The predicate against which to evaluate the items of <paramref name="source"/>.</param>
        /// <returns>The single item in the source list that matches the specified predicate.</returns>
        public static T Single<T>(this ObservableList<T> source, Predicate<T> predicate)
        {
            Contract.Require(source, "source");
            Contract.Require(predicate, "predicate");

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
        /// <param name="source">The <see cref="ObservableList{T}"/> to evaluate.</param>
        /// <returns>The single item in the source list, or a default value.</returns>
        public static T SingleOrDefault<T>(this ObservableList<T> source)
        {
            Contract.Require(source, "source");

            if (source.Count < 1)
                return default(T);

            if (source.Count > 1)
                throw new InvalidOperationException(NucleusStrings.SequenceHasMoreThanOneElement);

            return source[0];
        }

        /// <summary>
        /// Returns the only element in the list that satisfies the given predicate, or a default value if 
        /// no such items exist in the list.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="ObservableList{T}"/> to evaluate.</param>
        /// <param name="predicate">The predicate against which to evaluate the items of <paramref name="source"/>.</param>
        /// <returns>The single item in the source list, or a default value.</returns>
        public static T SingleOrDefault<T>(this ObservableList<T> source, Predicate<T> predicate)
        {
            Contract.Require(source, "source");
            Contract.Require(predicate, "predicate");

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
        /// <param name="source">The <see cref="ObservableList{T}"/> to evaluate.</param>
        /// <returns>The maximum item in the list.</returns>
        public static T Max<T>(this ObservableList<T> source)
        {
            Contract.Require(source, "source");

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
        /// <param name="source">The <see cref="ObservableList{T}"/> to evaluate.</param>
        /// <returns>The minimum item in the list.</returns>
        public static T Min<T>(this ObservableList<T> source)
        {
            Contract.Require(source, "source");

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
