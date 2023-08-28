using Connect4Engine.Core.Knowledge;
using Connect4Engine.Core.Serialization;
using Connect4Engine.Core.Utils;
using Connect4Engine.OpeningTableGeneration.Stretegies;

//ExploreOnly.Execute("");

//ExplorationResult positions;
//Console.WriteLine("Reading positions...");
//using (var stream = new StreamReader($"4-7-6-10__2023-01-03-01-06-52.{Consts.ExploreFilesExtension}"))
//{
//    positions = new ExplorationResultSerializer().Deserialize(stream.ReadToEnd());
//}
//Console.WriteLine($"{positions.Count} positions readed successfully!");
//Console.WriteLine("----------------------------------------");
//SolveOnly.Execute(positions);

//GenerationResult results;
//Console.WriteLine("Reading results...");
//using (var stream = new StreamReader($"4-7-6-10__2023-01-04-00-11-09.{Consts.DataFilesExtension}"))
//{
//    results = new GenerationResultSerializer().Deserialize(stream.ReadToEnd());
//}
//Console.WriteLine($"{results.Count} results readed successfully!");
//Console.WriteLine("----------------------------------------");
//AnalyzeOnly.Execute(results);

GenerationResult results;
Console.WriteLine("Reading results...");
using (StreamReader stream = new($"4-7-6-10__2023-01-04-00-11-09.{Consts.DataFilesExtension}"))
{
    results = new GenerationResultSerializer().Deserialize(stream.ReadToEnd());
}
Console.WriteLine($"{results.Count} results readed successfully!");
Console.WriteLine("----------------------------------------");
LintOnly.Execute(results);
