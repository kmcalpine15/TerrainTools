using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terrain.Data
{
    /// <summary>
    /// Represents a tranformation from one coordinate space to another.
    /// </summary>
    public class CoordinateTransform
    {
        public CoordinateSpace From { get; private set; }
        public CoordinateSpace To { get; private set; }

        private double _xScale;
        private double _yScale;

        public CoordinateTransform(CoordinateSpace fromSpace, CoordinateSpace toSpace)
        {
            From = fromSpace;
            To = toSpace;
            _xScale = (float)To.Width / From.Width;
            _yScale = (float)To.Height / From.Height;
        }
        
        public Point Transform(Point coordinate)
        {
            return new Point(
                (coordinate.X - From.MinX) * _xScale + To.MinX,
                (coordinate.Y - From.MinY) * _yScale + To.MinY
            );
        }
    }
}
