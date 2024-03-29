﻿using Connect4Engine.Core.AI;
using Connect4Engine.Core.Operation;
using Connect4Engine.Core.Serialization;
using System.Collections.Immutable;

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
            Dictionary<PositionMultiIdentifier, EvaluationInfo> results = new(data.Count * 2);
            ImmutableList<KeyValuePair<PositionMultiIdentifier, ImmutableSortedSet<UInt128>>[]> chunks = data.Chunk((int)Math.Ceiling(data.Count / (float)nbThreads)).ToImmutableList();
            Task[] tasks = new Task[nbThreads];
            for (int i = 0; i < nbThreads; i++)
            {
                bool ready = false;
                tasks[i] = Task.Run(() =>
                {
                    KeyValuePair<PositionMultiIdentifier, ImmutableSortedSet<UInt128>>[] chunk = chunks[i];
                    Solver solver = new(connectNeeded, width, height, new(ImmutableSortedDictionary.Create<UInt128, sbyte>()));
                    ready = true;
                    foreach (KeyValuePair<PositionMultiIdentifier, ImmutableSortedSet<UInt128>> pair in chunk)
                    {
                        GameEngine game = new(connectNeeded, width, height);
                        foreach (byte columnIndex in pair.Key.MoveRegistry.Select(Converter.ConvertBack))
                        {
                            game.Play(columnIndex);
                        }
                        sbyte score = (sbyte)solver.Solve(game, false);
                        lock (ResultMutex)
                        {
                            SolvedPositions++;
                            results.Add(pair.Key, new(pair.Value, solver.ExploredNodesCount, score));
                        }
                        solver.Reset();
                    }
                });
                while (!ready)
                {
                    ;
                }
            }
            CurrentStep = GeneratorStep.SolvingPositions;
            Task.WaitAll(tasks);
            CurrentStep = GeneratorStep.SortingResults;
            ImmutableDictionary<PositionMultiIdentifier, EvaluationInfo> sortedResults = results.OrderBy(pair => pair.Key).ToImmutableDictionary();
            CurrentStep = GeneratorStep.End;
            return new(data.StartingPosition, sortedResults);
        }
    }
}
