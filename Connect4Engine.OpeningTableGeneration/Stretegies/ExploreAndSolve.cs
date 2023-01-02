using Connect4Engine.Core.Knowledge;
using Connect4Engine.Core.Serialization;
using Connect4Engine.Core.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Engine.OpeningTableGeneration.Stretegies
{
    public class ExploreAndSolve
    {
        public static void Execute(string startingPosition = "")
        {
            Console.WriteLine($"Starting {nameof(ExploreAndSolve)} strategy!");
            Console.WriteLine("----------------------------------------");
            OpeningTableExplorator explorator = new();
            var positions = ExplorationResult.Empty;
            Task explore = Task.Run(() =>
            {
                positions = explorator.Explore(startingPosition, Global.ConnectedNeeded, Global.Width, Global.Height, Global.Depth);
            });
            DateTime exploreStart = DateTime.Now;
            while (explore.Status != TaskStatus.RanToCompletion && explore.Status != TaskStatus.Faulted && explore.Status != TaskStatus.Canceled)
            {
                Thread.Sleep(10000);
                var elapsed = (DateTime.Now - exploreStart).ToString(@"hh\:mm\:ss");
                switch (explorator.CurrentStep)
                {
                    case ExploratorStep.Initialization:
                        Console.WriteLine($"[{elapsed}] Explorator initialization...");
                        break;
                    case ExploratorStep.DiscoveringPotentialPositions:
                        Console.WriteLine($"[{elapsed}] Discovering potential positions... ({explorator.PotentialPositionsDiscovered} discovered)");
                        break;
                    case ExploratorStep.OrganizingPotentialPositions:
                        Console.WriteLine($"[{elapsed}] Filtering and sorting {explorator.PotentialPositionsDiscovered} potential positions...");
                        break;
                    case ExploratorStep.FetchingPositions:
                        Console.WriteLine($"[{elapsed}] Fetching positions... ({explorator.FetchedPositions}/{explorator.PositionsDiscovered} fetched)");
                        break;
                    case ExploratorStep.OptimizingFetchedPositions:
                        Console.WriteLine($"[{elapsed}] Optimizing {explorator.FetchedPositions} fetched positions...");
                        break;
                    case ExploratorStep.End:
                    case ExploratorStep.None:
                    default:
                        break;
                }
            }
            if (explore.Status != TaskStatus.RanToCompletion)
            {
                Console.Error.WriteLine("An error occured when exploring positions!");
                return;
            }
            Console.WriteLine($"[{DateTime.Now - exploreStart:hh\\:mm\\:ss}] Exploration finished, starting generation soon!");
            Console.WriteLine("----------------------------------------");
            OpeningTableGenerator generator = new();
            var results = GenerationResult.Empty;
            Task generate = Task.Run(() =>
            {
                results = generator.Generate(positions, Global.ConnectedNeeded, Global.Width, Global.Height, Global.Depth);
            });
            DateTime generateStart = DateTime.Now;
            while (generate.Status != TaskStatus.RanToCompletion && generate.Status != TaskStatus.Faulted && generate.Status != TaskStatus.Canceled)
            {
                Thread.Sleep(10000);
                var elapsed = (DateTime.Now - generateStart).ToString(@"hh\:mm\:ss");
                switch (generator.CurrentStep)
                {
                    case GeneratorStep.Initialization:
                        Console.WriteLine($"[{elapsed}] Generator initialization...");
                        break;
                    case GeneratorStep.SolvingPositions:
                        Console.WriteLine($"[{elapsed}] Solving positions... ({generator.SolvedPositions}/{positions.Count} solved)");
                        break;
                    case GeneratorStep.SortingResults:
                        Console.WriteLine($"[{elapsed}] Sorting {generator.SolvedPositions} results...");
                        break;
                    case GeneratorStep.End:
                    case GeneratorStep.None:
                    default:
                        break;
                }
            }
            if (generate.Status != TaskStatus.RanToCompletion)
            {
                Console.Error.WriteLine("An error occured when generating positions!");
                return;
            }
            Console.WriteLine($"[{DateTime.Now - generateStart:hh\\:mm\\:ss}] Generation finished, now it's time to write the results!");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Starting writing results...");
            using (var stream = new StreamWriter($"{Global.ConnectedNeeded}-{Global.Width}-{Global.Height}-{Global.Depth}_{startingPosition}_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.{Consts.DataFilesExtension}"))
            {
                stream.Write(new GenerationResultSerializer().Serialize(results));
            }
            Console.WriteLine("Done!");
        }
    }
}
