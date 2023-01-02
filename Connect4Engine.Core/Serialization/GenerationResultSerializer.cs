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
            var result = ImmutableDictionary.Create<UInt128, EvaluationInfo>();
            string[] entries = input.Split('\n');
            foreach (string entry in entries[1..])
            {
                if (string.IsNullOrEmpty(entry)) continue;
                string[] keyValue = entry.Split(':');
                string keyStr = keyValue[0];
                string valueStr = keyValue[1];
                string[] valueParts = valueStr.Split(',');
                uint value1 = uint.Parse(valueParts[0]);
                sbyte value2 = sbyte.Parse(valueParts[1]);
                result = result.Add(UInt128.Parse(keyStr), new(value1, value2));
            }
            return new(entries[0], result);
        }

        public string Serialize(GenerationResult input)
        {
            var builder = new StringBuilder(input.StartingPosition).Append('\n');
            foreach (var entry in input)
            {
                builder.Append(entry.Key.ToString());
                builder.Append(':');
                builder.Append(entry.Value.ExploredNodes);
                builder.Append(',');
                builder.Append(entry.Value.Score);
                builder.Append('\n');
            }
            return builder.ToString();
        }
    }
}
