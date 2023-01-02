using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Engine.Core.Knowledge
{
    public sealed class KnowledgeEntry : IEquatable<KnowledgeEntry>
    {
        public ulong PartialPositionKey1 { get; set; }

        public ulong PartialPositionKey2 { get; set; }

        public sbyte Score { get; set; }

        [NotMapped]
        public UInt128 PositionKey
        {
            get => (PartialPositionKey1 << 64) | PartialPositionKey2;
            set
            {
                PartialPositionKey1 = (ulong)(value >> 64);
                PartialPositionKey2 = (ulong)value;
            }
        }

        public bool Equals(KnowledgeEntry? other)
        {
            return other is not null && PositionKey == other.PositionKey;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as KnowledgeEntry);
        }

        public override int GetHashCode()
        {
            return PositionKey.GetHashCode();
        }
    }
}
