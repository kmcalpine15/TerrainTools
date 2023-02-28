using System;
using Terrain.Data;

namespace Terrain.Test
{
	[TestClass]
	public class CoordinateSpaceTest
	{
		[TestMethod]
		public void GetIntersectingPoints_ReturnsOnlyIntesectingPoints()
		{
			var testPoints = new Point[] {
				new Point(1.0, 1.0), //intersecting
				new Point(0.0, 0.0), //non-intersecting,
				new Point(1.5, 1.5), //intersecting just
				new Point(1.0,5.0), //intersecting 1 axis
			};

			var space = new CoordinateSpace(0.5, 0.5, 1.5, 1.5);
			var result = space.GetIntersectingPoints(testPoints).ToArray();

			Assert.AreEqual(2, result.Length);
			Assert.AreEqual(new Point(1.0, 1.0), result[0]);
            Assert.AreEqual(new Point(1.5, 1.5), result[1]);
        }
    }
}

