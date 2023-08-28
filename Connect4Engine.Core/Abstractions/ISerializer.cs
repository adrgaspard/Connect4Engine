namespace Connect4Engine.Core.Abstractions
{
    public interface ISerializer<TResource>
    {
        string Serialize(TResource input);

        TResource Deserialize(string input);
    }
}
