using System;
using OpenTK.Mathematics;
using Terrain.Data;

namespace TerrainRenderer.Test
{
	[TestClass]
	public class LandscapeTests
	{
		[TestMethod]
		public void GenerateVertices_GeneratesCorrectVertices()
		{
			float[][] testData = new float[2][];
			testData[0] = new float[2] { 1.0f, 2.0f };
			testData[1] = new float[2] { 3.0f, 4.0f };

			var testTile = new TerrainTile(testData, 10.0f, 0.0f, 0.0f);
			var testLandscape = new Landscape(testTile);

			var result = testLandscape.GenerateVertices();
			Assert.AreEqual(4*3, result.Length);
			Assert.AreEqual(0.0f, result[0]);
			Assert.AreEqual(1.0f, result[1]);
			Assert.AreEqual(0.0f, result[2]);

            Assert.AreEqual(10.0f, result[3]);
            Assert.AreEqual(2.0f, result[4]);
            Assert.AreEqual(0.0f, result[5]);

            Assert.AreEqual(0.0f, result[6]);
            Assert.AreEqual(3.0f, result[7]);
            Assert.AreEqual(10.0f, result[8]);

            Assert.AreEqual(10.0f, result[9]);
            Assert.AreEqual(4.0f, result[10]);
            Assert.AreEqual(10.0f, result[11]);

        }

		[TestMethod]
		public void GenerateIndices_GeneratesCorrectIndices()
		{
            float[][] testData = new float[2][];
            testData[0] = new float[2] { 1.0f, 2.0f };
            testData[1] = new float[2] { 3.0f, 4.0f };

            var testTile = new TerrainTile(testData, 10.0f, 0.0f, 0.0f);
            var testLandscape = new Landscape(testTile);

			var result = testLandscape.GenerateIndices();
			Assert.AreEqual(6, result.Length);

			var expected = new ushort[] { 0, 2, 3, 3, 1, 0 };
			CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GenerateIndices_GeneratesCorrectIndices_3x3()
        {
            float[][] testData = new float[3][];
            testData[0] = new float[3] { 1.0f, 2.0f, 3.0f };
            testData[1] = new float[3] { 4.0f, 5.0f, 6.0f };
            testData[2] = new float[3] { 8.0f, 9.0f, 10.0f };

            var testTile = new TerrainTile(testData, 10.0f, 0.0f, 0.0f);
            var testLandscape = new Landscape(testTile);

            var result = testLandscape.GenerateIndices();
            Assert.AreEqual(24, result.Length);

            var expected = new ushort[] { 0, 3, 4, 4, 1, 0, 1, 4, 5, 5, 2, 1, 3, 6, 7, 7, 4 ,3,  4, 7, 8, 8, 5, 4};
            CollectionAssert.AreEqual(expected, result);
        }
    }
}

