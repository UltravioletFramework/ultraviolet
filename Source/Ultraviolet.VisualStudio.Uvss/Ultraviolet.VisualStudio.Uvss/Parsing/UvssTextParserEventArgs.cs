using System;

namespace Ultraviolet.VisualStudio.Uvss.Parsing
{
    /// <summary>
    /// Represents the method that is called when the parsing service
    /// finishes parsing a document.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="args">A <see cref="UvssTextParserEventArgs"/> that contains the event data.</param>
    public delegate void UvssTextParserEventHandler(Object sender, UvssTextParserEventArgs args);

	/// <summary>
	/// Represents the arguments for the event that is raised when the parsing
	/// service finishes parsing a document.
	/// </summary>
	public sealed class UvssTextParserEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UvssTextParserEventArgs"/> class.
		/// </summary>
		/// <param name="result">The result of the parsing operation.</param>
		public UvssTextParserEventArgs(UvssTextParserResult result)
		{
			this.Result = result;
		}

		/// <summary>
		/// Gets the result of the parsing operation.
		/// </summary>
		public UvssTextParserResult Result { get; }
	}
}
