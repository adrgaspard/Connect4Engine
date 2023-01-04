using ChessEngine.MVVM.ViewModels.Abstractions;
using CommunityToolkit.Mvvm.Input;
using Connect4Engine.Core.AI;
using Connect4Engine.Core.Match;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Connect4Engine.MVVM
{
    public sealed class SolverViewModel : ViewModelBase
    {
        private const string NoScoreFound = "NoScoreFound";

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
            scores = NoScoreFound;
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
                Solver solver = new(game.ConnectNeeded, game.Width, game.Height);
                var results = solver.Analyze(game.Engine, Weak);
                Scores = string.Join(", ", results.Select(result => result == Solver.InvalidMoveScore ? "--" : result.ToString()));
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
        }
    }
}
