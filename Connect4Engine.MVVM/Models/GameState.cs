using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Engine.MVVM.Models
{
    [Flags]
    public enum GameState : ushort
    {
        None = 0,
        RedTurn = 1,
        YellowTurn = 2,
        Playing = RedTurn | YellowTurn,
        RedWonByPower = 4,
        YellowWonByPower = 8,
        WonByPower = RedWonByPower | YellowWonByPower,
        RedWonByResign = 16,
        YellowWonByResign = 32,
        WonByResign = RedWonByResign | YellowWonByResign,
        RedWonByTimeout = 64,
        YellowWonByTimeout = 128,
        WonByTimeout = RedWonByTimeout | YellowWonByTimeout,
        RedWon = RedWonByPower | RedWonByResign | RedWonByTimeout,
        YellowWon = YellowWonByPower | YellowWonByResign | YellowWonByTimeout,
        Won = RedWon | YellowWon,
        Draw = 256,
        Finished = Won | Draw
    }
}
