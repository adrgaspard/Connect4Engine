namespace Connect4Engine.MVVM.Models
{
    public sealed class PositionModifiedEventArgs : EventArgs
    {
        public readonly byte ColumnIndex;
        public readonly byte RowIndex;

        public PositionModifiedEventArgs(byte columnIndex, byte rowIndex)
        {
            ColumnIndex = columnIndex;
            RowIndex = rowIndex;
        }
    }

    public delegate void PositionModifiedEventHandler(object sender, PositionModifiedEventArgs eventArgs);
}

