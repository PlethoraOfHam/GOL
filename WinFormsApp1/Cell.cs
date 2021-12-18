using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfLife
{
    public class Cell
    {
        public Cell(int x, int y) 
            : this(x, y, false) { }

        public Cell(int x, int y, bool isAlive)
        {
            X = x;
            Y = y;
            IsAlive = isAlive;
        }

        public int X { get; private set; }
        public int Y { get; private set; }
        public bool IsAlive { get; set; }
        public int AliveNeighboringCells { get; set; }
    }
}
