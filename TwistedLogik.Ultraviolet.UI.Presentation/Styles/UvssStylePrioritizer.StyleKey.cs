using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
{
	partial class UvssStylePrioritizer
	{
		/// <summary>
		/// Represents the key which identifies a style + navigation expression.
		/// </summary>
		private struct StyleKey : IEquatable<StyleKey>
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="StyleKey"/> structure.
			/// </summary>
			/// <param name="name">The style's canonical name.</param>
			/// <param name="navigationExpression">The navigation expression for the style.</param>
			public StyleKey(String name, NavigationExpression? navigationExpression)
			{
				Contract.Require(name, "name");

				this.name = name;
				this.navigationExpression = navigationExpression;
			}

			/// <summary>
			/// Compares two <see cref="StyleKey"/> values for equality.
			/// </summary>
			/// <param name="stk1">The first <see cref="StyleKey"/> to compare.</param>
			/// <param name="stk2">The second <see cref="StyleKey"/> to compare.</param>
			/// <returns><see langword="true"/> if the specified keys are equal; otherwise, <see langword="false"/>.</returns>
			public static Boolean operator ==(StyleKey stk1, StyleKey stk2)
			{
				return stk1.Equals(stk2);
			}

			/// <summary>
			/// Compares two <see cref="StyleKey"/> values for inequality.
			/// </summary>
			/// <param name="stk1">The first <see cref="StyleKey"/> to compare.</param>
			/// <param name="stk2">The second <see cref="StyleKey"/> to compare.</param>
			/// <returns><see langword="true"/> if the specified keys are unequal; otherwise, <see langword="false"/>.</returns>
			public static Boolean operator !=(StyleKey stk1, StyleKey stk2)
			{
				return !stk1.Equals(stk2);
			}

			/// <inheritdoc/>
			public override Int32 GetHashCode()
			{
				unchecked
				{
					var hash = 17;
					hash = hash * 23 + (name == null ? 0 : name.GetHashCode());
					hash = hash * 23 + (navigationExpression == null ? 0 : navigationExpression.GetHashCode());
					return hash;
				}
			}

			/// <inheritdoc/>
			public override Boolean Equals(Object obj)
			{
				if (!(obj is StyleKey))
				{
					return false;
				}
				return Equals((StyleKey)obj);
			}

			/// <inheritdoc/>
			public Boolean Equals(StyleKey other)
			{
				return
					name == other.name &&
					navigationExpression.HasValue == other.navigationExpression.HasValue &&
					navigationExpression.GetValueOrDefault().Equals(other.navigationExpression.GetValueOrDefault());
			}

			/// <summary>
			/// Gets the canonical name of the style.
			/// </summary>
			public String Name
			{
				get { return name; }
			}

			/// <summary>
			/// Gets the navigation expression for the style.
			/// </summary>
			public NavigationExpression? NavigationExpression
			{
				get { return navigationExpression; }
			}

			// Property values.
			private readonly String name;
			private readonly NavigationExpression? navigationExpression;
		}
	}
}
