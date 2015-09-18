using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus.Collections;

namespace TwistedLogik.Nucleus.Splinq
{
    /// <summary>
    /// Contains SpLINQ extension methods for the <see cref="ObservableDictionary{TKey, TValue}"/> class.
    /// </summary>
    public static class ObservableDictionaryExtensions
    {
        /// <summary>
        /// Gets a value indicating whether the specified dictionary contains any elements.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="ObservableDictionary{TKey, TValue}"/> to evaluate.</param>
        /// <returns><c>true</c> if the source dictionary contains any elements; otherwise, <c>false</c>.</returns>
        public static Boolean Any<TKey, TValue>(this ObservableDictionary<TKey, TValue> source)
        {
            Contract.Require(source, "source");

            return source.Count > 0;
        }

        /// <summary>
        /// Gets a value indicating whether the specified dictionary contains any elements which match the specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="ObservableDictionary{TKey, TValue}"/> to evaluate.</param>
        /// <param name="predicate">The predicate against which to evaluate the items of <paramref name="source"/>.</param>
        /// <returns><c>true</c> if the source dictionary contains any elements; otherwise, <c>false</c>.</returns>
        public static Boolean Any<TKey, TValue>(this ObservableDictionary<TKey, TValue> source, Predicate<KeyValuePair<TKey, TValue>> predicate)
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
        /// Gets a value indicating whether all of the items in the specified dictionary match the specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="ObservableDictionary{TKey, TValue}"/> to evaluate.</param>
        /// <param name="predicate">The predicate against which to evaluate the items of <paramref name="source"/>.</param>
        /// <returns><c>true</c> if all of the items in the source dictionary match the specified predicate; otherwise, <c>false</c>.</returns>
        public static Boolean All<TKey, TValue>(this ObservableDictionary<TKey, TValue> source, Predicate<KeyValuePair<TKey, TValue>> predicate)
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
        /// Gets the number of items in the specified dictionary.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="ObservableDictionary{TKey, TValue}"/> to evaluate.</param>
        /// <returns>The number of items in the source dictionary.</returns>
        public static Int32 Count<TKey, TValue>(this ObservableDictionary<TKey, TValue> source)
        {
            Contract.Require(source, "source");

            return source.Count;
        }

        /// <summary>
        /// Gets the number of items in the specified dictionary which match the specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="ObservableDictionary{TKey, TValue}"/> to evaluate.</param>
        /// <param name="predicate">The predicate against which to evaluate the items of <paramref name="source"/>.</param>
        /// <returns>The number of items in the source dictionary.</returns>
        public static Int32 Count<TKey, TValue>(this ObservableDictionary<TKey, TValue> source, Predicate<KeyValuePair<TKey, TValue>> predicate)
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
        /// Gets the first item in the specified dictionary.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="ObservableDictionary{TKey, TValue}"/> to evaluate.</param>
        /// <returns>The first item in the source dictionary.</returns>
        public static KeyValuePair<TKey, TValue> First<TKey, TValue>(this ObservableDictionary<TKey, TValue> source)
        {
            Contract.Require(source, "source");

            foreach (var item in source)
                return item;

            throw new InvalidOperationException(NucleusStrings.SequenceHasNoElements);
        }

        /// <summary>
        /// Gets the first item in the specified dictionary that satisfies the given predicate.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="ObservableDictionary{TKey, TValue}"/> to evaluate.</param>
        /// <param name="predicate">The predicate against which to evaluate the items of <paramref name="source"/>.</param>
        /// <returns>The first item in the source dictionary that satisfies the predicate.</returns>
        public static KeyValuePair<TKey, TValue> First<TKey, TValue>(this ObservableDictionary<TKey, TValue> source, Predicate<KeyValuePair<TKey, TValue>> predicate)
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
        /// Gets the last item in the specified dictionary.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="ObservableDictionary{TKey, TValue}"/> to evaluate.</param>
        /// <returns>The last item in the source dictionary.</returns>
        public static KeyValuePair<TKey, TValue> Last<TKey, TValue>(this ObservableDictionary<TKey, TValue> source)
        {
            Contract.Require(source, "source");

            var value = default(KeyValuePair<TKey, TValue>);
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
        /// Gets the last item in the specified dictionary that satisfies the given predicate.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="ObservableDictionary{TKey, TValue}"/> to evaluate.</param>
        /// <param name="predicate">The predicate against which to evaluate the items of <paramref name="source"/>.</param>
        /// <returns>The last item in the source dictionary that satisfies the predicate.</returns>
        public static KeyValuePair<TKey, TValue> Last<TKey, TValue>(this ObservableDictionary<TKey, TValue> source, Predicate<KeyValuePair<TKey, TValue>> predicate)
        {
            Contract.Require(source, "source");
            Contract.Require(predicate, "predicate");

            var value = default(KeyValuePair<TKey, TValue>);
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
        /// Returns the only element in the dictionary, and throws an exception if there is not exactly
        /// one item in the dictionary.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="ObservableDictionary{TKey, TValue}"/> to evaluate.</param>
        /// <returns>The single item in the source dictionary.</returns>
        public static KeyValuePair<TKey, TValue> Single<TKey, TValue>(this ObservableDictionary<TKey, TValue> source)
        {
            Contract.Require(source, "source");

            if (source.Count > 1)
                throw new InvalidOperationException(NucleusStrings.SequenceHasMoreThanOneElement);

            foreach (var item in source)
                return item;

            throw new InvalidOperationException(NucleusStrings.SequenceHasNoElements);
        }

        /// <summary>
        /// Returns the only element in the dictionary that satisfies the given predicate, and throws an exception if there 
        /// is not exactly one such item in the dictionary.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="ObservableDictionary{TKey, TValue}"/> to evaluate.</param>
        /// <param name="predicate">The predicate against which to evaluate the items of <paramref name="source"/>.</param>
        /// <returns>The single item in the source dictionary that matches the specified predicate.</returns>
        public static KeyValuePair<TKey, TValue> Single<TKey, TValue>(this ObservableDictionary<TKey, TValue> source, Predicate<KeyValuePair<TKey, TValue>> predicate)
        {
            Contract.Require(source, "source");
            Contract.Require(predicate, "predicate");

            var count = 0;
            var value = default(KeyValuePair<TKey, TValue>);

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
        /// Returns the only element in the dictionary, or a default value if no items exist in the dictionary.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="ObservableDictionary{TKey, TValue}"/> to evaluate.</param>
        /// <returns>The single item in the source dictionary, or a default value.</returns>
        public static KeyValuePair<TKey, TValue> SingleOrDefault<TKey, TValue>(this ObservableDictionary<TKey, TValue> source)
        {
            Contract.Require(source, "source");

            if (source.Count > 1)
                throw new InvalidOperationException(NucleusStrings.SequenceHasMoreThanOneElement);

            foreach (var item in source)
                return item;

            return default(KeyValuePair<TKey, TValue>);
        }

        /// <summary>
        /// Returns the only element in the dictionary that satisfies the given predicate, or a default value if 
        /// no such items exist in the dictionary.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="ObservableDictionary{TKey, TValue}"/> to evaluate.</param>
        /// <param name="predicate">The predicate against which to evaluate the items of <paramref name="source"/>.</param>
        /// <returns>The single item in the source dictionary, or a default value.</returns>
        public static KeyValuePair<TKey, TValue> SingleOrDefault<TKey, TValue>(this ObservableDictionary<TKey, TValue> source, Predicate<KeyValuePair<TKey, TValue>> predicate)
        {
            Contract.Require(source, "source");
            Contract.Require(predicate, "predicate");

            var count = 0;
            var value = default(KeyValuePair<TKey, TValue>);

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    value = item;
                    count++;
                }
            }

            if (count == 0)
                return default(KeyValuePair<TKey, TValue>);

            if (count > 1)
                throw new InvalidOperationException(NucleusStrings.SequenceHasMoreThanOneElement);

            return value;
        }

        /// <summary>
        /// Gets the maximum item in the dictionary.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="ObservableDictionary{TKey, TValue}"/> to evaluate.</param>
        /// <returns>The maximum item in the dictionary.</returns>
        public static KeyValuePair<TKey, TValue> Max<TKey, TValue>(this ObservableDictionary<TKey, TValue> source)
        {
            Contract.Require(source, "source");

            var comparer = Comparer<KeyValuePair<TKey, TValue>>.Default;

            var value = default(KeyValuePair<TKey, TValue>);
            var valueFound = false;

            foreach (var item in source)
            {
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
        /// Gets the minimum item in the dictionary.
        /// </summary>
        /// <typeparam name="T">The type of item contained by <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="ObservableDictionary{TKey, TValue}"/> to evaluate.</param>
        /// <returns>The minimum item in the dictionary.</returns>
        public static KeyValuePair<TKey, TValue> Min<TKey, TValue>(this ObservableDictionary<TKey, TValue> source)
        {
            Contract.Require(source, "source");

            var comparer = Comparer<KeyValuePair<TKey, TValue>>.Default;

            var value = default(KeyValuePair<TKey, TValue>);
            var valueFound = false;

            foreach (var item in source)
            {
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
