using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Engine.Core.AI
{
    public record struct MovementDescriptor(bool Possible, sbyte Score);
}
