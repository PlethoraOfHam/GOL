using GameOfLive.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Settings : Form
    {
        private GameSettings _gameSettings;

        public Settings()
        {
            InitializeComponent();
        }

        public GameSettings GameSettings { get { return _gameSettings; } }

        private void Create_Click(object sender, EventArgs e)
        {
            var settings = new GameSettings()
            {
                SizeX = (int)X.Value,
                SizeY = (int)Y.Value,
                Seed = (int)Seed.Value,
                TurnTime = (int)TurnTime.Value
            };
            _gameSettings = settings;
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
