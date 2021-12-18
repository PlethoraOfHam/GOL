using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfLive.Settings
{
    public class DisplaySettings
    {
        public bool ShowGrid { get; set; }
        public bool ShowAliveNeighborCount { get; set; }
        public bool ShowHeadsUpDisplay { get; set; }
        public int CellSize { get; set; }
    }
}
