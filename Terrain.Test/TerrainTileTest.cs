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
            
            var tile = new TerrainTile(data, 50);

            Assert.AreEqual(50, tile.CellSize);
            Assert.AreEqual(data, tile.Data);
            Assert.AreEqual(2, tile.NumRows);
            Assert.AreEqual(4, tile.NumColumns);
        }


        [DataTestMethod]
        [DataRow(0.0f, 1000.0f, new float[] { 500.0f, 100.0f, 900.0f }, new float[] { 0.5f, 0.1f, 0.9f })]
        [DataRow(-1000.0f, 1000.0f, new float[] { 0.0f, -500.0f, 500.0f }, new float[] { 0.5f, 0.25f, 0.75f })]
        public void ShouldNormaliseInto0To1Range(float min, float max, float[] testData, float[] expected )
        {
            float[][] data = new float[1][];
            data[0] = testData;
            var tile = new TerrainTile(data, 50);

            var result = tile.Normalise(min, max);

            var resultRow = result.Data[0];

            for (int i = 0; i < resultRow.Length; i++)
            {
                Assert.AreEqual(expected[i], resultRow[i]);
            }
        }
    }
}