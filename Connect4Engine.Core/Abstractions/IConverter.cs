using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Engine.Core.Abstractions
{
    public interface IConverter<TInput, TOutput>
    {
        TOutput Convert(TInput value);

        TInput ConvertBack(TOutput value);
    }
}
