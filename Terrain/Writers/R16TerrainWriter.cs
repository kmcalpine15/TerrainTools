﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrain.Data;

namespace Terrain.Writers
{
    public class R16TerrainWriter
    {
        public async Task WriteToFile(TerrainTile data, string filePath)
        {
            var mappedTerrainData = data.Data.Select(row => row.Select(height => (ushort)(height * short.MaxValue)).ToArray()).ToArray();
            var reversed = mappedTerrainData.Reverse().ToArray();
            using (var fle = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (var wr = new BinaryWriter(fle))
                {
                    for (int i = 0; i < data.NumRows; i++)
                    {
                        for (int j = 0; j < data.NumColumns; j++)
                        {
                            ushort mapped = (ushort)(reversed[i][j]);
                            wr.Write(mapped);
                        }
                    }
                }
            }

            string metadataPath = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath) + ".meta");
            using (var fle = new StreamWriter(metadataPath))
            {
                fle.Write($"width:{data.NumColumns}");
                fle.Write($"height:{data.NumRows}");
                fle.Write($"left:{data.CoordinateSpace.Left}");
                fle.Write($"right:{data.CoordinateSpace.Right}");
                fle.Write($"top:{data.CoordinateSpace.Top}");
                fle.Write($"bottom:{data.CoordinateSpace.Bottom}");

            }
        }
    }
}
