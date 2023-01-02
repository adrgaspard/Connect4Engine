using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Engine.Core.Knowledge
{
    public record struct EvaluationInfo(uint ExploredNodes, sbyte Score);
}
