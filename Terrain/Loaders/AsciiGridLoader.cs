﻿using Terrain.Data;

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

            int cellSize = int.MinValue;
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
            CoordinateTransform transform = new CoordinateTransform(worldSpace, dataSpace);

            float[][] data = new float[dataSpace.Height][];
            foreach(var tile in tiles)
            {
                for(int y=0; y<tile.Data.Length; y++)
                {
                    int yCoord = tile.CoordinateSpace.MinY + (y * tile.CellSize);
                    for (int x = 0; x < tile.Data[y].Length; x++)
                    {
                        Coordinate worldCoord = new Coordinate(tile.CoordinateSpace.MinX + (x * tile.CellSize), yCoord);
                        Coordinate dataCoord = transform.Transform(worldCoord);

                        if (data[dataCoord.Y] == null)
                        {
                            data[dataCoord.Y] = new float[dataSpace.Width];
                        }

                        data[dataCoord.Y][dataCoord.X] = tile.Data[y][x];
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
