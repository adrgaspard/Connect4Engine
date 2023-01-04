using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Engine.Core.Knowledge
{
    public sealed class ExplorationResult : IReadOnlyDictionary<PositionMultiIdentifier, ImmutableSortedSet<UInt128>>
    {
        public readonly static ExplorationResult Empty = new();

        private readonly ImmutableSortedDictionary<PositionMultiIdentifier, ImmutableSortedSet<UInt128>> Data;

        public int Count => Data.Count;

        public IEnumerable<PositionMultiIdentifier> Keys => Data.Keys;

        public IEnumerable<ImmutableSortedSet<UInt128>> Values => Data.Values;

        public ImmutableSortedSet<UInt128> this[PositionMultiIdentifier key] => Data[key];

        public string StartingPosition { get; private init; }

        public ExplorationResult(string startingPosition, ImmutableSortedDictionary<PositionMultiIdentifier, ImmutableSortedSet<UInt128>> data)
        {
            Data = data;
            StartingPosition = startingPosition;
        }

        private ExplorationResult()
        {
            Data = Enumerable.Empty<KeyValuePair<PositionMultiIdentifier, ImmutableSortedSet<UInt128>>>().ToImmutableSortedDictionary();
            StartingPosition = "";
        }

        public IEnumerator<KeyValuePair<PositionMultiIdentifier, ImmutableSortedSet<UInt128>>> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        public bool ContainsKey(PositionMultiIdentifier key)
        {
            return Data.ContainsKey(key);
        }

        public bool TryGetValue(PositionMultiIdentifier key, [MaybeNullWhen(false)] out ImmutableSortedSet<UInt128> value)
        {
            return TryGetValue(key, out value);
        }
    }
}
