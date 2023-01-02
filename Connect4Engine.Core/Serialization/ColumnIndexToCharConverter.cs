using Connect4Engine.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Engine.Core.Serialization
{
    public sealed class ColumnIndexToCharConverter : IConverter<byte, char>
    {
        public char Convert(byte value)
        {
            if (value < 9)
            {
                return (char)('0' + value);
            }
            if (value < 16)
            {
                return (char)('A' + value - 10);
            }
            throw new ArgumentOutOfRangeException(nameof(value), "This converter does not support the values greater than 16.");
        }

        public byte ConvertBack(char value)
        {
            if (value >= '0' && value <= '9')
            {
                return (byte)(value - '0');
            }
            if (value >= 'A' && value <= 'F')
            {
                return (byte)(value - 'A' + 10);
            }
            throw new ArgumentOutOfRangeException(nameof(value), "This converter does not support hexadecimal characters.");
        }
    }
}
