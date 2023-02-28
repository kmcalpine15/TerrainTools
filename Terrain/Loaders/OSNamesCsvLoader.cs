using System;
using System.Text;
using Terrain.Data;

namespace Terrain.Loaders
{

	/// <summary>
	/// Loads the OSNames dataset
	/// </summary>
	public class OSNamesCsvLoader
	{
		public OSNamesCsvLoader()
		{
		}

		public List<Point> LoadPlaceNames(IEnumerable<string> filePaths )
		{
			List<Point> masterList = new List<Point>();
			foreach(var path in filePaths)
			{
				var places = LoadPlaceNames(path);
				masterList.AddRange(places);
			}

			return masterList;
		}

		public List<Point> LoadPlaceNames(string filePath)
		{
            List<Point> places = new List<Point>();
            using (var fle = new StreamReader(filePath))
			{
				while (true)
				{
					string? line = fle.ReadLine();
					if (line == null)
					{
						break;
					}

					var parts = SplitCSVLine(line);
					var name = parts[2];
					var type = parts[6];
					var x = double.Parse(parts[8]);
					var y = double.Parse(parts[9]);

					if(type != "landform" && type != "populatedPlace")
					{
						continue;
					}

					places.Add(new Point(x, y, name));
				}
			}

			return places;
		}

		public List<string> SplitCSVLine(string line)
		{
			bool inQuote = false;
			char? quoteChar = null;
			List<string> result = new List<string>();
			var currentString = new StringBuilder();
			for(int i=0; i<line.Length; i++)
			{
				var next = line[i];
				if(next == ',' && !inQuote)
				{
					result.Add(currentString.ToString());
					currentString.Clear();
					continue;
				}

				if(next == '"')
				{
					if(inQuote && quoteChar == '"')
					{
						inQuote = false;
						quoteChar = null;
						continue;
					}

					inQuote = true;
					quoteChar = '"';
					continue;
				}

                currentString.Append(next);
			}

			result.Append(currentString.ToString());
			return result;
		}
	}
}
