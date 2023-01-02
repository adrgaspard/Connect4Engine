using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Engine.Core.Match
{
    [Flags]
    public enum Colour : byte
    {
        None = 0,
        Red = 1,
        Yellow = 2
    }
}
