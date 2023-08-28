namespace Connect4Engine.Core.AI
{
    public sealed class MoveSorter
    {
        private int Size;
        private readonly MoveScore[] Entries;

        public MoveSorter(int width)
        {
            Size = 0;
            Entries = new MoveScore[width];
        }

        public void Add(UInt128 move, uint score)
        {
            int index = Size++;
            for (; index > 0 && Entries[index - 1].Score > score; --index)
            {
                Entries[index] = Entries[index - 1];
            }
            Entries[index] = new MoveScore(move, score);
        }

        public UInt128 GetNext()
        {
            return Size > 0 ? Entries[--Size].Move : 0;
        }

        public void Reset()
        {
            Size = 0;
        }

        private record struct MoveScore(UInt128 Move, uint Score);
    }
}
