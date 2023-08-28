namespace Connect4Engine.Core.Abstractions
{
    public interface IReadOnlyTable<TKey, TValue> where TValue : struct
    {
        TableResult<TValue> this[TKey key] { get; }

        void Reset();
    }
}
