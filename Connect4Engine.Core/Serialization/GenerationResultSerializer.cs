using Connect4Engine.Core.Abstractions;
using Connect4Engine.Core.Knowledge;
using System.Collections.Immutable;
using System.Text;

namespace Connect4Engine.Core.Serialization
{
    public sealed class GenerationResultSerializer : ISerializer<GenerationResult>
    {
        public GenerationResult Deserialize(string input)
        {
            ImmutableDictionary<PositionMultiIdentifier, EvaluationInfo> result = ImmutableDictionary.Create<PositionMultiIdentifier, EvaluationInfo>();
            string[] entries = input.Split('\n');
            foreach (string entry in entries[1..])
            {
                if (string.IsNullOrWhiteSpace(entry))
                {
                    continue;
                }

                string[] keyValue = entry.Split(':');
                string keyStr = keyValue[0];
                string valueStr = keyValue[1];
                string[] keyParts = keyStr.Split(',');
                UInt128 key1 = UInt128.Parse(keyParts[0]);
                string key2 = keyParts[1];
                PositionMultiIdentifier key = new(key1, key2);
                string[] valueElements = valueStr.Split(',');
                uint value1 = uint.Parse(valueElements[0]);
                sbyte value2 = sbyte.Parse(valueElements[1]);
                ImmutableSortedSet<UInt128> value3 = ImmutableSortedSet.CreateRange(valueElements[2].Split(';').Select(UInt128.Parse));
                result = result.Add(key, new(value3, value1, value2));
            }
            return new(entries[0], result);
        }

        public string Serialize(GenerationResult input)
        {
            StringBuilder builder = new StringBuilder(input.StartingPosition).Append('\n');
            foreach (KeyValuePair<PositionMultiIdentifier, EvaluationInfo> entry in input)
            {
                _ = builder.Append(entry.Key.SymetricBase3Key.ToString());
                _ = builder.Append(',');
                _ = builder.Append(entry.Key.MoveRegistry);
                _ = builder.Append(':');
                _ = builder.Append(entry.Value.ExploredNodes);
                _ = builder.Append(',');
                _ = builder.Append(entry.Value.Score);
                _ = builder.Append(',');
                _ = builder.Append(string.Join(";", entry.Value.Keys.Select(key => key.ToString())));
                _ = builder.Append('\n');
            }
            return builder.ToString();
        }
    }
}
