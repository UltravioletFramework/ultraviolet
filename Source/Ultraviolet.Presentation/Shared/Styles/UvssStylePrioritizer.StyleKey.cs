using System;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Styles
{
    partial class UvssStylePrioritizer
	{
		/// <summary>
		/// Represents the key which identifies a style + navigation expression.
		/// </summary>
		private partial struct StyleKey : IEquatable<StyleKey>
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="StyleKey"/> structure.
			/// </summary>
			/// <param name="name">The style's canonical name.</param>
			/// <param name="navigationExpression">The navigation expression for the style.</param>
			public StyleKey(String name, NavigationExpression? navigationExpression)
			{
				Contract.Require(name, nameof(name));

				this.name = name;
				this.navigationExpression = navigationExpression;
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
