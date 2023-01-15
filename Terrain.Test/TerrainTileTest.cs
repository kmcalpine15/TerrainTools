using Terrain.Data;

namespace Terrain.Test
{
    [TestClass]
    public class TerrainTileTest
    {
        [TestMethod]
        public void ShouldContructAValidTile()
        {
            float[][] data = new float[2][];
            data[0] = new float[4];
            
            var tile = new TerrainTile(data, 50, 5, 6);

            Assert.AreEqual(50, tile.CellSize);
            Assert.AreEqual(data, tile.Data);
            Assert.AreEqual(2, tile.NumRows);
            Assert.AreEqual(4, tile.NumColumns);
            Assert.AreEqual(5, tile.CoordinateSpace.MinX);
            Assert.AreEqual(6, tile.CoordinateSpace.MinY);
        }

        [DataTestMethod]
        [DataRow(0.0f, 1000.0f, new float[] { 500.0f, 100.0f, 900.0f }, new float[] { 0.5f, 0.1f, 0.9f })]
        [DataRow(-1000.0f, 1000.0f, new float[] { 0.0f, -500.0f, 500.0f }, new float[] { 0.5f, 0.25f, 0.75f })]
        public void ShouldNormaliseInto0To1Range(float min, float max, float[] testData, float[] expected )
        {
            float[][] data = new float[1][];
            data[0] = testData;
            var tile = new TerrainTile(data, 50, 1,1);

            var result = tile.Normalise(min, max);

            var resultRow = result.Data[0];

            for (int i = 0; i < resultRow.Length; i++)
            {
                Assert.AreEqual(expected[i], resultRow[i]);
            }
        }
        [TestMethod]
        public void Normalise_ShouldThrowIfMinGreatherThanMax()
        {
            float[][] data = new float[1][];
            data[0]  = new float[3];
            var tile = new TerrainTile(data, 50, 1, 1);

            Assert.ThrowsException<ArgumentException>(() => tile.Normalise(1000.0f, 500.0f));
             
        }

        [DataTestMethod]
        [DataRow(new float[] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f }, new float[] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f }, 0,0,50,0, true)]
        [DataRow(new float[] { 0.0f, 0.0f }, new float[] { 0.0f, 0.0f }, 0, 0, 20, 0, true)]
        [DataRow(new float[] { 0.0f, 0.0f }, new float[] { 0.0f, 0.0f }, 0, 0, 30, 0, false)]
        [DataRow(new float[] { 0.0f, 0.0f }, new float[] { 0.0f, 0.0f }, 0, 0, 20, 10, true)]
        [DataRow(new float[] { 0.0f, 0.0f }, new float[] { 0.0f, 0.0f }, 20, 10, 0, 0, true)]
        public void CollidesWith_ShouldReturnTrueForIntersectingTilesOnly(float[] tileData1, float[] tileData2, int tile1X, int tile1Y, int tile2X, int tile2Y, bool expected)
        {
            float[][] data1 = new float[1][];
            data1[0] = tileData1;
            TerrainTile tile1 = new TerrainTile(data1, 10, tile1X, tile1Y);

            float[][] data2 = new float[1][];
            data2[0] = tileData2;
            TerrainTile tile2 = new TerrainTile(data2, 10, tile2X, tile2Y);

            Assert.AreEqual(expected, tile1.CollidesWith(tile2));
        }


        [DataTestMethod]
        [DataRow(new float[] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f }, new float[] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f }, 0, 0, 50, 0, false)]
        [DataRow(new float[] { 0.0f, 0.0f }, new float[] { 0.0f, 0.0f }, 0, 0, 20, 0, false)]
        [DataRow(new float[] { 0.0f, 0.0f }, new float[] { 0.0f, 0.0f }, 0, 0, 30, 0, false)]
        [DataRow(new float[] { 0.0f, 0.0f }, new float[] { 0.0f, 0.0f }, 0, 0, 20, 10, false)]
        [DataRow(new float[] { 0.0f, 0.0f }, new float[] { 0.0f, 0.0f }, 20, 10, 0, 0, false)]
        [DataRow(new float[] { 0.0f, 0.0f }, new float[] { 0.0f, 0.0f }, 0, 0, 10, 0, true)]
        [DataRow(new float[] { 0.0f, 0.0f }, new float[] { 0.0f, 0.0f }, 0, 0, 0, 9, true)]
        [DataRow(new float[] { 0.0f, 0.0f }, new float[] { 0.0f, 0.0f }, 0, 0, 0, 10, false)]
        public void OverlapsWith_ShouldReturnTrueForOverlappingTilesOnly(float[] tileData1, float[] tileData2, int tile1X, int tile1Y, int tile2X, int tile2Y, bool expected)
        {
            float[][] data1 = new float[1][];
            data1[0] = tileData1;
            TerrainTile tile1 = new TerrainTile(data1, 10, tile1X, tile1Y);

            float[][] data2 = new float[1][];
            data2[0] = tileData2;
            TerrainTile tile2 = new TerrainTile(data2, 10, tile2X, tile2Y);

            Assert.AreEqual(expected, tile1.OverlapsWith(tile2));
        }
    }
}