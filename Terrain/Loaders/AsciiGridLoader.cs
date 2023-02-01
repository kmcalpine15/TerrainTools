using Terrain.Data;

namespace Terrain.Loaders
{
    public class AsciiGridLoader
    {
        public AsciiGridLoader()
        {
        }

        public async Task<TerrainTile> LoadTerrain(string directory)
        {
            var dirInfo = new DirectoryInfo(directory);

            double cellSize = double.MinValue;
            CoordinateSpace worldSpace = new CoordinateSpace(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);

            List<TerrainTile> tiles = new List<TerrainTile>();
            foreach (var file in dirInfo.GetFiles("*.asc"))
            {
                var tile = await LoadAsciiFile(file.FullName);
                worldSpace.GrowBounds(tile.CoordinateSpace);
                if (cellSize == int.MinValue)
                {
                    cellSize = tile.CellSize;
                }
                tiles.Add(tile);
            }

            CoordinateSpace dataSpace = new CoordinateSpace(0, 0,worldSpace.Width / cellSize, worldSpace.Height / cellSize);
            Transform transform = new Transform(worldSpace, dataSpace);

            float[][] data = new float[(int)dataSpace.Height][];
            for(int i=0; i<dataSpace.Height; i++)
            {
                data[i] = new float[(int)dataSpace.Width];
            }

            foreach(var tile in tiles)
            {
                for(int y=0; y<tile.Data.Length; y++)
                {
                    double yCoord = (tile.CoordinateSpace.MinY) + (y * tile.CellSize);
                    for (int x = 0; x < tile.Data[y].Length; x++)
                    {
                        Point worldCoord = new Point(tile.CoordinateSpace.MinX + (x * tile.CellSize), yCoord);
                        Point dataCoord = transform.Apply(worldCoord);

                        data[(int)dataCoord.Y][(int)dataCoord.X] = tile.Data[y][x];
                    }
                }
            }

            return new TerrainTile(data, cellSize, worldSpace.MinX, worldSpace.MinY);
        }
        
        private async Task<TerrainTile> LoadAsciiFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);

            var nCols = loadCheckLine(lines[0], "ncols");
            var nRows = loadCheckLine(lines[1], "nrows");
            var xllCorner = loadCheckLine(lines[2], "xllcorner");
            var yllCorner = loadCheckLine(lines[3], "yllcorner");
            var cellSize = loadCheckLine(lines[4], "cellsize");

            var dataLines = lines[5..];

            if (dataLines.Length != nRows)
            {
                throw new Exception("Invalid number of rows");
            }

            List<List<float>> parsedData = new List<List<float>>();
            foreach (var line in dataLines)
            {
                var parts = line.Split(' ');
                if (parts.Length != nCols)
                {
                    throw new Exception("Invalid number of columns");
                }

                var row = new List<float>();
                row.AddRange(parts.Select(p => float.Parse(p)));
                parsedData.Add(row);
            }

            parsedData.Reverse();
            return new TerrainTile(parsedData.Select(r => r.ToArray()).ToArray(), cellSize, xllCorner, yllCorner);
        }

        private int loadCheckLine(string line, string expectedRowHeader)
        {
            var parts = line.Split(' ');
            if (parts.Length != 2)
            {
                throw new Exception("Invalid header line.");
            }

            if (parts[0] != expectedRowHeader)
            {
                throw new Exception("Invalid header line.");
            }

            return int.Parse(parts[1]);
        }


    }
}
