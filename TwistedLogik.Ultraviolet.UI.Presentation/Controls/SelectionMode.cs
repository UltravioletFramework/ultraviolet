using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents the supported selection modes for a <see cref="ListBox"/>.
    /// </summary>
    public enum SelectionMode
    {
        /// <summary>
        /// Only a single item can be selected.
        /// </summary>
        Single,

        /// <summary>
        /// Multiple items can be selected simultaneously.
        /// </summary>
        Multiple,
    }
}
