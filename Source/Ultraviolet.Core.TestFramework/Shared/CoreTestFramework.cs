using System;
using System.Collections.Generic;
using System.Text;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Core.TestFramework
{
    /// <summary>
    /// Represents the test framework for the Ultraviolet core library's unit tests.
    /// </summary>
    public abstract class CoreTestFramework
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
        protected static CollectionResult<T> TheResultingCollection<T>(IEnumerable<T> value)
        {
            return new CollectionResult<T>(value);
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
        protected static StringResult TheResultingString(String value)
        {
            return new StringResult(value);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static StringResult TheResultingString(StringBuilder value)
        {
            return new StringResult(value == null ? null : value.ToString());
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static LocalizedStringResult TheResultingString(LocalizedString value)
        {
            return new LocalizedStringResult(value);
        }

        /// <summary>
        /// Wraps the specified unit test result for evaluation.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>The wrapped value.</returns>
        protected static LocalizedStringVariantResult TheResultingString(LocalizedStringVariant value)
        {
            return new LocalizedStringVariantResult(value);
        }
    }
}
