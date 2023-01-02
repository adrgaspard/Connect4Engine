using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Engine.Core.Utils
{
    public static class MathExtensions
    {
        public static ulong NextPrime(this ulong source)
        {
            if (IsPrimeNumber(source))
            {
                return source;
            }
            ulong i = source + 1;
            while (true)
            {
                if (IsPrimeNumber(i))
                {
                    return i;
                }
                i++;
            }
        }

        public static bool IsPrimeNumber(this ulong source)
        {
            if (source <= 1)
            {
                return false;
            }
            for (ulong i = 2; i <= Math.Sqrt(source); i++)
            {
                if (source % i == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public static uint OneBitCount(this UInt128 source)
        {
            uint count;
            for (count = 0; source > 0; count++)
            {
                source &= source - 1;
            }
            return count;
        }
    }
}
