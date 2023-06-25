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
    public sealed class OpeningTableSerializer : ISerializer<OpeningTable>
    {
        public OpeningTable Deserialize(string input)
        {
            string[] entries = input.Split('\n');
            var pairs = new List<KeyValuePair<UInt128, sbyte>>(entries.Length);
            foreach (string entry in entries)
            {
                if (string.IsNullOrWhiteSpace(entry)) continue;
                string[] keyValue = entry.Split(':');
                UInt128 key = UInt128.Parse(keyValue[0]);
                sbyte value = sbyte.Parse(keyValue[1]);
                pairs.Add(new(key, value));
            }
            return new(pairs.ToImmutableSortedDictionary(pair => pair.Key, pair => pair.Value, Comparer<UInt128>.Default));
        }

        public string Serialize(OpeningTable input)
        {
            var builder = new StringBuilder();
            foreach (var pair in input)
            {
                builder.Append(pair.Key);
                builder.Append(':');
                builder.Append(pair.Value);
                builder.Append('\n');
            }
            return builder.ToString();
        }
    }
}
