using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrain.Loaders;

namespace Terrain.Test
{
    [TestClass]
    public class ESRIShapeFileLoaderTest
    {
        [TestMethod]
        public void ShouldReadFile()
        {
            var ld = new ESRIShapeFileLoader();

            var hd = ld.LoadFile("D:\\OSMapData\\Strategi\\a_road.shp");

            Assert.AreEqual(9994, hd.FileCode);
        }
    }
}
