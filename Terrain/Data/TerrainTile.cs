namespace Terrain.Data
{
    public class TerrainTile
    {
        public int NumColumns { get; private set; }
        public int NumRows { get; private set; }
        public int CellSize { get; private set; }
        public float[][] Data { get; private set; }
        public CoordinateSpace CoordinateSpace { get; private set; }
        

        public TerrainTile(float[][]data, int cellSize, int x, int y)
        {
            CellSize = cellSize;
            Data = data;
            NumColumns = Data[0].Length;
            NumRows = Data.Length;
            CoordinateSpace = new CoordinateSpace(x, y, x + (NumColumns * CellSize), y + (NumRows * CellSize));
        }

        public TerrainTile Normalise(float globalMin, float globalMax)
        {
            if (globalMax < globalMin) { throw new ArgumentException("globalMin must be > than globalMax"); }
                
            float range = globalMax - globalMin;
            var resultData = Data.Select(row => row.Select(height => (height - globalMin) / range).ToArray()).ToArray();

            return new TerrainTile(resultData, CellSize, CoordinateSpace.MinX, CoordinateSpace.MinY);
        }
        
        public bool CollidesWith(TerrainTile other)
        {
            return (CoordinateSpace.Left <= other.CoordinateSpace.Right &&
                CoordinateSpace.Right >= other.CoordinateSpace.Left &&
                CoordinateSpace.Bottom <= other.CoordinateSpace.Top &&
                CoordinateSpace.Top >= other.CoordinateSpace.Bottom);
        }

        public bool OverlapsWith(TerrainTile other)
        {
            return (CoordinateSpace.Left < other.CoordinateSpace.Right &&
                CoordinateSpace.Right > other.CoordinateSpace.Left &&
                CoordinateSpace.Bottom < other.CoordinateSpace.Top &&
                CoordinateSpace.Top > other.CoordinateSpace.Bottom);
            //return (a_left < b_right && a_right > b_left && a_bottom < b_top && a_top > b_bottom);
        }


    }
}
