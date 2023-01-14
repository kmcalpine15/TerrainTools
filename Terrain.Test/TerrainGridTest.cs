using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terrain.Data;

namespace Terrain.Test
{
    [TestClass]
    public class TerrainGridTest
    {
        [TestMethod]
        public void ShouldContructAValidGrid()
        {

        }


        [DataTestMethod]
        [DataRow(new float[] { 0.0f, 0.0f }, new float[] { 0.0f, 0.0f }, 0, 0, 10, 0)]
        [DataRow(new float[] { 0.0f, 0.0f }, new float[] { 0.0f, 0.0f }, 0, 0, 0, 9)]
        public void AddTile_ShouldNotAllowAddingOverlappingTiles(float[] tileData1, float[] tileData2, int tile1X, int tile1Y, int tile2X, int tile2Y)
        {
            var tile1 = new TerrainTile(new float[][] { tileData1 }, 10, tile1X, tile1Y);
            var tile2 = new TerrainTile(new float[][] { tileData2 }, 10, tile2X, tile2Y);

            var grid = new TerrainGrid();
            grid.AddTile(tile1);
            Assert.ThrowsException<ArgumentException>(() => grid.AddTile(tile2));

        }


    }
}