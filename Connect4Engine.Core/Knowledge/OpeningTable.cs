using Connect4Engine.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Engine.Core.Knowledge
{
    public sealed class OpeningTable : IReadOnlyTable<UInt128, byte>
    {
        private readonly Dictionary<string, string> Files;

        public TableResult<byte> this[UInt128 key] => throw new NotImplementedException();

        public OpeningTable()
        {
            Files = new();
            Reset();
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }
    }
}
