using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terrain.Data
{
    public  class PolyLine
    {
        public List<Point> Points { get; set; }
        public PolyLine(IEnumerable<Point> points)
        {
            Points = points.ToList();
        }

        public PolyLine()
        {
            Points = new List<Point>();
        }
    }
}
