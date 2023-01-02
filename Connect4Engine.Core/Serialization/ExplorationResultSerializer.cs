using Connect4Engine.Core.Abstractions;
using Connect4Engine.Core.Knowledge;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Engine.OpeningTableGeneration.Serialization
{
    public sealed class ExplorationResultSerializer : ISerializer<ExplorationResult>
    {
        public ExplorationResult Deserialize(string input)
        {
            var result = ImmutableSortedDictionary.Create<PositionMultiIdentifier, ImmutableSortedSet<UInt128>>();
            string[] entries = input.Split('\n');
            foreach (string entry in entries[1..])
            {
                if (string.IsNullOrEmpty(entry)) continue;
                string[] keyValue = entry.Split(':');
                string keyStr = keyValue[0];
                string valueStr = keyValue[1];
                string[] keyParts = keyStr.Split(',');
                UInt128 key1 = UInt128.Parse(keyParts[0]);
                string key2 = keyParts[1];
                var key = new PositionMultiIdentifier(key1, key2);
                string[] valueElements = valueStr.Split(',');
                var value = ImmutableSortedSet.CreateRange(valueElements.Select(x => UInt128.Parse(x)));
                result = result.Add(key, value);
            }
            return new(entries[0], result);
        }

        public string Serialize(ExplorationResult input)
        {
            var builder = new StringBuilder(input.StartingPosition).Append('\n');
            foreach (var entry in input)
            {
                builder.Append(entry.Key.SymetricBase3Key.ToString());
                builder.Append(',');
                builder.Append(entry.Key.MoveRegistry);
                builder.Append(':');
                builder.Append(string.Join(",", entry.Value));
                builder.Append('\n');
            }
            return builder.ToString();
        }
    }
}
