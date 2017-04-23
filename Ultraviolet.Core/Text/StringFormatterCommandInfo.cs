namespace Ultraviolet.Core.Text
{
    /// <summary>
    /// Represents the metadata required to handle a string formatter command.
    /// </summary>
    internal struct StringFormatterCommandInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringFormatterCommandInfo"/> structure.
        /// </summary>
        /// <param name="commandName">The name of the command being handled.</param>
        /// <param name="commandArgs">The command's argument list.</param>
        /// <param name="commandHandler">The command's handler.</param>
        public StringFormatterCommandInfo(StringSegment commandName, 
            StringFormatterCommandArguments commandArgs, 
            StringFormatterCommandHandler commandHandler)
        {
            this.CommandName = commandName;
            this.CommandArguments = commandArgs;
            this.CommandHandler = commandHandler;
        }

        /// <summary>
        /// Gets the name of the command being handled.
        /// </summary>
        public StringSegment CommandName { get; }

        /// <summary>
        /// Gets the command's argument list.
        /// </summary>
        public StringFormatterCommandArguments CommandArguments { get; }

        /// <summary>
        /// Gets the command's handler.
        /// </summary>
        public StringFormatterCommandHandler CommandHandler { get; }
    }
}
