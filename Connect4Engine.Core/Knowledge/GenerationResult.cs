using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace Connect4Engine.Core.Knowledge
{
    public sealed class GenerationResult : IReadOnlyDictionary<PositionMultiIdentifier, EvaluationInfo>
    {
        public static readonly GenerationResult Empty = new();

        private readonly ImmutableDictionary<PositionMultiIdentifier, EvaluationInfo> Data;

        public int Count => Data.Count;

        public IEnumerable<PositionMultiIdentifier> Keys => Data.Keys;

        public IEnumerable<EvaluationInfo> Values => Data.Values;

        public EvaluationInfo this[PositionMultiIdentifier key] => Data[key];

        public string StartingPosition { get; private init; }

        public GenerationResult(string startingPosition, ImmutableDictionary<PositionMultiIdentifier, EvaluationInfo> data)
        {
            Data = data;
            StartingPosition = startingPosition;
        }

        private GenerationResult()
        {
            Data = Enumerable.Empty<KeyValuePair<PositionMultiIdentifier, EvaluationInfo>>().ToImmutableDictionary();
            StartingPosition = "";
        }

        public IEnumerator<KeyValuePair<PositionMultiIdentifier, EvaluationInfo>> GetEnumerator()
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

        public bool TryGetValue(PositionMultiIdentifier key, [MaybeNullWhen(false)] out EvaluationInfo value)
        {
            return TryGetValue(key, out value);
        }
    }
}
