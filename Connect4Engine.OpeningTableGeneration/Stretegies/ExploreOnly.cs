using Connect4Engine.Core.Knowledge;
using Connect4Engine.Core.Serialization;
using Connect4Engine.Core.Utils;

namespace Connect4Engine.OpeningTableGeneration.Stretegies
{
    public static class ExploreOnly
    {
        public static void Execute(string startingPosition = "")
        {
            OpeningTableExplorator explorator = new();
            ExplorationResult positions = ExplorationResult.Empty;
            Console.WriteLine($"Starting {nameof(ExploreOnly)} strategy!");
            Console.WriteLine("----------------------------------------");
            Task explore = Task.Run(() =>
            {
                positions = explorator.Explore(startingPosition, Global.ConnectedNeeded, Global.Width, Global.Height, Global.Depth);
            });
            DateTime exploreStart = DateTime.Now;
            while (explore.Status is not TaskStatus.RanToCompletion and not TaskStatus.Faulted and not TaskStatus.Canceled)
            {
                Thread.Sleep(10000);
                string elapsed = (DateTime.Now - exploreStart).ToString(@"hh\:mm\:ss");
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
                Console.Error.WriteLine(explore.Exception?.ToString() ?? "No exception found!");
                return;
            }
            Console.WriteLine($"[{DateTime.Now - exploreStart:hh\\:mm\\:ss}] Exploration finished, now it's time to write the results!");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Starting writing results...");
            using (StreamWriter stream = new($"{Global.ConnectedNeeded}-{Global.Width}-{Global.Height}-{Global.Depth}_{startingPosition}_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.{Consts.ExploreFilesExtension}"))
            {
                stream.Write(new ExplorationResultSerializer().Serialize(positions));
            }
            Console.WriteLine("Done!");
        }
    }
}
