namespace Ultraviolet.Presentation
{
    partial class DependencyObject
    {
        /// <summary>
        /// Represents the different sources from which a dependency property can retrieve its value.
        /// </summary>
        private enum ValueSource
        {
            /// <summary>
            /// The dependency property is currently exposing its default value.
            /// </summary>
            DefaultValue,

            /// <summary>
            /// The dependency property is currently exposing its inherited value.
            /// </summary>
            InheritedValue,

            /// <summary>
            /// The dependency property is currently exposing its styled value.
            /// </summary>
            StyledValue,

            /// <summary>
            /// The dependency property is currently exposing its triggered value.
            /// </summary>
            TriggeredValue,

            /// <summary>
            /// The dependency property is currently exposing its local value.
            /// </summary>
            LocalValue,

            /// <summary>
            /// The dependency property is currently exposing its bound value.
            /// </summary>
            BoundValue,

            /// <summary>
            /// The dependency property is currently exposing its animated value.
            /// </summary>
            AnimatedValue,

            /// <summary>
            /// The dependency property is currently exposing its coerced value.
            /// </summary>
            CoercedValue,
        }
    }
}
