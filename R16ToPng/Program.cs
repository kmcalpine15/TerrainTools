
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace R16ToPng
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string filePath = args[0];
            int width = int.Parse(args[1]);
            int height = int.Parse(args[2]);

            short[][] data = new short[height][];

            using (var reader = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using(var rd = new BinaryReader(reader))
                {
                    for (int i = 0; i < height; i++)
                    {
                        data[i] = new short[width];
                        for (int j = 0; j < width; j++)
                        {
                            data[i][j] = (short) rd.ReadUInt16();
                        }
                    }
                }
            }
            

            Bitmap img = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            
            for(int y=0;y<height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var val = data[y][x];
                    var col = (byte)(val / 256);
                    img.SetPixel(x, y, Color.FromArgb(col, col, col));
                }
            }
        

            img.Save("D:\\OSMapData\\RawHeightmap\\NT.png", ImageFormat.Png);
        }
    }
}