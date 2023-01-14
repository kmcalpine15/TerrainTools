using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terrain.Data
{
    public class TerrainGrid
    {
        public List<TerrainTile> Tiles { get; private set; }

        public TerrainGrid()
        {
            Tiles = new List<TerrainTile>();
        }

        public TerrainGrid(IEnumerable<TerrainTile> tiles)
        {
            Tiles = tiles.ToList();
        }
        
        public void AddTile(TerrainTile tile)
        {
            foreach(var existingTile in Tiles)
            {
                if(tile.OverlapsWith(existingTile)) 
                {
                    throw new ArgumentException("Tile overlaps with existing tile");
                }
            }
            Tiles.Add(tile);
        }

       
    }
}
