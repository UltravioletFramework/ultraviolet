using System;
using System.Text;

namespace Ultraviolet.Core.Text
{
    /// <summary>
    /// Represents an argument to a <see cref="StringFormatter"/> command.
    /// </summary>
    public struct StringFormatterCommandArgument :
        IEquatable<StringFormatterCommandArgument>,
        IEquatable<StringSegment>,
        IEquatable<String>,
        IEquatable<StringBuilder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringFormatterCommandArgument"/> structure.
        /// </summary>
        /// <param name="arglist">The argument list which produced this argument.</param>
        /// <param name="text">The argument's text.</param>
        internal StringFormatterCommandArgument(ref StringFormatterCommandArguments arglist, StringSegment text)
        {
            this.Text = text;
            this.ArgumentListStart = arglist.Start;
            this.ArgumentListLength = arglist.Length;
        }

        /// <summary>
        /// Determines whether a <see cref="StringFormatterCommandArgument"/> is equal to
        /// another <see cref="StringFormatterCommandArgument"/>.
        /// </summary>
        /// <param name="x">The first <see cref="StringFormatterCommandArgument"/> to compare.</param>
        /// <param name="y">The second <see cref="StringFormatterCommandArgument"/> to compare.</param>
        /// <returns><see langword="true"/> if the two objects are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(StringFormatterCommandArgument x, StringFormatterCommandArgument y)
        {
            return x.Equals(y);
        }

        /// <summary>
        /// Determines whether a <see cref="StringFormatterCommandArgument"/> is unequal to
        /// another <see cref="StringFormatterCommandArgument"/>.
        /// </summary>
        /// <param name="x">The first <see cref="StringFormatterCommandArgument"/> to compare.</param>
        /// <param name="y">The second <see cref="StringFormatterCommandArgument"/> to compare.</param>
        /// <returns><see langword="true"/> if the two objects are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(StringFormatterCommandArgument x, StringFormatterCommandArgument y)
        {
            return !x.Equals(y);
        }

        /// <summary>
        /// Determines whether a <see cref="StringFormatterCommandArgument"/> is equal to
        /// a <see cref="StringSegment"/>.
        /// </summary>
        /// <param name="x">The <see cref="StringFormatterCommandArgument"/> being compared.</param>
        /// <param name="y">The <see cref="StringSegment"/> being compared.</param>
        /// <returns><see langword="true"/> if the two objects are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(StringFormatterCommandArgument x, StringSegment y)
        {
            return x.Equals(y);
        }

        /// <summary>
        /// Determines whether a <see cref="StringFormatterCommandArgument"/> is unequal to
        /// a <see cref="StringSegment"/>.
        /// </summary>
        /// <param name="x">The <see cref="StringFormatterCommandArgument"/> being compared.</param>
        /// <param name="y">The <see cref="StringSegment"/> being compared.</param>
        /// <returns><see langword="true"/> if the two objects are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(StringFormatterCommandArgument x, StringSegment y)
        {
            return !x.Equals(y);
        }

        /// <summary>
        /// Determines whether a <see cref="StringSegment"/> is equal to
        /// a <see cref="StringFormatterCommandArgument"/>.
        /// </summary>
        /// <param name="x">The <see cref="StringSegment"/> being compared.</param>
        /// <param name="y">The <see cref="StringFormatterCommandArgument"/> being compared.</param>
        /// <returns><see langword="true"/> if the two objects are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(StringSegment x, StringFormatterCommandArgument y)
        {
            return y.Equals(x);
        }

        /// <summary>
        /// Determines whether a <see cref="StringSegment"/> is unequal to
        /// a <see cref="StringFormatterCommandArgument"/>.
        /// </summary>
        /// <param name="x">The <see cref="StringSegment"/> being compared.</param>
        /// <param name="y">The <see cref="StringFormatterCommandArgument"/> being compared.</param>
        /// <returns><see langword="true"/> if the two objects are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(StringSegment x, StringFormatterCommandArgument y)
        {
            return !y.Equals(x);
        }

        /// <summary>
        /// Determines whether a <see cref="StringFormatterCommandArgument"/> is equal to
        /// a <see cref="String"/>.
        /// </summary>
        /// <param name="x">The <see cref="StringFormatterCommandArgument"/> being compared.</param>
        /// <param name="y">The <see cref="String"/> being compared.</param>
        /// <returns><see langword="true"/> if the two objects are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(StringFormatterCommandArgument x, String y)
        {
            return x.Equals(y);
        }

        /// <summary>
        /// Determines whether a <see cref="StringFormatterCommandArgument"/> is unequal to
        /// a <see cref="String"/>.
        /// </summary>
        /// <param name="x">The <see cref="StringFormatterCommandArgument"/> being compared.</param>
        /// <param name="y">The <see cref="String"/> being compared.</param>
        /// <returns><see langword="true"/> if the two objects are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(StringFormatterCommandArgument x, String y)
        {
            return !x.Equals(y);
        }

        /// <summary>
        /// Determines whether a <see cref="String"/> is equal to
        /// a <see cref="StringFormatterCommandArgument"/>.
        /// </summary>
        /// <param name="x">The <see cref="String"/> being compared.</param>
        /// <param name="y">The <see cref="StringFormatterCommandArgument"/> being compared.</param>
        /// <returns><see langword="true"/> if the two objects are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(String x, StringFormatterCommandArgument y)
        {
            return y.Equals(x);
        }

        /// <summary>
        /// Determines whether a <see cref="String"/> is unequal to
        /// a <see cref="StringFormatterCommandArgument"/>.
        /// </summary>
        /// <param name="x">The <see cref="String"/> being compared.</param>
        /// <param name="y">The <see cref="StringFormatterCommandArgument"/> being compared.</param>
        /// <returns><see langword="true"/> if the two objects are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(String x, StringFormatterCommandArgument y)
        {
            return !y.Equals(x);
        }

        /// <summary>
        /// Determines whether a <see cref="StringFormatterCommandArgument"/> is equal to
        /// a <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="x">The <see cref="StringFormatterCommandArgument"/> being compared.</param>
        /// <param name="y">The <see cref="StringBuilder"/> being compared.</param>
        /// <returns><see langword="true"/> if the two objects are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(StringFormatterCommandArgument x, StringBuilder y)
        {
            return x.Equals(y);
        }

        /// <summary>
        /// Determines whether a <see cref="StringFormatterCommandArgument"/> is unequal to
        /// a <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="x">The <see cref="StringFormatterCommandArgument"/> being compared.</param>
        /// <param name="y">The <see cref="StringBuilder"/> being compared.</param>
        /// <returns><see langword="true"/> if the two objects are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(StringFormatterCommandArgument x, StringBuilder y)
        {
            return !x.Equals(y);
        }

        /// <summary>
        /// Determines whether a <see cref="StringBuilder"/> is equal to
        /// a <see cref="StringFormatterCommandArgument"/>.
        /// </summary>
        /// <param name="x">The <see cref="StringBuilder"/> being compared.</param>
        /// <param name="y">The <see cref="StringFormatterCommandArgument"/> being compared.</param>
        /// <returns><see langword="true"/> if the two objects are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(StringBuilder x, StringFormatterCommandArgument y)
        {
            return y.Equals(x);
        }

        /// <summary>
        /// Determines whether a <see cref="StringBuilder"/> is unequal to
        /// a <see cref="StringFormatterCommandArgument"/>.
        /// </summary>
        /// <param name="x">The <see cref="StringBuilder"/> being compared.</param>
        /// <param name="y">The <see cref="StringFormatterCommandArgument"/> being compared.</param>
        /// <returns><see langword="true"/> if the two objects are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(StringBuilder x, StringFormatterCommandArgument y)
        {
            return !y.Equals(x);
        }

        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 29 + Text.GetHashCode();
                hash = hash * 29 + ArgumentListStart.GetHashCode();
                hash = hash * 29 + ArgumentListLength.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc/>
        public override Boolean Equals(Object obj)
        {
            if (obj is StringFormatterCommandArgument)
                return Equals((StringFormatterCommandArgument)obj);

            if (obj is StringSegment)
                return Equals((StringSegment)obj);

            if (obj is String)
                return Equals((String)obj);

            if (obj is StringBuilder)
                return Equals((StringBuilder)obj);

            return false;
        }

        /// <inheritdoc/>
        public Boolean Equals(StringFormatterCommandArgument other)
        {
            return
                this.Text.Equals(other.Text) &&
                this.ArgumentListStart == other.ArgumentListStart &&
                this.ArgumentListLength == other.ArgumentListLength;
        }

        /// <inheritdoc/>
        public Boolean Equals(StringSegment other)
        {
            return Text.Equals(other);
        }

        /// <inheritdoc/>
        public Boolean Equals(String other)
        {
            return Text.Equals(other);
        }

        /// <inheritdoc/>
        public Boolean Equals(StringBuilder other)
        {
            return Text.Equals(other);
        }

        /// <summary>
        /// Retrieves the argument's value and converts it to an instance of <see cref="Byte"/>
        /// if possible, or throws an exception if the conversion is not possible.
        /// </summary>
        /// <returns>The converted value.</returns>
        public Byte GetValueAsByte()
        {
            var value = GetValueAsInt32();
            if (value < Byte.MinValue || value > Byte.MaxValue)
                throw new OverflowException();

            return (Byte)value;
        }

        /// <summary>
        /// Attempts to retrieve the argument's value and convert it to an 
        /// instance of <see cref="Byte"/> if possible.
        /// </summary>
        /// <param name="result">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public Boolean TryGetValueAsByte(out Int16 result)
        {
            result = 0;

            Int32 value;
            if (!TryGetValueAsInt32(out value))
                return false;

            if (value > Byte.MaxValue || value < Byte.MinValue)
                return false;

            result = (Byte)value;
            return true;
        }

        /// <summary>
        /// Retrieves the argument's value and converts it to an instance of <see cref="Int16"/>
        /// if possible, or throws an exception if the conversion is not possible.
        /// </summary>
        /// <returns>The converted value.</returns>
        public Int16 GetValueAsInt16()
        {
            var value = GetValueAsInt32();
            if (value < Int16.MinValue || value > Int16.MaxValue)
                throw new OverflowException();

            return (Int16)value;
        }

        /// <summary>
        /// Attempts to retrieve the argument's value and convert it to an 
        /// instance of <see cref="UInt16"/> if possible.
        /// </summary>
        /// <param name="result">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public Boolean TryGetValueAsInt16(out Int16 result)
        {
            result = 0;

            Int32 value;
            if (!TryGetValueAsInt32(out value))
                return false;

            if (value > Int16.MaxValue || value < Int16.MinValue)
                return false;

            result = (Int16)value;
            return true;
        }

        /// <summary>
        /// Retrieves the argument's value and converts it to an instance of <see cref="UInt16"/>
        /// if possible, or throws an exception if the conversion is not possible.
        /// </summary>
        /// <returns>The converted value.</returns>
        [CLSCompliant(false)]
        public UInt16 GetValueAsUInt16()
        {
            var value = GetValueAsInt32();
            if (value > UInt16.MaxValue)
                throw new OverflowException();

            return (UInt16)value;
        }

        /// <summary>
        /// Attempts to retrieve the argument's value and convert it to an 
        /// instance of <see cref="UInt16"/> if possible.
        /// </summary>
        /// <param name="result">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        [CLSCompliant(false)]
        public Boolean TryGetValueAsUInt16(out UInt16 result)
        {
            result = 0;

            Int32 value;
            if (!TryGetValueAsInt32(out value))
                return false;

            if (value < UInt16.MinValue)
                return false;

            result = (UInt16)value;
            return true;
        }

        /// <summary>
        /// Retrieves the argument's value and converts it to an instance of <see cref="Int32"/>
        /// if possible, or throws an exception if the conversion is not possible.
        /// </summary>
        /// <returns>The converted value.</returns>
        public Int32 GetValueAsInt32()
        {
            Int32 result;
            if (!TryGetValueAsInt32(out result))
                throw new FormatException();

            return result;
        }

        /// <summary>
        /// Attempts to retrieve the argument's value and convert it to an 
        /// instance of <see cref="Int32"/> if possible.
        /// </summary>
        /// <param name="result">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public Boolean TryGetValueAsInt32(out Int32 result)
        {
            return StringSegmentConversion.TryParseInt32(Text, out result);
        }

        /// <summary>
        /// Retrieves the argument's value and converts it to an instance of <see cref="UInt32"/>
        /// if possible, or throws an exception if the conversion is not possible.
        /// </summary>
        /// <returns>The converted value.</returns>
        [CLSCompliant(false)]
        public UInt32 GetValueAsUInt32()
        {
            UInt32 result;
            if (!TryGetValueAsUInt32(out result))
                throw new FormatException();

            return result;
        }

        /// <summary>
        /// Attempts to retrieve the argument's value and convert it to an 
        /// instance of <see cref="UInt32"/> if possible.
        /// </summary>
        /// <param name="result">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        [CLSCompliant(false)]
        public Boolean TryGetValueAsUInt32(out UInt32 result)
        {
            return StringSegmentConversion.TryParseUInt32(Text, out result);
        }

        /// <summary>
        /// Retrieves the argument's value and converts it to an instance of <see cref="Int64"/>
        /// if possible, or throws an exception if the conversion is not possible.
        /// </summary>
        /// <returns>The converted value.</returns>
        public Int64 GetValueAsInt64()
        {
            Int64 result;
            if (!TryGetValueAsInt64(out result))
                throw new FormatException();

            return result;
        }

        /// <summary>
        /// Attempts to retrieve the argument's value and convert it to an 
        /// instance of <see cref="Int64"/> if possible.
        /// </summary>
        /// <param name="result">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        public Boolean TryGetValueAsInt64(out Int64 result)
        {
            return StringSegmentConversion.TryParseInt64(Text, out result);
        }

        /// <summary>
        /// Retrieves the argument's value and converts it to an instance of <see cref="UInt64"/>
        /// if possible, or throws an exception if the conversion is not possible.
        /// </summary>
        /// <returns>The converted value.</returns>
        [CLSCompliant(false)]
        public UInt64 GetValueAsUInt64()
        {
            UInt64 result;
            if (!TryGetValueAsUInt64(out result))
                throw new FormatException();

            return result;
        }

        /// <summary>
        /// Attempts to retrieve the argument's value and convert it to an 
        /// instance of <see cref="UInt64"/> if possible.
        /// </summary>
        /// <param name="result">The converted value.</param>
        /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
        [CLSCompliant(false)]
        public Boolean TryGetValueAsUInt64(out UInt64 result)
        {
            return StringSegmentConversion.TryParseUInt64(Text, out result);
        }

        /// <summary>
        /// Gets the argument's text.
        /// </summary>
        public StringSegment Text { get; }

        /// <summary>
        /// Gets the argument's offset within its source string.
        /// </summary>
        public Int32 Start => Text.Start;

        /// <summary>
        /// Gets the argument's length.
        /// </summary>
        public Int32 Length => Text.Length;

        /// <summary>
        /// Gets the starting index of the argument list that produced this argument.
        /// </summary>
        public Int32 ArgumentListStart { get; }

        /// <summary>
        /// Gets the length of the argument list that produced this argument.
        /// </summary>
        public Int32 ArgumentListLength { get; }        
    }
}
