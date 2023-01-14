using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terrain.Data
{
    public class TerrainGrid
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int CellSize { get; set; }
        public float[][] Data { get; private set; }

        public TerrainGrid()
        {

        }
        
    }
}
