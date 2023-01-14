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

        public int Left { get; private set; }
        public int Right { get; private set; }
        public int Top { get; private set; }
        public int Bottom { get; private set; }

        public TerrainGrid()
        {
            Tiles = new List<TerrainTile>();
        }

        public TerrainGrid(IEnumerable<TerrainTile> tiles)
        {
            Tiles = tiles.ToList();
        }

        private void GrowBounds(TerrainTile tile)
        {
            if (tile.Left < Left) { Left = tile.Left; }
            if (tile.Right > Right) { Right = tile.Right; }
            if (tile.Top > Top) { Top = tile.Top; }
            if (tile.Bottom < Bottom) { Bottom = tile.Bottom; }
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
            GrowBounds(tile);
        }

        public float[][] ToMutliArray()
        {
            throw new NotImplementedException();
        }
    }
}
