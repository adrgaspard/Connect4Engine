using System.Collections.Immutable;

namespace Connect4Engine.Core.Knowledge
{
    public record struct EvaluationInfo(ImmutableSortedSet<UInt128> Keys, uint ExploredNodes, sbyte Score);
}
