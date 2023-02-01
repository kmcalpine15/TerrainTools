using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terrain.Data
{
    public class CoordinateSpace
    {
        public double MinX { get; set; }
        public double MinY { get; set; }
        public double MaxX { get; set; }
        public double MaxY { get; set; }
        public double Width { get { return MaxX - MinX; } }
        public double Height { get { return MaxY - MinY; } }
        public double Left { get { return MinX; } }
        public double Right { get { return MaxX; } }
        public double Top { get { return MaxY; } }
        public double Bottom { get { return MinY; } }

        public CoordinateSpace(double minX, double minY, double maxX, double maxY)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
        }
        
        public void GrowBounds(CoordinateSpace other)
        {
            if (other.MinX < MinX) { MinX = other.MinX; }
            if (other.MaxX > MaxX) { MaxX = other.MaxX; }
            if (other.MaxY > MaxY) { MaxY = other.MaxY; }
            if (other.MinY < MinY) { MinY = other.MinY; }
        }
    }
}
