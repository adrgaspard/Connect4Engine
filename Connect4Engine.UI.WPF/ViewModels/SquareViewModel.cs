using ChessEngine.MVVM.ViewModels.Abstractions;
using Connect4Engine.Core.Match;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
