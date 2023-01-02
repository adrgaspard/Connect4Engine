using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Engine.Core.Knowledge
{
    public enum GeneratorStep
    {
        None,
        Initialization,
        SolvingPositions,
        SortingResults,
        End
    }
}
