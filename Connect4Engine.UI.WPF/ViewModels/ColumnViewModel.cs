using ChessEngine.MVVM.ViewModels.Abstractions;
using CommunityToolkit.Mvvm.Input;
using Connect4Engine.Core.Match;
using Connect4Engine.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Connect4Engine.UI.WPF.ViewModels
{
    public sealed class ColumnViewModel : ViewModelBase
    {
        private readonly BoardViewModel BoardViewModel;
        private readonly byte Column;

        public ReadOnlyObservableCollection<SquareViewModel> SquareViewModels { get; private init; }

        public ICommand RequestPlayCommand { get; private init; }

        public ColumnViewModel(BoardViewModel boardViewModel, byte column, IEnumerable<SquareViewModel> squareViewModels)
        {
            BoardViewModel = boardViewModel;
            Column = column;
            SquareViewModels = new(new(squareViewModels));
            RequestPlayCommand = new RelayCommand(RequestPlay);
        }

        private void RequestPlay()
        {
            BoardViewModel.GameViewModel.TryPlayCommand.Execute(Column);
        }
    }
}
