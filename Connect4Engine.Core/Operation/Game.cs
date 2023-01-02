using Connect4Engine.Core.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Engine.Core.Match
{
    public class Game
    {
        public readonly GameEngine Engine;
        private readonly Stack<byte> PlaysHistory;
        private readonly Stack<byte> UndosHistory;
        private bool LastUndoWasWinning;
        private bool LastUndoWasDraw;
        private readonly byte[] ColumnHeight;

        public bool IsFinished { get; private set; }

        public Colour Winner { get; private set; }

        public Colour Current { get; private set; }

        public byte? LastPlayColumn => PlaysHistory.Count > 0 ? PlaysHistory.Peek() : null;

        public byte? LastUndoColumn => UndosHistory.Count > 0 ? UndosHistory.Peek() : null;

        public int ConnectNeeded => Engine.ConnectNeeded;

        public int Width => Engine.Width;

        public int Height => Engine.Height;

        public byte this[byte columnIndex] => ColumnHeight[columnIndex];

        public Colour this[byte columnIndex, byte rowIndex]
        {
            get
            {
                if ((Engine.AllPawns & (UInt128.One << rowIndex + columnIndex * (Engine.Height + 1))) == 0)
                {
                    return Colour.None;
                }
                if ((Engine.CurrentPawns & (UInt128.One << rowIndex + columnIndex * (Engine.Height + 1))) == 0)
                {
                    return Current == Colour.Red ? Colour.Yellow : Colour.Red;
                }
                return Current;
            }
        }

        public Game(int connectNeeded, int width, int height)
        {
            Engine = new(connectNeeded, width, height);
            LastUndoWasWinning = false;
            LastUndoWasDraw = false;
            PlaysHistory = new(width * height);
            UndosHistory = new(width * height);
            ColumnHeight = new byte[width];
            IsFinished = false;
            Winner = Colour.None;
            Current = Colour.Red;
        }

        public bool CanPlay(byte columnIndex)
        {
            return !IsFinished && columnIndex < Engine.Width && Engine.CanPlay(columnIndex);
        }

        public bool CanUndo()
        {
            return PlaysHistory.Count > 0;
        }

        public bool CanRedo()
        {
            return UndosHistory.Count > 0;
        }

        public void Play(byte columnIndex)
        {
            if (Engine.IsWinningMove(columnIndex))
            {
                IsFinished = true;
                Winner = Current;
            }
            if (Engine.RemainingMoves == 0)
            {
                IsFinished = true;
                Winner = Colour.None;
            }
            Engine.Play(columnIndex);
            ColumnHeight[columnIndex]++;
            Current = Current == Colour.Red ? Colour.Yellow : Colour.Red;
            PlaysHistory.Push(columnIndex);
            if (UndosHistory.Count > 0)
            {
                UndosHistory.Clear();
                LastUndoWasWinning = false;
                LastUndoWasDraw = false;
            }
        }

        public void Undo()
        {
            if (IsFinished)
            {
                IsFinished = false;
                LastUndoWasDraw = Winner == Colour.None;
                LastUndoWasWinning = !LastUndoWasDraw;
                Winner = Colour.None;
            }
            byte columnIndex = PlaysHistory.Pop();
            Engine.Undo();
            ColumnHeight[columnIndex]--;
            Current = Current == Colour.Red ? Colour.Yellow : Colour.Red;
            UndosHistory.Push(columnIndex);
        }

        public void Redo()
        {
            byte columnIndex = UndosHistory.Pop();
            if (UndosHistory.Count == 0 && (LastUndoWasWinning || LastUndoWasDraw))
            {
                IsFinished = true;
                Winner = LastUndoWasWinning ? Current : Colour.None;
                LastUndoWasWinning = false;
                LastUndoWasDraw = false;
            }
            Engine.Play(columnIndex);
            ColumnHeight[columnIndex]++;
            Current = Current == Colour.Red ? Colour.Yellow : Colour.Red;
            PlaysHistory.Push(columnIndex);
        }
    }
}
