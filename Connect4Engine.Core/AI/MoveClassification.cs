using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Engine.Core.AI
{
    [Flags]
    public enum MoveClassification : ushort
    {
        None = 0,
        Brilliant = 1,
        Great = 2,
        Best = 4,
        Excellent = 8,
        Good = 16,
        Theoric = 32,
        MissedWin = 64,
        Inaccuracy = 128,
        Mistake = 256,
        Blunder = 512,
        OnlyOne = 1024,
        Positives = Brilliant | Great | Best | Excellent | Good,
        Negatives = Inaccuracy | MissedWin | Mistake | Blunder,
    }
}
