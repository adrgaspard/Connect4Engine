using Connect4Engine.Core.Operation;
using Connect4Engine.MVVM;
using Connect4Engine.MVVM.Abstractions;
using Connect4Engine.MVVM.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Connect4Engine.UI.WPF.ViewModels
{
    public sealed class BoardViewModel : ViewModelBase
    {
        private readonly GameManagerViewModel GameManagerViewModel;
        private readonly Dictionary<(byte, byte), SquareViewModel> SquaresViewModels;
        private readonly ObservableCollection<ColumnViewModel> PrivateColumnViewModels;

        private GameViewModel gameViewModel;
        public GameViewModel GameViewModel
        {
            get => gameViewModel;
            set
            {
                if (gameViewModel != value)
                {
                    gameViewModel = value;
                    ResetProperties();
                    RaisePropertyChanged();
                }
            }
        }

        public ReadOnlyObservableCollection<ColumnViewModel> ColumnViewModels { get; private init; }

        public BoardViewModel(GameManagerViewModel gameManagerViewModel)
        {
            GameManagerViewModel = gameManagerViewModel;
            gameViewModel = GameManagerViewModel.GameViewModel;
            GameViewModel.PositionModified += OnGameViewModelPositionModified;
            gameManagerViewModel.PropertyChanged += OnGameManagerViewModelPropertyChanged;
            SquaresViewModels = new(GameViewModel.Game.Width * GameViewModel.Game.Height);
            PrivateColumnViewModels = new();
            ColumnViewModels = new(PrivateColumnViewModels);
            ResetProperties();
        }

        public void RequestSquareUpdate(byte columnIndex, byte rowIndex)
        {
            if (SquaresViewModels.TryGetValue((columnIndex, rowIndex), out SquareViewModel? squareViewModel))
            {
                squareViewModel.UpdateProperties();
            }
        }

        private void ResetProperties()
        {
            Game game = GameViewModel.Game;
            SquaresViewModels.Clear();
            PrivateColumnViewModels.Clear();
            for (byte i = 0; i < game.Width; i++)
            {
                List<SquareViewModel> squares = new(game.Height);
                for (byte j = 0; j < game.Height; j++)
                {
                    SquareViewModel square = new(game, i, j);
                    squares.Add(square);
                    SquaresViewModels[(i, j)] = square;
                }
                PrivateColumnViewModels.Add(new(this, i, squares));
            }
        }

        private void OnGameManagerViewModelPropertyChanged(object? sender, PropertyChangedEventArgs eventArgs)
        {
            if (ReferenceEquals(GameManagerViewModel, sender) && eventArgs.PropertyName == nameof(GameManagerViewModel.GameViewModel))
            {
                GameViewModel.PositionModified -= OnGameViewModelPositionModified;
                GameViewModel = GameManagerViewModel.GameViewModel;
                GameViewModel.PositionModified += OnGameViewModelPositionModified;
            }
        }

        private void OnGameViewModelPositionModified(object sender, PositionModifiedEventArgs eventArgs)
        {
            RequestSquareUpdate(eventArgs.ColumnIndex, eventArgs.RowIndex);
        }
    }
}
