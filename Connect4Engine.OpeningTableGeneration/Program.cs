using Connect4Engine.Core.Knowledge;
using Connect4Engine.Core.Utils;
using Connect4Engine.OpeningTableGeneration.Serialization;
using Connect4Engine.OpeningTableGeneration.Stretegies;

//ExploreOnly.Execute("");
ExplorationResult positions;
Console.WriteLine("Reading positions...");
using (var stream = new StreamReader($"4-7-6-10__2023-01-02-16-33-10.{Consts.DataFilesExtension}"))
{
    positions = new ExplorationResultSerializer().Deserialize(stream.ReadToEnd());
}
Console.WriteLine($"{positions.Count} positions readed successfully!");
Console.WriteLine("----------------------------------------");
SolveOnly.Execute(positions);