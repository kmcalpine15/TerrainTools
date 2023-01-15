using Terrain.Data;
using Terrain.Loaders;

namespace TerrainTools
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = "D:\\OSMapData\\temp_test\\";

            AsciiGridLoader loader = new AsciiGridLoader();
            var result = loader.LoadTerrain(filePath);
            
            
            Console.WriteLine("Hello, World!");
        }
    }
}