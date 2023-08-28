using Connect4Engine.Core.Abstractions;
using Connect4Engine.Core.Knowledge;
using System.Collections.Immutable;
using System.Text;

namespace Connect4Engine.Core.Serialization
{
    public sealed class OpeningTableSerializer : ISerializer<OpeningTable>
    {
        public OpeningTable Deserialize(string input)
        {
            string[] entries = input.Split('\n');
            List<KeyValuePair<UInt128, sbyte>> pairs = new(entries.Length);
            foreach (string entry in entries)
            {
                if (string.IsNullOrWhiteSpace(entry))
                {
                    continue;
                }

                string[] keyValue = entry.Split(':');
                UInt128 key = UInt128.Parse(keyValue[0]);
                sbyte value = sbyte.Parse(keyValue[1]);
                pairs.Add(new(key, value));
            }
            return new(pairs.ToImmutableSortedDictionary(pair => pair.Key, pair => pair.Value, Comparer<UInt128>.Default));
        }

        public string Serialize(OpeningTable input)
        {
            StringBuilder builder = new();
            foreach (KeyValuePair<UInt128, sbyte> pair in input)
            {
                _ = builder.Append(pair.Key);
                _ = builder.Append(':');
                _ = builder.Append(pair.Value);
                _ = builder.Append('\n');
            }
            return builder.ToString();
        }
    }
}
