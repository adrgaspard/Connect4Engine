using Connect4Engine.Core.Abstractions;
using Connect4Engine.Core.Knowledge;
using System.Collections.Immutable;
using System.Text;

namespace Connect4Engine.Core.Serialization
{
    public sealed class ExplorationResultSerializer : ISerializer<ExplorationResult>
    {
        public ExplorationResult Deserialize(string input)
        {
            ImmutableSortedDictionary<PositionMultiIdentifier, ImmutableSortedSet<UInt128>> result = ImmutableSortedDictionary.Create<PositionMultiIdentifier, ImmutableSortedSet<UInt128>>();
            string[] entries = input.Split('\n');
            foreach (string entry in entries[1..])
            {
                if (string.IsNullOrEmpty(entry))
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
                ImmutableSortedSet<UInt128> value = ImmutableSortedSet.CreateRange(valueElements.Select(UInt128.Parse));
                result = result.Add(key, value);
            }
            return new(entries[0], result);
        }

        public string Serialize(ExplorationResult input)
        {
            StringBuilder builder = new StringBuilder(input.StartingPosition).Append('\n');
            foreach (KeyValuePair<PositionMultiIdentifier, ImmutableSortedSet<UInt128>> entry in input)
            {
                _ = builder.Append(entry.Key.SymetricBase3Key.ToString());
                _ = builder.Append(',');
                _ = builder.Append(entry.Key.MoveRegistry);
                _ = builder.Append(':');
                _ = builder.Append(string.Join(",", entry.Value));
                _ = builder.Append('\n');
            }
            return builder.ToString();
        }
    }
}
