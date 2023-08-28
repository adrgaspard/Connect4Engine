using Connect4Engine.Core.Utils;
using System.Collections.ObjectModel;

namespace Connect4Engine.Core.Operation
{
    public class GameEngine : ICloneable
    {
        private static readonly Dictionary<(int, int), List<UInt128>> TopColumnMasksRegistry = new();
        private static readonly Dictionary<(int, int), List<UInt128>> BottomColumnMasksRegistry = new();
        private static readonly Dictionary<(int, int), List<UInt128>> ColumnMasksRegistry = new();

        private readonly Stack<UInt128> PlaysHistory;

        private readonly UInt128 BottomMask;
        private readonly UInt128 BoardMask;
        private readonly List<UInt128> TopColumnMasks;
        private readonly List<UInt128> BottomColumnMasks;
        private readonly List<UInt128> ColumnMasks;
        public readonly IReadOnlyList<UInt128> ReadOnlyColumnMasks;

        public readonly int ConnectNeeded;
        public readonly int Width;
        public readonly int Height;

        public UInt128 CurrentPawns { get; private set; }

        public UInt128 AllPawns { get; private set; }

        public UInt128 CurrentKey => AllPawns + CurrentPawns;

        public int RemainingMoves { get; private set; }

        public GameEngine(int connectNeeded, int width, int height)
        {
            if (connectNeeded < 3)
            {
                throw new ArgumentOutOfRangeException(nameof(connectNeeded), $"The connect needed must be equal or greater than 3.");
            }
            if (width < 1 || height < 1)
            {
                throw new ArgumentOutOfRangeException(null, $"The board size is invalid ! The parameters {nameof(width)} and {nameof(height)} must be positives.");
            }
            if (connectNeeded > width || connectNeeded > height)
            {
                throw new ArgumentOutOfRangeException(nameof(connectNeeded), "The connect needed is too high for the board size.");
            }
            if (width * (height + 1) > 128)
            {
                throw new ArgumentOutOfRangeException(null, $"The board size is too high ! {nameof(width)} * ({nameof(height)} + 1) must be equal or lower to 128.");
            }
            ConnectNeeded = connectNeeded;
            Width = width;
            Height = height;
            RemainingMoves = Width * Height;
            CurrentPawns = 0;
            AllPawns = 0;
            TopColumnMasks = GetOrInitializeTopMasks();
            BottomColumnMasks = GetOrInitializeBottomMasks();
            ColumnMasks = GetOrInitializeColumnMasks();
            ReadOnlyColumnMasks = new ReadOnlyCollection<UInt128>(ColumnMasks);
            BottomMask = GetBottomMask(width, height);
            BoardMask = BottomMask * ((UInt128.One << Height) - 1);
            PlaysHistory = new(Width * Height);
        }

        private GameEngine(Stack<UInt128> playsHistory, int connectNeeded, int width, int height, UInt128 bottomMask, UInt128 boardMask, List<UInt128> topColumnMasks,
            List<UInt128> bottomColumnMasks, List<UInt128> columnMasks, IReadOnlyList<UInt128> readOnlyColumnMasks, UInt128 currentPawns,
            UInt128 allPawns, int remainingMoves)
        {
            PlaysHistory = new(playsHistory.Reverse());
            ConnectNeeded = connectNeeded;
            Width = width;
            Height = height;
            BottomMask = bottomMask;
            BoardMask = boardMask;
            TopColumnMasks = topColumnMasks;
            BottomColumnMasks = bottomColumnMasks;
            ColumnMasks = columnMasks;
            ReadOnlyColumnMasks = readOnlyColumnMasks;
            CurrentPawns = currentPawns;
            AllPawns = allPawns;
            RemainingMoves = remainingMoves;

        }

        public bool CanPlay(byte columnIndex)
        {
            return (AllPawns & TopColumnMasks[columnIndex]) == 0;
        }

        public bool CanUndo()
        {
            return PlaysHistory.Count > 0;
        }

        public void Play(byte columnIndex)
        {
            Play((AllPawns + BottomColumnMasks[columnIndex]) & ColumnMasks[columnIndex]);
        }

        public void Undo()
        {
            UInt128 move = PlaysHistory.Pop();
            AllPawns &= ~move;
            CurrentPawns ^= AllPawns;
            RemainingMoves++;
        }

        public void Play(UInt128 move)
        {
            PlaysHistory.Push(move);
            CurrentPawns ^= AllPawns;
            AllPawns |= move;
            RemainingMoves--;
        }

        public bool CanWinAtNextMove()
        {
            return (GetCurrentWinningSpots() & GetPossiblesMoves()) != 0;
        }

        public UInt128 GetPossiblesNonLosingMoves()
        {
#if DEBUG
            if (CanWinAtNextMove())
            {
                throw new InvalidOperationException($"This method will fail if you have a winning move.");
            }
#endif
            UInt128 possibles = GetPossiblesMoves();
            UInt128 opponentWins = GetOpponentWinningSpots();
            UInt128 forcedMoves = possibles & opponentWins;
            if (forcedMoves != 0)
            {
                if ((forcedMoves & (forcedMoves - 1)) != 0)
                {
                    return 0;
                }
                else
                {
                    possibles = forcedMoves;
                }
            }
            return possibles & ~(opponentWins >> 1);
        }

        public uint GetMoveScore(UInt128 move)
        {
            return ComputeWinningSpots(CurrentPawns | move, AllPawns).OneBitCount();
        }

        public bool IsWinningMove(byte columnIndex)
        {
            return (GetCurrentWinningSpots() & GetPossiblesMoves() & ColumnMasks[columnIndex]) != 0;
        }

        public object Clone()
        {
            return new GameEngine(PlaysHistory, ConnectNeeded, Width, Height, BottomMask, BoardMask, TopColumnMasks, BottomColumnMasks,
                ColumnMasks, ReadOnlyColumnMasks, CurrentPawns, AllPawns, RemainingMoves);
        }

        public UInt128 GetSymetricBase3Key()
        {
            UInt128 keyForward = 0;
            for (int i = 0; i < Width; i++)
            {
                AssemblePartialSymetricBase3Key(ref keyForward, (byte)i);
            }

            UInt128 keyReverse = 0;
            for (int i = Width - 1; i >= 0; i--)
            {
                AssemblePartialSymetricBase3Key(ref keyReverse, (byte)i);
            }

            return keyForward < keyReverse ? keyForward / 3 : keyReverse / 3;
        }

        private void AssemblePartialSymetricBase3Key(ref UInt128 key, byte columnIndex)
        {
            UInt128 pos = UInt128.One << (columnIndex * (Height + 1));
            while ((pos & AllPawns) != 0)
            {
                key *= 3;
                if ((pos & CurrentPawns) != 0)
                {
                    key += 1;
                }
                else
                {
                    key += 2;
                }
                pos <<= 1;
            }
            key *= 3;
        }

        private UInt128 GetPossiblesMoves()
        {
            return (AllPawns + BottomMask) & BoardMask;
        }

        // TODO : Généraliser la fonction pour détecter la puissance requise et pas juste 4.
        private UInt128 ComputeWinningSpots(UInt128 position, UInt128 mask)
        {
            // Vertical checking.
            UInt128 r = (position << 1) & (position << 2) & (position << 3);
            // Horizontal checking.
            UInt128 p = (position << (Height + 1)) & (position << (2 * (Height + 1)));
            r |= p & (position << (3 * (Height + 1)));
            r |= p & (position >> (Height + 1));
            p = (position >> (Height + 1)) & (position >> (2 * (Height + 1)));
            r |= p & (position << (Height + 1));
            r |= p & (position >> (3 * (Height + 1)));
            // Diagonal 1 checking.
            p = (position << Height) & (position << (2 * Height));
            r |= p & (position << (3 * Height));
            r |= p & (position >> Height);
            p = (position >> Height) & (position >> (2 * Height));
            r |= p & (position << Height);
            r |= p & (position >> (3 * Height));
            // Diagonal 2 checking.
            p = (position << (Height + 2)) & (position << (2 * (Height + 2)));
            r |= p & (position << (3 * (Height + 2)));
            r |= p & (position >> (Height + 2));
            p = (position >> (Height + 2)) & (position >> (2 * (Height + 2)));
            r |= p & (position << (Height + 2));
            r |= p & (position >> (3 * (Height + 2)));
            // Returning bitmap of all spots for a winning position.
            return r & (BoardMask ^ mask);
        }

        private UInt128 GetOpponentWinningSpots()
        {
            return ComputeWinningSpots(CurrentPawns ^ AllPawns, AllPawns);
        }

        private UInt128 GetCurrentWinningSpots()
        {
            return ComputeWinningSpots(CurrentPawns, AllPawns);
        }

        private UInt128 GetBottomMask(int width, int height)
        {
            return width == 0 ? 0 : GetBottomMask(width - 1, height) | (UInt128.One << ((width - 1) * (height + 1)));
        }

        private List<UInt128> GetOrInitializeTopMasks()
        {
            return GetOrInitializeCustomMasks(TopColumnMasksRegistry, Height - 1, Height - 1);
        }

        private List<UInt128> GetOrInitializeBottomMasks()
        {
            return GetOrInitializeCustomMasks(BottomColumnMasksRegistry, 0, 0);
        }

        private List<UInt128> GetOrInitializeColumnMasks()
        {
            return GetOrInitializeCustomMasks(ColumnMasksRegistry, 0, Height - 1);
        }

        private List<UInt128> GetOrInitializeCustomMasks(Dictionary<(int, int), List<UInt128>> masks, int startingRow, int endingRow)
        {
            if (masks.TryGetValue((Width, Height), out List<UInt128>? foundMasks))
            {
                return foundMasks;
            }
            List<UInt128> result = new(Width);
            for (int columnIndex = 0; columnIndex < Width; columnIndex++)
            {
                UInt128 columnMask = 0;
                for (int rowIndex = startingRow; rowIndex <= endingRow; rowIndex++)
                {
                    columnMask |= UInt128.One << (rowIndex + (columnIndex * (Height + 1)));
                }
                result.Add(columnMask);
            }
            masks[(Width, Height)] = result;
            return result;
        }
    }
}
