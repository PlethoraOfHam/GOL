using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace GameOfLife
{
    class Game
    {
        //Keep track of whether we're in a game
        private bool _isGameRunning;
        private bool _gameEnded;
        private int _generation = 0;
        private List<Cell> _cellsChanged = new List<Cell>();

        private Universe _universe;

        public Game(List<Cell> cells)
        {
            _universe = new Universe(cells);
        }

        public Game(int x, int y)
        {
            _universe = new Universe(x, y);
            _universe.Generate();
        }

        public Game(int x, int y, int seed)
        {
            _universe = new Universe(x, y);
            _universe.Generate(seed);
        }

        public bool IsGameRunning { get { return _isGameRunning; } }
        public bool HasGameEnded { get { return _gameEnded; } }
        public int Generation { get { return _generation; } }
        public List<Cell> Cells { get { return _universe.Cells; } }
        public List<Cell> CellsChanged { get { return _cellsChanged; } }

        public void Start()
        {
            //Can't start game if it's currently running or has ended
            if (_gameEnded || _isGameRunning) return;
            
            _isGameRunning = true;
            Next();

        }

        private void End()
        {
            if (!_isGameRunning) return;

            _gameEnded = true;
            _isGameRunning = false;
        }

        public void Next()
        {
            if (_gameEnded) return;

            var cellsChanged = PlayTurn();
            _cellsChanged = cellsChanged;
            //Game has ended, no more moves!
            if (!cellsChanged.Any())
            {
                End();
            }
        }

        private List<Cell> PlayTurn()
        {
            _generation++;
            var cellsChanged = new List<Cell>();

            foreach (var cell in _universe.Cells)
            {
                var cellAliveStatus = cell.IsAlive;
                var neighboringCells = _universe.GetNeighboringCells(cell);
                var cellAliveStatusAfterRules = ApplyRules(cell, neighboringCells);
               
                if (cellAliveStatus != cellAliveStatusAfterRules)
                {
                    cellsChanged.Add(cell);
                }

                cell.IsAlive = cellAliveStatusAfterRules;
            }

            foreach(var cell in _universe.Cells)
            {
                var neighboringCells = _universe.GetNeighboringCells(cell);
                cell.AliveNeighboringCells = neighboringCells.Count(c => c.IsAlive);
            }

            return cellsChanged;
        }

        private static bool ApplyRules(Cell cell, List<Cell> neighboringCells)
        {
            var aliveCount = neighboringCells.Where(c => c.IsAlive).Count();
            if (cell.IsAlive)
            {
                return (aliveCount > 3 && aliveCount < 2);
            }
            else
            {
                return (aliveCount == 3);
            }
        }

        public void Stop()
        {
            _isGameRunning = false;
        }
    }
}
