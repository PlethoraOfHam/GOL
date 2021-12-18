using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GameOfLife
{
    class UniverseCell : Panel
    {
        private Cell _cell;
        private Label _label;
        public UniverseCell(int size, Cell cell)
        {
            this.MouseClick += new MouseEventHandler(UniverseCell_MouseClick);
            _cell = cell;
            Height = size;
            Width = size;
            Location = new Point(size * cell.X, size * cell.Y);
            Update();         
        }

        private void UniverseCell_MouseClick(object sender, MouseEventArgs e)
        {
            _cell.IsAlive = !_cell.IsAlive;
            Update();
        }

        private void UniverseCell_Click(object sender, EventArgs e)
        {
            _cell.IsAlive = !_cell.IsAlive;
        }

        public Cell Cell {  get { return _cell; } }

        public void Update(Cell cell = null)
        {
            if (cell != null)
            {
                _cell = cell;
            }
            SetColor();
            SetText();
            DrawBorder();
        }

        public void SetText()
        {
            if (_label == null)
            {
                _label = new Label();
                _label.MouseClick += UniverseCell_MouseClick;
                _label.Font = new Font(_label.Font.FontFamily, 25f);
                _label.Dock = DockStyle.Fill;
                Controls.Add(_label);
            }
                if (DisplayNeighborCount)
                {
                    _label.Text = Cell.AliveNeighboringCells.ToString();
                }
                else
                {
                    _label.Text = String.Empty;
                }
        }

        private void SetColor()
        {
            this.BackColor = _cell.IsAlive ? Color.Gray : Color.White;
        }

        private void DrawBorder()
        {
            BorderStyle = DisplayBorder ? BorderStyle.FixedSingle : BorderStyle.None;
        }

        public bool DisplayBorder { get; set; }
        public bool DisplayNeighborCount { get; set; }

    }
}
