using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Engine.Core.Knowledge
{
    public sealed class ExplorationResult : IReadOnlyCollection<KeyValuePair<PositionMultiIdentifier, ImmutableSortedSet<UInt128>>>
    {
        public static ExplorationResult Empty = new();

        private readonly ImmutableSortedDictionary<PositionMultiIdentifier, ImmutableSortedSet<UInt128>> Data;
        public int Count => Data.Count;

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
    }
}
