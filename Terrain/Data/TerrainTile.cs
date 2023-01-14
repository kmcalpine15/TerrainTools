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
        public int Left { get { return X; } }
        public int Right { get { return X + (CellSize * NumColumns); } }
        public int Bottom { get { return Y; } }
        public int Top { get { return Y + (CellSize * NumRows); } }

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
            return (Left <= other.Right && Right >= other.Left && Bottom <= other.Top && Top >= other.Bottom);
        }

        public bool OverlapsWith(TerrainTile other)
        {
            return (Left < other.Right && Right > other.Left && Bottom < other.Top && Top > other.Bottom);
            //return (a_left < b_right && a_right > b_left && a_bottom < b_top && a_top > b_bottom);
        }


    }
}
