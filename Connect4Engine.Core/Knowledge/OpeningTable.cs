using Connect4Engine.Core.Abstractions;
using System.Collections;
using System.Collections.Immutable;

namespace Connect4Engine.Core.Knowledge
{
    public sealed class OpeningTable : IReadOnlyTable<UInt128, sbyte>, IReadOnlyCollection<KeyValuePair<UInt128, sbyte>>
    {
        private readonly ImmutableSortedDictionary<UInt128, sbyte> Data;

        public int Count => Data.Count;

        public TableResult<sbyte> this[UInt128 key] => Data.TryGetValue(key, out sbyte value) ? value : TableResult<sbyte>.NotFound;

        public OpeningTable(ImmutableSortedDictionary<UInt128, sbyte> data)
        {
            Data = data;
        }

        public void Reset()
        {
        }

        public IEnumerator<KeyValuePair<UInt128, sbyte>> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Data.GetEnumerator();
        }
    }
}
