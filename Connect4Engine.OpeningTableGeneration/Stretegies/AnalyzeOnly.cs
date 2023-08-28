using Connect4Engine.Core.Knowledge;
using Connect4Engine.Core.Utils;

namespace Connect4Engine.OpeningTableGeneration.Stretegies
{
    public static class AnalyzeOnly
    {
        public static void Execute(GenerationResult scores)
        {
            Console.WriteLine($"Starting {nameof(AnalyzeOnly)} strategy!");
            Console.WriteLine("----------------------------------------");
            Dictionary<PositionMultiIdentifier, EvaluationInfo> orderedScores = scores.OrderByDescending(pair => pair.Value.ExploredNodes).ToDictionary(pair => pair.Key, pair => pair.Value);
            uint[] exploredNodeValues = orderedScores.Select(pair => pair.Value.ExploredNodes).ToArray();
            uint greater1M = 0;
            uint greater2M = 0;
            uint greater4M = 0;
            uint greater8M = 0;
            uint greater16M = 0;
            uint greater32M = 0;
            uint greater64M = 0;
            for (int i = 0; i < exploredNodeValues.Length; i++)
            {
                if (exploredNodeValues[i] >= 1000000)
                {
                    greater1M++;
                    if (exploredNodeValues[i] >= 2000000)
                    {
                        greater2M++;
                        if (exploredNodeValues[i] >= 4000000)
                        {
                            greater4M++;
                            if (exploredNodeValues[i] >= 8000000)
                            {
                                greater8M++;
                                if (exploredNodeValues[i] >= 16000000)
                                {
                                    greater16M++;
                                    if (exploredNodeValues[i] >= 32000000)
                                    {
                                        greater32M++;
                                        if (exploredNodeValues[i] >= 64000000)
                                        {
                                            greater64M++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            Console.WriteLine("-- Nodes explored per position --");
            Console.WriteLine("1. General informations");
            Console.WriteLine($"Average            : {exploredNodeValues.Average(num => num):0.00}");
            Console.WriteLine($"Standard deviation : {exploredNodeValues.StandardDeviation(num => num):0.00}");
            Console.WriteLine($"Minimum            : {exploredNodeValues.Last()}");
            Console.WriteLine($"First quartile     : {exploredNodeValues[exploredNodeValues.Length * 3 / 4]}");
            Console.WriteLine($"Median             : {exploredNodeValues[exploredNodeValues.Length / 2]}");
            Console.WriteLine($"Third quartile     : {exploredNodeValues[exploredNodeValues.Length / 4]}");
            Console.WriteLine($"Maximum            : {exploredNodeValues.First()}");
            Console.WriteLine("2. Percentages");
            Console.WriteLine($">= 1M              : {greater1M} ({greater1M * 100 / (double)exploredNodeValues.Length:0.00}%)");
            Console.WriteLine($">= 2M              : {greater2M} ({greater2M * 100 / (double)exploredNodeValues.Length:0.00}%)");
            Console.WriteLine($">= 4M              : {greater4M} ({greater4M * 100 / (double)exploredNodeValues.Length:0.00}%)");
            Console.WriteLine($">= 8M              : {greater8M} ({greater8M * 100 / (double)exploredNodeValues.Length:0.00}%)");
            Console.WriteLine($">= 16M             : {greater16M} ({greater16M * 100 / (double)exploredNodeValues.Length:0.00}%)");
            Console.WriteLine($">= 32M             : {greater32M} ({greater32M * 100 / (double)exploredNodeValues.Length:0.00}%)");
            Console.WriteLine($">= 64M             : {greater64M} ({greater64M * 100 / (double)exploredNodeValues.Length:0.00}%)");
        }
    }
}
