using CommunityToolkit.Mvvm.Input;
using Connect4Engine.Core.Operation;
using Connect4Engine.Core.Utils;
using Connect4Engine.MVVM.Abstractions;
using Connect4Engine.MVVM.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Connect4Engine.MVVM
{
    public sealed class GameViewModel : ViewModelBase
    {
        private Colour ResignedColour;

        public bool AllowUndoAndRedo { get; private init; }

        private bool started;
        public bool Started
        {
            get => started;
            private set
            {
                if (started != value)
                {
                    started = value;
                    RaisePropertyChanged();
                }
            }
        }

        private bool running;
        public bool Running
        {
            get => running;
            private set
            {
                if (running != value)
                {
                    running = value;
                    RaisePropertyChanged();
                }
            }
        }

        private GameState state;
        public GameState State
        {
            get => state;
            private set
            {
                if (state != value)
                {
                    state = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Game Game { get; private init; }

        public IReadOnlyDictionary<Colour, ClockViewModel> Clocks { get; private init; }

        public ICommand StartCommand { get; private init; }

        public ICommand TryTogglePauseCommand { get; private init; }

        public ICommand TryResignCommand { get; private init; }

        public ICommand TryPlayCommand { get; private init; }

        public ICommand TryUndoCommand { get; private init; }

        public ICommand TryRedoCommand { get; private init; }

        public event PositionModifiedEventHandler? PositionModified;

        public GameViewModel(int connectNeeded, int boardWidth, int boardHeight, ClockParameters clockParameters, bool allowUndoAndRedo)
        {
            ResignedColour = Colour.None;
            AllowUndoAndRedo = allowUndoAndRedo;
            Started = false;
            Running = false;
            Game = new(connectNeeded, boardWidth, boardHeight);
            Clocks = new ReadOnlyDictionary<Colour, ClockViewModel>(new Dictionary<Colour, ClockViewModel>(2)
            {
                { Colour.Red, new(clockParameters) },
                { Colour.Yellow, new(clockParameters) }
            });
            StartCommand = new RelayCommand(Start);
            TryTogglePauseCommand = new RelayCommand(TryTogglePause);
            TryResignCommand = new RelayCommand<Colour>(TryResign);
            TryPlayCommand = new RelayCommand<byte>(TryPlay);
            TryUndoCommand = new RelayCommand(TryUndo);
            TryRedoCommand = new RelayCommand(TryRedo);
            UpdateGameState();
        }

        private void Start()
        {
            if (Started || Running)
            {
                throw new InvalidOperationException("The game is already started.");
            }
            Started = true;
            TryTogglePause();
        }

        private void TryTogglePause()
        {
            if (!Started || (State & GameState.Playing) == 0)
            {
                return;
            }
            if (Running)
            {
                Clocks[Game.Current].PauseCommand.Execute(null);
            }
            else
            {
                Clocks[Game.Current].StartCommand.Execute(null);
            }
            Running = !Running;
        }

        private void TryResign(Colour colour)
        {
            if (Started && Running && (State & GameState.Playing) == 0)
            {
                ResignedColour = colour;
                UpdateGameState();
            }
        }

        private void TryPlay(byte columnIndex)
        {
            if (Started && Running && ((State & GameState.Playing) != 0) && Game.CanPlay(columnIndex))
            {
                Clocks[Game.Current].PauseCommand.Execute(null);
                Clocks[Game.Current].IncrementCommand.Execute(null);
                byte rowIndex = Game[columnIndex];
                Game.Play(columnIndex);
                UpdateGameState();
                PositionModified?.Invoke(this, new PositionModifiedEventArgs(columnIndex, rowIndex));
                Clocks[Game.Current].StartCommand.Execute(null);
            }
        }

        private void TryUndo()
        {
            if (AllowUndoAndRedo && Started && Running && Game.CanUndo())
            {
                byte columnIndex = Game.LastPlayColumn ?? byte.MinValue;
                Game.Undo();
                UpdateGameState();
                PositionModified?.Invoke(this, new PositionModifiedEventArgs(columnIndex, Game[columnIndex]));
            }
        }

        private void TryRedo()
        {
            if (AllowUndoAndRedo && Started && Running && Game.CanRedo())
            {
                byte columnIndex = Game.LastUndoColumn ?? byte.MinValue;
                byte rowIndex = Game[columnIndex];
                Game.Redo();
                UpdateGameState();
                PositionModified?.Invoke(this, new PositionModifiedEventArgs(columnIndex, rowIndex));
            }
        }

        private void UpdateGameState()
        {
            State = Game.IsFinished
                ? Game.Winner switch
                {
                    Colour.Red => GameState.RedWonByPower,
                    Colour.Yellow => GameState.YellowWonByPower,
                    _ => GameState.Draw,
                }
                : ResignedColour != Colour.None
                    ? ResignedColour == Colour.Red ? GameState.YellowWonByResign : GameState.RedWonByResign
                    : Clocks[Colour.Red].RemainingTime == TimeSpan.Zero
                                    ? GameState.YellowWonByTimeout
                                    : Clocks[Colour.Yellow].RemainingTime == TimeSpan.Zero
                                        ? GameState.RedWonByTimeout
                                        : Game.Current == Colour.Red ? GameState.RedTurn : GameState.YellowTurn;
        }
    }
}
