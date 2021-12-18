using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameOfLive.Settings;

namespace GameOfLife
{
    public partial class GameOfLife : Form
    {
        private System.Timers.Timer _timer;
        private GameSettings _gameSettings;
        private DisplaySettings _displaySettings;
        private Game _game;

        public GameOfLife()
        {
            InitializeComponent();
            _gameSettings = GetDefaultGameSettings();
            _displaySettings = GetDefaultDisplaySettings();
        }

        public GameSettings GetDefaultGameSettings()
        {
            var settings = new GameSettings()
            {
                TurnTime = 5000,
                SizeX = 25,
                SizeY = 25
            };

            return settings;
        }

        public DisplaySettings GetDefaultDisplaySettings()
        {
            var settings = new DisplaySettings()
            {
                CellSize = 50,
                ShowGrid = false
            };

            return settings;
        }

        public void Play()
        {
            if (_game == null)
            {
                Reset();
            }

            if (!_game.IsGameRunning && !_game.HasGameEnded)
            {
                _game.Start();
                UpdateUI();
                _timer = new System.Timers.Timer(_gameSettings.TurnTime);
                _timer.Elapsed += GameTimer_Elapsed;
                _timer.Start();
            }
        }

        private void GameTimer_Elapsed(object sender, EventArgs e)
        {
            if (_game.HasGameEnded)
            {
                _timer.Stop();
                MessageBox.Show("Current game has ended");
            }

            if (_game.IsGameRunning && !_game.HasGameEnded)
            {
                _game.Next();
                this.Invoke((MethodInvoker)delegate
                {
                    UpdateUI();
                });
            }
        }

        public void Pause()
        {
            if (_timer.Enabled)
            {
                _game.Stop();
                _timer.Stop();
            }
            UpdateUI();
        }

        public void Reset()
        {
            if (_gameSettings.Seed != null)
            {
                _game = new Game(_gameSettings.SizeX, _gameSettings.SizeY, _gameSettings.Seed.Value);
            }
            {
                _game = new Game(_gameSettings.SizeX, _gameSettings.SizeY);
            }
            ResetUI();
            DrawInitialGame();
            InitializeUI();
        }

        public void Reset(List<Cell> cells)
        {
            _game = new Game(cells);
            ResetUI();
            DrawInitialGame();
            InitializeUI();
        }

        private void DrawInitialGame()
        {
            universeGridContainer.Controls.Clear();
            foreach (var cell in _game.Cells)
            {
                var universeCell = new UniverseCell(_displaySettings.CellSize, cell);
                universeGridContainer.Controls.Add(universeCell);
            }
        }

        private void InitializeUI()
        {
            SetGameMenuEnabled(true);
        }

        private void ResetUI()
        {
            SetGameMenuEnabled(false);
            playToolStripMenuItem.Text = "Play";
            generationCount.Text = String.Empty;
            LivingCells.Text = String.Empty;
        }

        private void SetGameMenuEnabled(bool enabled)
        {
            playToolStripMenuItem.Enabled = enabled;
            NextToolStripMenuItem.Enabled = enabled;
            resetToolStripMenuItem.Enabled = enabled;
            clearToolStripMenuItem.Enabled = enabled;
        }

        private void UpdateUI()
        {
            playToolStripMenuItem.Text = !_game.IsGameRunning ? "Play" : "Stop";

                SetGameMenuEnabled(true);
                generationCount.Text = $"Generation: {_game.Generation}";
                LivingCells.Text = $"Living Cells: {_game.Cells.Count(c => c.IsAlive)}";

            foreach (var control in universeGridContainer.Controls)
            {
                var universeCell = control as UniverseCell;
                var cellToChange = _game.CellsChanged
                    .FirstOrDefault(c => c.X == universeCell.Cell.X && c.Y == universeCell.Cell.Y);

                    universeCell.Update(cellToChange);
            }
        }

        private string GameToFlatFile()
        {
            var stringBuilder = new StringBuilder(); ;

            for (int i = 0; i < _gameSettings.SizeY; i++)
            {
                var line = string.Empty;
                for (int j = 0; j < _gameSettings.SizeX; j++)
                {
                    var cell = _game.Cells.FirstOrDefault(c => c.X == j && c.Y == i);
                    line += cell.IsAlive ? "O" : ".";
                }
                stringBuilder.AppendLine(line);
            }

            return stringBuilder.ToString();
        }

        private List<Cell> FlatFileToCells(string fileName)
        {
            var cells = new List<Cell>();
            var contents = File.ReadAllLines(fileName);

            var y = 0;
            foreach (var line in contents)
            {
                var x = 0;
                foreach (var character in line)
                {
                    var cell = new Cell(x, y, character == 'O');
                    x++;
                    cells.Add(cell);
                }
                y++;
            }

            return cells;
        }

        private void Empty()
        {
            foreach (var cell in _game.Cells)
            {
                cell.IsAlive = false;
            }
            DrawInitialGame();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (resetToolStripMenuItem.Enabled)
            {
                Reset();
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveToolStripMenuItem.Enabled)
            {
                var output = GameToFlatFile();
                var dialog = new SaveFileDialog();
                var result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    File.WriteAllText(dialog.FileName, output);
                }
            }
        }

        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loadToolStripMenuItem.Enabled)
            {
                var dialog = new OpenFileDialog();
                var result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    var fileName = dialog.FileName;
                    var cells = FlatFileToCells(fileName);
                    Reset(cells);
                }
            }
        }

        private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (clearToolStripMenuItem.Enabled)
            {
                Empty();
            }
        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (playToolStripMenuItem.Enabled)
            {
                if (_game?.IsGameRunning ?? false)
                {
                    Pause();
                }
                else { Play(); }
            }
        }

        private void NextToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (NextToolStripMenuItem.Enabled)
            {
                _game.Next();
                UpdateUI();
            }
        }
        private void NewToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var gamesettings = new Settings();
            gamesettings.ShowDialog();
            if (gamesettings.GameSettings != null)
            {
                _gameSettings = gamesettings.GameSettings;
                Reset();
            }
        }

        private void ShowNeighborCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showNeighborCountToolStripMenuItem.Checked = !showNeighborCountToolStripMenuItem.Checked;
            foreach (var control in universeGridContainer.Controls)
            {
                var universeCell = control as UniverseCell;
                universeCell.DisplayNeighborCount = showNeighborCountToolStripMenuItem.Checked;
                universeCell.Update();
            }
        }

        private void ShowGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showGridToolStripMenuItem.Checked = !showGridToolStripMenuItem.Checked;
            foreach (var control in universeGridContainer.Controls)
            {
                var universeCell = control as UniverseCell;
                universeCell.DisplayBorder = showGridToolStripMenuItem.Checked;
                universeCell.Update();
            }
        }
    }
}
