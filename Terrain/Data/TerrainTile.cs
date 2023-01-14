namespace Terrain.Data
{
    public class TerrainTile
    {
        public int NumColumns { get; private set; }
        public int NumRows { get; private set; }
        public int CellSize { get; private set; }
        public float[][] Data { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public TerrainTile(float[][]data, int cellSize, int x, int y)
        {
            CellSize = cellSize;
            Data = data;
            NumColumns = Data[0].Length;
            NumRows = Data.Length;
            X = x;
            Y = y;
        }

        public TerrainTile Normalise(float globalMin, float globalMax)
        {
            if (globalMax < globalMin) { throw new ArgumentException("globalMin must be > than globalMax"); }
                
            float range = globalMax - globalMin;
            var resultData = Data.Select(row => row.Select(height => (height - globalMin) / range).ToArray()).ToArray();

            return new TerrainTile(resultData, CellSize, X, Y);
        }
        
        public bool CollidesWith(TerrainTile other)
        {
            int a_left = X;
            int a_bottom = Y;
            int a_right = X + (CellSize * NumColumns);
            int a_top = Y + (CellSize * NumRows);

            int b_left = other.X;
            int b_bottom = other.Y;
            int b_right = other.X + (other.CellSize * other.NumColumns);
            int b_top = other.Y + (other.CellSize * other.NumRows);

            return (a_left <= b_right && a_right >= b_left && a_bottom <= b_top && a_top >= b_bottom);
        }
       
    }
}
