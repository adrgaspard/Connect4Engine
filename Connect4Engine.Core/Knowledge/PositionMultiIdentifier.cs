using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Engine.Core.Knowledge
{
    public record struct PositionMultiIdentifier(UInt128 SymetricBase3Key, string MoveRegistry) : IComparable<PositionMultiIdentifier>
    {
        public int CompareTo(PositionMultiIdentifier other)
        {
            return MoveRegistry.CompareTo(other.MoveRegistry);
        }
    }
}
