using Microsoft.VisualStudio.Text;

namespace Ultraviolet.VisualStudio.Uvss.Errors
{
    /// <summary>
    /// Contains extension methods relating to error handling.
    /// </summary>
    internal static class ErrorExtensions
    {
        /// <summary>
        /// Gets the <see cref="ErrorList"/> instance associated
        /// with the specified text buffer.
        /// </summary>
        /// <param name="textBuffer">The text buffer for which to retrieve
        /// a <see cref="ErrorList"/> instance.</param>
        /// <returns>The <see cref="ErrorList"/> instance associated with
        /// the specified text buffer, or null if the buffer has no instance.</returns>
        public static ErrorList GetErrorList(this ITextBuffer textBuffer)
        {
            ErrorList result;
            textBuffer.Properties.TryGetProperty(typeof(ErrorList), out result);
            return result;
        }
    }
}
