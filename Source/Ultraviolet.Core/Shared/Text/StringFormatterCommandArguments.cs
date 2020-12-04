using System;

namespace Ultraviolet.Core.Text
{
    /// <summary>
    /// Represents the collection of arguments which have been passed to a <see cref="StringFormatter"/> command.
    /// </summary>
    public struct StringFormatterCommandArguments
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringFormatterCommandArguments"/> structure.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="start">The starting position of the command within the input string.</param>
        /// <param name="length">The number of characters to examine in the input string.</param>
        internal StringFormatterCommandArguments(String input, Int32 start, Int32 length)
        {
            Input = input;
            Start = start;
            Length = length;

            var argcount = 0;

            if (Length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    var c = input[start + i];

                    if (c == '}')
                        break;

                    if (c == ':')
                        argcount++;
                }

                Count = argcount + 1;
            }
            else
            {
                Count = 0;
            }
        }

        /// <inheritdoc/>
        public override String ToString() => Input?.Substring(Start, Length);

        /// <summary>
        /// Returns a new instance of the <see cref="StringFormatterCommandArguments"/> structure which
        /// contains the same arguments as this structure, but with the first <paramref name="count"/>
        /// arguments discarded.
        /// </summary>
        /// <param name="count">The number of arguments to discard.</param>
        /// <returns>The <see cref="StringFormatterCommandArguments"/> structure that was created.</returns>
        public StringFormatterCommandArguments Discard(Int32 count)
        {
            if (count < 0 || count > Count)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0)
                return this;

            if (count == Count)
                return new StringFormatterCommandArguments(Input, Start, 0);

            var argcount = 0;

            for (int i = 0; i < Length; i++)
            {
                var c = Input[Start + i];
                if (c == ':')
                {
                    argcount++;

                    if (argcount == count)
                    {
                        var lengthdiff = (i + 1);
                        return new StringFormatterCommandArguments(Input, Start + lengthdiff, Length - lengthdiff);
                    }
                }
            }

            throw new ArgumentOutOfRangeException(nameof(count));
        }

        /// <summary>
        /// Retrieves the argument with the specified index.
        /// </summary>
        /// <param name="index">The index of the argument to retrieve.</param>
        /// <returns>The argument with the specified index.</returns>
        public StringFormatterCommandArgument GetArgument(Int32 index)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            return GetArgumentInternal(0, index);
        }

        /// <summary>
        /// Retrieves the argument that comes immediately after the specified argument. 
        /// </summary>
        /// <param name="previousArgument">The argument which comes immediatley before the argument to retrieve.</param>
        /// <returns>The argument with the specified index.</returns>
        public StringFormatterCommandArgument GetNextArgument(ref StringFormatterCommandArgument previousArgument)
        {
            if (previousArgument.Text.SourceString != Input)
                throw new ArgumentException(nameof(previousArgument));

            if (previousArgument.ArgumentListStart != Start || previousArgument.ArgumentListLength != Length)
                throw new ArgumentException(nameof(previousArgument));

            var searchStartOffset = 1 + ((previousArgument.Start + previousArgument.Length) - Start);
            if (searchStartOffset >= Length)
                throw new ArgumentException(nameof(previousArgument));

            return GetArgumentInternal(searchStartOffset, 0);
        }

        /// <summary>
        /// Gets the input string from which the argument list was created.
        /// </summary>
        public String Input { get; }

        /// <summary>
        /// Gets the argument list's offset within the input string.
        /// </summary>
        public Int32 Start { get; }
        
        /// <summary>
        /// Gets the argument list's length within the input string.
        /// </summary>
        public Int32 Length { get; }

        /// <summary>
        /// Gets the number of arguments in the collection.
        /// </summary>
        public Int32 Count { get; }

        /// <summary>
        /// Searches the argument list for the specified argument.
        /// </summary>
        private StringFormatterCommandArgument GetArgumentInternal(Int32 argumentSearchStartOffset, Int32 argumentRelativeIndex)
        {
            var searchStart = Start + argumentSearchStartOffset;

            var argCount = 0;
            var argStart = searchStart;
            var argLength = 0;

            for (int i = argumentSearchStartOffset; i < Length; i++)
            {
                var c = Input[Start + i];

                if (c == '}')
                    break;

                if (c == ':')
                {
                    if (argCount == argumentRelativeIndex)
                    {
                        return new StringFormatterCommandArgument(ref this,
                            new StringSegment(Input, argStart, argLength));
                    }

                    argCount++;
                    argStart = searchStart + i + 1;
                    argLength = 0;

                    continue;
                }

                argLength++;
            }

            return new StringFormatterCommandArgument(ref this,
                new StringSegment(Input, argStart, argLength));
        }
    }
}
