using Connect4Engine.Core.Operation;
using Connect4Engine.MVVM.Abstractions;

namespace Connect4Engine.UI.WPF.ViewModels
{
    public sealed class SquareViewModel : ViewModelBase
    {
        private readonly Game Game;

        public byte Column { get; private init; }

        public byte Row { get; private init; }

        public Colour Colour { get; private set; }

        public SquareViewModel(Game game, byte column, byte row)
        {
            Game = game;
            Column = column;
            Row = row;
            Colour = Game[Column, Row];
        }

        public void UpdateProperties()
        {
            Colour = Game[Column, Row];
            RaisePropertyChanged(nameof(Colour));
        }
    }
}
