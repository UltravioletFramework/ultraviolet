using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Input
{
    /// <summary>
    /// Contains methods for performing navigation with the keyboard.
    /// </summary>
    public static class KeyboardNavigation
    {
        /// <summary>
        /// Identifies the TabIndex dependency property.
        /// </summary>
        public static readonly DependencyProperty TabIndexProperty = DependencyProperty.Register("TabIndex", typeof(Int32), typeof(KeyboardNavigation),
            new DependencyPropertyMetadata(null, () => CommonBoxedValues.Int32.MaxValue, DependencyPropertyOptions.None));

        /// <summary>
        /// Identifies the IsTabStop dependency property.
        /// </summary>
        public static readonly DependencyProperty IsTabStopProperty = DependencyProperty.Register("IsTabStop", typeof(Boolean), typeof(KeyboardNavigation),
            new DependencyPropertyMetadata(null, () => CommonBoxedValues.Boolean.True, DependencyPropertyOptions.None));
    }
}
