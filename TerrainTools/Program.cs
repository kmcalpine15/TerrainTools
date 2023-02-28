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
            //await GetTilesAroundPoint("D:\\OSMapData\\Terrain50ScotlandOnly", "D:\\OSMapData\\RawHeightmap\\", 318611, 662726, 8066);
            GeneratePlacesCSVAroundPoint("/Users/krismcalpine/Downloads/opname_csv_gb/Data/", "/Users/krismcalpine/Downloads/places.csv", 318611, 662726, 8066);
        }


        static async Task LoadAllAndSave()
        {
            string filePath = "D:\\OSMapData\\Terrain50ScotlandOnly";


            AsciiGridLoader loader = new AsciiGridLoader();

            Console.WriteLine($"Loading terrain from {filePath}");
            var result = loader.LoadTerrain(filePath);
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

        /// <summary>
        /// Outputs R16 raw files in files of 4033x4033 around a specified point
        /// for ingestion by unreal engine.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="outputDir"></param>
        /// <param name="x">Center X coordinate</param>
        /// <param name="y">Center Y coordinate</param>
        /// <param name="size">Width and height of the final landscape. 8066 seems to be a safe maximum.</param>
        /// <returns></returns>
        static async Task GetTilesAroundPoint(string filePath, string outputDir, int x, int y, int size)
        {
            int xBound = Math.Max(0, x - (int)(Math.Ceiling((double)size / 2)));
            int yBound = Math.Max(0, y - (int)(Math.Ceiling((double)size / 2)));

            Console.WriteLine($"Loading terrain from {filePath}");
            AsciiGridLoader loader = new AsciiGridLoader();
            var terrain = loader.LoadTerrain(filePath);
            Console.WriteLine($"Loaded terrain from {filePath}");
            Console.WriteLine($"Normalising Terrain");
            var normalised = terrain.Normalise(-135.0f, 1345.0f);

            var subTile = normalised.SubTile(xBound, yBound, size, size);
            var tiles = subTile.Split(4033); //4033 is derived from unreal engines terrain size requirements
            R16TerrainWriter writer = new R16TerrainWriter();
            
            foreach (var tile in tiles)
            {
                string fileName = $"scotland_X{tile.XIndex}_Y{tile.YIndex}.r16";
                string outputFilePath = Path.Combine(outputDir, fileName);
                await writer.WriteToFile(tile, outputFilePath);
                Console.WriteLine($"Terrain file {fileName} written");
            }
        }

        /// <summary>
        /// Reads in the OS Placenames data sets and outputs a CSV file in a format readable by unreal engine
        /// containing the coordinates and name of settlements and mountains.
        /// </summary>
        /// <param name="osPlaceNamesDatasetDir">The locaton of the extracted OSPlacenames csv dataset</param>
        /// <param name="outputCsvFile">Output csv file for unreal engine</param>
        /// <param name="x">Center X coordinate of the area to be output</param>
        /// <param name="y">Center Y coordinate of the area to be output</param>
        /// <param name="size">Width and height of the area to be output</param>
        static void GeneratePlacesCSVAroundPoint(string osPlaceNamesDatasetDir, string outputCsvFile, int x, int y, int size)
        {
            Console.WriteLine("Loading OS names dataset");
            int xBound = Math.Max(0, x - (int)(Math.Ceiling((double)size / 2)));
            int yBound = Math.Max(0, y - (int)(Math.Ceiling((double)size / 2)));

            var dirInfo = new DirectoryInfo(osPlaceNamesDatasetDir);
            var files = dirInfo.GetFiles("*.csv").Select(x => x.FullName);

            var placeLoader = new OSNamesCsvLoader();
            var places = placeLoader.LoadPlaceNames(files);
       

            Console.WriteLine($"{places.Count()} place names loaded.");
            Console.WriteLine("Extracting points within bounds");
            var bound = new CoordinateSpace(xBound, yBound, xBound + size, yBound + size);
            var inBoundsPlaces = bound.GetIntersectingPoints(places);

            Console.WriteLine($"{inBoundsPlaces.Count()} places within bounds.");
            using (var wr = new StreamWriter(outputCsvFile))
            {
                wr.WriteLineAsync("ID,Name,X,Y");
                int id = 0;
                foreach(var place in inBoundsPlaces)
                {
                    wr.WriteLine($"{id},{place.Name},{place.X},{place.Y}");
                    id++;
                }
            }

            Console.WriteLine("Complete");
        }
    }
}