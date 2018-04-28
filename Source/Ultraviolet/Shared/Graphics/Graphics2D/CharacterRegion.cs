using System;
using System.Collections.Generic;
using System.Linq;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a run of characters which is included in a font face.
    /// </summary>
    public struct CharacterRegion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterRegion"/> structure.
        /// </summary>
        /// <param name="start">The first character in the region.</param>
        /// <param name="end">The last character in the region.</param>
        public CharacterRegion(Char start, Char end)
        {
            if (end < start)
            {
                throw new ArgumentOutOfRangeException("end");
            }
            this.start = start;
            this.end   = end;
        }

        /// <summary>
        /// Creates a collection of character regions which represent all of the characters
        /// in the specified source text.
        /// </summary>
        /// <param name="text">The source text from which to create character regions.</param>
        /// <returns>The collection of character regions that was created.</returns>
        public static IEnumerable<CharacterRegion> CreateFromSourceText(String text)
        {
            Contract.Require(text, nameof(text));

            var regions = new List<CharacterRegion>();
            var characters = (from c in text
                              where
                              !Char.IsSurrogate(c) && (!Char.IsWhiteSpace(c) || c == ' ')
                              select c).Distinct().OrderBy(x => x).ToArray();

            if (text != null && text.Length > 0)
            {
                var start = characters[0];
                for (var i = 0; i < characters.Length; i++)
                {
                    var next = (i + 1 >= characters.Length) ? (Char?)null : characters[i + 1];
                    var diff = (next.HasValue ? next.Value - characters[i] : Int32.MaxValue);
                    if (diff != 1)
                    {
                        var region = new CharacterRegion(start, characters[i]);
                        regions.Add(region);

                        if (i + 1 < characters.Length)
                        {
                            start = characters[i + 1];
                        }
                    }
                }
            }

            return regions.OrderBy(x => x.Start).ToList();
        }

        /// <inheritdoc/>
        public override String ToString() => $"{{Start:{Start} End:{End}}}";

        /// <summary>
        /// Gets a value indicating whether the region contains the specified character.
        /// </summary>
        /// <param name="c">The character to evaluate.</param>
        /// <returns><see langword="true"/> if the region contains the specified character; otherwise, <see langword="false"/>.</returns>
        public Boolean Contains(Char c)
        {
            return c >= start && c <= end;
        }

        /// <summary>
        /// Gets the default character region for English text.
        /// </summary>
        public static CharacterRegion Default
        {
            get { return new CharacterRegion(' ', '~'); }
        }

        /// <summary>
        /// Gets the first character in the region.
        /// </summary>
        public Char Start
        {
            get { return start; }
        }

        /// <summary>
        /// Gets the last character in the region.
        /// </summary>
        public Char End
        {
            get { return end; }
        }

        /// <summary>
        /// Gets the number of characters in the region.
        /// </summary>
        public Int32 Count
        {
            get { return 1 + end - start; }
        }

        // Property values.
        private Char start;
        private Char end;
    }
}
