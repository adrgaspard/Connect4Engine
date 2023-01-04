using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Engine.Core.Knowledge
{
    public record struct EvaluationInfo(ImmutableSortedSet<UInt128> Keys, uint ExploredNodes, sbyte Score);
}
