namespace Connect4Engine.Core.Knowledge
{
    public record struct PositionMultiIdentifier(UInt128 SymetricBase3Key, string MoveRegistry) : IComparable<PositionMultiIdentifier>
    {
        public readonly int CompareTo(PositionMultiIdentifier other)
        {
            return MoveRegistry.CompareTo(other.MoveRegistry);
        }
    }
}
