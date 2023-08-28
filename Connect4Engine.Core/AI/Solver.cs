using Connect4Engine.Core.Abstractions;
using Connect4Engine.Core.Knowledge;
using Connect4Engine.Core.Operation;

namespace Connect4Engine.Core.AI
{
    public sealed class Solver
    {
        public readonly int ConnectNeeded;
        public readonly int Width;
        public readonly int Height;
        public readonly int MinScore;
        public readonly int MaxScore;
        private readonly TranspositionTable Table;
        private readonly byte[] ColumnOrder;
        private readonly object Mutex;
        private readonly OpeningTable OpeningTable;

        public uint ExploredNodesCount { get; private set; }

        public Solver(int connectNeeded, int width, int height, OpeningTable openingTable, int transpositionTableLogSize = 25)
        {
            if (width < 1 || height < 1)
            {
                throw new ArgumentOutOfRangeException(null, $"The board size is invalid ! The parameters {nameof(width)} and {nameof(height)} must be positives.");
            }
            ConnectNeeded = connectNeeded;
            Width = width;
            Height = height;
            MinScore = (-(Width * Height) / 2) + 3;
            MaxScore = (((Width * Height) + 1) / 2) - 3;
            Table = new(transpositionTableLogSize);
            ColumnOrder = new byte[width];
            for (int i = 0; i < width; i++)
            {
                ColumnOrder[i] = (byte)((width / 2) + ((1 - (2 * (i % 2))) * (i + 1) / 2));
            }
            Mutex = new();
            OpeningTable = openingTable;
        }

        public void Reset()
        {
            ExploredNodesCount = 0;
            Table.Reset();
        }

        public MovementDescriptor[] Analyze(GameEngine game, bool weak)
        {
            MovementDescriptor[] scores = new MovementDescriptor[Width];
            Task[] tasks = new Task[Width];
            uint tasksLaunched = 0;
            for (byte columnIndex = 0; columnIndex < Width; columnIndex++)
            {
                tasks[columnIndex] = Task.Run(() =>
                {
                    byte index;
                    lock (Mutex)
                    {
                        index = columnIndex;
                        tasksLaunched++;
                    }
                    GameEngine clone = (GameEngine)game.Clone();
                    if (clone.CanPlay(index))
                    {
                        if (clone.IsWinningMove(index))
                        {
                            scores[index] = new(true, (sbyte)((clone.RemainingMoves + 1) / 2));
                        }
                        else
                        {
                            clone.Play(index);
                            scores[index] = new(true, (sbyte)-Solve(clone, weak));
                        }
                    }
                    else
                    {
                        scores[index] = new(false, 0);
                    }
                });
                while (tasksLaunched <= columnIndex)
                {
                    ;
                }
            }
            Task.WaitAll(tasks);
            return scores;
        }

        public int Solve(GameEngine game, bool weak)
        {
            if (game.CanWinAtNextMove()) // AlphaBeta does not support this case.
            {
                return (game.RemainingMoves + 1) / 2;
            }
            int min = weak ? -1 : -game.RemainingMoves / 2;
            int max = weak ? 1 : (game.RemainingMoves + 1) / 2;
            while (min < max) // Iteratively narrow the min-max exploration window.
            {
                int median = min + ((max - min) / 2);
                if (median <= 0 && min / 2 < median)
                {
                    median = min / 2;
                }
                else if (median >= 0 && max / 2 > median)
                {
                    median = max / 2;
                }
                int result = AlphaBeta((GameEngine)game.Clone(), median, median + 1); // Use a null depth window to know if the actual score is greater or smaller than median.
                if (result <= median)
                {
                    max = result;
                }
                else
                {
                    min = result;
                }
            }
            return min;
        }

        private int AlphaBeta(GameEngine game, int alpha, int beta)
        {
#if DEBUG
            if (alpha >= beta)
            {
                throw new InvalidOperationException($"The parameter {nameof(alpha)} must be greater than {nameof(beta)}.");
            }
            if (game.CanWinAtNextMove())
            {
                throw new InvalidOperationException("The game can be won at next move, this method should not be called.");
            }
#endif
            ExploredNodesCount++;
            UInt128 possiblesMovements = game.GetPossiblesNonLosingMoves();
            if (possiblesMovements == 0)
            {
                return -game.RemainingMoves / 2; // Opponent wins next move.
            }
            if (game.RemainingMoves <= 2)
            {
                return 0; // Draw game.
            }
            int min = -(game.RemainingMoves - 2) / 2; // Lower bound of score as opponent cannot win next move.
            if (alpha < min)
            {
                alpha = min; // No need to keep alpha below the maximum possible score.
                if (alpha >= beta)
                {
                    return alpha; // Prune if [alpha;beta] is empty.
                }
            }
            int max = (game.RemainingMoves - 1) / 2; // Upper bound of score as current player cannot win immediately.
            if (beta > max)
            {
                beta = max; // No need to keep beta above the maximum possible score.
                if (alpha >= beta)
                {
                    return beta; // Prune if [alpha;beta] is empty.
                }
            }
            UInt128 key = game.CurrentKey;
            TableResult<byte> transpositionValue = Table[key];
            if (transpositionValue.Found)
            {
                if (transpositionValue > MaxScore - MinScore + 1)
                {  // Lower bound.
                    min = transpositionValue + (2 * MinScore) - MaxScore - 2;
                    if (alpha < min)
                    {
                        alpha = min; // No need to keep alpha below the maximum possible score.
                        if (alpha >= beta)
                        {
                            return alpha; // Prune if [alpha;beta] is empty.
                        }
                    }
                }
                else
                { // Upper bound.
                    max = transpositionValue + MinScore - 1;
                    if (beta > max)
                    {
                        beta = max; // No need to keep beta above the maximum possible score.
                        if (alpha >= beta)
                        {
                            return beta; // Prune if [alpha;beta] is empty.
                        }
                    }
                }
            }
            TableResult<sbyte> openingResult = OpeningTable[key];
            if (openingResult.Found)
            {
                return openingResult;
            }
            MoveSorter moveSorter = new(Width);
            for (int i = Width - 1; i >= 0; i--)
            {
                UInt128 move = possiblesMovements & game.ReadOnlyColumnMasks[ColumnOrder[i]];
                if (move != 0)
                {
                    moveSorter.Add(move, game.GetMoveScore(move));
                }
            }
            UInt128 next = moveSorter.GetNext();
            while (next != 0)
            {
                game.Play(next);
                int score = -AlphaBeta(game, -beta, -alpha); // Explore opponent's score within [-beta;-alpha] window.
                if (score >= beta)
                {
                    lock (Mutex)
                    {
                        Table.Put(key, (byte)(score + MaxScore - (2 * MinScore) + 2)); // Save the lower bound of the position.
                    }
                    game.Undo();
                    return score; // Prune if a better possible move is found.
                }
                if (score > alpha)
                {
                    alpha = score; // Reduce the [alpha;beta] window for next exploration.
                }
                game.Undo();
                next = moveSorter.GetNext();
            }
            lock (Mutex)
            {
                Table.Put(key, (byte)(alpha - MinScore + 1)); // Save the upper bound of the position.
            }
            return alpha;
        }
    }
}
