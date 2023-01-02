using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Engine.Core.Abstractions
{
    public interface ISerializer<TResource>
    {
        string Serialize(TResource input);

        TResource Deserialize(string input);
    }
}
