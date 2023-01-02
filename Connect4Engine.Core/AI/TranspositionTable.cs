using Connect4Engine.Core.Abstractions;
using Connect4Engine.Core.Utils;

namespace Connect4Engine.Core.AI
{
    public sealed class TranspositionTable : ITable<UInt128, byte>
    {
        private readonly ulong Size;
        private readonly UInt128[] Keys;
        private readonly byte[] Values;

        public TableResult<byte> this[UInt128 key]
        {
            get
            {
                ulong index = (ulong)(key % Size);
                return Keys[index] == key ? Values[index] : TableResult<byte>.NotFound;
            }
        }

        public TranspositionTable(int logSize)
        {
            if (logSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(logSize), $"The parameter {nameof(logSize)} must be positive.");
            }
            Size = (1UL << logSize).NextPrime();
            Keys = new UInt128[Size];
            Values = new byte[Size];
            Reset();
        }

        public void Put(UInt128 key, byte value)
        {
            ulong index = (ulong)(key % Size);
            Keys[index] = key;
            Values[index] = value;
        }

        public void Reset()
        {
            Array.Clear(Keys);
            Array.Clear(Values);
        }
    }
}
