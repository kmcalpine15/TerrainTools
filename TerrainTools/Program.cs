using System.Reflection.Metadata.Ecma335;
using Terrain.Data;
using Terrain.Loaders;
using Terrain.Writers;

namespace TerrainTools
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await GetTilesAroundPoint("D:\\OSMapData\\Terrain50ScotlandOnly", "D:\\OSMapData\\RawHeightmap\\", 318611, 662726, 8066);
        }


        static async Task LoadAllAndSave()
        {
            string filePath = "D:\\OSMapData\\Terrain50ScotlandOnly";


            AsciiGridLoader loader = new AsciiGridLoader();

            Console.WriteLine($"Loading terrain from {filePath}");
            var result = await loader.LoadTerrain(filePath);
            Console.WriteLine($"Loaded terrain from {filePath}");
            Console.WriteLine($"Normalising Terrain");
            var normalised = result.Normalise(-135.0f, 1345.0f);
            R16TerrainWriter writer = new R16TerrainWriter();


            //Ok, I need to write these 
            Console.WriteLine($"Splitting Terrain");
            var tiles = normalised.Split(4033);

            string outputDir = "D:\\OSMapData\\RawHeightmap\\";
            foreach (var tile in tiles)
            {
                string fileName = $"scotland_X{tile.XIndex}_Y{tile.YIndex}.r16";
                string outputFilePath = Path.Combine(outputDir, fileName);
                await writer.WriteToFile(tile, outputFilePath);
                Console.WriteLine($"Terrain file {fileName} written");
            }
        }

        static async Task GetTilesAroundPoint(string filePath, string outputDir, int x, int y, int size)
        {
            int xBound = Math.Max(0, x - (int)(Math.Ceiling((double)size / 2)));
            int yBound = Math.Max(0, y - (int)(Math.Ceiling((double)size / 2)));

            Console.WriteLine($"Loading terrain from {filePath}");
            AsciiGridLoader loader = new AsciiGridLoader();
            var terrain = await loader.LoadTerrain(filePath);
            Console.WriteLine($"Loaded terrain from {filePath}");
            Console.WriteLine($"Normalising Terrain");
            var normalised = terrain.Normalise(-135.0f, 1345.0f);

            var subTile = normalised.SubTile(xBound, yBound, size, size);
            var tiles = subTile.Split(4033);
            R16TerrainWriter writer = new R16TerrainWriter();
            
            foreach (var tile in tiles)
            {
                string fileName = $"scotland_X{tile.XIndex}_Y{tile.YIndex}.r16";
                string outputFilePath = Path.Combine(outputDir, fileName);
                await writer.WriteToFile(tile, outputFilePath);
                Console.WriteLine($"Terrain file {fileName} written");
            }
        }
        
    }
}