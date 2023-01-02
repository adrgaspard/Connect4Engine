using Connect4Engine.Core.Knowledge;
using Connect4Engine.Core.Serialization;
using Connect4Engine.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Engine.OpeningTableGeneration.Stretegies
{
    public static class SolveOnly
    {
        public static void Execute(ExplorationResult positions)
        {
            Console.WriteLine($"Starting {nameof(SolveOnly)} strategy!");
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
            using (var stream = new StreamWriter($"{Global.ConnectedNeeded}-{Global.Width}-{Global.Height}-{Global.Depth}_{positions.StartingPosition}_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.{Consts.DataFilesExtension}"))
            {
                stream.Write(new GenerationResultSerializer().Serialize(results));
            }
            Console.WriteLine("Done!");
        }
    }
}
