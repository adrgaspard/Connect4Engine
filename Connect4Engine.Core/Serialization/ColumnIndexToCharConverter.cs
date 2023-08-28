using Connect4Engine.Core.Abstractions;

namespace Connect4Engine.Core.Serialization
{
    public sealed class ColumnIndexToCharConverter : IConverter<byte, char>
    {
        public char Convert(byte value)
        {
            return value < 9
                ? (char)('0' + value)
                : value < 16
                ? (char)('A' + value - 10)
                : throw new ArgumentOutOfRangeException(nameof(value), "This converter does not support the values greater than 16.");
        }

        public byte ConvertBack(char value)
        {
            return value is >= '0' and <= '9'
                ? (byte)(value - '0')
                : value is >= 'A' and <= 'F'
                ? (byte)(value - 'A' + 10)
                : throw new ArgumentOutOfRangeException(nameof(value), "This converter does not support hexadecimal characters.");
        }
    }
}
