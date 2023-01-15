using Terrain.Data;
using Terrain.Loaders;
using Terrain.Writers;

namespace TerrainTools
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string filePath = "D:\\OSMapData\\Terain50GridExtracted";

            
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
    }
}