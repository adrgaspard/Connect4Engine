namespace Connect4Engine.Core.Utils
{
    public delegate void CountdownFinishedEventHandler(object? sender, CountdownFinishedEventArgs eventArgs);

    public sealed class CountdownFinishedEventArgs : EventArgs
    {
        public CountdownFinishedEventArgs()
        {
        }
    }
}