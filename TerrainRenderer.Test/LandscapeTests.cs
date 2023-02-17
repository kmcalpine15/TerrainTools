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
			Assert.AreEqual(2, result.Length);

			Assert.AreEqual(new Vector3i(0, 2, 3), result[0]);
            Assert.AreEqual(new Vector3i(3, 1, 0), result[1]);
        }
	}
}

