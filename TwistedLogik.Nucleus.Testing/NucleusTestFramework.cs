using System;
using System.Collections.Generic;
using System.Text;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Nucleus.Testing
{
    /// <summary>
    /// Represents the test framework for Nucleus unit tests.
    /// </summary>
    public abstract class NucleusTestFramework
    {
        /// <summary>
        /// Runs the specified action using the specified culture.
        /// </summary>
        /// <param name="culture">The culture in which to run the action.</param>
        /// <param name="action">The action to run in the specified culture.</param>
        protected void UsingCulture(String culture, Action action)
        {
            var previousCulture = Localization.CurrentCulture;
            try
            {
                Localization.CurrentCulture = culture;
                action();
            }
            finally
            {
                Localization.CurrentCulture = previousCulture;
            }
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="obj">The object to wrap.</param>
        /// <returns>The wrapped object.</returns>
        protected static ObjectResult<T> TheResultingObject<T>(T obj) where T : class
        {
            return new ObjectResult<T>(obj);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static ValueResult<T> TheResultingValue<T>(T value) where T : struct
        {
            return new ValueResult<T>(value);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static CollectionResult<T> TheResultingCollection<T>(IEnumerable<T> collection)
        {
            return new CollectionResult<T>(collection);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static BooleanResult TheResultingValue(Boolean value)
        {
            return new BooleanResult(value);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static SingleResult TheResultingValue(Single value)
        {
            return new SingleResult(value);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static DoubleResult TheResultingValue(Double value)
        {
            return new DoubleResult(value);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static StringResult TheResultingString(String str)
        {
            return new StringResult(str);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static StringResult TheResultingString(StringBuilder str)
        {
            return new StringResult(str == null ? null : str.ToString());
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static LocalizedStringResult TheResultingString(LocalizedString str)
        {
            return new LocalizedStringResult(str);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static LocalizedStringVariantResult TheResultingString(LocalizedStringVariant variant)
        {
            return new LocalizedStringVariantResult(variant);
        }
    }
}
