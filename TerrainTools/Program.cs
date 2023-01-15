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
            var result = await loader.LoadTerrain(filePath);
            var normalised = result.Normalise(-135.0f, 1345.0f);
            R16TerrainWriter writer = new R16TerrainWriter();
            await writer.WriteToFile(normalised, "D:\\OSMapData\\RawHeightmap\\Scotland.r16");
            
            Console.WriteLine("Hello, World!");
        }
    }
}