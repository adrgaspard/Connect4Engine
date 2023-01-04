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

        public static double Variance<T>(this IEnumerable<T> source, Func<T, double> extractNumber)
        {
            var sumOfSquared = 0.0;
            var sum = 0.0;
            var length = 0;
            foreach (var variable in source)
            {
                var number = extractNumber(variable);
                sumOfSquared += (number * number);
                sum += number;
                length++;
            }
            if (length <= 1) return 0;
            return Variance(sumOfSquared, sum, length);
        }

        public static double StandardDeviation<T>(this IEnumerable<T> source, Func<T, double> extractNumber)
        {
            var variance = Variance(source, extractNumber);
            if (variance <= 1) return 0;
            return Math.Sqrt(variance);
        }

        private static double Variance(double sumOfSquared, double sum, int length)
        {
            if (length <= 0)
            {
                throw new ArgumentException("CalculateVariance cannot accept length equal to or less than one", nameof(length));
            }
            return (sumOfSquared - ((sum * sum) / length)) / (length - 1);
        }

    }
}
