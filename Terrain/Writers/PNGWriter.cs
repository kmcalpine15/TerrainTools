using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrain.Data;
using Point = Terrain.Data.Point;

namespace Terrain.Writers
{
    public  class PNGWriter
    {
        public TerrainTile Tile { get; set; }
        private Bitmap Image { get; set; }
        public PNGWriter(TerrainTile tile)
        {
            Tile = tile;
            Image = new Bitmap(tile.NumColumns, tile.NumRows, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Draw(tile);
        }

        public void Draw(TerrainTile tile)
        {
            for (int y = 0; y < Tile.NumRows; y++)
            {
                for (int x = 0; x < Tile.NumColumns; x++)
                {
                    var val = tile.Data[y][x];

                    var col = (byte)(Math.Round(val * 255));
                    Image.SetPixel(x, y, Color.FromArgb(col, col, col));
                }
            }
        }

        public void Draw(PolyLine line, byte color)
        {
            var transform = new CoordinateTransform(Tile.CoordinateSpace, new CoordinateSpace(0, 0, Tile.NumColumns, Tile.NumRows));
            var pen = new Pen(Brushes.Red,1.0f);
  
            using(var g = Graphics.FromImage(Image))
            {
                for(int i=1; i<line.Points.Count; i++)
                {
                    var p1 = transform.Transform(line.Points[i - 1]);
                    var p2 = transform.Transform(line.Points[i]);
                    g.DrawLine(pen, new PointF(p1.X, p1.Y), new PointF(p2.X, p2.Y));
                }
            }
        }

        public void Draw(Point point)
        {
            
        }

        public void Save(string filepath)
        {
            Image.Save(filepath);
        }
    }
}
