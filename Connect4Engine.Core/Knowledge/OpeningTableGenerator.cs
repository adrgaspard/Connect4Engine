using Connect4Engine.Core.AI;
using Connect4Engine.Core.Match;
using Connect4Engine.Core.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Engine.Core.Knowledge
{
    public sealed class OpeningTableGenerator
    {
        private readonly ColumnIndexToCharConverter Converter;
        private readonly object ResultMutex;

        public GeneratorStep CurrentStep { get; private set; }

        public long SolvedPositions { get; private set; }

        public OpeningTableGenerator()
        {
            Converter = new();
            ResultMutex = new();
            CurrentStep = GeneratorStep.None;
            SolvedPositions = 0;
        }

        public void Reset()
        {
            if (CurrentStep != GeneratorStep.End)
            {
                CurrentStep = GeneratorStep.None;
                SolvedPositions = 0;
            }
            else
            {
                throw new InvalidOperationException("Can't reset while processing.");
            }
        }

        public GenerationResult Generate(ExplorationResult data, int connectNeeded, int width, int height, int nbThreads = 16)
        {
            if (nbThreads < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(nbThreads), "The number of threads must be a positive number.");
            }
            CurrentStep = GeneratorStep.Initialization;
            var results = new Dictionary<PositionMultiIdentifier, EvaluationInfo>(data.Count * 2);
            var chunks = data.Chunk((int)Math.Ceiling(data.Count / (float)nbThreads)).ToImmutableList();
            var tasks = new Task[nbThreads];
            for (int i = 0; i < nbThreads; i++)
            {
                bool ready = false;
                tasks[i] = Task.Run(() =>
                {
                    var chunk = chunks[i];
                    Solver solver = new(connectNeeded, width, height, new(ImmutableSortedDictionary.Create<UInt128, sbyte>()));
                    ready = true;
                    foreach (var pair in chunk)
                    {
                        GameEngine game = new(connectNeeded, width, height);
                        foreach (var columnIndex in pair.Key.MoveRegistry.Select(Converter.ConvertBack))
                        {
                            game.Play(columnIndex);
                        }
                        var score = (sbyte)solver.Solve(game, false);
                        lock (ResultMutex)
                        {
                            SolvedPositions++;
                            results.Add(pair.Key, new(pair.Value, solver.ExploredNodesCount, score));
                        }
                        solver.Reset();
                    }
                });
                while (!ready);
            }
            CurrentStep = GeneratorStep.SolvingPositions;
            Task.WaitAll(tasks);
            CurrentStep = GeneratorStep.SortingResults;
            var sortedResults = results.OrderBy(pair => pair.Key).ToImmutableDictionary();
            CurrentStep = GeneratorStep.End;
            return new(data.StartingPosition, sortedResults);
        }
    }
}
