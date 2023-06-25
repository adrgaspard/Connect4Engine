using ChessEngine.MVVM.ViewModels.Abstractions;
using CommunityToolkit.Mvvm.Input;
using Connect4Engine.Core.AI;
using Connect4Engine.Core.Knowledge;
using Connect4Engine.Core.Match;
using Connect4Engine.Core.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Connect4Engine.MVVM
{
    public sealed class SolverViewModel : ViewModelBase
    {
        private const string NoScoreFound = "NoScoreFound";

        private readonly OpeningTable OpeningTable;
        private readonly MoveAnalyzer Analyzer;
        private readonly List<sbyte> ScoreHistory;
        private readonly object Mutex;
        private bool Solving;

        public GameManagerViewModel GameManagerViewModel { get; private init; }

        public ICommand StartSolvingCommand { get; private init; }

        public ICommand StopSolvingCommand { get; private init; }

        private string scores;
        public string Scores
        {
            get => scores;
            private set
            {
                if (scores != value)
                {
                    scores = value;
                    RaisePropertyChanged();
                }
            }
        }

        private ObservableCollection<MoveClassification> moveClassifications;
        public ObservableCollection<MoveClassification> MoveClassifications
        {
            get => moveClassifications;
            private set
            {
                if (moveClassifications != value)
                {
                    moveClassifications = value;
                    RaisePropertyChanged();
                }
            }
        }

        private uint exploredNodes;
        public uint ExploredNodes
        {
            get => exploredNodes;
            private set
            {
                if (exploredNodes != value)
                {
                    exploredNodes = value;
                    RaisePropertyChanged();
                }
            }
        }

        private bool weak;
        public bool Weak
        {
            get => weak;
            set
            {
                if (weak != value)
                {
                    weak = value;
                    RaisePropertyChanged();
                }
            }
        }

        public SolverViewModel(GameManagerViewModel gameManagerViewModel)
        {
            ArgumentNullException.ThrowIfNull(gameManagerViewModel, nameof(gameManagerViewModel));
            GameManagerViewModel = gameManagerViewModel;
            Mutex = new();
            Solving = false;
            using (var stream = new StreamReader($"4-7-6-10__2023-01-05-01-31-09.c4knl"))
            {
                OpeningTable = new OpeningTableSerializer().Deserialize(stream.ReadToEnd());
            }
            Analyzer = new();
            ScoreHistory = new() { 1, -1 };
            scores = NoScoreFound;
            moveClassifications = new ObservableCollection<MoveClassification>();
            StartSolvingCommand = new RelayCommand(StartSolving);
            StopSolvingCommand = new RelayCommand(StopSolving);
        }

        private void StartSolving()
        {
            lock (Mutex)
            {
                if (!Solving)
                {
                    Solving = true;
                }
            }
            Task.Run(() =>
            {
                Game game = GameManagerViewModel.GameViewModel.Game;
                Solver solver = new(game.ConnectNeeded, game.Width, game.Height, OpeningTable);
                var scores = solver.Analyze(game.Engine, Weak);
                Scores = string.Join(", ", scores.Select(result => result.Possible ? result.Score.ToString() : "--"));
                var moveClassifications = new ObservableCollection<MoveClassification>();
                for (int i = 0; i < scores.Length; i++)
                {
                    moveClassifications.Add(Analyzer.Analyze(game.Engine, scores, scoresInBytes[i], ScoreHistory[ScoreHistory.Count - 2]));
                }
                MoveClassifications = moveClassifications;
                ExploredNodes = solver.ExploredNodesCount;
                solver.Reset();
            });
        }

        private void StopSolving()
        {
            lock (Mutex)
            {
                if (Solving)
                {
                    Solving = false;
                }
            }
            Scores = NoScoreFound;
            ExploredNodes = 0;
            MoveClassifications = new();
        }
    }
}
