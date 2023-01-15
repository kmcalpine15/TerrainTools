using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terrain.Data
{
    public class CoordinateTransform
    {
        public CoordinateSpace From { get; private set; }
        public CoordinateSpace To { get; private set; }

        private float _xScale;
        private float _yScale;

        public CoordinateTransform(CoordinateSpace fromSpace, CoordinateSpace toSpace)
        {
            From = fromSpace;
            To = toSpace;
            _xScale = (float)To.Width / From.Width;
            _yScale = (float)To.Height / From.Height;
        }
        
        public Coordinate Transform(Coordinate coordinate)
        {
            return new Coordinate(
                (int)Math.Round((coordinate.X - From.MinX) * _xScale + To.MinX),
                (int)Math.Round((coordinate.Y - From.MinY) * _yScale + To.MinY)
            );
        }
    }
}
