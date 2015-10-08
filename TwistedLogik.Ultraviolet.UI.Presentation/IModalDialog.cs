using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents a modal dialog box.
    /// </summary>
    public interface IModalDialog
    {
        /// <summary>
        /// Gets or sets the dialog's result value.
        /// </summary>
        Boolean? DialogResult
        {
            get;
            set;
        }
    }
}
