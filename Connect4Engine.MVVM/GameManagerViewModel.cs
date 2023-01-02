using ChessEngine.MVVM.ViewModels.Abstractions;
using CommunityToolkit.Mvvm.Input;
using Connect4Engine.Core.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Connect4Engine.MVVM
{
    public sealed class GameManagerViewModel : ViewModelBase
    {
        private static readonly IReadOnlyCollection<ClockParameters> InfiniteTimeOnly = new ReadOnlyCollection<ClockParameters>(new List<ClockParameters>(1) { ClockParametersConsts.InfiniteTime });

        private readonly IReadOnlyCollection<ClockParameters> CustomClockParameters;

        private int powerNeeded;
        public int PowerNeeded
        {
            get => powerNeeded;
            set
            {
                if (powerNeeded != value)
                {
                    powerNeeded = value;
                    RaisePropertyChanged();
                }
            }
        }

        private int boardWidth;
        public int BoardWidth
        {
            get => boardWidth;
            set
            {
                if (boardWidth != value)
                {
                    boardWidth = value;
                    RaisePropertyChanged();
                }
            }
        }

        private int boardHeight;
        public int BoardHeight
        {
            get => boardHeight;
            set
            {
                if (boardHeight != value)
                {
                    boardHeight = value;
                    RaisePropertyChanged();
                }
            }
        }

        private bool allowUndoAndRedo;
        public bool AllowUndoAndRedo
        {
            get => allowUndoAndRedo;
            set
            {
                if (allowUndoAndRedo != value)
                {
                    allowUndoAndRedo = value;
                    RaisePropertyChanged();
                    AvailableClockParameters = AllowUndoAndRedo ? InfiniteTimeOnly : CustomClockParameters;
                }
            }
        }

        private ClockParameters clockParameters;
        public ClockParameters ClockParameters
        {
            get => clockParameters;
            set
            {
                if (clockParameters != value)
                {
                    clockParameters = value;
                    RaisePropertyChanged();
                }
            }
        }

        private IReadOnlyCollection<ClockParameters> availableClockParameters;
        public IReadOnlyCollection<ClockParameters> AvailableClockParameters
        {
            get => availableClockParameters;
            private set
            {
                if (availableClockParameters != value)
                {
                    availableClockParameters = value;
                    RaisePropertyChanged();
                    ClockParameters = AvailableClockParameters.First();
                }
            }
        }

        private GameViewModel gameViewModel;
        public GameViewModel GameViewModel
        {
            get => gameViewModel;
            private set
            {
                if (gameViewModel != value)
                {
                    gameViewModel = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ICommand TryCreateNewCommand { get; private init; }

        public GameManagerViewModel(IEnumerable<ClockParameters>? customClockParameters = null)
        {
            PowerNeeded = 4;
            BoardWidth = 7;
            BoardHeight = 6;
            customClockParameters ??= ClockParametersConsts.PredefinedClockParameters;
            customClockParameters = customClockParameters.Any() ? customClockParameters : ClockParametersConsts.PredefinedClockParameters;
            CustomClockParameters = new ReadOnlyCollection<ClockParameters>(customClockParameters.ToList());
            availableClockParameters = CustomClockParameters;
            ClockParameters = AvailableClockParameters.First();
            AllowUndoAndRedo = false;
            gameViewModel = new(PowerNeeded, BoardWidth, BoardHeight, ClockParameters, AllowUndoAndRedo);
            TryCreateNewCommand = new RelayCommand(TryCreateNew);
        }

        private bool CanCreateNew()
        {
            return PowerNeeded > 0 && PowerNeeded <= BoardWidth && PowerNeeded <= BoardHeight && BoardWidth > 1 && BoardHeight > 1 && CustomClockParameters.Contains(ClockParameters);
        }

        private void TryCreateNew()
        {
            if (CanCreateNew())
            {
                GameViewModel = new(PowerNeeded, BoardWidth, BoardHeight, ClockParameters, AllowUndoAndRedo);
            }
        }
    }
}
