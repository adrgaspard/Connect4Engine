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
            double sumOfSquared = 0.0;
            double sum = 0.0;
            int length = 0;
            foreach (T? variable in source)
            {
                double number = extractNumber(variable);
                sumOfSquared += number * number;
                sum += number;
                length++;
            }
            return length <= 1 ? 0 : Variance(sumOfSquared, sum, length);
        }

        public static double StandardDeviation<T>(this IEnumerable<T> source, Func<T, double> extractNumber)
        {
            double variance = Variance(source, extractNumber);
            return variance <= 1 ? 0 : Math.Sqrt(variance);
        }

        private static double Variance(double sumOfSquared, double sum, int length)
        {
            return length <= 0
                ? throw new ArgumentException("CalculateVariance cannot accept length equal to or less than one", nameof(length))
                : (sumOfSquared - (sum * sum / length)) / (length - 1);
        }

    }
}
