using System.Net.Http.Headers;

namespace Terrain.Data
{
    public class TerrainTile
    {
        public int NumColumns { get; private set; }
        public int NumRows { get; private set; }
        public int XIndex { get; set; }
        public int YIndex { get; set; }
        public double CellSize { get; private set; }
        public float[][] Data { get; private set; }
        public CoordinateSpace CoordinateSpace { get; private set; }
        
        public TerrainTile(float[][]data, double cellSize, double x, double y)
        {
            CellSize = cellSize;
            Data = data;
            NumColumns = Data[0].Length;
            NumRows = Data.Length;
            CoordinateSpace = new CoordinateSpace(x, y, x + (NumColumns * CellSize), y + (NumRows * CellSize));
            XIndex = 0;
            YIndex = 0;
        }

        public TerrainTile(float[][]data, double cellSize, double x, double y, int xIndex, int yIndex)
        {
            CellSize = cellSize;
            Data = data;
            NumColumns = Data[0].Length;
            NumRows = Data.Length;
            CoordinateSpace = new CoordinateSpace(x, y, x + (NumColumns * CellSize), y + (NumRows * CellSize));
            XIndex = xIndex;
            YIndex = yIndex;
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

        public IEnumerable<TerrainTile> Split(int newTileSize,float noDataValue=float.MinValue)
        {
            int numTilesX = NumColumns % newTileSize > 0 ? (NumColumns / newTileSize) + 1 : NumColumns / newTileSize;
            int numTilesY = NumRows % newTileSize > 0 ? (NumRows / newTileSize) + 1 : NumRows / newTileSize;

            List<TerrainTile> results = new List<TerrainTile>();

            for (int tileY = 0; tileY < numTilesY; tileY++)
            {
                for (int tileX = 0; tileX < numTilesX; tileX++)
                {
                    float[][] data = new float[newTileSize][];
                    int yStart = tileY * newTileSize;
                    int xStart = tileX * newTileSize;

                    for (int y = 0; y < newTileSize; y++)
                    {
                        data[y] = new float[newTileSize];
                        for (int x = 0; x < newTileSize; x++)
                        {
                            int dataX = xStart + x;
                            int dataY = yStart + y;
                            if (dataX < NumColumns && dataY < NumRows)
                            {
                                data[y][x] = Data[dataY][dataX];
                            }
                            else
                            {
                                data[y][x] = noDataValue;
                            }
                        }
                    }

                    results.Add(new TerrainTile(data,
                        CellSize,
                        CoordinateSpace.MinX + (tileX * newTileSize * CellSize),
                        CoordinateSpace.MinY + (tileY * newTileSize * CellSize),
                        tileX+1,
                        numTilesY-1-tileY+1
                    ));
                }
            }

            return results;
        }

        public IEnumerable<TerrainTile> SplitAndHalfSize(int newTileSize, float noDataValue = float.MinValue)
        {
            int numTilesX = (NumColumns /2) % newTileSize > 0 ? (NumColumns / 2 / newTileSize) + 1 : NumColumns /2  / newTileSize;
            int numTilesY = (NumRows/2) % newTileSize > 0 ? (NumRows / 2 / newTileSize) + 1 : NumRows / 2 / newTileSize;

            List<TerrainTile> results = new List<TerrainTile>();

            for (int tileY = 0; tileY < numTilesY; tileY++)
            {
                for (int tileX = 0; tileX < numTilesX; tileX++)
                {
                    float[][] data = new float[newTileSize][];
                    int yStart = tileY * newTileSize;
                    int xStart = tileX * newTileSize;

                    for (int y = 0; y < newTileSize; y++)
                    {
                        data[y] = new float[newTileSize];
                        for (int x = 0; x < newTileSize; x++)
                        {
                            int dataX = xStart + x;
                            int dataY = yStart + y;
                            if (dataX*2 < NumColumns && dataY*2 < NumRows)
                            {
                                data[y][x] = Data[dataY*2][dataX*2];
                            }
                            else
                            {
                                data[y][x] = noDataValue;
                            }
                        }
                    }

                    results.Add(new TerrainTile(data,
                        CellSize,
                        CoordinateSpace.MinX + (tileX * newTileSize * CellSize),
                        CoordinateSpace.MinY + (tileY * newTileSize * CellSize),
                        tileX + 1,
                        numTilesY - 1 - tileY + 1
                    ));
                }
            }

            return results;
        }

        public TerrainTile SubTile(int x, int y, int width, int height, float noDataValue = float.MinValue)
        {
            float[][] data = new float[height][];
            for (int i = 0; i < height; i++)
            {
                data[i] = new float[width];
                for (int j = 0; j < width; j++)
                {

                    if (j < NumColumns && i < NumRows)
                    {
                        data[i][j] = Data[i][j];
                    }
                    else
                    {
                        data[i][j] = noDataValue;
                    }

                }
            }

            return new TerrainTile(data, CellSize, x, y);
        }

        public TerrainTile Resize(int newTileSize)
        {
            float[][] data = new float[newTileSize][];
            

            return new TerrainTile(data,
                CellSize,
                CoordinateSpace.MinX,
                CoordinateSpace.MinY);
        }
    }
}
