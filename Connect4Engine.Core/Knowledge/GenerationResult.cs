using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Engine.Core.Knowledge
{
    public sealed class GenerationResult : IReadOnlyCollection<KeyValuePair<UInt128, EvaluationInfo>>
    {
        public static readonly GenerationResult Empty = new();

        private readonly ImmutableDictionary<UInt128, EvaluationInfo> Data;

        public int Count => Data.Count;

        public string StartingPosition { get; private init; }

        public GenerationResult(string startingPosition, ImmutableDictionary<UInt128, EvaluationInfo> data)
        {
            Data = data;
            StartingPosition = startingPosition;
        }

        private GenerationResult()
        {
            Data = Enumerable.Empty<KeyValuePair<UInt128, EvaluationInfo>>().ToImmutableDictionary();
            StartingPosition = "";
        }

        public IEnumerator<KeyValuePair<UInt128, EvaluationInfo>> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Data.GetEnumerator();
        }
    }
}
