using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Engine.Core.Utils
{
    public readonly record struct ClockParameters : IEquatable<ClockParameters>
    {
        public readonly TimeSpan BaseTime;
        public readonly TimeSpan IncrementTime;

        public ClockParameters(TimeSpan baseTime, TimeSpan incrementTime)
        {
            BaseTime = baseTime;
            IncrementTime = incrementTime;
        }

        public bool Equals(ClockParameters other)
        {
            return BaseTime.Equals(other.BaseTime) && IncrementTime.Equals(other.IncrementTime);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BaseTime, IncrementTime);
        }

        public override string ToString()
        {
            return $"{BaseTime.TotalMinutes} + {IncrementTime.TotalSeconds}";
        }
    }
}
