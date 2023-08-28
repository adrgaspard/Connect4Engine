using Connect4Engine.Core.Knowledge;
using Connect4Engine.Core.Serialization;
using Connect4Engine.Core.Utils;
using System.Collections.Immutable;

namespace Connect4Engine.OpeningTableGeneration.Stretegies
{
    public static class LintOnly
    {
        public static void Execute(GenerationResult results)
        {
            Console.WriteLine("Creating opening table...");
            List<(UInt128, sbyte)> values = new(results.Count * 2);
            foreach (EvaluationInfo value in results.Values)
            {
                foreach (UInt128 positionKey in value.Keys)
                {
                    values.Add((positionKey, value.Score));
                }
            }
            OpeningTable openingTable = new(values.ToImmutableSortedDictionary(tuple => tuple.Item1, tuple => tuple.Item2, Comparer<UInt128>.Default));
            Console.WriteLine("Starting writing results...");
            using (StreamWriter stream = new($"{Global.ConnectedNeeded}-{Global.Width}-{Global.Height}-{Global.Depth}_{results.StartingPosition}_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.{Consts.KnowledgeFilesExtension}"))
            {
                stream.Write(new OpeningTableSerializer().Serialize(openingTable));
            }
            Console.WriteLine("Done!");
        }
    }
}
