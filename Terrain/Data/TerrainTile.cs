using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terrain.Data
{
    public class TerrainTile
    {
        public int NumColumns { get; private set; }
        public int NumRows { get; private set; }
        public int CellSize { get; private set; }
        public float[][] Data { get; private set; }

        public TerrainTile(float[][]data, int cellSize)
        {
            CellSize = cellSize;
            Data = data;
            NumColumns = Data[0].Length;
            NumRows = Data.Length;
        }

        public TerrainTile Normalise(float globalMin, float globalMax)
        {
            if (globalMax < globalMin) { throw new ArgumentException("globalMin must be > than globalMax"); }
                
            float range = globalMax - globalMin;
            var resultData = Data.Select(row => row.Select(height => (height - globalMin) / range).ToArray()).ToArray();

            return new TerrainTile(resultData, CellSize);
        }
        
    }
}
