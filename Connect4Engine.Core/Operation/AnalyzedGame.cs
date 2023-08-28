using Connect4Engine.Core.AI;
using Connect4Engine.Core.Knowledge;

namespace Connect4Engine.Core.Operation
{
    public class AnalyzedGame : Game
    {
        private readonly Solver Solver;
        private readonly MoveAnalyzer Analyzer;
        private readonly Stack<MovementDescriptor[]> PlaysEvaluations;
        private readonly Stack<MovementDescriptor[]> UndosEvaluations;
        private readonly List<sbyte> Scores;

        public MoveClassification LastMoveClassification { get; private set; }

        public AnalyzedGame(int connectNeeded, int width, int height, OpeningTable openingTable) : base(connectNeeded, width, height)
        {
            Solver = new(connectNeeded, width, height, openingTable);
            Analyzer = new();
            PlaysEvaluations = new(width * height);
            UndosEvaluations = new(width * height);
            Scores = new((width * height) + 2) { -1, 1 };
            PlaysEvaluations.Push(Solver.Analyze(Engine, false));
            LastMoveClassification = MoveClassification.None;
        }

        public override void Play(byte columnIndex)
        {
            base.Play(columnIndex);
            PlaysEvaluations.Push(Solver.Analyze(Engine, false));
            if (UndosEvaluations.Count > 0)
            {
                UndosEvaluations.Clear();
            }
            Scores.Add(PlaysEvaluations.Peek()[PlaysHistory.Peek()].Score);
            UpdateLastMoveClassification();
        }

        public override void Undo()
        {
            base.Undo();
            UndosEvaluations.Push(PlaysEvaluations.Pop());
            Scores.RemoveAt(Scores.Count - 1);
            UpdateLastMoveClassification();
        }

        public override void Redo()
        {
            base.Redo();
            PlaysEvaluations.Push(UndosEvaluations.Pop());
            Scores.Add(PlaysEvaluations.Peek()[PlaysHistory.Peek()].Score);
            UpdateLastMoveClassification();
        }

        private void UpdateLastMoveClassification()
        {
            IEnumerable<MovementDescriptor> possibles = PlaysEvaluations.Peek().Where(move => move.Possible);
            LastMoveClassification = Analyzer.Analyze(Engine, PlaysEvaluations.Peek(), PlaysEvaluations.Peek()[PlaysHistory.Peek()].Score, Scores[^3]);
        }

    }
}
