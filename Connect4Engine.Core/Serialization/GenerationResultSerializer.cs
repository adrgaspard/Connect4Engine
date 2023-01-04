using Connect4Engine.Core.Abstractions;
using Connect4Engine.Core.Knowledge;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Engine.Core.Serialization
{
    public sealed class GenerationResultSerializer : ISerializer<GenerationResult>
    {
        public GenerationResult Deserialize(string input)
        {
            var result = ImmutableDictionary.Create<PositionMultiIdentifier, EvaluationInfo>();
            string[] entries = input.Split('\n');
            foreach (string entry in entries[1..])
            {
                if (string.IsNullOrWhiteSpace(entry)) continue;
                string[] keyValue = entry.Split(':');
                string keyStr = keyValue[0];
                string valueStr = keyValue[1];
                string[] keyParts = keyStr.Split(',');
                UInt128 key1 = UInt128.Parse(keyParts[0]);
                string key2 = keyParts[1];
                var key = new PositionMultiIdentifier(key1, key2);
                string[] valueElements = valueStr.Split(',');
                uint value1 = uint.Parse(valueElements[0]);
                sbyte value2 = sbyte.Parse(valueElements[1]);
                var value3 = ImmutableSortedSet.CreateRange(valueElements[2].Split(';').Select(UInt128.Parse));
                result = result.Add(key, new(value3, value1, value2));
            }
            return new(entries[0], result);
        }

        public string Serialize(GenerationResult input)
        {
            var builder = new StringBuilder(input.StartingPosition).Append('\n');
            foreach (var entry in input)
            {
                builder.Append(entry.Key.SymetricBase3Key.ToString());
                builder.Append(',');
                builder.Append(entry.Key.MoveRegistry);
                builder.Append(':');
                builder.Append(entry.Value.ExploredNodes);
                builder.Append(',');
                builder.Append(entry.Value.Score);
                builder.Append(',');
                builder.Append(string.Join(";", entry.Value.Keys.Select(key => key.ToString())));
                builder.Append('\n');
            }
            return builder.ToString();
        }
    }
}
