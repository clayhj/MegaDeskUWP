using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaDeskUWP
{
    public struct Desk
    {
        public enum Material
        {
            Laminate = 100,
            Oak = 200,
            Rosewood = 300,
            Veneer = 125,
            Pine = 50
        };

        public string DeskMaterial { get; set; }

        public int Width { get; set; }

        public int Depth { get; set; }

        public int Drawers { get; set; }
    }
}
