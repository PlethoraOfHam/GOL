using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOfLife
{
    public class Universe
    {
        private int _sizeX;
        private int _sizeY;

        private static readonly int _minimumRandomValue = 0;
        private static readonly int _maximumRandomValue = 1;
        private static readonly (int x, int y)[] _neighboringCoordinates = {
            (-1, -1),
            (-1, 0),
            (-1, 1),
            (0, +1),
            (0, -1),
            (+1, -1),
            (+1, 0),
            (+1, 1)
        };

        public Universe(List<Cell> cells)
        {
            Cells = cells;
            _sizeX = cells.Max(c => c.X + 1);
            _sizeY = cells.Max(x => x.Y + 1);
        }

        public Universe(int x, int y)
        {
            _sizeX = x;
            _sizeY = y;
        }

        public List<Cell> Cells { get; private set; }

        public void Generate() => Generate(new Random().Next(int.MinValue, int.MaxValue));
        public void Generate(int seed)
        {
            var cells = new List<Cell>();

            var random = new Random(seed);
            
            for (int i = 0; i < _sizeX; i++)
            {
                for (int j = 0; j < _sizeY; j++)
                {
                    var randomNumber = random.NextDouble() * (_maximumRandomValue - _minimumRandomValue);
                    var isAlive = (randomNumber <= 0.5);
                    cells.Add(new Cell(i, j, isAlive));
                }
            }

            Cells = cells;
        }

        public List<Cell> GetNeighboringCells(Cell cell)
        {
            var neighboringCells = new List<Cell>();
            foreach(var coordinate in _neighboringCoordinates)
            {
                var neighboringCell = GetCellAtCoordinates(cell.X - coordinate.x, cell.Y - coordinate.y);

                if (neighboringCell != null)
                {
                    neighboringCells.Add(neighboringCell);
                }
            }

            return neighboringCells;
        }

        public Cell GetCellAtCoordinates(int x, int y)
        {
            if ((Cells?.Count ?? 0) == 0) return null;

            return Cells.FirstOrDefault(c => c.X == x && c.Y == y);
        }
    }
}
