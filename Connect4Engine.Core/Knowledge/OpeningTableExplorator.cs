using Connect4Engine.Core.Operation;
using Connect4Engine.Core.Serialization;
using System.Collections.Immutable;

namespace Connect4Engine.Core.Knowledge
{
    public sealed class OpeningTableExplorator
    {

        private readonly ColumnIndexToCharConverter Converter;

        public ExploratorStep CurrentStep { get; private set; }

        public long PotentialPositionsDiscovered { get; private set; }

        public long PositionsDiscovered { get; private set; }

        public long FetchedPositions { get; private set; }

        public long OptimizedFetchedPositions { get; private set; }

        public OpeningTableExplorator()
        {
            Converter = new();
            CurrentStep = ExploratorStep.None;
            PotentialPositionsDiscovered = 0;
            PositionsDiscovered = 0;
            FetchedPositions = 0;
            OptimizedFetchedPositions = 0;
        }

        public void Reset()
        {
            if (CurrentStep == ExploratorStep.End)
            {
                CurrentStep = ExploratorStep.None;
                PotentialPositionsDiscovered = 0;
                PositionsDiscovered = 0;
                FetchedPositions = 0;
                OptimizedFetchedPositions = 0;
            }
            else
            {
                throw new InvalidOperationException("Can't reset while processing.");
            }
        }

        public ExplorationResult Explore(string startingPosition, int connectNeeded, int width, int height, int depth, int estimatedPositionSize = 300000000)
        {
            if (CurrentStep != ExploratorStep.None)
            {
                throw new InvalidOperationException("An explore operation has already been launched!");
            }
            CurrentStep = ExploratorStep.Initialization;
            if (depth < 1 || depth > width * height)
            {
                throw new ArgumentOutOfRangeException(nameof(depth), "The depth must be a positive number lower than the number of squares.");
            }
            GameEngine game = new(connectNeeded, width, height);
            foreach (byte columnIndex in startingPosition.Select(Converter.ConvertBack))
            {
                game.Play(columnIndex);
            }
            CurrentStep = ExploratorStep.DiscoveringPotentialPositions;
            List<UInt128>? potentialPositions = new(estimatedPositionSize);
            Discover(game, 0, depth, potentialPositions);
            CurrentStep = ExploratorStep.OrganizingPotentialPositions;
            SortedSet<UInt128>? positionKeys = new(potentialPositions.Distinct().Order());
            PositionsDiscovered = positionKeys.Count;
            potentialPositions = null;
            GC.Collect(2);
            CurrentStep = ExploratorStep.FetchingPositions;
            List<(string, UInt128, UInt128)>? positions = new((int)PositionsDiscovered / 2);
            Fetch(game, 0, depth, new char[depth], positionKeys, positions);
            positionKeys = null;
            GC.Collect(2);
            CurrentStep = ExploratorStep.OptimizingFetchedPositions;
            ImmutableSortedDictionary<PositionMultiIdentifier, ImmutableSortedSet<UInt128>> optimizedPositions = positions.GroupBy(tuple => tuple.Item3).ToImmutableSortedDictionary(grouping => new PositionMultiIdentifier(
                grouping.Key, grouping.First().Item1), grouping => grouping.Select(item => item.Item2).ToImmutableSortedSet());
            OptimizedFetchedPositions = optimizedPositions.Count;
            CurrentStep = ExploratorStep.End;
            return new(startingPosition, optimizedPositions);
        }

        private void Discover(GameEngine game, int currentDepth, int discoverDepth, List<UInt128> potentialPositions)
        {
            if (currentDepth < discoverDepth)
            {
                for (byte i = 0; i < game.Width; i++)
                {
                    if (game.CanPlay(i) && !game.IsWinningMove(i))
                    {
                        game.Play(i);
                        Discover(game, currentDepth + 1, discoverDepth, potentialPositions);
                        game.Undo();
                    }
                }
            }
            else
            {
                potentialPositions.Add(game.CurrentKey);
                PotentialPositionsDiscovered++;
            }
        }

        private void Fetch(GameEngine game, int currentDepth, int discoverDepth, char[] positionArray, SortedSet<UInt128> positionKeys, List<(string, UInt128, UInt128)> positions)
        {
            if (currentDepth < discoverDepth)
            {
                for (byte i = 0; i < game.Width; i++)
                {
                    if (game.CanPlay(i))
                    {
                        game.Play(i);
                        positionArray[currentDepth] = Converter.Convert(i);
                        Fetch(game, currentDepth + 1, discoverDepth, positionArray, positionKeys, positions);
                        game.Undo();
                    }
                }
            }
            else
            {
                if (positionKeys.Remove(game.CurrentKey))
                {
                    positions.Add((new(positionArray), game.CurrentKey, game.GetSymetricBase3Key()));
                    FetchedPositions++;
                }
            }
        }
    }
}
