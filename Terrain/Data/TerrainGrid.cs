using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terrain.Data
{
    internal class TerrainGrid
    {
        public List<TerrainTile> Tiles { get; private set; }

        public TerrainGrid()
        {
            Tiles = new List<TerrainTile>();
        }

        public void AddTile(TerrainTile tile)
        {
            
        }
    }
}
