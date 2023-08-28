namespace Connect4Engine.Core.Abstractions
{
    public interface ITable<TKey, TValue> : IReadOnlyTable<TKey, TValue> where TValue : struct
    {
        void Put(TKey key, TValue value);
    }

    public record struct TableResult<TValue>(bool Found, TValue Value) where TValue : struct
    {
        public static readonly TableResult<TValue> NotFound = new(false, default);

        public static implicit operator TableResult<TValue>(TValue value)
        {
            return new(true, value);
        }

        public static implicit operator TValue(TableResult<TValue> result)
        {
            return result.Value;
        }
    }
}
