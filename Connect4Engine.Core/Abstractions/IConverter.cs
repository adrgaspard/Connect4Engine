namespace Connect4Engine.Core.Abstractions
{
    public interface IConverter<TInput, TOutput>
    {
        TOutput Convert(TInput value);

        TInput ConvertBack(TOutput value);
    }
}
