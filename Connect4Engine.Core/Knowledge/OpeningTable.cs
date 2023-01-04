using Connect4Engine.Core.Abstractions;
using Connect4Engine.Core.Serialization.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Engine.Core.Knowledge
{
    public sealed class OpeningTable : IReadOnlyTable<UInt128, sbyte>
    {
        private KnowledgeDbContext Context;

        public TableResult<sbyte> this[UInt128 key] => Context.KnowledgeEntries.FirstOrDefault(entry => entry.PositionKey == key) is KnowledgeEntry found ? found.Score : TableResult<sbyte>.NotFound;

        public OpeningTable()
        {
            Context = new();
        }

        public void Reset()
        {
            Context?.Dispose();
            Context = new();
        }
    }
}
