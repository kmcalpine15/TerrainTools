using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terrain.Data
{
    public class CoordinateSpace
    {
        public int MinX { get; private set; }
        public int MinY { get; private set; }
        public int MaxX { get; private set; }
        public int MaxY { get; private set; }
        public int Width { get { return MaxX - MinX; } }
        public int Height { get { return MaxY - MinY; } }
        public int Left { get { return MinX; } }
        public int Right { get { return MaxX; } }
        public int Top { get { return MaxY; } }
        public int Bottom { get { return MinY; } }

        public CoordinateSpace(int minX, int minY, int maxX, int maxY)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
        }
    }
}
